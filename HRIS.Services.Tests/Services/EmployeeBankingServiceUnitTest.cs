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
    private readonly Mock<AuthorizeIdentityMock> _identity;
    private readonly List<Employee> _employeeList;
    private readonly List<EmployeeBanking> _employeeBankingList;

    public EmployeeBankingServiceTest()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _identity = new Mock<AuthorizeIdentityMock>();
        _employeeBankingService = new EmployeeBankingService(_identity.Object ,_mockUnitOfWork.Object);
        _employeeTypeServiceMock = new Mock<IEmployeeTypeService>();
        _employeeList = EmployeeTestData.EmployeeOne.EntityToList();
        _employeeBankingList = EmployeeBankingTestData.EmployeeBankingOne.EntityToList();
    }

    [Fact(Skip = "Needs Work")]
    public async Task GetPendingReturnsPendingBankPass()
    {
        _identity.Setup(i => i.Role).Returns("Admin");
        _identity.SetupGet(i => i.EmployeeId).Returns(1);

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

    [Fact(Skip = "Needs Work")]
    public async Task UpdateReturnsUpdateBank()
    {
        _identity.Setup(i => i.Role).Returns("Admin");
        _identity.SetupGet(i => i.EmployeeId).Returns(1);

        _employeeTypeServiceMock
            .Setup(r => r.GetEmployeeTypeByName(EmployeeTypeTestData.DeveloperType.Name!))
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

    [Fact(Skip = "Needs Work")]
    public async Task UpdateByAdminReturnsUpdateBank()
    {
        _identity.Setup(i => i.Role).Returns("SuperAdmin");
        _identity.SetupGet(i => i.EmployeeId).Returns(1);

        _employeeTypeServiceMock
            .Setup(r => r.GetEmployeeTypeByName(EmployeeTypeTestData.DeveloperType.Name!))
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
            await _employeeBankingService.Update(EmployeeBankingTestData.EmployeeBankingOne.ToDto(), "Unauthorized access");

        Assert.Equivalent(EmployeeBankingTestData.EmployeeBankingOne.ToDto(), result);
    }

    [Fact(Skip = "Needs Work")]
    public async Task UpdateByPassReturnsUpdateBank()
    {
        _identity.Setup(i => i.Role).Returns("Admin");
        _identity.SetupGet(i => i.EmployeeId).Returns(1);

        _employeeTypeServiceMock
            .Setup(r => r.GetEmployeeTypeByName(EmployeeTypeTestData.DeveloperType.Name))
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

        await Assert.ThrowsAsync<CustomException>(
                                                            async () =>
                                                                await _employeeBankingService
                                                                    .Update(EmployeeBankingTestData.EmployeeBankingOne.ToDto(),
                                                                            "unauthorized.email@example.com"));
    }

    [Fact(Skip = "Needs Work")]
    public async Task SavePass()
    {
        _identity.Setup(i => i.Role).Returns("Admin");
        _identity.SetupGet(i => i.EmployeeId).Returns(1);

        _employeeTypeServiceMock
            .Setup(r => r.GetEmployeeTypeByName(EmployeeTypeTestData.DeveloperType.Name))
            .ReturnsAsync(EmployeeTypeTestData.DeveloperType.ToDto());

        _mockUnitOfWork
            .Setup(u => u.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
            .Returns(_employeeList.ToMockIQueryable());

        _mockUnitOfWork
            .Setup(u => u.EmployeeBanking.Add(It.IsAny<EmployeeBanking>()))
            .ReturnsAsync(EmployeeBankingTestData.EmployeeBankingOne);

        var result =
            await _employeeBankingService.Create(EmployeeBankingTestData.EmployeeBankingOne.ToDto(), "test@retrorabbit.co.za");

        Assert.Equivalent(EmployeeBankingTestData.EmployeeBankingOne.ToDto(), result);
    }

    [Fact(Skip = "Needs Work")]
    public async Task SaveByAdminPass()
    {
        _identity.Setup(i => i.Role).Returns("Admin");
        _identity.SetupGet(i => i.EmployeeId).Returns(1);

        _employeeTypeServiceMock
            .Setup(r => r.GetEmployeeTypeByName(EmployeeTypeTestData.DeveloperType.Name))
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
            await _employeeBankingService.Create(EmployeeBankingTestData.EmployeeBankingOne.ToDto(), "admin.email@example.com");

        Assert.NotNull(result);
        Assert.Equivalent(EmployeeBankingTestData.EmployeeBankingOne.ToDto(), result);
    }

    [Fact(Skip = "Needs Work")]
    public async Task SaveUnauthorizedPass()
    {
        _identity.Setup(i => i.Role).Returns("Employee");
        _identity.SetupGet(i => i.EmployeeId).Returns(1);

        _employeeTypeServiceMock
            .Setup(r => r.GetEmployeeTypeByName(EmployeeTypeTestData.DeveloperType.Name))
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

        await Assert.ThrowsAsync<CustomException>(
                                                            async () =>
                                                                await _employeeBankingService
                                                                    .Create(EmployeeBankingTestData.EmployeeBankingOne.ToDto(),
                                                                          "unauthorized.email@example.com"));
    }

    [Fact(Skip = "Needs Work")]
    public async Task GetBankingPass()
    {
        _identity.Setup(i => i.Role).Returns("Admin");
        _identity.SetupGet(i => i.EmployeeId).Returns(1);

        _mockUnitOfWork
            .Setup(u => u.EmployeeBanking.Get(It.IsAny<Expression<Func<EmployeeBanking, bool>>>()))
            .Returns(new List<EmployeeBanking> { EmployeeBankingTestData.EmployeeBankingOne }.AsQueryable()
            .BuildMock());

        var result = await _employeeBankingService.GetBankingById(1);

        Assert.NotNull(result);
        Assert.Equivalent(EmployeeBankingTestData.EmployeeBankingOne.ToDto(), result[0]);
    }

    [Fact(Skip = "Needs Work")]
    public async Task GetBankingFail()
    {
        _identity.Setup(i => i.Role).Returns("Journey");

        _mockUnitOfWork
            .Setup(u => u.EmployeeBanking.FirstOrDefault(It.IsAny<Expression<Func<EmployeeBanking, bool>>>()))
            .Throws<CustomException>();

        await Assert.ThrowsAsync<CustomException>(async () => await _employeeBankingService.GetBankingById(2));
    }

    [Fact(Skip = "Needs Work")]
    public async Task SaveFail()
    {
        _identity.Setup(i => i.Role).Returns("Admin");
        _identity.SetupGet(i => i.EmployeeId).Returns(1);

        _employeeTypeServiceMock
            .Setup(r => r.GetEmployeeTypeByName(EmployeeTypeTestData.DeveloperType.Name))
            .ReturnsAsync(EmployeeTypeTestData.DeveloperType.ToDto());

        _mockUnitOfWork
            .Setup(u => u.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
            .Returns(_employeeList.ToMockIQueryable());

        _mockUnitOfWork
            .Setup(u => u.EmployeeBanking.Add(It.IsAny<EmployeeBanking>()))
            .ReturnsAsync(EmployeeBankingTestData.EmployeeBankingOne);

        var result =
            await _employeeBankingService.Create(EmployeeBankingTestData.EmployeeBankingOne.ToDto(), "test@retrorabbit.co.za");

        Assert.Equivalent(EmployeeBankingTestData.EmployeeBankingOne.ToDto(), result);
    }
}