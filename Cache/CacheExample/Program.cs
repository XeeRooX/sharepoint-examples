﻿using CacheExample.Cache.Common;
using CacheExample.Cache.Memcached;
using CacheExample.Cache.Redis;
using Enyim.Caching;
using Enyim.Caching.Configuration;
using Enyim.Caching.Memcached;
using StackExchange.Redis;
using System;
using System.Collections.Generic;

namespace CacheExample
{
    class Program
    {
        static void Main(string[] args)
        {
            const string redisServer = "192.168.1.10:6379";
            const string redisPassword = "eYVX7EwVmmxKPCDmwMtyKVge8oLd2t81";
            const string memcachedServer = "192.168.1.10:11211";

            MemcachedExample(memcachedServer);
            RedisExample(redisServer, redisPassword);

            Console.ReadLine();
        }

        static void MemcachedExample(string server)
        {
            var memcachedConfiguration = new MemcachedClientConfiguration();
            memcachedConfiguration.AddServer(server);
            CacheContext cacheContext = new MemcachedContext(memcachedConfiguration);

            cacheContext.Set("key", "value", TimeSpan.FromSeconds(5));
            var value = cacheContext.Get("key");

            Console.WriteLine($"Memcached: {value}");
        }

        static void RedisExample(string server, string password)
        {
            var redisConfiguration = $"{server},password={password}";
            CacheContext cacheContext = new RedisContext(redisConfiguration);

            cacheContext.Set("key", "value", expiry: TimeSpan.FromSeconds(5));
            var value = cacheContext.Get("key");

            Console.WriteLine($"Redis: {value}");
        }
    }
}
