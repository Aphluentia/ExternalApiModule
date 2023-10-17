namespace DatabaseApi.Models.Entities
{
    public class Module
    {
        public Guid? Id { get; set; }
        public CustomModuleTemplate ModuleTemplate { get; set; }
    }
}
