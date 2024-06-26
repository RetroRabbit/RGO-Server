using HRIS.Models;
using HRIS.Services.Interfaces;
using HRIS.Services.Session;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RR.App.Controllers.HRIS;

[Route("employee-data")]
[ApiController]
public class EmployeeDataController : ControllerBase
{
    private readonly AuthorizeIdentity _identity;
    private readonly IEmployeeDataService _employeeDataService;

    public EmployeeDataController(AuthorizeIdentity identity, IEmployeeDataService employeeDataService)
    {
        _identity = identity;
        _employeeDataService = employeeDataService;
    }

    [Authorize(Policy = "AllRolesPolicy")]
    [HttpGet]
    public async Task<IActionResult> GetEmployeeData([FromQuery] int id)
    {
        try
        {
            if (_identity.Role is "SuperAdmin" or "Admin" or "Talent" or "Journey")
            {
                var getEmployeeData = await _employeeDataService.GetAllEmployeeData(id);
                if (getEmployeeData == null) throw new Exception("Employee data not found");
                return Ok(getEmployeeData);
            }

            if (id == _identity.EmployeeId)
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
            if (_identity.Role is "SuperAdmin" or "Admin" or "Talent" or "Journey")
            {
                var saveEmployeeData = await _employeeDataService.SaveEmployeeData(employeeDataDto);
                if (saveEmployeeData == null) throw new Exception("Employee data not saved");
                return Ok(saveEmployeeData);
            }

            if (employeeDataDto.EmployeeId == _identity.EmployeeId)
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
            if (_identity.Role is "SuperAdmin" or "Admin" or "Talent" or "Journey")
            {
                var saveEmployeeData = await _employeeDataService.UpdateEmployeeData(employeeDataDto);
                if (saveEmployeeData == null) throw new Exception("Employee data not updated");
                return Ok(saveEmployeeData);
            }

            if (employeeDataDto.EmployeeId == _identity.EmployeeId)
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