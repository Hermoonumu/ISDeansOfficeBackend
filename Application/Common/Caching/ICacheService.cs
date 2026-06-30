
using StackExchange.Redis;

namespace DeanInfoSystem.Application.Common.Caching;



public interface ICacheService
{
    public IDatabase GetRedis();
}