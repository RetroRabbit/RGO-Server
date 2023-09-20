﻿using Microsoft.AspNetCore.Mvc;
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

    [HttpGet("getall")]
    public async Task<IActionResult> GetAllEmployeeEvaluationTemplateItems(
        [FromQuery] string? section,
        [FromQuery] string? template)
    {
        List<EmployeeEvaluationTemplateItemDto> getEmployeeEvaluationTemplateItems;

        try
        {
            if (!string.IsNullOrEmpty(section))
                getEmployeeEvaluationTemplateItems = await _employeeEvaluationTemplateItemService.GetAllEmployeeEvaluationTemplateItemsBySection(section!);
            else if (!string.IsNullOrEmpty(template))
                getEmployeeEvaluationTemplateItems = await _employeeEvaluationTemplateItemService.GetAllEmployeeEvaluationTemplateItemsByTemplate(template!);
            else
                getEmployeeEvaluationTemplateItems = await _employeeEvaluationTemplateItemService.GetAllEmployeeEvaluationTemplateItems();

            return Ok(getEmployeeEvaluationTemplateItems);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPost("save")]
    public async Task<IActionResult> SaveEmployeeEvaluationTemplateItem(
        [FromQuery] string template,
        [FromQuery] string section,
        [FromQuery] string question)
    {
        try
        {
            EmployeeEvaluationTemplateItemDto savedEmployeeEvaluationTemplateItem =
                await _employeeEvaluationTemplateItemService
                .SaveEmployeeEvaluationTemplateItem(template, section, question);

            return Ok(savedEmployeeEvaluationTemplateItem);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPut("update")]
    public async Task<IActionResult> UpdateEmployeeEvaluationTemplateItem(
        [FromBody] EmployeeEvaluationTemplateItemDto employeeEvaluationTemplateItemDto)
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
    public async Task<IActionResult> DeleteEmployeeEvaluationTemplateItem(
        [FromQuery] string template,
        [FromQuery] string section,
        [FromQuery] string question)
    {
        try
        {
            await _employeeEvaluationTemplateItemService
                .DeleteEmployeeEvaluationTemplateItem(template, section, question);

            return Ok();
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }
}
