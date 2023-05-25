using ExternalAPI.Models.Dtos.Users;
using ExternalAPI.Models.Dtos;
using ExternalAPI.Operations;
using ExternalAPI.Services.Base;
using Microsoft.AspNetCore.Mvc;

namespace ExternalAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IServiceAggregator _ServiceAggregator;
        public UserController(IServiceAggregator ServiceAggregator)
        {
            this._ServiceAggregator = ServiceAggregator;
        }
        [HttpPost("CreateUser")]
        public async Task<OutputMessage<CreateUserOutputDto>> CreateUser([FromBody] CreateUserInputDto input) => await (new CreateUserOperation((ServiceAggregator)_ServiceAggregator)).Init(input);

        [HttpPost("RetrieveUserInformation")]
        public async Task<OutputMessage<RetrieveUserInfoOutputDto>> RetrieveUserInformation([FromBody] RetrieveUserInfoInputDto input) => await (new RetrieveUserInformationOperation((ServiceAggregator)_ServiceAggregator)).Init(input);
    }
}
