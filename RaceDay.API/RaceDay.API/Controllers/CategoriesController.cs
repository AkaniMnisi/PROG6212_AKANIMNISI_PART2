using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay.API.Data;
using RaceDay.API.Models;

namespace RaceDay.API.Controllers;

[ApiController]
[Route("api/events/{eventId}/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly RaceDayContext _context;
    public CategoriesController(RaceDayContext context) => _context = context;

    [HttpGet]
    public async Task<IActionResult> GetCategories(int eventId)
    {
        return Ok(await _context.Categories.Where(c => c.EventId == eventId).ToListAsync());
    }

    [HttpPost]
    public async Task<IActionResult> CreateCategory(int eventId, Category category)
    {
        if (HttpContext.Session.GetString("UserRole") != "Organiser")
            return Unauthorized("Only Organisers can add categories.");

        category.EventId = eventId;
        _context.Categories.Add(category);
        await _context.SaveChangesAsync();
        return Ok(category);
    }
}