using HRIS.Models;
using HRIS.Models.Enums;
using HRIS.Services.Interfaces;
using HRIS.Services.Session;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace RR.App.Controllers.HRIS;

[Route("employee-documents")]
[ApiController]
public class EmployeeDocumentController : ControllerBase
{
    private readonly AuthorizeIdentity _identity;
    private readonly IEmployeeDocumentService _employeeDocumentService;
   
    public EmployeeDocumentController(AuthorizeIdentity identity, IEmployeeDocumentService employeeDocumentService)
    {
        _identity = identity;
        _employeeDocumentService = employeeDocumentService;
    }

    [Authorize(Policy = "AllRolesPolicy")]
    [HttpGet("all/{employeeId}/{documentType}")]
    public async Task<IActionResult> GetEmployeeDocuments(int employeeId, int documentType)
    {
        var employeeDocuments = await _employeeDocumentService.GetEmployeeDocuments(employeeId, (DocumentType)documentType);
        return Ok(employeeDocuments);
    }

    [Authorize(Policy = "AllRolesPolicy")]
    [HttpPost("{documentType}")]
    public async Task<IActionResult> Save([FromBody] SimpleEmployeeDocumentDto employeeDocumentDto, int documentType)
    {
        var newEmployeeDocument = await _employeeDocumentService.SaveEmployeeDocument(employeeDocumentDto, _identity.Email, documentType);
        return Ok(newEmployeeDocument);
    }

    [Authorize(Policy = "AdminOrEmployeePolicy")]
    [HttpGet("{employeeId}/{filename}/{documentType}")]
    public async Task<IActionResult> GetEmployeeDocument(int employeeId, string filename, int documentType)
    {
        var employeeDocument = await _employeeDocumentService.GetEmployeeDocument(employeeId, filename, (DocumentType)documentType);
        return Ok(employeeDocument);
    }

    [Authorize(Policy = "AllRolesPolicy")]
    [HttpPut()]
    public async Task<IActionResult> Update([FromBody] EmployeeDocumentDto employeeDocumentDto)
    {
        var updatedEmployeeDocument = await _employeeDocumentService.UpdateEmployeeDocument(employeeDocumentDto, _identity.Email);
        return Ok(updatedEmployeeDocument);
    }

    [Authorize(Policy = "AllRolesPolicy")]
    [HttpDelete("{documentId}")]
    public async Task<IActionResult> Delete(int documentId)
    {
        var deletedEmployeeDocument = await _employeeDocumentService.DeleteEmployeeDocument(documentId);
        return Ok(deletedEmployeeDocument);
    }

    [Authorize(Policy = "AdminOrEmployeePolicy")]
    [HttpGet("{employeeId}/{status}")]
    public async Task<IActionResult> GetEmployeeDocumentsByStatus(int employeeId, DocumentStatus status)
    {
        var employeeDocuments = await _employeeDocumentService.GetEmployeeDocumentsByStatus(employeeId, status);
        return Ok(employeeDocuments);
    }

    [Authorize(Policy = "AdminOrEmployeePolicy")]
    [HttpGet()]
    public async Task<IActionResult> GetAllDocuments()
    {
        var employeeDocuments = await _employeeDocumentService.GetAllDocuments();
        return Ok(employeeDocuments);
    }
}