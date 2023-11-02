using Microsoft.Extensions.Options;
using System;
using SystemGateway.Configurations;
using SystemGatewayAPI.Dtos.Entities;
using SystemGatewayAPI.Dtos.Entities.Database;

namespace SystemGateway.Providers
{
    public class OperationsProvider : IOperationsProvider
    {
        private readonly HttpClient _httpClient;
        private readonly string _BaseUrl;
        public OperationsProvider(IOptions<OperationsApiConfigSection> options)
        {
            _httpClient = new HttpClient();
            _BaseUrl = options.Value.ConnectionString;
        }
        private async Task<ActionResponse> GetResponse(HttpResponseMessage httpResponse)
        {
            if (!httpResponse.IsSuccessStatusCode)
                return new ActionResponse
                {
                    Code = System.Net.HttpStatusCode.BadRequest,
                    Message = await httpResponse.Content.ReadAsStringAsync()
                };
            return new ActionResponse
            {
                Code = System.Net.HttpStatusCode.OK,
                Message = await httpResponse.Content.ReadAsStringAsync()
            };
        }
        // Application ----------------------------------------------------------------------------------------------------------
        public async Task<ActionResponse> RegisterApplication(Application application)
        {
            var response = await _httpClient.PostAsJsonAsync($"{_BaseUrl}/Application", application);
            return await GetResponse(response);
           
        }

        public async Task<ActionResponse> AddApplicationVersion(string ApplicationId, ModuleVersion version)
        {
            var response = await _httpClient.PutAsJsonAsync($"{_BaseUrl}/Application/{ApplicationId}", version);
            return await GetResponse(response);
        }
        public async Task<ActionResponse> UpdateApplicationVersion(string ApplicationId, string VersionId, ModuleVersion version)
        {
            var response = await _httpClient.PutAsJsonAsync($"{_BaseUrl}/Application/{ApplicationId}/Version/{VersionId}", version);
            return await GetResponse(response);
        }
        public async Task<ActionResponse> DeleteApplication(string ApplicationId)
        {
            var response = await _httpClient.DeleteAsync($"{_BaseUrl}/Application/{ApplicationId}");
            return await GetResponse(response);
        }

        public async Task<ActionResponse> DeleteApplicationVersion(string ApplicationId, string VersionId)
        {
            var response = await _httpClient.DeleteAsync($"{_BaseUrl}/Application/{ApplicationId}/Version/{VersionId}");
            return await GetResponse(response);
        }

        // Modules --------------------------------------------------------------------------------------------------------------
        public async Task<ActionResponse> RegisterModule(Module module)
        {
            var response = await _httpClient.PostAsJsonAsync($"{_BaseUrl}/Modules", module);
            return await GetResponse(response);
        }
        public async Task<ActionResponse> ModuleAddNewContext(string ModuleId, string ProfileName)
        {
            var response = await _httpClient.PostAsJsonAsync($"{_BaseUrl}/Modules/{ModuleId}/Profile/{ProfileName}", "");
            return await GetResponse(response);
        }

        public async Task<ActionResponse> ModuleDeleteContext(string ModuleId, string ProfileName)
        {
            var response = await _httpClient.DeleteAsync($"{_BaseUrl}/Modules/{ModuleId}/Profile/{ProfileName}");
            return await GetResponse(response);
        }
        public async Task<ActionResponse> UpdateModule(string ModuleId, Module module)
        {
            var response = await _httpClient.PutAsJsonAsync($"{_BaseUrl}/Modules/{ModuleId}", module.ModuleData);
            return await GetResponse(response);
        }
        public async Task<ActionResponse> UpdateModuleToVersion(string ModuleId, string VersionId)
        {
            var response = await _httpClient.PostAsJsonAsync($"{_BaseUrl}/Modules/{ModuleId}/Version/{VersionId}", "");
            return await GetResponse(response);
        }
        public async Task<ActionResponse> DeleteModule(string ModuleId)
        {
            var response = await _httpClient.DeleteAsync($"{_BaseUrl}/Modules/{ModuleId}");
            return await GetResponse(response);
        }

        // Patient --------------------------------------------------------------------------------------------------------------
        public async Task<ActionResponse> RegisterPatient(Patient patient)
        {
            var response = await _httpClient.PostAsJsonAsync($"{_BaseUrl}/Patient", patient);
            return await GetResponse(response);
        }
        public async Task<ActionResponse> UpdatePatient(string Email, Patient patient)
        {
            var response = await _httpClient.PutAsJsonAsync($"{_BaseUrl}/Patient/{Email}", patient);
            return await GetResponse(response);
        }
        public async Task<ActionResponse> DeletePatient(string Email)
        {
            var response = await _httpClient.DeleteAsync($"{_BaseUrl}/Patient/{Email}");
            return await GetResponse(response);
        }
        public async Task<ActionResponse> AddExistingModuleToPatient(string Email, string ModuleId)
        {
            var response = await _httpClient.PostAsJsonAsync($"{_BaseUrl}/Patient/{Email}/Modules/{ModuleId}", "");
            return await GetResponse(response);
        }
        public async Task<ActionResponse> AddNewModuleToPatient(string Email, Module Module)
        {
            var response = await _httpClient.PostAsJsonAsync($"{_BaseUrl}/Patient/{Email}/Modules", Module);
            return await GetResponse(response);
        }
        public async Task<ActionResponse> PatientAddNewModuleContext(string Email, string ModuleId, string ProfileName)
        {
            var response = await _httpClient.PostAsJsonAsync($"{_BaseUrl}/Patient/{Email}/Modules/{ModuleId}/Profile/{ProfileName}", "");
            return await GetResponse(response);
        }

        public async Task<ActionResponse> PatientDeleteModuleContext(string Email, string ModuleId, string ProfileName)
        {
            var response = await _httpClient.DeleteAsync($"{_BaseUrl}/Patient/{Email}/Modules/{ModuleId}/Profile/{ProfileName}");
            return await GetResponse(response);
        }
        public async Task<ActionResponse> UpdatePatientModule(string Email, string ModuleId, Module Module)
        {
            var response = await _httpClient.PutAsJsonAsync($"{_BaseUrl}/Patient/{Email}/Modules/{ModuleId}", Module);
            return await GetResponse(response);
        }
        public async Task<ActionResponse> UpdatePatientModuleToVersion(string Email, string ModuleId, string VersionId)
        {
            var response = await _httpClient.PutAsJsonAsync($"{_BaseUrl}/Patient/{Email}/Modules/{ModuleId}/Version/{VersionId}", "");
            return await GetResponse(response);
        }

        public async Task<ActionResponse> DeletePatientModule(string Email, string ModuleId)
        {
            var response = await _httpClient.DeleteAsync($"{_BaseUrl}/Patient/{Email}/Modules/{ModuleId}");
            return await GetResponse(response);
        }

        public async Task<ActionResponse> PatientRejectTherapist(string Email, string TherapistEmail)
        {
            var response = await _httpClient.DeleteAsync($"{_BaseUrl}/Patient/{Email}/Therapist/{TherapistEmail}");
            return await GetResponse(response);
        }
        public async Task<ActionResponse> PatientRequestTherapist(string Email, string TherapistEmail)
        {
            var response = await _httpClient.PutAsJsonAsync($"{_BaseUrl}/Patient/{Email}/Therapist/{TherapistEmail}", "");
            return await GetResponse(response);
        }


        // Therapist ------------------------------------------------------------------------------------------------------------
        public async Task<ActionResponse> RegisterTherapist(Therapist therapist)
        {
            var response = await _httpClient.PostAsJsonAsync($"{_BaseUrl}/Therapist", therapist);
            return await GetResponse(response);
        }
        public async Task<ActionResponse> UpdateTherapist(string Email, Therapist therapist)
        {
            var response = await _httpClient.PutAsJsonAsync($"{_BaseUrl}/Therapist/{Email}", therapist);
            return await GetResponse(response);
        }
        public async Task<ActionResponse> DeleteTherapist(string Email)
        {
            var response = await _httpClient.DeleteAsync($"{_BaseUrl}/Therapist/{Email}");
            return await GetResponse(response);
        }
        public async Task<ActionResponse> TherapistAcceptPatient(string Email, string PatientEmail)
        {
            var response = await _httpClient.PutAsJsonAsync($"{_BaseUrl}/Therapist/{Email}/Patient/{PatientEmail}", "");
            return await GetResponse(response);
        }
        public async Task<ActionResponse> TherapistRejectPatient(string Email, string PatientEmail)
        {
            var response = await _httpClient.DeleteAsync($"{_BaseUrl}/Therapist/{Email}/Patient/{PatientEmail}");
            return await GetResponse(response);
        }

       

    
    }
}
