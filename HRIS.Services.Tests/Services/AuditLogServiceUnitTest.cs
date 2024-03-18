using System.Linq.Expressions;
using HRIS.Models;
using HRIS.Models.Enums;
using HRIS.Services.Services;
using Moq;
using RR.UnitOfWork;
using RR.UnitOfWork.Entities.HRIS;
using Xunit;

namespace HRIS.Services.Tests.Services;

public class AuditLogServiceUnitTest
{
    private readonly AuditLogDto _auditLogDto;
    private readonly AuditLogService _auditLogService;
    private readonly EmployeeDto _employee;
    private readonly Mock<IUnitOfWork> _unitOfWork;
    private readonly ErrorLoggingService _errorLoggingService;

    public AuditLogServiceUnitTest()
    {
        _unitOfWork = new Mock<IUnitOfWork>();
        _auditLogService = new AuditLogService(_unitOfWork.Object,_errorLoggingService);
        _errorLoggingService = new ErrorLoggingService(_unitOfWork.Object);

        var employeeAddressDto =
            new EmployeeAddressDto{ Id = 1, UnitNumber = "2", ComplexName = "Complex", StreetNumber = "2", SuburbOrDistrict = "Suburb/District", City = "City", Country = "Country", Province = "Province", PostalCode = "1620" };
        EmployeeTypeDto employeeTypeDto = new EmployeeTypeDto { Id = 1, Name = "Developer" };
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

        _auditLogDto = new AuditLogDto
        {
            Id = 1,
            EditBy = _employee,
            EditFor = _employee,
            EditDate = new DateTime(),
            Description = "Test Description"
        };
    }

    [Fact]
    public async Task GetAllAuditLogTest()
    {
        var auditlogs = new List<AuditLogDto> { _auditLogDto };

        _unitOfWork.Setup(a => a.AuditLog.GetAll(null)).Returns(Task.FromResult(auditlogs));
        var result = await _auditLogService.GetAllAuditLogs();

        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal(auditlogs, result);
    }

    [Fact]
    public async Task GetAuditLogByEditedByIdTest()
    {
        var Id = 1;

        var auditloglistdto = new List<AuditLogDto> { _auditLogDto };

        _unitOfWork.Setup(x => x.AuditLog.GetAll(It.IsAny<Expression<Func<AuditLog, bool>>>()))
                   .Returns(Task.FromResult(auditloglistdto));

        var result = await _auditLogService.GetAuditLogByEditedById(Id);

        Assert.NotNull(result);
        Assert.Equal(auditloglistdto, result);
    }

    [Fact]
    public async Task GetAuditLogByEditedForIdTest()
    {
        var Id = 1;
        var auditloglistdto = new List<AuditLogDto> { _auditLogDto };

        _unitOfWork.Setup(x => x.AuditLog.GetAll(It.IsAny<Expression<Func<AuditLog, bool>>>()))
                   .Returns(Task.FromResult(auditloglistdto));

        var result = await _auditLogService.GetAuditLogByEditedForId(Id);

        Assert.NotNull(result);
        Assert.Equal(auditloglistdto, result);
    }

    [Fact]
    public async Task SaveAuditLogsTest()
    {
        var auditlog = new List<AuditLogDto> { _auditLogDto };

        _unitOfWork.Setup(x => x.AuditLog.GetAll(It.IsAny<Expression<Func<AuditLog, bool>>>()))
                   .Returns(Task.FromResult(auditlog));
        _unitOfWork.Setup(x => x.AuditLog.Add(It.IsAny<AuditLog>())).Returns(Task.FromResult(_auditLogDto));

        var result = await _auditLogService.SaveAuditLog(_auditLogDto);

        Assert.NotNull(result);
        Assert.Equal(_auditLogDto, result);

        _unitOfWork.Verify(x => x.AuditLog.Add(It.IsAny<AuditLog>()));
    }

    [Fact]
    public async Task UpdateAuditLogTest()
    {
        _unitOfWork.SetupSequence(a => a.AuditLog.GetById(It.IsAny<int>())).ReturnsAsync(_auditLogDto);
        _unitOfWork.Setup(a => a.AuditLog.Any(It.IsAny<Expression<Func<AuditLog, bool>>>())).ReturnsAsync(true);
        _unitOfWork.Setup(a => a.AuditLog.Update(It.IsAny<AuditLog>())).Returns(Task.FromResult(_auditLogDto));

        var result = await _auditLogService.UpdateAuditLog(_auditLogDto);

        Assert.NotNull(result);
        Assert.Equal(_auditLogDto, result);

        _unitOfWork.Verify(x => x.AuditLog.Update(It.IsAny<AuditLog>()));
    }

    [Fact]
    public async Task DeleteAuditLogTest()
    {
        _unitOfWork.Setup(a => a.AuditLog.GetById(It.IsAny<int>())).ReturnsAsync(_auditLogDto);
        _unitOfWork.Setup(x => x.AuditLog.Delete(It.IsAny<int>()))
                   .Returns(Task.FromResult(_auditLogDto));

        var result = await _auditLogService.DeleteAuditLog(_auditLogDto);

        Assert.NotNull(result);
        Assert.Equal(_auditLogDto, result);

        _unitOfWork.Verify(x => x.AuditLog.Delete(It.IsAny<int>()));
    }
}
