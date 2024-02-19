using HRIS.Models;
using HRIS.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace RR.App.Controllers.HRIS;

[Route("employee-date")]
[ApiController]
public class EmployeeDateController : ControllerBase
{
    private readonly IEmployeeDateService _employeeDateService;
    private readonly IEmployeeService _employeeService;

    public EmployeeDateController(IEmployeeDateService employeeDateService, IEmployeeService employeeService)
    {
        _employeeDateService = employeeDateService;
        _employeeService = employeeService;
    }

    [HttpPost]
    public async Task<IActionResult> SaveEmployeeDate([FromBody] EmployeeDateInput employeeDateInput)
    {
        try
        {
            var employeeDateDto = new EmployeeDateDto(
                                                      0,
                                                      await _employeeService.GetEmployee(employeeDateInput.Email),
                                                      employeeDateInput.Subject,
                                                      employeeDateInput.Note,
                                                      employeeDateInput.Date);
            await _employeeDateService.Save(employeeDateDto);

            return Ok();
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteEmployeeDate(int employeeDateId)
    {
        try
        {
            await _employeeDateService.Delete(employeeDateId);

            return Ok();
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPut]
    public async Task<IActionResult> UpdateEmployeeDate([FromBody] EmployeeDateDto employeeDate)
    {
        try
        {
            var employeeDateDto = new EmployeeDateDto(
                                                      employeeDate.Id,
                                                      await _employeeService.GetEmployee(employeeDate.Employee!.Email),
                                                      employeeDate.Subject,
                                                      employeeDate.Note,
                                                      employeeDate.Date);

            await _employeeDateService.Update(employeeDateDto);

            return Ok();
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAllEmployeeDate(
        [FromQuery] DateOnly? date = null,
        [FromQuery] string? email = null,
        [FromQuery] string? subject = null)
    {
        try
        {
            List<EmployeeDateDto> getAllEmployeeDate;

            if (date != null) getAllEmployeeDate = _employeeDateService.GetAllByDate((DateOnly)date);
            else if (email != null) getAllEmployeeDate = _employeeDateService.GetAllByEmployee(email);
            else if (subject != null) getAllEmployeeDate = _employeeDateService.GetAllBySubject(subject);
            else getAllEmployeeDate = _employeeDateService.GetAll();

            return Ok(getAllEmployeeDate);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }
}