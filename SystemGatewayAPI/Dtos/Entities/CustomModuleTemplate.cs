namespace DatabaseApi.Models.Entities
{
    public class CustomModuleTemplate: ModuleVersion
    {
        public string ModuleName { get; set; }
        public static CustomModuleTemplate FromModuleTemplate(Application template, ModuleVersion version)
        {
            return new CustomModuleTemplate
            {
                ModuleName = template.ModuleName,
                VersionId = version.VersionId,
                DataStructure = version.DataStructure,
                HtmlCard = version.HtmlCard,
                HtmlDashboard = version.HtmlDashboard

            };
        }
        public static CustomModuleTemplate FromExistingModule(Module module)
        {
            return new CustomModuleTemplate
            {
                ModuleName = module.ModuleTemplate.ModuleName,
                VersionId = module.ModuleTemplate.VersionId,
                DataStructure = module.ModuleTemplate.DataStructure,
                HtmlCard = module.ModuleTemplate.HtmlCard,
                HtmlDashboard = module.ModuleTemplate.HtmlDashboard

            };
        }
    }
}
