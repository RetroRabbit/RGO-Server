using RGO.Models;
using RGO.UnitOfWork.Entities;
using Xunit;

namespace RGO.UnitOfWork.Tests.Entities;

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
        RoleDto roleDto = new RoleDto(1, "Employee");
        RoleAccessDto roleAccessDto = new RoleAccessDto(1, "ViewEmployee");
        
        RoleAccessLinkDto roleAccessLinkDto = new RoleAccessLinkDto(
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
        RoleDto roleDto = new RoleDto(1, "Employee");
        RoleAccessDto roleAccessDto = new RoleAccessDto(1, "ViewEmployee");

        RoleAccessLinkDto roleAccessLinkDto = new RoleAccessLinkDto(
            1,
            roleDto,
            roleAccessDto);
        var roleAccessLink = new RoleAccessLink(roleAccessLinkDto);

        Assert.Null(roleAccessLink.Role);
        Assert.Null(roleAccessLink.RoleAccess);
        Assert.NotNull(roleAccessLink.ToDto());
    }
}
