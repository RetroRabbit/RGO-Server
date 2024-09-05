using System.ComponentModel.DataAnnotations;

namespace HRIS.Models;

public class EmployeeCertificationDto
{
    [Required (ErrorMessage = "Employee Certification 'Id' field is missing.")]
    public int Id { get; set; }
    [Required(ErrorMessage = "Employee Certification 'EmployeeId' field is missing.")]
    public int EmployeeId { get; set; }
    [Required(ErrorMessage = "Employee Certification 'CertificateName' field is missing.")]
    public string CertificateName { get; set; }
    [Required(ErrorMessage = "Employee Certification 'IssueOrganization' field is missing.")]
    public string IssueOrganization { get; set; }
    [Required(ErrorMessage = "Employee Certification 'IssueDate' field is missing.")]
    [DataType(DataType.DateTime)]
    public DateTime IssueDate { get; set; }
    [Required(ErrorMessage = "Employee Certification 'CertificateDocument' field is missing.")]
    public byte[] CertificateDocument { get; set; } = Array.Empty<byte>();
    public string DocumentName {  get; set; }   
}
