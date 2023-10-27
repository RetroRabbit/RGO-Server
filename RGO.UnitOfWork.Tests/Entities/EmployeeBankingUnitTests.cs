using RGO.Models;
using RGO.UnitOfWork.Entities;
using Xunit;

namespace RGO.UnitOfWork.Tests.Entities;

public class EmployeeBankingUnitTests
{
    private EmployeeDto _employee;

    public EmployeeBankingUnitTests()
    {
        EmployeeTypeDto employeeTypeDto = new EmployeeTypeDto(1, "Developer");
        EmployeeAddressDto employeeAddressDto = new EmployeeAddressDto(1, "2", "Complex", "2", "Suburb/District", "City", "Country", "Province", "1620");

        _employee = new EmployeeDto(1, "001", "34434434", new DateOnly(), new DateOnly(),
            null, false, "None", 4, employeeTypeDto, "Notes", 1, 28, 128, 100000, "Ms", "Dorothy", "D",
            "Mahoko", new DateOnly(), "South Africa", "South African", "0000080000000", " ",
            new DateOnly(), null, Models.Enums.Race.Black, Models.Enums.Gender.Male, null,
            "texample@retrorabbit.co.za", "test.example@gmail.com", "0000000000", null, null, employeeAddressDto, employeeAddressDto, null, null, null);
    }

    public EmployeeBanking CreateEmployeeBanking(EmployeeDto? employee = null)
    {
        EmployeeBanking employeeBanking = new EmployeeBanking
        {
            Id = 1,
            EmployeeId = 1,
            BankName = "BankName",
            Branch = "Branch",
            AccountNo = "AccountNo",
            AccountType = Models.Enums.EmployeeBankingAccountType.Cheque,
            AccountHolderName = "AccountHolderName"
        };

        if (employee != null)
            employeeBanking.Employee = new Employee(employee, employee.EmployeeType);

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
        var employeeBanking = CreateEmployeeBanking(employee: _employee);
        var dto = employeeBanking.ToDto();

        Assert.Equal(employeeBanking.EmployeeId, dto.EmployeeId);

        var initializedEmployeeBanking = new EmployeeBanking(dto);

        Assert.Null(initializedEmployeeBanking.Employee);
    }
}
