using Microsoft.AspNetCore.Mvc;
using RGO.Models;
using RGO.Services.Interfaces;

namespace RGO.App.Controllersp;

[Route("/employeeevaluation/")]
[ApiController]
public class EmployeeEvaluationController : Controller
{
    private readonly IEmployeeEvaluationService _employeeEvaluationService;

    public EmployeeEvaluationController(IEmployeeEvaluationService employeeEvaluationService)
    {
        _employeeEvaluationService = employeeEvaluationService;
    }

    [HttpGet("getall")]
    public async Task<IActionResult> GetAllEmployeeEvaluations([FromQuery] string email)
    {
        try
        {
            var getEmployeeEvaluations = await _employeeEvaluationService.GetAllEmployeeEvaluations(email);

            if (getEmployeeEvaluations == null) throw new Exception("No Employee Evaluations found");

            return Ok(getEmployeeEvaluations);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPost("save")]
    public async Task<IActionResult> SaveEmployeeEvaluation([FromBody] EmployeeEvaluationDto employeeEvaluationDto)
    {
        try
        {
            var savedEmployeeEvaluation = await _employeeEvaluationService.SaveEmployeeEvaluation(employeeEvaluationDto);
            return Ok(savedEmployeeEvaluation);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPut("update")]
    public async Task<IActionResult> UpdateEmployeeEvaluation([FromBody] EmployeeEvaluationDto employeeEvaluationDto)
    {
        try
        {
            await _employeeEvaluationService.UpdateEmployeeEvaluation(employeeEvaluationDto);
            return Ok();
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpDelete("delete")]
    public async Task<IActionResult> DeleteEmployeeEvaluation([FromBody] EmployeeEvaluationDto employeeEvaluationDto)
    {
        try
        {
            await _employeeEvaluationService.DeleteEmployeeEvaluationById(employeeEvaluationDto.Id);
            return Ok();
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }
}

