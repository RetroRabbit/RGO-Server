namespace HRIS.Models;

public class EmployeeCertificationDto
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public string CertificateName { get; set; }
    public string IssueOrganization { get; set; }
    public DateTime IssueDate { get; set; }
    public string CertificateDocument { get; set; }
    public string DocumentName {  get; set; }   
}
