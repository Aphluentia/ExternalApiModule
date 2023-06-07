using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SystemGateway.Dtos.Input;
using SystemGateway.Helpers;
using SystemGateway.Providers;

namespace SystemGateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ModuleConnectionController : ControllerBase
    {
        private readonly IServiceAggregator ServiceAggregator;
        public ModuleConnectionController(IServiceAggregator aggregator)
        {
            this.ServiceAggregator = aggregator;
        }
        [HttpPost]
        public async Task<IActionResult> RegisterModuleConnection([FromBody] ModuleConnectionInputDto input)
        {
            if (string.IsNullOrEmpty(await ServiceAggregator.SecurityManagerProvider.ValidateSession(input.Token)))
            {
                return Unauthorized();
            }
            var claims = await ServiceAggregator.SecurityManagerProvider.GetTokenData(input.Token);
            var dbUser = await ServiceAggregator.DatabaseProvider.FindUserById(claims.UserId);
            if (dbUser.WebPlatformId != input.ModuleConnection.WebPlatformId)
                return Unauthorized();
            
            var result = await ServiceAggregator.OperationsManagerProvider.RegisterConnection(input.ModuleConnection);
            if (!result)
                return BadRequest();
            return Ok();
        }
        [HttpPost]
        public async Task<IActionResult> DeleteModuleConnection([FromBody] ModuleConnectionInputDto input)
        {
            if (string.IsNullOrEmpty(await ServiceAggregator.SecurityManagerProvider.ValidateSession(input.Token)))
            {
                return Unauthorized();
            }
            var claims = await ServiceAggregator.SecurityManagerProvider.GetTokenData(input.Token);
            var dbUser = await ServiceAggregator.DatabaseProvider.FindUserById(claims.UserId);
            if (dbUser.WebPlatformId != input.ModuleConnection.WebPlatformId)
                return Unauthorized();
            
            var result = await ServiceAggregator.OperationsManagerProvider.DeleteConnection(input.ModuleConnection);
            if (!result)
                return BadRequest();
            return Ok();
        }
    }
}
