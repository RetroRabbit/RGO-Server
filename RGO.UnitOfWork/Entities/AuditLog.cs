using RGO.Models;
using RGO.UnitOfWork.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RGO.UnitOfWork.Entities;

[Table("AuditLogs")]
public class AuditLog : IModel<AuditLogDto>
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("editFor")]
    public int EditFor { get; set; }

    [Column("editBy")]
    public int EditBy { get; set; }

    [Column("editDate")]
    public DateTime EditDate { get; set; }

    [Column("description")]
    public string Description { get; set; }

    public AuditLog() { }

    public AuditLog(AuditLogDto auditLogsDto)
    {
        Id = auditLogsDto.Id;
        EditFor = auditLogsDto.EditFor;
        EditBy = auditLogsDto.EditBy;
        EditDate = auditLogsDto.EditDate;
        Description = auditLogsDto.Description;
    }

    public AuditLogDto ToDto()
    {
        return new AuditLogDto(
            Id,
            EditFor,
            EditBy,
            EditDate,
            Description);
    }
}
