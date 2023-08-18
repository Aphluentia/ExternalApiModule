using SystemGateway.Dtos.Enum;

namespace SystemGateway.Dtos.Input
{
    public class AuthenticateUserInputDto
    {
        public string Email { get; set; }
        public string Password { get; set; } 
        public UserType UserType { get; set; }
    }
}
