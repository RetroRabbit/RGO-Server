using HRIS.Models;
using HRIS.Models.Enums;
using RR.UnitOfWork.Entities.HRIS;
using Xunit;

namespace RR.UnitOfWork.Tests.Entities;

public class EmployeeDateUnitTests
{
    private readonly EmployeeDto _employee;

    public EmployeeDateUnitTests()
    {
        EmployeeTypeDto employeeTypeDto = new(1, "Developer");
        var employeeAddressDto =
            new EmployeeAddressDto(1, "2", "Complex", "2", "Suburb/District", "City", "Country", "Province", "1620");

        _employee = new EmployeeDto(1, "001", "34434434", new DateTime(), new DateTime(),
                                    null, false, "None", 4, employeeTypeDto, "Notes", 1, 28, 128, 100000, "Dorothy",
                                    "D",
                                    "Mahoko", new DateTime(), "South Africa", "South African", "0000080000000", " ",
                                    new DateTime(), null, Race.Black, Gender.Male, null!,
                                    "texample@retrorabbit.co.za", "test.example@gmail.com", "0000000000", null, null,
                                    employeeAddressDto, employeeAddressDto, null, null, null);
    }

    public EmployeeDate CreateEmployeeDate(EmployeeDto? employee = null)
    {
        EmployeeDate employeeDate = new()
        {
            Id = 1,
            EmployeeId = 1,
            Subject = "",
            Note = "",
            Date = DateOnly.FromDateTime(DateTime.Now)
        };

        if (employee != null)
            employeeDate.Employee = new Employee(employee, employee.EmployeeType!);

        return employeeDate;
    }

    [Fact]
    public void EmployeeDateTest()
    {
        var employeeDate = new EmployeeDate();
        Assert.IsType<EmployeeDate>(employeeDate);
        Assert.NotNull(employeeDate);
    }

    [Fact]
    public void EmployeeDateToDTO()
    {
        var employeeDate = CreateEmployeeDate(_employee);
        var employeeDateDto = employeeDate.ToDto();

        Assert.NotNull(employeeDateDto.Employee);
        Assert.Equal(employeeDate.EmployeeId, employeeDateDto.Employee!.Id);
        Assert.IsType<EmployeeDateDto>(employeeDateDto);

        var initializedEmployeeDate = new EmployeeDate(employeeDateDto);
        Assert.Null(initializedEmployeeDate.Employee);
    }
}