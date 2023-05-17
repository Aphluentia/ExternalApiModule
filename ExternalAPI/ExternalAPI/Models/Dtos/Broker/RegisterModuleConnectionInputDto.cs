namespace ExternalAPI.Models.Dtos.Broker
{
    public class RegisterModuleConnectionInputDto
    {
        public int ModuleType { get; set; }
        public string WebPlatformId { get; set; }
        public string ModuleId { get; set; }
    }
}
