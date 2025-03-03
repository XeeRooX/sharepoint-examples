using CacheExample.Cache.Common;
using StackExchange.Redis;
using System;
using System.Threading.Tasks;

namespace CacheExample.Cache.Redis
{
    public class RedisContext : CacheContext
    {
        private static ConnectionMultiplexer _multiplexer;
        private static IDatabase _db;
        private static RedisContext _instance;
        private static readonly object _lock = new object();
        private RedisContext()
        {
        }

        public static RedisContext Instance(ConfigurationOptions configuration)
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new RedisContext();
                        _multiplexer = ConnectionMultiplexer.Connect(configuration);
                        _db = _multiplexer.GetDatabase();
                    }
                }
            }

            return _instance;
        }

        public override void Dispose()
        {
            _multiplexer.Dispose();
        }

        public override string Get(string key)
        {
            var value = _db.StringGet(new RedisKey(key));
            return string.IsNullOrEmpty(value.ToString()) ? null : value.ToString();
        }

        public override bool Set(string key, string value, TimeSpan? expiry)
        {
            return _db.StringSet(new RedisKey(key), new RedisValue(value), expiry: expiry);
        }

        public override async Task<string> GetAsync(string key)
        {
            var result = await _db.StringGetAsync(new RedisKey(key));
            return result.ToString();
        }

        public override Task<bool> SetAsync(string key, string value, TimeSpan? expiry)
        {
            return _db.StringSetAsync(new RedisKey(key), new RedisValue(value), expiry: expiry);
        }
    }
}
