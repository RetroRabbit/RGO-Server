using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RGO.Domain.Interfaces.Services;

namespace RGO.App.Controllers;

[Route("/event/")]
[ApiController]
public class GradEventsController : ControllerBase
{
    private readonly IGradEventsService _gradEventsService;
    public GradEventsController(IGradEventsService gradEventsService)
    {
        _gradEventsService = gradEventsService;
    }

    [Authorize]
    [HttpGet("events")]
    public async Task<IActionResult> GetEvents()
    {

        try
        {
            var events = await _gradEventsService.GetEvents();
            return Ok(events);

        }
        catch (Exception e)
        {

            await Console.Out.WriteLineAsync(e.Message);
            return BadRequest(e.Message);
        }

    }
}