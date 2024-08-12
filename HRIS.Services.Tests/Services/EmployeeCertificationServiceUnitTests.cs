using System.Linq.Expressions;
using HRIS.Models;
using HRIS.Services.Interfaces;
using HRIS.Services.Services;
using HRIS.Services.Session;
using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
using RR.Tests.Data;
using RR.Tests.Data.Models.HRIS;
using RR.UnitOfWork;
using RR.UnitOfWork.Entities.HRIS;
using Xunit;

namespace HRIS.Services.Tests.Services;
public class EmployeeCertificationServiceUnitTests
{
    private readonly EmployeeCertificationService _employeeCertificationService;
    private readonly Mock<IUnitOfWork> _db = new();
    private readonly AuthorizeIdentityMock _identity = new AuthorizeIdentityMock();

    private readonly EmployeeCertification _employeeCertification = new()
    {
        Id = 1,
        EmployeeId = EmployeeTestData.EmployeeOne.Id,
        CertificateDocument = "base64",
        CertificateName = "Title",
        IssueOrganization = "Publisher",
        IssueDate = DateTime.UtcNow,
    };

    public EmployeeCertificationServiceUnitTests()
    {
        _identity = new AuthorizeIdentityMock("test@gmail.com", "test", "Admin", 1);
        _employeeCertificationService = new EmployeeCertificationService(_db.Object, _identity);
    }

    private void MockEmployeeRepositorySetup(Employee employee)
    {
        _db.Setup(u => u.Employee.Any(It.IsAny<Expression<Func<Employee, bool>>>()))
       .ReturnsAsync(true);

        _db.Setup(u => u.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
            .Returns(new List<Employee> { employee }.AsQueryable().BuildMock());
    }

    private void MockEmployeeRepositorySetupWithEmployee(Employee employee)
    {
        _db.Setup(u => u.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
                   .Returns(employee.ToMockIQueryable());
    }

    private void MockEmployeeCertificationRepositorySetupWithCertification(EmployeeCertification employeeCertification)
    {
        _db.Setup(u => u.EmployeeCertification.Get(It.IsAny<Expression<Func<EmployeeCertification, bool>>>()))
                   .Returns(employeeCertification.ToMockIQueryable());
    }

    private void MockEmployeeCertificationRepositorySetupForAddOrUpdate(EmployeeCertification employeeCertification, bool isAdd)
    {
        if (isAdd)
            _db.Setup(u => u.EmployeeCertification.Add(It.IsAny<EmployeeCertification>()))
                       .ReturnsAsync(employeeCertification);
        else
            _db.Setup(u => u.EmployeeCertification.Update(It.IsAny<EmployeeCertification>()))
                       .ReturnsAsync(employeeCertification);
    }

    private void MockEmployeeCertificationRepositorySetupForDelete(EmployeeCertification employeeCertificationDto)
    {
        _db.Setup(u => u.EmployeeCertification.Delete(It.IsAny<int>()))
                   .ReturnsAsync(employeeCertificationDto);
    }

    private void MockEmployeeCertificationRepositorySetupWithEmployeeAsync(Employee employee)
    {
        _db.Setup(u => u.Employee.Any(It.IsAny<Expression<Func<Employee, bool>>>()))
                   .ReturnsAsync(true);

        _db.Setup(u => u.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()));
    }

    private void MockEmployeeCertificationRepositorySetupEmptyAsync()
    {
        _db.Setup(u => u.EmployeeCertification.Any(It.IsAny<Expression<Func<EmployeeCertification, bool>>>()))
                   .ReturnsAsync(false);
    }

    private void MockEmployeeCertificationRepositorySetupForAdd(EmployeeCertification employeeCertification)
    {
        _db.Setup(u => u.EmployeeCertification.Add(It.IsAny<EmployeeCertification>()))
                   .ReturnsAsync(employeeCertification);
    }

    private void MockCheckIfCertificationExists(bool exists)
    {
        _db.Setup(u => u.EmployeeCertification.Any(It.IsAny<Expression<Func<EmployeeCertification, bool>>>()))
            .ReturnsAsync(exists);
    }

    [Fact]
    public async Task SaveEmployeeCertificationPass()
    {
        MockCheckIfCertificationExists(true);
        MockEmployeeCertificationRepositorySetupForAdd(_employeeCertification);

        var result = await _employeeCertificationService.CreateEmployeeCertification(_employeeCertification.ToDto());

        Assert.NotNull(result);
        _db.Verify(x => x.EmployeeCertification.Add(It.IsAny<EmployeeCertification>()), Times.Once);
    }

    [Fact]
    public async Task SaveEmployeeCertificationFailWhenCertificationExists()
    {
       _db.Setup(u => u.EmployeeCertification.Any(It.IsAny<Expression<Func<EmployeeCertification, bool>>>()))
               .ReturnsAsync(true);

        MockEmployeeRepositorySetup(EmployeeTestData.EmployeeOne);

        await Assert.ThrowsAsync<NullReferenceException>(() =>
            _employeeCertificationService.CreateEmployeeCertification(_employeeCertification.ToDto()));
    }

    [Fact]
    public async Task SaveEmployeeCertificationFailWhenExceptionThrown()
    {
        MockCheckIfCertificationExists(true);

        var identity = new AuthorizeIdentityMock("test@gmail.com", "test", "Employee", 2);
        var employeeCertificationService = new EmployeeCertificationService(_db.Object, identity);

        _db.Setup(u => u.EmployeeCertification.Any(It.IsAny<Expression<Func<EmployeeCertification, bool>>>()))
                .ReturnsAsync(true);

        MockEmployeeRepositorySetup(EmployeeTestData.EmployeeOne);

        await Assert.ThrowsAsync<CustomException>(() =>
            employeeCertificationService.CreateEmployeeCertification(_employeeCertification.ToDto()));
    }

    [Fact]
    public async Task SaveEmployeeCertificationFailWhenEmployeeNotFound()
    {
        _db.Setup(u => u.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
                   .Returns(new List<Employee>().ToMockIQueryable());

        await Assert.ThrowsAsync<NullReferenceException>(() =>
                    _employeeCertificationService.CreateEmployeeCertification(_employeeCertification.ToDto()));
    }

    [Fact]
    public async Task SaveEmployeeCertificationFailWhenCertificationNotFound()
    {
        MockCheckIfCertificationExists(false);

        await Assert.ThrowsAsync<CustomException>(() =>
            _employeeCertificationService.CreateEmployeeCertification(_employeeCertification.ToDto()));
    }

    [Fact]
    public async Task UpdateEmployeeCertificationPass()
    {
        _db.Setup(u => u.Employee.Any(It.IsAny<Expression<Func<Employee, bool>>>()))
                   .ReturnsAsync(true);

        _db.Setup(u => u.EmployeeCertification.Any(It.IsAny<Expression<Func<EmployeeCertification, bool>>>()))
                   .ReturnsAsync(true);

        MockEmployeeCertificationRepositorySetupForAddOrUpdate(_employeeCertification, false);

        var result = await _employeeCertificationService.UpdateEmployeeCertification(_employeeCertification.ToDto());
        Assert.NotNull(result);
    }

    [Fact]
    public async Task UpdateEmployeeCertificationFailWhenEmployeeNotFound()
    {
        _db.Setup(u => u.Employee.Any(It.IsAny<Expression<Func<Employee, bool>>>()))
                   .ReturnsAsync(false);

        _db.Setup(u => u.EmployeeCertification.Any(It.IsAny<Expression<Func<EmployeeCertification, bool>>>()))
                   .ReturnsAsync(true);

        await Assert.ThrowsAsync<NullReferenceException>(() =>
                    _employeeCertificationService.UpdateEmployeeCertification(_employeeCertification.ToDto()));
    }

    [Fact]
    public async Task UpdateEmployeeCertificationFailWhenExceptionThrown()
    {
        var employeeCertificationDto = new EmployeeCertificationDto
        {
            Id = 1,
            EmployeeId = 2,
            CertificateDocument = "base64",
            CertificateName = "Title",
            IssueOrganization = "Publisher",
            IssueDate = DateTime.UtcNow,
        };

        _db.Setup(u => u.EmployeeCertification.Any(It.IsAny<Expression<Func<EmployeeCertification, bool>>>()))
           .ReturnsAsync(true);

        var identity = new AuthorizeIdentityMock("test@gmail.com", "test", "Employee", 3);
        var employeeCertificationService = new EmployeeCertificationService(_db.Object, identity);

        var exception = await Assert.ThrowsAsync<CustomException>(() =>
            employeeCertificationService.UpdateEmployeeCertification(employeeCertificationDto));

        Assert.Equal("Unauthorized access.", exception.Message);
    }

    [Fact]
    public async Task UpdateEmployeeCertificationFailWhenCertificateNotFound()
    {
        var employeeCertificationDto = new EmployeeCertificationDto
        {
            Id = 1,
            EmployeeId = 2,
            CertificateDocument = "base64",
            CertificateName = "Title",
            IssueOrganization = "Publisher",
            IssueDate = DateTime.UtcNow,
        };

        _db.Setup(u => u.EmployeeCertification.Any(It.IsAny<Expression<Func<EmployeeCertification, bool>>>()))
           .ReturnsAsync(false);

        var exception = await Assert.ThrowsAsync<CustomException>(() =>
            _employeeCertificationService.UpdateEmployeeCertification(employeeCertificationDto));

        Assert.Equal("Certificate not found", exception.Message);
    }

    [Fact]
    public async Task DeleteEmployeeCertificationPass()
    {
        _db.Setup(u => u.EmployeeCertification.Any(It.IsAny<Expression<Func<EmployeeCertification, bool>>>()))
                   .ReturnsAsync(true);

        MockEmployeeCertificationRepositorySetupForDelete(_employeeCertification);

        var result = await _employeeCertificationService.DeleteEmployeeCertification(_employeeCertification.Id);

        Assert.NotNull(result);
        _db.Verify(x => x.EmployeeCertification.Delete(It.IsAny<int>()), Times.Once);
    }

    [Fact]
    public async Task DeleteEmployeeCertification_CertificateNotFound()
    {
        _db.Setup(u => u.EmployeeCertification.Any(It.IsAny<Expression<Func<EmployeeCertification, bool>>>()))
                   .ReturnsAsync(false);

        var exception = await Assert.ThrowsAsync<CustomException>(() =>
            _employeeCertificationService.DeleteEmployeeCertification(_employeeCertification.Id));

        Assert.Equal("Certificate not found", exception.Message);
    }

    [Fact]
    public async Task DeleteEmployeeCertificationFailWhenExceptionThrown()
    {
        var identity = new AuthorizeIdentityMock("test@gmail.com", "test", "Employee", 2);
        var employeeCertificationService = new EmployeeCertificationService(_db.Object, identity);

        _db.Setup(u => u.EmployeeCertification.Any(It.IsAny<Expression<Func<EmployeeCertification, bool>>>()))
           .ReturnsAsync(true);

        var exception = await Assert.ThrowsAsync<CustomException>(() =>
            employeeCertificationService.DeleteEmployeeCertification(_employeeCertification.Id));

        Assert.Equal("Unauthorized access.", exception.Message);
    }

    [Fact]
    public async Task GetEmployeeCertificationFailWhenCertificateNotFound()
    {
        var employeeId = 1;
        var certificationId = 2;

        _db.Setup(u => u.Employee.Any(It.IsAny<Expression<Func<Employee, bool>>>()))
           .ReturnsAsync(true);

        _db.Setup(u => u.EmployeeCertification.Any(It.IsAny<Expression<Func<EmployeeCertification, bool>>>()))
           .ReturnsAsync(false);

        var exception = await Assert.ThrowsAsync<CustomException>(() =>
            _employeeCertificationService.GetEmployeeCertificationByEmployeeIdAndCertificationId(employeeId, certificationId));

        Assert.Equal("Certificate not found", exception.Message);
    }

    [Fact]
    public async Task GetEmployeeCertificationFailWhenEmployeeNotFound()
    {
        _db.Setup(u => u.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
                   .Returns(new List<Employee>().ToMockIQueryable());

        await Assert.ThrowsAsync<CustomException>(() =>
                    _employeeCertificationService.GetEmployeeCertificationByEmployeeIdAndCertificationId(0, 0));
    }

    [Fact]
    public async Task GetAllEmployeeCertificationsFailWhenEmployeeNotFound()
    {
        _db.Setup(u => u.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
                   .Returns(new List<Employee>().ToMockIQueryable());

        await Assert.ThrowsAsync<CustomException>(() =>
                    _employeeCertificationService.GetEmployeeCertificationsByEmployeeId(EmployeeTestData.EmployeeOne.Id));
    }

    [Fact]
    public async Task GetEmployeeCertificationPass()
    {
        _db.Setup(u => u.Employee.Any(It.IsAny<Expression<Func<Employee, bool>>>()))
                   .ReturnsAsync(true);

        _db.Setup(u => u.EmployeeCertification.Any(It.IsAny<Expression<Func<EmployeeCertification, bool>>>()))
                   .ReturnsAsync(true);

        MockEmployeeCertificationRepositorySetupWithCertification(_employeeCertification);

        var result = await _employeeCertificationService.GetEmployeeCertificationByEmployeeIdAndCertificationId(_employeeCertification.EmployeeId, _employeeCertification.Id);

        Assert.NotNull(result);
    }

    [Fact]
    public async Task GetAllEmployeeCertificationsPass()
    {
        _db.Setup(u => u.Employee.Any(It.IsAny<Expression<Func<Employee, bool>>>()))
                   .ReturnsAsync(true);

        var employeeCertificationList = new List<EmployeeCertification>
        {
           _employeeCertification,
           _employeeCertification
        };

        _db.Setup(u => u.EmployeeCertification.Get(It.IsAny<Expression<Func<EmployeeCertification, bool>>>()))
                   .Returns(employeeCertificationList.ToMockIQueryable());

        var results = await _employeeCertificationService.GetEmployeeCertificationsByEmployeeId(_employeeCertification.EmployeeId);

        Assert.NotNull(results);
        Assert.Equal(2, results.Count);
    }

    [Fact]
    public async Task GetEmployeeCertificationFailWhenEmployeeCertificationRecordNotFound()
    {
        var employeeId = 1;
        var certificationId = 2;

        _db.Setup(ex => ex.Employee.Any(It.IsAny<Expression<Func<Employee, bool>>>()))
           .ReturnsAsync(true);

        _db.Setup(ex => ex.EmployeeCertification.Any(It.IsAny<Expression<Func<EmployeeCertification, bool>>>()))
           .ReturnsAsync(true);

        var emptyList = new List<EmployeeCertification>().AsQueryable().BuildMock();
        _db.Setup(u => u.EmployeeCertification.Get(It.IsAny<Expression<Func<EmployeeCertification, bool>>>()))
           .Returns(emptyList);

        var exception = await Assert.ThrowsAsync<CustomException>(() =>
            _employeeCertificationService.GetEmployeeCertificationByEmployeeIdAndCertificationId(employeeId, certificationId));

        Assert.Equal("Employee certification record not found", exception.Message);
    }

    [Fact]
    public async Task GetEmployeeCertificationByEmployeeIdAndCertificationId_UnauthorizedAccess_ThrowsCustomException()
    {
        var unauthorizedIdentity = new AuthorizeIdentityMock("test@gmail.com", "test", "Employee", 999);
        var employeeCertificationService = new EmployeeCertificationService(_db.Object, unauthorizedIdentity);

        var employeeId = 1;
        var certificationId = 1;

        MockCheckIfCertificationExists(true);
        MockEmployeeRepositorySetup(EmployeeTestData.EmployeeOne);

        var exception = await Assert.ThrowsAsync<CustomException>(() =>
            employeeCertificationService.GetEmployeeCertificationByEmployeeIdAndCertificationId(employeeId, certificationId));

        Assert.Equal("Unauthorized access.", exception.Message);
    }

    [Fact]
    public async Task GetEmployeeCertificationsByEmployeeId_UnauthorizedAccess_ThrowsCustomException()
    {
        var unauthorizedIdentity = new AuthorizeIdentityMock("test@gmail.com", "test", "Employee", 999);
        var employeeCertificationService = new EmployeeCertificationService(_db.Object, unauthorizedIdentity);

        var employeeId = 1;

        MockEmployeeRepositorySetup(EmployeeTestData.EmployeeOne);

        var exception = await Assert.ThrowsAsync<CustomException>(() =>
            employeeCertificationService.GetEmployeeCertificationsByEmployeeId(employeeId));

        Assert.Equal("Unauthorized access.", exception.Message);
    }
}
