namespace HRIS.Models;

public class ChartDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Type { get; set; }
    public List<string> DataTypes { get; set; }
    public List<string> Labels { get; set; }
    public List<int> Data { get; set; }
}
