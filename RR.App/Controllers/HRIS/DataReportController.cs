using HRIS.Models.Update;
using HRIS.Services.Interfaces.Reporting;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        return Ok(await _service.GetDataReportList());
    }

    [Authorize(Policy = "AdminOrTalentOrJourneyOrSuperAdminPolicy")]
    [HttpGet("get-data-report")]
    public async Task<IActionResult> GetDataReport([FromQuery] string code)
    {
        return Ok(await _service.GetDataReport(code));
    }

    [Authorize(Policy = "AdminOrTalentOrJourneyOrSuperAdminPolicy")]
    [HttpPost("update-report-input")]
    public async Task<IActionResult> UpdateReportInput([FromBody] UpdateReportCustomValue input)
    {
        await _service.UpdateReportInput(input);
        return Ok();
    }

    [Authorize(Policy = "AdminOrTalentOrJourneyOrSuperAdminPolicy")]
    [HttpGet("ui/get-column-menu")]
    public async Task<IActionResult> GetColumnMenu()
    {
        return Ok(await _control.GetColumnMenu());
    }

    [Authorize(Policy = "AdminOrTalentOrJourneyOrSuperAdminPolicy")]
    [HttpPut("ui/add-column-to-report")]
    public async Task<IActionResult> AddColumnToReport([FromBody] ReportColumnRequest input)
    {
        return Ok(await _control.AddColumnToReport(input));
    }

    [Authorize(Policy = "AdminOrTalentOrJourneyOrSuperAdminPolicy")]
    [HttpDelete("ui/archive-column-from-report")]
    public async Task<IActionResult> ArchiveColumnFromReport([FromQuery] int columnId)
    {
        await _control.ArchiveColumnFromReport(columnId);
        return Ok();
    }

    [Authorize(Policy = "AdminOrTalentOrJourneyOrSuperAdminPolicy")]
    [HttpPut("ui/move-column-on-report")]
    public async Task<IActionResult> MoveColumnOnReport([FromBody] ReportColumnRequest input)
    {
        await _control.MoveColumnOnReport(input);
        return Ok();
    }

    [Authorize(Policy = "AdminOrTalentOrJourneyOrSuperAdminPolicy")]
    [HttpPut("update-report")]
    public async Task<IActionResult> AddOrUpdateReport([FromBody] UpdateReportRequest input)
    {
        Console.WriteLine("Does it even get here");
        await _control.AddOrUpdateReport(input);
        return Ok();
    }

    [Authorize(Policy = "AdminOrTalentOrJourneyOrSuperAdminPolicy")]
    [HttpGet("get-report-access-availability")]
    [AllowAnonymous]
    public async Task<IActionResult> GetReportAccessAvailability([FromQuery] int reportId)
    {
        return Ok(await _access.GetReportAccessAvailability(reportId));
    }

    [Authorize(Policy = "AdminOrTalentOrJourneyOrSuperAdminPolicy")]
    [HttpPut("update-report-access")]
    public async Task<IActionResult> AddOrUpdateReportAccess([FromBody] UpdateReportAccessRequest input)
    {
        await _access.AddOrUpdateReportAccess(input);
        return Ok();
    }

    [Authorize(Policy = "AdminOrTalentOrJourneyOrSuperAdminPolicy")]
    [HttpDelete("archive-report-access")]
    public async Task<IActionResult> ArchiveReportAccess([FromQuery] int accessId)
    {
        await _access.ArchiveReportAccess(accessId);
        return Ok();
    }

    [Authorize(Policy = "AdminOrTalentOrJourneyOrSuperAdminPolicy")]
    [HttpDelete("delete-report")]
    public async Task<IActionResult> DeleteReport([FromQuery] string code)
    {
        await _service.DeleteReportfromList(code);
        return Ok();
    }

    [Authorize(Policy = "AdminOrTalentOrJourneyOrSuperAdminPolicy")]
    [HttpPut("update-data-report-filter")]
    public async Task<IActionResult> GetDataReportFilter([FromQuery] ReportFilterRequest request)
    {
        await _control.AddOrUpdateReportFilter(request);
        return Ok();
    }

    [Authorize(Policy = "AdminOrTalentOrJourneyOrSuperAdminPolicy")]
    [HttpPut("archive-data-report-filter")]
    public async Task<IActionResult> ArchiveDataReportFilter([FromQuery] int id)
    {
        await _control.DeleteReportFilterfromList(id);
        return Ok();
    }
}