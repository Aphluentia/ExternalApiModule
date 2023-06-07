using SystemGateway.Dtos.Entities;
using SystemGateway.Dtos.SecurityManager;

namespace SystemGateway.Dtos.Input
{
    public class DeleteUserInputDto: TokenDto
    {
        public Module Module { get; set; }

    }
}
