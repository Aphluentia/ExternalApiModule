namespace DatabaseApi.Models.Entities
{
    public class Module
    {
        public string Id { get; set; }
        public string Data { get; set; }
        public CustomModuleTemplate ModuleTemplate { get; set; }
        public DateTime Timestamp => DateTime.UtcNow;
        public string Checksum { get; set; }
    }
}
