namespace HRIS.Models;

public class EmployeeAccessDto
{
    public int Id { get; set; }
    public int Condition { get; set; }
    public bool Internal { get; set; }
    public string PropName { get; set; }
    public string Label { get; set; }
    public string Type { get; set; }
    public string Value { get; set; }
    public string? Description { get; set; }
    public string? Regex { get; set; }
    public List<string>? Options { get; set; }
}
