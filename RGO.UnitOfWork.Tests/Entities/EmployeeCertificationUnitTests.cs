using RGO.Models;
using RGO.Models.Enums;
using RGO.UnitOfWork.Entities;
using System.Text;
using Xunit;

namespace RGO.UnitOfWork.Tests.Entities;

public class EmployeeCertificationUnitTests
{
    private EmployeeDto _employee;
    private EmployeeDocumentDto _employeeDocument;

    public EmployeeCertificationUnitTests()
    {
        EmployeeTypeDto employeeTypeDto = new EmployeeTypeDto(1, "Developer");
        EmployeeAddressDto employeeAddressDto = new EmployeeAddressDto(1, "2", "Complex", "2", "Suburb/District", "City", "Country", "Province", "1620");

        _employee = new EmployeeDto(1, "001", "34434434", new DateTime(), new DateTime(),
            null, false, "None", 4, employeeTypeDto, "Notes", 1, 28, 128, 100000, "Dorothy", "D",
            "Mahoko", new DateTime(), "South Africa", "South African", "0000080000000", " ",
            new DateTime(), null, Models.Enums.Race.Black, Models.Enums.Gender.Male, null,
            "texample@retrorabbit.co.za", "test.example@gmail.com", "0000000000", null, null, employeeAddressDto, employeeAddressDto, null, null, null);


        _employeeDocument = new EmployeeDocumentDto(1, 1, "", "CVE256", FileCategory.Medical, "Picture", DocumentStatus.Approved, DateTime.Now, null, false);
    }

    public EmployeeCertification CreateEmployeeCertification(EmployeeDto? employee = null, EmployeeDocumentDto? employeeDocument = null, EmployeeDto? auditBy = null)
    {
        EmployeeCertification employeeCertification = new EmployeeCertification
        {
            Id = 1,
            EmployeeId = 1,
            EmployeeDocumentId = 1,
            Title = "Title",
            Publisher = "Publisher",
            Status = EmployeeCertificationStatus.Pending,
            AuditBy = 1,
            AuditDate = DateTime.Now,
            AuditNote = "AuditNote"
        };

        if (employee != null)
            employeeCertification.Employee = new Employee(employee, employee.EmployeeType);

        if (employeeDocument != null)
            employeeCertification.EmployeeDocument = new EmployeeDocument(employeeDocument);

        if (auditBy != null)
            employeeCertification.EmployeeAuditBy = new Employee(auditBy, auditBy.EmployeeType);

        return employeeCertification;
    }

    [Fact]
    public void EmployeeCertificationTest()
    {
        var employeeCertification = new EmployeeCertification();
        Assert.IsType<EmployeeCertification>(employeeCertification);
        Assert.NotNull(employeeCertification);
    }

    [Fact]
    public void EmployeeCertificationToDTO()
    {
        var employeeCertification = CreateEmployeeCertification(
            employee: _employee,
            employeeDocument: _employeeDocument,
            auditBy: _employee);
        var dto = employeeCertification.ToDto();

        Assert.NotNull(dto.Employee);
        Assert.NotNull(dto.EmployeeDocument);
        Assert.NotNull(dto.AuditBy);

        var initializedEmployeeCertification = new EmployeeCertification(dto);

        Assert.Null(initializedEmployeeCertification.Employee);
        Assert.Null(initializedEmployeeCertification.EmployeeDocument);
        Assert.Null(initializedEmployeeCertification.EmployeeAuditBy);

        employeeCertification = CreateEmployeeCertification(
            employee: _employee,
            employeeDocument: _employeeDocument);
        dto = employeeCertification.ToDto();

        Assert.NotNull(dto.Employee);
        Assert.NotNull(dto.EmployeeDocument);
        Assert.Null(dto.AuditBy);

        employeeCertification = CreateEmployeeCertification(
            employee: _employee,
            auditBy: _employee);
        dto = employeeCertification.ToDto();

        Assert.NotNull(dto.Employee);
        Assert.Null(dto.EmployeeDocument);
        Assert.NotNull(dto.AuditBy);

        employeeCertification = CreateEmployeeCertification(
            employeeDocument: _employeeDocument,
            auditBy: _employee);
        dto = employeeCertification.ToDto();

        Assert.Null(dto.Employee);
        Assert.NotNull(dto.EmployeeDocument);
        Assert.NotNull(dto.AuditBy);
    }
}
