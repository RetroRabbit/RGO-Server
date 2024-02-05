using MockQueryable.Moq;
using Moq;
using RGO.Models;
using RGO.Services.Interfaces;
using RGO.Services.Services;
using RGO.Tests.Data.Models;
using RGO.UnitOfWork;
using RGO.UnitOfWork.Entities;
using System.Linq.Expressions;
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
            .Returns(
                new List<Employee>
                {
                    new(EmployeeTd.EmployeeDto3, EmployeeTypeTd.DeveloperType)
                }.AsQueryable().BuildMock());

        _mockUnitOfWork
            .Setup(u => u.EmployeeBanking.Get(It.IsAny<Expression<Func<EmployeeBanking, bool>>>()))
            .Returns(
                new List<EmployeeBanking>
                {
                    new(EmployeeBankingTd.EmployeeBankingDto3)
                }.AsQueryable().BuildMock());

        var result = await _employeeBankingService.Get(0);
        Assert.Single(result);

        result = await _employeeBankingService.Get(1);
        Assert.Single(result);

        result = await _employeeBankingService.Get(2);
        Assert.Single(result);
    }

    [Fact]
    public async Task UpdateReturnsUpdateBankDtos()
    {
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

        var result = await _employeeBankingService.Update(EmployeeBankingTd.EmployeeBankingDto, "test@retrorabbit.co.za");

        Assert.Equal(EmployeeBankingTd.EmployeeBankingDto, result);
    }

    [Fact]
    public async Task UpdateByAdminReturnsUpdateBankDtos()
    {
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


        List<EmployeeRole> empRoles = new List<EmployeeRole> { 
            new EmployeeRole(
                new EmployeeRoleDto(1, EmployeeTd.EmployeeDto, new RoleDto(2, "Admin"))
            )};

        List<Role> roles = new List<Role> { new Role( new RoleDto(2, "Admin") ) };

        _mockUnitOfWork
            .Setup(x => x.EmployeeRole.Get(It.IsAny<Expression<Func<EmployeeRole, bool>>>()))
            .Returns(empRoles.AsQueryable().BuildMock());

        _mockUnitOfWork
            .Setup(x => x.Role.Get(It.IsAny<Expression<Func<Role, bool>>>()))
            .Returns(roles.AsQueryable().BuildMock());

        var result = await _employeeBankingService.Update(EmployeeBankingTd.EmployeeBankingDto, "admin.email@example.com");

        Assert.Equal(EmployeeBankingTd.EmployeeBankingDto, result);
    }

    [Fact]
    public async Task UpdateByPassReturnsUpdateBankDtos()
    {
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


        List<EmployeeRole> empRoles = new List<EmployeeRole> { 
            new EmployeeRole(
                new EmployeeRoleDto(1, EmployeeTd.EmployeeDto, new RoleDto(3, "Employee"))
            )};

        List<Role> roles = new List<Role> { new Role( new RoleDto(3, "Employee") ) };

        _mockUnitOfWork
            .Setup(x => x.EmployeeRole.Get(It.IsAny<Expression<Func<EmployeeRole, bool>>>()))
            .Returns(empRoles.AsQueryable().BuildMock());

        _mockUnitOfWork
            .Setup(x => x.Role.Get(It.IsAny<Expression<Func<Role, bool>>>()))
            .Returns(roles.AsQueryable().BuildMock());

        var exception = await Assert.ThrowsAsync<Exception>(
            async () => await _employeeBankingService.Update(EmployeeBankingTd.EmployeeBankingDto, "unauthorized.email@example.com"));

        Assert.Equal("Unauthorized access", exception.Message);
    }

    [Fact]
    public async Task SavePass()
    {
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
            .Setup(u => u.EmployeeBanking.Add(It.IsAny<EmployeeBanking>()))
            .ReturnsAsync(EmployeeBankingTd.EmployeeBankingDto);

        var result = await _employeeBankingService.Save(EmployeeBankingTd.EmployeeBankingDto, "test@retrorabbit.co.za");

        Assert.Equal(EmployeeBankingTd.EmployeeBankingDto, result);
    }

    [Fact]
    public async Task SaveByAdminPass()
    {
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
            .Setup(u => u.EmployeeBanking.Add(It.IsAny<EmployeeBanking>()))
            .ReturnsAsync(EmployeeBankingTd.EmployeeBankingDto);

        List<EmployeeRole> empRoles = new List<EmployeeRole> {
            new EmployeeRole(
                new EmployeeRoleDto(1, EmployeeTd.EmployeeDto, new RoleDto(2, "Admin"))
            )};

        List<Role> roles = new List<Role> { new Role(new RoleDto(2, "Admin")) };

        _mockUnitOfWork
            .Setup(x => x.EmployeeRole.Get(It.IsAny<Expression<Func<EmployeeRole, bool>>>()))
            .Returns(empRoles.AsQueryable().BuildMock());

        _mockUnitOfWork
            .Setup(x => x.Role.Get(It.IsAny<Expression<Func<Role, bool>>>()))
            .Returns(roles.AsQueryable().BuildMock());

        var result = await _employeeBankingService.Save(EmployeeBankingTd.EmployeeBankingDto, "admin.email@example.com");

        Assert.NotNull(result);
        Assert.Equal(EmployeeBankingTd.EmployeeBankingDto, result);

    }
    [Fact]
    public async Task SaveUnauthorizedPass()
    {
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
            .Setup(u => u.EmployeeBanking.Add(It.IsAny<EmployeeBanking>()))
            .ReturnsAsync(EmployeeBankingTd.EmployeeBankingDto);

        List<EmployeeRole> empRoles = new List<EmployeeRole> {
            new EmployeeRole(
                new EmployeeRoleDto(1, EmployeeTd.EmployeeDto, new RoleDto(3, "Employee"))
            )};

        List<Role> roles = new List<Role> { new Role(new RoleDto(3, "Employee")) };

        _mockUnitOfWork
            .Setup(x => x.EmployeeRole.Get(It.IsAny<Expression<Func<EmployeeRole, bool>>>()))
            .Returns(empRoles.AsQueryable().BuildMock());

        _mockUnitOfWork
            .Setup(x => x.Role.Get(It.IsAny<Expression<Func<Role, bool>>>()))
            .Returns(roles.AsQueryable().BuildMock());

        var exception = await Assert.ThrowsAsync<Exception>(
            async () => await _employeeBankingService.Save(EmployeeBankingTd.EmployeeBankingDto, "unauthorized.email@example.com"));

        Assert.Equal("Unauthorized access", exception.Message);
    }

    [Fact]
    public async Task GetBankingPass()
    {
        _mockUnitOfWork
            .Setup(u => u.EmployeeBanking.FirstOrDefault(It.IsAny<Expression<Func<EmployeeBanking, bool>>>()))
            .ReturnsAsync(EmployeeBankingTd.EmployeeBankingDto);

        var result = await _employeeBankingService.GetBanking(1);

        Assert.NotNull(result);
        Assert.Equal(EmployeeBankingTd.EmployeeBankingDto, result);
    }

    [Fact]
    public async Task GetBankingFail()
    {
        _mockUnitOfWork
            .Setup(u => u.EmployeeBanking.FirstOrDefault(It.IsAny<Expression<Func<EmployeeBanking, bool>>>()))
            .Throws<Exception>();

        await Assert.ThrowsAsync<Exception>(async () => await _employeeBankingService.GetBanking(2));
    }

    [Fact]
    public async Task SaveFail()
    {
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

        var result = await _employeeBankingService.Save(EmployeeBankingTd.EmployeeBankingDto, "test@retrorabbit.co.za");

        Assert.Equal(EmployeeBankingTd.EmployeeBankingDto, result);
    }
}