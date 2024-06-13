using HRIS.Models.Enums;

namespace HRIS.Models;

public class DataReportDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Code { get; set; }
    public ItemStatus Status { get; set; }
    public List<DataReportColumnsDto>? Columns { get; set; }
    public List<DataReportFilterDto>? Filters { get; set; }
}