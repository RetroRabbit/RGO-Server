/*using System.Linq.Expressions;
using HRIS.Models;
using HRIS.Models.Enums;
using HRIS.Services.Interfaces;
using HRIS.Services.Services;
using MockQueryable.Moq;
using Moq;
using RGO.Tests.Data.Models;
using RR.Tests.Data.Models.HRIS;
using RR.UnitOfWork;
using RR.UnitOfWork.Entities.HRIS;
using Xunit;

namespace HRIS.Services.Tests.Services;

public class EmployeeCertificationServiceUnitTests
{
    private readonly EmployeeCertificationService _employeeCertificationService;
    private readonly Mock<IUnitOfWork> _unitOfWork = new();
    private readonly Mock<IErrorLoggingService> _errorLoggingServiceMock;

    private readonly EmployeeCertificationDto employeeCertificationDto = new EmployeeCertificationDto
    {
        Id = 1,
        EmployeeId = EmployeeTestData.EmployeeDto.Id,
        CertificateDocument = "base64",
        CertificateName = "Title",
        IssueOrganization = "Publisher",
        IssueDate = DateTime.UtcNow,

    };

    public EmployeeCertificationServiceUnitTests()
    {
        _errorLoggingServiceMock = new Mock<IErrorLoggingService>();
        _employeeCertificationService = new EmployeeCertificationService(_unitOfWork.Object, _errorLoggingServiceMock.Object);
    }

    private EmployeeCertificationDto CreateEmployeeCertificationDto()
    {
        return employeeCertificationDto;
    }

    private void MockEmployeeRepositorySetup(EmployeeDto employeeDto)
    {
        var employeeList = new List<Employee> { new(employeeDto, employeeDto.EmployeeType!) };
        _unitOfWork.Setup(u => u.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
                   .Returns(employeeList.AsQueryable().BuildMock());
    }

    private void MockEmployeeCertificationRepositorySetup(EmployeeCertificationDto employeeCertificationDto)
    {
        var employeeCertificationList = new List<EmployeeCertification> { new(employeeCertificationDto) };
        _unitOfWork.Setup(u => u.EmployeeCertification.Get(It.IsAny<Expression<Func<EmployeeCertification, bool>>>()))
                   .Returns(employeeCertificationList.AsQueryable().BuildMock().Take(1));
    }

    private void MockEmployeeRepositorySetupWithEmployee(EmployeeDto employeeDto)
    {
        var employee = new Employee(employeeDto, employeeDto.EmployeeType!);
        var employeeList = new List<Employee> { employee };
        var mock = employeeList.AsQueryable().BuildMock();
        _unitOfWork.Setup(u => u.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
                   .Returns(mock);
    }

    private void MockEmployeeCertificationRepositorySetupWithCertification(
        EmployeeCertificationDto employeeCertificationDto)
    {
        var employeeCertification = new EmployeeCertification(employeeCertificationDto);
        var employeeCertificationList = new List<EmployeeCertification> { employeeCertification };
        var mock = employeeCertificationList.AsQueryable().BuildMock();
        _unitOfWork.Setup(u => u.EmployeeCertification.Get(It.IsAny<Expression<Func<EmployeeCertification, bool>>>()))
                   .Returns(mock);
    }

    private void MockEmployeeCertificationRepositorySetupForAddOrUpdate(
        EmployeeCertificationDto employeeCertificationDto, bool isAdd)
    {
        if (isAdd)
            _unitOfWork.Setup(u => u.EmployeeCertification.Add(It.IsAny<EmployeeCertification>()))
                       .ReturnsAsync(employeeCertificationDto);
        else
            _unitOfWork.Setup(u => u.EmployeeCertification.Update(It.IsAny<EmployeeCertification>()))
                       .ReturnsAsync(employeeCertificationDto);
    }

    private void MockEmployeeCertificationRepositorySetupForDelete(EmployeeCertificationDto employeeCertificationDto)
    {
        _unitOfWork.Setup(u => u.EmployeeCertification.Delete(It.IsAny<int>()))
                   .ReturnsAsync(employeeCertificationDto);
    }

    private void MockEmployeeRepositorySetupWithEmployeeAsync(EmployeeDto employeeDto)
    {
        var employee = new Employee(employeeDto, employeeDto.EmployeeType!);
        var employeeList = new List<Employee> { employee }.AsQueryable();

        var mock = employeeList.BuildMock();

        _unitOfWork.Setup(u => u.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
                   .Returns(mock);
    }

    private void MockEmployeeCertificationRepositorySetupEmptyAsync()
    {
        var employeeCertificationList = new List<EmployeeCertification>().AsQueryable();

        var mock = employeeCertificationList.BuildMock();

        _unitOfWork.Setup(u => u.EmployeeCertification.Get(It.IsAny<Expression<Func<EmployeeCertification, bool>>>()))
                   .Returns(mock);
    }

    private void MockEmployeeCertificationRepositorySetupForAdd(EmployeeCertificationDto employeeCertificationDto)
    {
        var employeeCertification = new EmployeeCertification(employeeCertificationDto);


        _unitOfWork.Setup(u => u.EmployeeCertification.Add(It.IsAny<EmployeeCertification>()))
                   .Returns(Task.FromResult(employeeCertificationDto));
    }

    [Fact]
    public async Task SaveEmployeeCertificationFailWhenEmployeeNotFound()
    { 
        var emptyEmployeeList = new List<Employee>().AsQueryable();
        var mock = emptyEmployeeList.BuildMock();
        _unitOfWork.Setup(u => u.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
                   .Returns(mock);

        var employeeCertificationDto = CreateEmployeeCertificationDto();

        _errorLoggingServiceMock.Setup(r => r.LogException(It.IsAny<Exception>()))
                                 .Throws(new Exception("Employee not found")); // Or a custom exception

        await Assert.ThrowsAsync<Exception>(() =>
                    _employeeCertificationService.SaveEmployeeCertification(employeeCertificationDto));

        _errorLoggingServiceMock.Verify(r => r.LogException(It.Is<Exception>(ex => ex.Message == "Employee not found")));
    }

    [Fact]
    public async Task SaveEmployeeCertificationPass()
    {
        var employeeCertificationDto = CreateEmployeeCertificationDto();

        MockEmployeeRepositorySetupWithEmployeeAsync(EmployeeTestData.EmployeeDto);
        MockEmployeeCertificationRepositorySetupEmptyAsync();
        MockEmployeeCertificationRepositorySetupForAdd(employeeCertificationDto);

        var result = await _employeeCertificationService.SaveEmployeeCertification(employeeCertificationDto);

        Assert.NotNull(result);
    }

    [Fact]
    public async Task GetEmployeeCertificationPass()
    {
        var employeeCertificationDto = CreateEmployeeCertificationDto();
        MockEmployeeRepositorySetup(EmployeeTestData.EmployeeDto);
        MockEmployeeCertificationRepositorySetupWithCertification(employeeCertificationDto);

        var result =
            await _employeeCertificationService
            .GetEmployeeCertification(employeeCertificationDto.EmployeeId, employeeCertificationDto.Id);

        Assert.NotNull(result);
    }

    [Fact]
    public async Task GetAllEmployeeCertificationsPass()
    {
        var employeeCertificationDto1 = CreateEmployeeCertificationDto();
        var employeeCertificationDto2 = CreateEmployeeCertificationDto();

        MockEmployeeRepositorySetupWithEmployee(EmployeeTestData.EmployeeDto);

        var employeeCertificationList = new List<EmployeeCertification>
        {
            new(employeeCertificationDto1),
            new(employeeCertificationDto2)
        };

        var mock = employeeCertificationList.AsQueryable().BuildMock();
        _unitOfWork.Setup(u => u.EmployeeCertification.Get(It.IsAny<Expression<Func<EmployeeCertification, bool>>>()))
                   .Returns(mock);

        var results =
            await _employeeCertificationService.GetAllEmployeeCertifications(employeeCertificationDto1.EmployeeId);

        Assert.NotNull(results);
        Assert.Equal(2, results.Count);
    }

    [Fact]
    public async Task UpdateEmployeeCertificationPass()
    {
        var employeeCertificationDto = CreateEmployeeCertificationDto();
        MockEmployeeRepositorySetupWithEmployee(EmployeeTestData.EmployeeDto);
        MockEmployeeCertificationRepositorySetupForAddOrUpdate(employeeCertificationDto, false);

        var result = await _employeeCertificationService.UpdateEmployeeCertification(employeeCertificationDto);

        Assert.NotNull(result);
    }

    [Fact]
    public async Task DeleteEmployeeCertificationPass()
    {
        var employeeCertificationDto = CreateEmployeeCertificationDto();
        MockEmployeeRepositorySetupWithEmployee(EmployeeTestData.EmployeeDto);
        MockEmployeeCertificationRepositorySetupForDelete(employeeCertificationDto);

        var result = await _employeeCertificationService.DeleteEmployeeCertification(employeeCertificationDto);

        Assert.NotNull(result);
    }

    [Fact]
    public async Task GetEmployeeCertificationFailWhenCertificateNotFound()
    {
        var employeeCertificationDto = CreateEmployeeCertificationDto();

        MockEmployeeRepositorySetupWithEmployee(EmployeeTestData.EmployeeDto);

        var emptyCertList = new List<EmployeeCertification>().AsQueryable();
        var certMock = emptyCertList.BuildMock();
        _unitOfWork.Setup(u => u.EmployeeCertification.Get(It.IsAny<Expression<Func<EmployeeCertification, bool>>>()))
                   .Returns(certMock);

        _errorLoggingServiceMock.Setup(r => r.LogException(It.IsAny<Exception>()))
                                 .Throws(new Exception("Employee certification record not found"));

        await Assert.ThrowsAsync<Exception>(() =>
            _employeeCertificationService.GetEmployeeCertification(employeeCertificationDto.EmployeeId, employeeCertificationDto.Id));

    }

    [Fact]
    public async Task GetEmployeeCertificationFailWhenEmployeeNotFound()
    {
        var emptyEmployeeList = new List<Employee>().AsQueryable();
        var mock = emptyEmployeeList.BuildMock();
        _unitOfWork.Setup(u => u.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
                   .Returns(mock);

        var employeeCertificationDto = CreateEmployeeCertificationDto();

        _errorLoggingServiceMock.Setup(r => r.LogException(It.IsAny<Exception>()))
                                 .Throws(new Exception("Employee not found")); // Or a custom exception

        await Assert.ThrowsAsync<Exception>(() =>
                    _employeeCertificationService.GetEmployeeCertification(0, 0));

        _errorLoggingServiceMock.Verify(r => r.LogException(It.Is<Exception>(ex => ex.Message == "Employee not found")));
    } 
    
    [Fact]
    public async Task UpdateEmployeeCertificationFailWhenEmployeeNotFound()
    {
        var emptyEmployeeList = new List<Employee>().AsQueryable();
        var mock = emptyEmployeeList.BuildMock();
        _unitOfWork.Setup(u => u.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
                   .Returns(mock);

        var employeeCertificationDto = CreateEmployeeCertificationDto();

        _errorLoggingServiceMock.Setup(r => r.LogException(It.IsAny<Exception>()))
                                 .Throws(new Exception("Employee not found")); // Or a custom exception

        await Assert.ThrowsAsync<Exception>(() =>
                    _employeeCertificationService.UpdateEmployeeCertification(employeeCertificationDto));

        _errorLoggingServiceMock.Verify(r => r.LogException(It.Is<Exception>(ex => ex.Message == "Employee not found")));
    }
    
    [Fact]
    public async Task DeleteEmployeeCertificationFailWhenEmployeeNotFound()
    {
        var emptyEmployeeList = new List<Employee>().AsQueryable();
        var mock = emptyEmployeeList.BuildMock();
        _unitOfWork.Setup(u => u.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
                   .Returns(mock);

        var employeeCertificationDto = CreateEmployeeCertificationDto();

        _errorLoggingServiceMock.Setup(r => r.LogException(It.IsAny<Exception>()))
                                 .Throws(new Exception("Employee not found"));

        await Assert.ThrowsAsync<Exception>(() =>
                    _employeeCertificationService.DeleteEmployeeCertification(employeeCertificationDto));

        _errorLoggingServiceMock.Verify(r => r.LogException(It.Is<Exception>(ex => ex.Message == "Employee not found")));
    } 
    
    [Fact]
    public async Task GetAllEmployeeCertificationsFailWhenEmployeeNotFound()
    {
        var emptyEmployeeList = new List<Employee>().AsQueryable();
        var mock = emptyEmployeeList.BuildMock();
        _unitOfWork.Setup(u => u.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
                   .Returns(mock);

        var employeeCertificationDto = CreateEmployeeCertificationDto();

        _errorLoggingServiceMock.Setup(r => r.LogException(It.IsAny<Exception>()))
                                 .Throws(new Exception("Employee not found"));

        await Assert.ThrowsAsync<Exception>(() =>
                    _employeeCertificationService.GetAllEmployeeCertifications(EmployeeTestData.EmployeeDto.Id));

        _errorLoggingServiceMock.Verify(r => r.LogException(It.Is<Exception>(ex => ex.Message == "Employee not found")));
    }
}
*/