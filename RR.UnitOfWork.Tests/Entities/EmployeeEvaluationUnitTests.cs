using HRIS.Models;
using HRIS.Models.Enums;
using RR.UnitOfWork.Entities.HRIS;
using Xunit;

namespace RR.UnitOfWork.Tests.Entities;

public class EmployeeEvaluationUnitTests
{
    private readonly EmployeeDto _employee;
    private readonly EmployeeEvaluationTemplateDto _template;

    public EmployeeEvaluationUnitTests()
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

        _template = new EmployeeEvaluationTemplateDto{ Id = 1, Description = "Template" };
    }

    private EmployeeEvaluationDto CreateEmployeeEvaluation(
        EmployeeDto? employee = null,
        EmployeeEvaluationTemplateDto? template = null,
        EmployeeDto? owner = null)
    {
        var entity = new EmployeeEvaluation
        {
            Id = 0,
            Subject = "Subject",
            StartDate = DateOnly.FromDateTime(DateTime.Now)
        };

        if (employee != null)
            entity.Employee = new Employee(employee, employee.EmployeeType!);

        if (template != null)
            entity.Template = new EmployeeEvaluationTemplate(template);

        if (owner != null)
            entity.Owner = new Employee(owner, owner.EmployeeType!);

        return entity.ToDto();
    }

    [Fact]
    public void InitialoizationTest()
    {
        var employeeEvaluation = new EmployeeEvaluation();
        Assert.NotNull(employeeEvaluation);
    }

    [Fact]
    public void InitialoizationWithDtoTest()
    {
        var employeeEvaluationDto = CreateEmployeeEvaluation(_employee, _template, _employee);
        var employeeEvaluation = new EmployeeEvaluation(employeeEvaluationDto);
        var dto = employeeEvaluation.ToDto();

        Assert.Null(dto.Employee);
        Assert.Null(dto.Template);
        Assert.Null(dto.Owner);
    }
}
