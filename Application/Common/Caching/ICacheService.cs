
using StackExchange.Redis;

namespace DeanInfoSystem.Application.Common.Caching;



public interface ICacheService
{
    public IDatabase GetRedis();
    public Task SetAsync(string key, string value, TimeSpan? ttl = null);
    public Task RemoveAsync(string key);
    public Task<string?> GetAsync(string key);
}