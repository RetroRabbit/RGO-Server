using RGO.Models.Enums;

namespace RGO.Models;

public record EmployeeDocumentDto(
    int Id,
    EmployeeDto? Employee,
    string? Reference,
    string FileName,
    FileCategory FileCategory,
    string Blob,
    DocumentStatus? Status,
    DateTime UploadDate,
    string? Reason);
