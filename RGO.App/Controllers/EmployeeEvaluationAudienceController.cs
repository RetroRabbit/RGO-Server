using Microsoft.AspNetCore.Mvc;
using RGO.Models;
using RGO.Services.Interfaces;

namespace RGO.App.Controllers;

[Route("evaluation-audience")]
[ApiController]
public class EmployeeEvaluationAudienceController : ControllerBase
{
    private readonly IEmployeeEvaluationAudienceService _employeeEvaluationAudienceService;


    public EmployeeEvaluationAudienceController(
        IEmployeeEvaluationAudienceService employeeEvaluationAudienceService)
    {
        _employeeEvaluationAudienceService = employeeEvaluationAudienceService;
    }

    [HttpPost()]
    public async Task<IActionResult> GetAll([FromBody] EmployeeEvaluationInput evaluationInput)
    {
        try
        {
            List<EmployeeEvaluationAudienceDto> getEmployeeEvaluationAudiences = await _employeeEvaluationAudienceService
                .GetAllbyEvaluation(evaluationInput);

            return Ok(getEmployeeEvaluationAudiences);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPost()]
    public async Task<IActionResult> SaveEmployeeEvaluationAudience(
        [FromQuery] string email,
        [FromBody] EmployeeEvaluationInput evaluationInput)
    {
        try
        {
            var savedEmployeeEvaluationAudience = await _employeeEvaluationAudienceService.Save(email, evaluationInput);

            return Ok(savedEmployeeEvaluationAudience);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpDelete()]
    public async Task<IActionResult> DeleteEmployeeEvaluationAudience(
        [FromQuery] string email,
        [FromBody] EmployeeEvaluationInput evaluation)
    {
        try
        {
            await _employeeEvaluationAudienceService.Delete(email, evaluation);

            return Ok();
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }
}
