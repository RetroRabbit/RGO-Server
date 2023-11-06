using RGO.Models;
using RGO.Models.Enums;
using RGO.UnitOfWork.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RGO.UnitOfWork.Entities;

[Table("EmployeeDocument")]
public class EmployeeDocument : IModel<EmployeeDocumentDto>
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("employeeId")]
    [ForeignKey("Employee")]
    public int EmployeeId { get; set; }

    [Column("onboardingDocumentId")]
    [ForeignKey("OnboardingDocument")]
    public int? OnboardingDocumentId { get; set; }

    [Column("reference")]
    public string? Reference { get; set; }

    [Column("fileName")]
    public string FileName { get; set; }

    [Column("blob")]
    public string Blob { get; set; }

    [Column("status")]
    public ItemStatus? Status { get; set; }

    [Column("uploadDate")]
    public DateTime UploadDate { get; set; }

    public virtual Employee Employee { get; set; }

    public virtual OnboardingDocument? OnboardingDocument { get; set; }

    public EmployeeDocument() { }

    public EmployeeDocument(EmployeeDocumentDto employeeDocumentsDto)
    {
        Id = employeeDocumentsDto.Id;
        EmployeeId = employeeDocumentsDto.Employee!.Id;
        OnboardingDocumentId = employeeDocumentsDto.OnboardingDocument?.Id;
        Reference = employeeDocumentsDto.Reference;
        FileName = employeeDocumentsDto.FileName;
        Blob = employeeDocumentsDto.Blob;
        Status = employeeDocumentsDto.Status;
        UploadDate = employeeDocumentsDto.UploadDate;
    }

    public EmployeeDocumentDto ToDto()
    {
        return new EmployeeDocumentDto(
            Id,
            Employee?.ToDto(),
            OnboardingDocument?.ToDto(),
            Reference,
            FileName,
            Blob,
            Status,
            UploadDate);
    }

}
