using System.Linq.Expressions;
using HRIS.Models;
using HRIS.Models.Enums;
using HRIS.Services.Interfaces;
using HRIS.Services.Services;
using MockQueryable.Moq;
using Moq;
using RR.Tests.Data.Models.HRIS;
using RR.UnitOfWork;
using RR.UnitOfWork.Entities.HRIS;
using RR.UnitOfWork.Interfaces.HRIS;
using Xunit;

namespace RGO.Tests.Services;

public class EmployeeServiceUnitTests
{
    private List<EmployeeDto> employeeList = new List<EmployeeDto>
        {
            EmployeeTestData.EmployeeDto
        };

    private List<Employee> employee = new List<Employee>
        {
            new(EmployeeTestData.EmployeeDto, EmployeeTypeTestData.DeveloperType)
        };

    private readonly Mock<IUnitOfWork> _dbMock;
    private readonly Mock<IEmployeeAddressService> employeeAddressServiceMock;
    private readonly Mock<IEmployeeTypeService> employeeTypeServiceMock;
    private readonly Mock<IRoleService> roleServiceMock;

    private readonly EmployeeRoleDto employeeRoleDto = new(0, EmployeeTestData.EmployeeDto, EmployeeRoleTestData.RoleDtoEmployee);
    private readonly EmployeeService employeeService;

    public EmployeeServiceUnitTests()
    {
        _dbMock = new Mock<IUnitOfWork>();
        employeeTypeServiceMock = new Mock<IEmployeeTypeService>();
        employeeAddressServiceMock = new Mock<IEmployeeAddressService>();
        roleServiceMock = new Mock<IRoleService>();
        employeeService = new EmployeeService(employeeTypeServiceMock.Object, _dbMock.Object,
                                              employeeAddressServiceMock.Object, roleServiceMock.Object);
    }

    [Fact]
    public async Task SaveEmployeeFailTest1()
    {
        _dbMock.Setup(r => r.Employee.Any(It.IsAny<Expression<Func<Employee, bool>>>())).Returns(Task.FromResult(true));

        await Assert.ThrowsAsync<Exception>(() => employeeService.SaveEmployee(EmployeeTestData.EmployeeDto));
    }

    [Fact]
    public async Task SaveEmployeeFailTest2()
    {
        _dbMock.Setup(r => r.Employee.Any(It.IsAny<Expression<Func<Employee, bool>>>())).Returns(Task.FromResult(false));

        employeeTypeServiceMock.Setup(r => r.GetEmployeeType(It.IsAny<string>())).Throws(new Exception());

        employeeTypeServiceMock.Setup(r => r.SaveEmployeeType(It.IsAny<EmployeeTypeDto>())).ReturnsAsync(EmployeeTypeTestData.DeveloperType);

        employeeAddressServiceMock.SetupSequence(r => r.CheckIfExists(It.IsAny<EmployeeAddressDto>()))
            .ReturnsAsync(false)
            .ReturnsAsync(false);

        employeeAddressServiceMock.SetupSequence(r => r.Save(It.IsAny<EmployeeAddressDto>()))
            .ReturnsAsync(EmployeeAddressTestData.EmployeeAddressDto)
            .ReturnsAsync(EmployeeAddressTestData.EmployeeAddressDto);

        _dbMock.Setup(r => r.Employee.Add(It.IsAny<Employee>())).Returns(Task.FromResult(EmployeeTestData.EmployeeDto));
        _dbMock.Setup(r => r.EmployeeRole.Add(It.IsAny<EmployeeRole>())).Returns(Task.FromResult(employeeRoleDto));

        roleServiceMock.Setup(r => r.GetRole("Employee")).Returns(Task.FromResult(EmployeeRoleTestData.RoleDtoEmployee));
        var result = await employeeService.SaveEmployee(EmployeeTestData.EmployeeDto);

        Assert.Equal(EmployeeTestData.EmployeeDto, result);

    }

    [Fact]
    public async Task SaveEmployeeFailTest3()
    {
        _dbMock.Setup(r => r.Employee.Any(It.IsAny<Expression<Func<Employee, bool>>>()))
               .Returns(Task.FromResult(false));

        employeeTypeServiceMock.Setup(r => r.GetEmployeeType(It.IsAny<string>())).Throws(new Exception());

        employeeTypeServiceMock.Setup(r => r.SaveEmployeeType(It.IsAny<EmployeeTypeDto>()))
                               .Returns(Task.FromResult(EmployeeTypeTestData.DeveloperType));

        employeeAddressServiceMock.SetupSequence(r => r.CheckIfExists(EmployeeAddressTestData.EmployeeAddressDto))
                                  .ReturnsAsync(true)
                                  .ReturnsAsync(true);

        employeeAddressServiceMock.Setup(x => x.Get(It.IsAny<EmployeeAddressDto>()))
                                  .ReturnsAsync(EmployeeAddressTestData.EmployeeAddressDto);

        employeeAddressServiceMock.SetupSequence(r => r.Save(EmployeeAddressTestData.EmployeeAddressDto))
                                  .ReturnsAsync(EmployeeAddressTestData.EmployeeAddressDto);

        _dbMock.Setup(r => r.Employee.Add(It.IsAny<Employee>())).Returns(Task.FromResult(EmployeeTestData.EmployeeDto));
        _dbMock.Setup(r => r.EmployeeRole.Add(It.IsAny<EmployeeRole>())).Returns(Task.FromResult(employeeRoleDto));

        roleServiceMock.Setup(r => r.GetRole("Employee")).Returns(Task.FromResult(EmployeeRoleTestData.RoleDtoEmployee));

        var result = await employeeService.SaveEmployee(EmployeeTestData.EmployeeDto);

        Assert.Equal(EmployeeTestData.EmployeeDto, result);
    }

    [Fact]
    public async Task SaveEmployeeTest()
    {
        employeeTypeServiceMock.Setup(r => r.GetEmployeeType(EmployeeTypeTestData.DeveloperType.Name))
                               .Returns(Task.FromResult(EmployeeTypeTestData.DeveloperType));
        var employeeRoleDto = new EmployeeRoleDto(0, EmployeeTestData.EmployeeDto, EmployeeRoleTestData.RoleDtoEmployee);

        employeeAddressServiceMock.SetupSequence(r => r.CheckIfExists(It.IsAny<EmployeeAddressDto>()))
                                  .ReturnsAsync(false)
                                  .ReturnsAsync(true);

        employeeAddressServiceMock.Setup(r => r.Save(It.IsAny<EmployeeAddressDto>()))
                                  .ReturnsAsync(EmployeeAddressTestData.EmployeeAddressDto);

        employeeAddressServiceMock.Setup(r => r.Get(It.IsAny<EmployeeAddressDto>()))
                                  .ReturnsAsync(EmployeeAddressTestData.EmployeeAddressDto);

        roleServiceMock.Setup(r => r.GetRole("Employee")).Returns(Task.FromResult(EmployeeRoleTestData.RoleDtoEmployee));

        _dbMock.Setup(r => r.Employee.Add(It.IsAny<Employee>())).Returns(Task.FromResult(EmployeeTestData.EmployeeDto));
        _dbMock.Setup(r => r.EmployeeRole.Add(It.IsAny<EmployeeRole>())).Returns(Task.FromResult(employeeRoleDto));

        var result = await employeeService.SaveEmployee(EmployeeTestData.EmployeeDto);
        Assert.NotNull(result);
        Assert.Equal(EmployeeTestData.EmployeeDto, result);
    }

    //There is not PushToPreducerTestPass because we cannot mock the connection to RabbitMQ Docker Instance

    [Fact]
    public void PushToProducerTestFail()
    {
        var employeeTypeServiceMock = new Mock<IEmployeeTypeService>();
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var _dbMock = new Mock<IUnitOfWork>();

        employeeTypeServiceMock.Setup(r => r.GetEmployeeType(EmployeeTypeTestData.DeveloperType.Name))
                               .Returns(Task.FromResult(EmployeeTypeTestData.DeveloperType));

        var employeeData = new Employee(EmployeeTestData.EmployeeDto, EmployeeTypeTestData.DeveloperType);

        employeeService.PushToProducer(employeeData);
    }


    [Fact]
    public async Task GetEmployeeTest()
    {
        var employeeTypeServiceMock = new Mock<IEmployeeTypeService>();
        var _dbMock = new Mock<IUnitOfWork>();
        var employeeRepositoryMock = new Mock<IEmployeeRepository>();

        var employeeList = new List<Employee>
        {
            new(EmployeeTestData.EmployeeDto, EmployeeTypeTestData.DeveloperType)
        };

        _dbMock.Setup(e => e.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
               .Returns(employeeList.AsQueryable().BuildMock());


        var result = employeeService.GetEmployee("dm@retrorabbit.co.za");

        Assert.NotNull(result);
    }

    [Fact]
    public async Task DeleteEmployeeTest()
    {
        employeeTypeServiceMock.Setup(r => r.GetEmployeeType(EmployeeTypeTestData.DeveloperType.Name))
                               .Returns(Task.FromResult(EmployeeTypeTestData.DeveloperType));

        var employeeList = new List<Employee>
        {
            new(EmployeeTestData.EmployeeDto, EmployeeTypeTestData.DeveloperType)
        };

        _dbMock.Setup(e => e.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
               .Returns(employeeList.AsQueryable().BuildMock());

        _dbMock.Setup(r => r.Employee.Delete(EmployeeTestData.EmployeeDto.Id))
               .Returns(Task.FromResult(EmployeeTestData.EmployeeDto));

        var result = await employeeService.DeleteEmployee(EmployeeTestData.EmployeeDto.Email);
        Assert.NotNull(result);
    }

    [Fact]
    public async Task GetAllTest()
    {
        var employees = new List<Employee>
        {
            new(EmployeeTestData.EmployeeDto, EmployeeTypeTestData.DeveloperType),
            new(EmployeeTestData.EmployeeDto2, EmployeeTypeTestData.DeveloperType),
            new(EmployeeTestData.EmployeeDto3, EmployeeTypeTestData.DeveloperType)
        };

        _dbMock.Setup(u => u.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
               .Returns(employees.AsQueryable().BuildMock());

        var result = await employeeService.GetAll();

        Assert.NotNull(result);
        Assert.IsType<List<EmployeeDto>>(result);
        Assert.True(result.SequenceEqual(result.OrderBy(e => e.Name)));
        _dbMock.Verify(u => u.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()), Times.Once);
    }

    [Fact]
    public async Task GetAllIsJourney()
    {
        var emp = new Employee(EmployeeTestData.EmployeeDto2, EmployeeTypeTestData.DeveloperType);
        emp.EmployeeType = new EmployeeType(EmployeeTypeTestData.DeveloperType);

        var empRoleDto = new EmployeeRoleDto(1, EmployeeTestData.EmployeeDto, EmployeeRoleTestData.RoleDtoEmployee);
        var empRole = new EmployeeRole(empRoleDto);

        var employees = new List<Employee> { emp };
        var empRoles = new List<EmployeeRole> { empRole };
        var roles = new List<Role> { new(EmployeeRoleTestData.RoleDtoEmployee) };

        var mockEmployees = employees;
        var expectedEmployees = new List<Employee> { new(EmployeeTestData.EmployeeDto3, EmployeeTypeTestData.DeveloperType) };
        _dbMock.Setup(x => x.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
               .Returns(mockEmployees.AsQueryable().BuildMock());

        _dbMock.Setup(x => x.EmployeeRole.Get(It.IsAny<Expression<Func<EmployeeRole, bool>>>()))
               .Returns(empRoles.AsQueryable().BuildMock());

        _dbMock.Setup(x => x.Role.Get(It.IsAny<Expression<Func<Role, bool>>>()))
               .Returns(roles.AsQueryable().BuildMock());

        _dbMock.Setup(x => x.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
               .Returns(expectedEmployees.AsQueryable().BuildMock());

        var journeyEmployees = await employeeService.GetAll(EmployeeTestData.EmployeeDto2.Email);

        Assert.Equal(journeyEmployees.Count, 1);
    }

    [Fact]
    public async Task CheckEmployeeExistsTest()
    {
        var employeeTypeServiceMock = new Mock<IEmployeeTypeService>();
        var _dbMock = new Mock<IUnitOfWork>();
        var employeeRepositoryMock = new Mock<IEmployeeRepository>();

        _dbMock.Setup(r => r.Employee.Any(It.IsAny<Expression<Func<Employee, bool>>>())).Returns(Task.FromResult(true));

        var result = employeeService.CheckUserExist("dm@retrorabbit.co.za");

        Assert.NotNull(result);
    }

    [Fact]
    public async Task UpdateEmployeeTestOwnProfile()
    {
        employeeTypeServiceMock.Setup(r => r.GetEmployeeType(EmployeeTypeTestData.DeveloperType.Name))
                               .Returns(Task.FromResult(EmployeeTypeTestData.DeveloperType));

        _dbMock.Setup(r => r.Employee.Update(It.IsAny<Employee>()))
               .Returns(Task.FromResult(EmployeeTestData.EmployeeDto));

        var result =
            await employeeService.UpdateEmployee(EmployeeTestData.EmployeeDto, EmployeeTestData.EmployeeDto.Email);

        Assert.Equal(EmployeeTestData.EmployeeDto, result);
    }

    [Fact]
    public async Task UpdateEmployeeTestAdminPass()
    {
        Employee emp = new Employee(EmployeeTestData.EmployeeDto2, EmployeeTypeTestData.DeveloperType);
        emp.EmployeeType = new EmployeeType(EmployeeTypeTestData.DeveloperType);

        RoleDto roleDto = new RoleDto(2, "Admin");
        EmployeeRoleDto empRoleDto = new EmployeeRoleDto(1, EmployeeTestData.EmployeeDto2, roleDto);
        EmployeeRole empRole = new EmployeeRole(empRoleDto);

        List<Employee> employees = new List<Employee> { emp };
        List<EmployeeRole> empRoles = new List<EmployeeRole> { empRole };
        List<Role> roles = new List<Role> { new Role(roleDto) };

        employeeTypeServiceMock.Setup(r => r.GetEmployeeType(EmployeeTypeTestData.DeveloperType.Name)).Returns(Task.FromResult(EmployeeTypeTestData.DeveloperType));

        _dbMock.Setup(r => r.Employee.Update(It.IsAny<Employee>())).Returns(Task.FromResult(EmployeeTestData.EmployeeDto));
        _dbMock.Setup(r => r.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>())).Returns(employees.AsQueryable().BuildMock());
        _dbMock.Setup(r => r.Employee.Any(It.IsAny<Expression<Func<Employee, bool>>>())).ReturnsAsync(true);
        _dbMock.Setup(r => r.EmployeeRole.Get(It.IsAny<Expression<Func<EmployeeRole, bool>>>())).Returns(empRoles.AsQueryable().BuildMock());
        _dbMock.Setup(r => r.Role.Get(It.IsAny<Expression<Func<Role, bool>>>())).Returns(roles.AsQueryable().BuildMock());

        var result = await employeeService.UpdateEmployee(EmployeeTestData.EmployeeDto, "admin@retrorabbit.co.za");

        Assert.NotNull(result);
        Assert.Equal(EmployeeTestData.EmployeeDto, result);
    }

    [Fact]
    public async Task UpdateEmployeeTestAdminFail()
    {
        var emp = new Employee(EmployeeTestData.EmployeeDto2, EmployeeTypeTestData.DeveloperType);
        emp.EmployeeType = new EmployeeType(EmployeeTypeTestData.DeveloperType);
        var employees = new List<Employee> { emp };

        var empRoleDto = new EmployeeRoleDto(1, EmployeeTestData.EmployeeDto2, EmployeeRoleTestData.RoleDtoEmployee);
        var empRole = new EmployeeRole(empRoleDto);

        var empRoles = new List<EmployeeRole> { empRole };
        var roles = new List<Role> { new(EmployeeRoleTestData.RoleDtoEmployee) };

        employeeTypeServiceMock.Setup(r => r.GetEmployeeType(EmployeeTypeTestData.DeveloperType.Name))
                               .Returns(Task.FromResult(EmployeeTypeTestData.DeveloperType));

        _dbMock.Setup(r => r.Employee.Update(It.IsAny<Employee>()))
               .Returns(Task.FromResult(EmployeeTestData.EmployeeDto));
        _dbMock.Setup(r => r.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
               .Returns(employees.AsQueryable().BuildMock());
        _dbMock.Setup(r => r.Employee.Any(It.IsAny<Expression<Func<Employee, bool>>>())).ReturnsAsync(true);
        _dbMock.Setup(r => r.EmployeeRole.Get(It.IsAny<Expression<Func<EmployeeRole, bool>>>()))
               .Returns(empRoles.AsQueryable().BuildMock());
        _dbMock.Setup(r => r.Role.Get(It.IsAny<Expression<Func<Role, bool>>>()))
               .Returns(roles.AsQueryable().BuildMock());

        var exception = await Assert.ThrowsAsync<Exception>(
                                                            async () =>
                                                                await employeeService
                                                                    .UpdateEmployee(EmployeeTestData.EmployeeDto,
                                                                     "unauthorized.email@retrorabbit.co.za"));

        Assert.Equal("Unauthorized action", exception.Message);
    }

    [Fact]
    public async Task UpdateEmployeeTestUserDoesNotExist()
    {
        var emp = new Employee(EmployeeTestData.EmployeeDto2, EmployeeTypeTestData.DeveloperType);
        emp.EmployeeType = new EmployeeType(EmployeeTypeTestData.DeveloperType);

        var employees = new List<Employee> { emp };

        var empRoleDto = new EmployeeRoleDto(1, EmployeeTestData.EmployeeDto2, EmployeeRoleTestData.RoleDtoEmployee);

        var roles = new List<Role> { new(EmployeeRoleTestData.RoleDtoEmployee) };

        employeeTypeServiceMock.Setup(r => r.GetEmployeeType(EmployeeTypeTestData.DeveloperType.Name))
                               .Returns(Task.FromResult(EmployeeTypeTestData.DeveloperType));
        _dbMock.Setup(r => r.Employee.Update(It.IsAny<Employee>()))
               .Returns(Task.FromResult(EmployeeTestData.EmployeeDto));
        _dbMock.Setup(r => r.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
               .Returns(employees.AsQueryable().BuildMock());
        _dbMock.Setup(r => r.Employee.Any(It.IsAny<Expression<Func<Employee, bool>>>())).ReturnsAsync(false);

        var exception = await Assert.ThrowsAsync<Exception>(
                                                            async () =>
                                                                await employeeService
                                                                    .UpdateEmployee(EmployeeTestData.EmployeeDto,
                                                                     "unauthorized.email@retrorabbit.co.za"));

        Assert.Equal("Unauthorized action", exception.Message);
    }

    [Fact]
    public async Task GetByIdPass()
    {
        _dbMock.Setup(x => x.Employee.GetById(EmployeeTestData.EmployeeDto.Id))
               .ReturnsAsync(EmployeeTestData.EmployeeDto);

        var result = await employeeService.GetById(EmployeeTestData.EmployeeDto.Id);

        Assert.NotNull(result);
        Assert.Equal(EmployeeTestData.EmployeeDto, result);
    }

    [Fact]
    public async Task GetSimpleProfileWithPCAndTeamLeadAndClient()
    {
        var allocatedClient = new ClientDto(1, "FNB");
        var clients = new List<Client> { new(allocatedClient) };

        var employeeList = new List<Employee> { new(EmployeeTestData.EmployeeDto4, EmployeeTypeTestData.DeveloperType) };
        employeeList.First().EmployeeType = new EmployeeType(EmployeeTypeTestData.DeveloperType);

        _dbMock.Setup(e => e.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
               .Returns(employeeList.AsQueryable().BuildMock());

        _dbMock.Setup(e => e.Employee.GetById(It.IsAny<int>()))
               .ReturnsAsync((int id) =>
                                 id == EmployeeTestData.EmployeeDto4.TeamLead
                                     ? EmployeeTestData.EmployeeDto3
                                     : EmployeeTestData.EmployeeDto2);

        _dbMock.Setup(db => db.Client.Get(It.IsAny<Expression<Func<Client, bool>>>()))
               .Returns(clients.AsQueryable().BuildMock());

        var result = await employeeService.GetSimpleProfile(EmployeeTestData.EmployeeDto4.Email);

        Assert.NotNull(result);
        Assert.Equal(EmployeeTestData.EmployeeDto4.TeamLead, result.TeamLeadId);
        Assert.Equal(EmployeeTestData.EmployeeDto4.PeopleChampion, result.PeopleChampionId);
        Assert.Equal(allocatedClient.Name, result.ClientAllocatedName);
    }

    [Fact]
    public async Task GetEmployeeByIdPass()
    {
        var employees = new List<Employee> { new(EmployeeTestData.EmployeeDto, EmployeeTypeTestData.DeveloperType) };

        employees.First().EmployeeType = new EmployeeType(EmployeeTypeTestData.DeveloperType);
        employees.First().PhysicalAddress = new EmployeeAddress(EmployeeAddressTestData.EmployeeAddressDto);
        employees.First().PostalAddress = new EmployeeAddress(EmployeeAddressTestData.EmployeeAddressDto);

        _dbMock.Setup(e => e.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
               .Returns(employees.AsQueryable().BuildMock());

        var result = await employeeService.GetEmployeeById(EmployeeTestData.EmployeeDto.Id);

        Assert.NotNull(result);
        Assert.Equivalent(EmployeeTestData.EmployeeDto, result);
    }

    [Fact]
    public async Task GetEmployeeByIdFail()
    {
        var mockEmployees = new List<Employee>();

        _dbMock.Setup(x => x.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
               .Returns(mockEmployees.AsQueryable().BuildMock());

        await Assert.ThrowsAsync<Exception>(() => employeeService.GetEmployeeById(2));
    }

    [Fact]
    public async Task GetCurrentMonthTotalReturnsExistingTotalTest()
    {
        
        var employeeList = new List<EmployeeDto>
        {
            EmployeeTestData.EmployeeDto
        };

        var employee = new List<Employee>
        {
            new(EmployeeTestData.EmployeeDto, EmployeeTypeTestData.DeveloperType)
        };

        _dbMock.Setup(u => u.Employee.GetAll(null)).ReturnsAsync(employeeList);

        _dbMock.Setup(e => e.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
               .Returns(employee.AsQueryable().BuildMock());

        var currentMonth = DateTime.Now.ToString("MMMM");

        var currentYear = DateTime.Now.Year;

        MonthlyEmployeeTotalDto monthlyEmployeeTotalDto = MonthlyEmployeeTotalTestData.monthlyEmployeeTotalDtoCurrentYearCurrentMonth;

        var montlhyEmployeeTotalList = new List<MonthlyEmployeeTotal>
        {
            new(monthlyEmployeeTotalDto)
        };

        _dbMock.Setup(e => e.MonthlyEmployeeTotal.Get(It.IsAny<Expression<Func<MonthlyEmployeeTotal, bool>>>()))
               .Returns(montlhyEmployeeTotalList.AsQueryable().BuildMock());

        var result = await employeeService.GetEmployeeCurrentMonthTotal();

        Assert.NotNull(result);
        Assert.Equal(monthlyEmployeeTotalDto.Month, result.Month);
    }

    [Fact]
    public async Task GetCurrentMonthTotalCreateNewTotalTest()
    {
        
        var employeeList = new List<EmployeeDto>
        {
            EmployeeTestData.EmployeeDto
        };

        var employee = new List<Employee>
        {
            new(EmployeeTestData.EmployeeDto, EmployeeTypeTestData.DeveloperType)
        };

        _dbMock.Setup(u => u.Employee.GetAll(null)).ReturnsAsync(employeeList);

        _dbMock.Setup(e => e.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
               .Returns(employee.AsQueryable().BuildMock());

        var previousMonth = DateTime.Now.AddMonths(-1).ToString("MMMM");

        var currentYear = DateTime.Now.Year;

        MonthlyEmployeeTotalDto monthlyEmployeeTotalDto = MonthlyEmployeeTotalTestData.monthlyEmployeeTotalDtoPreviuosMonthCurrentYear;

        var montlhyEmployeeTotalList = new List<MonthlyEmployeeTotal>
        {
            new(monthlyEmployeeTotalDto)
        };

        _dbMock.Setup(e => e.MonthlyEmployeeTotal.Get(It.IsAny<Expression<Func<MonthlyEmployeeTotal, bool>>>()))
               .Returns(montlhyEmployeeTotalList.AsQueryable().BuildMock());

        _dbMock.Setup(u => u.MonthlyEmployeeTotal.Add(It.IsAny<MonthlyEmployeeTotal>()))
               .ReturnsAsync(monthlyEmployeeTotalDto);

        var result = await employeeService.GetEmployeeCurrentMonthTotal();

        _dbMock.Verify(u => u.MonthlyEmployeeTotal.Add(It.IsAny<MonthlyEmployeeTotal>()), Times.Once);

        Assert.NotNull(result);
        Assert.Equal(1, result.EmployeeTotal);
        Assert.Equal(1, result.DeveloperTotal);
    }


    [Fact]
    public async Task GetPreviousMonthTotalReturnsExistingTotalTest()
    {
        var previousMonth = DateTime.Now.AddMonths(-1).ToString("MMMM");

        MonthlyEmployeeTotalDto monthlyEmployeeTotalDto = MonthlyEmployeeTotalTestData.monthlyEmployeeTotalDtoPreviuosMonthCurrentYear;

        var montlhyEmployeeTotalList = new List<MonthlyEmployeeTotal>
        {
            new(monthlyEmployeeTotalDto)
        };

        _dbMock.Setup(e => e.MonthlyEmployeeTotal.Get(It.IsAny<Expression<Func<MonthlyEmployeeTotal, bool>>>()))
               .Returns(montlhyEmployeeTotalList.AsQueryable().BuildMock());

        var result = await employeeService.GetEmployeePreviousMonthTotal();

        Assert.NotNull(result);
        Assert.Equal(monthlyEmployeeTotalDto.EmployeeTotal, result.EmployeeTotal);
    }


    [Fact]
    public async Task GetPreviousMonthTotalreateNewTotalTest()
    {
        var previousMonth = DateTime.Now.AddMonths(-1).ToString("MMMM");

        var employeeList = new List<EmployeeDto>
        {
            EmployeeTestData.EmployeeDto
        };

        var employee = new List<Employee>
        {
            new(EmployeeTestData.EmployeeDto, EmployeeTypeTestData.DeveloperType)
        };

        _dbMock.Setup(u => u.Employee.GetAll(null)).ReturnsAsync(employeeList);

        _dbMock.Setup(e => e.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
               .Returns(employee.AsQueryable().BuildMock());

        MonthlyEmployeeTotalDto monthlyEmployeeTotalDto = MonthlyEmployeeTotalTestData.monthlyEmployeeTotalDtoMonthNovCurrentYear;

        var montlhyEmployeeTotalList = new List<MonthlyEmployeeTotal>
        {
            new(monthlyEmployeeTotalDto)
        };

        _dbMock.Setup(e => e.MonthlyEmployeeTotal.Get(It.IsAny<Expression<Func<MonthlyEmployeeTotal, bool>>>()))
               .Returns(montlhyEmployeeTotalList.AsQueryable().BuildMock());

        _dbMock.Setup(u => u.MonthlyEmployeeTotal.Add(It.IsAny<MonthlyEmployeeTotal>()))
               .ReturnsAsync(monthlyEmployeeTotalDto);

        var result = await employeeService.GetEmployeePreviousMonthTotal();

        Assert.NotNull(result);
        Assert.Equal(1, result.EmployeeTotal);
    }

    [Fact]
    public async Task CalculateChurnRateTest()
    {
        var previousMonth = DateTime.Now.AddMonths(-1).ToString("MMMM");

        var employeeList = new List<EmployeeDto>
        {
            EmployeeTestData.EmployeeDto
        };

        var employee = new List<Employee>
        {
            new(EmployeeTestData.EmployeeDto, EmployeeTypeTestData.DeveloperType)
        };

        _dbMock.Setup(u => u.Employee.GetAll(null)).ReturnsAsync(employeeList);

        _dbMock.Setup(e => e.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
               .Returns(employee.AsQueryable().BuildMock());

        MonthlyEmployeeTotalDto monthlyEmployeeTotalDto = MonthlyEmployeeTotalTestData.monthlyEmployeeTotalDtoPreviuosMonthCurrentYear;

        var montlhyEmployeeTotalList = new List<MonthlyEmployeeTotal>
        {
            new(monthlyEmployeeTotalDto)
        };

        var churnRateDto = new ChurnRateDataCard
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
               .Returns(montlhyEmployeeTotalList.AsQueryable().BuildMock());

        _dbMock.Setup(u => u.MonthlyEmployeeTotal.Add(It.IsAny<MonthlyEmployeeTotal>()))
               .ReturnsAsync(monthlyEmployeeTotalDto);

        var result = await employeeService.CalculateEmployeeChurnRate();

        Assert.NotNull(result);
        Assert.Equal(churnRateDto.ChurnRate, result.ChurnRate);
    }

    [Fact]
    public async Task CalculateChurnRateIfStatementTest()
    {
        var previousMonth = DateTime.Now.AddMonths(-1).ToString("MMMM");

        var employeeList = new List<EmployeeDto>
        {
            EmployeeTestData.EmployeeDto
        };

        var employee = new List<Employee>
        {
            new(EmployeeTestData.EmployeeDto, EmployeeTypeTestData.DeveloperType)
        };

        _dbMock.Setup(u => u.Employee.GetAll(null)).ReturnsAsync(employeeList);

        _dbMock.Setup(e => e.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
               .Returns(employee.AsQueryable().BuildMock());

        MonthlyEmployeeTotalDto monthlyEmployeeTotalDto = MonthlyEmployeeTotalTestData.monthlyEmployeeTotalDtoPreviuosMonthCurrentYear;

        var montlhyEmployeeTotalList = new List<MonthlyEmployeeTotal>
        {
            new(monthlyEmployeeTotalDto)
        };

        var churnRateDto = new ChurnRateDataCard
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
               .Returns(montlhyEmployeeTotalList.AsQueryable().BuildMock());

        _dbMock.Setup(u => u.MonthlyEmployeeTotal.Add(It.IsAny<MonthlyEmployeeTotal>()))
               .ReturnsAsync(monthlyEmployeeTotalDto);

        var result = await employeeService.CalculateEmployeeChurnRate();

        Assert.NotNull(result);
        Assert.Equal(churnRateDto.ChurnRate, result.ChurnRate);
    }

    [Fact]
    public async Task GenerateEmployeeDataCardInfomrationTest()
    {
        EmployeeTypeDto employeeTypeDto = new(2, "Developer");
        EmployeeType employeeType = new(employeeTypeDto);
        EmployeeAddressDto employeeAddressDto = new EmployeeAddressDto(1, "2", "Complex", "2", "Suburb/District", "City", "Country", "Province", "1620");

        EmployeeDto employeeDto = new(1, "001", "34434434", new DateTime(), new DateTime(),
           null, false, "None", 4, employeeTypeDto, "Notes", 1, 28, 128, 100000, "Dotty", "D",
           "Missile", new DateTime(), "South Africa", "South African", "0000000000000", " ",
           new DateTime(), null,HRIS.Models.Enums.Race.Black, HRIS.Models.Enums.Gender.Female, null,
           "dm@.co.za", "test@gmail.com", "0123456789", null, null, employeeAddressDto, employeeAddressDto, null, null, null);

        var employeeList = new List<EmployeeDto>
            {
                employeeDto
            };

        var employee = new List<Employee>
            {
                new Employee(employeeDto,employeeTypeDto)
            };

        _dbMock.Setup(u => u.Employee.GetAll(null)).ReturnsAsync(employeeList);

        _dbMock.Setup(e => e.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
              .Returns(employee.AsQueryable().BuildMock());

        MonthlyEmployeeTotalDto monthlyEmployeeTotalDto = MonthlyEmployeeTotalTestData.monthlyEmployeeTotalDtoMonthNovCurrentYear;

        var montlhyEmployeeTotalList = new List<MonthlyEmployeeTotal>
            {
              new MonthlyEmployeeTotal (monthlyEmployeeTotalDto),
            };

        _dbMock.Setup(e => e.MonthlyEmployeeTotal.Get(It.IsAny<Expression<Func<MonthlyEmployeeTotal, bool>>>()))
             .Returns(montlhyEmployeeTotalList.AsQueryable().BuildMock());

        _dbMock.Setup(u => u.MonthlyEmployeeTotal.Add(It.IsAny<MonthlyEmployeeTotal>()))
           .ReturnsAsync(monthlyEmployeeTotalDto);

        var result = await employeeService.GenerateDataCardInformation();

        Assert.NotNull(result);
        Assert.Equal(1, result.DevsCount);
    }
}
