using ExternalAPI.Models.Dtos.Authentication;
using ExternalAPI.Models.Dtos;
using ExternalAPI.Operations;
using ExternalAPI.Services.Base;
using Microsoft.AspNetCore.Mvc;
using ExternalAPI.Models.Dtos.Broker;

namespace ExternalAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BrokerController : ControllerBase
    {
        private readonly IServiceAggregator _ServiceAggregator;
        public BrokerController(IServiceAggregator ServiceAggregator)
        {
            this._ServiceAggregator = ServiceAggregator;
        }

        [HttpPost("GenerateSession")]
        public async Task<OutputMessage<RegisterModuleConnectionOutputDto>> CreateSession([FromBody] RegisterModuleConnectionInputDto input) => await (new RegisterModuleConnectionOperation((ServiceAggregator)_ServiceAggregator)).Init(input);

    }
}
