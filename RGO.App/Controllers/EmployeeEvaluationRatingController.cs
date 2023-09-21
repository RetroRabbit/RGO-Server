using Microsoft.AspNetCore.Mvc;
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

    [HttpGet("get")]
    public async Task<IActionResult> GetEmployeeEvaluationRating([FromQuery] string email, [FromQuery] string employeeEamil, [FromQuery] string ownerEmail, [FromQuery] string template, [FromQuery] string subject)
    {
        try
        {
            var getEmployeeEvaluationRating = await _employeeEvaluationRatingService.GetEmployeeEvaluationRating(email, employeeEamil, ownerEmail, template, subject);

            return Ok(getEmployeeEvaluationRating);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet("getall")]
    public async Task<IActionResult> GetAllEmployeeEvaluationRatings(
        [FromQuery] string employeeEmail,
        [FromQuery] string ownerEmail,
        [FromQuery] string template,
        [FromQuery] string subject)
    {
        List<EmployeeEvaluationRatingDto> getEmployeeEvaluationRatings;

        try
        {
            getEmployeeEvaluationRatings = await _employeeEvaluationRatingService.GetAllEmployeeEvaluationRatingsByEvaluation(employeeEmail, ownerEmail, template, subject);

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
    public async Task<IActionResult> DeleteEmployeeEvaluationRating([FromQuery] string email, [FromQuery] string employeeEamil, [FromQuery] string ownerEmail, [FromQuery] string template, [FromQuery] string subject)
    {
        try
        {
            await _employeeEvaluationRatingService.DeleteEmployeeEvaluationRating(email, employeeEamil, ownerEmail, template, subject);

            return Ok();
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }
}
