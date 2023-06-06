using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        //public Task<bool> UpdateUser(string Email, User updatedUser);
        //public Task<bool> DeleteUser(string Email);
        //public Task<bool> RegisterConnection(ModuleConnection updatedUser);
        //public Task<bool> DeleteConnection(Guid WebPlatformId, Guid ModuleId);

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
        public async Task<IActionResult> UpdateUserData(string Email, [FromBody] RegisterUserInputDto input)
        {
            var updatedUser = input.User;
            var dbUser = await ServiceAggregator.DatabaseProvider.FindUserById(Email);
            if (dbUser == null)
                return NotFound();
            if (string.IsNullOrEmpdbUser.Name = updatedUser.Name;
            user.Password = AuthenticationHelper.HashPassword(user.Password);
            var result = await ServiceAggregator.OperationsManagerProvider.RegisterUser(user);
            if (!result)
                return BadRequest();
            return Ok();
        }
    }
}
