using HRIS.Models;
using HRIS.Models.Enums;
using HRIS.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RR.App.Controllers.HRIS;

[Route("employee-documents")]
[ApiController]
public class EmployeeDocumentController : ControllerBase
{
    private readonly IEmployeeDocumentService _employeeDocumentService;

    public EmployeeDocumentController(IEmployeeDocumentService employeeDocumentService)
    {
        _employeeDocumentService = employeeDocumentService;
    }

    [Authorize(Policy = "AllRolesPolicy")]
    [HttpGet("{employeeId}")]
    public async Task<IActionResult> GetAllEmployeeDocuments(int employeeId)
    {
        try
        {
            var employeeDocuments = await _employeeDocumentService.GetAllEmployeeDocuments(employeeId);
            return Ok(employeeDocuments);
        }
        catch (Exception)
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
        catch (Exception)
        {
            return StatusCode(500, "An error occurred while saving the employee document.");
        }
    }

    [Authorize(Policy = "AdminOrEmployeePolicy")]
    [HttpGet("{employeeId}/{filename}")]
    public async Task<IActionResult> GetEmployeeDocument(int employeeId, string filename)
    {
        try
        {
            var employeeDocument = await _employeeDocumentService.GetEmployeeDocument(employeeId, filename);
            return Ok(employeeDocument);
        }
        catch (Exception)
        {
            return StatusCode(500, "An error occurred while fetching the employee document.");
        }
    }

    [Authorize(Policy = "AllRolesPolicy")]
    [HttpPut("{employeeId}")]
    public async Task<IActionResult> Update([FromBody] EmployeeDocumentDto employeeDocumentDto)
    {
        try
        {
            var updatedEmployeeDocument = await _employeeDocumentService.UpdateEmployeeDocument(employeeDocumentDto);
            return Ok(updatedEmployeeDocument);
        }
        catch (Exception)
        {
            return StatusCode(500, "An error occurred while updating the employee document.");
        }
    }

    [Authorize(Policy = "AllRolesPolicy")]
    [HttpDelete]
    public async Task<IActionResult> Delete(int documentId)
    {
        try
        {
            var deletedEmployeeDocument = await _employeeDocumentService.DeleteEmployeeDocument(documentId);
            return Ok(deletedEmployeeDocument);
        }
        catch (Exception)
        {
            return StatusCode(500, "An error occurred while deleting the employee document.");
        }
    }

    [Authorize(Policy = "AdminOrEmployeePolicy")]
    [HttpGet("{employeeId}/{status}")]
    public async Task<IActionResult> GetEmployeeDocumentsByStatus(int employeeId, DocumentStatus status)
    {
        try
        {
            var employeeDocuments = await _employeeDocumentService.GetEmployeeDocumentsByStatus(employeeId, status);
            return Ok(employeeDocuments);
        }
        catch (Exception)
        {
            return StatusCode(500, "An error occurred while fetching the employee documents.");
        }
    }
}