using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using RaceDay.MVC.Services;
using System.Text.Json;

namespace RaceDay.MVC.Controllers
{
    public class ParticipantController : Controller
    {
        private readonly ApiService _apiService;

        public ParticipantController(ApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<IActionResult> Index()
        {
            var eventsJson = await _apiService.GetEventsAsync();

            var eventsList = JsonSerializer.Deserialize<List<Dictionary<string, object>>>(eventsJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return View(eventsList);
        }

        [HttpPost]
        public async Task<IActionResult> Enrol(int eventId)
        {
            string userEmail = HttpContext.Session.GetString("UserEmail") ?? "participant@test.com";

            var response = await _apiService.EnrolInEventAsync(eventId, userEmail);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("MyResults");
            }

            TempData["Error"] = "Failed to enrol. Please try again.";
            return RedirectToAction("Index");
        }

        public IActionResult MyResults()
        {
            return View();
        }
    }
}