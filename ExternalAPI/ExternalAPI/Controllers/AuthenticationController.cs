using ExternalAPI.Models.Dtos.Base;
using ExternalAPI.Models.Dtos;
using ExternalAPI.Operations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ExternalAPI.Models.Dtos.Authentication;
using Microsoft.Extensions.Options;
using ExternalAPI.Configurations;
using ExternalAPI.Services;
using ExternalApi.Services;
using ExternalAPI.Services.Base;

namespace ExternalAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IServiceAggregator _ServiceAggregator;
        public AuthenticationController(IServiceAggregator ServiceAggregator)
        {
            this._ServiceAggregator = ServiceAggregator;
        }

        [HttpPost("GenerateSession")]
        public async Task<OutputMessage<GenerateSessionOutputDto>> CreateSession([FromBody] GenerateSessionInputDto generateSessionInputDto) => await (new GenerateSessionOperation((ServiceAggregator)_ServiceAggregator)).Init(generateSessionInputDto);


       
    }
}
