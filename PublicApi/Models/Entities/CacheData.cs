namespace PublicAPI.Models.Entities
{
    public class CacheData
    {
        public string Checksum { get; set; }
        public DateTime Timestamp => DateTime.UtcNow;
    }
}
