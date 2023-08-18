namespace DatabaseApi.Models.Entities
{
    public class Application
    {
        public string ModuleName { get; set; }
        public ICollection<ModuleVersion> Versions { get; set; }
    }
}
