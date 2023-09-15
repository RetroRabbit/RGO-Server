using RGO.UnitOfWork.Interfaces;
using RGO.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RGO.UnitOfWork.Entities
{
    [Table("EmployeeEvaluationTemplateItem")]
    public class EmployeeEvaluationTemplateItem : IModel<EmployeeEvaluationTemplateItemDto>
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("templateId")]
        [ForeignKey("EmployeeEvaluationTemplate")]
        public int TemplateId { get; set; }

        [Column("section")]
        public string Section { get; set; }

        [Column("question")]
        public string Question { get; set; }

        public virtual EmployeeEvaluationTemplate EmployeeEvaluationTemplate { get; set; }
        public EmployeeEvaluationTemplateItem() { }

        public EmployeeEvaluationTemplateItem(EmployeeEvaluationTemplateItemDto employeeEvaluationTempItemDto)
        {
            Id = employeeEvaluationTempItemDto.Id;
            TemplateId = employeeEvaluationTempItemDto.TemplateId;
            Section = employeeEvaluationTempItemDto.Section;
            Question = employeeEvaluationTempItemDto.Question;
        }

        public EmployeeEvaluationTemplateItemDto ToDto()
        {
            return new EmployeeEvaluationTemplateItemDto(
                Id,
                TemplateId,
                Section,
                Question);
        }
    }
}
