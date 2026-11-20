using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace RaceDay.MVC.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiBaseUrl = "https://localhost:7112/api/";

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

        public async Task<HttpResponseMessage> UploadImageAsync(Stream fileStream, string fileName)
        {
            using var content = new MultipartFormDataContent();
            var fileContent = new StreamContent(fileStream);
            fileContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
            content.Add(fileContent, "file", fileName);
            return await _httpClient.PostAsync($"{_apiBaseUrl}Events/upload", content);
        }

        public async Task<HttpResponseMessage> CreateEventAsync(object eventData)
        {
            var content = new StringContent(JsonSerializer.Serialize(eventData), Encoding.UTF8, "application/json");
            return await _httpClient.PostAsync($"{_apiBaseUrl}Events", content);
        }
    }
}