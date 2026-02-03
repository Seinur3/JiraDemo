namespace JiraDemo.Redis;

public interface IRedis
{
    Task SetAsync(string key, string value, TimeSpan ttl);
    Task SetAsync(string key, TimeSpan ttl);
    Task<string?> GetAsync(string key);
    Task DeleteAsync(string key);
    Task<long> IncrementAsync(string key);
    
}