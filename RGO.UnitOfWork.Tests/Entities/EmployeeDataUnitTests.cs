using RGO.Models;
using RGO.UnitOfWork.Entities;
using Xunit;

namespace RGO.UnitOfWork.Tests.Entities;

public class EmployeeDataUnitTests
{
    private EmployeeDto _employee;
    private FieldCodeDto _fieldCode;

    public EmployeeDataUnitTests()
    {
        EmployeeTypeDto employeeTypeDto = new EmployeeTypeDto(1, "Developer");

        _employee = new EmployeeDto(1, "001", "34434434", new DateOnly(), new DateOnly(),
            null, false, "None", 4, employeeTypeDto, "Notes", 1, 28, 128, 100000, "Ms", "Dorothy", "D",
            "Mahoko", new DateOnly(), "South Africa", "South African", "0000080000000", " ",
            new DateOnly(), null, Models.Enums.Race.Black, Models.Enums.Gender.Male, null,
            "texample@retrorabbit.co.za", "test.example@gmail.com", "0000000000");

        _fieldCode = new FieldCodeDto(1, "email01", "Email", "desciption", "@(\\w+).co.za", Models.Enums.FieldCodeType.String, Models.Enums.ItemStatus.Active, true, "Employee");
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
        EmployeeData employeeData = new EmployeeData
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
            employee: _employee,
            fieldCode: _fieldCode);
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
