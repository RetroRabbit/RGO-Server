using RGO.Models;
using RGO.UnitOfWork.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace RGO.UnitOfWork.Entities;

[Table("EmployeeEvaluationRatings")]
public class EmployeeEvaluationRating : IModel<EmployeeEvaluationRatingDto>
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("employeeEvaluationId")]
    [ForeignKey("Evaluation")]
    public int EmployeeEvaluationId { get; set; }

    [Column("employeeId")]
    [ForeignKey("Employee")]
    public int EmployeeId { get; set; }

    [Column("description")]
    public string Description { get; set; } = string.Empty;

    [Column("score")]
    public float Score { get; set; }

    [Column("comment")]
    public string Comment { get; set; } = string.Empty;

    public virtual Employee Employee { get; set; } = null!;
    public virtual EmployeeEvaluation Evaluation { get; set; } = null!;

    public EmployeeEvaluationRating() { }

    public EmployeeEvaluationRating(EmployeeEvaluationRatingDto ratingsDto)
    {
        Id = ratingsDto.Id;
        EmployeeEvaluationId = ratingsDto.Evaluation!.Id;
        EmployeeId = ratingsDto.Employee!.Id;
        Description = ratingsDto.Description;
        Score = ratingsDto.Score;
        Comment = ratingsDto.Comment;
    }

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
