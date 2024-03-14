// Ignore Spelling: Dto

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
    public void PropertyAccessToDtoTest()
    {
        var propertyAccess = new PropertyAccess();
        var propertyAccessDto = propertyAccess.ToDto();

        Assert.IsType<PropertyAccessDto>(propertyAccessDto);
        Assert.NotNull(propertyAccessDto);
    }
}