using SystemGateway.Dtos.Entities;
using SystemGateway.Dtos.SecurityManager;

namespace SystemGateway.Dtos.Input
{
    public class ModuleConnectionInputDto: TokenDto
    {
        public ModuleConnection ModuleConnection { get; set; }
    }
}
