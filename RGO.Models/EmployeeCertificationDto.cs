using RGO.Models.Enums;

namespace RGO.Models;

public class EmployeeCertificationDto
{
    // TODO : Remove this Constructor + Update UNIT TESTS
    public EmployeeCertificationDto(int Id,
        EmployeeDto? Employee,
        EmployeeDocumentDto? EmployeeDocument,
        string Title,
        string Publisher,
        EmployeeCertificationStatus Status,
        EmployeeDto? AuditBy,
        DateTime? AuditDate,
        string? AuditNote)
    {
        this.Id = Id;
        this.Employee = Employee;
        this.EmployeeDocument = EmployeeDocument;
        this.Title = Title;
        this.Publisher = Publisher;
        this.Status = Status;
        this.AuditBy = AuditBy;
        this.AuditDate = AuditDate;
        this.AuditNote = AuditNote;
    }

    public int Id { get; set; }
    public EmployeeDto? Employee { get; set; }
    public EmployeeDocumentDto? EmployeeDocument { get; set; }
    public string Title { get; set; }
    public string Publisher { get; set; }
    public EmployeeCertificationStatus Status { get; set; }
    public EmployeeDto? AuditBy { get; set; }
    public DateTime? AuditDate { get; set; }
    public string? AuditNote { get; set; }
}