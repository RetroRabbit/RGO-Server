using HRIS.Models.Enums;

namespace HRIS.Models.Report;

public class DataReportDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Code { get; set; }
    public ItemStatus Status { get; set; }
    public List<DataReportColumnsDto>? DataReportColumns { get; set; }
    public List<DataReportFilterDto>? DataReportFilter { get; set; }
    public List<DataReportValuesDto>? DataReportValues { get; set; }
    public List<DataReportAccessDto>? DataReportAccess { get; set; }
}