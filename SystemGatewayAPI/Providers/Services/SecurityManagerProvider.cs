using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SystemGateway.Configurations;
using SystemGateway.Dtos.SecurityManager;
using SystemGatewayAPI.Dtos.SecurityManager;

namespace SystemGateway.Providers
{
    public class SecurityManagerProvider : ISecurityManagerProvider
    {
        private readonly HttpClient _httpClient;
        private readonly string _BaseUrl;
        public SecurityManagerProvider(IOptions<SecurityManagerConfigSection> options)
        {
            _httpClient = new HttpClient();
            _BaseUrl = options.Value.ConnectionString;
        }

        public async Task<bool> AddModuleSnapshot(string Token, ModuleSnapshot moduleSnapshot)
        {
            var response = await _httpClient.PostAsJsonAsync($"{_BaseUrl}/Session/{Token}/Modules", moduleSnapshot);
            if (!response.IsSuccessStatusCode)
                return false;
            return true;
        }
        public async Task<bool> UpdateModuleSnapshot(string Token, Guid ModuleId, ModuleSnapshot moduleSnapshot)
        {
            var response = await _httpClient.PutAsJsonAsync($"{_BaseUrl}/Session/{Token}/Modules/{ModuleId}", moduleSnapshot);
            if (!response.IsSuccessStatusCode)
                return false;
            return true;
        }

        public async Task<bool> DeleteModuleSnapshot(string Token, Guid ModuleId)
        {
            var response = await _httpClient.DeleteAsync($"{_BaseUrl}/Session/{Token}/Modules/{ModuleId}");
            if (!response.IsSuccessStatusCode)
                return false;
            return true;
        }

        public async Task<SecurityDataDto> FetchTokenData(string Token)
        {
            var response = await _httpClient.GetAsync($"{_BaseUrl}/Session/{Token}/Fetch");
            if (!response.IsSuccessStatusCode)
                return null;
            var data = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrEmpty(data)) return null;
            return JsonConvert.DeserializeObject<SecurityDataDto>(data);
        }
        public async Task<bool> ValidateSession(string _token)
        {
            var response = await _httpClient.GetAsync($"{_BaseUrl}/Session/{_token}/Validate");
            if (!response.IsSuccessStatusCode)
                return false;
            return true;
        }
        public async Task<bool> KeepAlive(string _token)
        {
            var response = await _httpClient.GetAsync($"{_BaseUrl}/Session/{_token}/KeepAlive");
            if (!response.IsSuccessStatusCode)
                return false;
            return true;
        }
        public async Task<string> GenerateSession(SecurityDataDto securityData)
        {
            var response = await _httpClient.PostAsJsonAsync($"{_BaseUrl}/Session/Generate", securityData);
            if (!response.IsSuccessStatusCode)
                return "";
            return await response.Content.ReadAsStringAsync();
        }

        
        
    }
}
