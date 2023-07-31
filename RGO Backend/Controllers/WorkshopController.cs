using Microsoft.AspNetCore.Mvc;

namespace RGO_Backend.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class WorkshopController : ControllerBase
    {
        [HttpGet("workshops")]
        public async Task<IActionResult> GetWorkShops()
        {
            var workshops = new List<string>() { "Workshop 1", "Workshop 2", "Workshop 3" };
            return Ok(workshops);
        }
    }
}
