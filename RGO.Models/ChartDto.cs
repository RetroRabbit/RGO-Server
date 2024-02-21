namespace HRIS.Models;

public class ChartDto
{
    // TODO : Remove this Constructor + Update UNIT TESTS
    public ChartDto(int Id,
                    string? Name,
                    string? Type,
                    List<string>? DataTypes,
                    List<string>? Labels,
                    List<int>? Data)
    {
        this.Id = Id;
        this.Name = Name;
        this.Type = Type;
        this.DataTypes = DataTypes;
        this.Labels = Labels;
        this.Data = Data;
    }

    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Type { get; set; }
    public List<string>? DataTypes { get; set; }
    public List<string>? Labels { get; set; }
    public List<int>? Data { get; set; }
}