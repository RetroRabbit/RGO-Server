namespace HRIS.Models;

public class EmployeeDateInput
{
    public int Id { get; set; }
    public string Email { get; set; }
    public string Subject { get; set; }
    public string Note { get; set; }
    public DateOnly Date { get; set; }
}