namespace SystemGatewayAPI.Dtos.Entities.Database
{
    public class Application
    {
        public string ApplicationName { get; set; }
        public ICollection<ModuleVersion> Versions { get; set; }
    }
}
