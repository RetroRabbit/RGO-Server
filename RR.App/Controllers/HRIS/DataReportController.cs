using HRIS.Models.Update;
using HRIS.Services.Interfaces.Reporting;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using HRIS.Models.Report.Request;

namespace RR.App.Controllers.HRIS;

[Route("data-reports")]
[ApiController]
public class DataReportController : ControllerBase
{
    private readonly IDataReportService _service;
    private readonly IDataReportControlService _control;
    private readonly IDataReportAccessService _access;

    public DataReportController(IDataReportService service, IDataReportControlService control, IDataReportAccessService access)
    {
        _service = service;
        _control = control;
        _access = access;
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

    [Authorize(Policy = "AdminOrTalentOrJourneyOrSuperAdminPolicy")]
    [HttpGet("ui/get-column-menu")]
    public async Task<IActionResult> GetColumnMenu()
    {
        try
        {
            return Ok(await _control.GetColumnMenu());
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [Authorize(Policy = "AdminOrTalentOrJourneyOrSuperAdminPolicy")]
    [HttpPut("ui/add-column-to-report")]
    public async Task<IActionResult> AddColumnToReport([FromBody] ReportColumnRequest input)
    {
        try
        {
            return Ok(await _control.AddColumnToReport(input));
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [Authorize(Policy = "AdminOrTalentOrJourneyOrSuperAdminPolicy")]
    [HttpDelete("ui/archive-column-from-report")]
    public async Task<IActionResult> ArchiveColumnFromReport(int columnId)
    {
        try
        {
            await _control.ArchiveColumnFromReport(columnId);
            return Ok();
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [Authorize(Policy = "AdminOrTalentOrJourneyOrSuperAdminPolicy")]
    [HttpPut("ui/move-column-on-report")]
    public async Task<IActionResult> MoveColumnOnReport([FromBody] ReportColumnRequest input)
    {
        try
        {
            return Ok(await _control.MoveColumnOnReport(input));
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [Authorize(Policy = "AdminOrTalentOrJourneyOrSuperAdminPolicy")]
    [HttpPut("update-report")]
    public async Task<IActionResult> AddOrUpdateReport([FromBody] UpdateReportRequest input)
    {
        try
        {
            await _control.AddOrUpdateReport(input);
            return Ok();
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [Authorize(Policy = "AdminOrTalentOrJourneyOrSuperAdminPolicy")]
    [HttpPut("update-report-access")]
    public async Task<IActionResult> AddOrUpdateReportAccess([FromBody] UpdateReportAccessRequest input)
    {
        try
        {
            await _access.AddOrUpdateReportAccess(input);
            return Ok();
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }
}