using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RGO.Domain.Interfaces.Services;

namespace RGO.App.Controllers;

[Route("/event/")]
[ApiController]
public class EventsController : ControllerBase
{
    private readonly IEventsService _eventsService;
    public EventsController(IEventsService eventsService)
    {
        _eventsService = eventsService;
    }

    [Authorize]
    [HttpGet("events")]
    public async Task<IActionResult> GetEvents()
    {

        try
        {
            var events = await _eventsService.GetEvents();
            return Ok(events);

        }
        catch (Exception e)
        {

            await Console.Out.WriteLineAsync(e.Message);
            return BadRequest(e.Message);
        }

    }
}