using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay.API.Data;
using RaceDay.API.Models;

namespace RaceDay.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ResultsController : ControllerBase
{
    private readonly RaceDayContext _context;
    public ResultsController(RaceDayContext context) => _context = context;

    [HttpPost]
    public async Task<IActionResult> CaptureResult(int enrolmentId, TimeSpan finishTime, int position)
    {
        if (HttpContext.Session.GetString("UserRole") != "Organiser")
            return Unauthorized();

        var result = new Result { EnrolmentId = enrolmentId, FinishTime = finishTime, Position = position };
        _context.Results.Add(result);
        await _context.SaveChangesAsync();
        return Ok("Result captured.");
    }

    [HttpGet("my-results")]
    public async Task<IActionResult> GetMyResults()
    {
        if (HttpContext.Session.GetString("UserRole") != "Participant")
            return Unauthorized();

        var userId = HttpContext.Session.GetInt32("UserId");
        if (userId == null) return Unauthorized();

        var results = await _context.Results
            .Include(r => r.Enrolment)
            .ThenInclude(e => e.Event)
            .Where(r => r.Enrolment.UserId == userId)
            .ToListAsync();

        return Ok(results);
    }
}