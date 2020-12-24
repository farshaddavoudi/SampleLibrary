using System;

namespace ATA.Library.Server.Model.AppSettings
{
    public class EasyCachingSettings
    {
        public bool Enabled { get; set; }
        public bool DisableInMemoryCache { get; set; }
        public string? RedisCacheProviderName { get; set; }
        public string? InMemoryCacheProviderName { get; set; }
        public int CacheBusPort { get; set; }
        public TimeSpan DefaultCacheTimespan { get; set; }
    }
}