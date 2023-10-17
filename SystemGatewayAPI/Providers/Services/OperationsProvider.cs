using DatabaseApi.Models.Entities;
using Microsoft.Extensions.Options;
using System;
using SystemGateway.Configurations;

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

        public async Task<bool> AddApplicationVersion(string ApplicationId, ModuleVersion version)
        {
            var response = await _httpClient.PutAsJsonAsync($"{_BaseUrl}/Application/{ApplicationId}", version);
            if (!response.IsSuccessStatusCode)
                return false;
            return true;
        }

        public async Task<bool> AddExistingModuleToPatient(string Email, string ModuleId)
        {
            var response = await _httpClient.PostAsJsonAsync($"{_BaseUrl}/Patient/{Email}/Modules/{ModuleId}", "");
            if (!response.IsSuccessStatusCode)
                return false;
            return true;
        }

        public async Task<bool> AddNewModuleToPatient(string Email, Module Module)
        {
            var response = await _httpClient.PostAsJsonAsync($"{_BaseUrl}/Patient/{Email}/Modules", Module);
            if (!response.IsSuccessStatusCode)
                return false;
            return true;
        }

        public async Task<bool> DeleteApplication(string ApplicationId)
        {
            var response = await _httpClient.DeleteAsync($"{_BaseUrl}/Application/{ApplicationId}");
            if (!response.IsSuccessStatusCode)
                return false;
            return true;
        }

        public async Task<bool> DeleteApplicationVersion(string ApplicationId, string VersionId)
        {
            var response = await _httpClient.DeleteAsync($"{_BaseUrl}/Application/{ApplicationId}/Version/{VersionId}");
            if (!response.IsSuccessStatusCode)
                return false;
            return true;
        }

        public async Task<bool> DeleteModule(string ModuleId)
        {
            var response = await _httpClient.DeleteAsync($"{_BaseUrl}/Modules/{ModuleId}");
            if (!response.IsSuccessStatusCode)
                return false;
            return true;
        }

        public async Task<bool> DeletePatient(string Email)
        {
            var response = await _httpClient.DeleteAsync($"{_BaseUrl}/Patient/{Email}");
            if (!response.IsSuccessStatusCode)
                return false;
            return true;
        }

        public async Task<bool> DeletePatientModule(string Email, string ModuleId)
        {
            var response = await _httpClient.DeleteAsync($"{_BaseUrl}/Patient/{Email}/Modules/{ModuleId}");
            if (!response.IsSuccessStatusCode)
                return false;
            return true;
        }

        public async Task<bool> DeleteTherapist(string Email)
        {
            var response = await _httpClient.DeleteAsync($"{_BaseUrl}/Therapist/{Email}");
            if (!response.IsSuccessStatusCode)
                return false;
            return true;
        }

        public async Task<bool> PatientRejectTherapist(string Email, string TherapistEmail)
        {
            var response = await _httpClient.DeleteAsync($"{_BaseUrl}/Patient/{Email}/Therapist/{TherapistEmail}");
            if (!response.IsSuccessStatusCode)
                return false;
            return true;
        }

        public async Task<bool> PatientRequestTherapist(string Email, string TherapistEmail)
        {
            var response = await _httpClient.PutAsJsonAsync($"{_BaseUrl}/Patient/{Email}/Therapist/{TherapistEmail}", "");
            if (!response.IsSuccessStatusCode)
                return false;
            return true;
        }

        public async Task<bool> RegisterApplication(Application application)
        {
            var response = await _httpClient.PostAsJsonAsync($"{_BaseUrl}/Application", application);
            if (!response.IsSuccessStatusCode)
                return false;
            return true;
        }

        public async Task<bool> RegisterModule(Module module)
        {
            var response = await _httpClient.PostAsJsonAsync($"{_BaseUrl}/Modules", module);
            if (!response.IsSuccessStatusCode)
                return false;
            return true;
        }

        public async Task<bool> RegisterPatient(Patient patient)
        {
            var response = await _httpClient.PostAsJsonAsync($"{_BaseUrl}/Patient", patient);
            if (!response.IsSuccessStatusCode)
                return false;
            return true;
        }

        public async Task<bool> RegisterTherapist(Therapist therapist)
        {
            var response = await _httpClient.PostAsJsonAsync($"{_BaseUrl}/Therapist", therapist);
            if (!response.IsSuccessStatusCode)
                return false;
            return true;
        }

        public async Task<bool> TherapistAcceptPatient(string Email, string PatientEmail)
        {
            var response = await _httpClient.PutAsJsonAsync($"{_BaseUrl}/Therapist/{Email}/Patient/{PatientEmail}", "");
            if (!response.IsSuccessStatusCode)
                return false;
            return true;
        }

        public async Task<bool> TherapistRejectPatient(string Email, string PatientEmail)
        {
            var response = await _httpClient.DeleteAsync($"{_BaseUrl}/Therapist/{Email}/Patient/{PatientEmail}");
            if (!response.IsSuccessStatusCode)
                return false;
            return true;
        }

        public async Task<bool> UpdateApplicationVersion(string ApplicationId, string VersionId, ModuleVersion version)
        {
            var response = await _httpClient.PutAsJsonAsync($"{_BaseUrl}/Application/{ApplicationId}/Version/{VersionId}", version);
            if (!response.IsSuccessStatusCode)
                return false;
            return true;
        }

        public async Task<bool> UpdateModule(string ModuleId, Module module)
        {
            module.Id = Guid.Parse(ModuleId);
            var response = await _httpClient.PutAsJsonAsync($"{_BaseUrl}/Modules/{ModuleId}", module);
            if (!response.IsSuccessStatusCode)
                return false;
            return true;
        }

        public async Task<bool> UpdateModuleToVersion(string ModuleId, string VersionId)
        {
            var response = await _httpClient.PostAsJsonAsync($"{_BaseUrl}/Modules/{ModuleId}/Version/{VersionId}", "");
            if (!response.IsSuccessStatusCode)
                return false;
            return true;
        }

        public async Task<bool> UpdateModuleVersion(string ModuleId, ModuleVersion version)
        {
            var response = await _httpClient.PutAsJsonAsync($"{_BaseUrl}/Modules/{ModuleId}/Version", version);
            if (!response.IsSuccessStatusCode)
                return false;
            return true;
        }

        public async Task<bool> UpdatePatient(string Email, Patient patient)
        {
            var response = await _httpClient.PutAsJsonAsync($"{_BaseUrl}/Patient/{Email}", patient);
            if (!response.IsSuccessStatusCode)
                return false;
            return true;
        }

        public async Task<bool> UpdatePatientModule(string Email, string ModuleId, Module Module)
        {
            Module.Id = Guid.Parse(ModuleId);
            var response = await _httpClient.PutAsJsonAsync($"{_BaseUrl}/Patient/{Email}/Modules/{ModuleId}", Module);
            if (!response.IsSuccessStatusCode)
                return false;
            return true;
        }

        public async Task<bool> UpdatePatientModuleToVersion(string Email, string ModuleId, string VersionId)
        {
            var response = await _httpClient.PutAsJsonAsync($"{_BaseUrl}/Patient/{Email}/Modules/{ModuleId}/Version/{VersionId}", "");
            if (!response.IsSuccessStatusCode)
                return false;
            return true;
        }

        public async Task<bool> UpdateTherapist(string Email, Therapist therapist)
        {
            var response = await _httpClient.PutAsJsonAsync($"{_BaseUrl}/Therapist/{Email}", therapist);
            if (!response.IsSuccessStatusCode)
                return false;
            return true;
        }
    }
}
