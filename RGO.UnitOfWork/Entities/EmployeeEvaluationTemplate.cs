using RGO.Models;
using RGO.UnitOfWork.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RGO.UnitOfWork.Entities;

[Table("EmployeeEvaluationTemplate")]
public class EmployeeEvaluationTemplate : IModel<EmployeeEvaluationTemplateDto>
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("description")]
    public string Description { get; set; } = string.Empty;

    public EmployeeEvaluationTemplate() { }

    public EmployeeEvaluationTemplate(EmployeeEvaluationTemplateDto employeeEvaluationTemplateDto)
    {
        Id = employeeEvaluationTemplateDto.Id;
        Description = employeeEvaluationTemplateDto.Description;
    }

    public EmployeeEvaluationTemplateDto ToDto()
    {
        return new EmployeeEvaluationTemplateDto(
            Id,
            Description);
    }
}
