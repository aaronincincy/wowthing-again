﻿using Wowthing.Lib.Utilities;

namespace Wowthing.Backend.Jobs.User;

public class UserCacheAchievementsJob : JobBase
{
    private JankTimer _timer;

    public override async Task Run(params string[] data)
    {
        _timer = new JankTimer();

        long userId = long.Parse(data[0]);
        using var shrug = UserLog(userId);

        var db = Redis.GetDatabase();
        await CacheService.CreateAchievementCacheAsync(Context, db, _timer, userId);

        Logger.Debug("{0}", _timer.ToString());
    }
}
