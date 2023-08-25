using RGO.Models.Enums;

namespace RGO.Models;

public record EmployeeDocumentDto(
    int Id,
    EmployeeDto Employee,
    OnboardingDocumentDto? OnboardingDocument,
    string Reference,
    string FileName,
    byte[] Blob,
    ItemStatus Status,
    DateTime UploadDate);