using HRIS.Models;
using RR.Tests.Data.Models.HRIS;
using RR.UnitOfWork.Entities.HRIS;
using Xunit;

namespace RR.UnitOfWork.Tests.Entities;

public class EmployeeCertificationUnitTests
{
    private readonly EmployeeDto _employee;
    private readonly EmployeeCertificationDto _employeeCertificate;
    private readonly EmployeeCertificationDto certificateDto = new EmployeeCertificationDto
    {
        Id = 1,
        IssueDate = DateTime.Now,
        IssueOrganization = "From",
        CertificateDocument = "base64",
        CertificateName = "Name",
    };

    public EmployeeCertificationUnitTests()
    {

        _employeeCertificate = new EmployeeCertificationDto
        {
            Id = 1,
            EmployeeId = EmployeeTestData.EmployeeOne.Id,
            IssueDate = DateTime.Now,
            IssueOrganization= "String",
            CertificateDocument = "as",
            CertificateName = "Name",
            DocumentName = "hello"
        };

    }

    public EmployeeCertification CreateEmployeeCertification(EmployeeDto? employee = null, EmployeeCertificationDto? certificate = null)
    {

        EmployeeCertificationDto certificateDto = new EmployeeCertificationDto
        {
            Id = 1,
            IssueDate = DateTime.Now,
            IssueOrganization = "From",
            CertificateDocument = "base64",
            CertificateName = "Name",
            DocumentName = "hello"
        };
        var employeeCertification = new EmployeeCertification(certificateDto);

        if (employee != null)
            employeeCertification.Employee = new Employee(employee, employee.EmployeeType!);

        if (certificate != null)
            employeeCertification = new EmployeeCertification(certificate);


        return employeeCertification;
    }

    [Fact]
    public void EmployeeCertificationTest()
    {
        var employeeCertification = new EmployeeCertification(certificateDto);
        Assert.IsType<EmployeeCertification>(employeeCertification);
        Assert.NotNull(employeeCertification);
    }

    [Fact]
    public void EmployeeCertificationToDTO()
    {
        var employeeCertification = CreateEmployeeCertification(_employee, _employeeCertificate);
        var dto = employeeCertification.ToDto();

        Assert.NotNull(dto);
        Assert.NotNull(employeeCertification.ToDto());

        var initializedEmployeeCertification = new EmployeeCertification(dto);

        Assert.Null(initializedEmployeeCertification.Employee);
        Assert.NotNull(initializedEmployeeCertification);

        employeeCertification = CreateEmployeeCertification(
                                                            _employee,
                                                            _employeeCertificate);
        dto = employeeCertification.ToDto();

        Assert.NotNull(dto);

        employeeCertification = CreateEmployeeCertification(_employee, _employeeCertificate);
        dto = employeeCertification.ToDto();

        Assert.NotNull(dto);

        employeeCertification = CreateEmployeeCertification(certificate: _employeeCertificate);
        dto = employeeCertification.ToDto();

        Assert.NotNull(dto);
    }
}
