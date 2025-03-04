using CacheExample.Cache.Common;
using Enyim.Caching;
using Enyim.Caching.Configuration;
using Enyim.Caching.Memcached;
using System;
using System.Threading.Tasks;

namespace CacheExample.Cache.Memcached
{
    public class MemcachedContext : CacheContext
    {
        private static MemcachedClient _client;
        private static MemcachedContext _instance;
        private static readonly object _lock = new object();

        private MemcachedContext()
        {
        }

        public static MemcachedContext Instance(MemcachedClientConfiguration configuration)
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new MemcachedContext();
                        _client = new MemcachedClient(configuration);
                    }
                }
            }

            return _instance;
        }
        public override string Get(string key)
        {  
            return _client.Get(key)?.ToString();
        }

        public override bool Set(string key, string value, TimeSpan? expiry = null)
        {
            if(expiry == null)
                return _client.Store(StoreMode.Add, key, value);
            else
                return _client.Store(StoreMode.Add, key, value, expiry.Value);
        }

        public override void Dispose()
        {
            _client.Dispose();
        }

        public override Task<string> GetAsync(string key)
        {
            return Task.Run(() => Get(key));
        }

        public override Task<bool> SetAsync(string key, string value, TimeSpan? expiry = null)
        {
            return Task.Run(() => Set(key, value, expiry));
        }
    }
}
