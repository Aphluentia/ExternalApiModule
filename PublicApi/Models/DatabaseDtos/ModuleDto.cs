using PublicAPI.Helpers;
using PublicAPI.Models.Enums;

namespace PublicAPI.Models.DatabaseDtos
{
    public class ModuleDto
    {
        public ModuleType ModuleType { get; set; }
        public string Data { get; set; }
        public Guid Id { get; set; }
        public DateTime? Timestamp => DateTime.UtcNow;
        public string? Checksum { get; set; }
    }
}
