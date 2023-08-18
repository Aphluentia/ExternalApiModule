using DatabaseApi.Models.Entities;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Text.Json.Serialization;
using SystemGateway.Configurations;

namespace SystemGateway.Providers
{
    public class DatabaseProvider : IDatabaseProvider
    {
        private readonly HttpClient _httpClient;
        private readonly string _BaseUrl;
        public DatabaseProvider(IOptions<DatabaseApiConfigSection> options)
        {
            _httpClient = new HttpClient();
            _BaseUrl = options.Value.ConnectionString;
        }

        public async Task<ICollection<Application>> FindAllApplications()
        {
            var response = await GetAsync("/Application");
            if (!response.IsSuccessStatusCode)
                return new List<Application>();
            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ICollection<Application>>(responseContent);
        }

        public async Task<ICollection<Module>> FindAllModules()
        {
            var response = await GetAsync("/Modules");
            if (!response.IsSuccessStatusCode)
                return new List<Module>();
            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ICollection<Module>>(responseContent);
        }

        public async Task<ICollection<Patient>> FindAllPatients()
        {
            var response = await GetAsync("/Patient");
            if (!response.IsSuccessStatusCode)
                return new List<Patient>();
            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ICollection<Patient>>(responseContent);
        }

        public async Task<ICollection<Therapist>> FindAllTherapists()
        {
            var response = await GetAsync("/Therapist");
            if (!response.IsSuccessStatusCode)
                return new List<Therapist>();
            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ICollection<Therapist>>(responseContent);
        }

        public async Task<Application?> FindApplicationById(string id)
        {
            var response = await GetAsync($"/Application/{id}");
            if (!response.IsSuccessStatusCode)
                return null;
            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Application>(responseContent);
        }

        public async Task<HttpResponseMessage> FindModuleById(string id)
        {
            var response = await GetAsync($"/Modules/{id}");
            if (!response.IsSuccessStatusCode)
                return null;
            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Module>(responseContent);
        }
        public async Task<Module> DeleteModuleById(string id)
        {
            var response = await GetAsync($"/Modules/{id}");
            if (!response.IsSuccessStatusCode)
                return null;
            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Module>(responseContent);
        }

        public async Task<Patient> FindPatientById(string email)
        {
            var response = await GetAsync($"/Patient/{email}");
            if (!response.IsSuccessStatusCode)
                return null;
            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Patient>(responseContent);
        }

        public async Task<Therapist?> FindTherapistById(string email)
        {
            var response = await GetAsync($"/Therapist/{email}");
            if (!response.IsSuccessStatusCode)
                return null;
            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Therapist>(responseContent);
        }

        private async Task<HttpResponseMessage> GetAsync(string endpoint) => await _httpClient.GetAsync(_BaseUrl+endpoint);
       
      
        
    }
}
