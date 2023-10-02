using RGO.Models;
using RGO.UnitOfWork.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RGO.UnitOfWork.Entities;

[Table("EmployeeEvaluationAudience")]
public class EmployeeEvaluationAudience : IModel<EmployeeEvaluationAudienceDto>
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

    public virtual Employee Employee { get; set; } = null!;
    public virtual EmployeeEvaluation Evaluation { get; set; } = null!;

    public EmployeeEvaluationAudience() { }

    public EmployeeEvaluationAudience(EmployeeEvaluationAudienceDto audienceDto)
    {
        Id = audienceDto.Id;
        EmployeeEvaluationId = audienceDto.Evaluation!.Id;
        EmployeeId = audienceDto.Employee!.Id;
    }

    public EmployeeEvaluationAudienceDto ToDto()
    {
        return new EmployeeEvaluationAudienceDto(
            Id,
            Evaluation?.ToDto(),
            Employee?.ToDto());
    }
}
