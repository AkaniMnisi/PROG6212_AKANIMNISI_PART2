using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace RaceDay.MVC.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;
        // REPLACE with your actual API URL (check your API's launchSettings.json)
        private readonly string _apiBaseUrl = "https://localhost:7123/api/";

        public ApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

       
        public async Task<HttpResponseMessage> LoginAsync(string email, string password)
        {
            var loginData = new { Email = email, Password = password };
            var content = new StringContent(JsonSerializer.Serialize(loginData), Encoding.UTF8, "application/json");

            return await _httpClient.PostAsync($"{_apiBaseUrl}Auth/login", content);
        }

       
        public async Task<string> GetEventsAsync()
        {
            var response = await _httpClient.GetAsync($"{_apiBaseUrl}Events");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
    }
}