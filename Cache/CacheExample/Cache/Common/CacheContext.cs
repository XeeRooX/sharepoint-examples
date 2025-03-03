using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CacheExample.Cache.Common
{
    public abstract class CacheContext : IDisposable
    {
        public abstract string Get(string key);
        public abstract bool Set(string key, string value, TimeSpan? expiry = null);
        public abstract Task<string> GetAsync(string key);
        public abstract Task<bool> SetAsync(string key, string value, TimeSpan? expiry = null);
        public abstract void Dispose();
    }
}
