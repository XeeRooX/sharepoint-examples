using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CacheExample.Cache.Common;
using Enyim.Caching.Configuration;
using CacheExample.Cache.Memcached;
using System.Collections.Generic;
using System.Linq;

namespace CacheExample.Test.Cache
{
    [TestClass]
    public class MemcachedContext
    {
        private CacheContextBuilder builder;
        [TestInitialize]
        public void Initialize()
        {
            var serverUrl = "192.168.1.10:11211";

            builder = new CacheContextBuilder()
                .ConfigureInitializer(conf =>
                {
                    var memcachedConfiguration = new MemcachedClientConfiguration();
                    memcachedConfiguration.AddServer(serverUrl);
                    return conf.UseMemcached(memcachedConfiguration);
                });
        }


        [TestMethod]
        public void TestSingleton()
        {
            int countInstance = 10;
            var guidList = new List<string>();

            for (int i = 0; i < countInstance; i++)
            {
                using (var cacheContext = builder.Build())
                {
                    var hashCode = cacheContext.GetHashCode().ToString();
                    Console.WriteLine(hashCode);
                    guidList.Add(hashCode);
                }
            }

            Assert.IsTrue(guidList.Distinct().Count() == 1);
        }
    }
}
