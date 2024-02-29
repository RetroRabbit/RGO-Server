using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HRIS.Models;
using RR.UnitOfWork.Interfaces;

namespace RR.UnitOfWork.Entities.HRIS;

[Table("AuditLogs")]
public class AuditLog : IModel<AuditLogDto>
{
    public AuditLog()
    {
    }

    public AuditLog(AuditLogDto auditLogsDto)
    {
        Id = auditLogsDto.Id;
        EditFor = auditLogsDto.EditFor!.Id;
        EditBy = auditLogsDto.EditBy!.Id;
        EditDate = auditLogsDto.EditDate;
        Description = auditLogsDto.Description;
    }

    [Column("editFor")]
    [ForeignKey("EmployeeEditFor")]
    public int EditFor { get; set; }

    [Column("editBy")]
    [ForeignKey("EmployeeEditBy")]
    public int EditBy { get; set; }

    [Column("editDate")] public DateTime EditDate { get; set; }

    [Column("description")] public string Description { get; set; }

    public virtual Employee EmployeeEditFor { get; set; }
    public virtual Employee EmployeeEditBy { get; set; }

    [Key] [Column("id")] public int Id { get; set; }

    public AuditLogDto ToDto()
    {
        return new AuditLogDto
        {
            Id = Id,
            EditFor = EmployeeEditFor?.ToDto(),
            EditBy = EmployeeEditBy?.ToDto(),
            EditDate = EditDate,
            Description = Description
        };
    }
}