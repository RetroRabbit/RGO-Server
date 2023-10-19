using Xunit;
using Moq;
using Microsoft.EntityFrameworkCore;
using RGO.Models;
using RGO.Models.Enums;
using RGO.UnitOfWork;
using RGO.Services.Services;
using System.Collections.Generic;
using System.Linq;
using RGO.UnitOfWork.Entities;
using System.Linq.Expressions;
using RGO.Services.Interfaces;
using MockQueryable.Moq;


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
            1, 1, "FNB", "Not Sure", "120", EmployeeBankingAccountType.Savings, "Name1", BankApprovalStatus.PendingApproval, "", "asd");
        EmployeeBankingDto test2 = new EmployeeBankingDto(
            2, 2, "FNB", "Not Sure", "120", EmployeeBankingAccountType.Savings, "Name1", BankApprovalStatus.Approved, "", "asd");
        EmployeeBankingDto test3 = new EmployeeBankingDto(
            3, 3, "FNB", "Not Sure", "120", EmployeeBankingAccountType.Savings, "Name1", BankApprovalStatus.Declined, "", "asd");


        var pendingBankEntries = new List<EmployeeBankingDto> { test1 };

        EmployeeTypeDto employeeTypeDto = new EmployeeTypeDto(1, "Developer");
        EmployeeType employeeType = new EmployeeType(employeeTypeDto);
        
        EmployeeDto testEmployee1 = new(1, "001", "34434434", new DateOnly(), new DateOnly(),
        null, false, "None", 4, employeeTypeDto, "Notes", 1, 28, 128, 100000, "Mr", "Matt", "MT",
        "Schoeman", new DateOnly(), "South Africa", "South African", "0000080000000", " ",
        new DateOnly(), null, Race.Black, Gender.Male, null,
        "test@retrorabbit.co.za", "test.example@gmail.com", "0000000000", null, null);

        EmployeeDto testEmployee2 = new(2, "001", "34434434", new DateOnly(), new DateOnly(),
        null, false, "None", 4, employeeTypeDto, "Notes", 1, 28, 128, 100000, "Mr", "Matt", "MT",
        "Schoeman", new DateOnly(), "South Africa", "South African", "0000080000000", " ",
        new DateOnly(), null, Race.Black, Gender.Male, null,
        "test@retrorabbit.co.za", "test.example@gmail.com", "0000000000", null, null);

        EmployeeDto testEmployee3 = new(3, "001", "34434434", new DateOnly(), new DateOnly(),
        null, false, "None", 4, employeeTypeDto, "Notes", 1, 28, 128, 100000, "Mr", "Matt", "MT",
        "Schoeman", new DateOnly(), "South Africa", "South African", "0000080000000", " ",
        new DateOnly(), null, Race.Black, Gender.Male, null,
        "test@retrorabbit.co.za", "test.example@gmail.com", "0000000000", null, null);

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
            1, 1, "FNB", "Not Sure", "120", EmployeeBankingAccountType.Savings, "Name1", BankApprovalStatus.PendingApproval, "", "asd");

        EmployeeTypeDto employeeTypeDto = new EmployeeTypeDto(1, "Developer");
        EmployeeType employeeType = new EmployeeType(employeeTypeDto);

        employeeTypeServiceMock.Setup(r => r.GetEmployeeType(employeeType.Name)).Returns(Task.FromResult(employeeTypeDto));
        
        EmployeeDto testEmployee = new(1, "001", "34434434", new DateOnly(), new DateOnly(),
        null, false, "None", 4, employeeTypeDto, "Notes", 1, 28, 128, 100000, "Mr", "Matt", "MT",
        "Schoeman", new DateOnly(), "South Africa", "South African", "0000080000000", " ",
        new DateOnly(), null, Race.Black, Gender.Male, null,
        "test@retrorabbit.co.za", "test.example@gmail.com", "0000000000", null, null);

        Employee employee = new Employee(testEmployee, testEmployee.EmployeeType);
        employee.EmployeeType = employeeType;
        List<Employee> employees = new List<Employee>();
        employees.Add(employee);
        var mockEmployees = employees;

        _mockUnitOfWork.Setup(u => u.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>())).Returns(mockEmployees.AsQueryable().BuildMock());
        _mockUnitOfWork.Setup(u => u.EmployeeBanking.Update(It.IsAny<EmployeeBanking>()));
        var result = await employeeBankingService.Update(test1);

        Assert.Equal(test1, result);
    }
}

