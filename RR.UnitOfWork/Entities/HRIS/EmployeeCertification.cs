﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HRIS.Models;
using RR.UnitOfWork.Interfaces;

namespace RR.UnitOfWork.Entities.HRIS;

[Table("EmployeeCertification")]
public class EmployeeCertification : IModel<EmployeeCertificationDto>
{ 
    public EmployeeCertification() 
    { 
    }

    public EmployeeCertification(EmployeeCertificationDto certificateDto)
    {
        Id = certificateDto.Id;
        EmployeeId = certificateDto.EmployeeId;
        CertificateName = certificateDto.CertificateName;
        CertificateDocument = certificateDto.CertificateDocument;
        IssueOrganization = certificateDto.IssueOrganization;
        IssueDate = certificateDto.IssueDate;
    }

    [Key][Column("id")] public int Id { get; set; }

    [Column("employeeId")][ForeignKey("Employee")] public int EmployeeId { get; set; }

    [Column("certificateName")] public required string CertificateName { get; set; }

    [Column("certificateDocument")] public required string CertificateDocument { get; set; }

    [Column("issueOrganization")] public required string IssueOrganization { get; set; }

    [Column("issueDate")] public required DateTime IssueDate { get; set; }

    public virtual Employee? Employee { get; set; }

    public EmployeeCertificationDto ToDto()
    {
        return new EmployeeCertificationDto
        {
            Id = Id,
            EmployeeId = EmployeeId,
            CertificateName = CertificateName,
            CertificateDocument = CertificateDocument,
            IssueDate = IssueDate,
            IssueOrganization = IssueOrganization
        };
    }
}
