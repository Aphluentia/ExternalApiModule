using DatabaseApi.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SystemGateway.Providers;
using SystemGatewayAPI.Dtos.Entities;

namespace SystemGatewayAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TherapistController : ControllerBase
    {
        private readonly IServiceAggregator ServiceAggregator;
        public TherapistController(IServiceAggregator aggregator)
        {
            this.ServiceAggregator = aggregator;
        }

        [HttpGet("{email}/{token}")]
        public async Task<IActionResult> FetchTherapistData(string Token)
        {
            var isTokenValid = await ServiceAggregator.SecurityManagerProvider.GetTokenData(Token);
            if (isTokenValid == null || isTokenValid.IsExpired) 
            {
                return BadRequest("Session is Expired");
            }
            if (isTokenValid.UserType == SystemGateway.Dtos.Enum.UserType.Patient)
                return BadRequest("User is not a Therapist");


            var user = await ServiceAggregator.DatabaseProvider.FindTherapistById(isTokenValid.Email);
            if (user == null) return NotFound("User not Found");
            return Ok(user);
        }
        [HttpGet("{token}/Patients")]
        public async Task<IActionResult> FetchPatients(string Token)
        {
            var isTokenValid = await ServiceAggregator.SecurityManagerProvider.GetTokenData(Token);
            if (isTokenValid == null || isTokenValid.IsExpired)
            {
                return BadRequest("Session is Expired");
            }
            if (isTokenValid.UserType == SystemGateway.Dtos.Enum.UserType.Patient)
                return BadRequest("User is not a Therapist");

            var therapist = await ServiceAggregator.DatabaseProvider.FindTherapistById(isTokenValid.Email);
            if (therapist == null) return NotFound("therapist not Found");


            var acceptedPatients = new List<SafePatient>();
            foreach (var email in therapist.PatientsAccepted)
            {
                var patient = await ServiceAggregator.DatabaseProvider.FindPatientById(email);
                if (patient == null) continue;
                else acceptedPatients.Add(SafePatient.FromPatient(patient));
            }
            var requestedPatients = new List<SafePatient>();
            foreach (var email in therapist.PatientRequests)
            {
                var patient = await ServiceAggregator.DatabaseProvider.FindPatientById(email);
                if (patient == null) continue;
                else requestedPatients.Add(SafePatient.FromPatient(patient));
            }


            var patients = await ServiceAggregator.DatabaseProvider.FindAllPatients();
            if (patients == null) return BadRequest("Failed to Retrieve Patients");
            var allPatients = SafePatient.FromAll(patients);
            var availablePatients = allPatients.FindAll(c => !therapist.PatientsAccepted.Contains(c.Email) && !therapist.PatientRequests.Contains(c.Email) && c.AcceptedTherapists.Count() == 0 && c.RequestedTherapists.Count()==0);
            var pendingPatients = allPatients.FindAll(c => c.RequestedTherapists.Contains(isTokenValid.Email));
            return Ok(new TherapistPatients
            {
                Accepted = acceptedPatients,
                Requested = requestedPatients,
                Available = availablePatients,
                Pending = pendingPatients
            });
        }

    




        [HttpPut("{token}")]
        public async Task<IActionResult> UpdateTherapistData(string Token, [FromBody]Therapist data)
        {
            var isTokenValid = await ServiceAggregator.SecurityManagerProvider.GetTokenData(Token);
            if (isTokenValid == null || isTokenValid.IsExpired)
            {
                return BadRequest("Session is Expired");
            }
            if (isTokenValid.UserType == SystemGateway.Dtos.Enum.UserType.Patient)
                return BadRequest("User is not a Therapist");


            var user = await ServiceAggregator.OperationsManagerProvider.UpdateTherapist(isTokenValid.Email, data);
            if (!user) return BadRequest("Failed to Update User");
            return Ok(user);
        }


        [HttpGet("{token}/Reject/{patientEmail}")]
        public async Task<IActionResult> TherapistRejectPatient(string token, string patientEmail)
        {
            var isTokenValid = await ServiceAggregator.SecurityManagerProvider.GetTokenData(token);
            if (isTokenValid == null || isTokenValid.IsExpired)
            {
                return BadRequest("Session is Expired");
            }
            if (isTokenValid.UserType == SystemGateway.Dtos.Enum.UserType.Patient)
                return BadRequest("User is not a Therapist");


            var user = await ServiceAggregator.OperationsManagerProvider.TherapistRejectPatient(isTokenValid.Email, patientEmail);
            if (!user) return BadRequest("Failed to Update User");
            return Ok(user);
        }

        [HttpGet("{token}/Accept/{patientEmail}")]
        public async Task<IActionResult> TherapistAcceptPatient(string token, string patientEmail)
        {
            var isTokenValid = await ServiceAggregator.SecurityManagerProvider.GetTokenData(token);
            if (isTokenValid == null || isTokenValid.IsExpired)
            {
                return BadRequest("Session is Expired");
            }
            if (isTokenValid.UserType == SystemGateway.Dtos.Enum.UserType.Patient)
                return BadRequest("User is not a Therapist");


            var user = await ServiceAggregator.OperationsManagerProvider.TherapistAcceptPatient(isTokenValid.Email, patientEmail);
            if (!user) return BadRequest("Failed to Update User");
            return Ok(user);
        }

    }
}
