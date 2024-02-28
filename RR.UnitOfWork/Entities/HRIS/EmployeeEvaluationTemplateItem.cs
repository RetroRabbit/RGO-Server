using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HRIS.Models;
using RR.UnitOfWork.Interfaces.HRIS;

namespace RR.UnitOfWork.Entities.HRIS;

[Table("EmployeeEvaluationTemplateItem")]
public class EmployeeEvaluationTemplateItem : IModel<EmployeeEvaluationTemplateItemDto>
{
    public EmployeeEvaluationTemplateItem()
    {
    }

    public EmployeeEvaluationTemplateItem(EmployeeEvaluationTemplateItemDto employeeEvaluationTempItemDto)
    {
        Id = employeeEvaluationTempItemDto.Id;
        TemplateId = employeeEvaluationTempItemDto.Template!.Id;
        Section = employeeEvaluationTempItemDto.Section;
        Question = employeeEvaluationTempItemDto.Question;
    }

    [Column("templateId")]
    [ForeignKey("Template")]
    public int TemplateId { get; set; }

    [Column("section")] public string Section { get; set; } = string.Empty;

    [Column("question")] public string Question { get; set; } = string.Empty;

    public virtual EmployeeEvaluationTemplate Template { get; set; } = null!;

    [Key] [Column("id")] public int Id { get; set; }

    public EmployeeEvaluationTemplateItemDto ToDto()
    {
        return new EmployeeEvaluationTemplateItemDto(
                                                     Id,
                                                     Template?.ToDto(),
                                                     Section,
                                                     Question);
    }
}