using Microsoft.AspNetCore.Mvc;
using RGO.Models;
using RGO.Services.Interfaces;

namespace RGO.App.Controllers;

[Route("/employeeevaluationaudience/")]
[ApiController]
public class EmployeeEvaluationAudienceController : ControllerBase
{
    private readonly IEmployeeEvaluationAudienceService _employeeEvaluationAudienceService;

    public EmployeeEvaluationAudienceController(IEmployeeEvaluationAudienceService employeeEvaluationAudienceService)
    {
        _employeeEvaluationAudienceService = employeeEvaluationAudienceService;
    }

    [HttpGet("get")]
    public async Task<IActionResult> GetEmployeeEvaluationAudience([FromQuery] string email, [FromBody] EmployeeEvaluationDto evaluation)
    {
        try
        {
            var getEmployeeEvaluationAudience = await _employeeEvaluationAudienceService.Get(evaluation, email);

            return Ok(getEmployeeEvaluationAudience);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet("getall")]
    public async Task<IActionResult> GetAllEmployeeEvaluationAudiences()
    {
        try
        {
            var getEmployeeEvaluationAudiences = await _employeeEvaluationAudienceService.GetAll();

            return Ok(getEmployeeEvaluationAudiences);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet("getall")]
    public async Task<IActionResult> GetAllEmployeeEvaluationAudiencesByEmployee([FromQuery] string email)
    {
        try
        {
            var getEmployeeEvaluationAudiences = await _employeeEvaluationAudienceService.GetAllbyEmployee(email);

            return Ok(getEmployeeEvaluationAudiences);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet("getall")]
    public async Task<IActionResult> GetAllEmployeeEvaluationAudienceByEvaluation([FromBody] EmployeeEvaluationDto evaluation)
    {
        try
        {
            var getEmployeeEvaluationAudiences = await _employeeEvaluationAudienceService.GetAllbyEvaluation(evaluation);

            return Ok(getEmployeeEvaluationAudiences);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPost("save")]
    public async Task<IActionResult> SaveEmployeeEvaluationAudience([FromBody] EmployeeEvaluationAudienceDto employeeEvaluationAudienceDto)
    {
        try
        {
            var savedEmployeeEvaluationAudience = await _employeeEvaluationAudienceService.Save(employeeEvaluationAudienceDto);

            return Ok(savedEmployeeEvaluationAudience);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPut("update")]
    public async Task<IActionResult> UpdateEmployeeEvaluationAudience([FromBody] EmployeeEvaluationAudienceDto employeeEvaluationAudienceDto)
    {
        try
        {
            await _employeeEvaluationAudienceService.Update(employeeEvaluationAudienceDto);

            return Ok();
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpDelete("delete")]
    public async Task<IActionResult> DeleteEmployeeEvaluationAudience([FromQuery] string email, [FromBody] EmployeeEvaluationDto evaluation)
    {
        try
        {
            await _employeeEvaluationAudienceService.Delete(evaluation, email);

            return Ok();
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }
}
