namespace ExternalAPI.Models.Dtos.Broker
{
    public class RegisterModuleConnectionInputDto: SafeSessionDto
    {
        public int ModuleType { get; set; }
        public string ModuleId { get; set; }
    }
}
