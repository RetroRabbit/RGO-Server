using HRIS.Models;
using HRIS.Services.Interfaces;
using HRIS.Services.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RR.App.Controllers.HRIS
{
    [Route("employee-qualifications")]
    [ApiController]
    public class EmployeeQualificationController : ControllerBase
    {
        private readonly IEmployeeQualificationService _employeeQualificationService;
        private readonly IEmployeeService _employeeService;

        public EmployeeQualificationController(IEmployeeQualificationService employeeQualificationService, IEmployeeService employeeService, IErrorLoggingService errorLoggingService)
        {
            _employeeQualificationService = employeeQualificationService;
            _employeeService = employeeService;
        }

        [Authorize(Policy = "AllRolesPolicy")]
        [HttpPost("Save")]
        public async Task<IActionResult> SaveEmployeeQualification([FromBody] EmployeeQualificationDto employeeQualificationDto, [FromQuery] int employeeId)
        {
            if (employeeQualificationDto == null)
            {
                return BadRequest("Invalid qualification data provided.");
            }

            try
            {
                var newEmployeeQualification = await _employeeQualificationService.SaveEmployeeQualification(employeeQualificationDto, employeeId);
                return Ok(newEmployeeQualification);
            }
            catch (KeyNotFoundException knf)
            {
                return NotFound(knf.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while saving the qualification: " + ex.Message);
            }
        }

        [Authorize(Policy = "AllRolesPolicy")]
        [HttpDelete]
        public async Task<IActionResult> DeleteEmployeeQualification(int id)
        {
            try
            {
                var deletedEmployeeQualification = await _employeeQualificationService.DeleteEmployeeQualification(id);
                if (deletedEmployeeQualification == null)
                {
                    return NotFound($"No qualification found with ID {id}.");
                }
                return Ok(deletedEmployeeQualification);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while deleting the qualification: " + ex.Message);
            }
        }

        [Authorize(Policy = "AllRolesPolicy")]
        [HttpGet("{employeeId}")]
        public async Task<IActionResult> GetAllQualificationsByEmployee(int employeeId)
        {
            try
            {
                var qualifications = await _employeeQualificationService.GetAllEmployeeQualificationsByEmployeeId(employeeId);
                if (qualifications == null || qualifications.Count == 0)
                {
                    return NotFound($"No qualifications found for employee with ID {employeeId}.");
                }
                return Ok(qualifications);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while retrieving qualifications: " + ex.Message);
            }
        }

        [Authorize(Policy = "AllRolesPolicy")]
        [HttpGet("{id}")]
        public async Task<ActionResult<EmployeeQualificationDto>> GetEmployeeQualificationById(int id)
        {
            try
            {
                var qualification = await _employeeQualificationService.GetEmployeeQualificationById(id);
                if (qualification == null)
                {
                    return NotFound();
                }
                return qualification;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Policy = "AllRolesPolicy")]
        [HttpGet("{employeeId}")]
        public async Task<ActionResult<List<EmployeeQualificationDto>>> GetAllEmployeeQualificationsByEmployeeId(int employeeId)
        {
            try
            {
                var qualifications = await _employeeQualificationService.GetAllEmployeeQualificationsByEmployeeId(employeeId);
                if (qualifications == null || qualifications.Count == 0)
                {
                    return NotFound("No qualifications found for this employee.");
                }
                return qualifications;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
