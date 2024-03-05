using System.Linq.Expressions;
using HRIS.Models;
using HRIS.Models.Enums;
using HRIS.Services.Interfaces;
using HRIS.Services.Services;
using Microsoft.Extensions.Configuration;
using MockQueryable.Moq;
using Moq;
using RR.UnitOfWork;
using RR.UnitOfWork.Entities.HRIS;
using Xunit;

namespace HRIS.Services.Tests.Services;

public class AuthServiceUnitTest
{
    private readonly AuthService _authService;
    private readonly Mock<IAuthService> _authServiceMock;
    private readonly Mock<IConfiguration> _configuration;
    //private readonly Mock<IEmployeeAddressService>? _employeeAddressService;
    private readonly Mock<IEmployeeRoleService> _employeeRoleServiceMock;
    private readonly Mock<IEmployeeService> _employeeService;
    private readonly Mock<IEmployeeTypeService> _employeeTypeServiceMock;
    private readonly Mock<IRoleAccessLinkService> _roleAccessLinkService;
    //private readonly Mock<IRoleService> _roleService;
    private readonly Mock<IUnitOfWork> _unitOfWork;
    private EmployeeAddressDto? employeeAddressDto;
    private readonly EmployeeType? employeeType1;
    private readonly EmployeeType? employeeType2;
    private readonly EmployeeTypeDto employeeTypeDto1;
    private readonly EmployeeTypeDto? employeeTypeDto2;
    private readonly ErrorLoggingService errorLoggingService;

    private EmployeeTypeService? employeeTypeService;
    
    public AuthServiceUnitTest()
    {
        _unitOfWork = new Mock<IUnitOfWork>();
        _roleAccessLinkService = new Mock<IRoleAccessLinkService>();
        _employeeService = new Mock<IEmployeeService>();
        _configuration = new Mock<IConfiguration>();
        _employeeTypeServiceMock = new Mock<IEmployeeTypeService>();
        _employeeRoleServiceMock = new Mock<IEmployeeRoleService>();
        _authServiceMock = new Mock<IAuthService>();
        errorLoggingService = new ErrorLoggingService(_unitOfWork.Object);

        employeeTypeService = new EmployeeTypeService(_unitOfWork.Object);
        employeeTypeDto1 = new EmployeeTypeDto{ Id = 3, Name = "Developer" };
        employeeTypeDto2 = new EmployeeTypeDto{ Id = 7, Name = "People Champion" };
        employeeType1 = new EmployeeType(employeeTypeDto1);
        employeeType2 = new EmployeeType(employeeTypeDto2);

        _employeeTypeServiceMock.Setup(r => r.GetEmployeeType(employeeType1.Name!))
                                .Returns(Task.FromResult(employeeTypeDto1));
        _employeeTypeServiceMock.Setup(r => r.GetEmployeeType(employeeType2.Name!))
                                .Returns(Task.FromResult(employeeTypeDto2));

        employeeAddressDto =
            new EmployeeAddressDto{ Id = 1, UnitNumber = "2", ComplexName = "Complex", StreetNumber = "2", SuburbOrDistrict = "Suburb/District", City = "City", Country = "Country", Province = "Province", PostalCode = "1620" };
        _authService = new AuthService(_configuration.Object, _employeeService.Object, _roleAccessLinkService.Object, errorLoggingService);
    }

    [Fact]
    public async Task CheckUserExist()
    {
        var employeeTypeDto = new EmployeeTypeDto{ Id = 1, Name = "Developer" };
        var employeeType = new EmployeeType(employeeTypeDto);
        employeeAddressDto =
            new EmployeeAddressDto{ Id = 1, UnitNumber = "2", ComplexName = "Complex", StreetNumber = "2", SuburbOrDistrict = "Suburb/District", City = "City", Country = "Country", Province = "Province", PostalCode = "1620" };

        var employeeDto = new EmployeeDto
        {
            Id = 1,
            EmployeeNumber = "001",
            TaxNumber = "34434434",
            EngagementDate = DateTime.Now,
            TerminationDate = DateTime.Now,
            PeopleChampion = null,
            Disability = false,
            DisabilityNotes = "None",
            Level = 4,
            EmployeeType = new EmployeeTypeDto { Id = 1, Name = "Developer" },
            Notes = "Notes",
            LeaveInterval = 1,
            SalaryDays = 28,
            PayRate = 128,
            Salary = 100000,
            Name = "Dotty",
            Initials = "D",
            Surname = "Missile",
            DateOfBirth = DateTime.Now,
            CountryOfBirth = "South Africa",
            Nationality = "South African",
            IdNumber = "5522522655",
            PassportNumber = " ",
            PassportExpirationDate = DateTime.Now,
            PassportCountryIssue = null,
            Race = Race.Black,
            Gender = Gender.Female,
            Photo = null,
            Email = "dm@retrorabbit.co.za",
            PersonalEmail = "test@gmail.com",
            CellphoneNo = "0123456789",
            ClientAllocated = null,
            TeamLead = null,
            PhysicalAddress = employeeAddressDto,
            PostalAddress = employeeAddressDto,
            HouseNo = null,
            EmergencyContactName = null,
            EmergencyContactNo = null
        };

        var employeeDtoList = new List<Employee>
        {
            new(employeeDto, employeeTypeDto1)
        };

        _unitOfWork.Setup(r => r.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
                   .Returns(employeeDtoList.AsQueryable().BuildMock());
        _employeeService.Setup(x => x.CheckUserExist("dm@retrorabbit.co.za")).ReturnsAsync(true);

        var result = await _authService.CheckUserExist("dm@retrorabbit.co.za");

        Assert.IsType<bool>(result);
        Assert.True(result);
    }

    [Fact]
    public async Task Login()
    {
        var employeeTypeDto = new EmployeeTypeDto{ Id = 1, Name = "Developer" };
        var employeeType = new EmployeeType(employeeTypeDto);
        employeeAddressDto =
            new EmployeeAddressDto{ Id = 1, UnitNumber = "2", ComplexName = "Complex", StreetNumber = "2", SuburbOrDistrict = "Suburb/District", City = "City", Country = "Country", Province = "Province", PostalCode = "1620" };
        var roleDto = new RoleDto { Id = 1, Description = "Developer" };

        var employeeDto = new EmployeeDto
        {
            Id = 1,
            EmployeeNumber = "001",
            TaxNumber = "34434434",
            EngagementDate = new DateTime(),
            TerminationDate = new DateTime(),
            PeopleChampion = null,
            Disability = false,
            DisabilityNotes = "None",
            Level = 4,
            EmployeeType = new EmployeeTypeDto { Id = 1, Name = "Developer" },
            Notes = "Notes",
            LeaveInterval = 1,
            SalaryDays = 28,
            PayRate = 128,
            Salary = 100000,
            Name = "Dotty",
            Initials = "D",
            Surname = "Missile",
            DateOfBirth = new DateTime(),
            CountryOfBirth = "South Africa",
            Nationality = "South African",
            IdNumber = "5522522655",
            PassportNumber = " ",
            PassportExpirationDate = new DateTime(),
            PassportCountryIssue = "South Africa",
            Race = Race.Black,
            Gender = Gender.Female,
            Email = "dm@retrorabbit.co.za",
            PersonalEmail = "test@gmail.com",
            CellphoneNo = "0123456789",
            PhysicalAddress = employeeAddressDto,
            PostalAddress = employeeAddressDto
        };

        var email = "dm@retrorabbit.co.za";

        var employeeDtoList = new List<Employee>
        {
            new(employeeDto, employeeTypeDto1)
        };

        var roles = new Dictionary<string, List<string>>
        {
            {
                "Admin", new List<string> { "Create", "Read", "Update", "Delete" }
            }
        };

        _unitOfWork.Setup(x => x.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
                   .Returns(employeeDtoList.AsQueryable().BuildMock());
        _employeeService.Setup(x => x.GetEmployee(email)).ReturnsAsync(employeeDto);
        _authServiceMock.Setup(x => x.GetUserRoles(email)).ReturnsAsync(roles);
        _roleAccessLinkService.Setup(x => x.GetRoleByEmployee(email)).ReturnsAsync(roles);

        _configuration.Setup(x => x["Auth:Key"]).Returns("34bDExFk0WHe$GaXn%0xcZY3^fT^#J05");
        _configuration.Setup(x => x["Auth:Expires"]).Returns("30");
        _configuration.Setup(x => x["Auth:Issuer"]).Returns("YourIssuer");
        _configuration.Setup(x => x["Auth:Audience"]).Returns("YourAudience");

        var token = await _authService.Login(email);

        Assert.NotNull(token);
    }

    [Fact]
    public async Task RegisterEmployee()
    {
        var employeeTypeDto = new EmployeeTypeDto{ Id = 1, Name = "Developer" };
        var employeeType = new EmployeeType(employeeTypeDto);
        employeeAddressDto =
            new EmployeeAddressDto{ Id = 1, UnitNumber = "2", ComplexName = "Complex", StreetNumber = "2", SuburbOrDistrict = "Suburb/District", City = "City", Country = "Country", Province = "Province", PostalCode = "1620" };

        var employeeDto = new EmployeeDto
        {
            Id = 1,
            EmployeeNumber = "001",
            TaxNumber = "34434434",
            EngagementDate = new DateTime(),
            TerminationDate = new DateTime(),
            PeopleChampion = null,
            Disability = false,
            DisabilityNotes = "None",
            Level = 4,
            EmployeeType = new EmployeeTypeDto { Id = 1, Name = "Developer" },
            Notes = "Notes",
            LeaveInterval = 1,
            SalaryDays = 28,
            PayRate = 128,
            Salary = 100000,
            Name = "Dotty",
            Initials = "D",
            Surname = "Missile",
            DateOfBirth = new DateTime(),
            CountryOfBirth = "South Africa",
            Nationality = "South African",
            IdNumber = "5522522655",
            PassportNumber = " ",
            PassportExpirationDate = new DateTime(),
            PassportCountryIssue = "South Africa",
            Race = Race.Black,
            Gender = Gender.Female,
            Email = "dm@retrorabbit.co.za",
            PersonalEmail = "test@gmail.com",
            CellphoneNo = "0123456789",
            PhysicalAddress = employeeAddressDto,
            PostalAddress = employeeAddressDto
        };

        var email = "dm@retrorabbit.co.za";

        var employeeDtoList = new List<Employee>
        {
            new(employeeDto, employeeTypeDto1)
        };

        var roles = new Dictionary<string, List<string>>
        {
            {
                "Admin", new List<string> { "Create", "Read", "Update", "Delete" }
            }
        };

        _unitOfWork.Setup(r => r.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
                   .Returns(employeeDtoList.AsQueryable().BuildMock());
        _employeeService.Setup(x => x.SaveEmployee(employeeDto)).ReturnsAsync(employeeDto);
        _roleAccessLinkService.Setup(x => x.GetRoleByEmployee(email)).ReturnsAsync(roles);

        _configuration.Setup(c => c["Auth:Key"]).Returns("34bDExFk0WHe$GaXn%0xcZY3^fT^#J05");
        _configuration.Setup(c => c["Auth:Expires"]).Returns("30");
        _configuration.Setup(c => c["Auth:Issuer"]).Returns("YourIssuer");
        _configuration.Setup(c => c["Auth:Audience"]).Returns("YourAudience");

        var token = await _authService.RegisterEmployee(employeeDto);

        Assert.NotNull(token);
    }

    [Fact]
    public async Task GetUserRoles()
    {
        var email = "dm@retrorabbit.co.za";
        var expectedRoles = new Dictionary<string, List<string>>
            { { "Admin", new List<string> { "Create", "Read", "Update", "Delete" } } };
        _roleAccessLinkService.Setup(x => x.GetRoleByEmployee(email)).ReturnsAsync(expectedRoles);

        var roles = await _authService.GetUserRoles(email);

        Assert.Equal(expectedRoles, roles);
    }
}
