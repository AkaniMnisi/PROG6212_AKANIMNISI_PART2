using Microsoft.AspNetCore.Mvc;
using RaceDay.MVC.Services;
using System.Text.Json;

namespace RaceDay.MVC.Controllers
{
    public class OrganiserController : Controller
    {
        private readonly ApiService _apiService;

        public OrganiserController(ApiService apiService)
        {
            _apiService = apiService;
        }

        
        public IActionResult Dashboard()
        {
           
            if (HttpContext.Session.GetString("UserRole") != "Organiser")
                return RedirectToAction("Login", "Account");

            return View();
        }

      
        [HttpGet]
        public IActionResult CreateEvent()
        {
            if (HttpContext.Session.GetString("UserRole") != "Organiser")
                return RedirectToAction("Login", "Account");

            return View();
        }

        
        [HttpPost]
        public async Task<IActionResult> CreateEvent(string name, string description, DateTime date, string location, string distance, string eventType, IFormFile imageFile)
        {
            if (HttpContext.Session.GetString("UserRole") != "Organiser")
                return RedirectToAction("Login", "Account");

            string imageUrl = "";

            
            if (imageFile != null && imageFile.Length > 0)
            {
                var response = await _apiService.UploadImageAsync(imageFile.OpenReadStream(), imageFile.FileName);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var result = JsonSerializer.Deserialize<Dictionary<string, string>>(json);
                    imageUrl = result["imageUrl"]; 
                }
            }

            
            var newEvent = new
            {
                Name = name,
                Description = description,
                Date = date,
                Location = location,
                Distance = distance,
                EventType = eventType,
                ImageUrl = imageUrl 
            };

           
            await _apiService.CreateEventAsync(newEvent);

           
            return RedirectToAction("Dashboard");
        }
    }
}