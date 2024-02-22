using HRIS.Models;
using RR.UnitOfWork.Entities.HRIS;
using Xunit;

namespace RR.UnitOfWork.Tests.Entities;

public class RoleAccessLinkUnitTests
{
    [Fact]
    public void roleAccessLinkTest()
    {
        var roleAccessLink = new RoleAccessLink();
        Assert.IsType<RoleAccessLink>(roleAccessLink);
        Assert.NotNull(roleAccessLink);
    }

    [Fact]
    public void roleAccessLinkToDtoTest()
    {
        var roleDto = new RoleDto(1, "Employee");
        var roleAccessDto = new RoleAccessDto(1, "ViewEmployee", "Employee Data");

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
    public void roleAccessLinkToDtoNullTest()
    {
        var roleDto = new RoleDto(1, "Employee");
        var roleAccessDto = new RoleAccessDto(1, "ViewEmployee", "Employee Data");

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