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
            new EmployeeAddressDto(1, "2", "Complex", "2", "Suburb/District", "City", "Country", "Province", "1620");

        var testEmployee = new EmployeeDto(1, "001", "34434434", new DateTime(), new DateTime(),
                                           null, false, "None", 4, employeeTypeDto, "Notes", 1, 28, 128, 100000,
                                           "Dorothy", "D",
                                           "Mahoko", new DateTime(), "South Africa", "South African", "0000080000000",
                                           " ",
                                           new DateTime(), null, Race.Black, Gender.Male, null,
                                           "texample@retrorabbit.co.za", "test.example@gmail.com", "0000000000", null,
                                           null, employeeAddressDto, employeeAddressDto, null, null, null);

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
            CreateTestAuditLog(new Employee(testEmployee, testEmployee.EmployeeType)),
            CreateTestAuditLog(editFor: new Employee(testEmployee, testEmployee.EmployeeType)),
            CreateTestAuditLog(
                               new Employee(testEmployee, testEmployee.EmployeeType),
                               new Employee(testEmployee, testEmployee.EmployeeType))
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
                                          new Employee(testEmployee, testEmployee.EmployeeType),
                                          new Employee(testEmployee, testEmployee.EmployeeType));

        var initializedAuditLog = new AuditLog(auditLog.ToDto());

        Assert.Equal(initializedAuditLog.EditBy, auditLog.EditBy);
        Assert.Equal(initializedAuditLog.EditFor, auditLog.EditFor);
    }
}
