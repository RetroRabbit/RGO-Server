using HRIS.Models;
using RR.UnitOfWork.Entities.HRIS;
using Xunit;

namespace RR.UnitOfWork.Tests.Entities;

public class EmployeeUnitTests
{
    [Fact]
    public async Task EmployeeTest()
    {
        var employee = new Employee();

        Assert.IsType<Employee>(employee);
        Assert.NotNull(employee);
    }

    [Fact]
    public async Task toDtoTest()
    {
        var employee = new Employee();
        var employeeDto = employee.ToDto();

        Assert.IsType<EmployeeDto>(employeeDto);
        Assert.NotNull(employeeDto);
    }
}