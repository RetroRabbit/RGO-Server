using HRIS.Models;
using HRIS.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace RR.App.Controllers.HRIS;

[Route("evaluation-rating")]
[ApiController]
public class EmployeeEvaluationRatingController : ControllerBase
{
    private readonly IEmployeeEvaluationRatingService _employeeEvaluationRatingService;

    public EmployeeEvaluationRatingController(IEmployeeEvaluationRatingService employeeEvaluationRatingService)
    {
        _employeeEvaluationRatingService = employeeEvaluationRatingService;
    }

    [HttpPost("all")]
    public async Task<IActionResult> GetAllEmployeeEvaluationRatings(
        [FromBody] EmployeeEvaluationInput evaluationInput)
    {
        try
        {
            var getEmployeeEvaluationRatings = await _employeeEvaluationRatingService
                .GetAllByEvaluation(evaluationInput);

            return Ok(getEmployeeEvaluationRatings);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> SaveEmployeeEvaluationRating([FromBody] EvaluationRatingInput rating)
    {
        try
        {
            var savedEmployeeEvaluationRating = await _employeeEvaluationRatingService.Save(rating);

            return Ok(savedEmployeeEvaluationRating);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPut]
    public async Task<IActionResult> UpdateEmployeeEvaluationRating([FromBody] EvaluationRatingInput rating)
    {
        try
        {
            await _employeeEvaluationRatingService.Update(rating);

            return Ok();
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteEmployeeEvaluationRating([FromBody] EvaluationRatingInput rating)
    {
        try
        {
            await _employeeEvaluationRatingService.Delete(rating);

            return Ok();
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }
}