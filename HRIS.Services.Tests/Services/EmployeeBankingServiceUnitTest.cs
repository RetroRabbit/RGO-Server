using System.Linq.Expressions;
using HRIS.Models;
using HRIS.Services.Interfaces;
using HRIS.Services.Services;
using MockQueryable.Moq;
using Moq;
using RR.Tests.Data.Models.HRIS;
using RR.UnitOfWork;
using RR.UnitOfWork.Entities.HRIS;
using Xunit;

namespace HRIS.Services.Tests.Services;

public class EmployeeBankingServiceTest
{
    private readonly EmployeeBankingService _employeeBankingService;
    private readonly Mock<IEmployeeTypeService> _employeeTypeServiceMock;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IErrorLoggingService> _errorLoggingServiceMock;
    public EmployeeBankingServiceTest()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _errorLoggingServiceMock = new Mock<IErrorLoggingService>();
        _employeeBankingService = new EmployeeBankingService(_mockUnitOfWork.Object,_errorLoggingServiceMock.Object);
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
                         new(EmployeeTestData.EmployeeDto, EmployeeTypeTestData.DeveloperType)
                     }.AsQueryable().BuildMock());

        _mockUnitOfWork
            .Setup(u => u.EmployeeBanking.Get(It.IsAny<Expression<Func<EmployeeBanking, bool>>>()))
            .Returns(
                     new List<EmployeeBanking>
                     {
                         new(EmployeeBankingTestData.EmployeeBankingDto)
                     }.AsQueryable().BuildMock());

        _mockUnitOfWork
            .Setup(u => u.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
            .Returns(
                     new List<Employee>
                     {
                         new(EmployeeTestData.EmployeeDto2, EmployeeTypeTestData.DeveloperType)
                     }.AsQueryable().BuildMock());

        _mockUnitOfWork
            .Setup(u => u.EmployeeBanking.Get(It.IsAny<Expression<Func<EmployeeBanking, bool>>>()))
            .Returns(
                     new List<EmployeeBanking>
                     {
                         new(EmployeeBankingTestData.EmployeeBankingDto2)
                     }.AsQueryable().BuildMock());


        _mockUnitOfWork
            .Setup(u => u.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
            .Returns(
                     new List<Employee>
                     {
                         new(EmployeeTestData.EmployeeDto3, EmployeeTypeTestData.DeveloperType)
                     }.AsQueryable().BuildMock());

        _mockUnitOfWork
            .Setup(u => u.EmployeeBanking.Get(It.IsAny<Expression<Func<EmployeeBanking, bool>>>()))
            .Returns(
                     new List<EmployeeBanking>
                     {
                         new(EmployeeBankingTestData.EmployeeBankingDto3)
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
            .Setup(r => r.GetEmployeeType(EmployeeTypeTestData.DeveloperType.Name))
            .ReturnsAsync(EmployeeTypeTestData.DeveloperType);

        _mockUnitOfWork
            .Setup(u => u.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
            .Returns(
                     new List<Employee>
                     {
                         new(EmployeeTestData.EmployeeDto, EmployeeTypeTestData.DeveloperType)
                         {
                             EmployeeType = new EmployeeType(EmployeeTypeTestData.DeveloperType),
                             PhysicalAddress = new EmployeeAddress(EmployeeAddressTestData.EmployeeAddressDto),
                             PostalAddress = new EmployeeAddress(EmployeeAddressTestData.EmployeeAddressDto)
                         }
                     }.AsQueryable().BuildMock());

        _mockUnitOfWork
            .Setup(u => u.EmployeeBanking.Get(It.IsAny<Expression<Func<EmployeeBanking, bool>>>()))
            .Returns(
                     new List<EmployeeBanking>
                     {
                         new(EmployeeBankingTestData.EmployeeBankingDto)
                     }.AsQueryable().BuildMock());

        var result =
            await _employeeBankingService.Update(EmployeeBankingTestData.EmployeeBankingDto, "test@retrorabbit.co.za");

        Assert.Equal(EmployeeBankingTestData.EmployeeBankingDto, result);
    }

    [Fact]
    public async Task UpdateByAdminReturnsUpdateBankDtos()
    {
        _employeeTypeServiceMock
            .Setup(r => r.GetEmployeeType(EmployeeTypeTestData.DeveloperType.Name))
            .ReturnsAsync(EmployeeTypeTestData.DeveloperType);

        _mockUnitOfWork
            .Setup(u => u.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
            .Returns(
                     new List<Employee>
                     {
                         new(EmployeeTestData.EmployeeDto, EmployeeTypeTestData.DeveloperType)
                         {
                             EmployeeType = new EmployeeType(EmployeeTypeTestData.DeveloperType),
                             PhysicalAddress = new EmployeeAddress(EmployeeAddressTestData.EmployeeAddressDto),
                             PostalAddress = new EmployeeAddress(EmployeeAddressTestData.EmployeeAddressDto)
                         }
                     }.AsQueryable().BuildMock());

        _mockUnitOfWork
            .Setup(u => u.EmployeeBanking.Get(It.IsAny<Expression<Func<EmployeeBanking, bool>>>()))
            .Returns(
                     new List<EmployeeBanking>
                     {
                         new(EmployeeBankingTestData.EmployeeBankingDto)
                     }.AsQueryable().BuildMock());


        var empRoles = new List<EmployeeRole>
        {
            new(
                new EmployeeRoleDto{ Id = 1, Employee = EmployeeTestData.EmployeeDto, Role = EmployeeRoleTestData.RoleDtoAdmin }
               )
        };

        var roles = new List<Role> { new(EmployeeRoleTestData.RoleDtoAdmin) };

        _mockUnitOfWork
            .Setup(x => x.EmployeeRole.Get(It.IsAny<Expression<Func<EmployeeRole, bool>>>()))
            .Returns(empRoles.AsQueryable().BuildMock());

        _mockUnitOfWork
            .Setup(x => x.Role.Get(It.IsAny<Expression<Func<Role, bool>>>()))
            .Returns(roles.AsQueryable().BuildMock());

        var result =
            await _employeeBankingService.Update(EmployeeBankingTestData.EmployeeBankingDto, "admin.email@example.com");

        Assert.Equal(EmployeeBankingTestData.EmployeeBankingDto, result);
    }

    [Fact]
    public async Task UpdateByPassReturnsUpdateBankDtos()
    {
        _employeeTypeServiceMock
            .Setup(r => r.GetEmployeeType(EmployeeTypeTestData.DeveloperType.Name))
            .ReturnsAsync(EmployeeTypeTestData.DeveloperType);

        _mockUnitOfWork
            .Setup(u => u.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
            .Returns(
                     new List<Employee>
                     {
                         new(EmployeeTestData.EmployeeDto, EmployeeTypeTestData.DeveloperType)
                         {
                             EmployeeType = new EmployeeType(EmployeeTypeTestData.DeveloperType),
                             PhysicalAddress = new EmployeeAddress(EmployeeAddressTestData.EmployeeAddressDto),
                             PostalAddress = new EmployeeAddress(EmployeeAddressTestData.EmployeeAddressDto)
                         }
                     }.AsQueryable().BuildMock());

        _mockUnitOfWork
            .Setup(u => u.EmployeeBanking.Get(It.IsAny<Expression<Func<EmployeeBanking, bool>>>()))
            .Returns(
                     new List<EmployeeBanking>
                     {
                         new(EmployeeBankingTestData.EmployeeBankingDto)
                     }.AsQueryable().BuildMock());


        var empRoles = new List<EmployeeRole>
        {
            new(
                new EmployeeRoleDto{ Id = 1, Employee = EmployeeTestData.EmployeeDto, Role = EmployeeRoleTestData.RoleDtoEmployee }
               )
        };

        var roles = new List<Role>
        {
            new(
                EmployeeRoleTestData.RoleDtoEmployee
               )
        };

        _mockUnitOfWork
            .Setup(x => x.EmployeeRole.Get(It.IsAny<Expression<Func<EmployeeRole, bool>>>()))
            .Returns(empRoles.AsQueryable().BuildMock());

        _mockUnitOfWork
            .Setup(x => x.Role.Get(It.IsAny<Expression<Func<Role, bool>>>()))
            .Returns(roles.AsQueryable().BuildMock());

        _errorLoggingServiceMock.Setup(r => r.LogException(It.IsAny<Exception>())).Throws(new Exception("Unauthorized access"));

        var exception = await Assert.ThrowsAsync<Exception>(
                                                            async () =>
                                                                await _employeeBankingService
                                                                    .Update(EmployeeBankingTestData.EmployeeBankingDto,
                                                                            "unauthorized.email@example.com"));

        Assert.Equal("Unauthorized access", exception.Message);
    }

    [Fact]
    public async Task SavePass()
    {
        _employeeTypeServiceMock
            .Setup(r => r.GetEmployeeType(EmployeeTypeTestData.DeveloperType.Name))
            .ReturnsAsync(EmployeeTypeTestData.DeveloperType);

        _mockUnitOfWork
            .Setup(u => u.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
            .Returns(
                     new List<Employee>
                     {
                         new(EmployeeTestData.EmployeeDto, EmployeeTypeTestData.DeveloperType)
                         {
                             EmployeeType = new EmployeeType(EmployeeTypeTestData.DeveloperType),
                             PhysicalAddress = new EmployeeAddress(EmployeeAddressTestData.EmployeeAddressDto),
                             PostalAddress = new EmployeeAddress(EmployeeAddressTestData.EmployeeAddressDto)
                         }
                     }.AsQueryable().BuildMock());

        _mockUnitOfWork
            .Setup(u => u.EmployeeBanking.Add(It.IsAny<EmployeeBanking>()))
            .ReturnsAsync(EmployeeBankingTestData.EmployeeBankingDto);

        var result =
            await _employeeBankingService.Save(EmployeeBankingTestData.EmployeeBankingDto, "test@retrorabbit.co.za");

        Assert.Equal(EmployeeBankingTestData.EmployeeBankingDto, result);
    }

    [Fact]
    public async Task SaveByAdminPass()
    {
        _employeeTypeServiceMock
            .Setup(r => r.GetEmployeeType(EmployeeTypeTestData.DeveloperType.Name))
            .ReturnsAsync(EmployeeTypeTestData.DeveloperType);

        _mockUnitOfWork
            .Setup(u => u.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
            .Returns(
                     new List<Employee>
                     {
                         new(EmployeeTestData.EmployeeDto, EmployeeTypeTestData.DeveloperType)
                         {
                             EmployeeType = new EmployeeType(EmployeeTypeTestData.DeveloperType),
                             PhysicalAddress = new EmployeeAddress(EmployeeAddressTestData.EmployeeAddressDto),
                             PostalAddress = new EmployeeAddress(EmployeeAddressTestData.EmployeeAddressDto)
                         }
                     }.AsQueryable().BuildMock());

        _mockUnitOfWork
            .Setup(u => u.EmployeeBanking.Add(It.IsAny<EmployeeBanking>()))
            .ReturnsAsync(EmployeeBankingTestData.EmployeeBankingDto);

        var empRoles = new List<EmployeeRole>
        {
            new(
                new EmployeeRoleDto{ Id = 1, Employee = EmployeeTestData.EmployeeDto, Role = EmployeeRoleTestData.RoleDtoAdmin }
               )
        };

        var roles = new List<Role> { new(EmployeeRoleTestData.RoleDtoAdmin) };

        _mockUnitOfWork
            .Setup(x => x.EmployeeRole.Get(It.IsAny<Expression<Func<EmployeeRole, bool>>>()))
            .Returns(empRoles.AsQueryable().BuildMock());

        _mockUnitOfWork
            .Setup(x => x.Role.Get(It.IsAny<Expression<Func<Role, bool>>>()))
            .Returns(roles.AsQueryable().BuildMock());

        var result =
            await _employeeBankingService.Save(EmployeeBankingTestData.EmployeeBankingDto, "admin.email@example.com");

        Assert.NotNull(result);
        Assert.Equal(EmployeeBankingTestData.EmployeeBankingDto, result);
    }

    [Fact]
    public async Task SaveUnauthorizedPass()
    {
        _employeeTypeServiceMock
            .Setup(r => r.GetEmployeeType(EmployeeTypeTestData.DeveloperType.Name))
            .ReturnsAsync(EmployeeTypeTestData.DeveloperType);

        _mockUnitOfWork
            .Setup(u => u.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
            .Returns(
                     new List<Employee>
                     {
                         new(EmployeeTestData.EmployeeDto, EmployeeTypeTestData.DeveloperType)
                         {
                             EmployeeType = new EmployeeType(EmployeeTypeTestData.DeveloperType),
                             PhysicalAddress = new EmployeeAddress(EmployeeAddressTestData.EmployeeAddressDto),
                             PostalAddress = new EmployeeAddress(EmployeeAddressTestData.EmployeeAddressDto)
                         }
                     }.AsQueryable().BuildMock());

        _mockUnitOfWork
            .Setup(u => u.EmployeeBanking.Add(It.IsAny<EmployeeBanking>()))
            .ReturnsAsync(EmployeeBankingTestData.EmployeeBankingDto);

        var empRoles = new List<EmployeeRole>
        {
            new(
                new EmployeeRoleDto{ Id = 1, Employee = EmployeeTestData.EmployeeDto, Role = EmployeeRoleTestData.RoleDtoEmployee }
               )
        };

        var roles = new List<Role>
        {
            new(
                EmployeeRoleTestData.RoleDtoEmployee
               )
        };

        _mockUnitOfWork
            .Setup(x => x.EmployeeRole.Get(It.IsAny<Expression<Func<EmployeeRole, bool>>>()))
            .Returns(empRoles.AsQueryable().BuildMock());

        _mockUnitOfWork
            .Setup(x => x.Role.Get(It.IsAny<Expression<Func<Role, bool>>>()))
            .Returns(roles.AsQueryable().BuildMock());

        _errorLoggingServiceMock.Setup(r => r.LogException(It.IsAny<Exception>())).Throws(new Exception("Unauthorized access"));

        var exception = await Assert.ThrowsAsync<Exception>(
                                                            async () =>
                                                                await _employeeBankingService
                                                                    .Save(EmployeeBankingTestData.EmployeeBankingDto,
                                                                          "unauthorized.email@example.com"));

        Assert.Equal("Unauthorized access", exception.Message);
    }

    [Fact]
    public async Task GetBankingPass()
    {
        _mockUnitOfWork
            .Setup(u => u.EmployeeBanking.Get(It.IsAny<Expression<Func<EmployeeBanking, bool>>>()))
            .Returns( new List<EmployeeBanking> { new(EmployeeBankingTestData.EmployeeBankingDto) }.AsQueryable()
                         .BuildMock());

        var result = await _employeeBankingService.GetBanking(1);

        Assert.NotNull(result);
        Assert.Equivalent(EmployeeBankingTestData.EmployeeBankingDto, result[0]);
    }

    [Fact]
    public async Task GetBankingFail()
    {
        _mockUnitOfWork
            .Setup(u => u.EmployeeBanking.FirstOrDefault(It.IsAny<Expression<Func<EmployeeBanking, bool>>>()))
            .Throws<Exception>();
        _errorLoggingServiceMock.Setup(r => r.LogException(It.IsAny<Exception>())).Throws(new Exception());

        await Assert.ThrowsAsync<Exception>(async () => await _employeeBankingService.GetBanking(2));
    }

    [Fact]
    public async Task SaveFail()
    {
        _employeeTypeServiceMock
            .Setup(r => r.GetEmployeeType(EmployeeTypeTestData.DeveloperType.Name))
            .ReturnsAsync(EmployeeTypeTestData.DeveloperType);

        _mockUnitOfWork
            .Setup(u => u.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
            .Returns(new List<Employee>
            {
                new(EmployeeTestData.EmployeeDto, EmployeeTypeTestData.DeveloperType)
                {
                    EmployeeType = new EmployeeType(EmployeeTypeTestData.DeveloperType),
                    PhysicalAddress = new EmployeeAddress(EmployeeAddressTestData.EmployeeAddressDto),
                    PostalAddress = new EmployeeAddress(EmployeeAddressTestData.EmployeeAddressDto)
                }
            }.AsQueryable().BuildMock());

        _mockUnitOfWork
            .Setup(u => u.EmployeeBanking.Add(It.IsAny<EmployeeBanking>()))
            .ReturnsAsync(EmployeeBankingTestData.EmployeeBankingDto);

        var result =
            await _employeeBankingService.Save(EmployeeBankingTestData.EmployeeBankingDto, "test@retrorabbit.co.za");

        Assert.Equal(EmployeeBankingTestData.EmployeeBankingDto, result);
    }
}
