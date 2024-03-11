using HRIS.Models;
using RR.UnitOfWork.Entities.HRIS;
using HRIS.Models.Enums;
using Xunit;

namespace RR.UnitOfWork.Tests.Entities;

public class PropertyAccessUnitTests
{
    [Fact]
    public void PropertyAccessTest()
    {
        var propertyAccess = new PropertyAccess();

        Assert.IsType<PropertyAccess>(propertyAccess);
        Assert.NotNull(propertyAccess);
    }

    [Fact]
    public void toDtoTest()
    {
        var propertyAccess = new Employee();
        var propertyAccessDto = propertyAccess.ToDto();

        Assert.IsType<EmployeeDto>(propertyAccessDto);
        Assert.NotNull(propertyAccessDto);
    }
}