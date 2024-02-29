using HRIS.Models;
using RR.UnitOfWork.Entities.HRIS;
using System.Security;
using Xunit;

namespace RR.UnitOfWork.Tests.Entities;

public class RoleAccessLinkUnitTests
{
    [Fact]
    public async Task roleAccessLinkTest()
    {
        var roleAccessLink = new RoleAccessLink();
        Assert.IsType<RoleAccessLink>(roleAccessLink);
        Assert.NotNull(roleAccessLink);
    }

    [Fact]
    public async Task roleAccessLinkToDtoTest()
    {
        var roleDto = new RoleDto { Id = 0, Description = "Employee" };
        var roleAccessDto = new RoleAccessDto{Id = 1, Permission = "ViewEmployee", Grouping = "Employee Data"};

        var roleAccessLinkDto = new RoleAccessLinkDto(
                                                      1,
                                                      roleDto,
                                                      roleAccessDto);
        var roleAccessLink = new RoleAccessLink(roleAccessLinkDto);

        roleAccessLink.Role = new Role(roleDto);
        roleAccessLink.RoleAccess = new RoleAccess(roleAccessDto);

        Assert.IsType<RoleAccessLinkDto>(roleAccessLink.ToDto());
        Assert.NotNull(roleAccessLink.ToDto());
    }

    [Fact]
    public async Task roleAccessLinkToDtoNullTest()
    {
        var roleDto = new RoleDto { Id = 0, Description = "Employee" };
        var roleAccessDto = new RoleAccessDto { Id = 1, Permission = "ViewEmployee", Grouping = "Employee Data" };

        var roleAccessLinkDto = new RoleAccessLinkDto(
                                                      1,
                                                      roleDto,
                                                      roleAccessDto);
        var roleAccessLink = new RoleAccessLink(roleAccessLinkDto);

        Assert.Null(roleAccessLink.Role);
        Assert.Null(roleAccessLink.RoleAccess);
        Assert.NotNull(roleAccessLink.ToDto());
    }
}