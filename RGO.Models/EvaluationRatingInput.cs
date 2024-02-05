namespace RGO.Models;

public class EvaluationRatingInput
{
    // TODO : Remove this Constructor + Update UNIT TESTS
    public EvaluationRatingInput(int Id,
        string EmployeeEmail,
        EmployeeEvaluationInput Evaluation,
        string Description,
        float Score,
        string Comment)
    {
        this.Id = Id;
        this.EmployeeEmail = EmployeeEmail;
        this.Evaluation = Evaluation;
        this.Description = Description;
        this.Score = Score;
        this.Comment = Comment;
    }

    public int Id { get; set; }
    public string EmployeeEmail { get; set; }
    public EmployeeEvaluationInput Evaluation { get; set; }
    public string Description { get; set; }
    public float Score { get; set; }
    public string Comment { get; set; }
}