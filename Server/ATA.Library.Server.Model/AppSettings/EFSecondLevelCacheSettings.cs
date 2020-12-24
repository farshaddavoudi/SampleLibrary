using System;

namespace ATA.Library.Server.Model.AppSettings
{
    public class EFSecondLevelCacheSettings
    {
        public bool Enabled { get; set; }
        public bool IsSlidingMode { get; set; }
        public TimeSpan DefaultCacheTimespan { get; set; }
    }
}