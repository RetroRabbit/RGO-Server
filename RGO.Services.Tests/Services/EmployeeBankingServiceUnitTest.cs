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
    private EmployeeBankingService employeeBankingService;

    public EmployeeBankingServiceTest()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _employeeBankingService = new Mock<IEmployeeBankingService>();
        employeeBankingService = new EmployeeBankingService(_mockUnitOfWork.Object);
    }

    [Fact]
    public async Task GetPendingReturnsPendingBankDtosPass()
    {


        EmployeeBankingDto test1 = new EmployeeBankingDto(
            1, 1, "FNB", "Not Sure", "120", EmployeeBankingAccountType.Savings, "Name1", BankApprovalStatus.PendingApproval, "", "asd");

        var pendingBankEntries = new List<EmployeeBankingDto> { test1 };

        EmployeeTypeDto employeeTypeDto = new EmployeeTypeDto(1, "Developer");
        EmployeeType employeeType = new EmployeeType(employeeTypeDto);
        EmployeeDto testEmployee = new(1, "001", "34434434", new DateOnly(), new DateOnly(),
        null, false, "None", 4, employeeTypeDto, "Notes", 1, 28, 128, 100000, "Mr", "Matt", "MT",
        "Schoeman", new DateOnly(), "South Africa", "South African", "0000080000000", " ",
        new DateOnly(), null, Race.Black, Gender.Male, null,
        "test@retrorabbit.co.za", "test.example@gmail.com", "0000000000");

        var employees = new List<Employee>
        {
            new Employee(testEmployee, employeeTypeDto)
        };
        var employeeBankings = new List<EmployeeBanking>
        {
            new EmployeeBanking(test1)
        };

        var mockEmployees = employees.AsQueryable().BuildMock();
        var mockEmployeeBankings = employeeBankings.AsQueryable().BuildMock();

        _mockUnitOfWork.Setup(u => u.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>())).Returns(mockEmployees);
        _mockUnitOfWork.Setup(u => u.EmployeeBanking.Get(It.IsAny<Expression<Func<EmployeeBanking, bool>>>())).Returns(mockEmployeeBankings);
        

        var result = await employeeBankingService.GetPending();

        Assert.Single(result);
    }

   /* [Fact]
    public async Task UpdateReturnsUpdateBankDtos()
    {

        var employeeTypeServiceMock = new Mock<IEmployeeTypeService>();

        EmployeeBankingDto test1 = new EmployeeBankingDto(
            1, 1, "FNB", "Not Sure", "120", EmployeeBankingAccountType.Savings, "Name1", BankApprovalStatus.PendingApproval, "", "asd");

        var pendingBankEntries = new List<EmployeeBankingDto> { test1 };

        EmployeeTypeDto employeeTypeDto = new EmployeeTypeDto(1, "Developer");
        EmployeeType employeeType = new EmployeeType(employeeTypeDto);

        EmployeeDto testEmployee = new(1, "001", "34434434", new DateOnly(), new DateOnly(),
        null, false, "None", 4, employeeTypeDto, "Notes", 1, 28, 128, 100000, "Mr", "Matt", "MT",
        "Schoeman", new DateOnly(), "South Africa", "South African", "0000080000000", " ",
        new DateOnly(), null, Race.Black, Gender.Male, null,
        "test@retrorabbit.co.za", "test.example@gmail.com", "0000000000");

        Assert.NotNull(testEmployee);
        Assert.NotNull(employeeTypeDto);

        var employees = new List<Employee>
        {
            new Employee(testEmployee, employeeTypeDto)
        };

        var empType = new List<EmployeeType>
        {
            new EmployeeType(employeeTypeDto)
        };

        var mockEmployees = employees.AsQueryable().BuildMock();

        employeeTypeServiceMock.Setup(r => r.GetEmployeeType(employeeTypeDto.Name)).Returns(Task.FromResult(employeeTypeDto));

        _mockUnitOfWork.Setup(u => u.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>())).Returns(mockEmployees);
        _mockUnitOfWork.Setup(u => u.EmployeeBanking.Update(It.IsAny<EmployeeBanking>()));
       // _mockUnitOfWork.Setup(u => u.EmployeeType.Any(It.IsAny<Expression<Func<EmployeeType, bool>>>())).Returns();
       // _mockUnitOfWork.Setup(u => u)
        var result = await employeeBankingService.UpdatePending(test1);

        Assert.Equal(test1, result);
    }*/
}

