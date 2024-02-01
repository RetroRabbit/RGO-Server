namespace RGO.Models;

public class EmployeeEvaluationInput
{
    // TODO : Remove this Constructor + Update UNIT TESTS
    public EmployeeEvaluationInput(int? Id,
        string OwnerEmail,
        string EmployeeEmail,
        string Template,
        string Subject)
    {
        this.Id = Id;
        this.OwnerEmail = OwnerEmail;
        this.EmployeeEmail = EmployeeEmail;
        this.Template = Template;
        this.Subject = Subject;
    }

    public int? Id { get; set; }
    public string OwnerEmail { get; set; }
    public string EmployeeEmail { get; set; }
    public string Template { get; set; }
    public string Subject { get; set; }
}