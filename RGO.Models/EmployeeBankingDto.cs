using RGO.Models.Enums;

namespace RGO.Models;

public record EmployeeBankingDto(
    int Id,
    EmployeeDto? Employee,
    string BankName,
    string Branch,
    string AccountNo,
    EmployeeBankingAccountType AccountType,
    string AccountHolderName
    );