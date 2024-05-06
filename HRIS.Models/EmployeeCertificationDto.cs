using HRIS.Models.Enums;

namespace HRIS.Models;

public class EmployeeCertificationDto
{
    public int Id { get; set; }
    public int EmployeeId {  get; set; }
    public required string CertificateName {  get; set; }
    public required string IssueOrganization {  get; set; }
    public required DateTime IssueDate { get; set; }
    public required string CertificateDocument { get; set; }
}
