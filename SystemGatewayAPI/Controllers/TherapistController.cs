using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using SystemGateway;
using SystemGateway.Helpers;
using SystemGateway.Providers;
using SystemGatewayAPI.Dtos;
using SystemGatewayAPI.Dtos.Entities;
using SystemGatewayAPI.Dtos.Entities.Database;
using SystemGatewayAPI.Dtos.Entities.Secure;
using SystemGatewayAPI.Dtos.SecurityManager;

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

        // Operations -----------------------------------------------------------------------------------------------------------

        [HttpPost("Register")] // public Task<ActionResponse> RegisterTherapist(Therapist therapist); CREATE_THERAPIST,
        public async Task<IActionResult> RegisterTherapist([FromBody] Therapist therapist)
        {
            var validate = AuthenticationHelper.ValidateUser((User)therapist);
            if (!string.IsNullOrEmpty(validate)) return BadRequest(validate);

            var therapistExists = await ServiceAggregator.DatabaseProvider.FindTherapistById(therapist.Email);
            if (therapistExists != null) return BadRequest("That Email is already In Use");

            therapist.Password = AuthenticationHelper.HashPassword(therapist.Password);

            var result = await ServiceAggregator.OperationsManagerProvider.RegisterTherapist(therapist);
            if (result.Code != System.Net.HttpStatusCode.OK) return BadRequest(result.Message);
            return Ok($"Welcome {therapist.FirstName} {therapist.LastName}");
        }
        [HttpPut("{token}")] // public Task<ActionResponse> UpdateTherapist(string Email, Therapist therapist); UPDATE_THERAPIST,
        public async Task<IActionResult> UpdateTherapistData(string Token, [FromBody] Therapist data)
        {
            var isTokenValid = await ServiceAggregator.SecurityManagerProvider.FetchTokenData(Token);
            if (isTokenValid == null || isTokenValid.IsExpired)
                return BadRequest("Session is Expired");

            if (isTokenValid.UserType == SystemGateway.Dtos.Enum.UserType.Patient)
                return BadRequest("User is not a Therapist");

            var result = await ServiceAggregator.OperationsManagerProvider.UpdateTherapist(isTokenValid.Email, data);
            if (result.Code != System.Net.HttpStatusCode.OK) return BadRequest(result.Message);
            return Ok($"Therapist Updated with Success");
        }
        [HttpDelete("{token}")] // public Task<ActionResponse> DeleteTherapist(string Email); DELETE_THERAPIST,
        public async Task<IActionResult> DeleteTherapist(string Token)
        {
            var isTokenValid = await ServiceAggregator.SecurityManagerProvider.FetchTokenData(Token);
            if (isTokenValid == null || isTokenValid.IsExpired)
                return BadRequest("Session is Expired");

            if (isTokenValid.UserType == SystemGateway.Dtos.Enum.UserType.Patient)
                return BadRequest("User is not a Therapist");

            var result = await ServiceAggregator.OperationsManagerProvider.DeleteTherapist(isTokenValid.Email);
            if (result.Code != System.Net.HttpStatusCode.OK) return BadRequest(result.Message);
            return Ok($"Therapist Removed with Success");
        }
        [HttpGet("{token}/Reject/{patientEmail}")] // public Task<ActionResponse> TherapistRejectPatient(string Email, string PatientEmail); THERAPIST_REJECT_PATIENT,
        public async Task<IActionResult> TherapistRejectPatient(string token, string patientEmail)
        {
            var isTokenValid = await ServiceAggregator.SecurityManagerProvider.FetchTokenData(token);
            if (isTokenValid == null || isTokenValid.IsExpired)
                return BadRequest("Session is Expired");

            if (isTokenValid.UserType == SystemGateway.Dtos.Enum.UserType.Patient)
                return BadRequest("User is not a Therapist");


            var result = await ServiceAggregator.OperationsManagerProvider.TherapistRejectPatient(isTokenValid.Email, patientEmail);
            if (result.Code != System.Net.HttpStatusCode.OK) return BadRequest(result.Message);
            return Ok($"Patient Rejected with Success");
        }
        [HttpGet("{token}/Accept/{patientEmail}")] // public Task<ActionResponse> TherapistAcceptPatient(string Email, string PatientEmail); THERAPIST_ACCEPT_PATIENT,
        public async Task<IActionResult> TherapistAcceptPatient(string token, string patientEmail)
        {
            var isTokenValid = await ServiceAggregator.SecurityManagerProvider.FetchTokenData(token);
            if (isTokenValid == null || isTokenValid.IsExpired)
                return BadRequest("Session is Expired");

            if (isTokenValid.UserType == SystemGateway.Dtos.Enum.UserType.Patient)
                return BadRequest("User is not a Therapist");

            var result = await ServiceAggregator.OperationsManagerProvider.TherapistAcceptPatient(isTokenValid.Email, patientEmail);
            if (result.Code != System.Net.HttpStatusCode.OK) return BadRequest(result.Message);
            return Ok($"Patient Accepted with Success");
        }


        // Database Fetch Operations --------------------------------------------------------------------------------------------


        [HttpGet("{token}")] // public Task<Therapist?> FindTherapistById(string email);
        public async Task<IActionResult> FetchTherapistData(string Token)
        {
            var isTokenValid = await ServiceAggregator.SecurityManagerProvider.FetchTokenData(Token);
            if (isTokenValid == null || isTokenValid.IsExpired) 
                return BadRequest("Session is Expired");

            if (isTokenValid.UserType == SystemGateway.Dtos.Enum.UserType.Patient)
                return BadRequest("User is not a Therapist");


            var user = await ServiceAggregator.DatabaseProvider.FindTherapistById(isTokenValid.Email);
            if (user == null) return NotFound("Therapist not Found");
            return Ok(user);
        }

        [HttpGet] // public Task<ICollection<SafeTherapist>> FindAllTherapists();
        public async Task<IActionResult> FetchAllTherapists()
        {
            var user = await ServiceAggregator.DatabaseProvider.FindAllTherapists();
            if (user == null) return NotFound(ApplicationErrors.FailedToCallDatabase);
            return Ok(user);
        }


        // Other Operations -----------------------------------------------------------------------------------------------------


        [HttpGet("{token}/Patients")]
        public async Task<IActionResult> FetchPatients(string Token)
        {
            var isTokenValid = await ServiceAggregator.SecurityManagerProvider.FetchTokenData(Token);
            if (isTokenValid == null || isTokenValid.IsExpired)
                return BadRequest("Session is Expired");

            if (isTokenValid.UserType == SystemGateway.Dtos.Enum.UserType.Patient)
                return BadRequest("User is not a Therapist");

            var therapist = await ServiceAggregator.DatabaseProvider.FindTherapistById(isTokenValid.Email);
            if (therapist == null) return NotFound("therapist not Found");


            var acceptedPatients = new List<SafePatient>();
            foreach (var email in therapist.AcceptedPatients)
            {
                var patient = await ServiceAggregator.DatabaseProvider.FindPatientById(email);
                if (patient == null) continue;
                else acceptedPatients.Add(SafePatient.FromPatient(patient));
            }

            var requestedPatients = new List<SafePatient>();
            foreach (var email in therapist.RequestedPatients)
            {
                var patient = await ServiceAggregator.DatabaseProvider.FindPatientById(email);
                if (patient == null) continue;
                else requestedPatients.Add(SafePatient.FromPatient(patient));
            }

            var allPatients = await ServiceAggregator.DatabaseProvider.FindAllPatients();
            if (allPatients == null) return BadRequest("Failed to Retrieve Patients");

            var availablePatients = allPatients.ToList().FindAll(c => !therapist.AcceptedPatients.Contains(c.Email) && !therapist.RequestedPatients.Contains(c.Email) && c.AcceptedTherapists.Count() == 0 && c.RequestedTherapists.Count()==0);
            var pendingPatients = allPatients.ToList().FindAll(c => c.RequestedTherapists.Contains(isTokenValid.Email));
            return Ok(new Associations<SafePatient>
            {
                Accepted = acceptedPatients,
                Requested = requestedPatients,
                Available = availablePatients,
                Pending = pendingPatients
            });
        }
        [HttpGet("{token}/Patients/All")]
        public async Task<IActionResult> FetchAllTherapistPatients(string Token)
        {
            var isTokenValid = await ServiceAggregator.SecurityManagerProvider.FetchTokenData(Token);
            if (isTokenValid == null || isTokenValid.IsExpired)
                return BadRequest("Session is Expired");

            if (isTokenValid.UserType == SystemGateway.Dtos.Enum.UserType.Patient)
                return BadRequest("User is not a Therapist");

            var therapist = await ServiceAggregator.DatabaseProvider.FindTherapistById(isTokenValid.Email);
            if (therapist == null) return NotFound("therapist not Found");


            var allPatients = new List<SafePatient>();
            foreach (var email in therapist.AcceptedPatients)
            {
                var patient = await ServiceAggregator.DatabaseProvider.FindPatientById(email);
                if (patient == null) continue;
                else allPatients.Add(SafePatient.FromPatient(patient));
            }

            return Ok(allPatients);
        }
        [HttpGet("{token}/Patients/{patientEmail}")]
        public async Task<IActionResult> FetchPatientData(string Token, string patientEmail)
        {
            var isTokenValid = await ServiceAggregator.SecurityManagerProvider.FetchTokenData(Token);
            if (isTokenValid == null || isTokenValid.IsExpired)
                return BadRequest("Session is Expired");

            if (isTokenValid.UserType == SystemGateway.Dtos.Enum.UserType.Patient)
                return BadRequest("User is not a Therapist");

            var therapist = await ServiceAggregator.DatabaseProvider.FindTherapistById(isTokenValid.Email);
            if (therapist == null) return NotFound("therapist not Found");
            if (!therapist.AcceptedPatients.Contains(patientEmail)) return Unauthorized("Access to Patient Information Not Authorized");


            var patient = await ServiceAggregator.DatabaseProvider.FindPatientById(patientEmail);
            if (patient == null) {
                await ServiceAggregator.OperationsManagerProvider.TherapistRejectPatient(isTokenValid.Email, patientEmail);
                return NotFound("Patient not Found");
            }
            return Ok(SafePatient.FromPatient(patient));
        }
        [HttpGet("{token}/Patients/{patientEmail}/Modules")]
        public async Task<IActionResult> FetchPatientsModules(string Token, string patientEmail)
        {
            var isTokenValid = await ServiceAggregator.SecurityManagerProvider.FetchTokenData(Token);
            if (isTokenValid == null || isTokenValid.IsExpired)
                return BadRequest("Session is Expired");

            if (isTokenValid.UserType == SystemGateway.Dtos.Enum.UserType.Patient)
                return BadRequest("User is not a Therapist");

            var therapist = await ServiceAggregator.DatabaseProvider.FindTherapistById(isTokenValid.Email);
            if (therapist == null) return NotFound("therapist not Found");
            if (!therapist.AcceptedPatients.Contains(patientEmail)) return Unauthorized("Access to Patient Information Not Authorized");


            var patient = await ServiceAggregator.DatabaseProvider.FindPatientById(patientEmail);
            if (patient == null || patient.Modules == null)
            {
                await ServiceAggregator.OperationsManagerProvider.TherapistRejectPatient(isTokenValid.Email, patientEmail);
                return NotFound("Patient not Found");
            }
            var sessionDataModules = isTokenValid.ModuleSnapshots;
            foreach (var module in patient.Modules)
            {
                var moduleSessionData = sessionDataModules.FirstOrDefault(c => c.ModuleId == module.Id);
                if (moduleSessionData == null)  // If Module Not Registered in Session
                {
                    await ServiceAggregator.SecurityManagerProvider.AddModuleSnapshot(Token, ModuleSnapshot.FromModule(module));
                }
                else                            // If Module is Registered in Session
                {
                    await ServiceAggregator.SecurityManagerProvider.UpdateModuleSnapshot(Token, (Guid)module.Id, ModuleSnapshot.FromModule(module));
                }
            }
            return Ok(patient.Modules);
        }
        [HttpGet("{token}/Patients/{patientEmail}/Modules/{ModuleId}")]
        public async Task<IActionResult> FetchPatientsModuleById(string Token, string patientEmail, Guid ModuleId)
        {
            var isTokenValid = await ServiceAggregator.SecurityManagerProvider.FetchTokenData(Token);
            if (isTokenValid == null || isTokenValid.IsExpired)
                return BadRequest("Session is Expired");

            if (isTokenValid.UserType == SystemGateway.Dtos.Enum.UserType.Patient)
                return BadRequest("User is not a Therapist");

            var therapist = await ServiceAggregator.DatabaseProvider.FindTherapistById(isTokenValid.Email);
            if (therapist == null) return NotFound("therapist not Found");
            if (!therapist.AcceptedPatients.Contains(patientEmail)) return Unauthorized("Access to Patient Information Not Authorized");


            var patient = await ServiceAggregator.DatabaseProvider.FindPatientById(patientEmail);
            if (patient == null || patient.Modules == null)
            {
                await ServiceAggregator.OperationsManagerProvider.TherapistRejectPatient(isTokenValid.Email, patientEmail);
                return NotFound("Patient not Found");
            }
            var module = patient.Modules.FirstOrDefault(c => c.Id == ModuleId);
            if (module == null)
                return NotFound("Module Not Found");

            await ServiceAggregator.SecurityManagerProvider.UpdateModuleSnapshot(Token, (Guid)module.Id, ModuleSnapshot.FromModule(module));
            return Ok(module);
        }
        [HttpPut("{Token}/Patients/{PatientEmail}/Modules/{ModuleId}")] // Update Patient Module
        public async Task<IActionResult> UpdatePatientModule(string Token, string PatientEmail, Guid ModuleId, [FromBody] Module module)
        {
            var isTokenValid = await ServiceAggregator.SecurityManagerProvider.FetchTokenData(Token);
            if (isTokenValid == null || isTokenValid.IsExpired)
                return BadRequest("Session is Expired");

            if (isTokenValid.UserType == SystemGateway.Dtos.Enum.UserType.Patient)
                return BadRequest("User is not a Therapist");

            var therapist = await ServiceAggregator.DatabaseProvider.FindTherapistById(isTokenValid.Email);
            if (therapist == null) return NotFound("therapist not Found");
            if (!therapist.AcceptedPatients.Contains(PatientEmail)) return Unauthorized("Access to Patient Information Not Authorized");

            module.Id = ModuleId;
            var actionResponse = await ServiceAggregator.OperationsManagerProvider.UpdatePatientModule(PatientEmail, ModuleId.ToString(), module);
            if (actionResponse.Code != HttpStatusCode.OK)
                return BadRequest(actionResponse.Message);
            return Ok("Module Updated with Success");
        }
        [HttpPut("{Token}/Patients/{PatientEmail}/Modules/{ModuleId}/Version/{VersionId}")] 
        public async Task<IActionResult> UpdatePatientModuleToVersion(string Token,string PatientEmail, Guid ModuleId, string VersionId)
        {
            var isTokenValid = await ServiceAggregator.SecurityManagerProvider.FetchTokenData(Token);
            if (isTokenValid == null || isTokenValid.IsExpired)
                return BadRequest("Session is Expired");

            if (isTokenValid.UserType == SystemGateway.Dtos.Enum.UserType.Patient)
                return BadRequest("User is not a Therapist");

            var therapist = await ServiceAggregator.DatabaseProvider.FindTherapistById(isTokenValid.Email);
            if (therapist == null) return NotFound("therapist not Found");
            if (!therapist.AcceptedPatients.Contains(PatientEmail)) return Unauthorized("Access to Patient Information Not Authorized");

            var actionResponse = await ServiceAggregator.OperationsManagerProvider.UpdatePatientModuleToVersion(PatientEmail, ModuleId.ToString(), VersionId);
            if (actionResponse.Code != HttpStatusCode.OK)
                return BadRequest(actionResponse.Message);
            return Ok("Module Updated with Success");
        }
        [HttpGet("{Token}/Patients/{PatientEmail}/Modules/{ModuleId}/Status")]
        public async Task<IActionResult> ModuleStatusCheck(string Token, string PatientEmail, Guid ModuleId)
        {
            var isTokenValid = await ServiceAggregator.SecurityManagerProvider.FetchTokenData(Token);
            if (isTokenValid == null || isTokenValid.IsExpired)
                return BadRequest("Session is Expired");

            if (isTokenValid.UserType == SystemGateway.Dtos.Enum.UserType.Patient)
                return BadRequest("User is not a Therapist");

            var therapist = await ServiceAggregator.DatabaseProvider.FindTherapistById(isTokenValid.Email);
            if (therapist == null) return NotFound("therapist not Found");
            if (!therapist.AcceptedPatients.Contains(PatientEmail)) return Unauthorized("Access to Patient Information Not Authorized");

            var data = isTokenValid.ModuleSnapshots.FirstOrDefault(c => c.ModuleId == ModuleId);
            if (data == null) return NotFound("Module Not Found");

            var patient = await ServiceAggregator.DatabaseProvider.FindPatientById(PatientEmail);
            if (patient == null) return NotFound("Patient Not Found");

            var module = patient.Modules.FirstOrDefault(c => c.Id == ModuleId);
            if (module == null) return NotFound("Module Not Found");

            if (module.ModuleData.Checksum != data.Checksum && module.ModuleData.Timestamp > data.Timestamp) return Ok(new ModuleStatusCheck { HasUpdates = true });
            return Ok(new ModuleStatusCheck { HasUpdates = false });
        }


    }
}
