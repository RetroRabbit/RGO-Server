using HRIS.Models.Employee.Commons;
using HRIS.Services.Interfaces;
using HRIS.Services.Services;
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
        if (!_identity.IsSupport && id != _identity.EmployeeId)
            throw new CustomException("User data being accessed does not match user making the request.");

        var data = await _employeeDataService.GetEmployeeData(id);
        return Ok(data);
    }

    [Authorize(Policy = "AllRolesPolicy")]
    [HttpPost]
    public async Task<IActionResult> CreateEmployeeData([FromBody] EmployeeDataDto employeeDataDto)
    {
        if (!_identity.IsSupport && employeeDataDto.EmployeeId != _identity.EmployeeId)
            throw new CustomException("User data being accessed does not match user making the request.");

        var data = await _employeeDataService.CreateEmployeeData(employeeDataDto);
        return Ok(data);
    }

    [Authorize(Policy = "AllRolesPolicy")]
    [HttpPut]
    public async Task<IActionResult> UpdateEmployeeData([FromBody] EmployeeDataDto employeeDataDto)
    {
        if (!_identity.IsSupport && employeeDataDto.EmployeeId != _identity.EmployeeId)
            throw new CustomException("User data being accessed does not match user making the request.");

        var data = await _employeeDataService.UpdateEmployeeData(employeeDataDto);
        return Ok(data);
    }

    [Authorize(Policy = "AdminOrTalentOrJourneyOrSuperAdminPolicy")]
    [HttpDelete]
    public async Task<IActionResult> DeleteEmployeeData(int employeeDataId)
    {
        if (!_identity.IsSupport)
            throw new CustomException("Unauthorized Access.");

        var data = await _employeeDataService.DeleteEmployeeData(employeeDataId);
        return Ok(data);
    }
}
