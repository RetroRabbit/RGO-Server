using RGO.Models;
using RGO.UnitOfWork.Entities;
using Xunit;

namespace RGO.UnitOfWork.Tests.Entities;

public class EmployeeTypeUnitTests
{
    [Fact]
    public void EmployeeTypeTest()
    {
        var employeeType = new EmployeeType();
        Assert.IsType<EmployeeType>(employeeType);
        Assert.NotNull(employeeType);
    }

    [Fact]
    public void EmployeeTypeToDtoTest()
    {
        var employeeTypeDto = new EmployeeTypeDto(1, "Name");
        var employeeType = new EmployeeType(employeeTypeDto);
        var dto = employeeType.ToDto();
        Assert.Equal(dto.Id, employeeType.Id);
        Assert.Equal(dto.Name, employeeType.Name);
    }
}
