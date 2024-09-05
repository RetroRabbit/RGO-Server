using System.Linq.Expressions;
using System.Net.Mail;
using AutoMapper;
using HRIS.Models;
using HRIS.Services.Interfaces;
using HRIS.Services.Interfaces.Helper;
using HRIS.Services.Services;
using Microsoft.EntityFrameworkCore;
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
    private readonly Mock<IAuthService> _authServiceMock;
    private readonly Mock<IEmailHelper> _emailHelper;
    private readonly Mock<IEmailService> _emailService;
    private readonly EmployeeService _employeeService;
    private readonly EmployeeService _employeeServiceUnauthorized;
    private readonly EmployeeService _employeeServiceJourney;
    private readonly AuthorizeIdentityMock _authorizedIdentity;
    private readonly AuthorizeIdentityMock _unauthorizedIdentity;
    private readonly AuthorizeIdentityMock _journeyIdentity;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<SmtpClient> _smtpClientMock;

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
        _authServiceMock = new Mock<IAuthService>();
        _authorizedIdentity = new AuthorizeIdentityMock("test@gmail.com", "test", "Admin", 1);
        _unauthorizedIdentity = new AuthorizeIdentityMock("test@gmail.com", "test", "Employee", 1);
        _journeyIdentity = new AuthorizeIdentityMock("test@retrorabbit.co.za", "test", "Journey", 1);
        _errorLoggingServiceMock = new Mock<IErrorLoggingService>();
        _emailHelper = new Mock<IEmailHelper>();
        _emailService = new Mock<IEmailService>();
        _mapperMock = new Mock<IMapper>();
        _smtpClientMock = new Mock<SmtpClient>();

        _roleServiceMock = new Mock<IRoleService>();

        _employeeService = new EmployeeService(_employeeTypeServiceMock.Object, _dbMock.Object,
            _employeeAddressServiceMock.Object, _roleServiceMock.Object, _authServiceMock.Object, _errorLoggingServiceMock.Object,
            _emailService.Object, _authorizedIdentity, _mapperMock.Object);

        _employeeServiceUnauthorized = new EmployeeService(_employeeTypeServiceMock.Object, _dbMock.Object,
           _employeeAddressServiceMock.Object, _roleServiceMock.Object, _authServiceMock.Object, _errorLoggingServiceMock.Object,
           _emailService.Object, _unauthorizedIdentity, _mapperMock.Object);

        _employeeServiceJourney = new EmployeeService(_employeeTypeServiceMock.Object, _dbMock.Object,
           _employeeAddressServiceMock.Object, _roleServiceMock.Object, _authServiceMock.Object, _errorLoggingServiceMock.Object,
           _emailService.Object, _journeyIdentity, _mapperMock.Object);
    }

    [Theory]
    [InlineData("Pass", false, false)]
    [InlineData("Unauthorized Access", false, false)]
    [InlineData("User already created", true, false)]
    [InlineData("Email Is Already in Use", false, true)]
    [InlineData("Employee Type Missing", false, false)]
    public async Task SaveEmployeeTests(string testCase, bool anySequenceOne, bool anySequenceTwo)
    {
        _dbMock.SetupSequence(e => e.Employee.Any(It.IsAny<Expression<Func<Employee, bool>>>()))
        .ReturnsAsync(anySequenceOne)
        .ReturnsAsync(anySequenceTwo);

        _employeeTypeServiceMock.Setup(r => r.GetEmployeeTypeByName(EmployeeTypeTestData.DesignerType.Name!))
                  .ReturnsAsync(EmployeeTypeTestData.DesignerType.ToDto());

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

        if (testCase == "Pass")
        {
            _mapperMock.Setup(m => m.Map<Employee>(EmployeeTestData.EmployeeOne.ToDto())).Returns(EmployeeTestData.EmployeeOne);
            _mapperMock.Setup(m => m.Map<EmployeeDto>(EmployeeTestData.EmployeeTwo)).Returns(EmployeeTestData.EmployeeOne.ToDto());


            _roleServiceMock.Setup(r => r.GetRole("Employee")).ReturnsAsync(EmployeeRoleTestData.RoleDtoEmployee.ToDto());

            _mapperMock.Setup(m => m.Map<EmployeeDto>(EmployeeTestData.EmployeeOne)).Returns(EmployeeTestData.EmployeeOne.ToDto());

            var result = await _employeeService.CreateEmployee(EmployeeTestData.EmployeeTwo.ToDto());
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
    }

    [Theory]
    [InlineData("Pass")]
    [InlineData("Unauthorized Access")]
    [InlineData("This model does not exist")]
    [InlineData("Deleting the currently logged-in user is not permitted")]
    public async Task DeleteEmployeeTest(string testCase)
    {
        _employeeTypeServiceMock.Setup(r => r.GetEmployeeTypeByName(EmployeeTypeTestData.DeveloperType.Name!))
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
            _mapperMock.Setup(m => m.Map<EmployeeDto>(EmployeeTestData.EmployeeTwo)).Returns(EmployeeTestData.EmployeeTwo.ToDto());

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
        _dbMock.Verify(u => u.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()), Times.Exactly(3));

        _mapperMock.Setup(m => m.Map<EmployeeDto>(EmployeeTestData.EmployeeTwo)).Returns(EmployeeTestData.EmployeeOne.ToDto());

        var failResultUnauthorized = await Assert.ThrowsAsync<CustomException>(() => _employeeServiceUnauthorized.GetAll(EmployeeTestData.EmployeeOne.Email!));
        Assert.Equal("Unauthorized Access", failResultUnauthorized.Message);
    }

    [Theory]
    [InlineData("Pass")]
    [InlineData("User email not found")]
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
            var result = _employeeService.GetEmployeeByEmail("dm@retrorabbit.co.za");
            Assert.NotNull(result);
        }

        if (testCase == "User email not found")
        {
            _dbMock.Setup(e => e.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
               .Returns(new List<Employee>().ToMockIQueryable());

            var failResult = await Assert.ThrowsAsync<CustomException>(() => _employeeService.GetEmployeeByEmail(EmployeeTestData.EmployeeOne.Email!));
            Assert.Equal(testCase, failResult.Message);
        }
    }

    [Fact]
    public async Task GetEmployeeByIdTests()
    {
        var employees = new List<Employee> { EmployeeTestData.EmployeeOne, EmployeeTestData.EmployeeTwo };

        _dbMock.Setup(e => e.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
               .Returns(employees.ToMockIQueryable());

        _mapperMock.Setup(m => m.Map<EmployeeDto>(EmployeeTestData.EmployeeTwo)).Returns(EmployeeTestData.EmployeeTwo.ToDto());

        var result = await _employeeService.GetEmployeeById(EmployeeTestData.EmployeeOne.Id);

        Assert.NotNull(result);
        Assert.Equivalent(EmployeeTestData.EmployeeOne.ToDto(), result);
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
    [InlineData("Model already exists and not being updated.")]
    [InlineData("Pass")]
    public async Task CheckDuplicateIdNumberTests(string testCase)
    {
        var employeeList = new List<Employee>
        {
            EmployeeTestData.EmployeeOne
        };
        EmployeeService employeeService;

        if (testCase == "Unauthorized Access")
        {
            var unauthorizedIdentity = new AuthorizeIdentityMock("test@gmail.com", "test", "Inactive", 1);
            employeeService = new EmployeeService(
                _employeeTypeServiceMock.Object,
                _dbMock.Object,
                _employeeAddressServiceMock.Object,
                _roleServiceMock.Object,
                _authServiceMock.Object,
                _errorLoggingServiceMock.Object,
                _emailService.Object,
                unauthorizedIdentity,
                _mapperMock.Object
            );

            var exception = await Assert.ThrowsAsync<CustomException>(() =>
                employeeService.CheckDuplicateIdNumber(EmployeeTestData.EmployeeOne.IdNumber!, EmployeeTestData.EmployeeOne.Id));

            Assert.Equal("Unauthorized Access", exception.Message);
        }
        else if (testCase == "Model already exists and not being updated.")
        {
            _dbMock.Setup(e => e.Employee.Any(It.IsAny<Expression<Func<Employee, bool>>>()))
                .ReturnsAsync(true);

            employeeService = new EmployeeService(
                _employeeTypeServiceMock.Object,
                _dbMock.Object,
                _employeeAddressServiceMock.Object,
                _roleServiceMock.Object,
                _authServiceMock.Object,
                _errorLoggingServiceMock.Object,
                _emailService.Object,
                _authorizedIdentity,
                _mapperMock.Object
            );

            var exception = await Assert.ThrowsAsync<CustomException>(() =>
                employeeService.CheckDuplicateIdNumber(EmployeeTestData.EmployeeOne.IdNumber!, EmployeeTestData.EmployeeOne.Id, false));

            Assert.Equal("Model already exists and not being updated.", exception.Message);
        }
        else if (testCase == "Pass")
        {
            _dbMock.Setup(e => e.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
                .Returns(employeeList.ToMockIQueryable());
            _dbMock.SetupSequence(e => e.Employee.Any(It.IsAny<Expression<Func<Employee, bool>>>()))
                .ReturnsAsync(false)
                .ReturnsAsync(true);

            employeeService = new EmployeeService(
                _employeeTypeServiceMock.Object,
                _dbMock.Object,
                _employeeAddressServiceMock.Object,
                _roleServiceMock.Object,
                _authServiceMock.Object,
                _errorLoggingServiceMock.Object,
                _emailService.Object,
                _authorizedIdentity,
                _mapperMock.Object
            );
            var result = await employeeService.CheckDuplicateIdNumber(EmployeeTestData.EmployeeOne.IdNumber!, EmployeeTestData.EmployeeOne.Id);
            Assert.True(result);
        }
    }

    [Fact]
    public async Task UpdateEmployee_UnauthorizedAccess_ThrowsCustomException()
    {
        var employeeDto = new EmployeeProfileDto { Id = 2 };

        var result = await Assert.ThrowsAsync<CustomException>(() => _employeeServiceUnauthorized.UpdateEmployee(employeeDto));

        Assert.Equal("Unauthorized Access", result.Message);
    }

    [Fact]
    public async Task UpdateEmployee_NullTeamLeadPeopleChampionClientAllocated_HandlesNulls()
    {
        var employeeDto = new EmployeeProfileDto { Id = 1, TeamLeadName = null, PeopleChampionName = null, ClientAllocatedName = null };
        var employee = EmployeeTestData.EmployeeOne;
        employee.TeamLead = null;
        employee.PeopleChampion = null;
        employee.ClientAllocated = null;

        _dbMock.Setup(db => db.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
               .Returns(new List<Employee> { employee }.AsQueryable().ToMockIQueryable());

        _mapperMock.Setup(m => m.Map<EmployeeDto>(It.IsAny<Employee>()))
                   .Returns(EmployeeTestData.EmployeeOne.ToDto());

        _dbMock.Setup(db => db.Employee.Update(It.IsAny<Employee>()))
               .ReturnsAsync(employee);

        var result = await _employeeService.UpdateEmployee(employeeDto);

        Assert.NotNull(result);
        Assert.Null(result.TeamLead);
        Assert.Null(result.PeopleChampion);
        Assert.Null(result.ClientAllocated);
    }

    [Fact]
    public async Task UpdateEmployee_EmployeeNotFound_ThrowsCustomException()
    {
        var employeeDto = new EmployeeProfileDto { Id = 1 };

        _dbMock.Setup(db => db.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
               .Returns(new List<Employee>().AsQueryable().ToMockIQueryable());

        var result = await Assert.ThrowsAsync<CustomException>(() => _employeeService.UpdateEmployee(employeeDto));

        Assert.Equal("User not found", result.Message);
    }

    [Fact]
    public async Task UpdateEmployee_WithTeamLeadPeopleChampionClientAllocated_UpdatesFields()
    {
        var employeeDto = new EmployeeProfileDto
        {
            Id = 1,
            TeamLeadName = "John Doe",
            PeopleChampionName = "Jane Smith",
            ClientAllocatedName = "Acme Corp"
        };

        var employee = EmployeeTestData.EmployeeOne;
        var teamLead = new Employee { Id = 2, Name = "John", Surname = "Doe" };
        var peopleChampion = new Employee { Id = 3, Name = "Jane", Surname = "Smith" };
        var client = new Client { Id = 4, Name = "Acme Corp" };
        employee.TeamLead = 2;
        employee.PeopleChampion = 3;
        employee.ClientAllocated = 4;

        _dbMock.Setup(db => db.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
               .Returns(new List<Employee> { teamLead, peopleChampion }.AsQueryable().ToMockIQueryable());

        _dbMock.Setup(db => db.Client.Get(It.IsAny<Expression<Func<Client, bool>>>()))
               .Returns(new List<Client> { client }.AsQueryable().ToMockIQueryable());

        _mapperMock.Setup(m => m.Map<EmployeeDto>(It.IsAny<Employee>()))
                   .Returns(EmployeeTestData.EmployeeOne.ToDto());

        _dbMock.Setup(db => db.Employee.Update(It.IsAny<Employee>()))
               .ReturnsAsync(employee);

        var result = await _employeeService.UpdateEmployee(employeeDto);

        Assert.NotNull(result);
        Assert.NotNull(result.TeamLead);
        Assert.NotNull(result.PeopleChampion);
        Assert.NotNull(result.ClientAllocated);
    }

    [Fact]
    public async Task GetEmployeeProfile_NullTeamLeadPeopleChampionClientAllocated_HandlesNulls()
    {
        var identifier = EmployeeTestData.EmployeeOne.Email;

        var employee = EmployeeTestData.EmployeeOne;
        employee.TeamLead = null;
        employee.PeopleChampion = null;
        employee.ClientAllocated = null;

        var employees = new List<Employee>
        {
            employee,
            EmployeeTestData.EmployeeTwo,
            EmployeeTestData.EmployeeThree
        };

        _dbMock.Setup(e => e.Employee.Any(It.IsAny<Expression<Func<Employee, bool>>>()))
               .ReturnsAsync(true);

        _dbMock.Setup(s => s.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>())).Returns(employees.ToMockIQueryable());

        _mapperMock.Setup(m => m.Map<EmployeeProfileDto>(It.IsAny<EmployeeDto>())).Returns(new EmployeeProfileDto());

        var result = await _employeeService.GetEmployeeProfile(identifier);

        Assert.NotNull(result);
        Assert.Null(result.TeamLeadName);
        Assert.Null(result.PeopleChampionName);
        Assert.Null(result.ClientAllocatedName);
    }

    [Fact]
    public async Task GetEmployeeProfile_ValidEmail_ReturnsEmployeeProfile()
    {
        var identifier = EmployeeTestData.EmployeeOne.Email;

        var employee = EmployeeTestData.EmployeeOne;
        employee.TeamLead = 2;
        employee.PeopleChampion = 3;
        employee.ClientAllocated = 4;

        var employees = new List<Employee>
        {
            employee,
            EmployeeTestData.EmployeeTwo,
            EmployeeTestData.EmployeeThree,
            EmployeeTestData.EmployeeFour
        };

        var client = new Client { Id = 4, Name = "ABC Corp" };

        _dbMock.Setup(e => e.Employee.Any(It.IsAny<Expression<Func<Employee, bool>>>()))
               .ReturnsAsync(true);

        _mapperMock.Setup(m => m.Map<EmployeeProfileDto>(It.IsAny<EmployeeDto>())).Returns(new EmployeeProfileDto());

        _dbMock.Setup(db => db.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>())).Returns(employees.ToMockIQueryable());
        _dbMock.Setup(db => db.Client.Get(It.IsAny<Expression<Func<Client, bool>>>())).Returns(new List<Client> { client }.ToMockIQueryable());

        var result = await _employeeService.GetEmployeeProfile(identifier);

        Assert.NotNull(result);
        Assert.NotNull(result.TeamLeadName);
        Assert.NotNull(result.PeopleChampionName);
        Assert.NotNull(result.ClientAllocatedName);
    }

    [Fact]
    public async Task GetEmployeeProfile_ValidEmployeeId_ReturnsEmployeeProfile()
    {
        var identifier = EmployeeTestData.EmployeeOne.Id.ToString();

        var employee = EmployeeTestData.EmployeeOne;
        employee.TeamLead = 2;
        employee.PeopleChampion = 3;
        employee.ClientAllocated = 4;

        var employees = new List<Employee>
        {
            employee,
            EmployeeTestData.EmployeeTwo,
            EmployeeTestData.EmployeeThree,
            EmployeeTestData.EmployeeFour
        };

        var client = new Client { Id = 4, Name = "ABC Corp" };

        _dbMock.Setup(e => e.Employee.Any(It.IsAny<Expression<Func<Employee, bool>>>()))
               .ReturnsAsync(true);

        _mapperMock.Setup(m => m.Map<EmployeeProfileDto>(It.IsAny<EmployeeDto>())).Returns(new EmployeeProfileDto());

        _dbMock.Setup(db => db.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>())).Returns(employees.ToMockIQueryable());
        _dbMock.Setup(db => db.Client.Get(It.IsAny<Expression<Func<Client, bool>>>())).Returns(new List<Client> { client }.ToMockIQueryable());

        var result = await _employeeService.GetEmployeeProfile(identifier);

        Assert.NotNull(result);
        Assert.NotNull(result.TeamLeadName);
        Assert.NotNull(result.PeopleChampionName);
        Assert.NotNull(result.ClientAllocatedName);
    }

    [Fact]
    public async Task GetEmployeeProfile_InvalidEmail_ThrowsCustomException()
    {
        var identifier = "invalid@example.com";

        _dbMock.Setup(e => e.Employee.Any(It.IsAny<Expression<Func<Employee, bool>>>()))
               .ReturnsAsync(false);

        await Assert.ThrowsAsync<CustomException>(() => _employeeService.GetEmployeeProfile(identifier));
    }

    [Fact]
    public async Task GetAllEmployeeProfiles_TeamLeadPeopleChampionClientAllocated_ReturnsEmployeeProfiles()
    {
        var employee = EmployeeTestData.EmployeeOne;
        employee.TeamLead = 2;
        employee.PeopleChampion = 3;
        employee.ClientAllocated = 4;

        var employees = new List<Employee>
        {
            employee,
            EmployeeTestData.EmployeeTwo,
            EmployeeTestData.EmployeeThree,
            EmployeeTestData.EmployeeFour
        };

        var client = new Client { Id = 4, Name = "ABC Corp" };

        _dbMock.Setup(u => u.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
               .Returns(employees.ToMockIQueryable());

        _dbMock.Setup(e => e.Employee.Any(It.IsAny<Expression<Func<Employee, bool>>>()))
               .ReturnsAsync(true);

        _mapperMock.Setup(m => m.Map<EmployeeProfileDto>(It.IsAny<EmployeeDto>())).Returns(new EmployeeProfileDto());

        _dbMock.Setup(db => db.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>())).Returns(employees.ToMockIQueryable());
        _dbMock.Setup(db => db.Client.Get(It.IsAny<Expression<Func<Client, bool>>>())).Returns(new List<Client> { client }.ToMockIQueryable());

        var result = await _employeeService.GetAllEmployeeProfiles();

        Assert.NotNull(result);
        Assert.NotNull(result[0]);
        Assert.NotNull(result[0].TeamLeadName);
        Assert.NotNull(result[0].PeopleChampionName);
        Assert.NotNull(result[0].ClientAllocatedName);
    }

    [Fact]
    public async Task CheckUserAuthentication_InvalidEmail_ThrowsCustomException()
    {
        var email = "invalid@example.com";
        var id = "0";
        var role = "";

        _dbMock.Setup(e => e.Employee.Any(It.IsAny<Expression<Func<Employee, bool>>>()))
               .ReturnsAsync(false);

        await Assert.ThrowsAsync<CustomException>(() => _employeeService.CheckUserAuthentication(email, id, role));
    }

    [Fact]
    public async Task CheckUserAuthentication_ValidEmail_RoleNotNull()
    {
        var email = EmployeeTestData.EmployeeOne.Email;
        var id = EmployeeTestData.EmployeeOne.Id.ToString();
        var role = "Employee";

        var employee = EmployeeTestData.EmployeeOne;
        employee.TeamLead = 2;
        employee.PeopleChampion = 3;
        employee.ClientAllocated = 4;

        var employees = new List<Employee>
        {
            employee,
            EmployeeTestData.EmployeeTwo,
            EmployeeTestData.EmployeeThree,
            EmployeeTestData.EmployeeFour
        };

        var client = new Client { Id = 4, Name = "ABC Corp" };

        _dbMock.Setup(e => e.Employee.Any(It.IsAny<Expression<Func<Employee, bool>>>()))
               .ReturnsAsync(true);

        _mapperMock.Setup(m => m.Map<EmployeeProfileDto>(It.IsAny<EmployeeDto>())).Returns(new EmployeeProfileDto());

        _dbMock.Setup(db => db.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>())).Returns(employees.ToMockIQueryable());
        _dbMock.Setup(db => db.Client.Get(It.IsAny<Expression<Func<Client, bool>>>())).Returns(new List<Client> { client }.ToMockIQueryable());

        var result = await _employeeService.CheckUserAuthentication(email, id, role);

        Assert.NotNull(result);
        Assert.NotNull(result.TeamLeadName);
        Assert.NotNull(result.PeopleChampionName);
        Assert.NotNull(result.ClientAllocatedName);
    }

    [Fact]
    public async Task CheckUserAuthentication_ValidEmail_RoleIsNull()
    {
        var email = EmployeeTestData.EmployeeOne.Email;
        var id = EmployeeTestData.EmployeeOne.Id.ToString();
        var role = string.Empty;

        var employee = EmployeeTestData.EmployeeOne;
        employee.TeamLead = 2;
        employee.PeopleChampion = 3;
        employee.ClientAllocated = 4;

        var employees = new List<Employee>
        {
            employee,
            EmployeeTestData.EmployeeTwo,
            EmployeeTestData.EmployeeThree,
            EmployeeTestData.EmployeeFour
        };

        var client = new Client { Id = 4, Name = "ABC Corp" };

        _dbMock.Setup(e => e.Employee.Any(It.IsAny<Expression<Func<Employee, bool>>>()))
               .ReturnsAsync(true);

        _mapperMock.Setup(m => m.Map<EmployeeProfileDto>(It.IsAny<EmployeeDto>())).Returns(new EmployeeProfileDto());

        _dbMock.Setup(db => db.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>())).Returns(employees.ToMockIQueryable());
        _dbMock.Setup(db => db.Client.Get(It.IsAny<Expression<Func<Client, bool>>>())).Returns(new List<Client> { client }.ToMockIQueryable());

        var result = await _employeeService.CheckUserAuthentication(email, id, role);

        Assert.NotNull(result);
        Assert.NotNull(result.TeamLeadName);
        Assert.NotNull(result.PeopleChampionName);
        Assert.NotNull(result.ClientAllocatedName);
    }
}
