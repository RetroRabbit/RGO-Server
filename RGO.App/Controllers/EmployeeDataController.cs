using Microsoft.AspNetCore.Mvc;
using RGO.Models;
using RGO.Services.Interfaces;

namespace RGO.App.Controllers
{
    [Route("/employeedata/")]
    [ApiController]
    public class EmployeeDataController : Controller
    {
        private readonly IEmployeeDataService _employeeDataService;
      
        public EmployeeDataController(IEmployeeDataService employeeDataService)
        {
            _employeeDataService = employeeDataService;
        }

        [HttpGet("get")]
        public async Task<IActionResult> GetEmployeeData(int id)
        {
            try
            {
                var getEmployeeData = await _employeeDataService.GetAllEmployeeData(id) ;
                if (getEmployeeData == null) throw new Exception("Employee data not found");

                return Ok(getEmployeeData);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost("save")]
        public async Task<IActionResult> SaveEmployeeData(EmployeeDataDto employeeDataDto)
        {
            try
            {
                var saveEmployeeData = await _employeeDataService.SaveEmployeeData(employeeDataDto);
                if (saveEmployeeData == null) throw new Exception("Employee data not saved");

                return Ok(saveEmployeeData);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateEmployeeData(EmployeeDataDto employeeDataDto)
        {
            try
            {
                var saveEmployeeData = await _employeeDataService.UpdateEmployeeData(employeeDataDto);
                if (saveEmployeeData == null) throw new Exception("Employee data not updated");

                return Ok(saveEmployeeData);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteEmployeeData(EmployeeDataDto employeeDataDto)
        {
            try
            {
                var deletedEmployeeData = await _employeeDataService.DeleteEmployeeData(employeeDataDto);
                return Ok(deletedEmployeeData);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

    }
}
