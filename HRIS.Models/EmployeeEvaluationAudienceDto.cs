namespace HRIS.Models;

public class EmployeeEvaluationAudienceDto
{
    public int Id { get; set; }
    public EmployeeEvaluationDto? Evaluation { get; set; }
    public EmployeeDto? Employee { get; set; }
}
