using MockQueryable.Moq;
using Moq;
using RGO.Models;
using RGO.Models.Enums;
using RGO.Services.Interfaces;
using RGO.Services.Services;
using RGO.UnitOfWork;
using RGO.UnitOfWork.Entities;
using System.Linq.Expressions;
using Xunit;

namespace RGO.Services.Tests;
public class EmployeeBankingServiceTest
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IEmployeeBankingService> _employeeBankingService;
    private readonly Mock<IEmployeeTypeService> employeeTypeServiceMock;
    private EmployeeBankingService employeeBankingService;

    EmployeeBankingDto test1 = new EmployeeBankingDto(1, 1, "FNB", "Not Sure", "120", EmployeeBankingAccountType.Savings, "Name1", BankApprovalStatus.PendingApproval, "", "asd", new DateOnly(), new DateOnly());
    EmployeeBankingDto test2 = new EmployeeBankingDto(2, 2, "FNB", "Not Sure", "120", EmployeeBankingAccountType.Savings, "Name1", BankApprovalStatus.Approved, "", "asd", new DateOnly(), new DateOnly());
    EmployeeBankingDto test3 = new EmployeeBankingDto(3, 3, "FNB", "Not Sure", "120", EmployeeBankingAccountType.Savings, "Name1", BankApprovalStatus.Declined, "", "asd", new DateOnly(), new DateOnly());

    static EmployeeTypeDto employeeTypeDto = new EmployeeTypeDto(1, "Developer");
    static EmployeeType employeeType = new EmployeeType(employeeTypeDto);
    static EmployeeAddressDto employeeAddressDto = new EmployeeAddressDto(1, "2", "Complex", "2", "Suburb/District", "City", "Country", "Province", "1620");

    EmployeeDto testEmployee1 = new(1, "001", "34434434", new DateTime(), new DateTime(), null, false, "None", 4, employeeTypeDto, "Notes", 1, 28, 128, 100000, "Matt", "MT",
    "Schoeman", new DateTime(), "South Africa", "South African", "0000080000000", " ", new DateTime(), null, Race.Black, Gender.Male, null,
    "test@retrorabbit.co.za", "test.example@gmail.com", "0000000000", null, null, employeeAddressDto, employeeAddressDto, null, null, null);

    EmployeeDto testEmployee2 = new(2, "001", "34434434", new DateTime(), new DateTime(), null, false, "None", 4, employeeTypeDto, "Notes", 1, 28, 128, 100000, "Matt", "MT",
    "Schoeman", new DateTime(), "South Africa", "South African", "0000080000000", " ", new DateTime(), null, Race.Black, Gender.Male, null,
    "test2@retrorabbit.co.za", "test.example@gmail.com", "0000000000", null, null, employeeAddressDto, employeeAddressDto, null, null, null);

    EmployeeDto testEmployee3 = new(3, "001", "34434434", new DateTime(), new DateTime(), null, false, "None", 4, employeeTypeDto, "Notes", 1, 28, 128, 100000, "Matt", "MT",
    "Schoeman", new DateTime(), "South Africa", "South African", "0000080000000", " ", new DateTime(), null, Race.Black, Gender.Male, null,
    "test3@retrorabbit.co.za", "test.example@gmail.com", "0000000000", null, null, employeeAddressDto, employeeAddressDto, null, null, null);
    public EmployeeBankingServiceTest()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _employeeBankingService = new Mock<IEmployeeBankingService>();
        employeeBankingService = new EmployeeBankingService(_mockUnitOfWork.Object);
        employeeTypeServiceMock = new Mock<IEmployeeTypeService>();
    }

    [Fact]
    public async Task GetPendingReturnsPendingBankDtosPass()
    {
        var pendingBankEntries = new List<EmployeeBankingDto> { test1 };
        var employees = new List<Employee>
    {
    new Employee(testEmployee1, employeeTypeDto)
    };
        var employeeBankings = new List<EmployeeBanking>
    {
    new EmployeeBanking(test1)
    };

        var mockEmployees = employees.AsQueryable().BuildMock();
        var mockEmployeeBankings = employeeBankings.AsQueryable().BuildMock();

        _mockUnitOfWork.Setup(u => u.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>())).Returns(mockEmployees);
        _mockUnitOfWork.Setup(u => u.EmployeeBanking.Get(It.IsAny<Expression<Func<EmployeeBanking, bool>>>())).Returns(mockEmployeeBankings);


        var result = await employeeBankingService.Get(0);

        Assert.Single(result);

        employees = new List<Employee>
    {
    new Employee(testEmployee2, employeeTypeDto)
    };

        employeeBankings = new List<EmployeeBanking>
    {
    new EmployeeBanking(test2)
    };

        mockEmployees = employees.AsQueryable().BuildMock();
        mockEmployeeBankings = employeeBankings.AsQueryable().BuildMock();

        _mockUnitOfWork.Setup(u => u.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>())).Returns(mockEmployees);
        _mockUnitOfWork.Setup(u => u.EmployeeBanking.Get(It.IsAny<Expression<Func<EmployeeBanking, bool>>>())).Returns(mockEmployeeBankings);
        result = await employeeBankingService.Get(1);

        Assert.Single(result);

        employees = new List<Employee>
    {
    new Employee(testEmployee3 , employeeTypeDto)
    };

        employeeBankings = new List<EmployeeBanking>
    {
    new EmployeeBanking(test3)
    };

        mockEmployees = employees.AsQueryable().BuildMock();
        mockEmployeeBankings = employeeBankings.AsQueryable().BuildMock();

        _mockUnitOfWork.Setup(u => u.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>())).Returns(mockEmployees);
        _mockUnitOfWork.Setup(u => u.EmployeeBanking.Get(It.IsAny<Expression<Func<EmployeeBanking, bool>>>())).Returns(mockEmployeeBankings);
        result = await employeeBankingService.Get(2);

        Assert.Single(result);
    }

    [Fact]
    public async Task UpdateReturnsUpdateBankDtos()
    {
        employeeTypeServiceMock.Setup(r => r.GetEmployeeType(employeeType.Name)).Returns(Task.FromResult(employeeTypeDto));
        EmployeeAddressDto employeeAddressDto = new EmployeeAddressDto(1, "2", "Complex", "2", "Suburb/District", "City", "Country", "Province", "1620");

        Employee employee = new Employee(testEmployee1, testEmployee1.EmployeeType);
        employee.EmployeeType = employeeType;
        employee.PhysicalAddress = new EmployeeAddress(employeeAddressDto);
        employee.PostalAddress = new EmployeeAddress(employeeAddressDto);
        List<Employee> employees = new List<Employee>
        {
                employee
        };
        var mockEmployees = employees;
        EmployeeBanking bankingObj = new EmployeeBanking(test1);
        List<EmployeeBanking> bankingS = new List<EmployeeBanking>
        {
                bankingObj
        };
        _mockUnitOfWork.Setup(u => u.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>())).Returns(mockEmployees.AsQueryable().BuildMock());
        _mockUnitOfWork.Setup(u => u.EmployeeBanking.Get(It.IsAny<Expression<Func<EmployeeBanking, bool>>>())).Returns(bankingS.AsQueryable().BuildMock());

        _mockUnitOfWork.Setup(u => u.EmployeeBanking.Update(It.IsAny<EmployeeBanking>()));
        var result = await employeeBankingService.Update(test1, "test@retrorabbit.co.za");

        Assert.Equal(test1, result);
    }

    [Fact]
    public async Task UpdatebyAdminPass()
    {
        Employee emp = new Employee(testEmployee1, employeeTypeDto);
        emp.EmployeeType = employeeType;
        List<Employee> employees = new List<Employee> { emp };
        RoleDto roleDto = new RoleDto(2, "Admin");
        EmployeeRoleDto empRoleDto = new EmployeeRoleDto(1, testEmployee1, roleDto);
        EmployeeRole empRole = new EmployeeRole(empRoleDto);
        List<EmployeeRole> empRoles = new List<EmployeeRole> { empRole };
        List<Role> roles = new List<Role> { new Role(roleDto) };
        EmployeeBanking bankingObj = new EmployeeBanking(test1);
        List<EmployeeBanking> bankingList = new List<EmployeeBanking>
        {
            bankingObj
        };
        var mockEmployees = employees;
        var authorizedEmail = "admin.email@example.com";

        _mockUnitOfWork.Setup(x => x.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
                      .Returns(mockEmployees.AsQueryable().BuildMock());

        _mockUnitOfWork.Setup(x => x.EmployeeRole.Get(It.IsAny<Expression<Func<EmployeeRole, bool>>>()))
                       .Returns(empRoles.AsQueryable().BuildMock());

        _mockUnitOfWork.Setup(x => x.Role.Get(It.IsAny<Expression<Func<Role, bool>>>()))
                       .Returns(roles.AsQueryable().BuildMock());

        _mockUnitOfWork.Setup(x => x.EmployeeBanking.Get(It.IsAny<Expression<Func<EmployeeBanking, bool>>>()))
            .Returns(bankingList.AsQueryable().BuildMock());

        _mockUnitOfWork.Setup(x => x.EmployeeBanking.Update(It.IsAny<EmployeeBanking>()))
                       .ReturnsAsync(test2);

        var validDtoObj = await employeeBankingService.Update(test2, authorizedEmail);

        Assert.Equal(test2, validDtoObj);
    }

    [Fact]
    public async Task UpdateUnauthorized()
    {
        Employee emp = new Employee(testEmployee1, employeeTypeDto);
        emp.EmployeeType = employeeType;
        List<Employee> employees = new List<Employee> { emp };
        RoleDto roleDto = new RoleDto(3, "Employee");
        EmployeeRoleDto empRoleDto = new EmployeeRoleDto(1, testEmployee1, roleDto);
        EmployeeRole empRole = new EmployeeRole(empRoleDto);
        List<EmployeeRole> empRoles = new List<EmployeeRole> { empRole };
        List<Role> roles = new List<Role> { new Role(roleDto) };
        EmployeeBanking bankingObj = new EmployeeBanking(test1);
        List<EmployeeBanking> bankingList = new List<EmployeeBanking>
        {
            bankingObj
        };
        var mockEmployees = employees;
        var unauthorizedEmail = "unauthorized.email@example.com";

        _mockUnitOfWork.Setup(x => x.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
                      .Returns(mockEmployees.AsQueryable().BuildMock());

        _mockUnitOfWork.Setup(x => x.EmployeeRole.Get(It.IsAny<Expression<Func<EmployeeRole, bool>>>()))
                       .Returns(empRoles.AsQueryable().BuildMock());

        _mockUnitOfWork.Setup(x => x.Role.Get(It.IsAny<Expression<Func<Role, bool>>>()))
                       .Returns(roles.AsQueryable().BuildMock());

        _mockUnitOfWork.Setup(x => x.EmployeeBanking.Get(It.IsAny<Expression<Func<EmployeeBanking, bool>>>()))
            .Returns(bankingList.AsQueryable().BuildMock());

        _mockUnitOfWork.Setup(x => x.EmployeeBanking.Update(It.IsAny<EmployeeBanking>()))
                       .ReturnsAsync(test2);

        var exception = await Assert.ThrowsAsync<Exception>(
            async () => await employeeBankingService.Update(test2, unauthorizedEmail));

        Assert.Equal("Unauthorized access", exception.Message);
    }

    [Fact]
    public async Task SavePass()
    {
        employeeTypeServiceMock.Setup(r => r.GetEmployeeType(employeeType.Name)).Returns(Task.FromResult(employeeTypeDto));
        EmployeeAddressDto employeeAddressDto = new EmployeeAddressDto(1, "2", "Complex", "2", "Suburb/District", "City", "Country", "Province", "1620");

        Employee employee = new Employee(testEmployee1, testEmployee1.EmployeeType);
        employee.EmployeeType = employeeType;
        employee.PhysicalAddress = new EmployeeAddress(employeeAddressDto);
        employee.PostalAddress = new EmployeeAddress(employeeAddressDto);

        List<Employee> employees = new List<Employee>
        {
        employee
        };
        var mockEmployees = employees;
        _mockUnitOfWork.Setup(u => u.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>())).Returns(mockEmployees.AsQueryable().BuildMock());
        _mockUnitOfWork.Setup(u => u.EmployeeBanking.Add(It.IsAny<EmployeeBanking>())).Returns(Task.FromResult(test1));
        var result = await employeeBankingService.Save(test1, "test@retrorabbit.co.za");

        Assert.Equal(test1, result);
    }

    [Fact]
    public async Task SaveByAdminPass()
    {
        Employee emp = new Employee(testEmployee1, employeeTypeDto);
        List<Employee> employees = new List<Employee> { emp };
        RoleDto roleDto = new RoleDto(2, "Admin");
        EmployeeRoleDto empRoleDto = new EmployeeRoleDto(1, testEmployee1, roleDto);
        EmployeeRole empRole = new EmployeeRole(empRoleDto);
        List<EmployeeRole> empRoles = new List<EmployeeRole> { empRole };
        List<Role> roles = new List<Role> { new Role(roleDto) };
        var mockEmployees = employees;
        var authorizedEmail = "admin.email@example.com";

        _mockUnitOfWork.Setup(x => x.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
                       .Returns(mockEmployees.AsQueryable().BuildMock());

        _mockUnitOfWork.Setup(x => x.EmployeeRole.Get(It.IsAny<Expression<Func<EmployeeRole, bool>>>()))
                       .Returns(empRoles.AsQueryable().BuildMock());

        _mockUnitOfWork.Setup(x => x.Role.Get(It.IsAny<Expression<Func<Role, bool>>>()))
                       .Returns(roles.AsQueryable().BuildMock());

        _mockUnitOfWork.Setup(x => x.EmployeeBanking.Add(It.IsAny<EmployeeBanking>()))
                       .ReturnsAsync(test1);

        var validDtoObj = await employeeBankingService.Save(test1, authorizedEmail);

        Assert.Equal(test1, validDtoObj);
    }

    [Fact]
    public async Task SaveUnauthorizedFail()
    {
        Employee emp = new Employee(testEmployee1, employeeTypeDto);
        List<Employee> employees = new List<Employee>{ emp };
        RoleDto roleDto = new RoleDto(3, "Employee");
        EmployeeRoleDto empRoleDto = new EmployeeRoleDto(1, testEmployee1, roleDto);
        EmployeeRole empRole = new EmployeeRole(empRoleDto);
        List<EmployeeRole> empRoles = new List<EmployeeRole> { empRole };
        List<Role> roles = new List<Role>{ new Role(roleDto) };
        var mockEmployees = employees;
        var unauthorizedUserEmail = "unauthorized@example.com";

        _mockUnitOfWork.Setup(x => x.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
                       .Returns(mockEmployees.AsQueryable().BuildMock());

        _mockUnitOfWork.Setup(x => x.EmployeeRole.Get(It.IsAny<Expression<Func<EmployeeRole, bool>>>()))
                       .Returns(empRoles.AsQueryable().BuildMock());

        _mockUnitOfWork.Setup(x => x.Role.Get(It.IsAny<Expression<Func<Role, bool>>>()))
                       .Returns(roles.AsQueryable().BuildMock());

        _mockUnitOfWork.Setup(x => x.EmployeeBanking.Add(It.IsAny<EmployeeBanking>()))
                       .ReturnsAsync(test1);

        var exception = await Assert.ThrowsAsync<Exception>(
            async () => await employeeBankingService.Save(test1, unauthorizedUserEmail));

        Assert.Equal("Unauthorized access", exception.Message);
    }

    [Fact]
    public async Task GetBankingPass()
    {
        int employeeId = 1;
        var banking = new EmployeeBanking(test1);

        var bankingQueryableMock = new List<EmployeeBanking> { banking }.AsQueryable().BuildMock();

        _mockUnitOfWork.Setup(u => u.EmployeeBanking.Get(It.IsAny<Expression<Func<EmployeeBanking, bool>>>()))
             .Returns(bankingQueryableMock);

        var result = await employeeBankingService.GetBanking(employeeId);

        Assert.NotNull(result);
        Assert.Equal(test1, result);
    }

    [Fact]
    public async Task GetBankingFail()
    {
        int employeeId = 2;
        var banking = new EmployeeBanking(test2);

        var bankingQueryableMock = new List<EmployeeBanking> { banking }.AsQueryable().BuildMock();

        _mockUnitOfWork.Setup(u => u.EmployeeBanking.Get(It.IsAny<Expression<Func<EmployeeBanking, bool>>>()))
             .Returns((IQueryable<EmployeeBanking>)null);

        await Assert.ThrowsAsync<Exception>(async () => await employeeBankingService.GetBanking(employeeId));
    }

    [Fact]
    public async Task SaveFail()
    {
        employeeTypeServiceMock.Setup(r => r.GetEmployeeType(employeeType.Name)).Returns(Task.FromResult(employeeTypeDto));

        Employee employee = new Employee(testEmployee1, testEmployee1.EmployeeType);
        employee.EmployeeType = employeeType;
        employee.PhysicalAddress = new EmployeeAddress(employeeAddressDto);
        employee.PostalAddress = new EmployeeAddress(employeeAddressDto);

        List<Employee> employees = new List<Employee>
    {
    employee
    };
        var mockEmployees = employees;
        _mockUnitOfWork.Setup(u => u.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>())).Returns(mockEmployees.AsQueryable().BuildMock());
        _mockUnitOfWork.Setup(u => u.EmployeeBanking.Add(It.IsAny<EmployeeBanking>())).Returns(Task.FromResult(test1));
        var result = await employeeBankingService.Save(test1, "test@retrorabbit.co.za");

        Assert.Equal(test1, result);
    }
}