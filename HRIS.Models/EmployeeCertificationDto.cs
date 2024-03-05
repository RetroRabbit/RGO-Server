using HRIS.Models.Enums;

namespace HRIS.Models;

public class EmployeeCertificationDto
{
    public int Id { get; set; }
    public EmployeeDto? Employee { get; set; }
    public EmployeeDocumentDto? EmployeeDocument { get; set; }
    public string? Title { get; set; }
    public string? Publisher { get; set; }
    public EmployeeCertificationStatus Status { get; set; }
    public EmployeeDto? AuditBy { get; set; }
    public DateTime? AuditDate { get; set; }
    public string? AuditNote { get; set; }
}
