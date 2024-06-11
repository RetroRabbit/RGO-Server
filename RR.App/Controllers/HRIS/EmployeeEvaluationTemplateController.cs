using HRIS.Models;
using HRIS.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RR.App.Controllers.HRIS;

[Route("evaluation-template")]
[ApiController]
public class EmployeeEvaluationTemplateController : ControllerBase
{
    private readonly IEmployeeEvaluationTemplateService _employeeEvaluationTemplateService;

    public EmployeeEvaluationTemplateController(IEmployeeEvaluationTemplateService employeeEvaluationTemplateService)
    {
        _employeeEvaluationTemplateService = employeeEvaluationTemplateService;
    }

    [Authorize(Policy = "AdminOrTalentOrJourneyOrSuperAdminPolicy")]
    [HttpGet]
    public async Task<IActionResult> GetEmployeeEvaluationTemplate(string template)
    {
        try
        {
            var getEmployeeEvaluationTemplate = await _employeeEvaluationTemplateService.Get(template);

            return Ok(getEmployeeEvaluationTemplate);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [Authorize(Policy = "AdminOrTalentOrJourneyOrSuperAdminPolicy")]
    [HttpGet("all")]
    public async Task<IActionResult> GetAllEmployeeEvaluationTemplates()
    {
        try
        {
            var getEmployeeEvaluationTemplates = await _employeeEvaluationTemplateService.GetAll();

            return Ok(getEmployeeEvaluationTemplates);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [Authorize(Policy = "AdminOrTalentOrJourneyOrSuperAdminPolicy")]
    [HttpPost]
    public async Task<IActionResult> SaveEmployeeEvaluationTemplate([FromQuery] string template)
    {
        try
        {
            var savedEmployeeEvaluationTemplate = await _employeeEvaluationTemplateService.Save(template);

            return Ok(savedEmployeeEvaluationTemplate);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [Authorize(Policy = "AdminOrTalentOrJourneyOrSuperAdminPolicy")]
    [HttpPut]
    public async Task<IActionResult> UpdateEmployeeEvaluationTemplate(
        [FromBody] EmployeeEvaluationTemplateDto employeeEvaluationTemplateDto)
    {
        try
        {
            await _employeeEvaluationTemplateService.Update(employeeEvaluationTemplateDto);

            return Ok();
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [Authorize(Policy = "AdminOrTalentOrJourneyOrSuperAdminPolicy")]
    [HttpDelete]
    public async Task<IActionResult> DeleteEmployeeEvaluationTemplate([FromQuery] string template)
    {
        try
        {
            await _employeeEvaluationTemplateService.Delete(template);

            return Ok();
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }
}