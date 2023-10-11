using Microsoft.AspNetCore.Mvc;
using RGO.Models;
using RGO.Services.Interfaces;

namespace RGO.App.Controllers;

[Route("/employeedate/")]
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

    [HttpPost("save")]
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

    [HttpDelete("delete")]
    public async Task<IActionResult> DeleteEmployeeDate([FromBody] EmployeeDateInput employeeDateInput)
    {
        try
        {
            var employeeDateDto = new EmployeeDateDto(
                0,
                await _employeeService.GetEmployee(employeeDateInput.Email),
                employeeDateInput.Subject,
                employeeDateInput.Note,
                employeeDateInput.Date);
            await _employeeDateService.Delete(employeeDateDto);

            return Ok();
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPut("update")]
    public async Task<IActionResult> UpdateEmployeeDate([FromBody] EmployeeDateInput employeeDateInput)
    {
        try
        {
            var employeeDateDto = new EmployeeDateDto(
                0,
                await _employeeService.GetEmployee(employeeDateInput.Email),
                employeeDateInput.Subject,
                employeeDateInput.Note,
                employeeDateInput.Date);

            var employeeDateToUpdate = await _employeeDateService.Get(employeeDateDto);

            employeeDateToUpdate = new EmployeeDateDto(
                employeeDateToUpdate.Id,
                employeeDateToUpdate.Employee,
                employeeDateInput.Subject,
                employeeDateInput.Note,
                employeeDateInput.Date);

            await _employeeDateService.Update(employeeDateToUpdate);

            return Ok();
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet("getall")]
    public async Task<IActionResult> GetAllEmployeeDate(
        [FromQuery] DateOnly? date = null,
        [FromQuery] string? email = null,
        [FromQuery] string? subject = null)
    {
        try
        {
            List<EmployeeDateDto> getAllEmployeeDate;

            if (date != null) getAllEmployeeDate = _employeeDateService.GetAllByDate((DateOnly) date);
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
