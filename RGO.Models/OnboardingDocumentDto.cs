using RGO.Models.Enums;

namespace RGO.Models;

public record OnboardingDocumentDto(
    int Id,
    string Title,
    string Description,
    string FileName,
    byte[] Blob,
    ItemStatus Status);