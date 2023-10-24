namespace SystemGatewayAPI.Dtos.Entities.Database
{
    public class Module
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public ModuleVersion ModuleData { get; set; }
    }
}
