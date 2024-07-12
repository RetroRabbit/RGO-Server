using System.Linq.Expressions;
using HRIS.Services.Interfaces;
using HRIS.Services.Services;
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
    private readonly Mock<IUnitOfWork> _unitOfWork = new();

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
        _employeeCertificationService = new EmployeeCertificationService(_unitOfWork.Object);
    }

    private void MockEmployeeRepositorySetup(Employee employee)
    {
        _unitOfWork.Setup(u => u.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
                   .Returns(employee.ToMockIQueryable());
    }

    private void MockEmployeeRepositorySetupWithEmployee(Employee employee)
    {
        _unitOfWork.Setup(u => u.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
                   .Returns(employee.ToMockIQueryable());
    }

    private void MockEmployeeCertificationRepositorySetupWithCertification(
        EmployeeCertification employeeCertification)
    {
        _unitOfWork.Setup(u => u.EmployeeCertification.Get(It.IsAny<Expression<Func<EmployeeCertification, bool>>>()))
                   .Returns(employeeCertification.ToMockIQueryable());
    }

    private void MockEmployeeCertificationRepositorySetupForAddOrUpdate(
        EmployeeCertification employeeCertification, bool isAdd)
    {
        if (isAdd)
            _unitOfWork.Setup(u => u.EmployeeCertification.Add(It.IsAny<EmployeeCertification>()))
                       .ReturnsAsync(employeeCertification);
        else
            _unitOfWork.Setup(u => u.EmployeeCertification.Update(It.IsAny<EmployeeCertification>()))
                       .ReturnsAsync(employeeCertification);
    }

    private void MockEmployeeCertificationRepositorySetupForDelete(EmployeeCertification employeeCertificationDto)
    {
        _unitOfWork.Setup(u => u.EmployeeCertification.Delete(It.IsAny<int>()))
                   .ReturnsAsync(employeeCertificationDto);
    }

    private void MockEmployeeRepositorySetupWithEmployeeAsync(Employee employee)
    {
        _unitOfWork.Setup(u => u.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
                   .Returns(employee.ToMockIQueryable());
    }

    private void MockEmployeeCertificationRepositorySetupEmptyAsync()
    {
        _unitOfWork.Setup(u => u.EmployeeCertification.Get(It.IsAny<Expression<Func<EmployeeCertification, bool>>>()))
                   .Returns(new List<EmployeeCertification>().ToMockIQueryable());
    }

    private void MockEmployeeCertificationRepositorySetupForAdd(EmployeeCertification employeeCertification)
    {
        _unitOfWork.Setup(u => u.EmployeeCertification.Add(It.IsAny<EmployeeCertification>()))
                   .ReturnsAsync(employeeCertification);
    }

    [Fact]
    public async Task SaveEmployeeCertificationPass()
    {
        MockEmployeeRepositorySetupWithEmployeeAsync(EmployeeTestData.EmployeeOne);
        MockEmployeeCertificationRepositorySetupEmptyAsync();
        MockEmployeeCertificationRepositorySetupForAdd(_employeeCertification);

        var result = await _employeeCertificationService.SaveEmployeeCertification(_employeeCertification.ToDto());

        Assert.NotNull(result);
    }

    [Fact]
    public async Task SaveEmployeeCertificationFailWhenEmployeeNotFound()
    {
        _unitOfWork.Setup(u => u.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
                   .Returns(new List<Employee>().ToMockIQueryable());

        await Assert.ThrowsAsync<CustomException>(() =>
                    _employeeCertificationService.SaveEmployeeCertification(_employeeCertification.ToDto()));

    }

    [Fact]
    public async Task GetEmployeeCertificationPass()
    {
        MockEmployeeRepositorySetup(EmployeeTestData.EmployeeOne);
        MockEmployeeCertificationRepositorySetupWithCertification(_employeeCertification);

        var result =
            await _employeeCertificationService
            .GetEmployeeCertification(_employeeCertification.EmployeeId, _employeeCertification.Id);

        Assert.NotNull(result);
    }

    [Fact]
    public async Task GetAllEmployeeCertificationsPass()
    {
        MockEmployeeRepositorySetupWithEmployee(EmployeeTestData.EmployeeOne);

        var employeeCertificationList = new List<EmployeeCertification>
        {
            _employeeCertification,
            _employeeCertification
        };

        _unitOfWork.Setup(u => u.EmployeeCertification.Get(It.IsAny<Expression<Func<EmployeeCertification, bool>>>()))
                   .Returns(employeeCertificationList.ToMockIQueryable());

        var results =
            await _employeeCertificationService.GetAllEmployeeCertifications(_employeeCertification.EmployeeId);

        Assert.NotNull(results);
        Assert.Equal(2, results.Count);
    }

    [Fact]
    public async Task UpdateEmployeeCertificationPass()
    {
        MockEmployeeRepositorySetupWithEmployee(EmployeeTestData.EmployeeOne);
        MockEmployeeCertificationRepositorySetupForAddOrUpdate(_employeeCertification, false);

        var result = await _employeeCertificationService.UpdateEmployeeCertification(_employeeCertification.ToDto());

        Assert.NotNull(result);
    }

    [Fact]
    public async Task DeleteEmployeeCertificationPass()
    {
        MockEmployeeRepositorySetupWithEmployee(EmployeeTestData.EmployeeOne);
        MockEmployeeCertificationRepositorySetupForDelete(_employeeCertification);

        var result = await _employeeCertificationService.DeleteEmployeeCertification(_employeeCertification.Id);

        Assert.NotNull(result);
    }

    [Fact]
    public async Task GetEmployeeCertificationFailWhenCertificateNotFound()
    {
        MockEmployeeRepositorySetupWithEmployee(EmployeeTestData.EmployeeOne);

        _unitOfWork.Setup(u => u.EmployeeCertification.Get(It.IsAny<Expression<Func<EmployeeCertification, bool>>>()))
                   .Returns(new List<EmployeeCertification>().ToMockIQueryable());

        await Assert.ThrowsAsync<CustomException>(() =>
            _employeeCertificationService.GetEmployeeCertification(_employeeCertification.EmployeeId, _employeeCertification.Id));

    }

    [Fact]
    public async Task GetEmployeeCertificationFailWhenEmployeeNotFound()
    {
        _unitOfWork.Setup(u => u.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
                   .Returns(new List<Employee>().ToMockIQueryable());

        await Assert.ThrowsAsync<CustomException>(() =>
                    _employeeCertificationService.GetEmployeeCertification(0, 0));
    }

    [Fact]
    public async Task UpdateEmployeeCertificationFailWhenEmployeeNotFound()
    {
        _unitOfWork.Setup(u => u.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
                   .Returns(new List<Employee>().ToMockIQueryable());

        await Assert.ThrowsAsync<CustomException>(() =>
                    _employeeCertificationService.UpdateEmployeeCertification(_employeeCertification.ToDto()));
    }


    [Fact]
    public async Task GetAllEmployeeCertificationsFailWhenEmployeeNotFound()
    {
        _unitOfWork.Setup(u => u.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
                   .Returns(new List<Employee>().ToMockIQueryable());

        await Assert.ThrowsAsync<CustomException>(() =>
                    _employeeCertificationService.GetAllEmployeeCertifications(EmployeeTestData.EmployeeOne.Id));
    }
}