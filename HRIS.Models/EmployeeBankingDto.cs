using HRIS.Models.Enums;

namespace HRIS.Models;

public class EmployeeBankingDto
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public string? BankName { get; set; }
    public string? Branch { get; set; }
    public string? AccountNo { get; set; }
    public EmployeeBankingAccountType AccountType { get; set; }
    public BankApprovalStatus Status { get; set; }
    public string? DeclineReason { get; set; }
    public string? File { get; set; }
    public DateOnly LastUpdateDate { get; set; }
    public DateOnly PendingUpdateDate { get; set; }
}
