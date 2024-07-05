using HRIS.Models.Enums;
using RR.UnitOfWork.Entities.HRIS;

namespace RR.Tests.Data.Models.HRIS;

public class EmployeeBankingTestData
{
    public static EmployeeBanking EmployeeBankingOne = new()
    {
        Id = 1,
        EmployeeId = 1,
        BankName = "FNB",
        Branch = "Not Sure",
        AccountNo = "120",
        AccountType = EmployeeBankingAccountType.Savings,
        Status = BankApprovalStatus.PendingApproval,
        DeclineReason = "",
        File = "asd",
        LastUpdateDate = new DateOnly(),
        PendingUpdateDate = new DateOnly()
    };

    public static EmployeeBanking EmployeeBankingTwo = new()
    {
        Id = 2,
        EmployeeId = 2,
        BankName = "FNB",
        Branch = "Not Sure",
        AccountNo = "120",
        AccountType = EmployeeBankingAccountType.Savings,
        Status = BankApprovalStatus.PendingApproval,
        DeclineReason = "",
        File = "asd",
        LastUpdateDate = new DateOnly(),
        PendingUpdateDate = new DateOnly()
    };

    public static EmployeeBanking EmployeeBankingThree = new()
    {
        Id = 3,
        EmployeeId = 3,
        BankName = "FNB",
        Branch = "Not Sure",
        AccountNo = "120",
        AccountType = EmployeeBankingAccountType.Savings,
        Status = BankApprovalStatus.PendingApproval,
        DeclineReason = "",
        File = "asd",
        LastUpdateDate = new DateOnly(),
        PendingUpdateDate = new DateOnly()
    };

    public static EmployeeBanking EmployeeBankingNew = new()
    {
        Id = 0,
        EmployeeId = 4,
        BankName = "FNB",
        Branch = "Not Sure",
        AccountNo = "120",
        AccountType = EmployeeBankingAccountType.Savings,
        Status = BankApprovalStatus.PendingApproval,
        DeclineReason = "",
        File = "asd",
        LastUpdateDate = new DateOnly(),
        PendingUpdateDate = new DateOnly()
    };

    public static EmployeeBanking GetModifiedEmployeeBankingDtoWithEmployeeId(int employeeId)
    {
        return new EmployeeBanking
        {
            Id = 0,
            EmployeeId = employeeId,
            BankName = "FNB",
            Branch = "Not Sure",
            AccountNo = "120",
            AccountType = EmployeeBankingAccountType.Savings,
            Status = BankApprovalStatus.PendingApproval,
            DeclineReason = "",
            File = "asd",
            LastUpdateDate = new DateOnly(),
            PendingUpdateDate = new DateOnly()
        };
    }
    
    public static EmployeeBanking GetModifiedEmployeeBankingDtoWithEmployeeIdAndBankingId(int bankingId, int employeeId)
    {
        return new EmployeeBanking
        {
            Id = bankingId,
            EmployeeId = employeeId,
            BankName = "FNB",
            Branch = "Not Sure",
            AccountNo = "120",
            AccountType = EmployeeBankingAccountType.Savings,
            Status = BankApprovalStatus.PendingApproval,
            DeclineReason = "",
            File = "asd",
            LastUpdateDate = new DateOnly(),
            PendingUpdateDate = new DateOnly()
        };
    }
}
