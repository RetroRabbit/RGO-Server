using RGO.Models;
using RGO.UnitOfWork.Entities;
using Xunit;

namespace RGO.UnitOfWork.Tests.Entities;

public class RoleAccessUnitTests
{
    [Fact]
    public async Task roleAccessTest()
    {
        var roleAccess = new RoleAccess();
        Assert.IsType<RoleAccess>(roleAccess);
        Assert.NotNull(roleAccess);
    }

    [Fact]
    public async Task roleAccessTodtoTest()
    {
        var roleAccess = new RoleAccess(new RoleAccessDto(1, "ViewEmployee", "Employee Data"));
        Assert.IsType<RoleAccessDto>(roleAccess.ToDto());
        Assert.NotNull(roleAccess.ToDto());
    }
}
