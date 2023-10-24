using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Text.Json.Serialization;
using SystemGateway.Configurations;
using SystemGatewayAPI.Dtos.Entities.Database;
using SystemGatewayAPI.Dtos.Entities.Secure;

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
        public async Task<Application?> FindApplicationById(string id)
        {
            var response = await GetAsync($"/Application/{id}");
            if (!response.IsSuccessStatusCode)
                return null;
            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Application>(responseContent);
        }


        public async Task<ICollection<Module>> FindAllModules()
        {
            var response = await GetAsync("/Modules");
            if (!response.IsSuccessStatusCode)
                return new List<Module>();
            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ICollection<Module>>(responseContent);
        }
        public async Task<Module> FindModuleById(Guid id)
        {
            var response = await GetAsync($"/Modules/{id}");
            if (!response.IsSuccessStatusCode)
                return null;
            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Module>(responseContent);
        }


        public async Task<ICollection<SafePatient>> FindAllPatients()
        {
            var response = await GetAsync("/Patient");
            if (!response.IsSuccessStatusCode)
                return new List<SafePatient>();
            var responseContent = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<ICollection<Patient>>(responseContent);
            return SafePatient.FromAll(data);
        }
        public async Task<Patient> FindPatientById(string email)
        {
            var response = await GetAsync($"/Patient/{email}");
            if (!response.IsSuccessStatusCode)
                return null;
            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Patient>(responseContent);
        }

        public async Task<ICollection<SafeTherapist>> FindAllTherapists()
        {
            var response = await GetAsync("/Therapist");
            if (!response.IsSuccessStatusCode)
                return new List<SafeTherapist>();
            var responseContent = await response.Content.ReadAsStringAsync();

            var data = JsonConvert.DeserializeObject<ICollection<Therapist>>(responseContent);
            return SafeTherapist.FromAll(data);
        }

        

        

        public async Task<Therapist?> FindTherapistById(string email)
        {
            var response = await GetAsync($"/Therapist/{email}");
            if (!response.IsSuccessStatusCode)
                return null;
            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Therapist>(responseContent);
        }
        public async Task<ICollection<Module>> FindAllPatientModules(string email)
        {
            var response = await GetAsync($"/Patient/{email}/Modules");
            if (!response.IsSuccessStatusCode)
                return null;
            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ICollection<Module>>(responseContent);
        }

        public async Task<Module?> FindPatientModuleById(string email, Guid ModuleId)
        {
            var response = await GetAsync($"/Patient/{email}/Modules/{ModuleId}");
            if (!response.IsSuccessStatusCode)
                return null;
            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Module>(responseContent);
        }

        private async Task<HttpResponseMessage> GetAsync(string endpoint) => await _httpClient.GetAsync(_BaseUrl + endpoint);
    }
}
