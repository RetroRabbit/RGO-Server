using RGO.Models.Enums;

namespace RGO.Models;

public class EmployeeBankingDto
{
    // TODO : Remove this Constructor + Update UNIT TESTS
    public EmployeeBankingDto(int Id,
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
        DateOnly PendingUpdateDate)
    {
        this.Id = Id;
        this.EmployeeId = EmployeeId;
        this.BankName = BankName;
        this.Branch = Branch;
        this.AccountNo = AccountNo;
        this.AccountType = AccountType;
        this.AccountHolderName = AccountHolderName;
        this.Status = Status;
        this.DeclineReason = DeclineReason;
        this.File = File;
        this.LastUpdateDate = LastUpdateDate;
        this.PendingUpdateDate = PendingUpdateDate;
    }

    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public string BankName { get; set; }
    public string Branch { get; set; }
    public string AccountNo { get; set; }
    public EmployeeBankingAccountType AccountType { get; set; }
    public string AccountHolderName { get; set; }
    public BankApprovalStatus Status { get; set; }
    public string? DeclineReason { get; set; }
    public string File { get; set; }
    public DateOnly LastUpdateDate { get; set; }
    public DateOnly PendingUpdateDate { get; set; }
}