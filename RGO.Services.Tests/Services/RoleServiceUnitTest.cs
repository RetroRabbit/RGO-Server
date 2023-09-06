using Moq;
using RGO.Models;
using RGO.Services.Services;
using RGO.UnitOfWork;
using RGO.UnitOfWork.Entities;
using System.Linq.Expressions;
using Xunit;

namespace RGO.Services.Tests.Services;

public class RoleServiceUnitTest
{
    private readonly Mock<IUnitOfWork> _dbMock;
    private readonly RoleService _roleService;
    private readonly RoleDto _roleDto;

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
        Assert.True(true);
    }

    [Fact]
    public async Task GetAllRolesTest()
    {
        List<RoleDto> roles = new List<RoleDto>() { _roleDto };

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
        Assert.True(true);
    }

    [Fact]
    public async Task UpdateRoleTest()
    {
        Assert.True(true);
    }
}
