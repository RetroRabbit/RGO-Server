using System.Linq.Expressions;
using System.Net.Mail;
using HRIS.Models;
using HRIS.Services.Interfaces;
using HRIS.Services.Interfaces.Helper;
using HRIS.Services.Services;
using Moq;
using RR.Tests.Data;
using RR.Tests.Data.Models.HRIS;
using RR.UnitOfWork;
using RR.UnitOfWork.Entities.HRIS;
using RR.UnitOfWork.Entities.Shared;
using Xunit;

namespace HRIS.Services.Tests.Services;

public class EmployeeServiceUnitTests
{
    private readonly Mock<IUnitOfWork> _dbMock;
    private readonly Mock<IEmployeeAddressService> _employeeAddressServiceMock;
    private readonly Mock<IEmployeeTypeService> _employeeTypeServiceMock;
    private readonly Mock<IRoleService> _roleServiceMock;
    private readonly Mock<IErrorLoggingService> _errorLoggingServiceMock;
    private readonly Mock<IEmailHelper> _emailHelper;
    private readonly Mock<IEmailService> _emailService;
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
        _errorLoggingServiceMock = new Mock<IErrorLoggingService> ();
        _emailHelper = new Mock<IEmailHelper>();
        _emailService = new Mock<IEmailService> ();

        Mock<IEmailService> emailService = new();
        _roleServiceMock = new Mock<IRoleService>();

        _employeeService = new EmployeeService(_employeeTypeServiceMock.Object, _dbMock.Object,
            _employeeAddressServiceMock.Object, _roleServiceMock.Object, _errorLoggingServiceMock.Object,
            emailService.Object, _authorizedIdentity);

        _employeeServiceUnauthorized = new EmployeeService(_employeeTypeServiceMock.Object, _dbMock.Object,
           _employeeAddressServiceMock.Object, _roleServiceMock.Object, _errorLoggingServiceMock.Object,
           emailService.Object, _unauthorizedIdentity);

        _employeeServiceJourney = new EmployeeService(_employeeTypeServiceMock.Object, _dbMock.Object,
           _employeeAddressServiceMock.Object, _roleServiceMock.Object, _errorLoggingServiceMock.Object,
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

        if (testCase == "User already created" || testCase == "Email Is Already in Use")
        {
            var result = await Assert.ThrowsAsync<CustomException>(() => _employeeService.CreateEmployee(EmployeeTestData.EmployeeOne.ToDto()));
            Assert.Equal(testCase, result.Message);
        }

        if (testCase == "Employee Type Missing")
        {
            var result = await Assert.ThrowsAsync<CustomException>(() => _employeeService.CreateEmployee(EmployeeTestData.EmployeeNullType.ToDto()));
            Assert.Equal("Employee Type Missing", result.Message);
        }

        //if (testCase == "Log Email exception")
        //{
        //    _emailService.Setup(x => x.Send(EmployeeTestData.EmployeeOne.ToDto(), "Employee Name")).Throws(new Exception());
        //    _emailHelper.Setup(x => x.GetTemplate(It.IsAny<string>())).ReturnsAsync(new EmailTemplate());
        //    _emailHelper.Setup(x => x.CompileMessage(It.IsAny<MailAddress>(), It.IsAny<EmailTemplate>(), It.IsAny<object>())).Returns(new MailMessage());
        //    _emailHelper.Setup(x => x.SendMailAsync(It.IsAny<MailMessage>())).Throws<SmtpException>();
        //    _dbMock.Setup(x => x.EmailHistory.Add(It.IsAny<EmailHistory>())).ReturnsAsync(new EmailHistory());
        //    _dbMock.Setup(x => x.EmailHistory.Update(It.IsAny<EmailHistory>())).ReturnsAsync(new EmailHistory());
        //    _errorLoggingServiceMock.Setup(x => x.LogException(It.IsAny<SmtpException>()));

        //    _emailHelper.Verify(x => x.GetTemplate(It.IsAny<string>()), Times.Once);
        //    _emailHelper.Verify(x => x.CompileMessage(It.IsAny<MailAddress>(), It.IsAny<EmailTemplate>(), It.IsAny<object>()), Times.Once);
        //    _emailHelper.Verify(x => x.SendMailAsync(It.IsAny<MailMessage>()), Times.Once);
        //    _dbMock.Verify(x => x.EmailHistory.Add(It.IsAny<EmailHistory>()), Times.Once);
        //    _dbMock.Verify(x => x.EmailHistory.Update(It.IsAny<EmailHistory>()), Times.Once);
        //    _errorLoggingServiceMock.Verify(x => x.LogException(It.IsAny<SmtpException>()), Times.Once);
        //}
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
    public async Task GetAllTests()
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

        var passResult = await _employeeService.GetAll();

        Assert.NotNull(passResult);
        Assert.IsType<List<EmployeeDto>>(passResult);
        Assert.True(passResult.SequenceEqual(passResult.OrderBy(e => e.Name)));
        _dbMock.Verify(u => u.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()), Times.Once);

        var passResultJourney = await _employeeServiceJourney.GetAll(_authorizedIdentity.Email);
        Assert.NotNull(passResultJourney);
        Assert.IsType<List<EmployeeDto>>(passResultJourney);
        Assert.True(passResultJourney.SequenceEqual(passResultJourney.OrderBy(e => e.Name)));
        _dbMock.Verify(u => u.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()), Times.Exactly(3));


        var failResultUnauthorized = await Assert.ThrowsAsync<CustomException>(() => _employeeServiceUnauthorized.GetAll(EmployeeTestData.EmployeeOne.Email!));
        Assert.Equal("Unauthorized Access", failResultUnauthorized.Message);
    }

    [Theory]
    [InlineData("Pass")]
    [InlineData("User email not found")]
    [InlineData("Unauthorized Access")]
    public async Task GetEmployeeTests(string testCase)
    {
        var employeeList = new List<Employee>
        {
            EmployeeTestData.EmployeeOne
        };

        _dbMock.Setup(e => e.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
               .Returns(employeeList.ToMockIQueryable());

        if (testCase == "Pass")
        {
            var result = _employeeService.GetEmployee("dm@retrorabbit.co.za");
            Assert.NotNull(result);
        }

        if (testCase == "User email not found")
        {
            _dbMock.Setup(e => e.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
               .Returns(new List<Employee>().ToMockIQueryable());

            var failResult = await Assert.ThrowsAsync<CustomException>(() => _employeeService.GetEmployee(EmployeeTestData.EmployeeOne.Email!));
            Assert.Equal(testCase, failResult.Message);
        }

        if (testCase == "Unauthorized Access")
        {
            _dbMock.Setup(e => e.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
                .Returns(new List<Employee> { EmployeeTestData.EmployeeTwo}.ToMockIQueryable());

            _dbMock.Setup(e => e.Employee.Any(It.IsAny<Expression<Func<Employee, bool>>>()))
                   .ReturnsAsync(true);

            var failResult = await Assert.ThrowsAsync<CustomException>(() => _employeeServiceUnauthorized.GetEmployee(EmployeeTestData.EmployeeTwo.Email!));
            Assert.Equal(testCase, failResult.Message);
        }
    }

    [Fact]
    public async Task GetEmployeeByIdTests()
    {
        var employees = new List<Employee> { EmployeeTestData.EmployeeOne, EmployeeTestData.EmployeeTwo };

        _dbMock.Setup(e => e.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
               .Returns(employees.ToMockIQueryable());

        var result = await _employeeService.GetEmployeeById(EmployeeTestData.EmployeeOne.Id);

        Assert.NotNull(result);
        Assert.Equivalent(EmployeeTestData.EmployeeOne.ToDto(), result);

        var failResultUnauthorized = await Assert.ThrowsAsync<CustomException>(() => _employeeServiceUnauthorized.GetEmployeeById(EmployeeTestData.EmployeeTwo.Id!));
        Assert.Equal("Unauthorized Access", failResultUnauthorized.Message);
    }

    [Theory]
    [InlineData("Pass")]
    [InlineData("Unauthorized Access")]
    [InlineData("User not found")]
    public async Task UpdateEmployee(string testCase)
    {
        if (testCase == "Unauthorized Access")
        {
            var failResultUnauthorized = await Assert.ThrowsAsync<CustomException>(() => _employeeServiceUnauthorized.UpdateEmployee(EmployeeTestData.EmployeeTwo.ToDto(),EmployeeTestData.EmployeeTwo.Email!));
            Assert.Equal(testCase, failResultUnauthorized.Message);
        }

        if (testCase == "User not found")
        {
            _dbMock.Setup(r => r.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>())).Returns(new List<Employee> { null }.ToMockIQueryable());

            var failResultUnauthorized = await Assert.ThrowsAsync<CustomException>(() => _employeeService.UpdateEmployee(EmployeeTestData.EmployeeTwo.ToDto(), EmployeeTestData.EmployeeTwo.Email!));
            Assert.Equal(testCase, failResultUnauthorized.Message);
        }

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

        if (testCase == "Pass")
        {
            Assert.NotNull(result);
            Assert.Equivalent(EmployeeTestData.EmployeeOne.ToDto(), result);
        }
    }

    [Theory]
    [InlineData("Pass")]
    [InlineData("Unauthorized Access")]
    [InlineData("User not found")]
    public async Task GetByIdTests(string testCase)
    {
        if (testCase == "Unauthorized Access")
        {
            var failResultUnauthorized = await Assert.ThrowsAsync<CustomException>(() => _employeeServiceUnauthorized.GetById(EmployeeTestData.EmployeeTwo.Id!));
            Assert.Equal(testCase, failResultUnauthorized.Message);
        }

        if (testCase == "User not found")
        {
            _dbMock.Setup(e => e.Employee.GetById(It.IsAny<int>()))
                .ReturnsAsync((int id) => null);

            var failResultUnauthorized = await Assert.ThrowsAsync<CustomException>(() => _employeeService.GetById(EmployeeTestData.EmployeeTwo.Id!));
            Assert.Equal(testCase, failResultUnauthorized.Message);
        }

        _dbMock.Setup(x => x.Employee.GetById(EmployeeTestData.EmployeeOne.Id))
               .ReturnsAsync(EmployeeTestData.EmployeeOne);

        if (testCase == "Pass")
        {
            var result = await _employeeService.GetById(EmployeeTestData.EmployeeOne.Id);

            Assert.NotNull(result);
            Assert.Equivalent(EmployeeTestData.EmployeeOne.ToDto(), result);
        }
    }

    [Theory]
    [InlineData("Pass")]
    [InlineData("Unauthorized Access")]
    [InlineData("Model not found")]
    public async Task GetSimpleProfileTests( string testCase)
    {
        if (testCase == "Unauthorized Access")
        {
            _dbMock.Setup(r => r.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>())).Returns(new Employee().ToMockIQueryable());
            _dbMock.Setup(r => r.Employee.Any(It.IsAny<Expression<Func<Employee, bool>>>())).ReturnsAsync(true);

            var failResultUnauthorized = await Assert.ThrowsAsync<CustomException>(() => _employeeServiceUnauthorized.GetSimpleProfile(EmployeeTestData.EmployeeTwo.Email!));
            Assert.Equal(testCase, failResultUnauthorized.Message);
        }

        if (testCase == "Model not found")
        {
            _dbMock.Setup(r => r.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>())).Returns(new Employee().ToMockIQueryable());
            _dbMock.Setup(r => r.Employee.Any(It.IsAny<Expression<Func<Employee, bool>>>())).ReturnsAsync(false);

            var failResultUnauthorized = await Assert.ThrowsAsync<CustomException>(() => _employeeService.GetSimpleProfile(EmployeeTestData.EmployeeTwo.Email!));
            Assert.Equal(testCase, failResultUnauthorized.Message);
        }

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

        if (testCase == "Pass")
        {
            var result = await _employeeService.GetSimpleProfile(EmployeeTestData.EmployeeFour.Email!);

            Assert.NotNull(result);
            Assert.Equal(EmployeeTestData.EmployeeFour.TeamLead, result.TeamLeadId);
            Assert.Equal(EmployeeTestData.EmployeeFour.PeopleChampion, result.PeopleChampionId);
            Assert.Equal(allocatedClient.Name, result.ClientAllocatedName);
        }
    }

    [Theory]
    [InlineData("Unauthorized Access")]
    [InlineData("Pass")]
    [InlineData("Users not found")]
    public async Task FilterEmployeesTests(string testCase)
    {
        var employee = EmployeeTestData.EmployeeFour;
        var employeeList = new List<Employee>
        {
            employee
        };

        if (testCase == "Unauthorized Access")
        {
            var failResultUnauthorized = await Assert.ThrowsAsync<CustomException>(() => _employeeServiceUnauthorized.FilterEmployees(employee.PeopleChampion!.Value, employee.EmployeeType!.Id, employee.Active));
            Assert.Equal(testCase, failResultUnauthorized.Message);
        }

        if (testCase == "Pass")
        {
            _dbMock.Setup(e => e.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
                .Returns(employeeList.ToMockIQueryable());

            var result = await _employeeService.FilterEmployees(employee.PeopleChampion!.Value, employee.EmployeeType!.Id, employee.Active);

            Assert.NotNull(result);
        }

        if (testCase == "Users not found")
        {
            _dbMock.Setup(e => e.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
                .Returns(new List<Employee>().ToMockIQueryable());
            var failResultUnauthorized = await Assert.ThrowsAsync<CustomException>(() => _employeeService.FilterEmployees(employee.PeopleChampion!.Value, employee.EmployeeType!.Id, employee.Active));
            Assert.Equal(testCase, failResultUnauthorized.Message);
        }
    }

    [Theory]
    [InlineData("Unauthorized Access")]
    [InlineData("Model not found")]
    [InlineData("Pass")]
    public async Task CheckDuplicateIdNumberTests(string testCase)
    {
        var employeeList = new List<Employee>
        {
            EmployeeTestData.EmployeeOne
        };

        if (testCase == "Unauthorized Access")
        {
            var failResultUnauthorized = await Assert.ThrowsAsync<CustomException>(() => _employeeServiceUnauthorized.CheckDuplicateIdNumber(EmployeeTestData.EmployeeOne.IdNumber!, EmployeeTestData.EmployeeOne.Id));
            Assert.Equal(testCase, failResultUnauthorized.Message);
        }

        if ( testCase == "Model not found")
        {
            _dbMock.Setup(e => e.Employee.Any(It.IsAny<Expression<Func<Employee, bool>>>()))
                .ReturnsAsync(false);

            var failResultUnauthorized = await Assert.ThrowsAsync<CustomException>(() => _employeeService.CheckDuplicateIdNumber(EmployeeTestData.EmployeeOne.IdNumber!, EmployeeTestData.EmployeeOne.Id));
            Assert.Equal(testCase, failResultUnauthorized.Message);
        }

        if (testCase == "Pass")
        {
            _dbMock.Setup(e => e.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
                .Returns(employeeList.ToMockIQueryable());
            _dbMock.SetupSequence(e => e.Employee.Any(It.IsAny<Expression<Func<Employee, bool>>>()))
                .ReturnsAsync(true)
                .ReturnsAsync(true);

            var result = await _employeeService.CheckDuplicateIdNumber(EmployeeTestData.EmployeeOne.IdNumber!, EmployeeTestData.EmployeeOne.Id);

            Assert.True(result);
        }
    }
}