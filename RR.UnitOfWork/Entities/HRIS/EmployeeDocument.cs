﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HRIS.Models;
using HRIS.Models.Enums;
using RR.UnitOfWork.Interfaces;

namespace RR.UnitOfWork.Entities.HRIS;

[Table("EmployeeDocument")]
public class EmployeeDocument : IModel
{
    public EmployeeDocument()
    {
    }

    public EmployeeDocument(EmployeeDocumentDto employeeDocumentsDto)
    {
        Id = employeeDocumentsDto.Id;
        EmployeeId = employeeDocumentsDto.EmployeeId!;
        Reference = employeeDocumentsDto.Reference;
        FileName = employeeDocumentsDto.FileName;
        FileCategory = employeeDocumentsDto.FileCategory;
        EmployeeFileCategory = employeeDocumentsDto.EmployeeFileCategory;
        AdminFileCategory = employeeDocumentsDto.AdminFileCategory;
        Blob = employeeDocumentsDto.Blob;
        Status = employeeDocumentsDto.Status;
        UploadDate = employeeDocumentsDto.UploadDate;
        Reason = employeeDocumentsDto.Reason;
        CounterSign = employeeDocumentsDto.CounterSign;
        DocumentType = employeeDocumentsDto?.DocumentType;
        LastUpdatedDate = employeeDocumentsDto.LastUpdatedDate;
    }

    [Column("employeeId")]
    [ForeignKey("Employee")]
    public int EmployeeId { get; set; }

    [Column("reference")] public string? Reference { get; set; }

    [Column("fileName")] public string? FileName { get; set; }

    [Column("fileCategory")] public FileCategory FileCategory { get; set; }

    [Column("employeeFileCategory")] public EmployeeFileCategory EmployeeFileCategory { get; set; }

    [Column("adminFileCategory")] public AdminFileCategory AdminFileCategory { get; set; }

    [Column("blob")] public string? Blob { get; set; }

    [Column("status")] public DocumentStatus? Status { get; set; }

    [Column("uploadDate")] public DateTime UploadDate { get; set; }

    [Column("reason")] public string? Reason { get; set; }

    [Column("counterSign")] public bool CounterSign { get; set; }

    [Column("documentType")] public DocumentType? DocumentType { get; set; }
    
    [Column("lastUpdatedDate")] public DateTime LastUpdatedDate { get; set; }

    public virtual Employee? Employee { get; set; }

    [Key] [Column("id")] public int Id { get; set; }

    public EmployeeDocumentDto ToDto()
    {
        return new EmployeeDocumentDto {
                                       Id = Id,
                                       EmployeeId = EmployeeId,
                                       Reference = Reference,
                                       FileName = FileName,
                                       FileCategory = FileCategory,
                                       DocumentType = DocumentType,
                                       EmployeeFileCategory = EmployeeFileCategory,
                                       AdminFileCategory = AdminFileCategory,
                                       Blob = Blob,
                                       Status = Status,
                                       UploadDate = UploadDate,
                                       Reason = Reason,
                                       CounterSign = CounterSign,
                                       LastUpdatedDate = LastUpdatedDate
        };
    }
}
