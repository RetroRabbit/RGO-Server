using Microsoft.AspNetCore.Mvc;
using RGO.Domain.Interfaces.Services;

namespace RGO.App.Controllers
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
            var events = await _eventsService.GetEvents();

            return Ok(events);

        }
    }
}
