using HRIS.Models.Enums;

namespace HRIS.Models;

public class DataReportFilterDto
{
    public int Id { get; set; }
    public string Table { get; set; }
    public string Column { get; set; }
    public string Condition { get; set; }
    public string? Value { get; set; }
    public string? Select { get; set; }
    public int ReportId { get; set; }
    public ItemStatus Status { get; set; }
}