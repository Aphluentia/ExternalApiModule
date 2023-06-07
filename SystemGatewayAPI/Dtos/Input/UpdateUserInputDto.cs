using SystemGateway.Dtos.Entities;
using SystemGateway.Dtos.SecurityManager;

namespace SystemGateway.Dtos.Input
{
    public class UpdateUserInputDto: TokenDto
    {
        public User User { get; set; }
    }
}
