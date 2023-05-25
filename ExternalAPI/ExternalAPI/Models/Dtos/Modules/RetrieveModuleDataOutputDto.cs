using ExternalAPI.Models.Entities;
using ExternalAPI.Models.Enums;
using System.Text.Json;

namespace ExternalAPI.Models.Dtos.Modules
{
    public class RetrieveModuleDataOutputDto
    {
        public ModuleType ModuleType { get; set; }
        public JsonDocument Data { get; set; }
        public Guid Id { get; set; }
        public DateTime DateTime { get; set; }
        public string Checksum { get; set; }

    }
}
