using StackExchange.Redis;

namespace JiraDemo.Redis;


public class RedisDb : IRedis
{
    private readonly IDatabase _db;
    
    public RedisDb(IDatabase db)
    {
        _db = db;
    }
    
    public async Task SetAsync(string key, string value, TimeSpan ttl)
    {
        await _db.StringSetAsync(key, value, ttl);
    }

    public async Task SetAsync(string key, TimeSpan ttl)
    {
        await _db.KeyExpireAsync(key, ttl);
    }

    public async Task<string?> GetAsync(string key)
    {
        return await _db.StringGetAsync(key);
    }

    public async Task DeleteAsync(string key)
    {
        await _db.KeyDeleteAsync(key);
    }

    public async Task<long> IncrementAsync(string key)
    {
        return await _db.StringIncrementAsync(key);
    }
}