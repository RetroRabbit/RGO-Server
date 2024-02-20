using HRIS.Models;
using HRIS.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace RR.App.Controllers.HRIS;

[Route("evaluation")]
[ApiController]
public class EmployeeEvaluationController : Controller
{
    private readonly IEmployeeEvaluationService _employeeEvaluationService;

    public EmployeeEvaluationController(IEmployeeEvaluationService employeeEvaluationService)
    {
        _employeeEvaluationService = employeeEvaluationService;
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetAllEmployeeEvaluations(
        [FromQuery] string email)
    {
        try
        {
            var getEmployeeEvaluations = await _employeeEvaluationService.GetAllEvaluationsByEmail(email);

            if (getEmployeeEvaluations == null) throw new Exception("No Employee Evaluations found");

            return Ok(getEmployeeEvaluations);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetEmployeeEvaluation(
        [FromQuery] string employeeEmail,
        [FromQuery] string ownerEmail,
        [FromQuery] string template,
        [FromQuery] string subject)
    {
        try
        {
            var getEmployeeEvaluation =
                await _employeeEvaluationService.Get(employeeEmail, ownerEmail, template, subject);

            return Ok(getEmployeeEvaluation);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> SaveEmployeeEvaluation(
        [FromBody] EmployeeEvaluationInput evaluationInput)
    {
        try
        {
            var savedEmployeeEvaluation = await _employeeEvaluationService.Save(evaluationInput);
            return Ok(savedEmployeeEvaluation);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPut]
    public async Task<IActionResult> UpdateEmployeeEvaluation(
        [FromBody] List<EmployeeEvaluationInput> evaluation)
    {
        try
        {
            if (evaluation.Count != 2) throw new Exception("Invalid input");

            await _employeeEvaluationService.Update(evaluation[0], evaluation[1]);
            return Ok();
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteEmployeeEvaluation([FromBody] EmployeeEvaluationInput evaluationInput)
    {
        try
        {
            await _employeeEvaluationService.Delete(evaluationInput);
            return Ok();
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }
}