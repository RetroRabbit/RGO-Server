using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
using Org.BouncyCastle.Security;
using RGO.Models;
using RGO.Services.Interfaces;
using RGO.Services.Services;
using RGO.Tests.Data.Models;
using RGO.UnitOfWork;
using RGO.UnitOfWork.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace RGO.Services.Tests.Services;

public class AuditLogServiceUnitTest
{
    private readonly Mock<IUnitOfWork> _unitOfWork;
    private readonly AuditLogService _auditLogService;
    private readonly AuditLogDto _auditLogDto;
    private readonly EmployeeDto _employee;
    public AuditLogServiceUnitTest()
    {
        _unitOfWork = new Mock<IUnitOfWork>();
        _auditLogService = new AuditLogService(_unitOfWork.Object);

        EmployeeAddressDto employeeAddressDto = new EmployeeAddressDto(1, "2", "Complex", "2", "Suburb/District", "City", "Country", "Province", "1620");
        EmployeeTypeDto employeeTypeDto = new(1, "Developer");
        _employee= new EmployeeDto(1, "001", "34434434", new DateTime(), new DateTime(),
           null, false, "None", 4, employeeTypeDto, "Notes", 1, 28, 128, 100000, "Kamo", "K.G.",
           "Smith", new DateTime(), "South Africa", "South African", "1234457899", " ",
           new DateTime(), null, Models.Enums.Race.Black, Models.Enums.Gender.Female, null!,
           "ksmith@retrorabbit.co.za", "kmaosmith@gmail.com", "0123456789", null, null, employeeAddressDto, employeeAddressDto, null, null, null);
       
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
        List<AuditLogDto> auditlogs = new List<AuditLogDto>() { _auditLogDto };

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
       
        _unitOfWork.Setup(x => x.AuditLog.GetAll(It.IsAny<Expression<Func<AuditLog, bool>>>())).Returns(Task.FromResult(auditloglistdto));

        var result = await _auditLogService.GetAuditLogByEditedById(Id);

        Assert.NotNull(result);
        Assert.Equal(auditloglistdto, result);
    }

    [Fact]
    public async Task GetAuditLogByEditedForIdTest()
    {
        var Id = 1;
        var auditloglistdto = new List<AuditLogDto> { _auditLogDto };

        _unitOfWork.Setup(x => x.AuditLog.GetAll(It.IsAny<Expression<Func<AuditLog, bool>>>())).Returns(Task.FromResult(auditloglistdto));

        var result = await _auditLogService.GetAuditLogByEditedForId(Id);

        Assert.NotNull(result);
        Assert.Equal(auditloglistdto, result);
    }

    [Fact]
    public async Task SaveAuditLogsTest()
    {
        List<AuditLogDto> auditlog = new List<AuditLogDto>() { _auditLogDto };

        _unitOfWork.Setup(x => x.AuditLog.GetAll(It.IsAny<Expression<Func<AuditLog, bool>>>())).Returns(Task.FromResult(auditlog));
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
