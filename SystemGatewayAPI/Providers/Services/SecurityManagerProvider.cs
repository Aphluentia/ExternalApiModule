using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SystemGateway.Configurations;
using SystemGateway.Dtos.SecurityManager;

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

        public async Task<string> GenerateSession(SecurityDataDto securityData)
        {
            var response = await _httpClient.PostAsJsonAsync($"{_BaseUrl}/Session/GenerateSession", securityData);
            if (!response.IsSuccessStatusCode)
                return "";
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<SecurityDataDto?> GetTokenData(string Token)
        {
            var response = await _httpClient.GetAsync($"{_BaseUrl}/Session/RetrieveSessionData/{Token}");
            if (!response.IsSuccessStatusCode)
                return null;
            var data = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrEmpty(data)) return null;
            return JsonConvert.DeserializeObject<SecurityDataDto>(data);
        }

        public async Task<bool> KeepAlive(string _token)
        {
            var response = await _httpClient.GetAsync($"{_BaseUrl}/Session/KeepAlive/{_token}");
            if (!response.IsSuccessStatusCode)
                return false;
            return true;
        }

        public async Task<bool> ValidateSession(string _token)
        {
            var response = await _httpClient.GetAsync($"{_BaseUrl}/Session/ValidateSession/{_token}");
            if (!response.IsSuccessStatusCode)
                return false;
            return true;
        }
    }
}
