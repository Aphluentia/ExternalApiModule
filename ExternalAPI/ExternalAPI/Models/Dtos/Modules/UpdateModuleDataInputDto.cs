using ExternalAPI.Models.DatabaseDtos;
using ExternalAPI.Models.Enums;

namespace ExternalAPI.Models.Dtos.Modules
{
    public class UpdateModuleDataInputDto
    {
        public ModuleType ModuleType { get; set; }
        public string Data { get; set; }
        public Guid Id { get; set; }
    }
}
