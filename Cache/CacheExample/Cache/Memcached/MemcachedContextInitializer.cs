using CacheExample.Cache.Common;
using Enyim.Caching.Configuration;

namespace CacheExample.Cache.Memcached
{
    public class MemcachedContextInitializer : CacheContextInitializer
    {
        private readonly MemcachedClientConfiguration _configuration;
        public MemcachedContextInitializer(MemcachedClientConfiguration configuration)
        {
            _configuration = configuration;
        }

        public override CacheContext Initialize()
        {
            return MemcachedContext.Instance(_configuration);
        }
    }

    public static class MemcachedContextConfigurationExtesnsion
    {
        public static CacheContextInitializer UseMemcached(this CacheContextConfiguration conf, MemcachedClientConfiguration configuration)
        {
            return new MemcachedContextInitializer(configuration);
        }
    }
}
