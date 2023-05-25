namespace ExternalAPI.Models.Dtos.Broker
{
    public class CloseModuleConnectionInputDto: SafeSessionDto
    {
        public string ModuleId { get; set; }
        public int ModuleType { get; set; }
    }
}
