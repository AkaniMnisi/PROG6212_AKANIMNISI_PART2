using Microsoft.AspNetCore.Mvc;
using RaceDay.MVC.Services;

namespace RaceDay.MVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApiService _apiService;

        public AccountController(ApiService apiService)
        {
            _apiService = apiService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            var response = await _apiService.LoginAsync(email, password);

            if (response.IsSuccessStatusCode)
            {
                // Parse the response to get the Role (assuming your API returns { "token": "...", "role": "Organiser" })
                var jsonResponse = await response.Content.ReadAsStringAsync();
                // You'll need to deserialize this properly based on your API's exact response

                // Store in Session
                HttpContext.Session.SetString("UserEmail", email);
                HttpContext.Session.SetString("UserRole", "Participant"); // Hardcoded example - replace with actual role from API

                // Redirect based on role
                if (HttpContext.Session.GetString("UserRole") == "Organiser")
                    return RedirectToAction("Dashboard", "Organiser");
                else
                    return RedirectToAction("Index", "Participant");
            }

            ViewBag.Error = "Invalid login credentials.";
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Account");
        }
    }
}