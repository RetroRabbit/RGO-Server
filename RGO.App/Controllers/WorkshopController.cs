using Microsoft.AspNetCore.Mvc;
using RGO.Domain.Interfaces.Services;

namespace RGO.App.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class WorkshopController : ControllerBase
    {
        private readonly IWorkshopService _workshopService;

        public WorkshopController(IWorkshopService workshopService)
        {
            _workshopService = workshopService;
        }

        [HttpGet("workshops")]
        public async Task<IActionResult> GetWorkShops()
        {
            var workshops = await _workshopService.GetWorkshops();
            return Ok(workshops);
        }
    }
}
