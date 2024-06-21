namespace HRIS.Models.DataReport;

public class DataReportValuesDto
{
    public int Id { get; set; }
    public int ReportId { get; set; }
    public int ColumnId { get; set; }
    public int EmployeeId { get; set; }
    public string Input { get; set; }
}