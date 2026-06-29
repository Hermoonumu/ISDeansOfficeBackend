
using StackExchange.Redis;

namespace DeanInfoSystem.Application.Extensions;



public interface IRedisCacheExt
{
    public IDatabase GetRedis();
}