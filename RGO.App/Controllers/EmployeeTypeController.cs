using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RGO.Models;
using RGO.Services.Interfaces;
using RGO.Services.Services;

namespace RGO.App.Controllers
{
    [Route("/employeetype/")]
    [ApiController]
    public class EmployeeTypeController : ControllerBase
    {
        private readonly IEmployeeTypeService _employeeTypeService;

        public EmployeeTypeController(IEmployeeTypeService employeeTypeService)
        {
            _employeeTypeService = employeeTypeService;
        }

        [Authorize(Policy = "AdminPolicy")]
        [HttpGet("types")]
        public async Task<IActionResult> GetAllEmployees()
        {
            try
            {
                var employeeTypes = await _employeeTypeService.GetAllEmployeeType();

                return Ok(employeeTypes);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
