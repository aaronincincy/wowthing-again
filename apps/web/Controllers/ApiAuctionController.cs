﻿using System.Net.Mime;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Npgsql;
using Wowthing.Lib.Contexts;
using Wowthing.Lib.Enums;
using Wowthing.Lib.Models;
using Wowthing.Lib.Models.Player;
using Wowthing.Lib.Models.Query;
using Wowthing.Lib.Models.Wow;
using Wowthing.Lib.Services;
using Wowthing.Lib.Utilities;
using Wowthing.Web.Forms;
using Wowthing.Web.Models;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Wowthing.Web.Controllers;

[Route("api/auctions")]
public class ApiAuctionController : Controller
{
    private readonly CacheService _cacheService;
    private readonly ILogger<ApiAuctionController> _logger;
    private readonly JsonSerializerOptions _jsonSerializerOptions;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly WowDbContext _context;

    public ApiAuctionController(
        CacheService cacheService,
        ILogger<ApiAuctionController> logger,
        JsonSerializerOptions jsonSerializerOptions,
        UserManager<ApplicationUser> userManager,
        WowDbContext context)
    {
        _cacheService = cacheService;
        _logger = logger;
        _jsonSerializerOptions = jsonSerializerOptions;
        _userManager = userManager;
        _context = context;
    }

    [HttpPost("extra-pets")]
    [Authorize]
    public async Task<IActionResult> ExtraPets([FromBody] ApiExtraPetsForm form)
    {
        var timer = new JankTimer();

        var user = await _userManager.GetUserAsync(HttpContext.User);
        if (user == null)
        {
            _logger.LogWarning("ruh roh");
            return NotFound();
        }

        var data = new UserAuctionData();
        var allPets = new Dictionary<long, PlayerAccountPetsPet>();

        var accounts = await _context.PlayerAccount
            .AsNoTracking()
            .Where(pa => pa.UserId == user.Id && pa.Enabled)
            .Include(pa => pa.Pets)
            .ToArrayAsync();

        var accountIds = accounts.SelectArray(account => account.Id);

        var accountPets = accounts
            .Where(pa => pa.Pets != null)
            .Select(pa => pa.Pets)
            .OrderByDescending(pap => pap.UpdatedAt)
            .ToArray();

        foreach (var pets in accountPets)
        {
            foreach (var (petId, pet) in pets.Pets)
            {
                allPets.TryAdd(petId, pet);
            }
        }

        // Pet itemId -> id map
        var petItemIdMap = await GetPetItems();
        var petItemIds = petItemIdMap.Keys.ToArray();

        // Caged pets
        var guildCages = await _context
            .PlayerGuildItem
            .Where(pgi =>
                pgi.Guild.UserId == user.Id &&
                pgi.ItemId == 82800 && // Pet Cage
                pgi.Context > 0
            )
            .ToArrayAsync();

        var playerCages = await _context
            .PlayerCharacterItem
            .Where(pci =>
                    pci.Character.AccountId.HasValue &&
                    accountIds.Contains(pci.Character.AccountId.Value) &&
                    pci.ItemId == 82800 // Pet Cage
            )
            .ToArrayAsync();

        // Learnable pets
        var guildLearnable = await _context
            .PlayerGuildItem
            .Where(pgi =>
                pgi.Guild.UserId == user.Id &&
                petItemIds.Contains(pgi.ItemId)
            )
            .ToArrayAsync();

        var playerLearnable = await _context
            .PlayerCharacterItem
            .Where(pci =>
                pci.Character.AccountId.HasValue &&
                accountIds.Contains(pci.Character.AccountId.Value) &&
                petItemIds.Contains(pci.ItemId)
            )
            .ToArrayAsync();

        var accountConnectedRealmIds = await GetConnectedRealmIds(user, accounts);

        timer.AddPoint("Accounts");

        var groupedPets = allPets.Values
            .GroupBy(pet => pet.SpeciesId)
            .ToDictionary(
                group => group.Key,
                group => group
                    .Select(pet => new UserAuctionDataPet(pet))
                    .ToList()
            );

        foreach (var cagedPet in guildCages)
        {
            int speciesId = cagedPet.Context;
            if (!groupedPets.TryGetValue(speciesId, out var pets))
            {
                pets = groupedPets[speciesId] = new List<UserAuctionDataPet>();
            }

            pets.Add(new UserAuctionDataPet(cagedPet, true));
        }

        foreach (var cagedPet in playerCages)
        {
            int speciesId = cagedPet.Context;
            if (!groupedPets.TryGetValue(speciesId, out var pets))
            {
                pets = groupedPets[speciesId] = new List<UserAuctionDataPet>();
            }

            pets.Add(new UserAuctionDataPet(cagedPet, true));
        }

        foreach (var learnablePet in guildLearnable)
        {
            int speciesId = petItemIdMap[learnablePet.ItemId];
            if (!groupedPets.TryGetValue(speciesId, out var pets))
            {
                pets = groupedPets[speciesId] = new List<UserAuctionDataPet>();
            }

            pets.Add(new UserAuctionDataPet(learnablePet));
        }

        foreach (var learnablePet in playerLearnable)
        {
            int speciesId = petItemIdMap[learnablePet.ItemId];
            if (!groupedPets.TryGetValue(speciesId, out var pets))
            {
                pets = groupedPets[speciesId] = new List<UserAuctionDataPet>();
            }

            pets.Add(new UserAuctionDataPet(learnablePet));
        }

        var extraPets = groupedPets
            .Where(kvp => form.IgnoreJournal
                ? (
                    kvp.Value.Any(pet => pet.Location == ItemLocation.PetCollection) &&
                    kvp.Value.Any(pet => pet.Location != ItemLocation.PetCollection)
                )
                : kvp.Value.Count > 1
            )
            .ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value
                    .OrderBy(pet => pet.Quality)
                    .ThenBy(pet => pet.Level)
                    .ToArray()
            );

        var extraSpeciesIds = extraPets.Keys.ToArray();

        var petSpeciesMap = await _context.WowPet
            .Where(pet => extraSpeciesIds.Contains(pet.Id))
            .ToDictionaryAsync(
                pet => pet.Id,
                pet => pet.CreatureId
            );

        long minimumValue = (user.Settings.Auctions?.MinimumExtraPetsValue ?? 0) * 10000;
        var auctions = await _context.WowAuction
            .AsNoTracking()
            .Where(auction =>
                accountConnectedRealmIds.Contains(auction.ConnectedRealmId) &&
                extraSpeciesIds.Contains(auction.PetSpeciesId) &&
                auction.BuyoutPrice >= minimumValue
            )
            .ToArrayAsync();

        data.RawAuctions = DoAuctionStuff(auctions.GroupBy(auction => petSpeciesMap[auction.PetSpeciesId]), false);

        timer.AddPoint("Auctions");

        var creatureIds = extraSpeciesIds.Select(speciesId => petSpeciesMap[speciesId]);
        data.Names = await _context.LanguageString
            .AsNoTracking()
            .Where(ls =>
                ls.Language == user.Settings.General.Language &&
                ls.Type == StringType.WowCreatureName &&
                creatureIds.Contains(ls.Id)
            )
            .ToDictionaryAsync(
                ls => ls.Id,
                ls => ls.String
            );

        timer.AddPoint("Strings");

        data.Pets = extraPets
            .ToDictionary(
                kvp => petSpeciesMap[kvp.Key],
                kvp => kvp.Value
            );

        timer.AddPoint("Data", true);

        _logger.LogInformation($"{timer}");

        var json = JsonSerializer.Serialize(data, _jsonSerializerOptions);
        return Content(json, MediaTypeNames.Application.Json);
    }

    [HttpPost("missing")]
    [Authorize]
    public async Task<IActionResult> Missing([FromBody] ApiMissingAuctionsForm form)
    {
        var timer = new JankTimer();

        var user = await _userManager.GetUserAsync(HttpContext.User);
        if (user == null)
        {
            _logger.LogWarning("ruh roh");
            return NotFound();
        }

        var accountQuery = _context.PlayerAccount
            .AsNoTracking()
            .Where(pa => pa.UserId == user.Id && pa.Enabled);

        if (form.Type == "pets")
        {
            accountQuery = accountQuery.Include(pa => pa.Pets);
        }
        else if (form.Type == "toys")
        {
            accountQuery = accountQuery.Include(pa => pa.Toys);
        }

        var accounts = await accountQuery.ToArrayAsync();

        timer.AddPoint("Accounts");

        var auctionQuery = _context.WowAuction
            .AsNoTracking();

        if (!form.AllRealms)
        {
            var accountConnectedRealmIds = await GetConnectedRealmIds(user, accounts);
            auctionQuery = auctionQuery
                .Where(auction => accountConnectedRealmIds.Contains(auction.ConnectedRealmId));
        }

        if (form.Region > 0)
        {
            var validRealmIds = await _context.WowRealm
                .AsNoTracking()
                .Where(realm => realm.Region == form.Region)
                .Select(realm => realm.ConnectedRealmId)
                .Distinct()
                .ToArrayAsync();

            auctionQuery = auctionQuery
                .Where(auction => validRealmIds.Contains(auction.ConnectedRealmId));
        }
        else
        {
            var validRealmIds = await GetRegionRealmIds(user, accounts);
            auctionQuery = auctionQuery
                .Where(auction => validRealmIds.Contains(auction.ConnectedRealmId));
        }

        var languageQuery = _context.LanguageString
            .AsNoTracking()
            .Where(ls => ls.Language == user.Settings.General.Language);

        var data = new UserAuctionData();
        if (form.Type == "mounts")
        {
            // Missing
            var userCache = await _cacheService.CreateOrUpdateMountCacheAsync(_context, timer, user.Id);
            var mountIds = userCache.MountIds.Select(id => (int)id).ToArray();

            var missingMounts = await _context.WowMount
                .AsNoTracking()
                .Where(mount =>
                    mount.ItemId > 0 &&
                    !mountIds.Contains(mount.Id)
                )
                .ToArrayAsync();

            // Auctions
            var mountSpellMap = missingMounts.ToDictionary(mount => mount.ItemId, mount => mount.SpellId);

            var mountAuctions = await auctionQuery
                .Where(auction => missingMounts.Select(mount => mount.ItemId).Contains(auction.ItemId))
                .ToArrayAsync();

            data.RawAuctions = DoAuctionStuff(mountAuctions.GroupBy(auction => mountSpellMap[auction.ItemId]));

            // Strings
            var mountIdToSpellId = missingMounts
                .ToDictionary(
                    mount => mount.Id,
                    mount => mount.SpellId
                );

            data.Names = await languageQuery
                .Where(ls =>
                    ls.Type == StringType.WowMountName &&
                    mountIdToSpellId.Keys.Contains(ls.Id)
                )
                .ToDictionaryAsync(
                    ls => mountIdToSpellId[ls.Id],
                    ls => ls.String
                );
        }
        else if (form.Type == "pets")
        {
            // Missing
            var accountPetIds = accounts
                .Where(account => account.Pets != null)
                .SelectMany(account => account.Pets.Pets
                    .EmptyIfNull()
                    .Select(pet => pet.Value.SpeciesId)
                )
                .Distinct()
                .ToArray();

            var missingPets = await _context.WowPet
                .AsNoTracking()
                .Where(pet =>
                    (pet.Flags & 32) == 0 && // HideFromJournal
                    //pet.SourceType != 4 && // WildPet
                    !accountPetIds.Contains(pet.Id))
                .ToArrayAsync();

            var petItemMap = missingPets
                .Where(pet => pet.ItemId > 0)
                .ToDictionary(pet => pet.ItemId, pet => pet.CreatureId);
            var petSpeciesMap = missingPets
                .ToDictionary(pet => pet.Id, pet => pet.CreatureId);

            var missingPetItemIds = petItemMap
                .Keys
                .ToArray();
            var missingPetSpeciesIds = petSpeciesMap
                .Keys
                .Select(speciesId => (short)speciesId)
                .ToArray();

            // Auctions
            var petAuctionQuery = auctionQuery;

            if (form.MissingPetsMaxLevel)
            {
                petAuctionQuery = petAuctionQuery.Where(auction => auction.PetLevel == 25);
            }

            petAuctionQuery = petAuctionQuery
                .Where(auction => missingPetItemIds.Contains(auction.ItemId))
                .Union(petAuctionQuery
                    .Where(auction => missingPetSpeciesIds.Contains(auction.PetSpeciesId))
                );

            var petAuctions = await petAuctionQuery.ToArrayAsync();

            data.RawAuctions = DoAuctionStuff(
                petAuctions.GroupBy(auction => auction.PetSpeciesId > 0
                    ? petSpeciesMap[auction.PetSpeciesId]
                    : petItemMap[auction.ItemId]
            ));

            // Strings
            var allCreatureIds = missingPets
                .Select(pet => pet.CreatureId)
                .Distinct();

            data.Names = await languageQuery
                .Where(ls =>
                    ls.Type == StringType.WowCreatureName &&
                    allCreatureIds.Contains(ls.Id)
                )
                .ToDictionaryAsync(
                    ls => ls.Id,
                    ls => ls.String
                );

        }
        else if (form.Type == "toys")
        {
            // Missing
            var accountToyIds = accounts
                .Where(account => account.Toys != null)
                .SelectMany(account => account.Toys.ToyIds.EmptyIfNull())
                .Distinct()
                .ToArray();

            var missingToys = await _context.WowToy
                .AsNoTracking()
                .Where(toy =>
                    toy.ItemId > 0 &&
                    !accountToyIds.Contains(toy.Id)
                )
                .ToArrayAsync();

            // Auctions
            var toyAuctions = await auctionQuery
                .Where(auction => missingToys.Select(toy => toy.ItemId).Contains(auction.ItemId))
                .ToArrayAsync();

            data.RawAuctions = DoAuctionStuff(toyAuctions.GroupBy(auction => auction.ItemId));

            // Strings
            var allItemIds = data.RawAuctions.Keys
                .Distinct()
                .ToArray();

            data.Names = await languageQuery
                .Where(ls =>
                    ls.Type == StringType.WowItemName &&
                    allItemIds.Contains(ls.Id)
                )
                .ToDictionaryAsync(
                    ls => ls.Id,
                    ls => ls.String
                );
        }

        timer.AddPoint("Data", true);

        _logger.LogInformation($"{timer}");

        var json = JsonSerializer.Serialize(data, _jsonSerializerOptions);
        return Content(json, MediaTypeNames.Application.Json);
    }

    [HttpPost("missing-appearance-ids")]
    [Authorize]
    public async Task<IActionResult> MissingAppearanceIds([FromBody] ApiMissingTransmogForm form)
    {
        var timer = new JankTimer();

        var user = await _userManager.GetUserAsync(HttpContext.User);
        if (user == null)
        {
            _logger.LogWarning("ruh roh");
            return NotFound();
        }

        bool hasCache = await _context.UserCache.AnyAsync(utc => utc.UserId == user.Id);
        if (!hasCache)
        {
            return NoContent();
        }

        // Always apply a region limit
        int[] connectedRealmIds = await _context.WowRealm
            .AsNoTracking()
            .Where(realm => realm.Region == (WowRegion)Math.Max(1, (int)form.Region))
            .Select(realm => realm.ConnectedRealmId)
            .Distinct()
            .ToArrayAsync();

        if (!form.AllRealms)
        {
            var accounts = await _context.PlayerAccount
                .AsNoTracking()
                .Where(pa => pa.UserId == user.Id && pa.Enabled)
                .Include(pa => pa.Pets)
                .ToArrayAsync();
            var accountConnectedRealmIds = await GetConnectedRealmIds(user, accounts);

            connectedRealmIds = connectedRealmIds
                .Intersect(accountConnectedRealmIds)
                .ToArray();
        }

        timer.AddPoint("Realms");

        var missingAppearanceIds = await _context.Database
            .SqlQuery<int>($@"
WITH transmog_cache (appearance_id) AS (
    SELECT  UNNEST(appearance_ids) AS appearance_id
    FROM    user_cache
    WHERE   user_id = {user.Id}
)
SELECT  DISTINCT wima.appearance_id
FROM    wow_item_modified_appearance wima
LEFT OUTER JOIN transmog_cache tc
    ON wima.appearance_id = tc.appearance_id
WHERE   tc.appearance_id IS NULL
").ToArrayAsync();

        timer.AddPoint("MissingAppearances");

        await using var connection = _context.GetConnection();
        await connection.OpenAsync();

        await using var command = new NpgsqlCommand(MissingTransmogByAppearanceIdQuery.Sql, connection)
        {
            Parameters =
            {
                new() { Value = connectedRealmIds },
                new() { Value = missingAppearanceIds },
            }
        };

        await using var reader = await command.ExecuteReaderAsync();
        var auctions = new List<MissingTransmogByAppearanceIdQuery>();
        while (await reader.ReadAsync())
        {
            auctions.Add(new()
            {
                ConnectedRealmId = reader.GetInt32(0),
                AppearanceId = reader.GetInt32(1),
                ItemId = reader.GetInt32(2),
                TimeLeft = (WowAuctionTimeLeft)reader.GetInt16(3),
                BuyoutPrice = reader.GetInt64(4),
                BonusIds = (int[])reader.GetValue(5),
            });
        }

        timer.AddPoint("Auctions");

        var grouped = auctions
            .GroupBy(auction => auction.AppearanceId)
            .ToDictionary(
                group => group.Key,
                group => group
                    .OrderBy(auction => auction.BuyoutPrice)
                    .Take(5)
                    .ToList()
            );

        timer.AddPoint("Grouping");

        string json = JsonSerializer.Serialize(grouped, _jsonSerializerOptions);

        timer.AddPoint("JSON", true);

        int kept = grouped.Values
            .Select(groupAuctions => groupAuctions.Count)
            .Sum();
        _logger.LogInformation($"{auctions.Count} rows, kept {kept}");
        _logger.LogInformation($"{timer}");

        return Content(json, MediaTypeNames.Application.Json);
    }

    [HttpPost("missing-appearance-sources")]
    [Authorize]
    public async Task<IActionResult> MissingAppearanceSources([FromBody] ApiMissingTransmogForm form)
    {
        var timer = new JankTimer();

        var user = await _userManager.GetUserAsync(HttpContext.User);
        if (user == null)
        {
            _logger.LogWarning("ruh roh");
            return NotFound();
        }

        bool hasCache = await _context.UserCache.AnyAsync(utc => utc.UserId == user.Id);
        if (!hasCache)
        {
            return NoContent();
        }

        // Always apply a region limit
        int[] connectedRealmIds = await _context.WowRealm
            .AsNoTracking()
            .Where(realm => realm.Region == (WowRegion)Math.Max(1, (int)form.Region))
            .Select(realm => realm.ConnectedRealmId)
            .Distinct()
            .ToArrayAsync();

        if (!form.AllRealms)
        {
            var accounts = await _context.PlayerAccount
                .AsNoTracking()
                .Where(pa => pa.UserId == user.Id && pa.Enabled)
                .Include(pa => pa.Pets)
                .ToArrayAsync();
            var accountConnectedRealmIds = await GetConnectedRealmIds(user, accounts);

            connectedRealmIds = connectedRealmIds
                .Intersect(accountConnectedRealmIds)
                .ToArray();
        }

        timer.AddPoint("Realms");

        var missingAppearanceSources = await _context.Database
            .SqlQuery<string>($@"
WITH transmog_cache (appearance_source) AS (
    SELECT  UNNEST(appearance_sources) AS appearance_source
    FROM    user_cache
    WHERE   user_id = {user.Id}
)
SELECT  sigh.oof
FROM (
    SELECT  DISTINCT wima.item_id || '_' || wima.modifier AS oof
    FROM    wow_item_modified_appearance wima
) sigh
LEFT OUTER JOIN transmog_cache tc
    ON sigh.oof = tc.appearance_source
WHERE   tc.appearance_source IS NULL
").ToArrayAsync();

        timer.AddPoint("MissingAppearances");

        await using var connection = _context.GetConnection();
        await connection.OpenAsync();

        await using var command = new NpgsqlCommand(MissingTransmogByAppearanceSourceQuery.Sql, connection)
        {
            Parameters =
            {
                new() { Value = connectedRealmIds },
                new() { Value = missingAppearanceSources },
            }
        };

        await using var reader = await command.ExecuteReaderAsync();
        var auctions = new List<MissingTransmogByAppearanceSourceQuery>();
        while (await reader.ReadAsync())
        {
            auctions.Add(new()
            {
                ConnectedRealmId = reader.GetInt32(0),
                AppearanceSource = reader.GetString(1),
                ItemId = reader.GetInt32(2),
                TimeLeft = (WowAuctionTimeLeft)reader.GetInt16(3),
                BuyoutPrice = reader.GetInt64(4),
                BonusIds = (int[])reader.GetValue(5),
            });
        }

        timer.AddPoint("Auctions");

        var grouped = auctions
            .GroupBy(auction => auction.AppearanceSource)
            .ToDictionary(
                group => group.Key,
                group => group
                    .OrderBy(auction => auction.BuyoutPrice)
                    .Take(5)
                    .ToList()
            );

        timer.AddPoint("Grouping");

        string json = JsonSerializer.Serialize(grouped, _jsonSerializerOptions);

        timer.AddPoint("JSON", true);

        int kept = grouped.Values
            .Select(groupAuctions => groupAuctions.Count)
            .Sum();
        _logger.LogInformation($"{auctions.Count} rows, kept {kept}");
        _logger.LogInformation($"{timer}");

        return Content(json, MediaTypeNames.Application.Json);
    }

    [HttpPost("missing-recipes")]
    [Authorize]
    public async Task<IActionResult> MissingProfessionRecipes([FromBody] ApiMissingProfessionRecipesForm form)
    {
        var timer = new JankTimer();

        var user = await _userManager.GetUserAsync(HttpContext.User);
        if (user == null)
        {
            _logger.LogWarning("ruh roh");
            return NotFound();
        }

        timer.AddPoint("User");

        var character = await _context.PlayerCharacter
            .Include(pc => pc.Professions)
            .Where(pc => pc.Account.UserId == user.Id && pc.Id == form.CharacterId)
            .FirstOrDefaultAsync();
        if (character == null)
        {
            return NotFound();
        }

        var characterRealm = await _context.WowRealm.FirstAsync(wr => wr.Id == character.RealmId);

        timer.AddPoint("Character");

        // Always apply a region limit
        int[] connectedRealmIds = await _context.WowRealm
            .AsNoTracking()
            .Where(realm => realm.Region == characterRealm.Region)
            .Select(realm => realm.ConnectedRealmId)
            .Distinct()
            .ToArrayAsync();

        if (!form.AllRealms)
        {
            var accounts = await _context.PlayerAccount
                .AsNoTracking()
                .Where(pa => pa.UserId == user.Id && pa.Enabled)
                .Include(pa => pa.Pets)
                .ToArrayAsync();
            var accountConnectedRealmIds = await GetConnectedRealmIds(user, accounts);

            connectedRealmIds = connectedRealmIds
                .Intersect(accountConnectedRealmIds)
                .ToArray();
        }

        timer.AddPoint("Realms");

        // Profession info
        var skillLineIds = new List<int>();
        var skillLineAbilityIds = new List<int>();
        foreach (var (rootId, subProfessions) in character.Professions.Professions)
        {
            skillLineIds.Add(rootId);
            foreach (var (subProfessionId, subProfession) in subProfessions)
            {
                skillLineIds.Add(subProfessionId);
                skillLineAbilityIds.AddRange(subProfession.KnownRecipes);
            }
        }

        // Missing recipes
        var missingRecipeItemIds = await _context.Database
            .SqlQuery<int>($@"
SELECT  item_id
FROM    wow_profession_recipe_item
WHERE   skill_line_id = ANY({skillLineIds})
        AND NOT skill_line_ability_id = ANY({skillLineAbilityIds})
").ToArrayAsync();

        timer.AddPoint("MissingRecipes");

        // Auctions
        await using var connection = _context.GetConnection();
        await connection.OpenAsync();

        await using var command = new NpgsqlCommand(MissingRecipeQuery.Sql, connection)
        {
            Parameters =
            {
                new() { Value = connectedRealmIds },
                new() { Value = missingRecipeItemIds },
            },
        };

        await using var reader = await command.ExecuteReaderAsync();
        var auctions = new List<MissingRecipeQuery>();
        while (await reader.ReadAsync())
        {
            auctions.Add(new()
            {
                ConnectedRealmId = reader.GetInt32(0),
                ItemId = reader.GetInt32(1),
                TimeLeft = (WowAuctionTimeLeft)reader.GetInt16(2),
                BuyoutPrice = reader.GetInt64(3)
            });
        }

        timer.AddPoint("Auctions");

        var grouped = auctions
            .GroupBy(auction => auction.ItemId)
            .ToDictionary(
                group => group.Key,
                group => group
                    .OrderBy(auction => auction.BuyoutPrice)
                    .Take(5)
                    .ToList()
            );

        timer.AddPoint("Grouping");

        string json = JsonSerializer.Serialize(grouped, _jsonSerializerOptions);

        timer.AddPoint("JSON", true);

        int kept = grouped.Values
            .Select(groupAuctions => groupAuctions.Count)
            .Sum();
        _logger.LogInformation($"{auctions.Count} rows, kept {kept}");
        _logger.LogInformation($"{timer}");

        return Content(json, MediaTypeNames.Application.Json);
    }

    private static Dictionary<int, List<WowAuction>> DoAuctionStuff(
        IEnumerable<IGrouping<int, WowAuction>> groupedAuctions,
        bool includeLowBid = true
    )
    {
        var groupedThings = groupedAuctions
            .ToDictionary(
                group => group.Key,
                group => group
                    .ToGroupedDictionary(auction => auction.ConnectedRealmId)
            );

        var ret = new Dictionary<int, List<WowAuction>>();
        foreach (var (thingId, itemRealms) in groupedThings)
        {
            ret[thingId] = new List<WowAuction>();
            foreach (var (_, realmAuctions) in itemRealms)
            {
                var lowestBid = realmAuctions
                    .Where(auction => auction.BidPrice > 0)
                    .MinBy(auction => auction.BidPrice);
                var lowestBuyout = realmAuctions
                    .Where(auction => auction.BuyoutPrice > 0)
                    .MinBy(auction => auction.BuyoutPrice);

                if (lowestBid == null)
                {
                    ret[thingId].Add(lowestBuyout);
                }
                else if (lowestBuyout == null)
                {
                    ret[thingId].Add(lowestBid);
                }
                else if (lowestBid.AuctionId == lowestBuyout.AuctionId)
                {
                    ret[thingId].Add(lowestBid);
                }
                else
                {
                    if (includeLowBid && lowestBid.BidPrice < lowestBuyout.BuyoutPrice)
                    {
                        ret[thingId].Add(lowestBid);
                    }
                    ret[thingId].Add(lowestBuyout);
                }
            }

            if (includeLowBid)
            {
                ret[thingId].Sort((a, b) => a.UsefulPrice.CompareTo(b.UsefulPrice));
            }
            else
            {
                ret[thingId].Sort((a, b) => a.BuyoutPrice.CompareTo(b.BuyoutPrice));
            }
        }

        return ret;
    }

    private async Task<int[]> GetConnectedRealmIds(ApplicationUser user, PlayerAccount[] accounts)
    {
        var accountIds = accounts.SelectArray(account => account.Id);
        var ignoredRealms = user.Settings.Auctions?.IgnoredRealms.EmptyIfNull();

        var accountConnectedRealmIds = await _context.WowRealm
            .AsNoTracking()
            .Where(realm =>
                _context.PlayerCharacter
                    .Where(pc => pc.AccountId != null && accountIds.Contains(pc.AccountId.Value))
                    .Select(pc => pc.RealmId)
                    .Contains(realm.Id) &&
                !ignoredRealms.Contains(realm.ConnectedRealmId)
            )
            .Select(realm => realm.ConnectedRealmId)
            .Distinct()
            .ToArrayAsync();

        return accountConnectedRealmIds;
    }

    private async Task<int[]> GetRegionRealmIds(ApplicationUser user, PlayerAccount[] accounts)
    {
        var accountIds = accounts.SelectArray(account => account.Id);
        var ignoredRealms = user.Settings.Auctions?.IgnoredRealms.EmptyIfNull();

        var regions = await _context.WowRealm
            .AsNoTracking()
            .Where(realm =>
                _context.PlayerCharacter
                    .Where(pc => pc.AccountId != null && accountIds.Contains(pc.AccountId.Value))
                    .Select(pc => pc.RealmId)
                    .Contains(realm.Id)
            )
            .Select(realm => realm.Region)
            .Distinct()
            .ToArrayAsync();

        var regionRealmIds = await _context.WowRealm
            .AsNoTracking()
            .Where(realm => regions.Contains(realm.Region))
            .Select(realm => realm.ConnectedRealmId)
            .ToArrayAsync();

        return regionRealmIds;
    }

    private async Task<Dictionary<int, int>> GetPetItems()
    {
        return await _context.WowPet
            .Where(pet => (pet.Flags & 32) == 0 && pet.ItemId > 0)
            .ToDictionaryAsync(pet => pet.ItemId, pet => pet.Id);
    }
}
