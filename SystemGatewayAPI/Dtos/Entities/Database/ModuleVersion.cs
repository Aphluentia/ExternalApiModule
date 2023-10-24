namespace SystemGatewayAPI.Dtos.Entities.Database
{
    public class ModuleVersion
    {
        public string VersionId { get; set; }
        public string ApplicationName { get; set; }
        public ICollection<DataPoint> DataStructure { get; set; } = new List<DataPoint>();
        public string? HtmlCard { get; set; } = "";
        public string? HtmlDashboard { get; set; } = "";
        public DateTime? Timestamp { get; set; } = DateTime.UtcNow;
        public string? Checksum { get; set; } = "";
    }
}
