using SystemGatewayAPI.Dtos.Entities.Database;

namespace SystemGatewayAPI.Dtos.SecurityManager
{
    public class ModuleSnapshot
    {
        public Guid ModuleId { get; set; }
        public string Checksum { get; set; }
        public DateTime Timestamp { get; set; }
        public static ModuleSnapshot FromModule(Module module)
        {
            return new ModuleSnapshot
            {
                ModuleId = (Guid)module.Id,
                Checksum = module.ModuleData.Checksum,
                Timestamp = ((DateTime)(module.ModuleData.Timestamp == null ? DateTime.UtcNow : module.ModuleData.Timestamp))
            };
        }
    }
}
