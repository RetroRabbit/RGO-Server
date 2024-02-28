using HRIS.Models;
using RR.UnitOfWork.Entities.HRIS;
using Xunit;

namespace RR.UnitOfWork.Tests.Entities;

public class RoleUnitTests
{
    [Fact]
    public async Task roleTest()
    {
        var role = new Role();
        Assert.IsType<Role>(role);
        Assert.NotNull(role);
    }

    [Fact]
    public async Task roleTodtoTest()
    {
        var role = new Role(new RoleDto { Id = 1, Description = "Employee" });
        Assert.IsType<RoleDto>(role.ToDto());
        Assert.NotNull(role.ToDto());
    }
}