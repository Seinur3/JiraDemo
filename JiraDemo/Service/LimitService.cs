using JiraDemo.Redis;

namespace WebApplication3.Services;

public class LimitService
{
    private readonly IRedis _redisDatabase;

    public LimitService(IRedis redisDatabase)
    {
        _redisDatabase = redisDatabase;
    }

    public async Task<bool> CanLogin(string ip)
    {
        var login = $"login:{ip}";
        var attempts = await _redisDatabase.IncrementAsync(login);
        if (attempts == 1)
        {
            await _redisDatabase.SetAsync(login, TimeSpan.FromSeconds(30));
        }
        return attempts < 5;
    }
}