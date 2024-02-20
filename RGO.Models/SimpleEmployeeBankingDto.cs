using HRIS.Models.Enums;

namespace HRIS.Models;

public class SimpleEmployeeBankingDto
{
    // TODO : Remove this Constructor + Update UNIT TESTS
    public SimpleEmployeeBankingDto(int Id,
                                    int EmployeeId,
                                    string BankName,
                                    string Branch,
                                    string AccountNo,
                                    EmployeeBankingAccountType AccountType,
                                    string AccountHolderName,
                                    BankApprovalStatus Status,
                                    string? DeclineReason,
                                    string File)
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
}