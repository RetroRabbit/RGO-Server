using Microsoft.AspNetCore.Mvc;
using RGO.Models;
using RGO.Services.Interfaces;

namespace RGO.App.Controllers
{
    [Route("employee-data")]
    [ApiController]
    public class EmployeeDataController : Controller
    {
        private readonly IEmployeeDataService _employeeDataService;
      
        public EmployeeDataController(IEmployeeDataService employeeDataService)
        {
            _employeeDataService = employeeDataService;
        }

        [HttpGet()]
        public async Task<IActionResult> GetEmployeeData([FromQuery] int id)
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

        [HttpPost()]
        public async Task<IActionResult> SaveEmployeeData([FromBody] EmployeeDataDto employeeDataDto)
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

        [HttpPut()]
        public async Task<IActionResult> UpdateEmployeeData([FromBody] EmployeeDataDto employeeDataDto)
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

        [HttpDelete()]
        public async Task<IActionResult> DeleteEmployeeData(int employeeDataId)
        {
            try
            {
                var deletedEmployeeData = await _employeeDataService.DeleteEmployeeData(employeeDataId);
                return Ok(deletedEmployeeData);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

    }
}
