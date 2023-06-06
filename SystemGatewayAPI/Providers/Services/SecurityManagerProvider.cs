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
            var response = await _httpClient.PostAsJsonAsync($"{_BaseUrl}/api/Session/GenerateSession", securityData);
            if (!response.IsSuccessStatusCode)
                return "";
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<SecurityDataDto> GetTokenData(string Token)
        {
            var response = await _httpClient.GetAsync($"{_BaseUrl}/api/Session/RetrieveSessionData/{Token}");
            if (!response.IsSuccessStatusCode)
                return SecurityDataDto.Empty();
            var data = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<SecurityDataDto>(data);
        }

        public async Task<string> KeepAlive(string _token)
        {
            var response = await _httpClient.PostAsJsonAsync($"{_BaseUrl}/api/Session/KeepAlive", new TokenDto { Token = _token });
            if (!response.IsSuccessStatusCode)
                return "";
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> ValidateSession(string _token)
        {
            var response = await _httpClient.PostAsJsonAsync($"{_BaseUrl}/api/Session/ValidateSession", new TokenDto { Token = _token });
            if (!response.IsSuccessStatusCode)
                return "";
            return await response.Content.ReadAsStringAsync();
        }
    }
}
