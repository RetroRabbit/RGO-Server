using HRIS.Models;
using HRIS.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RR.App.Controllers.HRIS;

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

    [Authorize(Policy = "AdminOrTalentOrJourneyOrSuperAdminPolicy")]
    [HttpPost("all")]
    public async Task<IActionResult> GetAll([FromBody] EmployeeEvaluationInput evaluationInput)
    {
        try
        {
            var getEmployeeEvaluationAudiences = await _employeeEvaluationAudienceService
                .GetAllbyEvaluation(evaluationInput);

            return Ok(getEmployeeEvaluationAudiences);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [Authorize(Policy = "AdminOrTalentOrJourneyOrSuperAdminPolicy")]
    [HttpPost]
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

    [Authorize(Policy = "AdminOrTalentOrJourneyOrSuperAdminPolicy")]
    [HttpDelete]
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