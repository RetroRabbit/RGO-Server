namespace HRIS.Models;

public class EmployeeDateDto
{
    public int Id { get; set; }
    public EmployeeDto? Employee { get; set; }
    public string? Subject { get; set; }
    public string? Note { get; set; }
    public DateOnly Date { get; set; }
}
