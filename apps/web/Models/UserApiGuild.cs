﻿using Wowthing.Lib.Models;
using Wowthing.Lib.Models.Player;

namespace Wowthing.Web.Models;

public class UserApiGuild
{
    public int Id { get; set; }
    public int RealmId { get; set; }
    public string Name { get; set; }

    public UserApiGuild(
        PlayerGuild guild,
        bool pub,
        ApplicationUserSettingsPrivacy privacy = null)
    {
        Id = guild.Id;

        if (pub && privacy?.Anonymized == true)
        {
            Name = $"Goose Farm {Id:X4}";
        }
        else
        {
            Name = guild.Name;
            RealmId = guild.RealmId;
        }
    }
}
