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
    public async Task<IActionResult> GetAllEmployeeEvaluations(
        [FromQuery] string email)
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

    [HttpGet("get")]
    public async Task<IActionResult> GetEmployeeEvaluation(
        [FromQuery] string employeeEmail,
        [FromQuery] string ownerEmail,
        [FromQuery] string template,
        [FromQuery] string subject)
    {
        try
        {
            var getEmployeeEvaluation = await _employeeEvaluationService.GetEmployeeEvaluation(employeeEmail, ownerEmail, template, subject);

            return Ok(getEmployeeEvaluation);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPost("save")]
    public async Task<IActionResult> SaveEmployeeEvaluation(
        [FromBody] EmployeeEvaluationInput evaluationInput)
    {
        try
        {
            var savedEmployeeEvaluation = await _employeeEvaluationService.SaveEmployeeEvaluation(evaluationInput);
            return Ok(savedEmployeeEvaluation);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPut("update")]
    public async Task<IActionResult> UpdateEmployeeEvaluation(
        [FromBody] List<EmployeeEvaluationInput> evaluation)
    {
        try
        {
            if (evaluation.Count != 2) throw new Exception("Invalid input");

            await _employeeEvaluationService.UpdateEmployeeEvaluation(evaluation[0], evaluation[1]);
            return Ok();
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpDelete("delete")]
    public async Task<IActionResult> DeleteEmployeeEvaluation(
        [FromBody] EmployeeEvaluationInput evaluationInput)
    {
        try
        {
            await _employeeEvaluationService.DeleteEmployeeEvaluation(evaluationInput);
            return Ok();
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }
}

