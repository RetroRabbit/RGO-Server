using HRIS.Models;
using HRIS.Models.Enums;
using RR.UnitOfWork.Entities.HRIS;
using Xunit;

namespace RR.UnitOfWork.Tests.Entities;

public class EmployeeDataUnitTests
{
    private readonly EmployeeDto _employee;
    private readonly FieldCodeDto _fieldCode;

    public EmployeeDataUnitTests()
    {
        var employeeTypeDto = new EmployeeTypeDto{ Id = 1, Name = "Developer" };
        var employeeAddressDto =
            new EmployeeAddressDto{ Id = 1, UnitNumber = "2", ComplexName = "Complex", StreetNumber = "2", SuburbOrDistrict = "Suburb/District", City = "City", Country = "Country", Province = "Province", PostalCode = "1620" };

        _employee = new EmployeeDto
        {
            Id = 1,
            EmployeeNumber = "001",
            TaxNumber = "34434434",
            EngagementDate = DateTime.Now,
            TerminationDate = DateTime.Now,
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
            DateOfBirth = DateTime.Now,
            CountryOfBirth = "South Africa",
            Nationality = "South African",
            IdNumber = "0000080000000",
            PassportNumber = " ",
            PassportExpirationDate = DateTime.Now,
            PassportCountryIssue = null,
            Race = Race.Black,
            Gender = Gender.Male,
            Email = "texample@retrorabbit.co.za",
            PersonalEmail = "test.example@gmail.com",
            CellphoneNo = "0000000000",
            PhysicalAddress = employeeAddressDto,
            PostalAddress = employeeAddressDto
        };

        _fieldCode = new FieldCodeDto
        {
            Id = 1,
            Code = "email01",
            Name = "Email",
            Description = "desciption",
            Regex = "@(\\w+).co.za",
            Type = FieldCodeType.String,
            Status = ItemStatus.Active,
            Internal = true,
            InternalTable = "Employee",
            Category = 0,
            Required = false
        };
    }

    [Fact]
    public void EmployeeDataTest()
    {
        var employeeData = new EmployeeData();
        Assert.IsType<EmployeeData>(employeeData);
        Assert.NotNull(employeeData);
    }

    public EmployeeData CreateEmployeeDataEntity(EmployeeDto? employee = null, FieldCodeDto? fieldCode = null)
    {
        var employeeData = new EmployeeData
        {
            Id = 1,
            EmployeeId = 1,
            FieldCodeId = 1,
            Value = "Value"
        };

        if (employee != null)
            employeeData.Employee = new Employee(employee, employee.EmployeeType!);

        if (fieldCode != null)
            employeeData.FieldCode = new FieldCode(fieldCode);

        return employeeData;
    }

    [Fact]
    public void EmployeeDataToDTO()
    {
        var employeeData = CreateEmployeeDataEntity(
                                                    _employee,
                                                    _fieldCode);
        var dto = employeeData.ToDto();

        Assert.Equal(dto.Value, employeeData.Value);

        var initializedEmployeeData = new EmployeeData(dto);

        Assert.Null(initializedEmployeeData.Employee);
        Assert.Null(initializedEmployeeData.FieldCode);

        employeeData = CreateEmployeeDataEntity();
        dto = employeeData.ToDto();

        Assert.Equal(dto.EmployeeId, employeeData.EmployeeId);
        Assert.Equal(dto.FieldCodeId, employeeData.FieldCodeId);
    }
}
