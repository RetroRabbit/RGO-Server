using HRIS.Models;
using HRIS.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RR.App.Controllers.HRIS;
using RR.Tests.Data.Models.HRIS;
using Xunit;

namespace RR.App.Tests.Controllers.HRIS;

public class EmployeeDateControllerUnitTests
{
    private readonly Mock<IEmployeeDateService> _employeeDateServiceMock;
    private readonly Mock<IEmployeeService> _employeeServiceMock;

    private readonly EmployeeDateController _controller;

    private readonly EmployeeDateInput _employeeDateInput;
    private readonly EmployeeDateDto _employeeDateDto;
    private readonly List<EmployeeDateDto> _employeeDateDtoList;

    public EmployeeDateControllerUnitTests()
    {
        _employeeDateServiceMock = new Mock<IEmployeeDateService>();
        _employeeServiceMock = new Mock<IEmployeeService>();
        _controller = new EmployeeDateController(_employeeDateServiceMock.Object, _employeeServiceMock.Object);

        _employeeDateInput = new EmployeeDateInput
        {
            Email = "test@retrorabbit.co.za",
            Subject = "Test Subject",
            Note = "Test Note",
            Date = new DateOnly(2023, 1, 1)
        };

        _employeeDateDto = new EmployeeDateDto
        {
            Id = 1,
            Employee = EmployeeTestData.EmployeeOne.ToDto(),
            Subject = "Test Subject",
            Note = "Test Note",
            Date = new DateOnly(2023, 1, 1)
        };

        _employeeDateDtoList = new List<EmployeeDateDto>
        {
            _employeeDateDto
        };
    }

    [Fact]
    public async Task SaveEmployeeDateValidInputReturnsOkResult()
    {
        _employeeServiceMock.Setup(x => x.GetEmployee(_employeeDateInput.Email))
                           .ReturnsAsync(EmployeeTestData.EmployeeOne.ToDto());

        _employeeDateServiceMock.Setup(x => x.Save(It.IsAny<EmployeeDateDto>()))
                               .Returns(Task.CompletedTask);

        var result = await _controller.SaveEmployeeDate(_employeeDateInput);

        Assert.IsType<OkResult>(result);
    }

    [Fact]
    public async Task DeleteEmployeeDateValidInputReturnsOkResult()
    {
        _employeeServiceMock.Setup(x => x.GetEmployee(_employeeDateInput.Email))
                           .ReturnsAsync(EmployeeTestData.EmployeeOne.ToDto());

        _employeeDateServiceMock.Setup(x => x.Delete(_employeeDateInput.Id)).Returns(Task.CompletedTask);

        var result = await _controller.DeleteEmployeeDate(_employeeDateInput.Id);

        Assert.IsType<OkResult>(result);
    }

    [Fact]
    public async Task UpdateEmployeeDateValidInputReturnsOkResult()
    {
        _employeeServiceMock.Setup(x => x.GetEmployee(_employeeDateDto.Employee!.Email!))
                           .ReturnsAsync(_employeeDateDto.Employee);

        _employeeDateServiceMock.Setup(x => x.Update(It.IsAny<EmployeeDateDto>()))
                               .Returns(Task.CompletedTask);

        var result = await _controller.UpdateEmployeeDate(_employeeDateDto);

        Assert.IsType<OkResult>(result);
    }

    [Fact]
    public void GetAllEmployeeDateByDateReturnsOkResultWithList()
    {
        _employeeDateServiceMock.Setup(x => x.GetAllByDate(_employeeDateDto.Date)).Returns(_employeeDateDtoList);

        var result = _controller.GetAllEmployeeDate(_employeeDateDto.Date);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var actualEmployeeDates = Assert.IsType<List<EmployeeDateDto>>(okResult.Value);
        Assert.Equal(_employeeDateDtoList, actualEmployeeDates);
    }

    [Fact]
    public void GetAllEmployeeDateByEmployeeReturnsOkResultWithList()
    {
        _employeeDateServiceMock.Setup(x => x.GetAllByEmployee(_employeeDateInput.Email)).Returns(_employeeDateDtoList);

        var result = _controller.GetAllEmployeeDate(email: _employeeDateInput.Email);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var actualEmployeeDates = Assert.IsType<List<EmployeeDateDto>>(okResult.Value);
        Assert.Equal(_employeeDateDtoList, actualEmployeeDates);
    }

    [Fact]
    public void GetAllEmployeeDateBySubjectReturnsOkResultWithList()
    {
        _employeeDateServiceMock.Setup(x => x.GetAllBySubject(_employeeDateInput.Subject)).Returns(_employeeDateDtoList);

        var result = _controller.GetAllEmployeeDate(subject: _employeeDateInput.Subject);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var actualEmployeeDates = Assert.IsType<List<EmployeeDateDto>>(okResult.Value);
        Assert.Equal(_employeeDateDtoList, actualEmployeeDates);
    }

    [Fact]
    public void GetAllEmployeeDateNoFiltersReturnsOkResultWithList()
    {
        _employeeDateServiceMock.Setup(x => x.GetAll()).Returns(_employeeDateDtoList);

        var result = _controller.GetAllEmployeeDate();

        var okResult = Assert.IsType<OkObjectResult>(result);
        var actualEmployeeDates = Assert.IsType<List<EmployeeDateDto>>(okResult.Value);
        Assert.Equal(_employeeDateDtoList, actualEmployeeDates);
    }

    [Fact]
    public void GetAllEmployeeDate_ExceptionThrown_ReturnsNotFoundWithMessage()
    {
        _employeeDateServiceMock.Setup(x => x.GetAll())
                               .Throws(new Exception("An error occurred while retrieving employee dates."));

        var result = _controller.GetAllEmployeeDate();

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("An error occurred while retrieving employee dates.", notFoundResult.Value);
    }
}