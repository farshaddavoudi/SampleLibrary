namespace ATA.Library.Server.Model.AppSettings
{
    public class AppSettings
    {
        public ConnectionStrings? ConnectionStrings { get; set; }
        public Urls? Urls { get; set; }
        public RedisServerSettings? RedisServerSettings { get; set; }
        public EasyCachingSettings? EasyCachingSettings { get; set; }
        public EFSecondLevelCacheSettings? EFSecondLevelCacheSettings { get; set; }
        public JobSettings? JobSettings { get; set; }
    }

}