namespace HRIS.Models.Report.Response;

public class ReportAccessResponse
{
    public int Id { get; set; }
    public int? EmployeeId { get; set; }
    public int? RoleId { get; set; }
    public bool ViewOnly { get; set; }
    public string Name { get; set; }
}