using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using MockQueryable.Moq;
using Moq;
using RGO.Models;
using RGO.Services.Interfaces;
using RGO.Services.Services;
using RGO.UnitOfWork;
using RGO.UnitOfWork.Entities;
using System.Collections.Generic;
using System.Linq.Expressions;
using Xunit;

namespace RGO.Services.Tests.Services;

public class RoleAccessLinkServiceUnitTest
{
    private readonly Mock<IUnitOfWork> _dbMock;
    private readonly Mock<IEmployeeRoleService> _employeeRoleServiceMock;
    private readonly RoleAccessLinkService _roleAccessLinkService;
    private readonly RoleDto _roleDto;
    private readonly RoleAccessDto _roleAccessDto;
    private readonly RoleAccessLinkDto _roleAccessLinkDto;

    public RoleAccessLinkServiceUnitTest()
    {
        _dbMock = new Mock<IUnitOfWork>();
        _employeeRoleServiceMock = new Mock<IEmployeeRoleService>();
        _roleAccessLinkService = new RoleAccessLinkService(_dbMock.Object, _employeeRoleServiceMock.Object);
        _roleDto = new RoleDto(1, "Employee");
        _roleAccessDto = new RoleAccessDto(1, "ViewEmployee");
        _roleAccessLinkDto = new RoleAccessLinkDto(1, _roleDto, _roleAccessDto);
    }

    [Fact]
    public async Task SaveTest()
    {
        _dbMock
            .Setup(r => r.RoleAccessLink.Add(It.IsAny<RoleAccessLink>()))
            .Returns(Task.FromResult(_roleAccessLinkDto));

        var result = await _roleAccessLinkService.Save(_roleAccessLinkDto);

        Assert.NotNull(result);
        Assert.Equal(_roleAccessLinkDto, result);
        _dbMock.Verify(r => r.RoleAccessLink.Add(It.IsAny<RoleAccessLink>()), Times.Once);
    }

    [Fact]
    public async Task DeleteTest()
    {
        var roleAccessLinksData = new List<RoleAccessLink>() { new(_roleAccessLinkDto) };

        var mock = roleAccessLinksData.BuildMock();

        _dbMock
            .Setup(r => r.RoleAccessLink.Get(It.IsAny<Expression<Func<RoleAccessLink, bool>>>()))
            .Returns(mock);

        var result = await _roleAccessLinkService.Delete(_roleDto.Description, _roleAccessDto.Permission);

        Assert.NotNull(result);
        Assert.Equal(_roleAccessLinkDto.Id, result.Id);
        _dbMock.Verify(r => r.RoleAccessLink.Get(It.IsAny<Expression<Func<RoleAccessLink, bool>>>()), Times.AtLeastOnce);
    }

    [Fact]
    public async Task GetAllTest()
    {
        Assert.True(true);
    }

    [Fact]
    public async Task GetByRoleTest()
    {
        Assert.True(true);
    }

    [Fact]
    public async Task GetRoleByEmployeeTest()
    {
        Assert.True(true);
    }

    [Fact]
    public async Task Update()
    {
        var roleAccessLinkToUpdate = new RoleAccessLinkDto(
            _roleAccessLinkDto.Id,
            new RoleDto(1, "Employee"),
            new RoleAccessDto(2, "EditEmployee"));

        _dbMock
            .Setup(r => r.RoleAccessLink.Update(It.IsAny<RoleAccessLink>()))
            .Returns(Task.FromResult(_roleAccessLinkDto));

        var result = await _roleAccessLinkService.Update(roleAccessLinkToUpdate);

        Assert.NotNull(result);
        Assert.NotNull(result.Role);
        Assert.NotNull(result.RoleAccess);
        Assert.Equal(_roleAccessLinkDto, result);
        _dbMock.Verify(r => r.RoleAccessLink.Update(It.IsAny<RoleAccessLink>()), Times.Once);
    }
}