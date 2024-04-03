using HRIS.Models;
using HRIS.Models.Enums;

namespace RR.Tests.Data.Models.HRIS;

public class EmployeeBankingTestData
{
    public static EmployeeBankingDto EmployeeBankingDto = new EmployeeBankingDto
    {
        Id = 1,
        EmployeeId = 1,
        BankName = "FNB",
        Branch = "Not Sure",
        AccountNo = "120",
        AccountType = EmployeeBankingAccountType.Savings,
        AccountHolderName = "Name1",
        Status = BankApprovalStatus.PendingApproval,
        DeclineReason = "",
        File = "asd",
        LastUpdateDate = new DateOnly(),
        PendingUpdateDate = new DateOnly()
    };

    public static EmployeeBankingDto EmployeeBankingDto2 = new EmployeeBankingDto
    {
        Id = 2,
        EmployeeId = 2,
        BankName = "FNB",
        Branch = "Not Sure",
        AccountNo = "120",
        AccountType = EmployeeBankingAccountType.Savings,
        AccountHolderName = "Name1",
        Status = BankApprovalStatus.PendingApproval,
        DeclineReason = "",
        File = "asd",
        LastUpdateDate = new DateOnly(),
        PendingUpdateDate = new DateOnly()
    };

    public static EmployeeBankingDto EmployeeBankingDto3 = new EmployeeBankingDto
    {
        Id = 3,
        EmployeeId = 3,
        BankName = "FNB",
        Branch = "Not Sure",
        AccountNo = "120",
        AccountType = EmployeeBankingAccountType.Savings,
        AccountHolderName = "Name1",
        Status = BankApprovalStatus.PendingApproval,
        DeclineReason = "",
        File = "asd",
        LastUpdateDate = new DateOnly(),
        PendingUpdateDate = new DateOnly()
    };

    public static EmployeeBankingDto EmployeeBankingDtoNew = new EmployeeBankingDto()
    {
        Id = 0,
        EmployeeId = 4,
        BankName = "FNB",
        Branch = "Not Sure",
        AccountNo = "120",
        AccountType = EmployeeBankingAccountType.Savings,
        AccountHolderName = "Name1",
        Status = BankApprovalStatus.PendingApproval,
        DeclineReason = "",
        File = "asd",
        LastUpdateDate = new DateOnly(),
        PendingUpdateDate = new DateOnly()
    };

    public static EmployeeBankingDto GetModifiedEmployeeBankingDtoWithEmployeeId(int employeeId)
    {
        return new EmployeeBankingDto
        {
            Id = 0,
            EmployeeId = employeeId,
            BankName = "FNB",
            Branch = "Not Sure",
            AccountNo = "120",
            AccountType = EmployeeBankingAccountType.Savings,
            AccountHolderName = "Name1",
            Status = BankApprovalStatus.PendingApproval,
            DeclineReason = "",
            File = "asd",
            LastUpdateDate = new DateOnly(),
            PendingUpdateDate = new DateOnly()
        };
    }
    
    public static EmployeeBankingDto GetModifiedEmployeeBankingDtoWithEmployeeIdAndBankingId(int bankingId, int employeeId)
    {
        return new EmployeeBankingDto
        {
            Id = bankingId,
            EmployeeId = employeeId,
            BankName = "FNB",
            Branch = "Not Sure",
            AccountNo = "120",
            AccountType = EmployeeBankingAccountType.Savings,
            AccountHolderName = "Name1",
            Status = BankApprovalStatus.PendingApproval,
            DeclineReason = "",
            File = "asd",
            LastUpdateDate = new DateOnly(),
            PendingUpdateDate = new DateOnly()
        };
    }
}
