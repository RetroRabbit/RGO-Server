using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RGO.Models;
using RGO.Services.Interfaces;
using System;
using System.Threading.Tasks;
namespace RGO.App.Controllers
{
    [Route("employeedocument")]
    [ApiController]
    //[Authorize(Policy = "AdminOrSuperAdminPolicy")]
    public class EmployeeDocumentController : ControllerBase
    {
        private readonly IEmployeeDocumentService _employeeDocumentService;
        public EmployeeDocumentController(IEmployeeDocumentService employeeDocumentService)
        {
            _employeeDocumentService = employeeDocumentService;
        }

        [Authorize(Policy = "AllRolesPolicy")]
        [HttpGet("all")]
        public async Task<IActionResult> GetAllEmployeeDocuments(int employeeId)
        {
            try
            {
                var employeeDocuments = await _employeeDocumentService.GetAllEmployeeDocuments(employeeId);
                return Ok(employeeDocuments);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while fetching employee documents.");
            }
        }

        [Authorize(Policy = "AllRolesPolicy")]
        [HttpPost("save")]
        public async Task<IActionResult> Save([FromBody] SimpleEmployeeDocumentDto employeeDocumentDto)
        {
            try
            {
                var newEmployeeDocument = await _employeeDocumentService.SaveEmployeeDocument(employeeDocumentDto);
                return Ok(newEmployeeDocument);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while saving the employee document.");
            }
        }

        [HttpGet("get/{employeeId}/{filename}")]
        public async Task<IActionResult> GetEmployeeDocument(int employeeId, string filename)
        {
            try
            {
                var employeeDocument = await _employeeDocumentService.GetEmployeeDocument(employeeId, filename);
                return Ok(employeeDocument);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while fetching the employee document.");
            }
        }

        [Authorize(Policy = "AllRolesPolicy")]
        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] EmployeeDocumentDto employeeDocumentDto)
        {
            try
            {
                var updatedEmployeeDocument = await _employeeDocumentService.UpdateEmployeeDocument(employeeDocumentDto);
                return Ok(updatedEmployeeDocument);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while updating the employee document.");
            }
        }

        [Authorize(Policy = "AllRolesPolicy")]
        [HttpDelete("delete")]
        public async Task<IActionResult> Delete([FromBody] EmployeeDocumentDto employeeDocumentDto)
        {
            try
            {
                var deletedEmployeeDocument = await _employeeDocumentService.DeleteEmployeeDocument(employeeDocumentDto);
                return Ok(deletedEmployeeDocument);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while deleting the employee document.");
            }
        }
    }
}