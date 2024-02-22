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
    [Fact]
    public async Task SaveEmployeeDateValidInputReturnsOkResult()
    {
        var employeeDateServiceMock = new Mock<IEmployeeDateService>();
        var employeeServiceMock = new Mock<IEmployeeService>();

        var controller = new EmployeeDateController(employeeDateServiceMock.Object, employeeServiceMock.Object);

        var employeeDateInput = new EmployeeDateInput
        {
            Email = "test@retrorabbit.co.za",
            Subject = "Test Subject",
            Note = "Test Note",
            Date = new DateOnly(2023, 1, 1)
        };

        employeeServiceMock.Setup(x => x.GetEmployee(employeeDateInput.Email))
                           .ReturnsAsync(EmployeeTestData.EmployeeDto);

        employeeDateServiceMock.Setup(x => x.Save(It.IsAny<EmployeeDateDto>()))
                               .Returns(Task.CompletedTask);

        var result = await controller.SaveEmployeeDate(employeeDateInput);

        Assert.IsType<OkResult>(result);
    }

    [Fact]
    public async Task SaveEmployeeDateExceptionThrownReturnsNotFoundWithMessage()
    {
        var employeeDateServiceMock = new Mock<IEmployeeDateService>();
        var employeeServiceMock = new Mock<IEmployeeService>();
        var controller = new EmployeeDateController(employeeDateServiceMock.Object, employeeServiceMock.Object);

        var employeeDateInput = new EmployeeDateInput
        {
            Email = "test@retrorabbit.co.za",
            Subject = "Test Subject",
            Note = "Test Note",
            Date = new DateOnly(2023, 1, 1)
        };

        employeeDateServiceMock.Setup(x => x.Save(It.IsAny<EmployeeDateDto>()))
                               .ThrowsAsync(new Exception("An error occurred while saving employee date information."));

        var result = await controller.SaveEmployeeDate(employeeDateInput);

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("An error occurred while saving employee date information.", notFoundResult.Value);
    }

    [Fact]
    public async Task DeleteEmployeeDateValidInputReturnsOkResult()
    {
        var employeeDateServiceMock = new Mock<IEmployeeDateService>();
        var employeeServiceMock = new Mock<IEmployeeService>();

        var controller = new EmployeeDateController(employeeDateServiceMock.Object, employeeServiceMock.Object);

        var employeeDateInput = new EmployeeDateInput
        {
            Id = 1,
            Email = "test@retrorabbit.co.za",
            Subject = "Test Subject",
            Note = "Test Note",
            Date = new DateOnly(2023, 1, 1)
        };

        employeeServiceMock.Setup(x => x.GetEmployee(employeeDateInput.Email))
                           .ReturnsAsync(EmployeeTestData.EmployeeDto);

        employeeDateServiceMock.Setup(x => x.Delete(employeeDateInput.Id)).Returns(Task.CompletedTask);

        var result = await controller.DeleteEmployeeDate(employeeDateInput.Id);

        Assert.IsType<OkResult>(result);
    }

    [Fact]
    public async Task DeleteEmployeeDateExceptionThrownReturnsNotFoundWithMessage()
    {
        var employeeDateServiceMock = new Mock<IEmployeeDateService>();
        var employeeServiceMock = new Mock<IEmployeeService>();
        var controller = new EmployeeDateController(employeeDateServiceMock.Object, employeeServiceMock.Object);

        var employeeDateInput = new EmployeeDateInput
        {
            Id = 1,
            Email = "test@retrorabbit.co.za",
            Subject = "Test Subject",
            Note = "Test Note",
            Date = new DateOnly(2023, 1, 1)
        };

        employeeDateServiceMock.Setup(x => x.Delete(employeeDateInput.Id))
                               .ThrowsAsync(new Exception("An error occurred while deleting employee date information."));

        var result = await controller.DeleteEmployeeDate(employeeDateInput.Id);

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("An error occurred while deleting employee date information.", notFoundResult.Value);
    }

    [Fact]
    public async Task UpdateEmployeeDateValidInputReturnsOkResult()
    {
        var employeeDateServiceMock = new Mock<IEmployeeDateService>();
        var employeeServiceMock = new Mock<IEmployeeService>();

        var controller = new EmployeeDateController(employeeDateServiceMock.Object, employeeServiceMock.Object);

        var employeeDateDto = new EmployeeDateDto
            (
             1,
             EmployeeTestData.EmployeeDto,
             "Test Subject",
             "Test Note",
             new DateOnly(2023, 1, 1)
            );

        employeeServiceMock.Setup(x => x.GetEmployee(employeeDateDto.Employee!.Email!))
                           .ReturnsAsync(employeeDateDto.Employee);

        employeeDateServiceMock.Setup(x => x.Update(It.IsAny<EmployeeDateDto>()))
                               .Returns(Task.CompletedTask);

        var result = await controller.UpdateEmployeeDate(employeeDateDto);

        Assert.IsType<OkResult>(result);
    }

    [Fact]
    public async Task UpdateEmployeeDateExceptionThrownReturnsNotFoundWithMessage()
    {
        var employeeDateServiceMock = new Mock<IEmployeeDateService>();
        var employeeServiceMock = new Mock<IEmployeeService>();
        var controller = new EmployeeDateController(employeeDateServiceMock.Object, employeeServiceMock.Object);

        var employeeDateDto = new EmployeeDateDto
            (
             1,
             EmployeeTestData.EmployeeDto,
             "Test Subject",
             "Test Note",
             new DateOnly(2023, 1, 1)
            );

        employeeDateServiceMock.Setup(x => x.Update(It.IsAny<EmployeeDateDto>()))
                               .ThrowsAsync(new Exception("An error occurred while updating employee date information."));

        var result = await controller.UpdateEmployeeDate(employeeDateDto);

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("An error occurred while updating employee date information.", notFoundResult.Value);
    }

    [Fact]
    public void GetAllEmployeeDateByDateReturnsOkResultWithList()
    {
        var employeeDateServiceMock = new Mock<IEmployeeDateService>();
        var employeeServiceMock = new Mock<IEmployeeService>();

        var controller = new EmployeeDateController(employeeDateServiceMock.Object, employeeServiceMock.Object);

        var date = new DateOnly(2023, 1, 1);
        var expectedEmployeeDates = new List<EmployeeDateDto>
        {
            new(
                1,
                EmployeeTestData.EmployeeDto,
                "Test Subject",
                "Test Note",
                date
               ),
            new(
                1,
                EmployeeTestData.EmployeeDto2,
                "Test Subject",
                "Test Note",
                date)
        };

        employeeDateServiceMock.Setup(x => x.GetAllByDate(date)).Returns(expectedEmployeeDates);

        var result = controller.GetAllEmployeeDate(date);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var actualEmployeeDates = Assert.IsType<List<EmployeeDateDto>>(okResult.Value);
        Assert.Equal(expectedEmployeeDates, actualEmployeeDates);
    }

    [Fact]
    public void GetAllEmployeeDateByEmployeeReturnsOkResultWithList()
    {
        var employeeDateServiceMock = new Mock<IEmployeeDateService>();
        var employeeServiceMock = new Mock<IEmployeeService>();

        var controller = new EmployeeDateController(employeeDateServiceMock.Object, employeeServiceMock.Object);

        var email = "test@retrorabbit.co.za";
        var expectedEmployeeDates = new List<EmployeeDateDto>
        {
            new(
                1,
                EmployeeTestData.EmployeeDto,
                "Test Subject",
                "Test Note",
                new DateOnly(2023, 1, 1)
               )
        };

        employeeDateServiceMock.Setup(x => x.GetAllByEmployee(email)).Returns(expectedEmployeeDates);

        var result = controller.GetAllEmployeeDate(email: email);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var actualEmployeeDates = Assert.IsType<List<EmployeeDateDto>>(okResult.Value);
        Assert.Equal(expectedEmployeeDates, actualEmployeeDates);
    }

    [Fact]
    public void GetAllEmployeeDateBySubjectReturnsOkResultWithList()
    {
        var employeeDateServiceMock = new Mock<IEmployeeDateService>();
        var employeeServiceMock = new Mock<IEmployeeService>();

        var controller = new EmployeeDateController(employeeDateServiceMock.Object, employeeServiceMock.Object);

        var subject = "Test Subject";
        var expectedEmployeeDates = new List<EmployeeDateDto>
        {
            new(
                1,
                EmployeeTestData.EmployeeDto,
                subject,
                "Test Note",
                new DateOnly(2023, 1, 1)
               )
        };

        employeeDateServiceMock.Setup(x => x.GetAllBySubject(subject)).Returns(expectedEmployeeDates);

        var result = controller.GetAllEmployeeDate(subject: subject);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var actualEmployeeDates = Assert.IsType<List<EmployeeDateDto>>(okResult.Value);
        Assert.Equal(expectedEmployeeDates, actualEmployeeDates);
    }

    [Fact]
    public void GetAllEmployeeDateNoFiltersReturnsOkResultWithList()
    {
        var employeeDateServiceMock = new Mock<IEmployeeDateService>();
        var employeeServiceMock = new Mock<IEmployeeService>();

        var controller = new EmployeeDateController(employeeDateServiceMock.Object, employeeServiceMock.Object);

        var expectedEmployeeDates = new List<EmployeeDateDto>
        {
            new(
                1,
                EmployeeTestData.EmployeeDto,
                "Test Subject",
                "Test Note",
                new DateOnly(2023, 1, 1)
               )
        };

        employeeDateServiceMock.Setup(x => x.GetAll()).Returns(expectedEmployeeDates);

        var result = controller.GetAllEmployeeDate();

        var okResult = Assert.IsType<OkObjectResult>(result);
        var actualEmployeeDates = Assert.IsType<List<EmployeeDateDto>>(okResult.Value);
        Assert.Equal(expectedEmployeeDates, actualEmployeeDates);
    }

    [Fact]
    public void GetAllEmployeeDate_ExceptionThrown_ReturnsNotFoundWithMessage()
    {
        var employeeDateServiceMock = new Mock<IEmployeeDateService>();
        var employeeServiceMock = new Mock<IEmployeeService>();
        var controller = new EmployeeDateController(employeeDateServiceMock.Object, employeeServiceMock.Object);

        employeeDateServiceMock.Setup(x => x.GetAll())
                               .Throws(new Exception("An error occurred while retrieving employee dates."));

        var result = controller.GetAllEmployeeDate();

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("An error occurred while retrieving employee dates.", notFoundResult.Value);
    }
}