using Microsoft.AspNetCore.Mvc;
using RGO.Domain.Interfaces.Services;
using RGO.Domain.Models;

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
            /*if (Authorization == "")
            {
                return Unauthorized("User not authorized");
            }*/

            var events = await _eventsService.GetEvents();

            return Ok(events);

        }
    }
}
