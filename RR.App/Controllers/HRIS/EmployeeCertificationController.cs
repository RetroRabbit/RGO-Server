using HRIS.Models;
using HRIS.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;

namespace RR.App.Controllers.HRIS;

[Route("certification")]
[ApiController]
public class EmployeeCertificationController : ControllerBase
{
    private IEmployeeCertificationService _employeeCertificationService;
    public EmployeeCertificationController(IEmployeeCertificationService employeeCertificationService)
    {
        _employeeCertificationService = employeeCertificationService;
    }

    [Authorize(Policy = "AdminOrSuperAdminPolicy")]
    [HttpGet]
    public async Task<IActionResult> GetAllEmployeelCertiificates(int employeeId)
    {
        try
        {
            var certificates = await _employeeCertificationService.GetAllEmployeeCertifications(employeeId);
            return Ok(certificates);

        }
        catch(Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [Authorize(Policy = "AdminOrSuperAdminPolicy")]
    [HttpPost]
    public async Task<IActionResult> SaveEmployeeCertificate(EmployeeCertificationDto employeeCertificationDto)
    {
        try
        {
            var certificate = await _employeeCertificationService.SaveEmployeeCertification(employeeCertificationDto);
            return Ok(certificate);

        }
        catch(Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [Authorize(Policy = "AdminOrSuperAdminPolicy")]
    [HttpGet("employee-certificate")]
    public async Task<IActionResult> GetEmployeeCertificate(int employeeId, int certificationId)
    {
        try
        {
            var certificate = await _employeeCertificationService.GetEmployeeCertification(employeeId, certificationId);
            return Ok(certificate);

        }
        catch(Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [Authorize(Policy = "AdminOrSuperAdminPolicy")]
    [HttpDelete]
    public async Task<IActionResult> DeleteEmployeeCertificate(int id)
    {
        try
        {
            var certificate = await _employeeCertificationService.DeleteEmployeeCertification(id);
            return Ok(certificate);

        }
        catch(Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [Authorize(Policy = "AdminOrSuperAdminPolicy")]
    [HttpPut]
    public async Task<IActionResult> UpdateCertificate(EmployeeCertificationDto employeeCertificationDto)
    {
        try
        {
            var certificate = await _employeeCertificationService.UpdateEmployeeCertification(employeeCertificationDto);
            return Ok(certificate);

        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
