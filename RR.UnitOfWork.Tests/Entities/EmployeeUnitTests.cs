using HRIS.Models;
using RR.UnitOfWork.Entities.HRIS;
using Xunit;

namespace RR.UnitOfWork.Tests.Entities;

public class EmployeeUnitTests
{
    [Fact]
    public void EmployeeTest()
    {
        var employee = new Employee();

        Assert.IsType<Employee>(employee);
        Assert.NotNull(employee);
    }

    [Fact]
    public void toDtoTest()
    {
        var employee = new Employee();
        var employeeDto = employee.ToDto();

        Assert.IsType<EmployeeDto>(employeeDto);
        Assert.NotNull(employeeDto);
    }
}