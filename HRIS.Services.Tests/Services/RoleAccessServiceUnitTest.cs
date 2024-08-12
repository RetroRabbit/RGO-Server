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
    private readonly RoleAccessService _roleAccessService2;

    public RoleAccessServiceUnitTest()
    {
        _dbMock = new Mock<IUnitOfWork>();
        _roleAccessService = new RoleAccessService(_dbMock.Object, new AuthorizeIdentityMock("test@gmail.com", "test", "Admin", 1));
        _roleAccessService2 = new RoleAccessService(_dbMock.Object, new AuthorizeIdentityMock("test@gmail.com", "test", "Employee", 2));
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
    public async Task GetAllRoleAccessTestPass()
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
    public async Task GetAllRoleAccessTestUnauthorised()
    {
        await Assert.ThrowsAsync<CustomException>(() => _roleAccessService2.GetAllRoleAccess());
    }

    [Fact]
    public async Task GetRoleAccessTestPass()
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

        _dbMock.Setup(x => x.RoleAccess.Any(It.IsAny<Expression<Func<RoleAccess, bool>>>()))
               .ReturnsAsync(true);

        _dbMock
            .Setup(r => r.RoleAccess.Get(It.IsAny<Expression<Func<RoleAccess, bool>>>()))
            .Returns(permissions.Where(criteria));

        var result = await _roleAccessService.GetRoleAccess(randPermission!);

        Assert.NotNull(result);
        Assert.Equal(randPermission, result.Permission);
        _dbMock.Verify(r => r.RoleAccess.Get(It.IsAny<Expression<Func<RoleAccess, bool>>>()), Times.Once);
    }

    [Fact]
    public async Task GetRoleAccessTestUnauthorised()
    {
        var permissions = new List<RoleAccess>
        {
            new() { Id = 1, Permission = "ViewEmployee" },
            new() { Id = 2, Permission = "ViewManager" },
            new() { Id = 3, Permission = "ViewAdmin" }
        }.ToMockIQueryable();

        var randPermission = permissions.Where(r => r.Id == 3).Select(r => r.Permission).First();

        _dbMock.Setup(x => x.RoleAccess.Any(It.IsAny<Expression<Func<RoleAccess, bool>>>()))
               .ReturnsAsync(true);

        await Assert.ThrowsAsync<CustomException>(() => _roleAccessService2.GetRoleAccess(randPermission!));
    }

    [Fact]
    public async Task GetRoleAccessTestFail()
    {
        var permissions = new List<RoleAccess>
        {
            new() { Id = 1, Permission = "ViewEmployee" },
            new() { Id = 2, Permission = "ViewManager" },
            new() { Id = 3, Permission = "ViewAdmin" }
        }.ToMockIQueryable();

        var randPermission = permissions.Where(r => r.Id == 3).Select(r => r.Permission).First();

        _dbMock.Setup(x => x.RoleAccess.Any(It.IsAny<Expression<Func<RoleAccess, bool>>>()))
               .ReturnsAsync(false);

        await Assert.ThrowsAsync<CustomException>(() => _roleAccessService2.GetRoleAccess(randPermission!));
    }

    [Fact]
    public async Task SaveRoleAccessTestPass()
    {
        _dbMock
            .Setup(r => r.RoleAccess.Add(It.IsAny<RoleAccess>()))
            .ReturnsAsync(_roleAccessDto);

        _dbMock.Setup(x => x.RoleAccess.Any(It.IsAny<Expression<Func<RoleAccess, bool>>>()))
               .ReturnsAsync(false);

        var result = await _roleAccessService.CreateRoleAccess(_roleAccessDto.ToDto());

        Assert.NotNull(result);
        Assert.Equivalent(_roleAccessDto.ToDto(), result);
        _dbMock.Verify(r => r.RoleAccess.Add(It.IsAny<RoleAccess>()), Times.Once);
    }

    [Fact]
    public async Task SaveRoleAccessTestUnauthorised()
    {
        _dbMock.Setup(x => x.RoleAccess.Any(It.IsAny<Expression<Func<RoleAccess, bool>>>()))
               .ReturnsAsync(false);

        await Assert.ThrowsAsync<CustomException>(() => _roleAccessService2.CreateRoleAccess(_roleAccessDto.ToDto()));
    }

    [Fact]
    public async Task SaveRoleAccessTestFail()
    {
        _dbMock.Setup(x => x.RoleAccess.Any(It.IsAny<Expression<Func<RoleAccess, bool>>>()))
               .ReturnsAsync(true);

        await Assert.ThrowsAsync<CustomException>(() => _roleAccessService2.CreateRoleAccess(_roleAccessDto.ToDto()));
    }

    [Fact]
    public async Task DeleteRoleAccessTestPass()
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

        _dbMock.Setup(x => x.RoleAccess.Any(It.IsAny<Expression<Func<RoleAccess, bool>>>()))
               .ReturnsAsync(true);

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
    public async Task DeleteRoleAccessTestUnauthorised()
    {
        var permissions = new List<RoleAccess>
        {
            new() { Id = 1, Permission = "ViewEmployee" },
            new() { Id = 2, Permission = "ViewManager" },
            new() { Id = 3, Permission = "ViewAdmin" }
        }.ToMockIQueryable();

        var randPermission = permissions.Where(r => r.Id == 3).Select(r => r.Permission).First();

        _dbMock.Setup(x => x.RoleAccess.Any(It.IsAny<Expression<Func<RoleAccess, bool>>>()))
               .ReturnsAsync(true);

        await Assert.ThrowsAsync<CustomException>(() => _roleAccessService2.DeleteRoleAccess(randPermission!));
    }

    [Fact]
    public async Task DeleteRoleAccessTestFail()
    {
        var permissions = new List<RoleAccess>
        {
            new() { Id = 1, Permission = "ViewEmployee" },
            new() { Id = 2, Permission = "ViewManager" },
            new() { Id = 3, Permission = "ViewAdmin" }
        }.ToMockIQueryable();

        var randPermission = permissions.Where(r => r.Id == 3).Select(r => r.Permission).First();

        _dbMock.Setup(x => x.RoleAccess.Any(It.IsAny<Expression<Func<RoleAccess, bool>>>()))
               .ReturnsAsync(false);

        await Assert.ThrowsAsync<CustomException>(() => _roleAccessService2.DeleteRoleAccess(randPermission!));
    }

    [Fact]
    public async Task UpdateRoleAccessTestPass()
    {
        _dbMock.Setup(x => x.RoleAccess.Any(It.IsAny<Expression<Func<RoleAccess, bool>>>()))
               .ReturnsAsync(true);

        _dbMock
            .Setup(r => r.RoleAccess.Update(It.IsAny<RoleAccess>()))
            .ReturnsAsync(_roleAccessDto);

        var result = await _roleAccessService.UpdateRoleAccess(_roleAccessDto.ToDto());

        Assert.NotNull(result);
        Assert.Equivalent(_roleAccessDto.ToDto(), result);
        _dbMock.Verify(r => r.RoleAccess.Update(It.IsAny<RoleAccess>()), Times.Once);
    }

    [Fact]
    public async Task UpdateRoleAccessTestUnauthorised()
    {
        _dbMock.Setup(x => x.RoleAccess.Any(It.IsAny<Expression<Func<RoleAccess, bool>>>()))
               .ReturnsAsync(true);

        await Assert.ThrowsAsync<CustomException>(() => _roleAccessService2.UpdateRoleAccess(_roleAccessDto.ToDto()));
    }

    [Fact]
    public async Task UpdateRoleAccessTestFail()
    {
        _dbMock.Setup(x => x.RoleAccess.Any(It.IsAny<Expression<Func<RoleAccess, bool>>>()))
               .ReturnsAsync(false);

        await Assert.ThrowsAsync<CustomException>(() => _roleAccessService2.UpdateRoleAccess(_roleAccessDto.ToDto()));
    }
}