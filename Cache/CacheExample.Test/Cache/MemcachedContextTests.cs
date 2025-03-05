using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CacheExample.Cache.Common;
using Enyim.Caching.Configuration;
using CacheExample.Cache.Memcached;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CacheExample.Test.Cache
{
    [TestClass]
    public class MemcachedContextTests
    {
        [TestInitialize]
        public void Initialize()
        {
        }


        [TestMethod]
        public void SingletonTest()
        {
            var serverUrl = "192.168.1.10:11211";
            var memcachedConfiguration = new MemcachedClientConfiguration();
            memcachedConfiguration.AddServer(serverUrl);

            int countInstance = 10;
            var guidList = new List<string>();

            for (int i = 0; i < countInstance; i++)
            {
                var context = new MemcachedContextTest(memcachedConfiguration);            
                guidList.Add(context.GetClientHashCode());
            }

            Assert.IsTrue(guidList.Distinct().Count() == 1);
        }

        [TestMethod]
        public void ConfigurationChangeTest()
        {
            // Arrange
            var serverUrl = "192.168.1.10:11211";
            var serverUrlSecond = "192.168.1.11:11211";

            var memcachedConfiguration = new MemcachedClientConfiguration();
            memcachedConfiguration.AddServer(serverUrl);
            var memcachedConfigurationSecond = new MemcachedClientConfiguration();
            memcachedConfigurationSecond.AddServer(serverUrlSecond);

            // Act
            var context = new MemcachedContextTest(memcachedConfiguration);
            var clientHash = context.GetClientHashCode();
            var contextChanged = new MemcachedContextTest(memcachedConfigurationSecond);
            var clientSecondHash = context.GetClientHashCode();


            // Assert
            Assert.AreNotEqual(clientHash, clientSecondHash);
        }

        [TestMethod]
        public void MultithreadTest()
        {
            // Arrange
            int taskCount = 10000;
            var tasks = new List<Task>();

            var serverUrl = "192.168.1.10:11211";
            var memcachedConfiguration = new MemcachedClientConfiguration();
            memcachedConfiguration.AddServer(serverUrl);
            var guidList = new List<string>();

            var locker = new object();
            for (int i = 0; i < taskCount; i++)
            {
                tasks.Add(Task.Run(() => 
                {   
                    var context = new MemcachedContextTest(memcachedConfiguration);
                    lock (locker)
                    {
                        guidList.Add(context.GetClientHashCode());
                    }     
                }));
            }

            // Act
            Task.WaitAll(tasks.ToArray());

            // Assert
            Assert.IsTrue(guidList.Distinct().Count() == 1);
        }
    }
}
