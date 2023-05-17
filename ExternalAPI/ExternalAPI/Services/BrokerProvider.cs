using ExternalAPI.Configurations;
using Microsoft.Extensions.Options;
using System.Net.Http;

namespace ExternalAPI.Services
{
    public class BrokerProvider: IBrokerProvider
    {
        private readonly HttpClient _httpClient;
        private readonly string _BaseUrl;
        public BrokerProvider(IOptions<BrokerApiConfigSection> options)
        {
            _httpClient = new HttpClient();
            _BaseUrl = options.Value.ConnectionString;
        }

        public async Task<(bool, string?)> CloseConnection(string ModuleId, string WebPlatformId, int ModuleType)
        {
            return await this.Get("Close", ModuleId, WebPlatformId, ModuleType);
        }

        public async Task<(bool, string?)> CreateConnection(string ModuleId, string WebPlatformId, int ModuleType)
        {
            return await this.Get("Create", ModuleId, WebPlatformId, ModuleType);
        }

        private async Task<(bool, string?)> Get(string Action, string ModuleId, string WebPlatformId, int ModuleType)
        {
            var url = $"{_BaseUrl}/Kafka/{Action}?ModuleId={ModuleId}&WebPlatformId={WebPlatformId}&ModuleType={ModuleType}";
            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                return (false, null);
            }

            var responseContent = await response.Content.ReadAsStringAsync();

            return (true, responseContent);
        }

    }
}
