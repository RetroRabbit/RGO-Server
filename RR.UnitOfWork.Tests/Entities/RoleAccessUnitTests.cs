using HRIS.Models;
using RR.UnitOfWork.Entities.HRIS;
using Xunit;

namespace RR.UnitOfWork.Tests.Entities;

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
        var roleAccess = new RoleAccess(new RoleAccessDto { Id = 1, Permission = "ViewEmployee", Grouping = "Employee Data" });
        Assert.IsType<RoleAccessDto>(roleAccess.ToDto());
        Assert.NotNull(roleAccess.ToDto());
    }
}