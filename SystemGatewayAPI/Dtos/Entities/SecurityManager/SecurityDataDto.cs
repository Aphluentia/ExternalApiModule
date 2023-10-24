using SystemGateway.Dtos.Enum;
using SystemGatewayAPI.Dtos.SecurityManager;

namespace SystemGateway.Dtos.SecurityManager
{
    public class SecurityDataDto
    {
        public string Email { get; set; }
        public UserType UserType { get; set; }
        public DateTime Expires { get; set; }
        public bool IsExpired => DateTime.UtcNow > Expires;
        public ICollection<ModuleSnapshot> ModuleSnapshots { get; set; } = new List<ModuleSnapshot>();
    }
}
