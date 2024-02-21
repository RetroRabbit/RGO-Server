using HRIS.Models;
using HRIS.Models.Enums;
using RR.UnitOfWork.Entities.HRIS;
using Xunit;

namespace RR.UnitOfWork.Tests.Entities;

public class EmployeeDocumentUnitTests
{
    private readonly EmployeeDto _employee;

    public EmployeeDocumentUnitTests()
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
    }

    public EmployeeDocument CreateEmployeeDocument(EmployeeDto? employee = null)
    {
        var employeeDocument = new EmployeeDocument
        {
            Id = 1,
            EmployeeId = 1,
            Status = DocumentStatus.Approved
        };

        if (employee != null)
            employeeDocument.Employee = new Employee(employee, employee.EmployeeType!);

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
                                                      _employee);
        var dto = employeeDocument.ToDto();

        Assert.Equal(dto.EmployeeId!, employeeDocument.Employee.Id);

        var initializedEmployeeDocument = new EmployeeDocument(dto);

        Assert.Null(initializedEmployeeDocument.Employee);

        dto = initializedEmployeeDocument.ToDto();
    }
}