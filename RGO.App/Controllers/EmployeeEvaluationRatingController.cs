using Microsoft.AspNetCore.Mvc;
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

    [HttpGet("get")]
    public async Task<IActionResult> GetEmployeeEvaluationRating([FromQuery] string email, [FromBody] EmployeeEvaluationDto evaluation)
    {
        try
        {
            var getEmployeeEvaluationRating = await _employeeEvaluationRatingService.GetEmployeeEvaluationRating(email, evaluation);

            return Ok(getEmployeeEvaluationRating);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet("getall")]
    public async Task<IActionResult> GetAllEmployeeEvaluationRatings()
    {
        try
        {
            var getEmployeeEvaluationRatings = await _employeeEvaluationRatingService.GetAllEmployeeEvaluationRatings();

            return Ok(getEmployeeEvaluationRatings);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet("getall")]
    public async Task<IActionResult> GetAllEmployeeEvaluationRatingsByEmployee([FromQuery] string email)
    {
        try
        {
            var getEmployeeEvaluationRatings = await _employeeEvaluationRatingService.GetAllEmployeeEvaluationRatingsByEmployee(email);

            return Ok(getEmployeeEvaluationRatings);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet("getall")]
    public async Task<IActionResult> GetAllEmployeeEvaluationRatingsByEvaluation([FromBody] EmployeeEvaluationDto evaluation)
    {
        try
        {
            var getEmployeeEvaluationRatings = await _employeeEvaluationRatingService.GetAllEmployeeEvaluationRatingsByEvaluation(evaluation);

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
    public async Task<IActionResult> DeleteEmployeeEvaluationRating([FromQuery] string email, [FromBody] EmployeeEvaluationDto evaluation)
    {
        try
        {
            await _employeeEvaluationRatingService.DeleteEmployeeEvaluationRating(email, evaluation);

            return Ok();
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }
}
