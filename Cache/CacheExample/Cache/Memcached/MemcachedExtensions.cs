using Enyim.Caching.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CacheExample.Cache.Memcached
{
    public static class MemcachedConfigurationExtension
    {
        public static bool IsEqual(this MemcachedClientConfiguration confSrc, MemcachedClientConfiguration confComaprable)
        {
            return ServersIsEqual(confSrc.Servers, confComaprable.Servers)
                && confSrc.Protocol == confComaprable.Protocol
                && confSrc.SocketPool.ConnectionTimeout == confComaprable.SocketPool.ConnectionTimeout
                && confSrc.SocketPool.DeadTimeout == confComaprable.SocketPool.DeadTimeout
                && confSrc.SocketPool.MinPoolSize == confComaprable.SocketPool.MinPoolSize
                && confSrc.SocketPool.MaxPoolSize == confComaprable.SocketPool.MaxPoolSize
                && confSrc.KeyTransformer.GetType().Name == confComaprable.KeyTransformer.GetType().Name;
        }

        private static bool ServersIsEqual(IList<IPEndPoint> serversSrc, IList<IPEndPoint> serversDest)
        {
            var serversIpsSource = serversSrc.Select(x => x.ToString());
            var serverIpsDestination = serversDest.Select(x => x.ToString());

            var result = serversIpsSource.SequenceEqual(serverIpsDestination);
            return result;
        }
    }
}
