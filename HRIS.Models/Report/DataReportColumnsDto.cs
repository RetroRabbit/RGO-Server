using HRIS.Models.Enums;

namespace HRIS.Models.Report;

public class DataReportColumnsDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Prop { get; set; }
    public int Sequence { get; set; }
    public bool IsCustom { get; set; }
    public string? FieldType { get; set; }
    public ItemStatus Status { get; set; }
}