using PublicAPI.Models.Dtos.Users;
using PublicAPI.Models.Dtos;
using PublicAPI.Operations;
using PublicAPI.Services.Base;
using Microsoft.AspNetCore.Mvc;
using PublicAPI.Models.Dtos.Authentication;

namespace PublicAPI.Controllers
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

        [HttpPost("GenerateSession")]
        public async Task<OutputMessage<GenerateSessionOutputDto>> CreateSession([FromBody] GenerateSessionInputDto generateSessionInputDto) => await (new GenerateSessionOperation((ServiceAggregator)_ServiceAggregator)).Init(generateSessionInputDto);


    }
}
