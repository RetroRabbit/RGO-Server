
namespace HRIS.Models.Report.Request
{
    public class ReportFilterRequest
    {
        public int ReportFilterId { get; set; }
        public int EmployeeId { get; set; }
        public string TableName { get; set; }
        public string ColumnName { get; set; }
        public string Condition { get; set; }
        public string Value { get; set; }
        public int ReportId { get; set; }


    }
}
