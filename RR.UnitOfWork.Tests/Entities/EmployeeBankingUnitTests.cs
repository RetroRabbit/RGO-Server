using HRIS.Models;
using HRIS.Models.Employee.Commons;
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
            new EmployeeAddressDto{ Id = 1, UnitNumber = "2", ComplexName = "Complex", StreetNumber = "2", SuburbOrDistrict = "Suburb/District", City = "City", Country = "Country", Province = "Province", PostalCode = "1620" };

        _employee = new EmployeeDto
        {
            Id = 1,
            EmployeeNumber = "001",
            TaxNumber = "34434434",
            EngagementDate = new DateTime(),
            TerminationDate = new DateTime(),
            PeopleChampion = null,
            Disability = false,
            DisabilityNotes = "None",
            Level = 4,
            EmployeeType = employeeTypeDto,
            Notes = "Notes",
            LeaveInterval = 1,
            SalaryDays = 28,
            PayRate = 128,
            Salary = 100000,
            Name = "Dotty",
            Initials = "D",
            Surname = "Missile",
            DateOfBirth = new DateTime(),
            CountryOfBirth = "South Africa",
            Nationality = "South African",
            IdNumber = "0000000000000",
            PassportNumber = " ",
            PassportExpirationDate = new DateTime(),
            PassportCountryIssue = "South Africa",
            Race = HRIS.Models.Enums.Race.Black,
            Gender = HRIS.Models.Enums.Gender.Female,
            Email = "dm@.co.za",
            PersonalEmail = "test@gmail.com",
            CellphoneNo = "0123456789",
            PhysicalAddress = employeeAddressDto,
            PostalAddress = employeeAddressDto
        };
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
