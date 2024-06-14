namespace HRIS.Models.Update;

public class UpdateReportCustomValue
{
    public int ReportId { get; set; }
    public int ColumnId { get; set; }
    public int EmployeeId { get; set; }
    public string Input { get; set; }
}