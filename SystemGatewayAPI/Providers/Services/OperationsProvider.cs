using Microsoft.Extensions.Options;
using SystemGateway.Configurations;
using SystemGateway.Dtos.Entities;
using SystemGateway.Providers;

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
        public async Task<bool> RegisterModule(Module module)
        {
            var response = await _httpClient.PostAsJsonAsync($"{_BaseUrl}/api/Modules", module);
            if (!response.IsSuccessStatusCode)
                return false;
            return true;
        }
        public async Task<bool> RegisterUser(User user)
        {
            var response = await _httpClient.PostAsJsonAsync($"{_BaseUrl}/api/User", user);
            if (!response.IsSuccessStatusCode)
                return false;
            return true;
        }
        public async Task<bool> RegisterConnection(ModuleConnection moduleConnection)
        {
            var response = await _httpClient.PostAsJsonAsync($"{_BaseUrl}/api/User/{moduleConnection.WebPlatformId}/Connection", moduleConnection);
            if (!response.IsSuccessStatusCode)
                return false;
            return true;
        }
        public async Task<bool> UpdateModule(string ModuleId, Module module)
        {
            var response = await _httpClient.PutAsJsonAsync($"{_BaseUrl}/api/Modules/{ModuleId}", module);
            if (!response.IsSuccessStatusCode)
                return false;
            return true;
        }
        public async Task<bool> UpdateUser(string Email, User updatedUser)
        {
            var response = await _httpClient.PutAsJsonAsync($"{_BaseUrl}/api/User/{Email}", updatedUser);
            if (!response.IsSuccessStatusCode)
                return false;
            return true;
        }
        public async Task<bool> DeleteConnection(ModuleConnection moduleConnection)
        {
            var response = await _httpClient.DeleteAsync($"{_BaseUrl}/api/User/Connection/{moduleConnection.WebPlatformId}/{moduleConnection.ModuleId}");
            if (!response.IsSuccessStatusCode)
                return false;
            return true;
        }
        public async Task<bool> DeleteModule(string ModuleId)
        {
            var response = await _httpClient.DeleteAsync($"{_BaseUrl}/api/Modules/{ModuleId}");
            if (!response.IsSuccessStatusCode)
                return false;
            return true;
        }
        public async Task<bool> DeleteUser(string Email)
        {
            var response = await _httpClient.DeleteAsync($"{_BaseUrl}/api/User/{Email}");
            if (!response.IsSuccessStatusCode)
                return false;
            return true;
        }
        
    }
}
