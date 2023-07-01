using System.Diagnostics.Metrics;
using System.Text.Json;
using System.Text;
using Workload.Models;


namespace Workload.Services
{
    public class ApiService
    {
        private HttpClient _client;
        private JsonSerializerOptions _options;
        public ApiService(string url)
        {
            _client = new HttpClient
            {
                BaseAddress = new Uri(url)
            };
            _options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
        }
        public async Task PostDuty(DutyModel duty)
        {
            var json = JsonSerializer.Serialize(duty, _options);

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            await _client.PostAsync("/api/duties", content);
        }
    }
}