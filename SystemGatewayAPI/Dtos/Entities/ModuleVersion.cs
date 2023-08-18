namespace DatabaseApi.Models.Entities
{
    public class ModuleVersion
    {
        public string VersionId { get; set; }
        public DateTime? Timestamp { get; set; }
        public string? DataStructure { get; set; }
        public string? HtmlCard { get; set; }
        public string? HtmlDashboard { get; set; }
    }
}
