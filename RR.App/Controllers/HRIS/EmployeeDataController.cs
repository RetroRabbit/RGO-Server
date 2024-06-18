using HRIS.Models;
using HRIS.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

    [Authorize(Policy = "AdminOrTalentOrJourneyOrSuperAdminPolicy")]
    [HttpGet]
    public async Task<IActionResult> GetEmployeeData([FromQuery] int id)
    {
        try
        {
            var getEmployeeData = await _employeeDataService.GetAllEmployeeData(id);
            if (getEmployeeData == null) throw new Exception("Employee data not found");

            return Ok(getEmployeeData);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [Authorize(Policy = "AdminOrTalentOrJourneyOrSuperAdminPolicy")]
    [HttpPost]
    public async Task<IActionResult> SaveEmployeeData([FromBody] EmployeeDataDto employeeDataDto)
    {
        try
        {
            var saveEmployeeData = await _employeeDataService.SaveEmployeeData(employeeDataDto);
            if (saveEmployeeData == null) throw new Exception("Employee data not saved");

            return Ok(saveEmployeeData);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [Authorize(Policy = "AdminOrTalentOrJourneyOrSuperAdminPolicy")]
    [HttpPut]
    public async Task<IActionResult> UpdateEmployeeData([FromBody] EmployeeDataDto employeeDataDto)
    {
        try
        {
            var saveEmployeeData = await _employeeDataService.UpdateEmployeeData(employeeDataDto);
            if (saveEmployeeData == null) throw new Exception("Employee data not updated");

            return Ok(saveEmployeeData);
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