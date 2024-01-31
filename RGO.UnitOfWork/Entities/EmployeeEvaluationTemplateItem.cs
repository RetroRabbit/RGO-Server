using RGO.Models;
using RGO.UnitOfWork.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RGO.UnitOfWork.Entities;

[Table("EmployeeEvaluationTemplateItem")]
public class EmployeeEvaluationTemplateItem : IModel<EmployeeEvaluationTemplateItemDto>
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("templateId")]
    [ForeignKey("Template")]
    public int TemplateId { get; set; }

    [Column("section")]
    public string Section { get; set; } = string.Empty;

    [Column("question")]
    public string Question { get; set; } = string.Empty;

    public virtual EmployeeEvaluationTemplate Template { get; set; } = null!;

    public EmployeeEvaluationTemplateItem() { }

    public EmployeeEvaluationTemplateItem(EmployeeEvaluationTemplateItemDto employeeEvaluationTempItemDto)
    {
        Id = employeeEvaluationTempItemDto.Id;
        TemplateId = employeeEvaluationTempItemDto.Template!.Id;
        Section = employeeEvaluationTempItemDto.Section;
        Question = employeeEvaluationTempItemDto.Question;
    }

    public EmployeeEvaluationTemplateItemDto ToDto()
    {
        return new EmployeeEvaluationTemplateItemDto(
            Id,
            Template?.ToDto(),
            Section,
            Question);
    }
}
