using System.Linq.Expressions;
using HRIS.Models;
using HRIS.Models.Enums;
using HRIS.Services.Interfaces;
using HRIS.Services.Services;
using Moq;
using RR.Tests.Data;
using RR.Tests.Data.Models.HRIS;
using RR.UnitOfWork;
using RR.UnitOfWork.Entities.HRIS;
using Xunit;

namespace HRIS.Services.Tests.Services;

public class EmployeeServiceUnitTests
{
    private readonly Mock<IUnitOfWork> _dbMock;
    private readonly Mock<IEmployeeAddressService> _employeeAddressServiceMock;
    private readonly Mock<IEmployeeTypeService> _employeeTypeServiceMock;
    private readonly Mock<IRoleService> _roleServiceMock;
    private readonly Mock<IErrorLoggingService> _errorLoggingServiceMock;
    private readonly EmployeeService _employeeService;

    private readonly EmployeeRole _employeeRoleDto = new()
    {
        Id = 0,
        Employee = EmployeeTestData.EmployeeOne,
        Role = EmployeeRoleTestData.RoleDtoEmployee
    };

    public EmployeeServiceUnitTests()
    {
        _dbMock = new Mock<IUnitOfWork>();
        _employeeTypeServiceMock = new Mock<IEmployeeTypeService>();
        _employeeAddressServiceMock = new Mock<IEmployeeAddressService>();
        Mock<IErrorLoggingService> errorLoggingServiceMock = new();
        Mock<IEmailService> emailService = new();
        _roleServiceMock = new Mock<IRoleService>();
        _employeeService = new EmployeeService(_employeeTypeServiceMock.Object, _dbMock.Object,
            _employeeAddressServiceMock.Object, _roleServiceMock.Object, errorLoggingServiceMock.Object,
            emailService.Object);
    }

    [Fact]
    public async Task SaveEmployeeFailTest1()
    {
        _dbMock.Setup(r => r.Employee.Any(It.IsAny<Expression<Func<Employee, bool>>>())).ReturnsAsync(true);

        await Assert.ThrowsAsync<CustomException>(() => _employeeService.SaveEmployee(EmployeeTestData.EmployeeOne.ToDto()));
    }

    [Fact]
    public async Task SaveEmployeeTest()
    {
        _employeeTypeServiceMock.Setup(r => r.GetEmployeeType(EmployeeTypeTestData.DeveloperType.Name))
                               .ReturnsAsync(EmployeeTypeTestData.DeveloperType.ToDto());
        var employeeRole = new EmployeeRole
        {
            Id = 0,
            Employee = EmployeeTestData.EmployeeOne,
            Role = EmployeeRoleTestData.RoleDtoEmployee
        };

        _employeeAddressServiceMock.SetupSequence(r => r.CheckIfExists(It.IsAny<int>()))
                                  .ReturnsAsync(false)
                                  .ReturnsAsync(true);

        _employeeAddressServiceMock.Setup(r => r.Create(It.IsAny<EmployeeAddressDto>()))
                                  .ReturnsAsync(EmployeeAddressTestData.EmployeeAddressOne.ToDto());

        _employeeAddressServiceMock.Setup(r => r.GetById(It.IsAny<int>()))
                                  .ReturnsAsync(EmployeeAddressTestData.EmployeeAddressOne.ToDto());

        _roleServiceMock.Setup(r => r.GetRole("Employee")).ReturnsAsync(EmployeeRoleTestData.RoleDtoEmployee.ToDto());

        _dbMock.Setup(r => r.Employee.Add(It.IsAny<Employee>())).ReturnsAsync(EmployeeTestData.EmployeeOne);
        _dbMock.Setup(r => r.EmployeeRole.Add(It.IsAny<EmployeeRole>())).ReturnsAsync(employeeRole);

        var result = await _employeeService.SaveEmployee(EmployeeTestData.EmployeeOne.ToDto());
        Assert.NotNull(result);
        Assert.Equivalent(EmployeeTestData.EmployeeOne.ToDto(), result);
    }

    [Fact]
    public void GetEmployeeTest()
    {
        var employeeList = new List<Employee>
        {
            EmployeeTestData.EmployeeOne
        };

        _dbMock.Setup(e => e.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
               .Returns(employeeList.ToMockIQueryable());

        var result = _employeeService.GetEmployee("dm@retrorabbit.co.za");

        Assert.NotNull(result);
    }

    [Fact]
    public async Task DeleteEmployeeTest()
    {
        _employeeTypeServiceMock.Setup(r => r.GetEmployeeType(EmployeeTypeTestData.DeveloperType.Name))
                               .ReturnsAsync(EmployeeTypeTestData.DeveloperType.ToDto());

        var employeeList = new List<Employee>
        {
            EmployeeTestData.EmployeeOne
        };

        _dbMock.Setup(e => e.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
               .Returns(employeeList.ToMockIQueryable());

        _dbMock.Setup(r => r.Employee.Delete(EmployeeTestData.EmployeeOne.Id))
               .ReturnsAsync(EmployeeTestData.EmployeeOne);

        var result = await _employeeService.DeleteEmployee(EmployeeTestData.EmployeeOne.Email!);
        Assert.NotNull(result);
    }

    [Fact]
    public async Task GetAllTest()
    {
        var employees = new List<Employee>
        {
            EmployeeTestData.EmployeeOne,
            EmployeeTestData.EmployeeTwo,
            EmployeeTestData.EmployeeThree
        };

        _dbMock.Setup(u => u.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
               .Returns(employees.ToMockIQueryable());

        var result = await _employeeService.GetAll();

        Assert.NotNull(result);
        Assert.IsType<List<EmployeeDto>>(result);
        Assert.True(result.SequenceEqual(result.OrderBy(e => e.Name)));
        _dbMock.Verify(u => u.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()), Times.Once);
    }

    [Fact]
    public async Task GetAllIsJourney()
    {
        var emp = EmployeeTestData.EmployeeTwo;
        emp.EmployeeType = EmployeeTypeTestData.DeveloperType;

        var empRole = new EmployeeRole
        {
            Id = 1,
            Employee = EmployeeTestData.EmployeeOne,
            Role = EmployeeRoleTestData.RoleDtoEmployee
        };

        var employees = new List<Employee> { emp };
        var empRoles = new List<EmployeeRole> { empRole };
        var roles = new List<Role> { EmployeeRoleTestData.RoleDtoEmployee };

        var mockEmployees = employees;
        var expectedEmployees = new List<Employee> { EmployeeTestData.EmployeeThree };
        _dbMock.Setup(x => x.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
               .Returns(mockEmployees.ToMockIQueryable());

        _dbMock.Setup(x => x.EmployeeRole.Get(It.IsAny<Expression<Func<EmployeeRole, bool>>>()))
               .Returns(empRoles.ToMockIQueryable());

        _dbMock.Setup(x => x.Role.Get(It.IsAny<Expression<Func<Role, bool>>>()))
               .Returns(roles.ToMockIQueryable());

        _dbMock.Setup(x => x.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
               .Returns(expectedEmployees.ToMockIQueryable());

        var journeyEmployees = await _employeeService.GetAll(EmployeeTestData.EmployeeTwo.Email!);

        Assert.Single(journeyEmployees);
    }

    [Fact]
    public void CheckEmployeeExistsTest()
    {
        _dbMock.Setup(r => r.Employee.Any(It.IsAny<Expression<Func<Employee, bool>>>())).ReturnsAsync(true);

        var result = _employeeService.CheckUserExist("dm@retrorabbit.co.za");

        Assert.NotNull(result);
    }

    [Fact]
    public async Task UpdateEmployee()
    {
        var emp = EmployeeTestData.EmployeeTwo;

        var roleDto = new Role { Id = 2, Description = "Admin" };
        var empRole = new EmployeeRole { Id = 1, Employee = EmployeeTestData.EmployeeTwo, Role = roleDto };

        List<Employee> employees = new() { emp };
        List<EmployeeRole> empRoles = new() { empRole };
        List<Role> roles = new() { roleDto };

        _employeeTypeServiceMock.Setup(r => r.GetEmployeeType(EmployeeTypeTestData.DeveloperType.Name)).ReturnsAsync(EmployeeTypeTestData.DeveloperType.ToDto());

        _dbMock.Setup(r => r.Employee.Update(It.IsAny<Employee>())).ReturnsAsync(EmployeeTestData.EmployeeOne);
        _dbMock.Setup(r => r.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>())).Returns(EmployeeTestData.EmployeeOne.ToMockIQueryable());
        _dbMock.Setup(r => r.Employee.Any(It.IsAny<Expression<Func<Employee, bool>>>())).ReturnsAsync(true);
        _dbMock.Setup(r => r.EmployeeRole.Get(It.IsAny<Expression<Func<EmployeeRole, bool>>>())).Returns(empRoles.ToMockIQueryable());
        _dbMock.Setup(r => r.Role.Get(It.IsAny<Expression<Func<Role, bool>>>())).Returns(roles.ToMockIQueryable());

        var result = await _employeeService.UpdateEmployee(EmployeeTestData.EmployeeOne.ToDto(), "admin@retrorabbit.co.za");

        Assert.NotNull(result);
        Assert.Equivalent(EmployeeTestData.EmployeeOne.ToDto(), result);
    }

    [Fact]
    public async Task GetByIdPass()
    {
        _dbMock.Setup(x => x.Employee.GetById(EmployeeTestData.EmployeeOne.Id))
               .ReturnsAsync(EmployeeTestData.EmployeeOne);

        var result = await _employeeService.GetById(EmployeeTestData.EmployeeOne.Id);

        Assert.NotNull(result);
        Assert.Equivalent(EmployeeTestData.EmployeeOne.ToDto(), result);
    }

    [Fact]
    public async Task GetSimpleProfileWithPcAndTeamLeadAndClient()
    {
        var allocatedClient = new ClientDto { Id = 1, Name = "FNB" };
        var clients = new List<Client> { new(allocatedClient) };

        var employeeList = new List<Employee> { EmployeeTestData.EmployeeFour };
        
        _dbMock.Setup(e => e.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
               .Returns(employeeList.ToMockIQueryable());

        _dbMock.Setup(e => e.Employee.GetById(It.IsAny<int>()))
               .ReturnsAsync((int id) =>
                                 id == EmployeeTestData.EmployeeFour.TeamLead
                                     ? EmployeeTestData.EmployeeThree
                                     : EmployeeTestData.EmployeeTwo);

        _dbMock.Setup(db => db.Client.Get(It.IsAny<Expression<Func<Client, bool>>>()))
               .Returns(clients.ToMockIQueryable());

        var result = await _employeeService.GetSimpleProfile(EmployeeTestData.EmployeeFour.Email!);

        Assert.NotNull(result);
        Assert.Equal(EmployeeTestData.EmployeeFour.TeamLead, result.TeamLeadId);
        Assert.Equal(EmployeeTestData.EmployeeFour.PeopleChampion, result.PeopleChampionId);
        Assert.Equal(allocatedClient.Name, result.ClientAllocatedName);
    }

    [Fact]
    public async Task GetEmployeeByIdPass()
    {
        var employees = new List<Employee> { EmployeeTestData.EmployeeOne };

        _dbMock.Setup(e => e.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
               .Returns(employees.ToMockIQueryable());

        var result = await _employeeService.GetEmployeeById(EmployeeTestData.EmployeeOne.Id);

        Assert.NotNull(result);
        Assert.Equivalent(EmployeeTestData.EmployeeOne.ToDto(), result);
    }

    [Fact]
    public async Task GetEmployeeByIdFail()
    {
        var mockEmployees = new List<Employee>();

        _dbMock.Setup(x => x.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
               .Returns(mockEmployees.ToMockIQueryable());

        await Assert.ThrowsAsync<CustomException>(() => _employeeService.GetEmployeeById(2));
    }   
}
