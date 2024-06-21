namespace HRIS.Models.DataReport;

public class DataReportColumnMenuDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Prop { get; set; }
    public List<DataReportColumnMenuDto>? Children { get; set; }
}