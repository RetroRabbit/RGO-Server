namespace HRIS.Models;

public class EmployeeEvaluationRatingDto
{
    public int Id { get; set; }
    public EmployeeEvaluationDto? Evaluation { get; set; }
    public EmployeeDto? Employee { get; set; }
    public string Description { get; set; }
    public float Score { get; set; }
    public string Comment { get; set; }
}
