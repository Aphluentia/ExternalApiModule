using SystemGateway.Dtos.Enum;

namespace SystemGateway.Dtos.Entities
{
    public class Module
    {
        public Guid Id { get; set; }
        public ModuleType ModuleType { get; set; }
        public string Data { get; set; }
        public DateTime DateTime { get; set; }
        public string Checksum { get; set; }
    }
}
