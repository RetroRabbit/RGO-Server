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


        EmployeeBankingDto test1 = new EmployeeBankingDto(
        1, 1, "FNB", "Not Sure", "120", EmployeeBankingAccountType.Savings, "Name1", BankApprovalStatus.PendingApproval, "", "asd", new DateOnly(), new DateOnly());
        EmployeeBankingDto test2 = new EmployeeBankingDto(
        2, 2, "FNB", "Not Sure", "120", EmployeeBankingAccountType.Savings, "Name1", BankApprovalStatus.Approved, "", "asd", new DateOnly(), new DateOnly());
        EmployeeBankingDto test3 = new EmployeeBankingDto(
        3, 3, "FNB", "Not Sure", "120", EmployeeBankingAccountType.Savings, "Name1", BankApprovalStatus.Declined, "", "asd", new DateOnly(), new DateOnly());


        var pendingBankEntries = new List<EmployeeBankingDto> { test1 };

        EmployeeTypeDto employeeTypeDto = new EmployeeTypeDto(1, "Developer");
        EmployeeType employeeType = new EmployeeType(employeeTypeDto);
        EmployeeAddressDto employeeAddressDto = new EmployeeAddressDto(1, "2", "Complex", "2", "Suburb/District", "City", "Country", "Province", "1620");

        EmployeeDto testEmployee1 = new(1, "001", "34434434", new DateTime(), new DateTime(),
        null, false, "None", 4, employeeTypeDto, "Notes", 1, 28, 128, 100000, "Matt", "MT",
        "Schoeman", new DateTime(), "South Africa", "South African", "0000080000000", " ",
        new DateTime(), null, Race.Black, Gender.Male, null,
        "test@retrorabbit.co.za", "test.example@gmail.com", "0000000000", null, null, employeeAddressDto, employeeAddressDto, null, null, null);

        EmployeeDto testEmployee2 = new(2, "001", "34434434", new DateTime(), new DateTime(),
        null, false, "None", 4, employeeTypeDto, "Notes", 1, 28, 128, 100000, "Matt", "MT",
        "Schoeman", new DateTime(), "South Africa", "South African", "0000080000000", " ",
        new DateTime(), null, Race.Black, Gender.Male, null,
        "test@retrorabbit.co.za", "test.example@gmail.com", "0000000000", null, null, employeeAddressDto, employeeAddressDto, null, null, null);

        EmployeeDto testEmployee3 = new(3, "001", "34434434", new DateTime(), new DateTime(),
        null, false, "None", 4, employeeTypeDto, "Notes", 1, 28, 128, 100000, "Matt", "MT",
        "Schoeman", new DateTime(), "South Africa", "South African", "0000080000000", " ",
        new DateTime(), null, Race.Black, Gender.Male, null,
        "test@retrorabbit.co.za", "test.example@gmail.com", "0000000000", null, null, employeeAddressDto, employeeAddressDto, null, null, null);

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
        EmployeeBankingDto test1 = new EmployeeBankingDto(
        1, 1, "FNB", "Not Sure", "120", EmployeeBankingAccountType.Savings, "Name1", BankApprovalStatus.PendingApproval, "", "asd", new DateOnly(), new DateOnly());

        EmployeeTypeDto employeeTypeDto = new EmployeeTypeDto(1, "Developer");
        EmployeeType employeeType = new EmployeeType(employeeTypeDto);

        employeeTypeServiceMock.Setup(r => r.GetEmployeeType(employeeType.Name)).Returns(Task.FromResult(employeeTypeDto));
        EmployeeAddressDto employeeAddressDto = new EmployeeAddressDto(1, "2", "Complex", "2", "Suburb/District", "City", "Country", "Province", "1620");

        EmployeeDto testEmployee = new(1, "001", "34434434", new DateTime(), new DateTime(),
        null, false, "None", 4, employeeTypeDto, "Notes", 1, 28, 128, 100000, "Matt", "MT",
        "Schoeman", new DateTime(), "South Africa", "South African", "0000080000000", " ",
        new DateTime(), null, Race.Black, Gender.Male, null,
        "test@retrorabbit.co.za", "test.example@gmail.com", "0000000000", null, null, employeeAddressDto, employeeAddressDto, null, null, null);

        Employee employee = new Employee(testEmployee, testEmployee.EmployeeType);
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
        var result = await employeeBankingService.Update(test1);

        Assert.Equal(test1, result);
    }

    [Fact]
    public async Task SavePass()
    {
        EmployeeBankingDto test1 = new EmployeeBankingDto(
         1, 1, "FNB", "Not Sure", "120", EmployeeBankingAccountType.Savings, "Name1", BankApprovalStatus.PendingApproval, "file", "asd", new DateOnly(), new DateOnly());

        EmployeeTypeDto employeeTypeDto = new EmployeeTypeDto(1, "Developer");
        EmployeeType employeeType = new EmployeeType(employeeTypeDto);

        employeeTypeServiceMock.Setup(r => r.GetEmployeeType(employeeType.Name)).Returns(Task.FromResult(employeeTypeDto));
        EmployeeAddressDto employeeAddressDto = new EmployeeAddressDto(1, "2", "Complex", "2", "Suburb/District", "City", "Country", "Province", "1620");

        EmployeeDto testEmployee = new(1, "001", "34434434", new DateTime(), new DateTime(),
        null, false, "None", 4, employeeTypeDto, "Notes", 1, 28, 128, 100000, "Matt", "MT",
        "Schoeman", new DateTime(), "South Africa", "South African", "0000080000000", " ",
        new DateTime(), null, Race.Black, Gender.Male, null,
        "test@retrorabbit.co.za", "test.example@gmail.com", "0000000000", null, null, employeeAddressDto, employeeAddressDto, null, null, null);

        Employee employee = new Employee(testEmployee, testEmployee.EmployeeType);
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
        var result = await employeeBankingService.Save(test1);

        Assert.Equal(test1, result);
    }


    [Fact]
    public async Task GetBankingPass()
    {
        int employeeId = 1;
        var expectedBankingDto = new EmployeeBankingDto(
         1, 1, "FNB", "Not Sure", "120", EmployeeBankingAccountType.Savings, "Name1", BankApprovalStatus.PendingApproval, "file", "asd", new DateOnly(), new DateOnly());
        var banking = new EmployeeBanking(expectedBankingDto);

        var bankingQueryableMock = new List<EmployeeBanking> { banking }.AsQueryable().BuildMock();

        _mockUnitOfWork.Setup(u => u.EmployeeBanking.Get(It.IsAny<Expression<Func<EmployeeBanking, bool>>>()))
             .Returns(bankingQueryableMock);

        var result = await employeeBankingService.GetBanking(employeeId);

        Assert.NotNull(result);
        Assert.Equal(expectedBankingDto, result);
    }

    [Fact]
    public async Task GetBankingFail()
    {
        int employeeId = 2;

        var expectedBankingDto = new EmployeeBankingDto(
        1, 1, "FNB", "Not Sure", "120", EmployeeBankingAccountType.Savings, "Name1", BankApprovalStatus.PendingApproval, "file", "asd", new DateOnly(), new DateOnly());
        var banking = new EmployeeBanking(expectedBankingDto);

        var bankingQueryableMock = new List<EmployeeBanking> { banking }.AsQueryable().BuildMock();

        _mockUnitOfWork.Setup(u => u.EmployeeBanking.Get(It.IsAny<Expression<Func<EmployeeBanking, bool>>>()))
             .Returns((IQueryable<EmployeeBanking>)null);

        await Assert.ThrowsAsync<Exception>(async () => await employeeBankingService.GetBanking(employeeId));
    }

    [Fact]
    public async Task SaveFail()
    {
        EmployeeBankingDto test1 = new EmployeeBankingDto(
         1, 1, "FNB", "Not Sure", "120", EmployeeBankingAccountType.Savings, "Name1", BankApprovalStatus.PendingApproval, "file", "asd", new DateOnly(), new DateOnly());

        EmployeeTypeDto employeeTypeDto = new EmployeeTypeDto(1, "Developer");
        EmployeeType employeeType = new EmployeeType(employeeTypeDto);

        employeeTypeServiceMock.Setup(r => r.GetEmployeeType(employeeType.Name)).Returns(Task.FromResult(employeeTypeDto));
        EmployeeAddressDto employeeAddressDto = new EmployeeAddressDto(1, "2", "Complex", "2", "Suburb/District", "City", "Country", "Province", "1620");

        EmployeeDto testEmployee = new(1, "001", "34434434", new DateTime(), new DateTime(),
        null, false, "None", 4, employeeTypeDto, "Notes", 1, 28, 128, 100000, "Matt", "MT",
        "Schoeman", new DateTime(), "South Africa", "South African", "0000080000000", " ",
        new DateTime(), null, Race.Black, Gender.Male, null,
        "test@retrorabbit.co.za", "test.example@gmail.com", "0000000000", null, null, employeeAddressDto, employeeAddressDto, null, null, null);

        Employee employee = new Employee(testEmployee, testEmployee.EmployeeType);
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
        var result = await employeeBankingService.Save(test1);

        Assert.Equal(test1, result);
    }
}