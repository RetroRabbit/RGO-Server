using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HRIS.Models;
using HRIS.Models.Enums;
using RR.UnitOfWork.Interfaces;

namespace RR.UnitOfWork.Entities.HRIS;

[Table("EmployeeCertification")]
public class EmployeeCertification : IModel<EmployeeCertificationDto>
{
    public EmployeeCertification()
    {
    }

    public EmployeeCertification(EmployeeCertificationDto employeeCertificationDto)
    {
        Id = employeeCertificationDto.Id;
        EmployeeId = employeeCertificationDto.Employee!.Id;
        EmployeeDocumentId = employeeCertificationDto.EmployeeDocument!.Id;
        Title = employeeCertificationDto.Title;
        Publisher = employeeCertificationDto.Publisher;
        Status = employeeCertificationDto.Status;
        AuditBy = employeeCertificationDto.AuditBy!.Id;
        AuditDate = employeeCertificationDto.AuditDate;
        AuditNote = employeeCertificationDto.AuditNote;
    }

    [Column("employeeId")]
    [ForeignKey("Employee")]
    public int EmployeeId { get; set; }

    [Column("employeeDocumentId")]
    [ForeignKey("EmployeeDocument")]
    public int EmployeeDocumentId { get; set; }

    [Column("title")] public string? Title { get; set; }

    [Column("publisher")] public string? Publisher { get; set; }

    [Column("status")] public EmployeeCertificationStatus Status { get; set; }

    [Column("auditBy")]
    [ForeignKey("EmployeeAuditBy")]
    public int? AuditBy { get; set; }

    [Column("auditDate")] public DateTime? AuditDate { get; set; }

    [Column("auditNote")] public string? AuditNote { get; set; }

    public virtual Employee? Employee { get; set; }
    public virtual Employee? EmployeeAuditBy { get; set; }
    public virtual EmployeeDocument? EmployeeDocument { get; set; }

    [Key] [Column("id")] public int Id { get; set; }

    public EmployeeCertificationDto ToDto()
    {
        return new EmployeeCertificationDto
        {
            Id = Id,
            Employee = Employee?.ToDto(),
            EmployeeDocument = EmployeeDocument?.ToDto(),
            Title = Title,
            Publisher = Publisher,
            Status = Status,
            AuditBy = EmployeeAuditBy?.ToDto(),
            AuditDate = AuditDate,
            AuditNote = AuditNote
        };
    }
}
