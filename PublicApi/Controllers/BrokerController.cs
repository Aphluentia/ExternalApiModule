using PublicAPI.Models.Dtos.Authentication;
using PublicAPI.Models.Dtos;
using PublicAPI.Operations;
using PublicAPI.Services.Base;
using Microsoft.AspNetCore.Mvc;
using PublicAPI.Models.Dtos.Broker;

namespace PublicAPI.Controllers
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

        [HttpPost("RegisterModuleConnection")]
        public async Task<OutputMessage<RegisterModuleConnectionOutputDto>> RegisterModuleConnection([FromBody] RegisterModuleConnectionInputDto input) => await (new RegisterModuleConnectionOperation((ServiceAggregator)_ServiceAggregator)).Init(input);
        
        [HttpPost("CloseModuleConnection")]
        public async Task<OutputMessage<CloseModuleConnectionOutputDto>> CloseModuleConnection([FromBody] CloseModuleConnectionInputDto input) => await (new CloseModuleConnectionOperation((ServiceAggregator)_ServiceAggregator)).Init(input);

    }
}
