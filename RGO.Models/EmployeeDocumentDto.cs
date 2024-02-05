﻿using RGO.Models.Enums;

namespace RGO.Models;

public record EmployeeDocumentDto(
    int Id,
    int EmployeeId,
    string? Reference,
    string FileName,
    FileCategory FileCategory,
    string Blob,
    DocumentStatus? Status,
    DateTime UploadDate,
    string? Reason,
    bool CounterSign);
