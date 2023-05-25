using ExternalAPI.Models.Entities;
using ExternalAPI.Models.Enums;

namespace ExternalAPI.Models.DatabaseDtos
{
    public class UserDto
    {

        public Guid? WebPlatformId { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public ISet<string>? Modules { get; set; }
        public ISet<string>? ActiveScenariosIds { get; set; }

        public string Password { get; set; }
        public PermissionLevel PermissionLevel { get; set; }
    }
}
