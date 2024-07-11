using HRIS.Services.Interfaces;
using HRIS.Services.Services;
using Moq;
using RR.Tests.Data;
using RR.Tests.Data.Models.HRIS;
using RR.UnitOfWork;
using RR.UnitOfWork.Entities.HRIS;
using System.Linq.Expressions;
using RR.Tests.Data.Models;
using Xunit;

namespace HRIS.Services.Tests.Services;

public class TerminationServiceUnitTests
{
    private readonly TerminationService _terminationService;
    private readonly Termination _terminationDto;
    private readonly Employee _employeeDto;
    private readonly EmployeeType _employeeTypeDto;
    private readonly Mock<IUnitOfWork> _db;
    private readonly Mock<IEmployeeTypeService> _employeeTypeServiceMock;
    private readonly Mock<IEmployeeService> _employeeServiceMock;
    private readonly Mock<IAuthService> _authServiceMock;

    public TerminationServiceUnitTests()
    {
        _db = new Mock<IUnitOfWork>();
        _employeeTypeServiceMock = new Mock<IEmployeeTypeService>();
        _employeeServiceMock = new Mock<IEmployeeService>();
        _authServiceMock = new Mock<IAuthService>();
        _terminationService = new TerminationService(_db.Object, _authServiceMock.Object, _employeeTypeServiceMock.Object, _employeeServiceMock.Object);

        _terminationDto = new Termination
        {
            Id = 1,
            EmployeeId = 1,
            TerminationOption = 0,
            DayOfNotice = new DateTime(),
            LastDayOfEmployment = new DateTime(),
            ReemploymentStatus = false,
            EquipmentStatus = true,
            AccountsStatus = true,
            TerminationDocument = "document",
            DocumentName = "document name",
            TerminationComments = "termination comment",
        };

        _employeeDto = EmployeeTestData.EmployeeOne;
        _employeeTypeDto = EmployeeTypeTestData.DeveloperType;
    }

    [Fact]
    public async Task CheckIfExistsReturnsFalse()
    {
        _db.Setup(x => x.Termination.Any(It.IsAny<Expression<Func<Termination, bool>>>())).ReturnsAsync(false);

        var exists = await _terminationService.CheckTerminationExist(2);

        Assert.False(exists);
    }

    [Fact]
    public async Task CheckIfExistsPassTest()
    {
        _db.Setup(x => x.Termination.Any(It.IsAny<Expression<Func<Termination, bool>>>())).ReturnsAsync(true);

        var exists = await _terminationService.CheckTerminationExist(1);

        Assert.True(exists);
    }

    [Fact]
    public async Task SavePassTest()
    {
        _db.Setup(x => x.Termination.Any(It.IsAny<Expression<Func<Termination, bool>>>()))
            .ReturnsAsync(false);

        _db.Setup(e => e.Employee.Update(It.IsAny<Employee>()))
            .ReturnsAsync(EmployeeTestData.EmployeeOne);

        _db.Setup(e => e.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
            .Returns(EmployeeTestData.EmployeeOne.ToMockIQueryable());

        _db.Setup(e => e.Employee.Any(It.IsAny<Expression<Func<Employee, bool>>>()))
            .ReturnsAsync(true);

        _db.Setup(er => er.EmployeeRole.Get(It.IsAny<Expression<Func<EmployeeRole, bool>>>()))
            .Returns(new EmployeeRole { Id = 1, Employee = EmployeeTestData.EmployeeOne, Role = RoleTestData.EmployeeRole }.ToMockIQueryable());

        _db.Setup(r => r.Role.Get(It.IsAny<Expression<Func<Role, bool>>>()))
            .Returns(RoleTestData.EmployeeRole.ToMockIQueryable());

        _db.Setup(r => r.Termination.Add(It.IsAny<Termination>()))
            .ReturnsAsync(new Termination());

        _employeeServiceMock.Setup(e => e.GetEmployeeById(It.IsAny<int>()))
            .ReturnsAsync(_employeeDto.ToDto);

        _employeeTypeServiceMock.Setup(et => et.GetEmployeeType(EmployeeTypeTestData.DeveloperType!.Name))
            .ReturnsAsync(EmployeeTypeTestData.DeveloperType.ToDto());

        _employeeTypeServiceMock.Setup(et => et.GetEmployeeType(_employeeDto.EmployeeType!.Name))
            .ReturnsAsync(_employeeTypeDto.ToDto);

        _authServiceMock.Setup(a => a.DeleteUser(It.IsAny<string>())).ReturnsAsync(true);

        await _terminationService.SaveTermination(_terminationDto.ToDto());

        _db.Verify(x => x.Termination.Add(It.IsAny<Termination>()), Times.Once);
    }

    [Fact]
    public async Task UpdatePassTest()
    {
        _db.Setup(x => x.Termination.Any(It.IsAny<Expression<Func<Termination, bool>>>())).ReturnsAsync(true);

        _db.Setup(r => r.Termination.Update(It.IsAny<Termination>()))
            .ReturnsAsync(new Termination());

        await _terminationService.SaveTermination(_terminationDto.ToDto());

        _db.Verify(x => x.Termination.Update(It.IsAny<Termination>()), Times.Once);
    }

    [Fact]
    public async Task GetByIdFailTest()
    {
        _db.Setup(x => x.Termination.Any(It.IsAny<Expression<Func<Termination, bool>>>())).ReturnsAsync(false);

        await Assert.ThrowsAsync<CustomException>(() => _terminationService.GetTerminationByEmployeeId(_terminationDto.EmployeeId));
    }

    [Fact]
    public async Task GetByIdPassTest()
    {
        _db.Setup(x => x.Termination.Any(It.IsAny<Expression<Func<Termination, bool>>>())).ReturnsAsync(true);

        _db.Setup(x => x.Termination.FirstOrDefault(It.IsAny<Expression<Func<Termination, bool>>>())).ReturnsAsync(_terminationDto);

        await _terminationService.GetTerminationByEmployeeId(1);

        _db.Verify(x => x.Termination.FirstOrDefault(It.IsAny<Expression<Func<Termination, bool>>>()), Times.Once);
    }
}
