using System.Linq.Expressions;
using MockQueryable.Moq;
using Moq;
using RGO.Services.Interfaces;
using RGO.Services.Services;
using RGO.Tests.Data.Models;
using RGO.UnitOfWork;
using RGO.UnitOfWork.Entities;
using Xunit;

namespace RGO.Services.Tests.Services;

public class EmployeeBankingServiceTest
{
    private readonly EmployeeBankingService _employeeBankingService;
    private readonly Mock<IEmployeeTypeService> _employeeTypeServiceMock;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;

    public EmployeeBankingServiceTest()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _employeeBankingService = new EmployeeBankingService(_mockUnitOfWork.Object);
        _employeeTypeServiceMock = new Mock<IEmployeeTypeService>();
    }

    [Fact]
    public async Task GetPendingReturnsPendingBankDtosPass()
    {
        _mockUnitOfWork
            .Setup(u => u.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
            .Returns(
                new List<Employee>
                {
                    new(EmployeeTd.EmployeeDto, EmployeeTypeTd.DeveloperType)
                }.AsQueryable().BuildMock());

        _mockUnitOfWork
            .Setup(u => u.EmployeeBanking.Get(It.IsAny<Expression<Func<EmployeeBanking, bool>>>()))
            .Returns(
                new List<EmployeeBanking>
                {
                    new(EmployeeBankingTd.EmployeeBankingDto)
                }.AsQueryable().BuildMock());

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

        _mockUnitOfWork
            .Setup(u => u.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
            .Returns(
                new List<Employee>
                {
                    new(EmployeeTd.EmployeeDto2, EmployeeTypeTd.DeveloperType)
                }.AsQueryable().BuildMock());

        _mockUnitOfWork
            .Setup(u => u.EmployeeBanking.Get(It.IsAny<Expression<Func<EmployeeBanking, bool>>>()))
            .Returns(
                new List<EmployeeBanking>
                {
                    new(EmployeeBankingTd.EmployeeBankingDto2)
                }.AsQueryable().BuildMock());


        _mockUnitOfWork
            .Setup(u => u.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
        _employeeTypeServiceMock
            .Setup(r => r.GetEmployeeType(EmployeeTypeTd.DeveloperType.Name))
            .ReturnsAsync(EmployeeTypeTd.DeveloperType);

        _mockUnitOfWork
            .Setup(u => u.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
            .Returns(
                new List<Employee>
                {
                    new(EmployeeTd.EmployeeDto, EmployeeTypeTd.DeveloperType)
                    {
                        EmployeeType = new EmployeeType(EmployeeTypeTd.DeveloperType),
                        PhysicalAddress = new EmployeeAddress(EmployeeAddressTd.EmployeeAddressDto),
                        PostalAddress = new EmployeeAddress(EmployeeAddressTd.EmployeeAddressDto)
                    }
                }.AsQueryable().BuildMock());

        _mockUnitOfWork
            .Setup(u => u.EmployeeBanking.Get(It.IsAny<Expression<Func<EmployeeBanking, bool>>>()))
            .Returns(
                new List<EmployeeBanking>
                {
                    new(EmployeeBankingTd.EmployeeBankingDto)
                }.AsQueryable().BuildMock());
    [Fact]
        var result = await _employeeBankingService.Update(EmployeeBankingTd.EmployeeBankingDto);
        EmployeeBankingDto test1 = new EmployeeBankingDto(
        1, 1, "FNB", "Not Sure", "120", EmployeeBankingAccountType.Savings, "Name1", BankApprovalStatus.PendingApproval, "", "asd", new DateOnly(), new DateOnly());

        EmployeeTypeDto employeeTypeDto = new EmployeeTypeDto(1, "Developer");
        EmployeeType employeeType = new EmployeeType(employeeTypeDto);
    public async Task SavePass()
        employeeTypeServiceMock.Setup(r => r.GetEmployeeType(employeeType.Name)).Returns(Task.FromResult(employeeTypeDto));
        _employeeTypeServiceMock
            .Setup(r => r.GetEmployeeType(EmployeeTypeTd.DeveloperType.Name))
            .ReturnsAsync(EmployeeTypeTd.DeveloperType);
        Employee employee = new Employee(testEmployee, testEmployee.EmployeeType);
        _mockUnitOfWork
            .Setup(u => u.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
            .Returns(
                new List<Employee>
                {
                    new(EmployeeTd.EmployeeDto, EmployeeTypeTd.DeveloperType)
                    {
                        EmployeeType = new EmployeeType(EmployeeTypeTd.DeveloperType),
                        PhysicalAddress = new EmployeeAddress(EmployeeAddressTd.EmployeeAddressDto),
                        PostalAddress = new EmployeeAddress(EmployeeAddressTd.EmployeeAddressDto)
                    }
                }.AsQueryable().BuildMock());

        _mockUnitOfWork
            .Setup(u => u.EmployeeBanking.Add(It.IsAny<EmployeeBanking>()))
            .ReturnsAsync(EmployeeBankingTd.EmployeeBankingDto);
        {
        var result = await _employeeBankingService.Save(EmployeeBankingTd.EmployeeBankingDto);
        Assert.Equal(EmployeeBankingTd.EmployeeBankingDto, result);
    }

        var result = await employeeBankingService.Save(test1);
        Assert.Equal(EmployeeBankingTd.EmployeeBankingDto, result);
    }

        _mockUnitOfWork
            .Setup(u => u.EmployeeBanking.FirstOrDefault(It.IsAny<Expression<Func<EmployeeBanking, bool>>>()))
            .ReturnsAsync(EmployeeBankingTd.EmployeeBankingDto);
        EmployeeRole empRole = new EmployeeRole(empRoleDto);
        List<EmployeeRole> empRoles = new List<EmployeeRole> { empRole };
        List<Role> roles = new List<Role> { new Role(roleDto) };
        var mockEmployees = employees;
        Assert.Equal(EmployeeBankingTd.EmployeeBankingDto, result);

        _mockUnitOfWork.Setup(x => x.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
                       .Returns(mockEmployees.AsQueryable().BuildMock());

        _mockUnitOfWork.Setup(x => x.EmployeeRole.Get(It.IsAny<Expression<Func<EmployeeRole, bool>>>()))
        _mockUnitOfWork
            .Setup(u => u.EmployeeBanking.FirstOrDefault(It.IsAny<Expression<Func<EmployeeBanking, bool>>>()))
            .Throws<Exception>();

        var validDtoObj = await employeeBankingService.Save(test1, authorizedEmail);

        Assert.Equal(test1, validDtoObj);
    }


        _employeeTypeServiceMock
            .Setup(r => r.GetEmployeeType(EmployeeTypeTd.DeveloperType.Name))
            .ReturnsAsync(EmployeeTypeTd.DeveloperType);

        _mockUnitOfWork
            .Setup(u => u.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
            .Returns(new List<Employee>
            {
                new(EmployeeTd.EmployeeDto, EmployeeTypeTd.DeveloperType)
                {
                    EmployeeType = new EmployeeType(EmployeeTypeTd.DeveloperType),
                    PhysicalAddress = new EmployeeAddress(EmployeeAddressTd.EmployeeAddressDto),
                    PostalAddress = new EmployeeAddress(EmployeeAddressTd.EmployeeAddressDto)
                }
            }.AsQueryable().BuildMock());

        _mockUnitOfWork
            .Setup(u => u.EmployeeBanking.Add(It.IsAny<EmployeeBanking>()))
            .ReturnsAsync(EmployeeBankingTd.EmployeeBankingDto);
    [Fact]
        var result = await _employeeBankingService.Save(EmployeeBankingTd.EmployeeBankingDto);
        var bankingQueryableMock = new List<EmployeeBanking> { banking }.AsQueryable().BuildMock();

        _mockUnitOfWork.Setup(u => u.EmployeeBanking.Get(It.IsAny<Expression<Func<EmployeeBanking, bool>>>()))
             .Returns((IQueryable<EmployeeBanking>)null);

        await Assert.ThrowsAsync<Exception>(async () => await _employeeBankingService.GetBanking(2));
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

        Assert.Equal(EmployeeBankingTd.EmployeeBankingDto, result);
    }
}