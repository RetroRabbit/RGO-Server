using System.Linq.Expressions;
using HRIS.Services.Interfaces;
using HRIS.Services.Services;
using Microsoft.EntityFrameworkCore;
using Moq;
using RR.Tests.Data;
using RR.UnitOfWork;
using RR.UnitOfWork.Entities.HRIS;
using Xunit;

namespace HRIS.Services.Tests.Services;

public class RoleServiceUnitTest
{
    private readonly Mock<IUnitOfWork> _dbMock;
    private readonly Role _role;
    private readonly RoleService _roleService;

    public RoleServiceUnitTest()
    {
        _dbMock = new Mock<IUnitOfWork>();
        var employeeAuthServiceMock = new Mock<IAuthService>();
        _roleService = new RoleService(_dbMock.Object, employeeAuthServiceMock.Object, new AuthorizeIdentityMock("test@gmail.com", "test", "Admin", 1));
        _role = new Role { Id = 1, Description = "Employee" };
    }

    [Fact]
    public async Task CheckRoleTest()
    {
        _dbMock
            .Setup(r => r.Role.Any(It.IsAny<Expression<Func<Role, bool>>>()))
            .ReturnsAsync(true);

        var result = await _roleService.CheckRole(_role.Description!);

        Assert.True(result);
        _dbMock.Verify(r => r.Role.Any(It.IsAny<Expression<Func<Role, bool>>>()), Times.Once);
    }

    [Fact]
    public async Task SaveRoleTest()
    {
        _dbMock
            .Setup(r => r.Role.Add(It.IsAny<Role>()))
            .ReturnsAsync(_role);

        var result = await _roleService.CreateRole(_role.ToDto());

        Assert.NotNull(result);
        Assert.Equivalent(_role.ToDto(), result);
        _dbMock.Verify(r => r.Role.Add(It.IsAny<Role>()), Times.Once);
    }

    [Fact]
    public async Task DeleteRoleTest()
    {
        var roleQueryable = new List<Role>
        {
            new() { Id = 2, Description = "Manager" },
            new() { Id = 3, Description = "Admin" }
        }.ToMockIQueryable();

        Expression<Func<Role, bool>> criteria = r => r.Description == "Admin";
        var expect = await roleQueryable.Where(criteria).Select(r => r).FirstAsync();

        _dbMock
            .Setup(r => r.Role.Get(It.IsAny<Expression<Func<Role, bool>>>()))
            .Returns(roleQueryable.Where(criteria));

        _dbMock
            .Setup(r => r.Role.Delete(It.IsAny<int>()))
            .ReturnsAsync(expect);

        var result = await _roleService.DeleteRole(3);

        Assert.NotNull(result);
        Assert.Equivalent(expect, result);
        _dbMock.Verify(r => r.Role.Delete(It.IsAny<int>()), Times.Once);
    }

    [Fact]
    public async Task GetAllRolesTest()
    {
        var roles = new List<Role> { _role };

        _dbMock
            .Setup(r => r.Role.GetAll(null))
            .ReturnsAsync(roles);

        var result = await _roleService.GetAll();

        Assert.NotNull(result);
        Assert.Equivalent(roles.Select(x => x.ToDto()).ToList(), result);
        _dbMock.Verify(r => r.Role.GetAll(null), Times.Once);
    }

    [Fact]
    public async Task GetRoleTest()
    {
        _dbMock
            .Setup(r => r.Role.Get(It.IsAny<Expression<Func<Role, bool>>>()))
            .Returns(_role.ToMockIQueryable());

        var result = await _roleService.GetRole(_role.Description!);

        Assert.NotNull(result);
        Assert.Equivalent(_role.ToDto(), result);
        _dbMock.Verify(r => r.Role.Get(It.IsAny<Expression<Func<Role, bool>>>()), Times.Once);
    }

    [Fact]
    public async Task UpdateRoleTest()
    {
        var roleQueryable = new List<Role>
        {
            new() { Id = 2, Description = "Manager" },
            new() { Id = 3, Description = "Admin" }
        }.ToMockIQueryable();

        Expression<Func<Role, bool>> criteria = r => r.Description == "Admin";
        var expect = await roleQueryable.Where(criteria).FirstAsync();

        _dbMock
            .Setup(r => r.Role.Get(It.IsAny<Expression<Func<Role, bool>>>()))
            .Returns(roleQueryable.Where(criteria));

        _dbMock
            .Setup(r => r.Role.Update(It.IsAny<Role>()))
            .ReturnsAsync(expect);

        var result = await _roleService.UpdateRole("Admin");

        Assert.NotNull(result);
        Assert.Equivalent(expect.ToDto(), result);
        _dbMock.Verify(r => r.Role.Update(It.IsAny<Role>()), Times.Once);
    }
}