using RGO.Models.Enums;

namespace RGO.Models;

public record EmployeeBankingDto(
    int Id,
    int EmployeeId,
    string BankName,
    string Branch,
    string AccountNo,
    EmployeeBankingAccountType AccountType,
    string AccountHolderName,
    BankApprovalStatus Status,
    string? DeclineReason,
    string File,
    DateOnly LastUpdateDate,
    DateOnly PendingUpdateDate
    );