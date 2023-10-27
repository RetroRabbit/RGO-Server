using RGO.Models;
using RGO.UnitOfWork.Entities;
using Xunit;

namespace RGO.UnitOfWork.Tests.Entities;

public class EmployeeDocumentUnitTests
{
    private EmployeeDto _employee;
    private OnboardingDocumentDto _onboardingDocument;

    public EmployeeDocumentUnitTests()
    {
        EmployeeTypeDto employeeTypeDto = new EmployeeTypeDto(1, "Developer");
        EmployeeAddressDto employeeAddressDto = new EmployeeAddressDto(1, "2", "Complex", "2", "Suburb/District", "City", "Country", "Province", "1620");

        _employee = new EmployeeDto(1, "001", "34434434", new DateOnly(), new DateOnly(),
            null, false, "None", 4, employeeTypeDto, "Notes", 1, 28, 128, 100000, "Ms", "Dorothy", "D",
            "Mahoko", new DateOnly(), "South Africa", "South African", "0000080000000", " ",
            new DateOnly(), null, Models.Enums.Race.Black, Models.Enums.Gender.Male, null,
            "texample@retrorabbit.co.za", "test.example@gmail.com", "0000000000", null, null, employeeAddressDto, employeeAddressDto, null, null, null);

        _onboardingDocument = new OnboardingDocumentDto(0, "Title", "Description", "FileName", new byte[] { 0 }, Models.Enums.ItemStatus.Active);
    }

    public EmployeeDocument CreateEmployeeDocument(EmployeeDto? employee = null, OnboardingDocumentDto? onboardingDocument = null)
    {
        EmployeeDocument employeeDocument = new EmployeeDocument
        {
            Id = 1,
            EmployeeId = 1,
            OnboardingDocumentId = 1,
            Status = Models.Enums.ItemStatus.Active
        };

        if (employee != null)
            employeeDocument.Employee = new Employee(employee, employee.EmployeeType);

        if (onboardingDocument != null)
            employeeDocument.OnboardingDocument = new OnboardingDocument(onboardingDocument);

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
            employee: _employee,
            onboardingDocument: _onboardingDocument);
        var dto = employeeDocument.ToDto();

        Assert.Equal(dto.Employee!.Id, employeeDocument.Employee.Id);
        Assert.Equal(dto.OnboardingDocument!.Id, employeeDocument.OnboardingDocument.Id);

        var initializedEmployeeDocument = new EmployeeDocument(dto);

        Assert.Null(initializedEmployeeDocument.Employee);
        Assert.Null(initializedEmployeeDocument.OnboardingDocument);

        dto = initializedEmployeeDocument.ToDto();

        Assert.Null(dto.Employee);
        Assert.Null(dto.OnboardingDocument);
    }
}
