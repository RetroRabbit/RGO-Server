using HRIS.Models;
using HRIS.Models.Employee.Commons;
using HRIS.Models.Enums;
using RR.UnitOfWork.Entities.HRIS;
using Xunit;

namespace RR.UnitOfWork.Tests.Entities;

public class EmployeeDocumentUnitTests
{
    private readonly EmployeeDto _employee;

    public EmployeeDocumentUnitTests()
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
            PassportCountryIssue = "South Africa",
            Race = Race.Black,
            Gender = Gender.Male,
            Photo = null,
            Email = "texample@retrorabbit.co.za",
            PersonalEmail = "test.example@gmail.com",
            CellphoneNo = "0000000000",
            ClientAllocated = null,
            TeamLead = null,
            PhysicalAddress = employeeAddressDto,
            PostalAddress = employeeAddressDto,
            HouseNo = null,
            EmergencyContactName = null,
            EmergencyContactNo = null
        };

    }

    public EmployeeDocument CreateEmployeeDocument(EmployeeDto? employee = null)
    {
        var employeeDocument = new EmployeeDocument
        {
            Id = 1,
            EmployeeId = 1,
            Status = DocumentStatus.Approved
        };

        if (employee != null)
            employeeDocument.Employee = new Employee(employee, employee.EmployeeType!);

        return employeeDocument;
    }

    [Fact]
    public void EmployeeDocumentTest()
    {
        var employeeDocument = new EmployeeDocument();
        Assert.IsType<EmployeeDocument>(employeeDocument);
        Assert.NotNull(employeeDocument);
    }

    [Fact]
    public void EmployeeDocumentToDTO()
    {
        var employeeDocument = CreateEmployeeDocument(
                                                      _employee);
        var dto = employeeDocument.ToDto();

        Assert.Equal(dto.EmployeeId!, employeeDocument.Employee!.Id);

        var initializedEmployeeDocument = new EmployeeDocument(dto);

        Assert.Null(initializedEmployeeDocument.Employee);

        dto = initializedEmployeeDocument.ToDto();
    }
}
