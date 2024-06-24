namespace HRIS.Models.Report.Request;

public class UpdateReportAccessRequest
{
    public int ReportId { get; set; }
    public List<UpdateReportAccessItemRequest> Access { get; set; }

    public class UpdateReportAccessItemRequest
    {
        public int? EmployeeId { get; set; }
        public int? RoleId { get; set; }
        public bool ViewOnly { get; set; }
    }
}