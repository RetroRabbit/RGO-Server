using HRIS.Models.Enums;

namespace HRIS.Models.Report.Request;

public class ReportColumnRequest
{
    public int? Id { get; set; }
    public int ReportId { get; set; }
    public int? MenuId { get; set; }
    public int Sequence { get; set; }
    public string? CustomType { get; set; }
    public string? Name { get; set; }
    public string? Prop { get; set; }

    public DataReportColumnType GetColumnType()
    {
        if (CustomType == "CheckBox")
            return DataReportColumnType.Checkbox;
        if(CustomType == "Text")
            return DataReportColumnType.Text;
        return DataReportColumnType.Employee;
    }
}