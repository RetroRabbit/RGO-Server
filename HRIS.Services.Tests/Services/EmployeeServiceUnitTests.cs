using System.Linq.Expressions;
using HRIS.Models;
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
    private readonly EmployeeService _employeeServiceUnauthorized;
    private readonly EmployeeService _employeeServiceJourney;
    private readonly AuthorizeIdentityMock _authorizedIdentity;
    private readonly AuthorizeIdentityMock _unauthorizedIdentity;
    private readonly AuthorizeIdentityMock _journeyIdentity;

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
        _authorizedIdentity = new AuthorizeIdentityMock("test@gmail.com", "test", "Admin", 1);
        _unauthorizedIdentity = new AuthorizeIdentityMock("test@gmail.com", "test", "Employee", 1);
        _journeyIdentity = new AuthorizeIdentityMock("test@retrorabbit.co.za", "test", "Journey", 1);
        Mock<IErrorLoggingService> errorLoggingServiceMock = new();
        Mock<IEmailService> emailService = new();
        _roleServiceMock = new Mock<IRoleService>();

        _employeeService = new EmployeeService(_employeeTypeServiceMock.Object, _dbMock.Object,
            _employeeAddressServiceMock.Object, _roleServiceMock.Object, errorLoggingServiceMock.Object,
            emailService.Object, _authorizedIdentity);

        _employeeServiceUnauthorized = new EmployeeService(_employeeTypeServiceMock.Object, _dbMock.Object,
           _employeeAddressServiceMock.Object, _roleServiceMock.Object, errorLoggingServiceMock.Object,
           emailService.Object, _unauthorizedIdentity);

        _employeeServiceJourney = new EmployeeService(_employeeTypeServiceMock.Object, _dbMock.Object,
           _employeeAddressServiceMock.Object, _roleServiceMock.Object, errorLoggingServiceMock.Object,
           emailService.Object, _journeyIdentity);
    }

    [Theory]
    [InlineData("Pass", false, false)]
    [InlineData("Unauthorized Access", false, false)]
    [InlineData("User already created", true, false)]
    [InlineData("Email Is Already in Use", false, true)]
    [InlineData("Employee Type Missing", false, false)]
    [InlineData("Log Email exception", false, false)]
    public async Task SaveEmployeeTests(string testCase, bool anySequenceOne, bool anySequenceTwo)
    {

        _dbMock.SetupSequence(e => e.Employee.Any(It.IsAny<Expression<Func<Employee, bool>>>()))
        .ReturnsAsync(anySequenceOne)
        .ReturnsAsync(anySequenceTwo);

        _employeeTypeServiceMock.Setup(r => r.GetEmployeeType(EmployeeTypeTestData.DeveloperType.Name))
                  .ReturnsAsync(EmployeeTypeTestData.DeveloperType.ToDto());

        var employeeRole = new EmployeeRole
        {
            Id = 0,
            Employee = EmployeeTestData.EmployeeOne,
            Role = EmployeeRoleTestData.RoleDtoEmployee
        };

        _employeeAddressServiceMock.Setup(r => r.Save(It.IsAny<EmployeeAddressDto>()))
                                  .ReturnsAsync(EmployeeAddressTestData.EmployeeAddressOne.ToDto());

        _employeeAddressServiceMock.Setup(r => r.Get(It.IsAny<EmployeeAddressDto>()))
                                  .ReturnsAsync(EmployeeAddressTestData.EmployeeAddressOne.ToDto());

        _roleServiceMock.Setup(r => r.GetRole("Employee")).ReturnsAsync(EmployeeRoleTestData.RoleDtoEmployee.ToDto());

        _dbMock.Setup(r => r.Employee.Add(It.IsAny<Employee>())).ReturnsAsync(EmployeeTestData.EmployeeOne);
        _dbMock.Setup(r => r.EmployeeRole.Add(It.IsAny<EmployeeRole>())).ReturnsAsync(employeeRole);

        if (testCase == "Pass")
        {
            var result = await _employeeService.CreateEmployee(EmployeeTestData.EmployeeOne.ToDto());
            Assert.NotNull(result);
            Assert.Equivalent(EmployeeTestData.EmployeeOne.ToDto(), result);
        }

        if (testCase == "Unauthorized Access")
        {
            var result = await Assert.ThrowsAsync<CustomException>(() => _employeeServiceUnauthorized.CreateEmployee(EmployeeTestData.EmployeeOne.ToDto()));
            Assert.Equal("Unauthorized Access", result.Message);
        }

        if (testCase == "Existing user" || testCase == "Email Is Already in Use")
        {
            var result = await Assert.ThrowsAsync<CustomException>(() => _employeeService.CreateEmployee(EmployeeTestData.EmployeeOne.ToDto()));
            Assert.Equal(testCase, result.Message);
        }

        if (testCase == "Employee Type Missing")
        {
            var result = await Assert.ThrowsAsync<CustomException>(() => _employeeService.CreateEmployee(EmployeeTestData.EmployeeNullType.ToDto()));
            Assert.Equal("Employee Type Missing", result.Message);
        }

        if (testCase == "Log Email exception")
        {
            //var result = await Assert.ThrowsAsync<CustomException>(() => _employeeService.CreateEmployee(EmployeeTestData.EmployeeOne.ToDto()));
            //Assert.Equal("", result.Message);
        }
    }


    [Theory]
    [InlineData("Pass")]
    [InlineData("Unauthorized Access")]
    [InlineData("This model does not exist")]
    [InlineData("Deleting the currently logged-in user is not permitted")]
    public async Task DeleteEmployeeTest(string testCase)
    {
        _employeeTypeServiceMock.Setup(r => r.GetEmployeeType(EmployeeTypeTestData.DeveloperType.Name))
                               .ReturnsAsync(EmployeeTypeTestData.DeveloperType.ToDto());

        var employeeList = new List<Employee>
        {
            EmployeeTestData.EmployeeTwo
        };

        var employeeList2 = new List<Employee>
        {
            EmployeeTestData.EmployeeOne
        };

        _dbMock.Setup(e => e.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
               .Returns(employeeList.ToMockIQueryable());

        _dbMock.Setup(e => e.Employee.Any(It.IsAny<Expression<Func<Employee, bool>>>()))
               .ReturnsAsync(true);

        _dbMock.Setup(r => r.Employee.Delete(EmployeeTestData.EmployeeTwo.Id))
               .ReturnsAsync(EmployeeTestData.EmployeeTwo);

        if (testCase == "Pass")
        {
            var result = await _employeeService.DeleteEmployee(EmployeeTestData.EmployeeTwo.Email!);
            Assert.NotNull(result);
        }

        if (testCase == "Unauthorized Access")
        {
            var result = await Assert.ThrowsAsync<CustomException>(() => _employeeServiceUnauthorized.DeleteEmployee(EmployeeTestData.EmployeeTwo.Email!));
            Assert.Equal("Unauthorized Access", result.Message);
        }

        if (testCase == "This model does not exist")
        {
            _dbMock.SetupSequence(e => e.Employee.Any(It.IsAny<Expression<Func<Employee, bool>>>()))
               .ReturnsAsync(true)
               .ReturnsAsync(false);

            var result = await Assert.ThrowsAsync<CustomException>(() => _employeeService.DeleteEmployee(EmployeeTestData.EmployeeTwo.Email!));
            Assert.Equal("This model does not exist", result.Message);
        }

        if (testCase == "Deleting the currently logged-in user is not permitted")
        {
            _dbMock.Setup(e => e.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
               .Returns(employeeList2.ToMockIQueryable());

            var result = await Assert.ThrowsAsync<CustomException>(() => _employeeService.DeleteEmployee(EmployeeTestData.EmployeeOne.Email!));
            Assert.Equal(testCase, result.Message);
        }
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

        _dbMock.Setup(e => e.Employee.Any(It.IsAny<Expression<Func<Employee, bool>>>()))
       .ReturnsAsync(true);

        var result = await _employeeService.GetAll();

        Assert.NotNull(result);
        Assert.IsType<List<EmployeeDto>>(result);
        Assert.True(result.SequenceEqual(result.OrderBy(e => e.Name)));
        _dbMock.Verify(u => u.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()), Times.Once);

        var result2 = await _employeeServiceJourney.GetAll(_authorizedIdentity.Email);
        Assert.NotNull(result);
        Assert.IsType<List<EmployeeDto>>(result);
        Assert.True(result.SequenceEqual(result.OrderBy(e => e.Name)));
        _dbMock.Verify(u => u.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()), Times.Exactly(3));
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

        var result = _employeeService.CheckUserEmailExist("dm@retrorabbit.co.za");

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

        _dbMock.Setup(e => e.Employee.Any(It.IsAny<Expression<Func<Employee, bool>>>()))
       .ReturnsAsync(true);

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
