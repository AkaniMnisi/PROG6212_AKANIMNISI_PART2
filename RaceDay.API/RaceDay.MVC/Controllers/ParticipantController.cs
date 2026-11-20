using Microsoft.AspNetCore.Mvc;
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

       
        public IActionResult Enrol(int id)
        {
           
            return RedirectToAction("Index");
        }
    }
}