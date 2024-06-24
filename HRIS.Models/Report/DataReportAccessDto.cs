namespace HRIS.Models.Report;

public class DataReportAccessDto
{
    public int Id { get; set; }
    public int ReportId { get; set; }
    public int? EmployeeId { get; set; }
    public int? RoleId { get; set; }
    public bool ViewOnly { get; set; }
}