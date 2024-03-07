using HRIS.Models;
using HRIS.Models.Enums;
using RR.UnitOfWork.Entities.HRIS;
using Xunit;

namespace RR.UnitOfWork.Tests.Entities;

public class AuditLogUnitTests
{
    public EmployeeDto CreateTestEmployee()
    {
        var employeeTypeDto = new EmployeeTypeDto{ Id = 1, Name = "Developer" };
        var employeeAddressDto =
            new EmployeeAddressDto{ Id = 1, UnitNumber = "2", ComplexName = "Complex", StreetNumber = "2", SuburbOrDistrict = "Suburb/District", City = "City", Country = "Country", Province = "Province", PostalCode = "1620" };

        var testEmployee = new EmployeeDto
        {
            Id = 1,
            EmployeeNumber = "001",
            TaxNumber = "34434434",
            EngagementDate = DateTime.Now,
            TerminationDate = null,
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

        return testEmployee;
    }

    public AuditLog CreateTestAuditLog(Employee? editBy = null, Employee? editFor = null)
    {
        var auditLog = new AuditLog
        {
            Id = 1,
            Description = "Description",
            EditBy = 1,
            EditFor = 1,
            EditDate = DateTime.Now
        };

        if (editBy != null)
            auditLog.EmployeeEditBy = editBy;

        if (editFor != null)
            auditLog.EmployeeEditFor = editFor;

        return auditLog;
    }

    [Fact]
    public void auditLogTest()
    {
        var auditLog = new AuditLog();
        Assert.IsType<AuditLog>(auditLog);
        Assert.NotNull(auditLog);
    }

    [Fact]
    public void AuditLogToDtoTest()
    {
        var testEmployee = CreateTestEmployee();

        var auditLogs = new List<AuditLog>
        {
            CreateTestAuditLog(),
            CreateTestAuditLog(new Employee(testEmployee, testEmployee.EmployeeType!)),
            CreateTestAuditLog(editFor: new Employee(testEmployee, testEmployee.EmployeeType!)),
            CreateTestAuditLog(
                               new Employee(testEmployee, testEmployee.EmployeeType!),
                               new Employee(testEmployee, testEmployee.EmployeeType!))
        };

        var auditLogDto = auditLogs[0].ToDto();
        Assert.IsType<AuditLogDto>(auditLogDto);
        Assert.Null(auditLogDto.EditFor);
        Assert.Null(auditLogDto.EditBy);
        Assert.Equal(auditLogs[0].Id, auditLogDto.Id);

        auditLogDto = auditLogs[1].ToDto();
        Assert.IsType<AuditLogDto>(auditLogDto);
        Assert.Null(auditLogDto.EditFor);
        Assert.NotNull(auditLogDto.EditBy);
        Assert.Equal(auditLogs[1].Id, auditLogDto.Id);

        auditLogDto = auditLogs[2].ToDto();
        Assert.IsType<AuditLogDto>(auditLogDto);
        Assert.NotNull(auditLogDto.EditFor);
        Assert.Null(auditLogDto.EditBy);
        Assert.Equal(auditLogs[2].Id, auditLogDto.Id);

        auditLogDto = auditLogs[3].ToDto();
        Assert.IsType<AuditLogDto>(auditLogDto);
        Assert.NotNull(auditLogDto.EditFor);
        Assert.NotNull(auditLogDto.EditBy);
        Assert.Equal(auditLogs[3].Id, auditLogDto.Id);
    }

    [Fact]
    public void AuditLog_InitialzeWithDtoTest()
    {
        var testEmployee = CreateTestEmployee();
        var auditLog = CreateTestAuditLog(
                                          new Employee(testEmployee, testEmployee.EmployeeType!),
                                          new Employee(testEmployee, testEmployee.EmployeeType!));

        var initializedAuditLog = new AuditLog(auditLog.ToDto());

        Assert.Equal(initializedAuditLog.EditBy, auditLog.EditBy);
        Assert.Equal(initializedAuditLog.EditFor, auditLog.EditFor);
    }
}
