using PublicAPI.Models.Entities;
using PublicAPI.Models.Enums;

namespace PublicAPI.Models.DatabaseDtos
{
    public class UserDto
    {
        public Guid WebPlatformId { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public ISet<string> Modules { get; set; }
        public ISet<string> ActiveScenarios { get; set; }
        public PermissionLevel PermissionLevel { get; set; }
    }
}
