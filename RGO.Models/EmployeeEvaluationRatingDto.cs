namespace RGO.Models;

public class EmployeeEvaluationRatingDto
{
    // TODO : Remove this Constructor + Update UNIT TESTS
    public EmployeeEvaluationRatingDto(int Id,
        EmployeeEvaluationDto? Evaluation,
        EmployeeDto? Employee,
        string Description,
        float Score,
        string Comment)
    {
        this.Id = Id;
        this.Evaluation = Evaluation;
        this.Employee = Employee;
        this.Description = Description;
        this.Score = Score;
        this.Comment = Comment;
    }

    public int Id { get; set; }
    public EmployeeEvaluationDto? Evaluation { get; set; }
    public EmployeeDto? Employee { get; set; }
    public string Description { get; set; }
    public float Score { get; set; }
    public string Comment { get; set; }
}
