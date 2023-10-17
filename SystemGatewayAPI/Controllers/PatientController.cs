using DatabaseApi.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SystemGateway.Providers;
using SystemGatewayAPI.Dtos.Entities;

namespace SystemGatewayAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly IServiceAggregator ServiceAggregator;
        public PatientController(IServiceAggregator aggregator)
        {
            this.ServiceAggregator = aggregator;
        }

        [HttpGet("{token}")]
        public async Task<IActionResult> FetchAllPatients(string Token)
        {
            var validToken = await ServiceAggregator.SecurityManagerProvider.ValidateSession(Token);
            if (!validToken) return BadRequest("Token is Not Valid");

            var allPatients = await ServiceAggregator.DatabaseProvider.FindAllPatients();
            if (allPatients == null) 
            {
                return BadRequest("Database is Not Available");
            }
            return Ok(SafePatient.FromAll(allPatients));
          
        }

        [HttpGet("{email}/{token}")]
        public async Task<IActionResult> FetchPatientData(string email, string Token)
        {
            var isTokenValid = await ServiceAggregator.SecurityManagerProvider.GetTokenData(Token);
            if (isTokenValid == null || isTokenValid.IsExpired)
            {
                return BadRequest("Session is Expired");
            }
            if (isTokenValid.UserType == SystemGateway.Dtos.Enum.UserType.Therapist)
                return BadRequest("User is not a Patient");
            if (isTokenValid.Email != email)
                return Unauthorized("You do not Have Access");


            var user = await ServiceAggregator.DatabaseProvider.FindPatientById(isTokenValid.Email);
            if (user == null) return NotFound("User not Found");
            return Ok(user);
        }
        [HttpGet("{Token}/Therapist")]
        public async Task<IActionResult> FetchPatientTherapists(string Token)
        {
            var isTokenValid = await ServiceAggregator.SecurityManagerProvider.GetTokenData(Token);
            if (isTokenValid == null || isTokenValid.IsExpired)
            {
                return BadRequest("Session is Expired");
            }
            if (isTokenValid.UserType == SystemGateway.Dtos.Enum.UserType.Therapist)
                return BadRequest("User is not a Patient");

            var patient = await ServiceAggregator.DatabaseProvider.FindPatientById(isTokenValid.Email);
            if (patient == null) return NotFound("Patient not Found");


            var acceptedTherapists = new List<SafeTherapist>();
            foreach (var email in patient.AcceptedTherapists)
            {
                var therapist = await ServiceAggregator.DatabaseProvider.FindTherapistById(email);
                if (therapist == null) continue;
                else acceptedTherapists.Add(SafeTherapist.FromTherapist(therapist));
            }
            var requestedTherapists= new List<SafeTherapist>();
            foreach (var email in patient.RequestedTherapists)
            {
                var therapist = await ServiceAggregator.DatabaseProvider.FindTherapistById(email);
                if (therapist == null) continue;
                else requestedTherapists.Add(SafeTherapist.FromTherapist(therapist));
            }


            var therapists = await ServiceAggregator.DatabaseProvider.FindAllTherapists();
            if (therapists == null) return BadRequest("Failed to Retrieve Therapists");
            var allTherapists = SafeTherapist.FromAll(therapists);
            var availableTherapists= allTherapists.FindAll(c => !patient.AcceptedTherapists.Contains(c.Email) && !patient.RequestedTherapists.Contains(c.Email) && !c.PatientRequests.Contains(patient.Email));
            var pendingTherapists = allTherapists.FindAll(c => c.PatientRequests.Contains(isTokenValid.Email));
            return Ok(new PatientTherapist
            {
                Accepted = acceptedTherapists,
                Requested = requestedTherapists,
                Available = availableTherapists,
                Pending = pendingTherapists
            });
        }



        [HttpPut("{Token}")]
        public async Task<IActionResult> UpdatePatientData(string Token, [FromBody] Patient data)
        {
            var isTokenValid = await ServiceAggregator.SecurityManagerProvider.GetTokenData(Token);
            if (isTokenValid == null || isTokenValid.IsExpired)
            {
                return BadRequest("Session is Expired");
            }
            if (isTokenValid.UserType == SystemGateway.Dtos.Enum.UserType.Therapist)
                return BadRequest("User is not a Patient");


            var user = await ServiceAggregator.OperationsManagerProvider.UpdatePatient(isTokenValid.Email, data);
            if (!user) return BadRequest("Failed to Update User");
            return Ok(user);
        }


        [HttpGet("{token}/Reject/{TherapistEmail}")]
        public async Task<IActionResult> PatientRejectTherapist(string token, string TherapistEmail)
        {
            var isTokenValid = await ServiceAggregator.SecurityManagerProvider.GetTokenData(token);
            if (isTokenValid == null || isTokenValid.IsExpired)
            {
                return BadRequest("Session is Expired"); 
            }
            if (isTokenValid.UserType == SystemGateway.Dtos.Enum.UserType.Therapist)
                return BadRequest("User is not a Patient");


            var user = await ServiceAggregator.OperationsManagerProvider.PatientRejectTherapist(isTokenValid.Email, TherapistEmail);
            if (!user) return BadRequest("Failed to Update User");
            return Ok(user);
        }

        [HttpGet("{token}/Accept/{therapistEmail}")]
        public async Task<IActionResult> PatientAcceptTherapist(string token, string therapistEmail)
        {
            var isTokenValid = await ServiceAggregator.SecurityManagerProvider.GetTokenData(token);
            if (isTokenValid == null || isTokenValid.IsExpired)
            {
                return BadRequest("Session is Expired");
            }
            if (isTokenValid.UserType == SystemGateway.Dtos.Enum.UserType.Therapist)
                return BadRequest("User is not a Patient");


            var user = await ServiceAggregator.OperationsManagerProvider.PatientRequestTherapist(isTokenValid.Email, therapistEmail);
            if (!user) return BadRequest("Failed to Update User");
            return Ok(user);
        }
    }
}
