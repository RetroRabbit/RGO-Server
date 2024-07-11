using HRIS.Models;
using HRIS.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RR.App.Controllers.HRIS;

[Route("employee-date")]
[ApiController]
public class EmployeeDateController : ControllerBase
{
    private readonly IEmployeeDateService _employeeDateService;
    //private readonly IEmployeeService _employeeService;

    public EmployeeDateController(IEmployeeDateService employeeDateService, IEmployeeService employeeService)
    {
        _employeeDateService = employeeDateService;
        //_employeeService = employeeService;
    }

    [Authorize(Policy = "AdminOrTalentOrJourneyOrSuperAdminPolicy")]
    [HttpPost]
    public async Task<IActionResult> SaveEmployeeDate([FromBody] EmployeeDateInput employeeDateInput)
    {
        //var employee = await _employeeService.GetEmployee(employeeDateInput.Email!);
        //var employeeDateDto = new EmployeeDateDto
        //{
        //    Id = 0,
        //    Employee = employee,
        //    Subject = employeeDateInput.Subject,
        //    Note = employeeDateInput.Note,
        //    Date = employeeDateInput.Date
        //};

        //await _employeeDateService.Save(employeeDateDto);
        //return Ok();
        await _employeeDateService.SaveEmployeeDate(employeeDateInput);
        return Ok();
    }

    [Authorize(Policy = "AdminOrTalentOrJourneyOrSuperAdminPolicy")]
    [HttpDelete]
    public async Task<IActionResult> DeleteEmployeeDate(int employeeDateId)
    {
        await _employeeDateService.Delete(employeeDateId);
        return Ok();
    }

    [Authorize(Policy = "AdminOrTalentOrJourneyOrSuperAdminPolicy")]
    [HttpPut]
    public async Task<IActionResult> UpdateEmployeeDate([FromBody] EmployeeDateDto employeeDate)
    {
        //var employee = await _employeeService.GetEmployee(employeeDate.Employee!.Email!);
        //var employeeDateDto = new EmployeeDateDto
        //{
        //    Id = employeeDate.Id,
        //    Employee = employee,
        //    Subject = employeeDate.Subject,
        //    Note = employeeDate.Note,
        //    Date = employeeDate.Date
        //};

        //await _employeeDateService.Update(employeeDate);
        //return Ok();

        await _employeeDateService.UpdateEmployeeDate(employeeDate);
        return Ok();
    }

    [Authorize(Policy = "AdminOrTalentOrJourneyOrSuperAdminPolicy")]
    [HttpGet]
    public IActionResult GetAllEmployeeDate([FromQuery] DateOnly? date = null, [FromQuery] string? email = null, [FromQuery] string? subject = null)
    {
        var employeeDates = _employeeDateService.GetEmployeeDates(date, email, subject);
        return Ok(employeeDates);
    }
}