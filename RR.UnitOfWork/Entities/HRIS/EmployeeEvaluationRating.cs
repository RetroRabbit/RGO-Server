using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HRIS.Models;
using RR.UnitOfWork.Interfaces.HRIS;

namespace RR.UnitOfWork.Entities.HRIS;

[Table("EmployeeEvaluationRatings")]
public class EmployeeEvaluationRating : IModel<EmployeeEvaluationRatingDto>
{
    public EmployeeEvaluationRating()
    {
    }

    public EmployeeEvaluationRating(EmployeeEvaluationRatingDto ratingsDto)
    {
        Id = ratingsDto.Id;
        EmployeeEvaluationId = ratingsDto.Evaluation!.Id;
        EmployeeId = ratingsDto.Employee!.Id;
        Description = ratingsDto.Description;
        Score = ratingsDto.Score;
        Comment = ratingsDto.Comment;
    }

    [Column("employeeEvaluationId")]
    [ForeignKey("Evaluation")]
    public int EmployeeEvaluationId { get; set; }

    [Column("employeeId")]
    [ForeignKey("Employee")]
    public int EmployeeId { get; set; }

    [Column("description")] public string Description { get; set; } = string.Empty;

    [Column("score")] public float Score { get; set; }

    [Column("comment")] public string Comment { get; set; } = string.Empty;

    public virtual Employee Employee { get; set; } = null!;
    public virtual EmployeeEvaluation Evaluation { get; set; } = null!;

    [Key] [Column("id")] public int Id { get; set; }

    public EmployeeEvaluationRatingDto ToDto()
    {
        return new EmployeeEvaluationRatingDto(
                                               Id,
                                               Evaluation?.ToDto(),
                                               Employee?.ToDto(),
                                               Description,
                                               Score,
                                               Comment);
    }
}