using ExternalAPI.Helpers;
using ExternalAPI.Models.Enums;

namespace ExternalAPI.Models.DatabaseDtos
{
    public class ModuleDto
    {
        public ModuleType ModuleType { get; set; }
        public string Data { get; set; }
        public Guid Id { get; set; }
        public DateTime? DateTime { get; set; }
        public string? Checksum { get; set; }
    }
}
