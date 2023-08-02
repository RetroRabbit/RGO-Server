using Microsoft.AspNetCore.Mvc;
using RGO.Domain.Interfaces.Services;
using RGO.Domain.Models;
using RGO.Domain.Services;

namespace RGO_Backend.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly IEventsService _eventsService;
        public EventsController(IEventsService eventsService) 
        {
            _eventsService = eventsService;
        }

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
}
