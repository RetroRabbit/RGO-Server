using System;

namespace RGO.Models;

public record AuditLogDto(
    int Id,
    int EditFor,
    int EditBy,
    DateTime EditDate,
    string Description);