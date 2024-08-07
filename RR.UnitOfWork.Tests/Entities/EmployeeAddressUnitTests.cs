using HRIS.Models;
using HRIS.Models.Enums;
using RR.UnitOfWork.Entities.HRIS;
using Xunit;

namespace RR.UnitOfWork.Tests.Entities;

public class EmployeeAddressUnitTests
{
    private EmployeeDto _employee;

    public EmployeeAddressUnitTests()
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
            Name = "Dorothy",
            Initials = "D",
            Surname = "Mahoko",
            DateOfBirth = new DateTime(),
            CountryOfBirth = "South Africa",
            Nationality = "South African",
            IdNumber = "0000080000000",
            PassportNumber = " ",
            PassportExpirationDate = new DateTime(),
            PassportCountryIssue = null,
            Race = Race.Black,
            Gender = Gender.Male,
            Email = "texample@retrorabbit.co.za",
            PersonalEmail = "test.example@gmail.com",
            CellphoneNo = "0000000000",
            //PhysicalAddress = employeeAddressDto,
            //PostalAddress = employeeAddressDto
        };
    }

    public EmployeeAddress CreateEmployeeAddress()
    {
        var employeeAddress = new EmployeeAddress
        {
            Id = 1,
            UnitNumber = "1",
            ComplexName = "Complex",
            StreetNumber = "1",
            SuburbOrDistrict = "Suburb/District",
            Country = "Country",
            Province = "Province",
            PostalCode = "1620"
        };

        return employeeAddress;
    }

    [Fact]
    public void EmployeeAddressTest()
    {
        var employeeAddress = new EmployeeAddress();
        Assert.IsType<EmployeeAddress>(employeeAddress);
        Assert.NotNull(employeeAddress);
    }

    [Fact]
    public void EmployeeAddressToDTO()
    {
        var employeeAddress = CreateEmployeeAddress();
        var employeeAddressDto = employeeAddress.ToDto();

        Assert.Equal(employeeAddress.Id, employeeAddressDto.Id);
        Assert.Equal(employeeAddress.UnitNumber, employeeAddressDto.UnitNumber);
        Assert.Equal(employeeAddress.ComplexName, employeeAddressDto.ComplexName);
        Assert.Equal(employeeAddress.StreetNumber, employeeAddressDto.StreetNumber);
        Assert.Equal(employeeAddress.SuburbOrDistrict, employeeAddressDto.SuburbOrDistrict);
        Assert.Equal(employeeAddress.Country, employeeAddressDto.Country);
        Assert.Equal(employeeAddress.Province, employeeAddressDto.Province);
        Assert.Equal(employeeAddress.PostalCode, employeeAddressDto.PostalCode);
    }
}
