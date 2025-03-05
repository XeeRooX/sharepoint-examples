using CacheExample.Cache.Memcached;
using Enyim.Caching.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CacheExample.Test.Cache
{
    class MemcachedContextTest : MemcachedContext
    {
        public MemcachedContextTest(MemcachedClientConfiguration configuration) : base(configuration)
        {
        }

        public string GetClientHashCode() => _client.GetHashCode().ToString();
    }
}
