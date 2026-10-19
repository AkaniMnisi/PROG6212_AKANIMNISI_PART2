using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay.API.Data;
using RaceDay.API.Models;

namespace RaceDay.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EnrolmentsController : ControllerBase
{
    private readonly RaceDayContext _context;
    public EnrolmentsController(RaceDayContext context) => _context = context;

    [HttpPost]
    public async Task<IActionResult> Enrol(int eventId, int categoryId)
    {
        if (HttpContext.Session.GetString("UserRole") != "Participant")
            return Unauthorized("Only Participants can enrol.");

        var userId = HttpContext.Session.GetInt32("UserId");
        if (userId == null) return Unauthorized();

        var enrolment = new Enrolment
        {
            UserId = userId.Value,
            EventId = eventId,
            CategoryId = categoryId
        };

        _context.Enrolments.Add(enrolment);
        await _context.SaveChangesAsync();
        return Ok("Successfully enrolled.");
    }

    [HttpGet("event/{eventId}")]
    public async Task<IActionResult> GetEventEnrolments(int eventId)
    {
        if (HttpContext.Session.GetString("UserRole") != "Organiser")
            return Unauthorized();

        var enrolments = await _context.Enrolments
            .Include(e => e.User)
            .Include(e => e.Category)
            .Where(e => e.EventId == eventId)
            .ToListAsync();

        return Ok(enrolments);
    }
}