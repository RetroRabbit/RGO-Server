using HRIS.Models;
using RR.UnitOfWork.Entities.HRIS;
using Xunit;

namespace RR.UnitOfWork.Tests.Entities;

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
        var employeeTypeDto = new EmployeeTypeDto{ Id = 1, Name = "Name" };
        var employeeType = new EmployeeType(employeeTypeDto);
        var dto = employeeType.ToDto();
        Assert.Equal(dto.Id, employeeType.Id);
        Assert.Equal(dto.Name, employeeType.Name);
    }
}
