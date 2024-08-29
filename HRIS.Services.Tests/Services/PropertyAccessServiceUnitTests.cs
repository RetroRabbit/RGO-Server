using System.Linq.Expressions;
using HRIS.Models;
using HRIS.Models.Enums;
using HRIS.Services.Interfaces;
using HRIS.Services.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RR.Tests.Data;
using RR.Tests.Data.Models.HRIS;
using RR.UnitOfWork;
using RR.UnitOfWork.Entities.HRIS;
using Xunit;

namespace HRIS.Services.Tests.Services;

public class PropertyAccessServiceUnitTests
{
    private readonly Mock<IUnitOfWork> _dbMock;
    private readonly Mock<IPropertyAccessService> _propertyAccessService;
    private readonly PropertyAccessService propertyAccessService;
    private readonly PropertyAccessService propertyAccessService2;

    private readonly List<RoleDto> roleList;
    private readonly List<EmployeeRole> employeeRoleList;

    public PropertyAccessServiceUnitTests()
    {
        _dbMock = new Mock<IUnitOfWork>();
        _propertyAccessService = new Mock<IPropertyAccessService>();
        propertyAccessService = new PropertyAccessService(_dbMock.Object, new AuthorizeIdentityMock("test@gmail.com", "test", "Admin", 1));
        propertyAccessService2 = new PropertyAccessService(_dbMock.Object, new AuthorizeIdentityMock("test@gmail.com", "test", "Employee", 2));

        var testEmployee = EmployeeTestData.EmployeeOne;
        roleList = new List<RoleDto>
        {
            new () {Id = 1, Description = "Admin" },
            new () { Id = 2, Description = "Manager" },
            new () { Id = 3, Description = "Employee" },
            new () { Id = 4, Description = "Intern" }
        };

        employeeRoleList = new List<EmployeeRole>
        {
            new()
            {
                Id = 1,
                EmployeeId = testEmployee.Id,
                RoleId = roleList[0].Id,
                Employee = testEmployee,
                Role = new Role(roleList[0])
            },
            new()
            {
                Id = 2,
                EmployeeId = testEmployee.Id,
                RoleId = roleList[1].Id,
                Employee = testEmployee,
                Role = new Role(roleList[1])
            },
            new()
            {
                Id = 3,
                EmployeeId = testEmployee.Id,
                RoleId = roleList[2].Id,
                Employee = testEmployee,
                Role = new Role(roleList[2])
            }
        };
    }

    [Fact]
    public async Task GetAllTestPass()
    {
        _dbMock.Setup(p => p.PropertyAccess.Get(It.IsAny<Expression<Func<PropertyAccess, bool>>>()))
               .Returns(PropertyAccessTestData.PropertyAccessList.ToMockIQueryable());

        var result = await propertyAccessService.GetAll();

        Assert.NotNull(result);
        Assert.IsType<List<PropertyAccessDto>>(result);
        Assert.True(result.SequenceEqual(result.OrderBy(e => e.Id)));
        _dbMock.Verify(p => p.PropertyAccess.Get(It.IsAny<Expression<Func<PropertyAccess, bool>>>()), Times.Once);
    }

    [Fact]
    public async Task GetAllTestUnauthorised()
    {
        _dbMock.Setup(p => p.PropertyAccess.Get(It.IsAny<Expression<Func<PropertyAccess, bool>>>()))
               .Returns(PropertyAccessTestData.PropertyAccessList.ToMockIQueryable());

        await Assert.ThrowsAsync<CustomException>(() => propertyAccessService2.GetAll());
    }

    [Fact]
    public void GetAccessListByEmployeeIdTestPass()
    {
        _dbMock.Setup(e => e.PropertyAccess.Get(It.IsAny<Expression<Func<PropertyAccess, bool>>>()))
               .Returns(PropertyAccessTestData.PropertyAccessList.ToMockIQueryable());

        _dbMock.Setup(e => e.EmployeeRole.Get(It.IsAny<Expression<Func<EmployeeRole, bool>>>()))
               .Returns(employeeRoleList[0].ToMockIQueryable());

        _dbMock
            .Setup(r => r.Employee.Any(It.IsAny<Expression<Func<Employee, bool>>>()))
            .ReturnsAsync(true);

        _dbMock
            .Setup(r => r.Role.Any(It.IsAny<Expression<Func<Role, bool>>>()))
            .ReturnsAsync(true);

        _dbMock.Setup(e => e.PropertyAccess.GetAll(It.IsAny<Expression<Func<PropertyAccess, bool>>>()))
               .ReturnsAsync(PropertyAccessTestData.PropertyAccessOne.ToMockIQueryable().ToList());

        var result = propertyAccessService.GetAccessListByEmployeeId(1); 

        Assert.NotNull(result);
        Assert.Equivalent(PropertyAccessTestData.PropertyAccessOne.ToDto(), result.Result[0]);
    }

    [Fact]
    public async Task GetAccessListByEmployeeIdTestFail()
    {
        _dbMock
            .Setup(r => r.Employee.Any(It.IsAny<Expression<Func<Employee, bool>>>()))
            .ReturnsAsync(false);

        await Assert.ThrowsAsync<CustomException>(() => propertyAccessService2.GetAccessListByEmployeeId(1));
    }

    [Fact]
    public async Task GetAccessListByEmployeeIdTestUnauthorised()
    {
        _dbMock
            .Setup(r => r.Employee.Any(It.IsAny<Expression<Func<Employee, bool>>>()))
            .ReturnsAsync(true);

        await Assert.ThrowsAsync<CustomException>(() => propertyAccessService2.GetAccessListByEmployeeId(1));
    }

    [Fact]
    public void GetAccessListByRoleIdTestPass()
    {
        _dbMock.Setup(e => e.PropertyAccess.GetAll(It.IsAny<Expression<Func<PropertyAccess, bool>>>()))
               .ReturnsAsync(PropertyAccessTestData.PropertyAccessList.ToMockIQueryable().ToList());

        _dbMock
            .Setup(r => r.Role.Any(It.IsAny<Expression<Func<Role, bool>>>()))
            .ReturnsAsync(true);

        var result = propertyAccessService.GetAccessListByRoleId(1);

        Assert.NotNull(result);
    }

    [Fact]
    public async Task GetAccessListByRoleIdTestFail()
    {
        _dbMock.Setup(e => e.PropertyAccess.GetAll(It.IsAny<Expression<Func<PropertyAccess, bool>>>()))
               .ReturnsAsync(PropertyAccessTestData.PropertyAccessList.ToMockIQueryable().ToList());

        _dbMock
            .Setup(r => r.Role.Any(It.IsAny<Expression<Func<Role, bool>>>()))
            .ReturnsAsync(false);

        await Assert.ThrowsAsync<CustomException>(() => propertyAccessService2.GetAccessListByRoleId(1));
    }

    [Fact]
    public async Task UpdatePropertyAccessPass()
    {
        _propertyAccessService.Setup(r => r.UpdatePropertyAccess(PropertyAccessTestData.PropertyAccessOne.Id, PropertyAccessLevel.read));

        _dbMock
            .Setup(r => r.PropertyAccess.Any(It.IsAny<Expression<Func<PropertyAccess, bool>>>()))
            .ReturnsAsync(true);

        _dbMock.Setup(r => r.PropertyAccess.Update(It.IsAny<PropertyAccess>()))
               .ReturnsAsync(PropertyAccessTestData.PropertyAccessOne);

        _dbMock.Setup(e => e.PropertyAccess.Get(It.IsAny<Expression<Func<PropertyAccess, bool>>>()))
               .Returns(PropertyAccessTestData.PropertyAccessOne.ToMockIQueryable());

        await propertyAccessService.UpdatePropertyAccess(PropertyAccessTestData.PropertyAccessOne.Id, PropertyAccessLevel.read);

        _dbMock.Verify(p => p.PropertyAccess.Update(PropertyAccessTestData.PropertyAccessOne), Times.Once);
    }

    [Fact]
    public async Task UpdatePropertyAccessFail()
    {
        _propertyAccessService.Setup(r => r.UpdatePropertyAccess(PropertyAccessTestData.PropertyAccessOne.Id, PropertyAccessLevel.read));

        _dbMock
            .Setup(r => r.PropertyAccess.Any(It.IsAny<Expression<Func<PropertyAccess, bool>>>()))
            .ReturnsAsync(false);

        _dbMock.Setup(r => r.PropertyAccess.Update(It.IsAny<PropertyAccess>()))
               .ReturnsAsync(PropertyAccessTestData.PropertyAccessOne);

        await Assert.ThrowsAsync<CustomException>(() => propertyAccessService2.UpdatePropertyAccess(PropertyAccessTestData.PropertyAccessOne.Id, PropertyAccessLevel.read));
    }

    [Fact]
    public async Task UpdatePropertyAccessUnauthorised()
    {
        _propertyAccessService.Setup(r => r.UpdatePropertyAccess(PropertyAccessTestData.PropertyAccessOne.Id, PropertyAccessLevel.read));

        _dbMock
            .Setup(r => r.PropertyAccess.Any(It.IsAny<Expression<Func<PropertyAccess, bool>>>()))
            .ReturnsAsync(true);

        _dbMock.Setup(r => r.PropertyAccess.Update(It.IsAny<PropertyAccess>()))
               .ReturnsAsync(PropertyAccessTestData.PropertyAccessOne);

        await Assert.ThrowsAsync<CustomException>(() => propertyAccessService2.UpdatePropertyAccess(PropertyAccessTestData.PropertyAccessOne.Id, PropertyAccessLevel.read));
    }

    [Fact]
    public async Task CreatePropertyAccessEntriesPass()
    {
        _propertyAccessService.Setup(r => r.UpdatePropertyAccess(PropertyAccessTestData.PropertyAccessOne.Id, PropertyAccessLevel.read));

        List<Role> roles = new List<Role>();

        foreach (RoleDto item in roleList)
        {
            roles.Add(new Role(item));
        }

        _dbMock.Setup(e => e.Role.GetAll(It.IsAny<Expression<Func<Role, bool>>>()))
               .ReturnsAsync(roles);

        List<string> columns = new List<string>() { "ID", "Description" };

        _dbMock.Setup(e => e.GetColumnNames(It.IsAny<string>())).ReturnsAsync(columns);

        _dbMock.Setup(e => e.PropertyAccess.GetAll(It.IsAny<Expression<Func<PropertyAccess, bool>>>()))
               .ReturnsAsync(PropertyAccessTestData.PropertyAccessList.ToMockIQueryable().ToList());

        var result = await propertyAccessService.CreatePropertyAccessEntries();

        Assert.NotNull(result);
        _dbMock.Verify(p => p.PropertyAccess.AddRange(result), Times.Once);
    }

    [Fact]
    public async Task CreatePropertyAccessEntriesUnauthorised()
    {
        _propertyAccessService.Setup(r => r.UpdatePropertyAccess(PropertyAccessTestData.PropertyAccessOne.Id, PropertyAccessLevel.read));

        List<Role> roles = new List<Role>();

        foreach (RoleDto item in roleList)
        {
            roles.Add(new Role(item));
        }

        _dbMock.Setup(e => e.Role.GetAll(It.IsAny<Expression<Func<Role, bool>>>()))
               .ReturnsAsync(roles);

        List<string> columns = new List<string>() { "ID", "Description" };

        _dbMock.Setup(e => e.GetColumnNames(It.IsAny<string>())).ReturnsAsync(columns);

        _dbMock.Setup(e => e.PropertyAccess.GetAll(It.IsAny<Expression<Func<PropertyAccess, bool>>>()))
               .ReturnsAsync(PropertyAccessTestData.PropertyAccessList.ToMockIQueryable().ToList());

        await Assert.ThrowsAsync<CustomException>(() => propertyAccessService2.CreatePropertyAccessEntries());
    }
}