using StackExchange.Redis;

namespace DeanInfoSystem.Application.Extensions.Implementation;

public class RedisCacheExt : IRedisCacheExt
{
    private IConnectionMultiplexer _muxer;
    public IDatabase _redis;
    private IServer _srv;
    public RedisCacheExt(IConnectionMultiplexer muxer)
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