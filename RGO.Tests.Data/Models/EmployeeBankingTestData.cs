using RGO.Models;
using RGO.Models.Enums;

namespace RGO.Tests.Data.Models;

public class EmployeeBankingTestData
{
    public static EmployeeBankingDto EmployeeBankingDto = new(1, 1, "FNB", "Not Sure", "120",
        EmployeeBankingAccountType.Savings, "Name1", BankApprovalStatus.PendingApproval, "", "asd", new DateOnly(),
        new DateOnly());


    public static EmployeeBankingDto EmployeeBankingDto2 = new(2, 2, "FNB", "Not Sure", "120",
        EmployeeBankingAccountType.Savings, "Name1", BankApprovalStatus.PendingApproval, "", "asd", new DateOnly(),
        new DateOnly());

    public static EmployeeBankingDto EmployeeBankingDto3 = new(3, 3, "FNB", "Not Sure", "120",
        EmployeeBankingAccountType.Savings, "Name1", BankApprovalStatus.PendingApproval, "", "asd", new DateOnly(),
        new DateOnly());
}