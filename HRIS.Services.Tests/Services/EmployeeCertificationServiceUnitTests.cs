using System.Linq.Expressions;
using HRIS.Models;
using HRIS.Models.Enums;
using HRIS.Services.Services;
using MockQueryable.Moq;
using Moq;
using RR.UnitOfWork;
using RR.UnitOfWork.Entities.HRIS;
using Xunit;

namespace HRIS.Services.Tests.Services;

public class EmployeeCertificationServiceUnitTests
{
    private readonly EmployeeCertificationService _employeeCertificationService;
    private readonly Mock<IUnitOfWork> _unitOfWork = new();

    public EmployeeCertificationServiceUnitTests()
    {
        _employeeCertificationService = new EmployeeCertificationService(_unitOfWork.Object);
    }

    private EmployeeCertificationDto CreateEmployeeCertificationDto()
    {
        var employeeTypeDto = new EmployeeTypeDto{ Id = 1, Name = "Developer" };
        var employeeAddressDto =
            new EmployeeAddressDto(1, "2", "Complex", "2", "Suburb/District", "City", "Country", "Province", "1620");

        var employeeDto = new EmployeeDto(1, "001", "34434434", new DateTime(2020, 1, 1), new DateTime(2020, 1, 1),
                                          null, false, "None", 4, new EmployeeTypeDto{ Id = 1, Name = "Developer" }, "Notes", 1, 28,
                                          128, 100000, "Dotty", "D",
                                          "Missile", new DateTime(1990, 1, 1), "South Africa", "South African",
                                          "5522522655", " ",
                                          new DateTime(2020, 1, 1), null, Race.Black, Gender.Female, null,
                                          "dm@retrorabbit.co.za", "test@gmail.com", "0123456789", null, null,
                                          employeeAddressDto, employeeAddressDto, null, null, null);

        var employeeDocumentDto = new EmployeeDocumentDto
        {
            Id = 1,
            EmployeeId = 1,
            Reference = null,
            FileName = "filename",
            FileCategory = FileCategory.FixedTerm,
            Blob = "blobstring",
            Status = DocumentStatus.Approved,
            UploadDate = new DateTime(2020, 1, 1),
            Reason = null,
            CounterSign = true
        };


        var employeeCertificationDto = new EmployeeCertificationDto(1, employeeDto, employeeDocumentDto, "Title",
                                                                    "Publisher",
                                                                    EmployeeCertificationStatus.Approved, employeeDto,
                                                                    new DateTime(2020, 1, 1), "audit note");

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

        await Assert.ThrowsAsync<Exception>(() =>
                                                _employeeCertificationService
                                                    .SaveEmployeeCertification(employeeCertificationDto));
    }

    [Fact]
    public async Task SaveEmployeeCertificationFailWhenCertificationExists()
    {
        var employeeCertificationDto = CreateEmployeeCertificationDto();
        MockEmployeeRepositorySetup(employeeCertificationDto.Employee!);
        MockEmployeeCertificationRepositorySetup(employeeCertificationDto);

        await Assert.ThrowsAsync<Exception>(() =>
                                                _employeeCertificationService
                                                    .SaveEmployeeCertification(employeeCertificationDto));
    }

    [Fact]
    public async Task SaveEmployeeCertificationPass()
    {
        var employeeCertificationDto = CreateEmployeeCertificationDto();

        MockEmployeeRepositorySetupWithEmployeeAsync(employeeCertificationDto.Employee!);
        MockEmployeeCertificationRepositorySetupEmptyAsync();
        MockEmployeeCertificationRepositorySetupForAdd(employeeCertificationDto);

        var result = await _employeeCertificationService.SaveEmployeeCertification(employeeCertificationDto);

        Assert.NotNull(result);
    }

    [Fact]
    public async Task GetEmployeeCertificationPass()
    {
        var employeeCertificationDto = CreateEmployeeCertificationDto();
        MockEmployeeRepositorySetup(employeeCertificationDto.Employee!);
        MockEmployeeCertificationRepositorySetupWithCertification(employeeCertificationDto);

        var result =
            await _employeeCertificationService.GetEmployeeCertification(employeeCertificationDto.Employee!.Id,
                                                                         employeeCertificationDto.Id);

        Assert.NotNull(result);
    }

    [Fact]
    public async Task GetAllEmployeeCertificationsPass()
    {
        var employeeCertificationDto1 = CreateEmployeeCertificationDto();
        var employeeCertificationDto2 = CreateEmployeeCertificationDto();

        MockEmployeeRepositorySetupWithEmployee(employeeCertificationDto1.Employee!);

        var employeeCertificationList = new List<EmployeeCertification>
        {
            new(employeeCertificationDto1),
            new(employeeCertificationDto2)
        };

        var mock = employeeCertificationList.AsQueryable().BuildMock();
        _unitOfWork.Setup(u => u.EmployeeCertification.Get(It.IsAny<Expression<Func<EmployeeCertification, bool>>>()))
                   .Returns(mock);

        var results =
            await _employeeCertificationService.GetAllEmployeeCertifications(employeeCertificationDto1.Employee!.Id);

        Assert.NotNull(results);
        Assert.Equal(2, results.Count);
    }

    [Fact]
    public async Task UpdateEmployeeCertificationPass()
    {
        var employeeCertificationDto = CreateEmployeeCertificationDto();
        MockEmployeeRepositorySetupWithEmployee(employeeCertificationDto.Employee!);
        MockEmployeeCertificationRepositorySetupForAddOrUpdate(employeeCertificationDto, false);

        var result = await _employeeCertificationService.UpdateEmployeeCertification(employeeCertificationDto);

        Assert.NotNull(result);
    }

    [Fact]
    public async Task DeleteEmployeeCertificationPass()
    {
        var employeeCertificationDto = CreateEmployeeCertificationDto();
        MockEmployeeRepositorySetupWithEmployee(employeeCertificationDto.Employee!);
        MockEmployeeCertificationRepositorySetupForDelete(employeeCertificationDto);

        var result = await _employeeCertificationService.DeleteEmployeeCertification(employeeCertificationDto);

        Assert.NotNull(result);
    }
}
