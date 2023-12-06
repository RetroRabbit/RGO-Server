using RGO.Models;
using RGO.UnitOfWork.Entities;
using Xunit;

namespace RGO.UnitOfWork.Tests.Entities;

public class EmployeeDocumentUnitTests
{
    private EmployeeDto _employee;

    public EmployeeDocumentUnitTests()
    {
        EmployeeTypeDto employeeTypeDto = new EmployeeTypeDto(1, "Developer");
        EmployeeAddressDto employeeAddressDto = new EmployeeAddressDto(1, "2", "Complex", "2", "Suburb/District", "City", "Country", "Province", "1620");

        _employee = new EmployeeDto(1, "001", "34434434", new DateTime(), new DateTime(),
            null, false, "None", 4, employeeTypeDto, "Notes", 1, 28, 128, 100000, "Dorothy", "D",
            "Mahoko", new DateTime(), "South Africa", "South African", "0000080000000", " ",
            new DateTime(), null, Models.Enums.Race.Black, Models.Enums.Gender.Male, null,
            "texample@retrorabbit.co.za", "test.example@gmail.com", "0000000000", null, null, employeeAddressDto, employeeAddressDto, null, null, null);
    }

    public EmployeeDocument CreateEmployeeDocument(EmployeeDto? employee = null)
    {
        EmployeeDocument employeeDocument = new EmployeeDocument
        {
            Id = 1,
            EmployeeId = 1,
            Status = Models.Enums.DocumentStatus.Approved
        };

        if (employee != null)
            employeeDocument.Employee = new Employee(employee, employee.EmployeeType);

        return employeeDocument;
    }

    [Fact]
    public void EmployeeDocumentTest()
    {
        var employeeDocument = new EmployeeDocument();
        Assert.IsType<EmployeeDocument>(employeeDocument);
        Assert.NotNull(employeeDocument);
    }

    [Fact]
    public void EmployeeDocumentToDTO()
    {
        var employeeDocument = CreateEmployeeDocument(
            employee: _employee);
        var dto = employeeDocument.ToDto();

        Assert.Equal(dto.Employee!.Id, employeeDocument.Employee.Id);

        var initializedEmployeeDocument = new EmployeeDocument(dto);

        Assert.Null(initializedEmployeeDocument.Employee);

        dto = initializedEmployeeDocument.ToDto();

        Assert.Null(dto.Employee);
    }
}
