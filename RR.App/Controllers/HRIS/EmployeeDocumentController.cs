using HRIS.Models;
using HRIS.Models.Enums;
using HRIS.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
    [HttpGet("all/{employeeId}/{documentType}")]
    public async Task<IActionResult> GetAllEmployeeDocuments(int employeeId, int documentType)
    {
        try
        {
                var employeeDocuments = await _employeeDocumentService.GetAllEmployeeDocuments(employeeId, (DocumentType)documentType);
                return Ok(employeeDocuments);
        }
        catch (Exception)
        {
            return StatusCode(500, "An error occurred while fetching employee documents.");
        }
    }

    [Authorize(Policy = "AllRolesPolicy")]
    [HttpPost("{documentType}")]
    public async Task<IActionResult> Save([FromBody] SimpleEmployeeDocumentDto employeeDocumentDto, int documentType)
    {
        try
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var newEmployeeDocument = await _employeeDocumentService.SaveEmployeeDocument(employeeDocumentDto, claimsIdentity?.FindFirst(ClaimTypes.Email)?.Value!, documentType);
            return Ok(newEmployeeDocument);
        }
        catch (Exception)
        {
            return StatusCode(500, "An error occurred while saving the employee document.");
        }
    }

    [Authorize(Policy = "AllRolesPolicy")]
    [HttpPost("additional-document/{documentType}")]
    public async Task<IActionResult> addNewAdditionalDocument([FromBody] SimpleEmployeeDocumentDto employeeDocumentDto, int documentType)
    {
        try
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var newEmployeeDocument = await _employeeDocumentService.SaveEmployeeDocument(employeeDocumentDto, claimsIdentity?.FindFirst(ClaimTypes.Email)?.Value!, documentType);
            return Ok(newEmployeeDocument);
        }
        catch (Exception)
        {
            return StatusCode(500, "An error occurred while saving the employee document.");
        }
    }

    [Authorize(Policy = "AdminOrEmployeePolicy")]
    [HttpGet("{employeeId}/{filename}/{documentType}")]
    public async Task<IActionResult> GetEmployeeDocument(int employeeId, string filename, int documentType)
    {
        try
        {
            var employeeDocument = await _employeeDocumentService.GetEmployeeDocument(employeeId, filename, (DocumentType)documentType);
            return Ok(employeeDocument);
        }
        catch (Exception)
        {
            return StatusCode(500, "An error occurred while fetching the employee document.");
        }
    }

    [Authorize(Policy = "AllRolesPolicy")]
    [HttpPut()]
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
    [HttpDelete("{documentId}")]
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

    [Authorize(Policy = "AdminOrEmployeePolicy")]
    [HttpGet()]
    public async Task<IActionResult> GetAllDocuments()
    {
        try
        {
            var employeeDocuments = await _employeeDocumentService.GetAllDocuments();
            return Ok(employeeDocuments);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "An error occurred while fetching the employee documents.");
        }
    }
}