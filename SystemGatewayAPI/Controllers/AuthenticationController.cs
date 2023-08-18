using DatabaseApi.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using SystemGateway.Dtos.Input;
using SystemGateway.Dtos.SecurityManager;
using SystemGateway.Helpers;
using SystemGateway.Providers;
using SystemGatewayAPI.Dtos;

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
        [HttpPost("AuthenticateUser")]
        public async Task<IActionResult> AuthenticateUser([FromBody] AuthenticateUserInputDto input)
        {
            switch (input.UserType)
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
                    return BadRequest();
            }
          
            return Ok("User Authenticated");
        }
        [HttpPost("RegisterPatient")]
        public async Task<IActionResult> RegisterPatient([FromBody] Patient patient)
        {
            if (string.IsNullOrEmpty(patient.Email)) return BadRequest("Email is Required");
            if (string.IsNullOrEmpty(patient.Password)) return BadRequest("Password is Required");
            if (string.IsNullOrEmpty(patient.FirstName)) return BadRequest("First Name is Required");
            if (string.IsNullOrEmpty(patient.LastName)) return BadRequest("Last Name is Required");
            var patientExists = await ServiceAggregator.DatabaseProvider.FindPatientById(patient.Email);
            if (patientExists != null) return BadRequest("That Email is already In Use");

            patient.Password = AuthenticationHelper.HashPassword(patient.Password);
            patient.WebPlatform = null;
            
            var registerPatient = await ServiceAggregator.OperationsManagerProvider.RegisterPatient(patient);
            if (!registerPatient) return BadRequest("Failed to register user");
            return Ok($"Welcome {patient.FirstName} {patient.LastName}");
        }
        [HttpPost("RegisterTherapist")]
        public async Task<IActionResult> RegisterTherapist([FromBody] Therapist therapist)
        {
            if (string.IsNullOrEmpty(therapist.Email)) return BadRequest("Email is Required");
            if (string.IsNullOrEmpty(therapist.Password)) return BadRequest("Password is Required");
            if (string.IsNullOrEmpty(therapist.FirstName)) return BadRequest("First Name is Required");
            if (string.IsNullOrEmpty(therapist.LastName)) return BadRequest("Last Name is Required");
            var patientExists = await ServiceAggregator.DatabaseProvider.FindTherapistById(therapist.Email);
            if (patientExists != null) return BadRequest("That Email is already In Use");

            therapist.Password = AuthenticationHelper.HashPassword(therapist.Password);

            var registerPatient = await ServiceAggregator.OperationsManagerProvider.RegisterTherapist(therapist);
            if (!registerPatient) return BadRequest("Failed to register user");
            return Ok($"Welcome {therapist.FirstName} {therapist.LastName}");
        }

        [HttpGet("Information/{UserEmail}")]
        public async Task<ActionResult<UserDetailsDto>> GetUserDetails(string UserEmail)
        {
            if (string.IsNullOrEmpty(UserEmail)) return BadRequest("Email is Required");
            var patient = await ServiceAggregator.DatabaseProvider.FindPatientById(UserEmail);
            if (patient != null)
            {
                return Ok(UserDetailsDto.FromPatient(patient));
            }
            var therapist = await ServiceAggregator.DatabaseProvider.FindTherapistById(UserEmail);
            if (therapist != null)
            {
                return Ok(UserDetailsDto.FromTherapist(therapist));
            }

            return NotFound("User Not Found");
        }



    }
}
