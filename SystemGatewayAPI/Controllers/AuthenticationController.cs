using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using SystemGateway.Configurations;
using SystemGateway.Dtos;
using SystemGateway.Dtos.Input;
using SystemGateway.Dtos.SecurityManager;
using SystemGateway.Helpers;
using SystemGateway.Providers;

namespace SystemGateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IServiceAggregator ServiceAggregator;
        public AuthenticationController(IServiceAggregator aggregator) 
        {
            this.ServiceAggregator = aggregator;
        }
        [HttpPost]
        public async Task<IActionResult> AuthenticateUser([FromBody] AuthenticateUserInputDto input)
        {
            var user = await ServiceAggregator.DatabaseProvider.FindUserById(input.Email);
            
            if (AuthenticationHelper.AuthenticateUser(UserPassword: user.Password, ProvidedPassword: input.Password))
                return Unauthorized();
            var token = await ServiceAggregator.SecurityManagerProvider.GenerateSession(new Dtos.SecurityManager.SecurityDataDto
            {
                UserId = user.Email,
                UserName = user.Name,
                PermissionLevel = user.PermissionLevel,
                WebPlatformId = user.WebPlatformId
            });
            if (string.IsNullOrEmpty(token))
                return BadRequest();
            return Ok(token);
        }
        [HttpPost]
        public async Task<IActionResult> KeepAlive([FromBody] TokenDto input)
        {
            if (string.IsNullOrEmpty(await ServiceAggregator.SecurityManagerProvider.ValidateSession(input.Token)))
            {
                return Unauthorized();
            }
            var token = await ServiceAggregator.SecurityManagerProvider.KeepAlive(input.Token);
            if (string.IsNullOrEmpty(token))
                return BadRequest();
            return Ok(token);
        }


    }
}
