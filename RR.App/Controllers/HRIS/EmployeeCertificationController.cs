using HRIS.Models;
using HRIS.Services.Interfaces;
using HRIS.Services.Services;
using HRIS.Services.Session;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RR.App.Controllers.HRIS;

[Route("certification")]
[ApiController]
public class EmployeeCertificationController : ControllerBase
{
    private readonly AuthorizeIdentity _identity;
    private IEmployeeCertificationService _employeeCertificationService;

    public EmployeeCertificationController(AuthorizeIdentity identity, IEmployeeCertificationService employeeCertificationService)
    {
        _identity = identity;
        _employeeCertificationService = employeeCertificationService;
    }

    [Authorize(Policy = "AllRolesPolicy")]
    [HttpGet]
    public async Task<IActionResult> GetAllEmployeelCertiificates(int employeeId)
    {
        if ((_identity.Role is not ("SuperAdmin" or "Admin" or "Talent" or "Journey")) && employeeId != _identity.EmployeeId)
        {
            throw new CustomException("User data being accessed does not match user making the request.");
        }
         
        var certificates = await _employeeCertificationService.GetAllEmployeeCertifications(employeeId);
        return Ok(certificates);

    }

    [Authorize(Policy = "AdminOrSuperAdminPolicy")]
    [HttpPost]
    public async Task<IActionResult> SaveEmployeeCertificate(EmployeeCertificationDto employeeCertificationDto)
    { 
        var certificate = await _employeeCertificationService.SaveEmployeeCertification(employeeCertificationDto);
        return Ok(certificate);
    }

    [Authorize(Policy = "AdminOrSuperAdminPolicy")]
    [HttpGet("employee-certificate")]
    public async Task<IActionResult> GetEmployeeCertificate(int employeeId, int certificationId)
    {
        if ((_identity.Role is not ("SuperAdmin" or "Admin" or "Talent" or "Journey")) && employeeId != _identity.EmployeeId)
        {
            throw new CustomException("User data being accessed does not match user making the request.");
        }
         
        var certificate = await _employeeCertificationService.GetEmployeeCertification(employeeId, certificationId);
        return Ok(certificate);

    }

    [Authorize(Policy = "AdminOrSuperAdminPolicy")]
    [HttpDelete]
    public async Task<IActionResult> DeleteEmployeeCertificate(int id)
{
        var certificate = await _employeeCertificationService.DeleteEmployeeCertification(id);
        return Ok(certificate);
    }

    [Authorize(Policy = "AdminOrSuperAdminPolicy")]
    [HttpPut]
    public async Task<IActionResult> UpdateCertificate(EmployeeCertificationDto employeeCertificationDto)
    {
        var certificate = await _employeeCertificationService.UpdateEmployeeCertification(employeeCertificationDto);
        return Ok(certificate);
    }
}
