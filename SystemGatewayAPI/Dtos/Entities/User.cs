using SystemGateway.Dtos.Enum;

namespace SystemGateway.Dtos.Entities
{
    public class User
    {

        public Guid WebPlatformId { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public ISet<string> Modules { get; set; }
        public ISet<string> ActiveScenarios { get; set; }
        public Permission PermissionLevel { get; set; }
    }
}
