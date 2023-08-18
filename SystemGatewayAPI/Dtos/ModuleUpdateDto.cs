using DatabaseApi.Models.Entities;

namespace SystemGatewayAPI.Dtos
{
    public class ModuleUpdateDto
    {
        public bool HasUpdates { get; set; }
        public Module? ModuleData { get; set; }
    }
}
