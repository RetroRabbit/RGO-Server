using RGO.Models;
using RGO.UnitOfWork.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace RGO.UnitOfWork.Entities
{
    [Table("EmployeeEvaluationRatings")]
    public class EmployeeEvaluationRating : IModel<EmployeeEvaluationRatingDto>
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("employeeEvaluationId")]
        [ForeignKey("EmployeeEvaluation")]
        public int EmployeeEvaluationId { get; set; }

        [Column("employeeId")]
        [ForeignKey("Employee")]
        public int EmployeeId { get; set; }

        [Column("score")]
        public float Score { get; set; }

        [Column("comment")]
        public string Comment { get; set; }

        public virtual Employee Employee { get; set; }
        public virtual EmployeeEvaluation EmployeeEvaluation { get; set; }

        public EmployeeEvaluationRating() { }

        public EmployeeEvaluationRating(EmployeeEvaluationRatingDto ratingsDto)
        {
            Id = ratingsDto.Id;
            EmployeeEvaluationId = ratingsDto.EmployeeEvaluationId;
            EmployeeId = ratingsDto.EmployeeId;
            Score = ratingsDto.Score;
            Comment = ratingsDto.Comment;
        }

        public EmployeeEvaluationRatingDto ToDto()
        {
            return new EmployeeEvaluationRatingDto(
                Id,
                EmployeeEvaluationId,
                EmployeeId,
                Score,
                Comment);
        }
    }
}
