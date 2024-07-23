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
    private readonly Termination _termination;
    private readonly Employee _employee;
    private readonly EmployeeType _employeeType;
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
        
        _terminationService = new TerminationService(_db.Object, _employeeTypeServiceMock.Object, _employeeServiceMock.Object, _authServiceMock.Object,
            new AuthorizeIdentityMock("test@gmail.com", "test", "Admin", 1));

        _termination = new Termination
        {
            Id = 1,
            EmployeeId = 3,
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

        _employee = EmployeeTestData.EmployeeOne;
        _employeeType = EmployeeTypeTestData.DeveloperType;
    }

    [Fact]
    public async Task CheckIfModelExistsReturnsTrue()
    {
        var testId = 1;
        _db.Setup(x => x.Termination.Any(It.IsAny<Expression<Func<Termination, bool>>>()))
            .ReturnsAsync(true);

        var result = await _terminationService.CheckTerminationExist(testId);

        Assert.True(result);
        _db.Verify(x => x.Termination.Any(It.IsAny<Expression<Func<Termination, bool>>>()), Times.Once);
    }

    [Fact]
    public async Task CheckIfModelExistsReturnsFalse()
    {
        var testId = 1;
        _db.Setup(x => x.Termination.Any(It.IsAny<Expression<Func<Termination, bool>>>()))
            .ReturnsAsync(false);

        var result = await _terminationService.CheckTerminationExist(testId);

        Assert.False(result);
        _db.Verify(x => x.Termination.Any(It.IsAny<Expression<Func<Termination, bool>>>()), Times.Once);
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
           .ReturnsAsync(true);

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
            .ReturnsAsync(_termination);

        _employeeServiceMock.Setup(e => e.GetEmployeeById(It.IsAny<int>()))
            .ReturnsAsync(_employee.ToDto);

        _employeeTypeServiceMock.Setup(et => et.GetEmployeeType(EmployeeTypeTestData.DeveloperType!.Name))
            .ReturnsAsync(EmployeeTypeTestData.DeveloperType.ToDto());

        _employeeTypeServiceMock.Setup(et => et.GetEmployeeType(_employee.EmployeeType!.Name))
            .ReturnsAsync(_employeeType.ToDto);

        _authServiceMock.Setup(a => a.DeleteUser(It.IsAny<string>())).ReturnsAsync(true);

        await _terminationService.CreateTermination(_termination.ToDto());

        _db.Verify(x => x.Termination.Add(It.IsAny<Termination>()), Times.Once);
    }

    [Fact]
    public async Task SaveFailTest_UnauthorizedAccess()
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
            .ReturnsAsync(_termination);

        _employeeServiceMock.Setup(e => e.GetEmployeeById(It.IsAny<int>()))
            .ReturnsAsync(_employee.ToDto);

        _employeeTypeServiceMock.Setup(et => et.GetEmployeeType(EmployeeTypeTestData.DeveloperType!.Name))
            .ReturnsAsync(EmployeeTypeTestData.DeveloperType.ToDto());

        _employeeTypeServiceMock.Setup(et => et.GetEmployeeType(_employee.EmployeeType!.Name))
            .ReturnsAsync(_employeeType.ToDto);

        _authServiceMock.Setup(a => a.DeleteUser(It.IsAny<string>())).ReturnsAsync(true);

        await Assert.ThrowsAsync<CustomException>(() => _terminationService.CreateTermination(_termination.ToDto()));
    }

    [Fact]
    public async Task UpdatePassTest()
    {
        _db.Setup(x => x.Termination.Any(It.IsAny<Expression<Func<Termination, bool>>>())).ReturnsAsync(true);
      
        _db.Setup(r => r.Termination.Update(It.IsAny<Termination>()))
            .ReturnsAsync(_termination);

        await _terminationService.UpdateTermination(_termination.ToDto());

        _db.Verify(x => x.Termination.Update(It.IsAny<Termination>()), Times.Once);
    }

    [Fact]
    public async Task UpdateFailTest_ModelNotFound()
    {
        _db.Setup(x => x.Termination.Any(It.IsAny<Expression<Func<Termination, bool>>>()))
           .ReturnsAsync(false);

        _db.Setup(r => r.Termination.Update(It.IsAny<Termination>()))
            .ReturnsAsync(_termination);

        await Assert.ThrowsAsync<CustomException>(() => _terminationService.UpdateTermination(_termination.ToDto()));
    }

    [Fact]
    public async Task GetByIdFailTest()
    {
        _db.Setup(x => x.Termination.Any(It.IsAny<Expression<Func<Termination, bool>>>())).ReturnsAsync(false);

        await Assert.ThrowsAsync<CustomException>(() => _terminationService.GetTerminationByEmployeeId(0));
    }

    [Fact]
    public async Task GetByIdPassTest()
    {
        _db.Setup(x => x.Termination.Any(It.IsAny<Expression<Func<Termination, bool>>>())).ReturnsAsync(true);
        _db.Setup(x => x.Termination.FirstOrDefault(It.IsAny<Expression<Func<Termination, bool>>>())).ReturnsAsync(_termination);

        var result = await _terminationService.GetTerminationByEmployeeId(1);

        _db.Verify(x => x.Termination.FirstOrDefault(It.IsAny<Expression<Func<Termination, bool>>>()), Times.Once);
        Assert.NotNull(result);
    }

    [Fact]
    public async Task SaveFailTest_UnauthorizedAccessByRole()
    {
        var nonSupportIdentity = new AuthorizeIdentityMock("test@gmail.com", "test", "User", 1); 
        var terminationServiceWithNonSupportIdentity = new TerminationService(_db.Object, _employeeTypeServiceMock.Object, _employeeServiceMock.Object, _authServiceMock.Object, nonSupportIdentity);

        _db.Setup(x => x.Termination.Any(It.IsAny<Expression<Func<Termination, bool>>>()))
           .ReturnsAsync(true);

        _employeeServiceMock.Setup(e => e.GetEmployeeById(It.IsAny<int>()))
            .ReturnsAsync(_employee.ToDto);

        await Assert.ThrowsAsync<CustomException>(() => terminationServiceWithNonSupportIdentity.CreateTermination(_termination.ToDto()));
    }

    [Fact]
    public async Task UpdateTermination_UnauthorizedAccessByRole()
    {
        var nonSupportIdentity = new AuthorizeIdentityMock("test@gmail.com", "test", "User", 1);
        var terminationServiceWithNonSupportIdentity = new TerminationService(_db.Object, _employeeTypeServiceMock.Object, _employeeServiceMock.Object, _authServiceMock.Object, nonSupportIdentity);

        _db.Setup(x => x.Termination.Any(It.IsAny<Expression<Func<Termination, bool>>>()))
           .ReturnsAsync(true);

        _employeeServiceMock.Setup(e => e.GetEmployeeById(It.IsAny<int>()))
            .ReturnsAsync(_employee.ToDto);

        await Assert.ThrowsAsync<CustomException>(() => terminationServiceWithNonSupportIdentity.UpdateTermination(_termination.ToDto()));
    }

    [Fact]
    public async Task GetTermination_UnauthorizedAccessByRole()
    {
        var nonSupportIdentity = new AuthorizeIdentityMock("test@gmail.com", "test", "User", 1);
        var terminationServiceWithNonSupportIdentity = new TerminationService(_db.Object, _employeeTypeServiceMock.Object, _employeeServiceMock.Object, _authServiceMock.Object, nonSupportIdentity);

        _db.Setup(x => x.Termination.Any(It.IsAny<Expression<Func<Termination, bool>>>()))
           .ReturnsAsync(true);

        _employeeServiceMock.Setup(e => e.GetEmployeeById(It.IsAny<int>()))
            .ReturnsAsync(_employee.ToDto);

        await Assert.ThrowsAsync<CustomException>(() => terminationServiceWithNonSupportIdentity.GetTerminationByEmployeeId(1));
    }

    [Fact]
    public async Task SaveFailTest_SelfTermination()
    {
        var identityTerminatingSelf = new AuthorizeIdentityMock("test@gmail.com", "test", "Admin", _termination.EmployeeId); // EmployeeId same as termination
        var terminationServiceWithSelfTerminatingIdentity = new TerminationService(_db.Object, _employeeTypeServiceMock.Object, _employeeServiceMock.Object, _authServiceMock.Object, identityTerminatingSelf);

        _db.Setup(x => x.Termination.Any(It.IsAny<Expression<Func<Termination, bool>>>()))
           .ReturnsAsync(true);

        _employeeServiceMock.Setup(e => e.GetEmployeeById(It.IsAny<int>()))
            .ReturnsAsync(_employee.ToDto);

        await Assert.ThrowsAsync<CustomException>(() => terminationServiceWithSelfTerminatingIdentity.CreateTermination(_termination.ToDto()));
    }
}
