using HRIS.Models;
using HRIS.Models.Enums;
using RR.UnitOfWork.Entities.HRIS;
using Xunit;

namespace RR.UnitOfWork.Tests.Entities;

public class PropertyAccessUnitTests
{
    private readonly FieldCodeDto _fieldCode;
    private readonly RoleDto _role;

    public PropertyAccessUnitTests()
    {
        _role = new RoleDto {Id = 0, Description = "Role" };
        _fieldCode = new FieldCodeDto
        {
            Id = 1,
            Code = "email01",
            Name = "Email",
            Description = "desciption",
            Regex = "@(\\w+).co.za",
            Type = FieldCodeType.String,
            Status = ItemStatus.Active,
            Internal = true,
            InternalTable = "Employee",
            Category = 0,
            Required = false
        };
    }

    public PropertyAccess CreatePropertyAccess(RoleDto? role = null, FieldCodeDto? fieldCode = null)
    {
        var propertyAccess = new PropertyAccess
        {
            Id = 1,
            RoleId = 1,
            Condition = 1,
            FieldCodeId = 1
        };

        if (role != null)
            propertyAccess.Role = new Role(role);

        if (fieldCode != null)
            propertyAccess.FieldCode = new FieldCode(fieldCode);

        return propertyAccess;
    }

    [Fact]
    public void PropertyAccessTest()
    {
        var propertyAccess = new PropertyAccess();
        Assert.IsType<PropertyAccess>(propertyAccess);
        Assert.NotNull(propertyAccess);
    }

    [Fact]
    public void PropertyAccessToDTO()
    {
        var propertyAccess = CreatePropertyAccess(_role, _fieldCode);
        var propertyAccessDto = propertyAccess.ToDto();

        Assert.NotNull(propertyAccessDto.Role);
        Assert.NotNull(propertyAccessDto.FieldCode);

        propertyAccess = new PropertyAccess(propertyAccessDto);
        propertyAccessDto = propertyAccess.ToDto();

        Assert.Null(propertyAccessDto.Role);
        Assert.Null(propertyAccessDto.FieldCode);
    }
}
