using HRIS.Models;
using HRIS.Models.Enums;
using RR.UnitOfWork.Entities.HRIS;
using Xunit;

namespace RR.UnitOfWork.Tests.Entities;

public class EmployeeCertificationUnitTests
{
    private readonly EmployeeDto _employee;
    private readonly EmployeeDocumentDto _employeeCertificate;
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
        /*var employeeTypeDto = new EmployeeTypeDto { Id = 1, Name = "Developer" };
        var employeeAddressDto =
            new EmployeeAddressDto { Id = 1, UnitNumber = "2", ComplexName = "Complex", StreetNumber = "2", SuburbOrDistrict = "Suburb/District", City = "City", Country = "Country", Province = "Province", PostalCode = "1620" };

        _employee = new EmployeeDto
        {
            Id = 1,
            EmployeeNumber = "001",
            TaxNumber = "34434434",
            EngagementDate = DateTime.Now,
            TerminationDate = null,
            PeopleChampion = null,
            Disability = false,
            DisabilityNotes = "None",
            Level = 4,
            EmployeeType = employeeTypeDto,
            Notes = "Notes",
            LeaveInterval = 1,
            SalaryDays = 28,
            PayRate = 128,
            Salary = 100000,
            Name = "Dorothy",
            Initials = "D",
            Surname = "Mahoko",
            DateOfBirth = DateTime.Now,
            CountryOfBirth = "South Africa",
            Nationality = "South African",
            IdNumber = "0000080000000",
            PassportNumber = " ",
            PassportExpirationDate = DateTime.Now,
            PassportCountryIssue = "South Africa",
            Race = Race.Black,
            Gender = Gender.Male,
            Photo = null,
            Email = "texample@retrorabbit.co.za",
            PersonalEmail = "test.example@gmail.com",
            CellphoneNo = "0000000000",
            ClientAllocated = null,
            TeamLead = null,
            PhysicalAddress = employeeAddressDto,
            PostalAddress = employeeAddressDto,
            HouseNo = null,
            EmergencyContactName = null,
            EmergencyContactNo = null
        };*/

        _employeeCertificate = new EmployeeDocumentDto
        {
            Id = 1,
            EmployeeId = 1,
            Reference = "",
            FileName = "CVE256",
            FileCategory = FileCategory.Medical,
            Blob = "Picture",
            Status = DocumentStatus.Approved,
            UploadDate = DateTime.Now,
            Reason = null,
            CounterSign = false
        };

    }

    public EmployeeCertification CreateEmployeeCertification(EmployeeDto? employee = null, EmployeeDocumentDto? employeeDocument = null)
    {

        EmployeeCertificationDto certificateDto = new EmployeeCertificationDto
        {
            Id = 1,
            IssueDate = DateTime.Now,
            IssueOrganization = "From",
            CertificateDocument = "base64",
            CertificateName = "Name",
        };
        var employeeCertification = new EmployeeCertification(certificateDto);

        //if (employee != null)
        //    employeeCertification.Employee = new Employee(employee, employee.EmployeeType!);

        //if (employeeDocument != null)
        //    employeeCertification.EmployeeDocument = new EmployeeDocument(employeeDocument);

        //if (auditBy != null)
        //    employeeCertification.EmployeeAuditBy = new Employee(auditBy, auditBy.EmployeeType!);

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
/*
        Assert.NotNull(dto.Employee);
        Assert.NotNull(dto.EmployeeDocument);*/
        Assert.NotNull(employeeCertification.ToDto());

        var initializedEmployeeCertification = new EmployeeCertification(dto);

        /*Assert.Null(initializedEmployeeCertification.Employee);
        Assert.Null(initializedEmployeeCertification.EmployeeDocument);*/
        Assert.NotNull(initializedEmployeeCertification);

        employeeCertification = CreateEmployeeCertification(
                                                            _employee,
                                                            _employeeCertificate);
        dto = employeeCertification.ToDto();

        /*Assert.NotNull(dto.Employee);
        Assert.NotNull(dto.EmployeeDocument);*/
        Assert.NotNull(dto);

        employeeCertification = CreateEmployeeCertification(_employee, _employeeCertificate);
        dto = employeeCertification.ToDto();

       /* Assert.NotNull(dto.Employee);
        Assert.Null(dto.EmployeeDocument);*/
        Assert.NotNull(dto);

        employeeCertification = CreateEmployeeCertification(employeeDocument: _employeeCertificate);
        dto = employeeCertification.ToDto();

        /*Assert.Null(dto.Employee);
        Assert.NotNull(dto.EmployeeDocument);*/
        Assert.NotNull(dto);
    }
}
