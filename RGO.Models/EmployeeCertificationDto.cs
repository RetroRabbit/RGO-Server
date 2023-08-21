using RGO.Models.Enums;

namespace RGO.Models;

public record EmployeeCertificationDto(
    int Id,
    EmployeeDto Employee,
    EmployeeDocumentDto EmployeeDocument,
    string Title,
    string Publisher,
    EmployeeCertificationStatus Status,
    int? AuditBy,
    DateTime? AuditDate,
    string? AuditNote);