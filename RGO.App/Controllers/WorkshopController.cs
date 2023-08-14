using Microsoft.AspNetCore.Mvc;
using RGO.Domain.Interfaces.Services;
using RGO.Domain.Services;

namespace RGO.App.Controllers;

[Route("/workshop/")]
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
        try
        {
            
            try
            {
                var workshops = await _workshopService.GetWorkshops();
                return Ok(workshops);

            }
            catch (Exception e)
            {

                await Console.Out.WriteLineAsync(e.Message);
                return BadRequest(e.Message);
            }
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}