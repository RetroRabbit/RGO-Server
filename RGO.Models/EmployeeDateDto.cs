namespace HRIS.Models;

public class EmployeeDateDto
{
    // TODO : Remove this Constructor + Update UNIT TESTS
    public EmployeeDateDto(int Id,
                           EmployeeDto? Employee,
                           string? Subject,
                           string? Note,
                           DateOnly Date)
    {
        this.Id = Id;
        this.Employee = Employee;
        this.Subject = Subject;
        this.Note = Note;
        this.Date = Date;
    }

    public int Id { get; set; }
    public EmployeeDto? Employee { get; set; }
    public string? Subject { get; set; }
    public string? Note { get; set; }
    public DateOnly Date { get; set; }
}