using HRIS.Models;
using HRIS.Models.Enums;
using RR.UnitOfWork.Entities.HRIS;
using Xunit;

namespace RR.UnitOfWork.Tests.Entities;

public class EmployeeDataUnitTests
{
    private readonly EmployeeDto _employee;
    private readonly FieldCodeDto _fieldCode;

    public EmployeeDataUnitTests()
    {
        var employeeTypeDto = new EmployeeTypeDto(1, "Developer");
        var employeeAddressDto =
            new EmployeeAddressDto(1, "2", "Complex", "2", "Suburb/District", "City", "Country", "Province", "1620");

        _employee = new EmployeeDto(1, "001", "34434434", new DateTime(), new DateTime(),
                                    null, false, "None", 4, employeeTypeDto, "Notes", 1, 28, 128, 100000, "Dorothy",
                                    "D",
                                    "Mahoko", new DateTime(), "South Africa", "South African", "0000080000000", " ",
                                    new DateTime(), null, Race.Black, Gender.Male, null,
                                    "texample@retrorabbit.co.za", "test.example@gmail.com", "0000000000", null, null,
                                    employeeAddressDto, employeeAddressDto, null, null, null);

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

    [Fact]
    public void EmployeeDataTest()
    {
        var employeeData = new EmployeeData();
        Assert.IsType<EmployeeData>(employeeData);
        Assert.NotNull(employeeData);
    }

    public EmployeeData CreateEmployeeDataEntity(EmployeeDto? employee = null, FieldCodeDto? fieldCode = null)
    {
        var employeeData = new EmployeeData
        {
            Id = 1,
            EmployeeId = 1,
            FieldCodeId = 1,
            Value = "Value"
        };

        if (employee != null)
            employeeData.Employee = new Employee(employee, employee.EmployeeType);

        if (fieldCode != null)
            employeeData.FieldCode = new FieldCode(fieldCode);

        return employeeData;
    }

    [Fact]
    public void EmployeeDataToDTO()
    {
        var employeeData = CreateEmployeeDataEntity(
                                                    _employee,
                                                    _fieldCode);
        var dto = employeeData.ToDto();

        Assert.Equal(dto.Value, employeeData.Value);

        var initializedEmployeeData = new EmployeeData(dto);

        Assert.Null(initializedEmployeeData.Employee);
        Assert.Null(initializedEmployeeData.FieldCode);

        employeeData = CreateEmployeeDataEntity();
        dto = employeeData.ToDto();

        Assert.Equal(dto.EmployeeId, employeeData.EmployeeId);
        Assert.Equal(dto.FieldCodeId, employeeData.FieldCodeId);
    }
}
