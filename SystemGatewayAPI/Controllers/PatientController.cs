using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Net;
using SystemGateway.Dtos.Enum;
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
    public class PatientController : ControllerBase
    {
        private readonly IServiceAggregator ServiceAggregator;
        public PatientController(IServiceAggregator aggregator)
        {
            this.ServiceAggregator = aggregator;
        }


        [HttpPost("Register")] // public Task<ActionResponse> RegisterPatient(Patient patient); CREATE_PATIENT
        public async Task<IActionResult> RegisterPatient([FromBody] Patient patient)
        {
            var validate = AuthenticationHelper.ValidateUser((User)patient);
            if (!string.IsNullOrEmpty(validate)) return BadRequest(patient);

            var therapistExists = await ServiceAggregator.DatabaseProvider.FindPatientById(patient.Email);
            if (therapistExists != null) return BadRequest("That Email is already In Use");
            patient.Password = AuthenticationHelper.HashPassword(patient.Password);
            patient.AcceptedTherapists = new HashSet<string>();
            patient.RequestedTherapists = new HashSet<string>();
            patient.Modules = new List<Module>();

            var result = await ServiceAggregator.OperationsManagerProvider.RegisterPatient(patient);
            if (result.Code != System.Net.HttpStatusCode.OK) 
                return BadRequest(result.Message);
            return Ok($"Patient {patient.FirstName} {patient.LastName} Registered with Success");
        }

        [HttpDelete("{token}")] // public Task<ActionResponse> DeletePatient(string Email); DELETE_PATIENT,
        public async Task<IActionResult> RemovePatient(string Token)
        {
            var isTokenValid = await ServiceAggregator.SecurityManagerProvider.FetchTokenData(Token);
            if (isTokenValid == null || isTokenValid.IsExpired)
                return BadRequest("Session is Expired");

            if (isTokenValid.UserType == SystemGateway.Dtos.Enum.UserType.Therapist)
                return Unauthorized("User is not a Patient");

            var result = await ServiceAggregator.OperationsManagerProvider.DeletePatient(isTokenValid.Email);
            if (result.Code != System.Net.HttpStatusCode.OK) 
                return BadRequest(result.Message);
            return Ok("Patient Removed");
        }

        [HttpPut("{Token}")] // public Task<ActionResponse> UpdatePatient(string Email, Patient patient); UPDATE_PATIENT,
        public async Task<IActionResult> UpdatePatient(string Token, [FromBody] Patient data)
        {
            var isTokenValid = await ServiceAggregator.SecurityManagerProvider.FetchTokenData(Token);
            if (isTokenValid == null || isTokenValid.IsExpired)
                return BadRequest("Session is Expired");
            if (isTokenValid.UserType == SystemGateway.Dtos.Enum.UserType.Therapist)
                return BadRequest("User is not a Patient");

            var result = await ServiceAggregator.OperationsManagerProvider.UpdatePatient(isTokenValid.Email, data);
            if (result.Code != System.Net.HttpStatusCode.OK) return BadRequest(result.Message);
            return Ok("Patient Updated");
        }

        [HttpPost("{Token}/Modules/{ModuleId}")] // public Task<ActionResponse> AddExistingModuleToPatient(string Email, string ModuleId); PATIENT_ADD_EXISTING_MODULE,
        public async Task<IActionResult> PatientAddExistingModule(string Token, Guid ModuleId)
        {
            var isTokenValid = await ServiceAggregator.SecurityManagerProvider.FetchTokenData(Token);

            if (isTokenValid == null || isTokenValid.IsExpired)
                return BadRequest("Session is Expired");

            if (isTokenValid.UserType == SystemGateway.Dtos.Enum.UserType.Therapist)
                return BadRequest("User is not a Patient");

            var actionResponse = await ServiceAggregator.OperationsManagerProvider.AddExistingModuleToPatient(isTokenValid.Email, ModuleId.ToString());
            if (actionResponse.Code != HttpStatusCode.OK)
                return BadRequest(actionResponse.Message);
            return Ok("Module Paired with Success");
        }
        
        [HttpPost("{Token}/Modules")] // public Task<ActionResponse> AddNewModuleToPatient(string Email, Module Module); PATIENT_NEW_MODULE
        public async Task<IActionResult> PatientAddNewModule(string Token, [FromBody] Module Module)
        {
            var isTokenValid = await ServiceAggregator.SecurityManagerProvider.FetchTokenData(Token);
            Module.Id = Guid.NewGuid();

            if (isTokenValid == null || isTokenValid.IsExpired)
                return BadRequest("Session is Expired");

            if (isTokenValid.UserType == SystemGateway.Dtos.Enum.UserType.Therapist)
                return BadRequest("User is not a Patient");

            var actionResponse = await ServiceAggregator.OperationsManagerProvider.AddNewModuleToPatient(isTokenValid.Email, Module);
            if (actionResponse.Code != HttpStatusCode.OK)
                return BadRequest(actionResponse.Message);
            return Ok(actionResponse.Message);
        }

        [HttpPut("{Token}/Modules/{ModuleId}")] // public Task<ActionResponse> UpdatePatientModule(string Email, string ModuleId, Module Module); UPDATE_PATIENT_MODULE
        public async Task<IActionResult> UpdatePatientModule(string Token, Guid ModuleId, [FromBody] Module module)
        {
            var isTokenValid = await ServiceAggregator.SecurityManagerProvider.FetchTokenData(Token);
            if (isTokenValid == null || isTokenValid.IsExpired)
                return BadRequest("Session is Expired");

            if (isTokenValid.UserType == SystemGateway.Dtos.Enum.UserType.Therapist)
                return BadRequest("User is not a Patient");

            module.Id = ModuleId;
            module.ModuleData.Timestamp = DateTime.UtcNow;
            var actionResponse = await ServiceAggregator.OperationsManagerProvider.UpdatePatientModule(isTokenValid.Email, ModuleId.ToString(), module);
            if (actionResponse.Code != HttpStatusCode.OK)
                return BadRequest(actionResponse.Message);
            return Ok("Module Paired with Success");
        }

        [HttpPut("{Token}/Modules/{ModuleId}/Version/{VersionId}")] // public Task<ActionResponse> UpdatePatientModuleToVersion(string Email, string ModuleId, string VersionId); UPDATE_PATIENT_MODULE_VERSION
        public async Task<IActionResult> UpdatePatientModuleToVersion(string Token, Guid ModuleId, string VersionId)
        {
            var isTokenValid = await ServiceAggregator.SecurityManagerProvider.FetchTokenData(Token);
            if (isTokenValid == null || isTokenValid.IsExpired)
                return BadRequest("Session is Expired");

            if (isTokenValid.UserType == SystemGateway.Dtos.Enum.UserType.Therapist)
                return BadRequest("User is not a Patient");

            var actionResponse = await ServiceAggregator.OperationsManagerProvider.UpdatePatientModuleToVersion(isTokenValid.Email, ModuleId.ToString(), VersionId);
            if (actionResponse.Code != HttpStatusCode.OK)
                return BadRequest(actionResponse.Message);
            return Ok($"Module Updated to Version {VersionId} with Success");
        }
        
        [HttpDelete("{Token}/Modules/{ModuleId}")] // public Task<ActionResponse> DeletePatientModule(string Email, string ModuleId); DELETE_PATIENT_MODULE
        public async Task<IActionResult> DeletePatientModule(string Token, Guid ModuleId)
        {
            var isTokenValid = await ServiceAggregator.SecurityManagerProvider.FetchTokenData(Token);
            if (isTokenValid == null || isTokenValid.IsExpired)
                return BadRequest("Session is Expired");

            if (isTokenValid.UserType == SystemGateway.Dtos.Enum.UserType.Therapist)
                return BadRequest("User is not a Patient");


            var actionResponse = await ServiceAggregator.OperationsManagerProvider.DeletePatientModule(isTokenValid.Email, ModuleId.ToString());
            if (actionResponse.Code != HttpStatusCode.OK)
                return BadRequest(actionResponse.Message);
            await ServiceAggregator.SecurityManagerProvider.DeleteModuleSnapshot(Token, ModuleId);
            return Ok($"Module Removed From Patient");
        }
        
        [HttpGet("{token}/Reject/{TherapistEmail}")] // public Task<ActionResponse> PatientRejectTherapist(string Email, string TherapistEmail); PATIENT_REJECT_THERAPIST
        public async Task<IActionResult> PatientRejectTherapist(string token, string TherapistEmail)
        {
            var isTokenValid = await ServiceAggregator.SecurityManagerProvider.FetchTokenData(token);
            if (isTokenValid == null || isTokenValid.IsExpired)
                return BadRequest("Session is Expired");

            if (isTokenValid.UserType == SystemGateway.Dtos.Enum.UserType.Therapist)
                return BadRequest("User is not a Patient");

            var result = await ServiceAggregator.OperationsManagerProvider.PatientRejectTherapist(isTokenValid.Email, TherapistEmail);
            if (result.Code != System.Net.HttpStatusCode.OK) return BadRequest(result.Message);
            return Ok("Therapist Rejected");
        }

        [HttpGet("{token}/Accept/{therapistEmail}")] // public Task<ActionResponse> PatientRequestTherapist(string Email, string TherapistEmail); PATIENT_REQUEST_THERAPIST
        public async Task<IActionResult> PatientAcceptTherapist(string token, string therapistEmail)
        {
            var isTokenValid = await ServiceAggregator.SecurityManagerProvider.FetchTokenData(token);
            if (isTokenValid == null || isTokenValid.IsExpired)
            {
                return BadRequest("Session is Expired");
            }
            if (isTokenValid.UserType == SystemGateway.Dtos.Enum.UserType.Therapist)
                return BadRequest("User is not a Patient");


            var result = await ServiceAggregator.OperationsManagerProvider.PatientRequestTherapist(isTokenValid.Email, therapistEmail);
            if (result.Code != System.Net.HttpStatusCode.OK) return BadRequest(result.Message);
            return Ok("Therapist Accepted");
        }

        [HttpPost("{token}/Modules/{ModuleId}/Profile/{ProfileName}")] 
        public async Task<IActionResult> AddNewProfile(string token, Guid ModuleId, string ProfileName)
        {

            var isTokenValid = await ServiceAggregator.SecurityManagerProvider.FetchTokenData(token);
            if (isTokenValid == null || isTokenValid.IsExpired)
            {
                return BadRequest("Session is Expired");
            }
            if (isTokenValid.UserType == SystemGateway.Dtos.Enum.UserType.Therapist)
                return BadRequest("User is not a Patient");


            var result = await ServiceAggregator.OperationsManagerProvider.PatientAddNewModuleContext(isTokenValid.Email, ModuleId.ToString(), ProfileName);
            if (result.Code != System.Net.HttpStatusCode.OK) return BadRequest(result.Message);
            return Ok("Created new Profile");
        }

        [HttpDelete("{token}/Modules/{ModuleId}/Profile/{ProfileName}")] // public Task<ActionResponse> UpdateModule(string ModuleId, Module module); //UPDATE_MODULE,
        public async Task<IActionResult> DeleteProfile(string token, Guid ModuleId, string ProfileName)
        {
            var isTokenValid = await ServiceAggregator.SecurityManagerProvider.FetchTokenData(token);
            if (isTokenValid == null || isTokenValid.IsExpired)
            {
                return BadRequest("Session is Expired");
            }
            if (isTokenValid.UserType == SystemGateway.Dtos.Enum.UserType.Therapist)
                return BadRequest("User is not a Patient");


            var result = await ServiceAggregator.OperationsManagerProvider.PatientDeleteModuleContext(isTokenValid.Email, ModuleId.ToString(), ProfileName);
            if (result.Code != System.Net.HttpStatusCode.OK) return BadRequest(result.Message);
            return Ok("Removed Profile");
        }
        // Database Fetch Operations --------------------------------------------------------------------------------------------

        [HttpGet("{token}")] // public Task<Patient?> FindPatientById(string email);
        public async Task<IActionResult> FetchPatientData(string Token) 
        {
            var isTokenValid = await ServiceAggregator.SecurityManagerProvider.FetchTokenData(Token);
            if (isTokenValid == null || isTokenValid.IsExpired)
                return BadRequest("Session is Expired");
            if (isTokenValid.UserType == SystemGateway.Dtos.Enum.UserType.Therapist)
                return BadRequest("User is not a Patient");

            var user = await ServiceAggregator.DatabaseProvider.FindPatientById(isTokenValid.Email);
            if (user == null) return NotFound("User not Found");
            var modules = await ServiceAggregator.DatabaseProvider.FindAllPatientModules(isTokenValid.Email);
            if (modules == null) return NotFound("User not Found");
            var sessionDataModules = isTokenValid.ModuleSnapshots;
            foreach (var module in modules)
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

            return Ok(user);
        }

        [HttpGet("{token}/Modules")] 
        public async Task<IActionResult> FetchPatientModules(string Token) 
        {
            var isTokenValid = await ServiceAggregator.SecurityManagerProvider.FetchTokenData(Token);
            if (isTokenValid == null || isTokenValid.IsExpired)
                return BadRequest("Session is Expired");
            if (isTokenValid.UserType == SystemGateway.Dtos.Enum.UserType.Therapist)
                return BadRequest("User is not a Patient");

            var modules = await ServiceAggregator.DatabaseProvider.FindAllPatientModules(isTokenValid.Email);
            if (modules == null) return NotFound("User not Found");
            var sessionDataModules = isTokenValid.ModuleSnapshots;
            foreach (var module in modules)
            {
                var moduleSessionData = sessionDataModules.FirstOrDefault(c=>c.ModuleId == module.Id);
                if (moduleSessionData == null)  // If Module Not Registered in Session
                {
                    await ServiceAggregator.SecurityManagerProvider.AddModuleSnapshot(Token, ModuleSnapshot.FromModule(module));
                }
                else                            // If Module is Registered in Session
                {
                    await ServiceAggregator.SecurityManagerProvider.UpdateModuleSnapshot(Token, (Guid)module.Id, ModuleSnapshot.FromModule(module));
                }
            }
          
            return Ok(modules);
        }

        [HttpGet("{token}/Modules/{ModuleId}")] 
        public async Task<IActionResult> FetchPatientModuleById(string Token, Guid ModuleId) 
        {
            var isTokenValid = await ServiceAggregator.SecurityManagerProvider.FetchTokenData(Token);
            if (isTokenValid == null || isTokenValid.IsExpired)
                return BadRequest("Session is Expired");
            if (isTokenValid.UserType == SystemGateway.Dtos.Enum.UserType.Therapist)
                return BadRequest("User is not a Patient");


            var module = await ServiceAggregator.DatabaseProvider.FindPatientModuleById(isTokenValid.Email, ModuleId);
            if (module == null) return NotFound("Patient Module not Found");

            await ServiceAggregator.SecurityManagerProvider.UpdateModuleSnapshot(Token, (Guid)module.Id, ModuleSnapshot.FromModule(module));
            return Ok(module);
        }
        [HttpGet] 
        public async Task<IActionResult> FetchAllPatients()
        {
            var users = await ServiceAggregator.DatabaseProvider.FindAllPatients();
            if (users == null) return NotFound("User not Found");
            return Ok(users);
        }

        // Other Operations -----------------------------------------------------------------------------------------------------
        [HttpGet("{Token}/Modules/{ModuleId}/Status")]
        public async Task<IActionResult> ModuleStatusCheck(string Token, Guid ModuleId)
        {
            var isTokenValid = await ServiceAggregator.SecurityManagerProvider.FetchTokenData(Token);
            if (isTokenValid == null || isTokenValid.IsExpired)
                return BadRequest("Session is Expired");

            if (isTokenValid.UserType == SystemGateway.Dtos.Enum.UserType.Therapist)
                return BadRequest("User is not a Patient");

            var data = isTokenValid.ModuleSnapshots.FirstOrDefault(c => c.ModuleId == ModuleId);
            if (data == null) return NotFound("Module Not Found");
           
            var patient = await ServiceAggregator.DatabaseProvider.FindPatientById(isTokenValid.Email);
            if (patient == null) return NotFound("Patient Not Found");

            var module = patient.Modules.FirstOrDefault(c => c.Id == ModuleId);
            if (module == null) return NotFound("Module Not Found");

            if (module.ModuleData.Checksum != data.Checksum && module.ModuleData.Timestamp > data.Timestamp) return Ok(new ModuleStatusCheck { HasUpdates = true });
            return Ok(new ModuleStatusCheck { HasUpdates = false });
        }

     
        [HttpGet("{Token}/Therapists")]
        public async Task<IActionResult> FetchPatientTherapists(string Token)
        {
            var isTokenValid = await ServiceAggregator.SecurityManagerProvider.FetchTokenData(Token);
            if (isTokenValid == null || isTokenValid.IsExpired)
                return BadRequest("Session is Expired");

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
            var requestedTherapists = new List<SafeTherapist>();
            foreach (var email in patient.RequestedTherapists)
            {
                var therapist = await ServiceAggregator.DatabaseProvider.FindTherapistById(email);
                if (therapist == null) continue;
                else requestedTherapists.Add(SafeTherapist.FromTherapist(therapist));
            }

            var allTherapists = await ServiceAggregator.DatabaseProvider.FindAllTherapists();
            if (allTherapists == null) return BadRequest("Failed to Retrieve Therapists");
            var availableTherapists = allTherapists.ToList().FindAll(c => !patient.AcceptedTherapists.Contains(c.Email) && !patient.RequestedTherapists.Contains(c.Email) && !c.RequestedPatients.Contains(patient.Email));
            var pendingTherapists = allTherapists.ToList().FindAll(c => c.RequestedPatients.Contains(isTokenValid.Email));
            return Ok(new Associations<SafeTherapist>
            {
                Accepted = acceptedTherapists,
                Requested = requestedTherapists,
                Available = availableTherapists,
                Pending = pendingTherapists
            });
        }



    }
}
