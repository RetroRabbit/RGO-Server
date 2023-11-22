using Microsoft.AspNetCore.Mvc;
using Moq;
using RGO.App.Controllers;
using RGO.Models;
using RGO.Models.Enums;
using RGO.Services.Interfaces;
using Xunit;

namespace RGO.App.Tests.Controllers;

public class EmployeeDateControllerUnitTests
{
    [Fact]
    public async Task SaveEmployeeDateValidInputReturnsOkResult()
    {
        var employeeDateServiceMock = new Mock<IEmployeeDateService>();
        var employeeServiceMock = new Mock<IEmployeeService>();

        var controller = new EmployeeDateController(employeeDateServiceMock.Object, employeeServiceMock.Object);

        var employeeDateInput = new EmployeeDateInput
        (
            "test@retrorabbit.co.za",
            "Test Subject",
            "Test Note",
            new DateOnly(2023, 1, 1)
        );

        employeeServiceMock.Setup(x => x.GetEmployee(employeeDateInput.Email)).ReturnsAsync(new EmployeeDto
        (
            1,
            "1001",
            "1000000001",
            new DateOnly(1985, 5, 15),
            new DateOnly(2023, 12, 31),
            1,
            false,
            "No disabilities",
            3,
            new EmployeeTypeDto(1, "Developer"),
            "Experienced software engineer with a focus on web development.",
            1.5f,
            20.5f,
            15.0f,
            50000,
            "John",
            "J",
            "Doe",
            new DateOnly(1980, 8, 10),
            "South Africa",
            "South African",
            "ID123456",
            "ZA123456",
            new DateOnly(2025, 6, 30),
            "PassportCountry",
            Race.White,
            Gender.Male,
            "path/to/photo.jpg",
            "test@retrorabbit.co.za",
            "john.personal@example.com",
            "123456789",
            1,
            2,
            new EmployeeAddressDto(1, "Apt 102", "Tech Towers", "123 Tech Street", "Tech City", "Tech District", "Techland", "Tech Province", "T12345"),
            new EmployeeAddressDto(2, "PO Box 567", "Tech Towers", "123 Tech Street", "Tech City", "Tech District", "Techland", "Tech Province", "T54321"),
            "123",
            "Emergency Contact",
            "987654321"));

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
        (
            "test@retrorabbit.co.za",
            "Test Subject",
            "Test Note",
            new DateOnly(2023, 1, 1)
        );

        employeeDateServiceMock.Setup(x => x.Save(It.IsAny<EmployeeDateDto>())).ThrowsAsync(new Exception("An error occurred while saving employee date information."));

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
        (
            "test@retrorabbit.co.za",
            "Test Subject",
            "Test Note",
            new DateOnly(2023, 1, 1)
        );

        employeeServiceMock.Setup(x => x.GetEmployee(employeeDateInput.Email))
        .ReturnsAsync((new EmployeeDto
        (
            1,
            "1001",
            "1000000001",
            new DateOnly(1985, 5, 15),
            new DateOnly(2023, 12, 31),
            1,
            false,
            "No disabilities",
            3,
            new EmployeeTypeDto(1, "Developer"),
            "Experienced software engineer with a focus on web development.",
            1.5f,
            20.5f,
            15.0f,
            50000,
            "John",
            "J",
            "Doe",
            new DateOnly(1980, 8, 10),
            "South Africa",
            "South African",
            "ID123456",
            "ZA123456",
            new DateOnly(2025, 6, 30),
            "PassportCountry",
            Race.White,
            Gender.Male,
            "path/to/photo.jpg",
            "test@retrorabbit.co.za",
            "john.personal@example.com",
            "123456789",
            1,
            2,
            new EmployeeAddressDto(1, "Apt 102", "Tech Towers", "123 Tech Street", "Tech City", "Tech District", "Techland", "Tech Province", "T12345"),
            new EmployeeAddressDto(2, "PO Box 567", "Tech Towers", "123 Tech Street", "Tech City", "Tech District", "Techland", "Tech Province", "T54321"),
            "123",
            "Emergency Contact",
            "987654321")));

        employeeDateServiceMock.Setup(x => x.Delete(It.IsAny<EmployeeDateDto>())).Returns(Task.CompletedTask);

        var result = await controller.DeleteEmployeeDate(employeeDateInput);

        Assert.IsType<OkResult>(result);
    }

    [Fact]
    public async Task DeleteEmployeeDateExceptionThrownReturnsNotFoundWithMessage()
    {
        var employeeDateServiceMock = new Mock<IEmployeeDateService>();
        var employeeServiceMock = new Mock<IEmployeeService>();
        var controller = new EmployeeDateController(employeeDateServiceMock.Object, employeeServiceMock.Object);

        var employeeDateInput = new EmployeeDateInput
        (
            "test@retrorabbit.co.za",
            "Test Subject",
            "Test Note",
            new DateOnly(2023, 1, 1)
        );

        employeeDateServiceMock.Setup(x => x.Delete(It.IsAny<EmployeeDateDto>())).ThrowsAsync(new Exception("An error occurred while deleting employee date information."));

        var result = await controller.DeleteEmployeeDate(employeeDateInput);

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
            new EmployeeDto
            (
                1,
                "1001",
                "1000000001",
                new DateOnly(1985, 5, 15),
                new DateOnly(2023, 12, 31),
                1,
                false,
                "No disabilities",
                3,
                new EmployeeTypeDto(1, "Developer"),
                "Experienced software engineer with a focus on web development.",
                1.5f,
                20.5f,
                15.0f,
                50000,
                "John",
                "J",
                "Doe",
                new DateOnly(1980, 8, 10),
                "South Africa",
                "South African",
                "ID123456",
                "ZA123456",
                new DateOnly(2025, 6, 30),
                "PassportCountry",
                Race.White,
                Gender.Male,
                "path/to/photo.jpg",
                "test@retrorabbit.co.za",
                "john.personal@example.com",
                "123456789",
                1,
                2,
                new EmployeeAddressDto(1, "Apt 102", "Tech Towers", "123 Tech Street", "Tech City", "Tech District", "Techland", "Tech Province", "T12345"),
                new EmployeeAddressDto(2, "PO Box 567", "Tech Towers", "123 Tech Street", "Tech City", "Tech District", "Techland", "Tech Province", "T54321"),
                "123",
                "Emergency Contact",
                "987654321"),
            "Test Subject",
            "Test Note",
            new DateOnly(2023, 1, 1)
        );

        employeeServiceMock.Setup(x => x.GetEmployee(employeeDateDto.Employee!.Email)).ReturnsAsync(employeeDateDto.Employee);

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
            new EmployeeDto
            (
                1,
                "1001",
                "1000000001",
                new DateOnly(1985, 5, 15),
                new DateOnly(2023, 12, 31),
                1,
                false,
                "No disabilities",
                3,
                new EmployeeTypeDto(1, "Developer"),
                "Experienced software engineer with a focus on web development.",
                1.5f,
                20.5f,
                15.0f,
                50000,
                "John",
                "J",
                "Doe",
                new DateOnly(1980, 8, 10),
                "South Africa",
                "South African",
                "ID123456",
                "ZA123456",
                new DateOnly(2025, 6, 30),
                "PassportCountry",
                Race.White,
                Gender.Male,
                "path/to/photo.jpg",
                "test@retrorabbit.co.za",
                "john.personal@example.com",
                "123456789",
                1,
                2,
                new EmployeeAddressDto(1, "Apt 102", "Tech Towers", "123 Tech Street", "Tech City", "Tech District", "Techland", "Tech Province", "T12345"),
                new EmployeeAddressDto(2, "PO Box 567", "Tech Towers", "123 Tech Street", "Tech City", "Tech District", "Techland", "Tech Province", "T54321"),
                "123",
                "Emergency Contact",
                "987654321"),
            "Test Subject",
            "Test Note",
            new DateOnly(2023, 1, 1)
        );

        employeeDateServiceMock.Setup(x => x.Update(It.IsAny<EmployeeDateDto>())).ThrowsAsync(new Exception("An error occurred while updating employee date information."));

        var result = await controller.UpdateEmployeeDate(employeeDateDto);

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("An error occurred while updating employee date information.", notFoundResult.Value);
    }

    [Fact]
    public async Task GetAllEmployeeDateByDateReturnsOkResultWithList()
    {
        var employeeDateServiceMock = new Mock<IEmployeeDateService>();
        var employeeServiceMock = new Mock<IEmployeeService>();

        var controller = new EmployeeDateController(employeeDateServiceMock.Object, employeeServiceMock.Object);

        var date = new DateOnly(2023, 1, 1);
        var expectedEmployeeDates = new List<EmployeeDateDto>
        {
            new EmployeeDateDto
            (
                1,
                new EmployeeDto
                (
                    1,
                    "1001",
                    "1000000001",
                    new DateOnly(1985, 5, 15),
                    new DateOnly(2023, 12, 31),
                    1,
                    false,
                    "No disabilities",
                    3,
                    new EmployeeTypeDto(1, "Developer"),
                    "Experienced software engineer with a focus on web development.",
                    1.5f,
                    20.5f,
                    15.0f,
                    50000,
                    "John",
                    "J",
                    "Doe",
                    new DateOnly(1980,8, 10),
                    "South Africa",
                    "South African",
                    "ID123456",
                    "ZA123456",
                    new DateOnly(2025, 6, 30),
                    "PassportCountry",
                    Race.White,
                    Gender.Male,
                    "path/to/photo.jpg",
                    "test@retrorabbit.co.za",
                    "john.personal@example.com",
                    "123456789",
                    1,
                    2,
                    new EmployeeAddressDto(1, "Apt 102", "Tech Towers", "123 Tech Street", "Tech City", "Tech District", "Techland", "Tech Province", "T12345"),
                    new EmployeeAddressDto(2, "PO Box 567", "Tech Towers", "123 Tech Street", "Tech City", "Tech District", "Techland", "Tech Province", "T54321"),
                    "123",
                    "Emergency Contact",
                    "987654321"),
                "Test Subject",
                "Test Note",
                date
            ),
            new EmployeeDateDto
            (
                1,
                new EmployeeDto
                (
                    1,
                    "1001",
                    "1000000001",
                    new DateOnly(1985, 5, 15),
                    new DateOnly(2023, 12, 31),
                    1,
                    false,
                    "No disabilities",
                    3,
                    new EmployeeTypeDto(1, "Developer"),
                    "Experienced software engineer with a focus on web development.",
                    1.5f,
                    20.5f,
                    15.0f,
                    50000,
                    "John",
                    "J",
                    "Doe",
                    new DateOnly(1980,8, 10),
                    "South Africa",
                    "South African",
                    "ID123456",
                    "ZA123456",
                    new DateOnly(2025, 6, 30),
                    "PassportCountry",
                    Race.White,
                    Gender.Male,
                    "path/to/photo.jpg",
                    "test@retrorabbit.co.za",
                    "john.personal@example.com",
                    "123456789",
                    1,
                    2,
                    new EmployeeAddressDto(1, "Apt 102", "Tech Towers", "123 Tech Street", "Tech City", "Tech District", "Techland", "Tech Province", "T12345"),
                    new EmployeeAddressDto(2, "PO Box 567", "Tech Towers", "123 Tech Street", "Tech City", "Tech District", "Techland", "Tech Province", "T54321"),
                    "123",
                    "Emergency Contact",
                    "987654321"),
                "Test Subject",
                "Test Note",
                date)
        };

        employeeDateServiceMock.Setup(x => x.GetAllByDate(date)).Returns(expectedEmployeeDates);

        var result = await controller.GetAllEmployeeDate(date: date);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var actualEmployeeDates = Assert.IsType<List<EmployeeDateDto>>(okResult.Value);
        Assert.Equal(expectedEmployeeDates, actualEmployeeDates);
    }

    [Fact]
    public async Task GetAllEmployeeDateByEmployeeReturnsOkResultWithList()
    {
        var employeeDateServiceMock = new Mock<IEmployeeDateService>();
        var employeeServiceMock = new Mock<IEmployeeService>();

        var controller = new EmployeeDateController(employeeDateServiceMock.Object, employeeServiceMock.Object);

        var email = "test@retrorabbit.co.za";
        var expectedEmployeeDates = new List<EmployeeDateDto>
        {
            new EmployeeDateDto
            (
                1,
                new EmployeeDto
                (
                    1,
                    "1001",
                    "1000000001",
                    new DateOnly(1985, 5, 15),
                    new DateOnly(2023, 12, 31),
                    1,
                    false,
                    "No disabilities",
                    3,
                    new EmployeeTypeDto(1, "Developer"),
                    "Experienced software engineer with a focus on web development.",
                    1.5f,
                    20.5f,
                    15.0f,
                    50000,
                    "John",
                    "J",
                    "Doe",
                    new DateOnly(1980,8, 10),
                    "South Africa",
                    "South African",
                    "ID123456",
                    "ZA123456",
                    new DateOnly(2025, 6, 30),
                    "PassportCountry",
                    Race.White,
                    Gender.Male,
                    "path/to/photo.jpg",
                    "test@retrorabbit.co.za",
                    "john.personal@example.com",
                    "123456789",
                    1,
                    2,
                    new EmployeeAddressDto(1, "Apt 102", "Tech Towers", "123 Tech Street", "Tech City", "Tech District", "Techland", "Tech Province", "T12345"),
                    new EmployeeAddressDto(2, "PO Box 567", "Tech Towers", "123 Tech Street", "Tech City", "Tech District", "Techland", "Tech Province", "T54321"),
                    "123",
                    "Emergency Contact",
                    "987654321"),
                "Test Subject",
                "Test Note",
                new DateOnly(2023, 1, 1)
            ),

        };

        employeeDateServiceMock.Setup(x => x.GetAllByEmployee(email)).Returns(expectedEmployeeDates);

        var result = await controller.GetAllEmployeeDate(email: email);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var actualEmployeeDates = Assert.IsType<List<EmployeeDateDto>>(okResult.Value);
        Assert.Equal(expectedEmployeeDates, actualEmployeeDates);
    }

    [Fact]
    public async Task GetAllEmployeeDateBySubjectReturnsOkResultWithList()
    {
        var employeeDateServiceMock = new Mock<IEmployeeDateService>();
        var employeeServiceMock = new Mock<IEmployeeService>();

        var controller = new EmployeeDateController(employeeDateServiceMock.Object, employeeServiceMock.Object);

        var subject = "Test Subject";
        var expectedEmployeeDates = new List<EmployeeDateDto>
        {
            new EmployeeDateDto
            (
                1,
                new EmployeeDto
                (
                    1,
                    "1001",
                    "1000000001",
                    new DateOnly(1985, 5, 15),
                    new DateOnly(2023, 12, 31),
                    1,
                    false,
                    "No disabilities",
                    3,
                    new EmployeeTypeDto(1, "Developer"),
                    "Experienced software engineer with a focus on web development.",
                    1.5f,
                    20.5f,
                    15.0f,
                    50000,
                    "John",
                    "J",
                    "Doe",
                    new DateOnly(1980,8, 10),
                    "South Africa",
                    "South African",
                    "ID123456",
                    "ZA123456",
                    new DateOnly(2025, 6, 30),
                    "PassportCountry",
                    Race.White,
                    Gender.Male,
                    "path/to/photo.jpg",
                    "test@retrorabbit.co.za",
                    "john.personal@example.com",
                    "123456789",
                    1,
                    2,
                    new EmployeeAddressDto(1, "Apt 102", "Tech Towers", "123 Tech Street", "Tech City", "Tech District", "Techland", "Tech Province", "T12345"),
                    new EmployeeAddressDto(2, "PO Box 567", "Tech Towers", "123 Tech Street", "Tech City", "Tech District", "Techland", "Tech Province", "T54321"),
                    "123",
                    "Emergency Contact",
                    "987654321"),
                subject,
                "Test Note",
                new DateOnly(2023, 1, 1)
            ),
        };

        employeeDateServiceMock.Setup(x => x.GetAllBySubject(subject)).Returns(expectedEmployeeDates);

        var result = await controller.GetAllEmployeeDate(subject: subject);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var actualEmployeeDates = Assert.IsType<List<EmployeeDateDto>>(okResult.Value);
        Assert.Equal(expectedEmployeeDates, actualEmployeeDates);
    }

    [Fact]
    public async Task GetAllEmployeeDateNoFiltersReturnsOkResultWithList()
    {
        var employeeDateServiceMock = new Mock<IEmployeeDateService>();
        var employeeServiceMock = new Mock<IEmployeeService>();

        var controller = new EmployeeDateController(employeeDateServiceMock.Object, employeeServiceMock.Object);

        var expectedEmployeeDates = new List<EmployeeDateDto>
        {
            new EmployeeDateDto
            (
                1,
                new EmployeeDto
                (
                    1,
            "1001",
            "1000000001",
            new DateOnly(1985, 5, 15),
            new DateOnly(2023, 12, 31),
            1,
            false,
            "No disabilities",
            3,
            new EmployeeTypeDto(1, "Developer"),
            "Experienced software engineer with a focus on web development.",
            1.5f,
            20.5f,
            15.0f,
            50000,
            "John",
            "J",
            "Doe",
            new DateOnly(1980,8, 10),
            "South Africa",
            "South African",
            "ID123456",
            "ZA123456",
            new DateOnly(2025, 6, 30),
            "PassportCountry",
            Race.White,
            Gender.Male,
            "path/to/photo.jpg",
            "test@retrorabbit.co.za",
            "john.personal@example.com",
            "123456789",
            1,
            2,
            new EmployeeAddressDto(1, "Apt 102", "Tech Towers", "123 Tech Street", "Tech City", "Tech District", "Techland", "Tech Province", "T12345"),
            new EmployeeAddressDto(2, "PO Box 567", "Tech Towers", "123 Tech Street", "Tech City", "Tech District", "Techland", "Tech Province", "T54321"),
            "123",
            "Emergency Contact",
            "987654321"),
                "Test Subject",
                "Test Note",
                new DateOnly(2023, 1, 1)
            ),

        };

        employeeDateServiceMock.Setup(x => x.GetAll()).Returns(expectedEmployeeDates);

        var result = await controller.GetAllEmployeeDate();

        var okResult = Assert.IsType<OkObjectResult>(result);
        var actualEmployeeDates = Assert.IsType<List<EmployeeDateDto>>(okResult.Value);
        Assert.Equal(expectedEmployeeDates, actualEmployeeDates);
    }

    [Fact]
    public async Task GetAllEmployeeDate_ExceptionThrown_ReturnsNotFoundWithMessage()
    {
        var employeeDateServiceMock = new Mock<IEmployeeDateService>();
        var employeeServiceMock = new Mock<IEmployeeService>();
        var controller = new EmployeeDateController(employeeDateServiceMock.Object, employeeServiceMock.Object);

        employeeDateServiceMock.Setup(x => x.GetAll()).Throws(new Exception("An error occurred while retrieving employee dates."));

        var result = await controller.GetAllEmployeeDate();

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("An error occurred while retrieving employee dates.", notFoundResult.Value);
    }
}