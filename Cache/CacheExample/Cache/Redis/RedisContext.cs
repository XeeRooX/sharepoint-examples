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
        private static string _configurationString;
        private static readonly object _lock = new object();
        public RedisContext(string configurationString)
        {
            InstanceMultiplexer(configurationString);
        }
        private static bool ConfigurationIsUpdated(string configurationString) => configurationString != _configurationString;

        private static void InstanceMultiplexer(string configurationString)
        {
            if (_multiplexer == null || ConfigurationIsUpdated(configurationString))
            {
                lock (_lock)
                {
                    if (_multiplexer == null)
                    {
                        var configuration = ConfigurationOptions.Parse(configurationString);

                        _multiplexer = ConnectionMultiplexer.Connect(configuration);
                        _db = _multiplexer.GetDatabase();
                        _configurationString = configurationString;
                    }
                }
            }
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
