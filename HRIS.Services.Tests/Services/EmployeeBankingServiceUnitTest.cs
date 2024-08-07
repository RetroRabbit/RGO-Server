using System.Linq.Expressions;
using Auth0.ManagementApi.Models;
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
    private readonly Employee _testEmployee = EmployeeTestData.EmployeeOne;
    private readonly List<EmployeeBanking> _employeeBankingList;

    public EmployeeBankingServiceTest()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _identity = new Mock<AuthorizeIdentityMock>();
        _employeeBankingService = new EmployeeBankingService(_mockUnitOfWork.Object, _identity.Object);
        _employeeTypeServiceMock = new Mock<IEmployeeTypeService>();
        _employeeList = EmployeeTestData.EmployeeOne.EntityToList();
        _employeeBankingList = EmployeeBankingTestData.EmployeeBankingOne.EntityToList();
    }

    [Fact]
    public async Task GetPending_Pass()
    {
        _identity.Setup(i => i.Role).Returns("SuperAdmin");

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
    public async Task GetPending_UnauthorizedAccess()
    {
        _identity.Setup(i => i.Role).Returns("Employee");

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

        var exception = await Assert.ThrowsAsync<CustomException>(() =>
               _employeeBankingService.Get(EmployeeBankingTestData.EmployeeBankingOne.EmployeeId));

        Assert.Equivalent("Unauthorized Access", exception.Message);
    }

    [Fact]
    public async Task Update_Pass()
    {
        _identity.Setup(i => i.Role).Returns("Admin");
        _identity.SetupGet(i => i.EmployeeId).Returns(1);

        var employeeList = new List<Employee>
        {
            EmployeeTestData.EmployeeOne
        };

        _mockUnitOfWork.Setup(e => e.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
             .Returns(employeeList.ToMockIQueryable());

        _mockUnitOfWork.Setup(e => e.Employee.Any(It.IsAny<Expression<Func<Employee, bool>>>()))
           .ReturnsAsync(true);

        _mockUnitOfWork
       .Setup(u => u.EmployeeBanking.Get(It.IsAny<Expression<Func<EmployeeBanking, bool>>>()))
       .Returns(EmployeeBankingTestData.EmployeeBankingOne.ToMockIQueryable());

        var result =
            await _employeeBankingService.Update(EmployeeBankingTestData.EmployeeBankingOne.ToDto());

        Assert.Equivalent(EmployeeBankingTestData.EmployeeBankingOne.ToDto(), result);
    }

    [Fact]
    public async Task Update_UnauthorizedAccess()
    {
        _identity.Setup(i => i.Role).Returns("Journey");
        _identity.Setup(x => x.EmployeeId).Returns(2);

        _mockUnitOfWork
            .Setup(u => u.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
            .Returns(_testEmployee.ToMockIQueryable());

        _mockUnitOfWork
            .Setup(u => u.EmployeeBanking.Get(It.IsAny<Expression<Func<EmployeeBanking, bool>>>()))
            .Returns(EmployeeBankingTestData.EmployeeBankingOne.ToMockIQueryable());

        _mockUnitOfWork.Setup(e => e.Employee.Any(It.IsAny<Expression<Func<Employee, bool>>>()))
        .ReturnsAsync(true);

        var exception = await Assert.ThrowsAsync<CustomException>(() =>
               _employeeBankingService.Update(EmployeeBankingTestData.EmployeeBankingOne.ToDto()));

        Assert.Equivalent("Unauthorized Access", exception.Message);
    }

    [Fact]
    public async Task GetUpdate_DoesNotExist()
    {
        _mockUnitOfWork
            .Setup(u => u.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
            .Returns(_employeeList.ToMockIQueryable());

        _mockUnitOfWork
            .Setup(u => u.EmployeeBanking.Get(It.IsAny<Expression<Func<EmployeeBanking, bool>>>()))
            .Returns(EmployeeBankingTestData.EmployeeBankingOne.ToMockIQueryable());

        await Assert.ThrowsAsync<CustomException>(() => _employeeBankingService.Update(EmployeeBankingTestData.EmployeeBankingOne.ToDto()));

        _mockUnitOfWork.Verify(x => x.EmployeeBanking.Get(It.IsAny<Expression<Func<EmployeeBanking, bool>>>()), Times.Never);
    }

    [Fact]
    public async Task Create_Pass()
    {
        _identity.Setup(i => i.Role).Returns("Admin");
        _identity.SetupGet(x => x.EmployeeId).Returns(1);

        _mockUnitOfWork
            .Setup(u => u.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
            .Returns(_employeeList.ToMockIQueryable());

        _mockUnitOfWork
            .Setup(u => u.EmployeeBanking.Add(It.IsAny<EmployeeBanking>()))
            .ReturnsAsync(EmployeeBankingTestData.EmployeeBankingOne);

        _mockUnitOfWork.Setup(e => e.Employee.Any(It.IsAny<Expression<Func<Employee, bool>>>()))
        .ReturnsAsync(true);

        var result =
            await _employeeBankingService.Create(EmployeeBankingTestData.EmployeeBankingOne.ToDto());

        Assert.Equivalent(EmployeeBankingTestData.EmployeeBankingOne.ToDto(), result);
    }

    [Fact]
    public async Task Create_UnauthorizedAccess()
    {
        _identity.Setup(i => i.Role).Returns("Employee");
        _identity.SetupGet(x => x.EmployeeId).Returns(2);

        _mockUnitOfWork
            .Setup(u => u.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
            .Returns(_employeeList.ToMockIQueryable());

        _mockUnitOfWork
            .Setup(u => u.EmployeeBanking.Add(It.IsAny<EmployeeBanking>()))
            .ReturnsAsync(EmployeeBankingTestData.EmployeeBankingOne);

        _mockUnitOfWork.Setup(e => e.Employee.Any(It.IsAny<Expression<Func<Employee, bool>>>()))
        .ReturnsAsync(true);

        var exception = await Assert.ThrowsAsync<CustomException>(() =>
               _employeeBankingService.Create(EmployeeBankingTestData.EmployeeBankingOne.ToDto()));

        Assert.Equivalent("Unauthorized Access", exception.Message);
    }

    [Fact]
    public async Task Create_AlreadyExists()
    {
        _identity.Setup(i => i.Role).Returns("Admin");
        _identity.SetupGet(x => x.EmployeeId).Returns(1);

        _mockUnitOfWork
            .Setup(u => u.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
            .Returns(_employeeList.ToMockIQueryable());

        _mockUnitOfWork
            .Setup(u => u.EmployeeBanking.Add(It.IsAny<EmployeeBanking>()))
            .ReturnsAsync(EmployeeBankingTestData.EmployeeBankingOne);

        _mockUnitOfWork.Setup(e => e.Employee.Any(It.IsAny<Expression<Func<Employee, bool>>>()))
        .ReturnsAsync(true);

        _mockUnitOfWork.Setup(e => e.EmployeeBanking.Any(It.IsAny<Expression<Func<EmployeeBanking, bool>>>()))
        .ReturnsAsync(true);

        await Assert.ThrowsAnyAsync<CustomException>(() => _employeeBankingService
                .Create(EmployeeBankingTestData.EmployeeBankingOne.ToDto()));

        _mockUnitOfWork.Verify(x => x.EmployeeBanking.Add(It.IsAny<EmployeeBanking>()), Times.Never);
        _mockUnitOfWork.Verify(x => x.EmployeeBanking.FirstOrDefault(It.IsAny<Expression<Func<EmployeeBanking, bool>>>()), Times.Never);
    }

    [Fact]
    public async Task GetBankingById_Pass()
    {
        _identity.Setup(i => i.Role).Returns("Admin");
        _identity.Setup(x => x.EmployeeId).Returns(1);

        _mockUnitOfWork
            .Setup(u => u.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
            .Returns(_employeeList.ToMockIQueryable());

        _mockUnitOfWork
            .Setup(u => u.EmployeeBanking.Get(It.IsAny<Expression<Func<EmployeeBanking, bool>>>()))
            .Returns(EmployeeBankingTestData.EmployeeBankingOne.ToMockIQueryable());

        _mockUnitOfWork.Setup(e => e.Employee.Any(It.IsAny<Expression<Func<Employee, bool>>>()))
        .ReturnsAsync(true);

        var result = await _employeeBankingService.GetBankingById(0);

        Assert.NotNull(result);
        Assert.Equivalent(EmployeeBankingTestData.EmployeeBankingOne.ToDto(), result[0]);
    }

    [Fact]
    public async Task GetBankingById_NotAuthorized()
    {
        _identity.Setup(i => i.Role).Returns("Journey");
        _identity.Setup(x => x.EmployeeId).Returns(2);

        _mockUnitOfWork
            .Setup(u => u.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
            .Returns(_testEmployee.ToMockIQueryable());

        _mockUnitOfWork
            .Setup(u => u.EmployeeBanking.Get(It.IsAny<Expression<Func<EmployeeBanking, bool>>>()))
            .Returns(EmployeeBankingTestData.EmployeeBankingOne.ToMockIQueryable());

        _mockUnitOfWork.Setup(e => e.Employee.Any(It.IsAny<Expression<Func<Employee, bool>>>()))
        .ReturnsAsync(true);

        var exception = await Assert.ThrowsAsync<CustomException>(() =>
               _employeeBankingService.GetBankingById(0));

        Assert.Equivalent("Unauthorized Access", exception.Message);
    }

    [Fact]
    public async Task GetBankingById_DoesNotExist()
    {
        _mockUnitOfWork
            .Setup(u => u.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
            .Returns(_employeeList.ToMockIQueryable());

        _mockUnitOfWork
            .Setup(u => u.EmployeeBanking.Get(It.IsAny<Expression<Func<EmployeeBanking, bool>>>()))
            .Returns(EmployeeBankingTestData.EmployeeBankingOne.ToMockIQueryable());

        await Assert.ThrowsAsync<CustomException>(() => _employeeBankingService.GetBankingById(1));

        _mockUnitOfWork.Verify(x => x.EmployeeBanking.Get(It.IsAny<Expression<Func<EmployeeBanking, bool>>>()), Times.Never);
    }

    [Fact]
    public async Task Delete_Pass()
    {
        _identity.Setup(i => i.Role).Returns("SuperAdmin");
        _identity.Setup(x => x.EmployeeId).Returns(1);

        _mockUnitOfWork
            .Setup(u => u.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
            .Returns(_employeeList.ToMockIQueryable());

        _mockUnitOfWork
            .Setup(u => u.EmployeeBanking.Get(It.IsAny<Expression<Func<EmployeeBanking, bool>>>()))
            .Returns(EmployeeBankingTestData.EmployeeBankingOne.ToMockIQueryable());

        _mockUnitOfWork.Setup(e => e.Employee.Any(It.IsAny<Expression<Func<Employee, bool>>>()))
        .ReturnsAsync(true);

        _mockUnitOfWork.Setup(e => e.EmployeeBanking.Any(It.IsAny<Expression<Func<EmployeeBanking, bool>>>()))
        .ReturnsAsync(true);

        _mockUnitOfWork.Setup(x => x.EmployeeBanking.Delete(It.IsAny<int>()))
                           .ReturnsAsync(EmployeeBankingTestData.EmployeeBankingOne);

        var result = await _employeeBankingService.Delete(0);
        Assert.NotNull(result);

        Assert.Equivalent(EmployeeBankingTestData.EmployeeBankingOne.ToDto(), result);
    }

    [Fact]
    public async Task Delete_DoesNotExist()
    {
        _mockUnitOfWork
            .Setup(u => u.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
            .Returns(_employeeList.ToMockIQueryable());

        _mockUnitOfWork
            .Setup(u => u.EmployeeBanking.Get(It.IsAny<Expression<Func<EmployeeBanking, bool>>>()))
            .Returns(EmployeeBankingTestData.EmployeeBankingOne.ToMockIQueryable());

        var exception = await Assert.ThrowsAsync<CustomException>(() => _employeeBankingService.Delete(0));

        Assert.Equivalent("Employee Banking Not Found", exception.Message);
    }
}