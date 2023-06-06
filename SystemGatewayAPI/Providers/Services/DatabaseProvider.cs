using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using SystemGateway.Configurations;
using SystemGateway.Dtos.Entities;

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
        public async Task<ICollection<User>> FindAllUsers()
        {
            var url = $"{_BaseUrl}/api/User";
            var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
                return new List<User>();
            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ICollection<User>>(responseContent);
        }

        public async Task<User?> FindUserById(string Email)
        {
            var url = $"{_BaseUrl}/api/User/{Email}";
            var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
                return null;
            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<User>(responseContent);
        }
        public async Task<ICollection<string>> FindPairedModulesByUserId(string Email)
        {
            var url = $"{_BaseUrl}/api/User/{Email}/Connection";
            var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
                return new List<string>();
            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ISet<string>>(responseContent);
        }

        public async Task<ICollection<Module>> FindAllModules()
        {
            var url = $"{_BaseUrl}/api/Modules";
            var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
                return new List<Module>();
            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ICollection<Module>>(responseContent);
        }

        public async Task<Module?> FindModuleById(string Id)
        {
            var url = $"{_BaseUrl}/api/Modules/{Id}";
            var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
                return null;
            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Module>(responseContent);
        }
    }
}
