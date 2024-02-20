using System.Linq.Expressions;
using HRIS.Models;
using HRIS.Services.Services;
using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
using RR.UnitOfWork;
using RR.UnitOfWork.Entities.HRIS;
using Xunit;

namespace RGO.Tests.Services;

public class RoleServiceUnitTest
{
    private readonly Mock<IUnitOfWork> _dbMock;
    private readonly RoleDto _roleDto;
    private readonly RoleService _roleService;

    public RoleServiceUnitTest()
    {
        _dbMock = new Mock<IUnitOfWork>();
        _roleService = new RoleService(_dbMock.Object);
        _roleDto = new RoleDto(1, "Employee");
    }

    [Fact]
    public async Task CheckRoleTest()
    {
        _dbMock
            .Setup(r => r.Role.Any(It.IsAny<Expression<Func<Role, bool>>>()))
            .Returns(Task.FromResult(true));

        var result = await _roleService.CheckRole(_roleDto.Description);

        Assert.True(result);
        _dbMock.Verify(r => r.Role.Any(It.IsAny<Expression<Func<Role, bool>>>()), Times.Once);
    }

    [Fact]
    public async Task SaveRoleTest()
    {
        _dbMock
            .Setup(r => r.Role.Add(It.IsAny<Role>()))
            .Returns(Task.FromResult(_roleDto));

        var result = await _roleService.SaveRole(_roleDto);

        Assert.NotNull(result);
        Assert.Equal(_roleDto, result);
        _dbMock.Verify(r => r.Role.Add(It.IsAny<Role>()), Times.Once);
    }

    [Fact]
    public async Task DeleteRoleTest()
    {
        var roleQueryable = new List<Role>
        {
            new() { Id = 2, Description = "Manager" },
            new() { Id = 3, Description = "Admin" }
        }.AsQueryable().BuildMock();

        Expression<Func<Role, bool>> criteria = r => r.Description == "Admin";
        var expect = await roleQueryable.Where(criteria).Select(r => r.ToDto()).FirstAsync();

        _dbMock
            .Setup(r => r.Role.Get(It.IsAny<Expression<Func<Role, bool>>>()))
            .Returns(roleQueryable.Where(criteria));

        _dbMock
            .Setup(r => r.Role.Delete(It.IsAny<int>()))
            .Returns(Task.FromResult(expect));

        var result = await _roleService.DeleteRole(3);

        Assert.NotNull(result);
        Assert.Equivalent(expect, result);
        _dbMock.Verify(r => r.Role.Delete(It.IsAny<int>()), Times.Once);
    }

    [Fact]
    public async Task GetAllRolesTest()
    {
        var roles = new List<RoleDto> { _roleDto };

        _dbMock
            .Setup(r => r.Role.GetAll(null))
            .Returns(Task.FromResult(roles));

        var result = await _roleService.GetAll();

        Assert.NotNull(result);
        Assert.Equal(roles, result);
        _dbMock.Verify(r => r.Role.GetAll(null), Times.Once);
    }

    [Fact]
    public async Task GetRoleTest()
    {
        var roleQueryable = new List<Role> { new(_roleDto) }.AsQueryable().BuildMock();

        _dbMock
            .Setup(r => r.Role.Get(It.IsAny<Expression<Func<Role, bool>>>()))
            .Returns(roleQueryable);

        var result = await _roleService.GetRole(_roleDto.Description);

        Assert.NotNull(result);
        Assert.Equivalent(_roleDto, result);
        _dbMock.Verify(r => r.Role.Get(It.IsAny<Expression<Func<Role, bool>>>()), Times.Once);
    }

    [Fact]
    public async Task UpdateRoleTest()
    {
        var roleQueryable = new List<Role>
        {
            new() { Id = 2, Description = "Manager" },
            new() { Id = 3, Description = "Admin" }
        }.AsQueryable().BuildMock();

        Expression<Func<Role, bool>> criteria = r => r.Description == "Admin";
        var expect = await roleQueryable.Where(criteria).FirstAsync();

        _dbMock
            .Setup(r => r.Role.Get(It.IsAny<Expression<Func<Role, bool>>>()))
            .Returns(roleQueryable.Where(criteria));

        _dbMock
            .Setup(r => r.Role.Update(It.IsAny<Role>()))
            .Returns(Task.FromResult(expect.ToDto()));

        var result = await _roleService.UpdateRole("Admin");

        Assert.NotNull(result);
        Assert.Equivalent(expect.ToDto(), result);
        _dbMock.Verify(r => r.Role.Update(It.IsAny<Role>()), Times.Once);
    }
}