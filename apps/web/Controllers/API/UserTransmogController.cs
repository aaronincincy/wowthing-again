﻿using System.Net.Mime;
using Microsoft.Extensions.Logging;
using Wowthing.Lib.Constants;
using Wowthing.Lib.Contexts;
using Wowthing.Lib.Services;
using Wowthing.Lib.Utilities;
using Wowthing.Web.Services;

namespace Wowthing.Web.Controllers.API;

public class UserTransmogController : Controller
{
    private readonly CacheService _cacheService;
    private readonly ILogger<UserTransmogController> _logger;
    private readonly UserService _userService;
    private readonly WowDbContext _context;

    public UserTransmogController(
        CacheService cacheService,
        ILogger<UserTransmogController> logger,
        UserService userService,
        WowDbContext context
    )
    {
        _cacheService = cacheService;
        _logger = logger;
        _userService = userService;
        _context = context;
    }

    [HttpGet("api/user/{username:username}/transmog-{modified:long}.json")]
    public async Task<IActionResult> UserTransmogData([FromRoute] string username, [FromRoute] long modified)
    {
        var timer = new JankTimer();

        var apiResult = await _userService.CheckUser(User, username);
        if (apiResult.NotFound)
        {
            return NotFound();
        }

        timer.AddPoint("CheckUser");

        var (isModified, lastModified) =
            await _cacheService.CheckLastModified(RedisKeys.UserLastModifiedTransmog, null, apiResult);
        var lastUnix = lastModified.ToUnixTimeSeconds();
        if (lastUnix != modified)
        {
            return RedirectToAction("UserTransmogData", new { username, modified = lastUnix });
        }

        timer.AddPoint("LastModified");

        (string json, lastModified) = await _cacheService
            .GetOrCreateTransmogCacheAsync(_context, timer, apiResult.User.Id, lastModified);

        timer.AddPoint("Build", true);
        _logger.LogDebug("{Timer}", timer);

        if (lastModified > DateTimeOffset.MinValue)
        {
            Response.AddLongApiCacheHeaders(lastModified);
        }

        return Content(json, MediaTypeNames.Application.Json);
    }
}
