using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using SystemGateway.Dtos.Entities;
using SystemGateway.Dtos.Input;
using SystemGateway.Helpers;
using SystemGateway.Providers;

namespace SystemGateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IServiceAggregator ServiceAggregator;
        public UserController(IServiceAggregator aggregator)
        {
            this.ServiceAggregator = aggregator;
        }

        [HttpPost]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterUserInputDto input)
        {
            var user = input.User;
            user.Password = AuthenticationHelper.HashPassword(user.Password);
            var result = await ServiceAggregator.OperationsManagerProvider.RegisterUser(user);
            if (!result)
                return BadRequest();
            return Ok();
        }
        [HttpPut("{Email}")]
        public async Task<IActionResult> UpdateUserData(string Email, [FromBody] UpdateUserInputDto input)
        {
            if (string.IsNullOrEmpty(await ServiceAggregator.SecurityManagerProvider.ValidateSession(input.Token)))
            {
                return Unauthorized();
            }
            var claims = await ServiceAggregator.SecurityManagerProvider.GetTokenData(input.Token);
            if (claims.UserId != Email) return Unauthorized();

            var updatedUser = input.User;
            var dbUser = await ServiceAggregator.DatabaseProvider.FindUserById(Email);
            if (dbUser == null)
                return NotFound();
            if (!string.IsNullOrEmpty(updatedUser.Name)) dbUser.Name = updatedUser.Name;
            if (!string.IsNullOrEmpty(updatedUser.Password)) dbUser.Password = AuthenticationHelper.HashPassword(updatedUser.Password);
            dbUser.Modules = updatedUser.Modules;
            dbUser.ActiveScenarios = updatedUser.ActiveScenarios;
            var result = await ServiceAggregator.OperationsManagerProvider.UpdateUser(Email, updatedUser);
            if (!result)
                return BadRequest();
            return Ok();
        }
        [HttpPost("{Email}")]
        public async Task<IActionResult> DeleteUserData(string Email,[FromQuery] string Token)
        {
            if (string.IsNullOrEmpty(await ServiceAggregator.SecurityManagerProvider.ValidateSession(Token)))
            {
                return Unauthorized();
            }
            var claims = await ServiceAggregator.SecurityManagerProvider.GetTokenData(Token);
            if (claims.UserId != Email) return Unauthorized();

            var dbUser = await ServiceAggregator.DatabaseProvider.FindUserById(Email);
            if (dbUser == null)
                return NotFound();
            var result = await ServiceAggregator.OperationsManagerProvider.DeleteUser(Email);
            if (!result)
                return BadRequest();
            return Ok();
        }
    }
}
