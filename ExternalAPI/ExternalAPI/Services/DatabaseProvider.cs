using ExternalAPI.Configurations;
using Microsoft.Extensions.ObjectPool;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace ExternalAPI.Services
{
    public class DatabaseProvider: IDatabaseProvider
    {
        private readonly HttpClient _httpClient;
        private readonly string _BaseUrl;
        public DatabaseProvider(IOptions<DatabaseApiConfigSection> options)
        {
            _httpClient = new HttpClient();
            _BaseUrl = options.Value.ConnectionString;
        }

        public async Task<(bool, string?)> Get(string endpoint)
        {
            var url = _BaseUrl + endpoint;
            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                return (false, null);
            }

            var responseContent = await response.Content.ReadAsStringAsync();

            return (true, responseContent);
        }

        public async Task<(bool, string?)> Post(string endpoint, object body)
        {
            var response = await _httpClient.PostAsJsonAsync(_BaseUrl + endpoint, body);

            if (!response.IsSuccessStatusCode)
            {
                return (false, null);
            }

            var responseContent = await response.Content.ReadAsStringAsync();

            return (true, responseContent);
        }

        public async Task<(bool, string?)> Put(string endpoint, object body)
        {
            var response = await _httpClient.PutAsJsonAsync(_BaseUrl + endpoint, body);

            if (!response.IsSuccessStatusCode)
            {
                return (false, null);
            }

            var responseContent = await response.Content.ReadAsStringAsync();

            return (true, responseContent);
        }

        public async Task<(bool, string?)> Delete(string endpoint)
        {
            var response = await _httpClient.DeleteAsync(_BaseUrl + endpoint);

            if (!response.IsSuccessStatusCode)
            {
                return (false, null);
            }

            var responseContent = await response.Content.ReadAsStringAsync();

            return (true, responseContent);
        }

       

    }
}
