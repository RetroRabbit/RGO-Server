using HRIS.Models.Update;
using HRIS.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RR.App.Controllers.HRIS;

[Route("data-reports")]
[ApiController]
public class DataReportController : ControllerBase
{
    private readonly IDataReportService _service;

    public DataReportController(IDataReportService service)
    {
        _service = service;
    }

    [Authorize(Policy = "AdminOrTalentOrJourneyOrSuperAdminPolicy")]
    [HttpGet("get-data-report-list")]
    public async Task<IActionResult> GetDataReportList()
    {
        try
        {
            return Ok(await _service.GetDataReportList());
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [Authorize(Policy = "AdminOrTalentOrJourneyOrSuperAdminPolicy")]
    [HttpGet("get-data-report")]
    public async Task<IActionResult> GetDataReport([FromQuery] string code)
    {
        try
        {
            return Ok(await _service.GetDataReport(code));
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [Authorize(Policy = "AdminOrTalentOrJourneyOrSuperAdminPolicy")]
    [HttpPost("update-report-input")]
    public async Task<IActionResult> UpdateReportInput([FromBody] UpdateReportCustomValue input)
    {
        try
        {
            await _service.UpdateReportInput(input);
            return Ok();
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }
}