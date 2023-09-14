using RGO.Models;
using RGO.UnitOfWork.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace RGO.UnitOfWork.Entities
{
    [Table("EmployeeEvaluations")]
    public class EmployeeEvaluation : IModel<EmployeeEvaluationDto>
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("employeeId")]
        [ForeignKey("Employee")]
        public int EmployeeId { get; set; }

        [Column("templateId")]
        public int TemplateId { get; set; }

        [Column("ownerId")]
        public int OwnerId { get; set; }

        [Column("subject")]
        public string Subject { get; set; }

        [Column("startDate")]
        public DateTime StartDate { get; set; }

        [Column("endDate")]
        public DateTime? EndDate { get; set; }

        public virtual Employee Employee { get; set; }

        public EmployeeEvaluation() { }

        public EmployeeEvaluation(EmployeeEvaluationDto employeeEvaluationDto)
        {
            Id = employeeEvaluationDto.Id;
            EmployeeId = employeeEvaluationDto.EmployeeId;
            TemplateId = employeeEvaluationDto.TemplateId;
            OwnerId = employeeEvaluationDto.OwnerId;
            Subject = employeeEvaluationDto.Subject;
            StartDate = employeeEvaluationDto.StartDate;
            EndDate = employeeEvaluationDto.EndDate;
        }

        public EmployeeEvaluationDto ToDto()
        {
            return new EmployeeEvaluationDto(
                Id,
                EmployeeId,
                TemplateId,
                OwnerId,
                Subject,
                StartDate,
                EndDate);
        }
    }
}
