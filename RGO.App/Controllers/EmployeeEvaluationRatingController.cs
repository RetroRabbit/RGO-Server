﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using RGO.Models;
using RGO.Services.Interfaces;

namespace RGO.App.Controllers;

[Route("/employeeevaluationrating/")]
[ApiController]
public class EmployeeEvaluationRatingController : ControllerBase
{
    private readonly IEmployeeEvaluationRatingService _employeeEvaluationRatingService;

    public EmployeeEvaluationRatingController(IEmployeeEvaluationRatingService employeeEvaluationRatingService)
    {
        _employeeEvaluationRatingService = employeeEvaluationRatingService;
    }

    [HttpPost("get")]
    public async Task<IActionResult> GetEmployeeEvaluationRating(
        [FromQuery] string email,
        [FromBody] EmployeeEvaluationInput evaluationInput)
    {
        try
        {
            var getEmployeeEvaluationRating = await _employeeEvaluationRatingService.GetEmployeeEvaluationRating(email, evaluationInput);

            return Ok(getEmployeeEvaluationRating);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPost("getall")]
    public async Task<IActionResult> GetAllEmployeeEvaluationRatings(
        [FromBody] EmployeeEvaluationInput evaluationInput)
    {
        List<EmployeeEvaluationRatingDto> getEmployeeEvaluationRatings;

        try
        {
            getEmployeeEvaluationRatings = await _employeeEvaluationRatingService.GetAllEmployeeEvaluationRatingsByEvaluation(evaluationInput);

            if (getEmployeeEvaluationRatings.IsNullOrEmpty())
                getEmployeeEvaluationRatings = await _employeeEvaluationRatingService.GetAllEmployeeEvaluationRatings();

            return Ok(getEmployeeEvaluationRatings);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPost("save")]
    public async Task<IActionResult> SaveEmployeeEvaluationRating([FromBody] EmployeeEvaluationRatingDto employeeEvaluationRatingDto)
    {
        try
        {
            var savedEmployeeEvaluationRating = await _employeeEvaluationRatingService.SaveEmployeeEvaluationRating(employeeEvaluationRatingDto);

            return Ok(savedEmployeeEvaluationRating);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPut("update")]
    public async Task<IActionResult> UpdateEmployeeEvaluationRating([FromBody] EmployeeEvaluationRatingDto employeeEvaluationRatingDto)
    {
        try
        {
            await _employeeEvaluationRatingService.UpdateEmployeeEvaluationRating(employeeEvaluationRatingDto);

            return Ok();
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpDelete("delete")]
    public async Task<IActionResult> DeleteEmployeeEvaluationRating(
        [FromQuery] string email,
        [FromBody] EmployeeEvaluationInput evaluationInput)
    {
        try
        {
            await _employeeEvaluationRatingService.DeleteEmployeeEvaluationRating(email, evaluationInput);

            return Ok();
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }
}
