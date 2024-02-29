using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HRIS.Models;
using RR.UnitOfWork.Interfaces;

namespace RR.UnitOfWork.Entities.HRIS;

[Table("EmployeeEvaluationAudience")]
public class EmployeeEvaluationAudience : IModel<EmployeeEvaluationAudienceDto>
{
    public EmployeeEvaluationAudience()
    {
    }

    public EmployeeEvaluationAudience(EmployeeEvaluationAudienceDto audienceDto)
    {
        Id = audienceDto.Id;
        EmployeeEvaluationId = audienceDto.Evaluation!.Id;
        EmployeeId = audienceDto.Employee!.Id;
    }

    [Column("employeeEvaluationId")]
    [ForeignKey("Evaluation")]
    public int EmployeeEvaluationId { get; set; }

    [Column("employeeId")]
    [ForeignKey("Employee")]
    public int EmployeeId { get; set; }

    public virtual Employee Employee { get; set; } = null!;
    public virtual EmployeeEvaluation Evaluation { get; set; } = null!;

    [Key] [Column("id")] public int Id { get; set; }

    public EmployeeEvaluationAudienceDto ToDto()
    {
        return new EmployeeEvaluationAudienceDto(
                                                 Id,
                                                 Evaluation?.ToDto(),
                                                 Employee?.ToDto());
    }
}