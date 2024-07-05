using System.Linq.Expressions;
using HRIS.Services.Interfaces;
using HRIS.Services.Services;
using MockQueryable.Moq;
using Moq;
using RR.Tests.Data;
using RR.Tests.Data.Models.HRIS;
using RR.UnitOfWork;
using RR.UnitOfWork.Entities.HRIS;
using Xunit;

namespace HRIS.Services.Tests.Services;

public class EmployeeBankingServiceTest
{
    private readonly EmployeeBankingService _employeeBankingService;
    private readonly Mock<IEmployeeTypeService> _employeeTypeServiceMock;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IErrorLoggingService> _errorLoggingServiceMock;
    private readonly List<Employee> _employeeList;
    private readonly List<EmployeeBanking> _employeeBankingList;

    public EmployeeBankingServiceTest()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _errorLoggingServiceMock = new Mock<IErrorLoggingService>();
        _employeeBankingService = new EmployeeBankingService(_mockUnitOfWork.Object, _errorLoggingServiceMock.Object);
        _employeeTypeServiceMock = new Mock<IEmployeeTypeService>();
        _employeeList = EmployeeTestData.EmployeeOne.EntityToList();
        _employeeBankingList = EmployeeBankingTestData.EmployeeBankingOne.EntityToList();
    }

    [Fact]
    public async Task GetPendingReturnsPendingBankPass()
    {
        _mockUnitOfWork
            .Setup(u => u.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
            .Returns(EmployeeTestData.EmployeeOne.ToMockIQueryable());

        _mockUnitOfWork
            .Setup(u => u.EmployeeBanking.Get(It.IsAny<Expression<Func<EmployeeBanking, bool>>>()))
            .Returns(EmployeeBankingTestData.EmployeeBankingOne.ToMockIQueryable());

        _mockUnitOfWork
            .Setup(u => u.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
            .Returns(EmployeeTestData.EmployeeTwo.ToMockIQueryable());

        _mockUnitOfWork
            .Setup(u => u.EmployeeBanking.Get(It.IsAny<Expression<Func<EmployeeBanking, bool>>>()))
            .Returns(EmployeeBankingTestData.EmployeeBankingTwo.ToMockIQueryable());


        _mockUnitOfWork
            .Setup(u => u.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
            .Returns(EmployeeTestData.EmployeeThree.ToMockIQueryable());

        _mockUnitOfWork
            .Setup(u => u.EmployeeBanking.Get(It.IsAny<Expression<Func<EmployeeBanking, bool>>>()))
            .Returns(EmployeeBankingTestData.EmployeeBankingThree.ToMockIQueryable());

        var result = await _employeeBankingService.Get(0);
        Assert.Single(result);

        result = await _employeeBankingService.Get(1);
        Assert.Single(result);

        result = await _employeeBankingService.Get(2);
        Assert.Single(result);
    }

    [Fact]
    public async Task UpdateReturnsUpdateBank()
    {
        _employeeTypeServiceMock
            .Setup(r => r.GetEmployeeType(EmployeeTypeTestData.DeveloperType.Name!))
            .ReturnsAsync(EmployeeTypeTestData.DeveloperType.ToDto());

        _mockUnitOfWork
            .Setup(u => u.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
            .Returns(_employeeList.ToMockIQueryable());

        _mockUnitOfWork
            .Setup(u => u.EmployeeBanking.Get(It.IsAny<Expression<Func<EmployeeBanking, bool>>>()))
            .Returns(EmployeeBankingTestData.EmployeeBankingOne.ToMockIQueryable());

        var result =
            await _employeeBankingService.Update(EmployeeBankingTestData.EmployeeBankingOne.ToDto(), "test@retrorabbit.co.za");

        Assert.Equivalent(EmployeeBankingTestData.EmployeeBankingOne.ToDto(), result);
    }

    [Fact]
    public async Task UpdateByAdminReturnsUpdateBank()
    {
        _employeeTypeServiceMock
            .Setup(r => r.GetEmployeeType(EmployeeTypeTestData.DeveloperType.Name!))
            .ReturnsAsync(EmployeeTypeTestData.DeveloperType.ToDto());

        _mockUnitOfWork
            .Setup(u => u.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
            .Returns(_employeeList.ToMockIQueryable());

        _mockUnitOfWork
            .Setup(u => u.EmployeeBanking.Get(It.IsAny<Expression<Func<EmployeeBanking, bool>>>()))
            .Returns(_employeeBankingList.ToMockIQueryable());

        var empRoles = new EmployeeRole { Id = 1, Employee = EmployeeTestData.EmployeeOne, Role = EmployeeRoleTestData.RoleDtoAdmin }.EntityToList();
        var roles = EmployeeRoleTestData.RoleDtoAdmin.EntityToList();

        _mockUnitOfWork
            .Setup(x => x.EmployeeRole.Get(It.IsAny<Expression<Func<EmployeeRole, bool>>>()))
            .Returns(empRoles.ToMockIQueryable());

        _mockUnitOfWork
            .Setup(x => x.Role.Get(It.IsAny<Expression<Func<Role, bool>>>()))
            .Returns(roles.ToMockIQueryable());

        var result =
            await _employeeBankingService.Update(EmployeeBankingTestData.EmployeeBankingOne.ToDto(), "admin.email@example.com");

        Assert.Equivalent(EmployeeBankingTestData.EmployeeBankingOne.ToDto(), result);
    }

    [Fact]
    public async Task UpdateByPassReturnsUpdateBank()
    {
        _employeeTypeServiceMock
            .Setup(r => r.GetEmployeeType(EmployeeTypeTestData.DeveloperType.Name))
            .ReturnsAsync(EmployeeTypeTestData.DeveloperType.ToDto());

        _mockUnitOfWork
            .Setup(u => u.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
            .Returns(_employeeList.ToMockIQueryable());

        _mockUnitOfWork
            .Setup(u => u.EmployeeBanking.Get(It.IsAny<Expression<Func<EmployeeBanking, bool>>>()))
            .Returns(_employeeBankingList.ToMockIQueryable());


        var empRoles = new EmployeeRole { Id = 1, Employee = EmployeeTestData.EmployeeOne, Role = EmployeeRoleTestData.RoleDtoEmployee }.EntityToList();
        var roles = EmployeeRoleTestData.RoleDtoEmployee.EntityToList();

        _mockUnitOfWork
            .Setup(x => x.EmployeeRole.Get(It.IsAny<Expression<Func<EmployeeRole, bool>>>()))
            .Returns(empRoles.ToMockIQueryable());

        _mockUnitOfWork
            .Setup(x => x.Role.Get(It.IsAny<Expression<Func<Role, bool>>>()))
            .Returns(roles.ToMockIQueryable());

        _errorLoggingServiceMock.Setup(r => r.LogException(It.IsAny<Exception>())).Throws(new Exception("Unauthorized access"));

        var exception = await Assert.ThrowsAsync<Exception>(
                                                            async () =>
                                                                await _employeeBankingService
                                                                    .Update(EmployeeBankingTestData.EmployeeBankingOne.ToDto(),
                                                                            "unauthorized.email@example.com"));

        Assert.Equal("Unauthorized access", exception.Message);
    }

    [Fact]
    public async Task SavePass()
    {
        _employeeTypeServiceMock
            .Setup(r => r.GetEmployeeType(EmployeeTypeTestData.DeveloperType.Name))
            .ReturnsAsync(EmployeeTypeTestData.DeveloperType.ToDto());

        _mockUnitOfWork
            .Setup(u => u.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
            .Returns(_employeeList.ToMockIQueryable());

        _mockUnitOfWork
            .Setup(u => u.EmployeeBanking.Add(It.IsAny<EmployeeBanking>()))
            .ReturnsAsync(EmployeeBankingTestData.EmployeeBankingOne);

        var result =
            await _employeeBankingService.Save(EmployeeBankingTestData.EmployeeBankingOne.ToDto(), "test@retrorabbit.co.za");

        Assert.Equivalent(EmployeeBankingTestData.EmployeeBankingOne.ToDto(), result);
    }

    [Fact]
    public async Task SaveByAdminPass()
    {
        _employeeTypeServiceMock
            .Setup(r => r.GetEmployeeType(EmployeeTypeTestData.DeveloperType.Name))
            .ReturnsAsync(EmployeeTypeTestData.DeveloperType.ToDto());

        _mockUnitOfWork
            .Setup(u => u.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
            .Returns(_employeeList.ToMockIQueryable());

        _mockUnitOfWork
            .Setup(u => u.EmployeeBanking.Add(It.IsAny<EmployeeBanking>()))
            .ReturnsAsync(EmployeeBankingTestData.EmployeeBankingOne);

        var empRoles = new EmployeeRole { Id = 1, Employee = EmployeeTestData.EmployeeOne, Role = EmployeeRoleTestData.RoleDtoAdmin }.EntityToList();

        var roles = EmployeeRoleTestData.RoleDtoAdmin.EntityToList();

        _mockUnitOfWork
            .Setup(x => x.EmployeeRole.Get(It.IsAny<Expression<Func<EmployeeRole, bool>>>()))
            .Returns(empRoles.ToMockIQueryable());

        _mockUnitOfWork
            .Setup(x => x.Role.Get(It.IsAny<Expression<Func<Role, bool>>>()))
            .Returns(roles.ToMockIQueryable());

        var result =
            await _employeeBankingService.Save(EmployeeBankingTestData.EmployeeBankingOne.ToDto(), "admin.email@example.com");

        Assert.NotNull(result);
        Assert.Equivalent(EmployeeBankingTestData.EmployeeBankingOne.ToDto(), result);
    }

    [Fact]
    public async Task SaveUnauthorizedPass()
    {
        _employeeTypeServiceMock
            .Setup(r => r.GetEmployeeType(EmployeeTypeTestData.DeveloperType.Name))
            .ReturnsAsync(EmployeeTypeTestData.DeveloperType.ToDto());

        _mockUnitOfWork
            .Setup(u => u.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
            .Returns(_employeeList.ToMockIQueryable());

        _mockUnitOfWork
            .Setup(u => u.EmployeeBanking.Add(It.IsAny<EmployeeBanking>()))
            .ReturnsAsync(EmployeeBankingTestData.EmployeeBankingOne);

        var empRoles = new EmployeeRole { Id = 1, Employee = EmployeeTestData.EmployeeOne, Role = EmployeeRoleTestData.RoleDtoEmployee }.EntityToList();

        var roles = EmployeeRoleTestData.RoleDtoEmployee.EntityToList();

        _mockUnitOfWork
            .Setup(x => x.EmployeeRole.Get(It.IsAny<Expression<Func<EmployeeRole, bool>>>()))
            .Returns(empRoles.ToMockIQueryable());

        _mockUnitOfWork
            .Setup(x => x.Role.Get(It.IsAny<Expression<Func<Role, bool>>>()))
            .Returns(roles.ToMockIQueryable());

        _errorLoggingServiceMock.Setup(r => r.LogException(It.IsAny<Exception>())).Throws(new Exception("Unauthorized access"));

        var exception = await Assert.ThrowsAsync<Exception>(
                                                            async () =>
                                                                await _employeeBankingService
                                                                    .Save(EmployeeBankingTestData.EmployeeBankingOne.ToDto(),
                                                                          "unauthorized.email@example.com"));

        Assert.Equal("Unauthorized access", exception.Message);
    }

    [Fact]
    public async Task GetBankingPass()
    {
        _mockUnitOfWork
            .Setup(u => u.EmployeeBanking.Get(It.IsAny<Expression<Func<EmployeeBanking, bool>>>()))
            .Returns(new List<EmployeeBanking> { EmployeeBankingTestData.EmployeeBankingOne }.AsQueryable()
            .BuildMock());

        var result = await _employeeBankingService.GetBanking(1);

        Assert.NotNull(result);
        Assert.Equivalent(EmployeeBankingTestData.EmployeeBankingOne.ToDto(), result[0]);
    }

    [Fact]
    public async Task GetBankingFail()
    {
        _mockUnitOfWork
            .Setup(u => u.EmployeeBanking.FirstOrDefault(It.IsAny<Expression<Func<EmployeeBanking, bool>>>()))
            .Throws<Exception>();
        _errorLoggingServiceMock.Setup(r => r.LogException(It.IsAny<Exception>())).Throws(new Exception());

        await Assert.ThrowsAsync<Exception>(async () => await _employeeBankingService.GetBanking(2));
    }

    [Fact]
    public async Task SaveFail()
    {
        _employeeTypeServiceMock
            .Setup(r => r.GetEmployeeType(EmployeeTypeTestData.DeveloperType.Name))
            .ReturnsAsync(EmployeeTypeTestData.DeveloperType.ToDto());

        _mockUnitOfWork
            .Setup(u => u.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
            .Returns(_employeeList.ToMockIQueryable());

        _mockUnitOfWork
            .Setup(u => u.EmployeeBanking.Add(It.IsAny<EmployeeBanking>()))
            .ReturnsAsync(EmployeeBankingTestData.EmployeeBankingOne);

        var result =
            await _employeeBankingService.Save(EmployeeBankingTestData.EmployeeBankingOne.ToDto(), "test@retrorabbit.co.za");

        Assert.Equivalent(EmployeeBankingTestData.EmployeeBankingOne.ToDto(), result);
    }
}