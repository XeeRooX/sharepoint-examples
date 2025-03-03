using CacheExample.Cache.Common;
using StackExchange.Redis;

namespace CacheExample.Cache.Redis
{
    public class RedisContextInitializer : CacheContextInitializer
    {
        private readonly ConfigurationOptions _configuration;
        public RedisContextInitializer(ConfigurationOptions configuration)
        {
            _configuration = configuration;
        }
        public override CacheContext Initialize()
        {
            return RedisContext.Instance(_configuration);
        }
    }

    public static class RedisContextConfigurationExtesnsion
    {
        public static CacheContextInitializer UseRedis(this CacheContextConfiguration conf,  ConfigurationOptions configuration)
        {
            return new RedisContextInitializer(configuration);
        }
    }
}
