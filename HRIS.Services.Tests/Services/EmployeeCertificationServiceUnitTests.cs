﻿using System.Linq.Expressions;
using HRIS.Models;
using HRIS.Models.Enums;
using HRIS.Services.Interfaces;
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
    private readonly Mock<IErrorLoggingService> _errorLoggingServiceMock;

    public EmployeeCertificationServiceUnitTests()
    {
        _errorLoggingServiceMock = new Mock<IErrorLoggingService>();
        _employeeCertificationService = new EmployeeCertificationService(_unitOfWork.Object, _errorLoggingServiceMock.Object);
    }

    private EmployeeCertificationDto CreateEmployeeCertificationDto()
    {
        var employeeTypeDto = new EmployeeTypeDto{ Id = 1, Name = "Developer" };
        var employeeAddressDto =
            new EmployeeAddressDto{ Id = 1, UnitNumber = "2", ComplexName = "Complex", StreetNumber = "2", SuburbOrDistrict = "Suburb/District", City = "City", Country = "Country", Province = "Province", PostalCode = "1620" };

        var employeeDto = new EmployeeDto
        {
            Id = 1,
            EmployeeNumber = "001",
            TaxNumber = "34434434",
            EngagementDate = DateTime.Now,
            TerminationDate = DateTime.Now,
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
            PassportCountryIssue = null,
            Race = Race.Black,
            Gender = Gender.Male,
            Email = "texample@retrorabbit.co.za",
            PersonalEmail = "test.example@gmail.com",
            CellphoneNo = "0000000000",
            PhysicalAddress = employeeAddressDto,
            PostalAddress = employeeAddressDto
        };

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


        var employeeCertificationDto = new EmployeeCertificationDto
        {
            Id = 1,
            Employee = employeeDto,
            EmployeeDocument = employeeDocumentDto,
            Title = "Title",
            Publisher = "Publisher",
            Status = EmployeeCertificationStatus.Approved,
            AuditBy = employeeDto,
            AuditDate = new DateTime(2020, 1, 1),
            AuditNote = "audit note"
        };
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

        _errorLoggingServiceMock.Setup(r => r.LogException(It.IsAny<Exception>())).Throws(new Exception());

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

        _errorLoggingServiceMock.Setup(r => r.LogException(It.IsAny<Exception>())).Throws(new Exception());

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
