using DeanInfoSystem.Application.Common.Caching;
using StackExchange.Redis;

namespace DeanInfoSystem.Infrastructure.Caching;

public class RedisCache : ICacheService
{
    private IConnectionMultiplexer _muxer;
    public IDatabase _redis;
    private IServer _srv;
    public RedisCache(IConnectionMultiplexer muxer)
    {
        _muxer = muxer;
        _redis = _muxer.GetDatabase();
        _srv = _muxer.GetServer(_muxer.GetEndPoints().FirstOrDefault()!);
        _srv.FlushAllDatabases();
    }

    public IDatabase GetRedis()
    {
        return _redis;
    }
}