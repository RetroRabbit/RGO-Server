namespace HRIS.Models;

public class EmployeeEvaluationDto
{
    // TODO : Remove this Constructor + Update UNIT TESTS
    public EmployeeEvaluationDto(int Id,
                                 EmployeeDto? Employee,
                                 EmployeeEvaluationTemplateDto? Template,
                                 EmployeeDto? Owner,
                                 string Subject,
                                 DateOnly StartDate,
                                 DateOnly? EndDate)
    {
        this.Id = Id;
        this.Employee = Employee;
        this.Template = Template;
        this.Owner = Owner;
        this.Subject = Subject;
        this.StartDate = StartDate;
        this.EndDate = EndDate;
    }

    public int Id { get; set; }
    public EmployeeDto? Employee { get; set; }
    public EmployeeEvaluationTemplateDto? Template { get; set; }
    public EmployeeDto? Owner { get; set; }
    public string Subject { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
}