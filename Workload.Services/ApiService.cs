using System.Diagnostics.Metrics;
using System.Text.Json;
using System.Text;
using Workload.Models;
using System.Collections.ObjectModel;

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

        public async Task<ObservableCollection<DutyModel>> GetDuties()
        {
            var response = await _client.GetAsync("/api/duties");
            var content = await response.Content.ReadAsStringAsync();

            var collection = JsonSerializer.Deserialize<ObservableCollection<DutyModel>>(content, _options);

            return collection;
        }

        public async Task DeleteDuty(int dutyId)
        {
            await _client.DeleteAsync($"/api/duties/{dutyId}");
        }

        public async Task UpdateDuty(DutyModel duty)
        {
            var json = JsonSerializer.Serialize(duty, _options);

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            await _client.PutAsync($"/api/duties/{duty.Id}", content);
        }



    public async Task PostEmployee(EmployeeModel employee)
        {
            var json = JsonSerializer.Serialize(employee, _options);

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            await _client.PostAsync("/api/employees", content);
        }

        public async Task<ObservableCollection<EmployeeModel>> GetEmployees()
        {
            var response = await _client.GetAsync("/api/employees");
            var content = await response.Content.ReadAsStringAsync();

            var collection = JsonSerializer.Deserialize<ObservableCollection<EmployeeModel>>(content, _options);

            return collection;
        }
    }
}