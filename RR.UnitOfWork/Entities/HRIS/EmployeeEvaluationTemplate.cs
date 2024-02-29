using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HRIS.Models;
using RR.UnitOfWork.Interfaces;

namespace RR.UnitOfWork.Entities.HRIS;

[Table("EmployeeEvaluationTemplate")]
public class EmployeeEvaluationTemplate : IModel<EmployeeEvaluationTemplateDto>
{
    public EmployeeEvaluationTemplate()
    {
    }

    public EmployeeEvaluationTemplate(EmployeeEvaluationTemplateDto employeeEvaluationTemplateDto)
    {
        Id = employeeEvaluationTemplateDto.Id;
        Description = employeeEvaluationTemplateDto.Description;
    }

    [Column("description")] public string Description { get; set; } = string.Empty;

    [Key] [Column("id")] public int Id { get; set; }

    public EmployeeEvaluationTemplateDto ToDto()
    {
        return new EmployeeEvaluationTemplateDto(
                                                 Id,
                                                 Description);
    }
}