using RGO.Models;
using RGO.UnitOfWork.Entities;
using Xunit;

namespace RGO.UnitOfWork.Tests.Entities;

public class PropertyAccessUnitTests
{
    private RoleDto _role;
    private FieldCodeDto _fieldCode;

    public PropertyAccessUnitTests()
    {
        _role = new RoleDto(0, "Role");
        _fieldCode = new FieldCodeDto(0, "email01", "Email", "desciption", "@(\\w+).co.za", Models.Enums.FieldCodeType.String, Models.Enums.ItemStatus.Active, true, "Employee");
    }

    public PropertyAccess CreatePropertyAccess(RoleDto? role = null, FieldCodeDto? fieldCode = null)
    {
        PropertyAccess propertyAccess = new PropertyAccess
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
