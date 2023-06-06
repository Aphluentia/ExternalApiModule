using PublicAPI.Models.DatabaseDtos;
using PublicAPI.Models.Enums;

namespace PublicAPI.Models.Dtos.Modules
{
    public class UpdateModuleDataInputDto
    {
        public ModuleType ModuleType { get; set; }
        public string Data { get; set; }
        public Guid Id { get; set; }
    }
}
