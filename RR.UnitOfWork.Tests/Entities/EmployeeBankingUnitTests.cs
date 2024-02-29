using HRIS.Models;
using HRIS.Models.Enums;
using RR.UnitOfWork.Entities.HRIS;
using Xunit;

namespace RR.UnitOfWork.Tests.Entities;

public class EmployeeBankingUnitTests
{
    private readonly EmployeeDto _employee;

    public EmployeeBankingUnitTests()
    {
        var employeeTypeDto = new EmployeeTypeDto{ Id = 1, Name = "Developer" };
        var employeeAddressDto =
            new EmployeeAddressDto(1, "2", "Complex", "2", "Suburb/District", "City", "Country", "Province", "1620");

        _employee = new EmployeeDto(1, "001", "34434434", new DateTime(), new DateTime(),
                                    null, false, "None", 4, employeeTypeDto, "Notes", 1, 28, 128, 100000, "Dorothy",
                                    "D",
                                    "Mahoko", new DateTime(), "South Africa", "South African", "0000080000000", " ",
                                    new DateTime(), null, Race.Black, Gender.Male, null,
                                    "texample@retrorabbit.co.za", "test.example@gmail.com", "0000000000", null, null,
                                    employeeAddressDto, employeeAddressDto, null, null, null);
    }

    public EmployeeBanking CreateEmployeeBanking(EmployeeDto? employee = null)
    {
        var employeeBanking = new EmployeeBanking
        {
            Id = 1,
            EmployeeId = 1,
            BankName = "BankName",
            Branch = "Branch",
            AccountNo = "AccountNo",
            AccountType = EmployeeBankingAccountType.Cheque,
            AccountHolderName = "AccountHolderName"
        };

        if (employee != null)
            employeeBanking.Employee = new Employee(employee, employee.EmployeeType!);

        return employeeBanking;
    }

    [Fact]
    public void EmployeeBankingTest()
    {
        var employeeBanking = new EmployeeBanking();
        Assert.IsType<EmployeeBanking>(employeeBanking);
        Assert.NotNull(employeeBanking);
    }

    [Fact]
    public void EmployeeBankingToDTO()
    {
        var employeeBanking = CreateEmployeeBanking(_employee);
        var dto = employeeBanking.ToDto();

        Assert.Equal(employeeBanking.EmployeeId, dto.EmployeeId);

        var initializedEmployeeBanking = new EmployeeBanking(dto);

        Assert.Null(initializedEmployeeBanking.Employee);
    }
}
