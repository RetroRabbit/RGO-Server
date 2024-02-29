using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HRIS.Models;
using RR.UnitOfWork.Interfaces;

namespace RR.UnitOfWork.Entities.HRIS;

[Table("EmployeeEvaluations")]
public class EmployeeEvaluation : IModel<EmployeeEvaluationDto>
{
    public EmployeeEvaluation()
    {
    }

    public EmployeeEvaluation(EmployeeEvaluationDto employeeEvaluationDto)
    {
        Id = employeeEvaluationDto.Id;
        EmployeeId = employeeEvaluationDto.Employee!.Id;
        TemplateId = employeeEvaluationDto.Template!.Id;
        OwnerId = employeeEvaluationDto.Owner!.Id;
        Subject = employeeEvaluationDto.Subject;
        StartDate = employeeEvaluationDto.StartDate;
        EndDate = employeeEvaluationDto.EndDate;
    }

    [Column("employeeId")]
    [ForeignKey("Employee")]
    public int EmployeeId { get; set; }

    [Column("templateId")]
    [ForeignKey("Template")]
    public int TemplateId { get; set; }

    [Column("ownerId")]
    [ForeignKey("Owner")]
    public int OwnerId { get; set; }

    [Column("subject")] public string Subject { get; set; } = string.Empty;

    [Column("startDate")] public DateOnly StartDate { get; set; }

    [Column("endDate")] public DateOnly? EndDate { get; set; }

    public virtual Employee Employee { get; set; } = null!;
    public virtual Employee Owner { get; set; } = null!;
    public virtual EmployeeEvaluationTemplate Template { get; set; } = null!;

    [Key] [Column("id")] public int Id { get; set; }

    public EmployeeEvaluationDto ToDto()
    {
        return new EmployeeEvaluationDto(
                                         Id,
                                         Employee?.ToDto(),
                                         Template?.ToDto(),
                                         Owner?.ToDto(),
                                         Subject,
                                         StartDate,
                                         EndDate);
    }
}