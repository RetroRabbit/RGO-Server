namespace HRIS.Models;

public class EmployeeEvaluationTemplateItemDto
{
    public int Id { get; set; }
    public EmployeeEvaluationTemplateDto? Template { get; set; }
    public string Section { get; set; }
    public string Question { get; set; }
}