namespace RGO.Models;

public class EmployeeEvaluationAudienceDto
{
    // TODO : Remove this Constructor + Update UNIT TESTS
    public EmployeeEvaluationAudienceDto(int Id,
        EmployeeEvaluationDto? Evaluation,
        EmployeeDto? Employee)
    {
        this.Id = Id;
        this.Evaluation = Evaluation;
        this.Employee = Employee;
    }

    public int Id { get; set; }
    public EmployeeEvaluationDto? Evaluation { get; set; }
    public EmployeeDto? Employee { get; set; }
}