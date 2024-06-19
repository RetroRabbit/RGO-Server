using HRIS.Models;
using HRIS.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RR.UnitOfWork;
using System.Security.Claims;

namespace RR.App.Controllers.HRIS;

[Route("employee-data")]
[ApiController]
public class EmployeeDataController : Controller
{
    private readonly IEmployeeDataService _employeeDataService;

    public EmployeeDataController(IEmployeeDataService employeeDataService)
    {
        _employeeDataService = employeeDataService;
    }

    [Authorize(Policy = "AllRolesPolicy")]
    [HttpGet]
    public async Task<IActionResult> GetEmployeeData([FromQuery] int id)
    {
        try
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            var role = claimsIdentity?.FindFirst(ClaimTypes.Role)?.Value!;
            if ("SuperAdmin" == role || "Admin" == role || "Talent" == role || "Journey" == role)
            {
                var getEmployeeData = await _employeeDataService.GetAllEmployeeData(id);
                if (getEmployeeData == null) throw new Exception("Employee data not found");
                return Ok(getEmployeeData);
            }
            var authEmail = claimsIdentity?.FindFirst(ClaimTypes.Email)?.Value!;
            var userId = GlobalVariables.GetUserId();
            if (id == userId)
            {
                var getEmployeeData = await _employeeDataService.GetAllEmployeeData(id);
                if (getEmployeeData == null) throw new Exception("Employee data not found");
                return Ok(getEmployeeData);
            }
            return NotFound("User data being accessed does not match user making the request.");
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [Authorize(Policy = "AllRolesPolicy")]
    [HttpPost]
    public async Task<IActionResult> SaveEmployeeData([FromBody] EmployeeDataDto employeeDataDto)
    {
        try
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            var role = claimsIdentity?.FindFirst(ClaimTypes.Role)?.Value!;
            if ("SuperAdmin" == role || "Admin" == role || "Talent" == role || "Journey" == role)
            {
                var saveEmployeeData = await _employeeDataService.SaveEmployeeData(employeeDataDto);
                if (saveEmployeeData == null) throw new Exception("Employee data not saved");
                return Ok(saveEmployeeData);
            }

            var userId = GlobalVariables.GetUserId();
            if (employeeDataDto.EmployeeId == userId)
            {
                var saveEmployeeData = await _employeeDataService.SaveEmployeeData(employeeDataDto);
                if (saveEmployeeData == null) throw new Exception("Employee data not saved");
                return Ok(saveEmployeeData);
            }
            return NotFound("User data being accessed does not match user making the request.");
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [Authorize(Policy = "AllRolesPolicy")]
    [HttpPut]
    public async Task<IActionResult> UpdateEmployeeData([FromBody] EmployeeDataDto employeeDataDto)
    {
        try
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            var role = claimsIdentity?.FindFirst(ClaimTypes.Role)?.Value!;
            if ("SuperAdmin" == role || "Admin" == role || "Talent" == role || "Journey" == role)
            {
                var saveEmployeeData = await _employeeDataService.UpdateEmployeeData(employeeDataDto);
                if (saveEmployeeData == null) throw new Exception("Employee data not updated");
                return Ok(saveEmployeeData);
            }

            var userId = GlobalVariables.GetUserId();
            if (employeeDataDto.EmployeeId == userId)
            {
                var saveEmployeeData = await _employeeDataService.UpdateEmployeeData(employeeDataDto);
                if (saveEmployeeData == null) throw new Exception("Employee data not updated");
                return Ok(saveEmployeeData);
            }
            return NotFound("User data being accessed does not match user making the request.");
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [Authorize(Policy = "AdminOrTalentOrJourneyOrSuperAdminPolicy")]
    [HttpDelete]
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