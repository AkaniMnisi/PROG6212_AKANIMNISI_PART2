using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay.API.Data;
using RaceDay.API.Models;

namespace RaceDay.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventsController : ControllerBase
{
    private readonly RaceDayContext _context;
    public EventsController(RaceDayContext context) => _context = context;

    [HttpGet]
    public async Task<IActionResult> GetEvents()
    {
        return Ok(await _context.Events.ToListAsync());
    }

    [HttpPost]
    public async Task<IActionResult> CreateEvent(Event newEvent)
    {
        if (HttpContext.Session.GetString("UserRole") != "Organiser")
            return Unauthorized("Only Organisers can create events.");

        _context.Events.Add(newEvent);
        await _context.SaveChangesAsync();
        return Ok(newEvent);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateEvent(int id, Event updatedEvent)
    {
        if (HttpContext.Session.GetString("UserRole") != "Organiser")
            return Unauthorized();

        var ev = await _context.Events.FindAsync(id);
        if (ev == null) return NotFound();

        ev.Name = updatedEvent.Name;
        ev.Description = updatedEvent.Description;
        ev.Date = updatedEvent.Date;
        ev.Location = updatedEvent.Location;
        ev.EventType = updatedEvent.EventType;

        await _context.SaveChangesAsync();
        return Ok(ev);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEvent(int id)
    {
        if (HttpContext.Session.GetString("UserRole") != "Organiser")
            return Unauthorized();

        var ev = await _context.Events.FindAsync(id);
        if (ev == null) return NotFound();

        _context.Events.Remove(ev);
        await _context.SaveChangesAsync();
        return Ok("Event deleted.");
    }
}