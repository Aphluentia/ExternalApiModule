using SystemGateway.Dtos.Enum;

namespace SystemGateway.Dtos.SecurityManager
{
    public class SecurityDataDto
    {
        public string Email { get; set; }
        public UserType UserType { get; set; }
        public DateTime Expires { get; set; }
        public bool IsExpired { get; set; }
        public int TimeLeft { get; set; }
    }
}
