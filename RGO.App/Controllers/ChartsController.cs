using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace RGO.App.Controllers
{
    [Route("/chart/")]
    [ApiController]
    public class ChartsController : ControllerBase
    {
        public readonly IChartRepository
        public ChartsController() { }

        [HttpPost("create")]
        public async Task<IActionResult> RegisterEmployee([FromBody] ChartDto newEmployee)
        {
            try
            {
              
                var chart = await _authService.RegisterEmployee(newEmployee);

                return Ok(token);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
