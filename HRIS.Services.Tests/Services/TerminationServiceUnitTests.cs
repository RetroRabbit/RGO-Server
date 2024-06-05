using HRIS.Models;
using HRIS.Models.Enums;
using HRIS.Services.Interfaces;
using HRIS.Services.Services;
using MockQueryable.Moq;
using Moq;
using RR.Tests.Data.Models.HRIS;
using RR.UnitOfWork;
using RR.UnitOfWork.Entities.HRIS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace HRIS.Services.Tests.Services;

public class TerminationServiceUnitTests
{
    private readonly TerminationService _terminationService;
    private readonly TerminationDto _terminationDto;
    private readonly EmployeeDto _employeeDto;
    private readonly EmployeeTypeDto _employeeTypeDto;
    private readonly Mock<IUnitOfWork> _db;
    private readonly Mock<ITerminationService> _terminationServiceMock;
    private readonly Mock<IErrorLoggingService> _errorLogggingServiceMock;
    private readonly Mock<IEmployeeTypeService> _employeeTypeServiceMock;
    private readonly Mock<IEmployeeService> _employeeServiceMock;

    public TerminationServiceUnitTests()
    {
        _db = new Mock<IUnitOfWork>();
        _terminationServiceMock = new Mock<ITerminationService>();
        _errorLogggingServiceMock = new Mock<IErrorLoggingService>();
        _employeeTypeServiceMock = new Mock<IEmployeeTypeService>();
        _employeeServiceMock = new Mock<IEmployeeService>();
        _terminationService = new TerminationService(_db.Object, _errorLogggingServiceMock.Object, _employeeTypeServiceMock.Object, _employeeServiceMock.Object);

        _terminationDto = new TerminationDto
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

        _employeeDto = EmployeeTestData.EmployeeDto;

        _employeeTypeDto = EmployeeTypeTestData.DeveloperType;
    }

    [Fact]
    public async Task CheckIfExistsFailTest()
    {
        _db.Setup(x => x.Termination.Any(It.IsAny<Expression<Func<Termination, bool>>>()))
               .ReturnsAsync(false);

        var nonExistantEmployeeId = 2;
        var exists = await _terminationService.CheckTerminationExist(nonExistantEmployeeId);

        Assert.False(exists);
    }

    [Fact]
    public async Task CheckIfExistsPassTest()
    {
        _db.Setup(x => x.Termination.Any(It.IsAny<Expression<Func<Termination, bool>>>()))
               .ReturnsAsync(true);
        var existingEmployeeId = 1;
        var exists = await _terminationService.CheckTerminationExist(existingEmployeeId);

        Assert.True(exists);
    }

    [Fact]
    public async Task SavePassTest()
    {
        _db.Setup(x => x.Termination.Any(It.IsAny<Expression<Func<Termination, bool>>>()))
            .ReturnsAsync(false);

        var existingEmployeeId = 1;

        Employee emp = new Employee(EmployeeTestData.EmployeeDto, EmployeeTypeTestData.DeveloperType);
        emp.EmployeeType = new EmployeeType(EmployeeTypeTestData.DeveloperType);

        RoleDto roleDto = new RoleDto { Id = 1, Description = "Developer" };
        EmployeeRoleDto empRoleDto = new EmployeeRoleDto { Id = 1, Employee = EmployeeTestData.EmployeeDto, Role = roleDto };
        EmployeeRole empRole = new EmployeeRole(empRoleDto);

        List<Employee> employees = new List<Employee> { emp };
        List<EmployeeRole> empRoles = new List<EmployeeRole> { empRole };
        List<Role> roles = new List<Role> { new Role(roleDto) };

        _employeeTypeServiceMock.Setup(r => r.GetEmployeeType(EmployeeTypeTestData.DeveloperType.Name)).Returns(Task.FromResult(EmployeeTypeTestData.DeveloperType));

        _db.Setup(r => r.Employee.Update(It.IsAny<Employee>())).Returns(Task.FromResult(EmployeeTestData.EmployeeDto));
        _db.Setup(r => r.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>())).Returns(employees.AsQueryable().BuildMock());
        _db.Setup(r => r.Employee.Any(It.IsAny<Expression<Func<Employee, bool>>>())).ReturnsAsync(true);
        _db.Setup(r => r.EmployeeRole.Get(It.IsAny<Expression<Func<EmployeeRole, bool>>>())).Returns(empRoles.AsQueryable().BuildMock());
        _db.Setup(r => r.Role.Get(It.IsAny<Expression<Func<Role, bool>>>())).Returns(roles.AsQueryable().BuildMock());

        _employeeServiceMock.Setup(e => e.GetEmployeeById(existingEmployeeId)).ReturnsAsync(_employeeDto);
        _employeeTypeServiceMock.Setup(t => t.GetEmployeeType(_employeeDto.EmployeeType.Name)).ReturnsAsync(_employeeTypeDto);

        _db.Setup(x => x.Termination.Update(new Termination(_terminationDto)))
           .Returns(Task.FromResult(_terminationDto));

        await _terminationService.SaveTermination(_terminationDto);

        _db.Verify(x => x.Termination.Add(It.IsAny<Termination>()), Times.Once);
    }

    [Fact]
    public async Task UpdatePassTest()
    {
        _db.Setup(x => x.Termination.Any(It.IsAny<Expression<Func<Termination, bool>>>()))
            .ReturnsAsync(true);

        await _terminationService.SaveTermination(_terminationDto);

        _db.Verify(x => x.Termination.Update(It.IsAny<Termination>()), Times.Once);
    }

    [Fact]
    public async Task GetByIdFailTest()
    {
        _db.Setup(x => x.Termination.Any(It.IsAny<Expression<Func<Termination, bool>>>()))
                .ReturnsAsync(false);

        _errorLogggingServiceMock.Setup(r => r.LogException(It.IsAny<Exception>())).Throws(new Exception("termination not found"));

        await Assert.ThrowsAsync<Exception>(() => _terminationService.GetTerminationByEmployeeId(_terminationDto.EmployeeId));
    }

    [Fact]
    public async Task GetByIdPassTest()
    {
        _db.Setup(x => x.Termination.Any(It.IsAny<Expression<Func<Termination, bool>>>()))
                .ReturnsAsync(true);

        await _terminationService.GetTerminationByEmployeeId(1);

        _db.Verify(x => x.Termination.FirstOrDefault(It.IsAny<Expression<Func<Termination, bool>>>()), Times.Once);
    }
}
