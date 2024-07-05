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

    private readonly EmployeeRole _employeeRoleDto = new()
    {
        Id = 0,
        Employee = EmployeeTestData.EmployeeOne,
        Role = EmployeeRoleTestData.RoleDtoEmployee
    };

    private readonly EmployeeService _employeeService;
    private readonly DashboardService _dashboardService;

    public EmployeeServiceUnitTests()
    {
        _dbMock = new Mock<IUnitOfWork>();
        _employeeTypeServiceMock = new Mock<IEmployeeTypeService>();
        _employeeAddressServiceMock = new Mock<IEmployeeAddressService>();
        _errorLoggingServiceMock = new Mock<IErrorLoggingService>();
        Mock<IEmailService> emailService = new();
        _roleServiceMock = new Mock<IRoleService>();
        _employeeService = new EmployeeService(_employeeTypeServiceMock.Object, _dbMock.Object,
            _employeeAddressServiceMock.Object, _roleServiceMock.Object, _errorLoggingServiceMock.Object,
            emailService.Object);
    }

    [Fact]
    public async Task SaveEmployeeFailTest1()
    {
        _dbMock.Setup(r => r.Employee.Any(It.IsAny<Expression<Func<Employee, bool>>>())).ReturnsAsync(true);
        _errorLoggingServiceMock.Setup(r => r.LogException(It.IsAny<Exception>())).Throws(new Exception());

        await Assert.ThrowsAsync<Exception>(() => _employeeService.SaveEmployee(EmployeeTestData.EmployeeOne.ToDto()));
    }

    [Fact]
    public async Task SaveEmployeeFailTest2()
    {
        _dbMock.Setup(r => r.Employee.Any(It.IsAny<Expression<Func<Employee, bool>>>())).ReturnsAsync(false);

        _employeeTypeServiceMock.Setup(r => r.GetEmployeeType(It.IsAny<string>())).Throws(new Exception());

        _employeeTypeServiceMock.Setup(r => r.SaveEmployeeType(It.IsAny<EmployeeTypeDto>())).ReturnsAsync(EmployeeTypeTestData.DeveloperType.ToDto());

        _employeeAddressServiceMock.SetupSequence(r => r.CheckIfExists(It.IsAny<EmployeeAddressDto>()))
            .ReturnsAsync(false)
            .ReturnsAsync(false);

        _employeeAddressServiceMock.SetupSequence(r => r.Save(It.IsAny<EmployeeAddressDto>()))
            .ReturnsAsync(EmployeeAddressTestData.EmployeeAddressOne.ToDto())
            .ReturnsAsync(EmployeeAddressTestData.EmployeeAddressOne.ToDto());

        _dbMock.Setup(r => r.Employee.Add(It.IsAny<Employee>())).ReturnsAsync(EmployeeTestData.EmployeeOne);
        _dbMock.Setup(r => r.EmployeeRole.Add(It.IsAny<EmployeeRole>())).ReturnsAsync(_employeeRoleDto);

        _roleServiceMock.Setup(r => r.GetRole("Employee")).ReturnsAsync(EmployeeRoleTestData.RoleDtoEmployee.ToDto());
        var result = await _employeeService.SaveEmployee(EmployeeTestData.EmployeeOne.ToDto());

        Assert.Equivalent(EmployeeTestData.EmployeeOne.ToDto(), result);

    }

    [Fact]
    public async Task SaveEmployeeFailTest3()
    {
        _dbMock.Setup(r => r.Employee.Any(It.IsAny<Expression<Func<Employee, bool>>>()))
               .ReturnsAsync(false);

        _employeeTypeServiceMock.Setup(r => r.GetEmployeeType(It.IsAny<string>()))
            .Throws(new Exception());

        _employeeTypeServiceMock.Setup(r => r.SaveEmployeeType(It.IsAny<EmployeeTypeDto>()))
                               .ReturnsAsync(EmployeeTypeTestData.DeveloperType.ToDto());

        _employeeAddressServiceMock.SetupSequence(r => r.CheckIfExists(EmployeeAddressTestData.EmployeeAddressOne.ToDto()))
                                  .ReturnsAsync(true)
                                  .ReturnsAsync(true);

        _employeeAddressServiceMock.Setup(x => x.Get(It.IsAny<EmployeeAddressDto>()))
                                  .ReturnsAsync(EmployeeAddressTestData.EmployeeAddressOne.ToDto());

        _employeeAddressServiceMock.Setup(r => r.Save(It.IsAny<EmployeeAddressDto>()))
                                  .ReturnsAsync(EmployeeAddressTestData.EmployeeAddressOne.ToDto());

        _dbMock.Setup(r => r.Employee.Add(It.IsAny<Employee>())).ReturnsAsync(EmployeeTestData.EmployeeOne);
        _dbMock.Setup(r => r.EmployeeRole.Add(It.IsAny<EmployeeRole>())).ReturnsAsync(_employeeRoleDto);

        _roleServiceMock.Setup(r => r.GetRole(It.IsAny<string>())).ReturnsAsync(EmployeeRoleTestData.RoleDtoEmployee.ToDto());

        var result = await _employeeService.SaveEmployee(EmployeeTestData.EmployeeOne.ToDto());

        Assert.Equivalent(EmployeeTestData.EmployeeOne.ToDto(), result);
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

        _employeeAddressServiceMock.SetupSequence(r => r.CheckIfExists(It.IsAny<EmployeeAddressDto>()))
                                  .ReturnsAsync(false)
                                  .ReturnsAsync(true);

        _employeeAddressServiceMock.Setup(r => r.Save(It.IsAny<EmployeeAddressDto>()))
                                  .ReturnsAsync(EmployeeAddressTestData.EmployeeAddressOne.ToDto());

        _employeeAddressServiceMock.Setup(r => r.Get(It.IsAny<EmployeeAddressDto>()))
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
    public async Task UpdateEmployeeTestOwnProfile()
    {
        _employeeTypeServiceMock.Setup(r => r.GetEmployeeType(EmployeeTypeTestData.DeveloperType.Name))
                               .ReturnsAsync(EmployeeTypeTestData.DeveloperType.ToDto());

        _dbMock.Setup(r => r.Employee.Update(It.IsAny<Employee>()))
               .ReturnsAsync(EmployeeTestData.EmployeeOne);

        var result =
            await _employeeService.UpdateEmployee(EmployeeTestData.EmployeeOne.ToDto(), EmployeeTestData.EmployeeOne.Email!);

        Assert.Equivalent(EmployeeTestData.EmployeeOne.ToDto(), result);
    }

    [Fact]
    public async Task UpdateEmployeeTestAdminPass()
    {
        var emp = EmployeeTestData.EmployeeTwo;

        var roleDto = new Role { Id = 2, Description = "Admin" };
        var empRole = new EmployeeRole { Id = 1, Employee = EmployeeTestData.EmployeeTwo, Role = roleDto };

        List<Employee> employees = new() { emp };
        List<EmployeeRole> empRoles = new() { empRole };
        List<Role> roles = new() { roleDto };

        _employeeTypeServiceMock.Setup(r => r.GetEmployeeType(EmployeeTypeTestData.DeveloperType.Name)).ReturnsAsync(EmployeeTypeTestData.DeveloperType.ToDto());

        _dbMock.Setup(r => r.Employee.Update(It.IsAny<Employee>())).ReturnsAsync(EmployeeTestData.EmployeeOne);
        _dbMock.Setup(r => r.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>())).Returns(employees.ToMockIQueryable());
        _dbMock.Setup(r => r.Employee.Any(It.IsAny<Expression<Func<Employee, bool>>>())).ReturnsAsync(true);
        _dbMock.Setup(r => r.EmployeeRole.Get(It.IsAny<Expression<Func<EmployeeRole, bool>>>())).Returns(empRoles.ToMockIQueryable());
        _dbMock.Setup(r => r.Role.Get(It.IsAny<Expression<Func<Role, bool>>>())).Returns(roles.ToMockIQueryable());

        var result = await _employeeService.UpdateEmployee(EmployeeTestData.EmployeeOne.ToDto(), "admin@retrorabbit.co.za");

        Assert.NotNull(result);
        Assert.Equivalent(EmployeeTestData.EmployeeOne.ToDto(), result);
    }

    [Fact]
    public async Task UpdateEmployeeTestAdminFail()
    {
        var emp = EmployeeTestData.EmployeeTwo;
        var employees = new List<Employee> { emp };
        var empRole = new EmployeeRole { Id = 1, Employee = EmployeeTestData.EmployeeTwo, Role = EmployeeRoleTestData.RoleDtoEmployee };
        var empRoles = new List<EmployeeRole> { empRole };
        var roles = new List<Role> { EmployeeRoleTestData.RoleDtoEmployee };

        _employeeTypeServiceMock.Setup(r => r.GetEmployeeType(EmployeeTypeTestData.DeveloperType.Name))
                               .ReturnsAsync(EmployeeTypeTestData.DeveloperType.ToDto());

        _dbMock.Setup(r => r.Employee.Update(It.IsAny<Employee>()))
               .ReturnsAsync(EmployeeTestData.EmployeeOne);
        _dbMock.Setup(r => r.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
               .Returns(employees.ToMockIQueryable());
        _dbMock.Setup(r => r.Employee.Any(It.IsAny<Expression<Func<Employee, bool>>>())).ReturnsAsync(true);
        _dbMock.Setup(r => r.EmployeeRole.Get(It.IsAny<Expression<Func<EmployeeRole, bool>>>()))
               .Returns(empRoles.ToMockIQueryable());
        _dbMock.Setup(r => r.Role.Get(It.IsAny<Expression<Func<Role, bool>>>()))
               .Returns(roles.ToMockIQueryable());

        _errorLoggingServiceMock.Setup(r => r.LogException(It.IsAny<Exception>())).Throws(new Exception("Unauthorized action: You are not an Admin"));

        var exception = await Assert.ThrowsAsync<Exception>(
                                                    async () =>
                                                        await _employeeService
                                                            .UpdateEmployee(EmployeeTestData.EmployeeOne.ToDto(),
                                                             "unauthorized.email@retrorabbit.co.za"));

        Assert.Equal("Unauthorized action: You are not an Admin", exception.Message);
    }

    [Fact]
    public async Task UpdateEmployeeTestUserDoesNotExist()
    {
        var emp = EmployeeTestData.EmployeeTwo;
        var employees = new List<Employee> { emp };

        _employeeTypeServiceMock.Setup(r => r.GetEmployeeType(EmployeeTypeTestData.DeveloperType.Name))
                               .ReturnsAsync(EmployeeTypeTestData.DeveloperType.ToDto());
        _dbMock.Setup(r => r.Employee.Update(It.IsAny<Employee>()))
               .ReturnsAsync(EmployeeTestData.EmployeeOne);
        _dbMock.Setup(r => r.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
               .Returns(employees.ToMockIQueryable());
        _dbMock.Setup(r => r.Employee.Any(It.IsAny<Expression<Func<Employee, bool>>>())).ReturnsAsync(false);

        _errorLoggingServiceMock.Setup(r => r.LogException(It.IsAny<Exception>())).Throws(new Exception("User already exists"));

        var exception = await Assert.ThrowsAsync<Exception>(
                                                            async () =>
                                                                await _employeeService
                                                                    .UpdateEmployee(EmployeeTestData.EmployeeOne.ToDto(),
                                                                     "unauthorized.email@retrorabbit.co.za"));

        Assert.Equal("User already exists", exception.Message);
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

        _errorLoggingServiceMock.Setup(r => r.LogException(It.IsAny<Exception>())).Throws(new Exception());

        await Assert.ThrowsAsync<CustomException>(() => _employeeService.GetEmployeeById(2));
    }

    [Fact]
    public async Task GetCurrentMonthTotalReturnsExistingTotalTest()
    {

        var employeeList = new List<Employee>
        {
            EmployeeTestData.EmployeeOne
        };

        var employee = new List<Employee>
        {
            EmployeeTestData.EmployeeOne
        };

        _dbMock.Setup(u => u.Employee.GetAll(null)).ReturnsAsync(employeeList);

        _dbMock.Setup(e => e.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
               .Returns(employee.ToMockIQueryable());

        var monthlyEmployeeTotalDto = MonthlyEmployeeTotalTestData.MonthlyEmployeeTotal_CurrentYear_CurrentMonth;

        var montlhyEmployeeTotalList = new List<MonthlyEmployeeTotal>
        {
            monthlyEmployeeTotalDto
        };

        _dbMock.Setup(e => e.MonthlyEmployeeTotal.Get(It.IsAny<Expression<Func<MonthlyEmployeeTotal, bool>>>()))
               .Returns(montlhyEmployeeTotalList.ToMockIQueryable());

        var result = await _dashboardService.GetEmployeeCurrentMonthTotal();

        Assert.NotNull(result);
        Assert.Equal(monthlyEmployeeTotalDto.Month, result.Month);
    }

    [Fact]
    public async Task GetCurrentMonthTotalCreateNewTotalTest()
    {

        var employeeList = new List<Employee>
        {
            EmployeeTestData.EmployeeOne
        };

        var employee = new List<Employee>
        {
            EmployeeTestData.EmployeeOne
        };

        _dbMock.Setup(u => u.Employee.GetAll(null)).ReturnsAsync(employeeList);

        _dbMock.Setup(e => e.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
               .Returns(employee.ToMockIQueryable());

        var monthlyEmployeeTotalDto = MonthlyEmployeeTotalTestData.MonthlyEmployeeTotal_PreviuosMonth_CurrentYear;

        var montlhyEmployeeTotalList = new List<MonthlyEmployeeTotal>
        {
            monthlyEmployeeTotalDto
        };

        _dbMock.Setup(e => e.MonthlyEmployeeTotal.Get(It.IsAny<Expression<Func<MonthlyEmployeeTotal, bool>>>()))
               .Returns(montlhyEmployeeTotalList.ToMockIQueryable());

        _dbMock.Setup(u => u.MonthlyEmployeeTotal.Add(It.IsAny<MonthlyEmployeeTotal>()))
               .ReturnsAsync(monthlyEmployeeTotalDto);

        var result = await _dashboardService.GetEmployeeCurrentMonthTotal();

        _dbMock.Verify(u => u.MonthlyEmployeeTotal.Add(It.IsAny<MonthlyEmployeeTotal>()), Times.Once);

        Assert.NotNull(result);
        Assert.Equal(1, result.EmployeeTotal);
        Assert.Equal(1, result.DeveloperTotal);
    }


    [Fact]
    public async Task GetPreviousMonthTotalReturnsExistingTotalTest()
    {
        var monthlyEmployeeTotalDto = MonthlyEmployeeTotalTestData.MonthlyEmployeeTotal_PreviuosMonth_CurrentYear;

        var montlhyEmployeeTotalList = new List<MonthlyEmployeeTotal>
        {
            monthlyEmployeeTotalDto
        };

        _dbMock.Setup(e => e.MonthlyEmployeeTotal.Get(It.IsAny<Expression<Func<MonthlyEmployeeTotal, bool>>>()))
               .Returns(montlhyEmployeeTotalList.ToMockIQueryable());

        var result = await _dashboardService.GetEmployeePreviousMonthTotal();

        Assert.NotNull(result);
        Assert.Equal(monthlyEmployeeTotalDto.EmployeeTotal, result.EmployeeTotal);
    }


    [Fact]
    public async Task GetPreviousMonthTotalCreateNewTotalTest()
    {
        var employeeList = new List<Employee>
        {
            EmployeeTestData.EmployeeOne
        };

        var employee = new List<Employee>
        {
            EmployeeTestData.EmployeeOne
        };

        _dbMock.Setup(u => u.Employee.GetAll(null)).ReturnsAsync(employeeList);

        _dbMock.Setup(e => e.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
               .Returns(employee.ToMockIQueryable());

        var monthlyEmployeeTotalDto = MonthlyEmployeeTotalTestData.MonthlyEmployeeTotal_MonthNov_CurrentYear;

        var montlhyEmployeeTotalList = new List<MonthlyEmployeeTotal>
        {
            monthlyEmployeeTotalDto
        };

        _dbMock.Setup(e => e.MonthlyEmployeeTotal.Get(It.IsAny<Expression<Func<MonthlyEmployeeTotal, bool>>>()))
               .Returns(montlhyEmployeeTotalList.ToMockIQueryable());

        _dbMock.Setup(u => u.MonthlyEmployeeTotal.Add(It.IsAny<MonthlyEmployeeTotal>()))
               .ReturnsAsync(monthlyEmployeeTotalDto);

        var result = await _dashboardService.GetEmployeePreviousMonthTotal();

        Assert.NotNull(result);
        Assert.Equal(1, result.EmployeeTotal);
    }

    [Fact]
    public async Task CalculateChurnRateTest()
    {
        var employeeList = new List<Employee>
        {
            EmployeeTestData.EmployeeOne
        };

        var employee = new List<Employee>
        {
            EmployeeTestData.EmployeeOne
        };

        _dbMock.Setup(u => u.Employee.GetAll(null)).ReturnsAsync(employeeList);

        _dbMock.Setup(e => e.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
               .Returns(employee.ToMockIQueryable());

        var monthlyEmployeeTotalDto = MonthlyEmployeeTotalTestData.MonthlyEmployeeTotal_PreviuosMonth_CurrentYear;

        var montlhyEmployeeTotalList = new List<MonthlyEmployeeTotal>
        {
            monthlyEmployeeTotalDto
        };

        var churnRateDto = new ChurnRateDataCardDto
        {
            ChurnRate = Math.Round(0.0, 0),
            DeveloperChurnRate = Math.Round(0.0, 0),
            DesignerChurnRate = Math.Round(0.0, 0),
            ScrumMasterChurnRate = Math.Round(0.0, 0),
            BusinessSupportChurnRate = Math.Round(0.0, 0),
            Month = "January",
            Year = 2024
        };

        _dbMock.Setup(e => e.MonthlyEmployeeTotal.Get(It.IsAny<Expression<Func<MonthlyEmployeeTotal, bool>>>()))
               .Returns(montlhyEmployeeTotalList.ToMockIQueryable());

        _dbMock.Setup(u => u.MonthlyEmployeeTotal.Add(It.IsAny<MonthlyEmployeeTotal>()))
               .ReturnsAsync(monthlyEmployeeTotalDto);

        var result = await _dashboardService.CalculateEmployeeChurnRate();

        Assert.NotNull(result);
        Assert.Equal(churnRateDto.ChurnRate, result.ChurnRate);
    }

    [Fact]
    public async Task CalculateChurnRateIfStatementTest()
    {
        var employeeList = new List<Employee>
        {
            EmployeeTestData.EmployeeOne
        };

        var employee = new List<Employee>
        {
            EmployeeTestData.EmployeeOne
        };

        _dbMock.Setup(u => u.Employee.GetAll(null)).ReturnsAsync(employeeList);

        _dbMock.Setup(e => e.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
               .Returns(employee.ToMockIQueryable());

        var monthlyEmployeeTotalDto = MonthlyEmployeeTotalTestData.MonthlyEmployeeTotal_PreviuosMonth_CurrentYear;

        var montlhyEmployeeTotalList = new List<MonthlyEmployeeTotal>
        {
            monthlyEmployeeTotalDto
        };

        var churnRateDto = new ChurnRateDataCardDto
        {
            ChurnRate = Math.Round(0.0, 0),
            DeveloperChurnRate = Math.Round(0.0, 0),
            DesignerChurnRate = Math.Round(0.0, 0),
            ScrumMasterChurnRate = Math.Round(0.0, 0),
            BusinessSupportChurnRate = Math.Round(0.0, 0),
            Month = "January",
            Year = 2024
        };

        _dbMock.Setup(e => e.MonthlyEmployeeTotal.Get(It.IsAny<Expression<Func<MonthlyEmployeeTotal, bool>>>()))
               .Returns(montlhyEmployeeTotalList.ToMockIQueryable());

        _dbMock.Setup(u => u.MonthlyEmployeeTotal.Add(It.IsAny<MonthlyEmployeeTotal>()))
               .ReturnsAsync(monthlyEmployeeTotalDto);

        var result = await _dashboardService.CalculateEmployeeChurnRate();

        Assert.NotNull(result);
        Assert.Equal(churnRateDto.ChurnRate, result.ChurnRate);
    }

    [Fact(Skip = "Needs Work")]
    public async Task GenerateEmployeeDataCardInfomrationTest()
    {
        var employeeType = new EmployeeType { Id = 2, Name = "Developer" };
        var employeeAddress = new EmployeeAddress { Id = 1, UnitNumber = "2", ComplexName = "Complex", StreetNumber = "2", SuburbOrDistrict = "Suburb/District", City = "City", Country = "Country", Province = "Province", PostalCode = "1620" };

        var employeeDto = new Employee
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
            EmployeeType = employeeType,
            Notes = "Notes",
            LeaveInterval = 1,
            SalaryDays = 28,
            PayRate = 128,
            Salary = 100000,
            Name = "Dorothy",
            Initials = "D",
            Surname = "Mahoko",
            DateOfBirth = DateTime.Now,
            CountryOfBirth = "South Africa",
            Nationality = "South African",
            IdNumber = "0000080000000",
            PassportNumber = " ",
            PassportExpirationDate = DateTime.Now,
            PassportCountryIssue = null,
            Race = Race.Black,
            Gender = Gender.Male,
            Email = "texample@retrorabbit.co.za",
            PersonalEmail = "test.example@gmail.com",
            CellphoneNo = "0000000000",
            PhysicalAddress = employeeAddress,
            PostalAddress = employeeAddress
        };

        var employeeList = new List<Employee>
            {
                employeeDto
            };

        var employee = new List<Employee>
            {
                employeeDto
            };

        _dbMock.Setup(u => u.Employee.GetAll(It.IsAny<Expression<Func<Employee, bool>>>())).ReturnsAsync(employeeList);
        _dbMock.Setup(e => e.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>())).Returns(employee.ToMockIQueryable());

        var monthlyEmployeeTotalDto = MonthlyEmployeeTotalTestData.MonthlyEmployeeTotal_MonthNov_CurrentYear;

        var montlhyEmployeeTotalList = new List<MonthlyEmployeeTotal>
            {
              monthlyEmployeeTotalDto,
            };

        _dbMock.Setup(e => e.MonthlyEmployeeTotal.Get(It.IsAny<Expression<Func<MonthlyEmployeeTotal, bool>>>()))
             .Returns(montlhyEmployeeTotalList.ToMockIQueryable());

        _dbMock.Setup(u => u.MonthlyEmployeeTotal.Add(It.IsAny<MonthlyEmployeeTotal>()))
           .ReturnsAsync(monthlyEmployeeTotalDto);

        var result = await _dashboardService.GenerateDataCardInformation();

        Assert.NotNull(result);
        Assert.Equivalent(1, result.DevsCount);
    }
}
