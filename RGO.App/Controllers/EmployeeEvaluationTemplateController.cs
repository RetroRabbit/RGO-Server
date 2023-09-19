using Microsoft.AspNetCore.Mvc;
using RGO.Models;
using RGO.Services.Interfaces;

namespace RGO.App.Controllers;

[Route("/employeeevaluationtemplate/")]
[ApiController]
public class EmployeeEvaluationTemplateController : ControllerBase
{
    private readonly IEmployeeEvaluationTemplateService _employeeEvaluationTemplateService;

    public EmployeeEvaluationTemplateController(IEmployeeEvaluationTemplateService employeeEvaluationTemplateService)
    {
        _employeeEvaluationTemplateService = employeeEvaluationTemplateService;
    }

    [HttpGet("get")]
    public async Task<IActionResult> GetEmployeeEvaluationTemplate(string template)
    {
        try
        {
            var getEmployeeEvaluationTemplate = await _employeeEvaluationTemplateService.GetEmployeeEvaluationTemplate(template);

            return Ok(getEmployeeEvaluationTemplate);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet("getall")]
    public async Task<IActionResult> GetAllEmployeeEvaluationTemplates()
    {
        try
        {
            var getEmployeeEvaluationTemplates = await _employeeEvaluationTemplateService.GetAllEmployeeEvaluationTemplates();

            return Ok(getEmployeeEvaluationTemplates);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPost("save")]
    public async Task<IActionResult> SaveEmployeeEvaluationTemplate([FromQuery] string template)
    {
        try
        {
            var savedEmployeeEvaluationTemplate = await _employeeEvaluationTemplateService.SaveEmployeeEvaluationTemplate(template);

            return Ok(savedEmployeeEvaluationTemplate);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPut("update")]
    public async Task<IActionResult> UpdateEmployeeEvaluationTemplate([FromBody] EmployeeEvaluationTemplateDto employeeEvaluationTemplateDto)
    {
        try
        {
            await _employeeEvaluationTemplateService.UpdateEmployeeEvaluationTemplate(employeeEvaluationTemplateDto);

            return Ok();
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpDelete("delete")]
    public async Task<IActionResult> DeleteEmployeeEvaluationTemplate([FromQuery] string template)
    {
        try
        {
            await _employeeEvaluationTemplateService.DeleteEmployeeEvaluationTemplate(template);

            return Ok();
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }
}
