using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using SystemGateway.Dtos.Enum;
using SystemGateway.Dtos.SecurityManager;
using SystemGateway.Helpers;
using SystemGateway.Providers;
using SystemGatewayAPI.Dtos;
using SystemGatewayAPI.Dtos.Entities.Database;
using SystemGatewayAPI.Dtos.Entities.SecurityManager;
using SystemGatewayAPI.Dtos.SecurityManager;

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
        [HttpPost("Authenticate/{UserType}")]
        public async Task<IActionResult> AuthenticateAndGenerateToken(UserType userType, [FromBody] AuthenticateInputDto input)
        {
            switch (userType)
            {
                case Dtos.Enum.UserType.Patient:
                    var patient = await ServiceAggregator.DatabaseProvider.FindPatientById(input.Email);
                    if (patient == null) return BadRequest(ApplicationErrors.UserNotFound);
                    if (!AuthenticationHelper.AuthenticateUser(patient.Password, input.Password)) return BadRequest(ApplicationErrors.InvalidCredentials);
                    break;
                case Dtos.Enum.UserType.Therapist:
                    var therapist = await ServiceAggregator.DatabaseProvider.FindTherapistById(input.Email);
                    if (therapist == null) return BadRequest(ApplicationErrors.UserNotFound);
                    if (!AuthenticationHelper.AuthenticateUser(therapist.Password, input.Password)) return BadRequest(ApplicationErrors.InvalidCredentials);
                    break;
                default:
                    return BadRequest("User Type not defined");
            }
            return Ok(await ServiceAggregator.SecurityManagerProvider.GenerateSession(new SecurityDataDto
            {
                Email = input.Email,
                UserType = userType
            }));
        }

        [HttpGet("Information/{Token}")]
        public async Task<ActionResult<SessionData>> FetchUserDetails(string Token)
        {
            var validToken = await ServiceAggregator.SecurityManagerProvider.ValidateSession(Token);
            if (!validToken) return BadRequest("Token is Not Valid");

            var data = await ServiceAggregator.SecurityManagerProvider.FetchTokenData(Token);
            User user = null;

            if (data.UserType == Dtos.Enum.UserType.Therapist)
            {
                user = await ServiceAggregator.DatabaseProvider.FindTherapistById(data.Email);
            }
            else
            {
                user = await ServiceAggregator.DatabaseProvider.FindPatientById(data.Email);
            }
            if (user != null)
                return Ok(SessionData.MapFromUser(user, data));

            return NotFound("User Not Found");
        }

        [HttpDelete("{Token}/Modules")]
        public async Task<IActionResult> ClearCachedData(string Token)
        {
            var validToken = await ServiceAggregator.SecurityManagerProvider.ValidateSession(Token);
            if (!validToken) return BadRequest("Token is Not Valid");

            var data = await ServiceAggregator.SecurityManagerProvider.FetchTokenData(Token);
            foreach(ModuleSnapshot moduleSnapshot in data.ModuleSnapshots)
            {
                await ServiceAggregator.SecurityManagerProvider.DeleteModuleSnapshot(Token, moduleSnapshot.ModuleId);
            }
            return Ok();
        }





    }
}
