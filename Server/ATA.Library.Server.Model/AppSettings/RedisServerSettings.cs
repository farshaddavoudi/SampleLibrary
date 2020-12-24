namespace ATA.Library.Server.Model.AppSettings
{
    public class RedisServerSettings
    {
        public bool IsRedisAvailable { get; set; }
        public string? RedisHost { get; set; }
        public int RedisPort { get; set; }
        public string? RedisServerPass { get; set; }
        public int RedisDbNo { get; set; }
    }
}