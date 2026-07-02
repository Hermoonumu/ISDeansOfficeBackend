using System.ComponentModel;
using DeanInfoSystem.Application.Common.Caching;
using StackExchange.Redis;

namespace DeanInfoSystem.Infrastructure.Caching;

public class RedisCache : ICacheService
{
    private IConnectionMultiplexer _muxer;
    public IDatabase _redis;
    private IServer _srv;
    private SystemDbContext _db;
    public RedisCache(IConnectionMultiplexer muxer, SystemDbContext db)
    {
        _muxer = muxer;
        _redis = _muxer.GetDatabase();
        _srv = _muxer.GetServer(_muxer.GetEndPoints().FirstOrDefault()!);
        _db = db;
    }

    public IDatabase GetRedis()
    {
        return _redis;
    }


    public async Task SetAsync(string key, string value, TimeSpan? ttl = null)
    {
        await _redis.StringSetAsync(key, value, ttl);
    }

    public async Task RemoveAsync(string key)
    {
        if (key is null) return;
        Console.WriteLine($"THE KEY: {key}");
        await _redis.KeyDeleteAsync(key);
    }

    public async Task<string?> GetAsync(string key)
    {
        return await _redis.StringGetAsync(key);
    }
}