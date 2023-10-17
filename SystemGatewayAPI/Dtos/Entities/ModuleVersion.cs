namespace DatabaseApi.Models.Entities
{
    public class ModuleVersion
    {
        public string VersionId { get; set; }
        public ICollection<DataPoint> DataStructure { get; set; }
        public string? HtmlCard { get; set; }
        public string? HtmlDashboard { get; set; }
        public DateTime Timestamp => DateTime.UtcNow;
        public string Checksum { get; set; }
    }
}
