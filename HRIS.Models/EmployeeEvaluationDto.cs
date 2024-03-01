namespace HRIS.Models;

public class EmployeeEvaluationDto
{
    public int Id { get; set; }
    public EmployeeDto? Employee { get; set; }
    public EmployeeEvaluationTemplateDto? Template { get; set; }
    public EmployeeDto? Owner { get; set; }
    public string Subject { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
}
