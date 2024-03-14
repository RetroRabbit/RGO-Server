using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HRIS.Models;
using HRIS.Models.Enums;
using RR.UnitOfWork.Entities.HRIS;
using RR.UnitOfWork.Interfaces;

namespace RR.UnitOfWork.Entities;

[Table("AuditLogs")]
public class AuditLog : IModel<AuditLogDto>
{
    public AuditLog()
    {
    }

    public AuditLog(AuditLogDto auditLogsDto)
    {
        Id = auditLogsDto.Id;
        Date = auditLogsDto.Date;
        CRUDOperation = auditLogsDto.CRUDOperation;
        CreatedBy = new Employee(auditLogsDto.CreatedBy!, auditLogsDto.CreatedBy!.EmployeeType!);
        CreatedById = auditLogsDto.CreatedBy.Id;
        Table = auditLogsDto.Table;
        Data = auditLogsDto.Data;
    }

    [Key] 
    [Column("id")] 
    public int Id { get; set; }

    [Column("Date")] 
    public DateTime Date { get; set; }

    [Column("CRUDOperation")]
    public CRUDOperations CRUDOperation { get; set; }

    [Column("createdById")]
    [ForeignKey("createdBy")]
    public int CreatedById { get; set; }

    [Column("table")]
    public string? Table { get; set; }

    [Column("data")] 
    public string? Data { get; set; }

    public virtual Employee? CreatedBy { get; set; }


    public AuditLogDto ToDto()
    {
        return new AuditLogDto
        {
            Id = Id,
            Date = Date,
            CRUDOperation = CRUDOperation,
            CreatedBy = CreatedBy!.ToDto(),
            Table = Table,
            Data = Data
        };
    }
}