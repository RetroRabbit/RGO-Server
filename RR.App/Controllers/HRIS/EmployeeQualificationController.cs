using Microsoft.AspNetCore.Mvc;
using HRIS.Services.Interfaces;
using HRIS.Models;
using Microsoft.AspNetCore.Authorization;

namespace RR.App.Controllers.HRIS
{
    [Route("employee-qualifications")]
    [ApiController]
    public class EmployeeQualificationController : ControllerBase
    {
        private readonly IEmployeeQualificationService _employeeQualificationService;

        public EmployeeQualificationController(IEmployeeQualificationService employeeQualificationService)
        {
            _employeeQualificationService = employeeQualificationService;
        }

        [Authorize(Policy = "AllRolesPolicy")]
        [HttpPost]
        public async Task<IActionResult> Save([FromBody] EmployeeQualificationDto employeeQualificationDto)
        {
            try
            {
                var newEmployeeQualification = await _employeeQualificationService.SaveEmployeeQualification(employeeQualificationDto);
                return Ok(newEmployeeQualification);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while saving the employee qualification.");
            }
        }

        [Authorize(Policy = "AllRolesPolicy")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployeeQualification(int id)
        {
            try
            {
                var employeeQualification = await _employeeQualificationService.GetEmployeeQualification(id);
                return Ok(employeeQualification);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while fetching the employee qualification.");
            }
        }

        [Authorize(Policy = "AllRolesPolicy")]
        [HttpGet]
        public async Task<IActionResult> GetAllEmployeeQualifications()
        {
            try
            {
                var employeeQualifications = await _employeeQualificationService.GetAllEmployeeQualifications();
                return Ok(employeeQualifications);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while fetching all employee qualifications.");
            }
        }

        [Authorize(Policy = "AllRolesPolicy")]
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] EmployeeQualificationDto employeeQualificationDto)
        {
            try
            {
                var updatedEmployeeQualification = await _employeeQualificationService.UpdateEmployeeQualification(employeeQualificationDto);
                return Ok(updatedEmployeeQualification);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while updating the employee qualification.");
            }
        }

        [Authorize(Policy = "AllRolesPolicy")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var deletedEmployeeQualification = await _employeeQualificationService.DeleteEmployeeQualification(id);
                return Ok(deletedEmployeeQualification);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while deleting the employee qualification.");
            }
        }
    }
}
