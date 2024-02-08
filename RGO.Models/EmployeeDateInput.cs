namespace RGO.Models;

public class EmployeeDateInput
{
    // TODO : Remove this Constructor + Update UNIT TESTS
    public EmployeeDateInput(string Email,
        string Subject,
        string Note,
        DateOnly Date)
    {
        this.Email = Email;
        this.Subject = Subject;
        this.Note = Note;
        this.Date = Date;
    }

    public string Email { get; set; }
    public string Subject { get; set; }
    public string Note { get; set; }
    public DateOnly Date { get; set; }
}
