using Microsoft.AspNetCore.Mvc;
using RGO.Models;
using RGO.Services.Interfaces;

namespace RGO.App.Controllers;

[Route("/employeeevaluationitem/")]
[ApiController]
public class EmployeeEvaluationTemplateItemController : ControllerBase
{
    private readonly IEmployeeEvaluationTemplateItemService _employeeEvaluationTemplateItemService;

    public EmployeeEvaluationTemplateItemController(IEmployeeEvaluationTemplateItemService employeeEvaluationTemplateItemService)
    {
        _employeeEvaluationTemplateItemService = employeeEvaluationTemplateItemService;
    }

    [HttpGet("get")]
    public async Task<IActionResult> GetEmployeeEvaluationTemplateItem(EmployeeEvaluationTemplateItemDto employeeEvaluationTemplateItemDto)
    {
        try
        {
            var getEmployeeEvaluationTemplateItem = await _employeeEvaluationTemplateItemService.GetEmployeeEvaluationTemplateItem(employeeEvaluationTemplateItemDto);

            return Ok(getEmployeeEvaluationTemplateItem);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet("getall")]
    public async Task<IActionResult> GetAllEmployeeEvaluationTemplateItems()
    {
        try
        {
            var getEmployeeEvaluationTemplateItems = await _employeeEvaluationTemplateItemService.GetAllEmployeeEvaluationTemplateItems();

            return Ok(getEmployeeEvaluationTemplateItems);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet("getall")]
    public async Task<IActionResult> GetAllEmployeeEvaluationTemplateItemsBySection([FromQuery] string section)
    {
        try
        {
            var getEmployeeEvaluationTemplateItems = await _employeeEvaluationTemplateItemService.GetAllEmployeeEvaluationTemplateItemsBySection(section);

            return Ok(getEmployeeEvaluationTemplateItems);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet("getall")]
    public async Task<IActionResult> GetAllEmployeeEvaluationTemplateItemsByTemplate([FromQuery] string template)
    {
        try
        {
            var getEmployeeEvaluationTemplateItems = await _employeeEvaluationTemplateItemService.GetAllEmployeeEvaluationTemplateItemsByTemplate(template);

            return Ok(getEmployeeEvaluationTemplateItems);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPost("save")]
    public async Task<IActionResult> SaveEmployeeEvaluationTemplateItem([FromBody] EmployeeEvaluationTemplateItemDto employeeEvaluationTemplateItemDto)
    {
        try
        {
            var savedEmployeeEvaluationTemplateItem = await _employeeEvaluationTemplateItemService.SaveEmployeeEvaluationTemplateItem(employeeEvaluationTemplateItemDto);

            return Ok(savedEmployeeEvaluationTemplateItem);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPut("update")]
    public async Task<IActionResult> UpdateEmployeeEvaluationTemplateItem([FromBody] EmployeeEvaluationTemplateItemDto employeeEvaluationTemplateItemDto)
    {
        try
        {
            await _employeeEvaluationTemplateItemService.UpdateEmployeeEvaluationTemplateItem(employeeEvaluationTemplateItemDto);

            return Ok();
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpDelete("delete")]
    public async Task<IActionResult> DeleteEmployeeEvaluationTemplateItem([FromBody] EmployeeEvaluationTemplateItemDto employeeEvaluationTemplateItemDto)
    {
        try
        {
            await _employeeEvaluationTemplateItemService.DeleteEmployeeEvaluationTemplateItem(employeeEvaluationTemplateItemDto);

            return Ok();
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }
}
