using HRIS.Models;
using RR.UnitOfWork.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        DocumentName = certificateDto.DocumentName;
    }

    [Key][Column("id")] public int Id { get; set; }

    [Column("employeeId")][ForeignKey("Employee")] public int EmployeeId { get; set; }

    [Column("certificateName")] public string CertificateName { get; set; }

    [Column("certificateDocument")] public string CertificateDocument { get; set; }

    [Column("issueOrganization")] public string IssueOrganization { get; set; }

    [Column("issueDate")] public DateTime IssueDate { get; set; }

    [Column("documentName")] public string DocumentName { get; set; }

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
            IssueOrganization = IssueOrganization,
            DocumentName = DocumentName
        };
    }
}
