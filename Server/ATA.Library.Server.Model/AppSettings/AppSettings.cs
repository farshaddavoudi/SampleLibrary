namespace ATA.Library.Server.Model.AppSettings
{
    public class AppSettings
    {
        public ConnectionStrings? ConnectionStrings { get; set; }

        public Urls? Urls { get; set; }

        public Seq? Seq { get; set; }

        public RedisServerSettings? RedisServerSettings { get; set; }

        public EasyCachingSettings? EasyCachingSettings { get; set; }

        public EFSecondLevelCacheSettings? EFSecondLevelCacheSettings { get; set; }

        public JobSettings? JobSettings { get; set; }

        public FileUploadPath? FileUploadPath { get; set; }
    }

}