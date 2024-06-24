namespace HRIS.Models.Report.Request;

public class UpdateReportRequest
{
    public int ReportId { get; set; }
    public string Name { get; set; }
    public string Code { get; set; }
}