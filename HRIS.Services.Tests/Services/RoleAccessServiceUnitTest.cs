using System.Linq.Expressions;
using HRIS.Services.Services;
using Microsoft.EntityFrameworkCore;
using Moq;
using RR.Tests.Data;
using RR.UnitOfWork;
using RR.UnitOfWork.Entities.HRIS;
using Xunit;

namespace HRIS.Services.Tests.Services;

public class RoleAccessServiceUnitTest
{
    private readonly Mock<IUnitOfWork> _dbMock;
    private readonly RoleAccess _roleAccessDto;
    private readonly RoleAccessService _roleAccessService;

    public RoleAccessServiceUnitTest()
    {
        _dbMock = new Mock<IUnitOfWork>();
        _roleAccessService = new RoleAccessService(_dbMock.Object);
        _roleAccessDto = new RoleAccess { Id = 1, Permission = "ViewEmplopyee", Grouping = "Employee Data" };
    }

    [Fact]
    public async Task CheckRoleAccessTest()
    {
        _dbMock
            .Setup(r => r.RoleAccess.Any(It.IsAny<Expression<Func<RoleAccess, bool>>>()))
            .ReturnsAsync(true);

        var result = await _roleAccessService.CheckRoleAccess(_roleAccessDto.Permission);

        Assert.True(result);
        _dbMock.Verify(r => r.RoleAccess.Any(It.IsAny<Expression<Func<RoleAccess, bool>>>()), Times.Once);
    }

    [Fact]
    public async Task GetAllRoleAccessTest()
    {
        var listOfRoleAccess = new List<RoleAccess> { _roleAccessDto };

        _dbMock
            .Setup(r => r.RoleAccess.GetAll(null))
            .ReturnsAsync(listOfRoleAccess);

        var result = await _roleAccessService.GetAllRoleAccess();

        Assert.NotNull(result);
        Assert.Equivalent(listOfRoleAccess.Select(x => x.ToDto()).ToList(), result);
        _dbMock.Verify(r => r.RoleAccess.GetAll(null), Times.Once);
    }

    [Fact]
    public async Task GetRoleAccessTest()
    {
        var permissions = new List<RoleAccess>
        {
            new() { Id = 1, Permission = "ViewEmployee" },
            new() { Id = 2, Permission = "ViewManager" },
            new() { Id = 3, Permission = "ViewAdmin" }
        }.ToMockIQueryable();

        var randPermission = permissions.Where(r => r.Id == 3).Select(r => r.Permission).First();

        Expression<Func<RoleAccess, bool>> criteria = r => r.Permission == randPermission;
        await permissions.Where(criteria).Select(r => r.ToDto()).FirstAsync();

        _dbMock
            .Setup(r => r.RoleAccess.Get(It.IsAny<Expression<Func<RoleAccess, bool>>>()))
            .Returns(permissions.Where(criteria));

        var result = await _roleAccessService.GetRoleAccess(randPermission!);

        Assert.NotNull(result);
        Assert.Equal(randPermission, result.Permission);
        _dbMock.Verify(r => r.RoleAccess.Get(It.IsAny<Expression<Func<RoleAccess, bool>>>()), Times.Once);
    }

    [Fact]
    public async Task SaveRoleAccessTest()
    {
        _dbMock
            .Setup(r => r.RoleAccess.Add(It.IsAny<RoleAccess>()))
            .ReturnsAsync(_roleAccessDto);

        var result = await _roleAccessService.SaveRoleAccess(_roleAccessDto.ToDto());

        Assert.NotNull(result);
        Assert.Equivalent(_roleAccessDto.ToDto(), result);
        _dbMock.Verify(r => r.RoleAccess.Add(It.IsAny<RoleAccess>()), Times.Once);
    }

    [Fact]
    public async Task DeleteRoleAccessTest()
    {
        var permissions = new List<RoleAccess>
        {
            new() { Id = 1, Permission = "ViewEmployee" },
            new() { Id = 2, Permission = "ViewManager" },
            new() { Id = 3, Permission = "ViewAdmin" }
        }.ToMockIQueryable();

        var randPermission = permissions.Where(r => r.Id == 3).Select(r => r.Permission).First();

        Expression<Func<RoleAccess, bool>> criteria = r => r.Permission == randPermission;
        var expect = await permissions.Where(criteria).Select(r => r).FirstAsync();

        _dbMock
            .Setup(r => r.RoleAccess.Get(It.IsAny<Expression<Func<RoleAccess, bool>>>()))
            .Returns(permissions.Where(criteria));

        _dbMock
            .Setup(r => r.RoleAccess.Delete(It.IsAny<int>()))
            .ReturnsAsync(expect);

        var result = await _roleAccessService.DeleteRoleAccess(randPermission!);

        Assert.NotNull(result);
        Assert.Equivalent(expect.ToDto(), result);
        _dbMock.Verify(r => r.RoleAccess.Delete(It.IsAny<int>()), Times.Once);
    }

    [Fact]
    public async Task UpdateRoleAccessTest()
    {
        _dbMock
            .Setup(r => r.RoleAccess.Update(It.IsAny<RoleAccess>()))
            .ReturnsAsync(_roleAccessDto);

        var result = await _roleAccessService.UpdateRoleAccess(_roleAccessDto.ToDto());

        Assert.NotNull(result);
        Assert.Equivalent(_roleAccessDto.ToDto(), result);
        _dbMock.Verify(r => r.RoleAccess.Update(It.IsAny<RoleAccess>()), Times.Once);
    }
}