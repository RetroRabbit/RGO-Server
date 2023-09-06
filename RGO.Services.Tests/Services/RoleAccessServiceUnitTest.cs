using Moq;
using MockQueryable.Moq;
using RGO.Models;
using RGO.Services.Services;
using RGO.UnitOfWork;
using RGO.UnitOfWork.Entities;
using System.Linq.Expressions;
using Xunit;
using Microsoft.EntityFrameworkCore;

namespace RGO.Services.Tests.Services;

public class RoleAccessServiceUnitTest
{
    private readonly Mock<IUnitOfWork> _dbMock;
    private readonly RoleAccessService _roleAccessService;
    private readonly RoleAccessDto _roleAccessDto;

    public RoleAccessServiceUnitTest()
    {
        _dbMock = new Mock<IUnitOfWork>();
        _roleAccessService = new RoleAccessService(_dbMock.Object);
        _roleAccessDto = new RoleAccessDto(1, "ViewEmplopyee");
    }

    [Fact]
    public async Task CheckRoleAccessTest()
    {
        _dbMock
            .Setup(r => r.RoleAccess.Any(It.IsAny<Expression<Func<RoleAccess, bool>>>()))
            .Returns(Task.FromResult(true));

        var result = await _roleAccessService.CheckRoleAccess(_roleAccessDto.Permission);

        Assert.True(result);
        _dbMock.Verify(r => r.RoleAccess.Any(It.IsAny<Expression<Func<RoleAccess, bool>>>()), Times.Once);
    }

    [Fact]
    public async Task GetAllRoleAccessTest()
    {
        var listOfRoleAccess = new List<RoleAccessDto>() { _roleAccessDto };

        _dbMock
            .Setup(r => r.RoleAccess.GetAll(null))
            .Returns(Task.FromResult(listOfRoleAccess));

        var result = await _roleAccessService.GetAllRoleAccess();

        Assert.NotNull(result);
        Assert.Equal(listOfRoleAccess, result);
        _dbMock.Verify(r => r.RoleAccess.GetAll(null), Times.Once);
    }

    [Fact]
    public async Task GetRoleAccessTest()
    {
        var permissions = new List<RoleAccess>
        {
            new RoleAccess { Id = 1, Permission = "ViewEmployee" },
            new RoleAccess { Id = 2, Permission = "ViewManager" },
            new RoleAccess { Id = 3, Permission = "ViewAdmin" }
        }.AsQueryable().BuildMock();

        var randPermission = permissions.Where(r => r.Id == 3).Select(r => r.Permission).First();

        Expression<Func<RoleAccess, bool>> criteria = r => r.Permission == randPermission;
        var expect = await permissions.Where(criteria).Select(r => r.ToDto()).FirstAsync();

        _dbMock
            .Setup(r => r.RoleAccess.Get(It.IsAny<Expression<Func<RoleAccess, bool>>>()))
            .Returns(permissions.Where(criteria));

        var result = await _roleAccessService.GetRoleAccess(randPermission);

        Assert.NotNull(result);
        Assert.Equal(randPermission, result.Permission);
        _dbMock.Verify(r => r.RoleAccess.Get(It.IsAny<Expression<Func<RoleAccess, bool>>>()), Times.Once);
    }

    [Fact]
    public async Task SaveRoleAccessTest()
    {
        _dbMock
            .Setup(r => r.RoleAccess.Add(It.IsAny<RoleAccess>()))
            .Returns(Task.FromResult(_roleAccessDto));

        var result = await _roleAccessService.SaveRoleAccess(_roleAccessDto);

        Assert.NotNull(result);
        Assert.Equal(_roleAccessDto, result);
        _dbMock.Verify(r => r.RoleAccess.Add(It.IsAny<RoleAccess>()), Times.Once);
    }

    [Fact]
    public async Task DeleteRoleAccessTest()
    {
        var permissions = new List<RoleAccess>
        {
            new RoleAccess { Id = 1, Permission = "ViewEmployee" },
            new RoleAccess { Id = 2, Permission = "ViewManager" },
            new RoleAccess { Id = 3, Permission = "ViewAdmin" }
        }.AsQueryable().BuildMock();

        var randPermission = permissions.Where(r => r.Id == 3).Select(r => r.Permission).First();

        Expression<Func<RoleAccess, bool>> criteria = r => r.Permission == randPermission;
        var expect = await permissions.Where(criteria).Select(r => r.ToDto()).FirstAsync();

        _dbMock
            .Setup(r => r.RoleAccess.Get(It.IsAny<Expression<Func<RoleAccess, bool>>>()))
            .Returns(permissions.Where(criteria));

        _dbMock
            .Setup(r => r.RoleAccess.Delete(It.IsAny<int>()))
            .Returns(Task.FromResult(expect));

        var result = await _roleAccessService.DeleteRoleAccess(randPermission);

        Assert.NotNull(result);
        Assert.Equal(expect, result);
        _dbMock.Verify(r => r.RoleAccess.Delete(It.IsAny<int>()), Times.Once);
    }

    [Fact]
    public async Task UpdateRoleAccessTest()
    {
        _dbMock
            .Setup(r => r.RoleAccess.Update(It.IsAny<RoleAccess>()))
            .Returns(Task.FromResult(_roleAccessDto));

        var result = await _roleAccessService.UpdateRoleAccess(_roleAccessDto);

        Assert.NotNull(result);
        Assert.Equal(_roleAccessDto, result);
        _dbMock.Verify(r => r.RoleAccess.Update(It.IsAny<RoleAccess>()), Times.Once);
    }
}
