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
            "test@example.com",
            "Test Subject",
            "Test Note",
            new DateOnly(2023, 1, 1)
        );

        employeeServiceMock.Setup(x => x.GetEmployee(employeeDateInput.Email)).ReturnsAsync(new EmployeeDto
        (
            1,
            "EmployeeNumber Test",
            "TaxNumber Test",
            new DateOnly(2023, 11, 20),
            new DateOnly(2023, 12, 23),
            1,
            false,
            "Disabilities false",
            3,
            new EmployeeTypeDto(1, "Type"),
            "Your notes here",
            1.5f,
            20.5f,
            15.0f,
            50000,
            "John",
            "J",
            "Doe",
            new DateOnly(1990, 1, 1),
            "Country",
            "Nationality",
            "ID123456",
            "AB123456",
            new DateOnly(2025, 12, 31),
            "PassportCountry",
            Race.White,
            Gender.Male,
            "path/to/photo.jpg",
            "test@example.com",
            "john.personal@example.com",
            "123456789",
            1,
            2,
            new EmployeeAddressDto(1, "UnitNumber Test", "ComplexName Test", "StreetNumber Test", "SuburbOrDistrict Test", "City Test", "Country Test", "Province Test", "PostalCode Test"),
            new EmployeeAddressDto(2, "UnitNumber Test", "ComplexName Test", "StreetNumber Test", "SuburbOrDistrict Test", "City Test", "Country Test", "Province Test", "PostalCode Test"),
            "123",
            "Emergency Name",
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
            "test@example.com",
            "Test Subject",
            "Test Note",
            new DateOnly(2023, 1, 1)
        );

        employeeDateServiceMock.Setup(x => x.Save(It.IsAny<EmployeeDateDto>())).ThrowsAsync(new Exception("Exception message"));

        var result = await controller.SaveEmployeeDate(employeeDateInput);

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("Exception message", notFoundResult.Value);
    }

    [Fact]
    public async Task DeleteEmployeeDateValidInputReturnsOkResult()
    {
        var employeeDateServiceMock = new Mock<IEmployeeDateService>();
        var employeeServiceMock = new Mock<IEmployeeService>();

        var controller = new EmployeeDateController(employeeDateServiceMock.Object, employeeServiceMock.Object);

        var employeeDateInput = new EmployeeDateInput
        (
            "test@example.com",
            "Test Subject",
            "Test Note",
            new DateOnly(2023, 1, 1)
        );

        employeeServiceMock.Setup(x => x.GetEmployee(employeeDateInput.Email))
        .ReturnsAsync((new EmployeeDto
        (
            1,
            "EmployeeNumber Test",
            "TaxNumber Test",
            new DateOnly(2023, 11, 20),
            new DateOnly(2023, 12, 23),
            1,
            false,
            "Disabilities false",
            3,
            new EmployeeTypeDto(1, "Type"),
            "Your notes here",
            1.5f,
            20.5f,
            15.0f,
            50000,
            "John",
            "J",
            "Doe",
            new DateOnly(1990, 1, 1),
            "Country",
            "Nationality",
            "ID123456",
            "AB123456",
            new DateOnly(2025, 12, 31),
            "PassportCountry",
            Race.White,
            Gender.Male,
            "path/to/photo.jpg",
            "test@example.com",
            "john.personal@example.com",
            "123456789",
            1,
            2,
            new EmployeeAddressDto(1, "UnitNumber Test", "ComplexName Test", "StreetNumber Test", "SuburbOrDistrict Test", "City Test", "Country Test", "Province Test", "PostalCode Test"),
            new EmployeeAddressDto(2, "UnitNumber Test", "ComplexName Test", "StreetNumber Test", "SuburbOrDistrict Test", "City Test", "Country Test", "Province Test", "PostalCode Test"),
            "123",
            "Emergency Name",
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
            "test@example.com",
            "Test Subject",
            "Test Note",
            new DateOnly(2023, 1, 1)
        );

        employeeDateServiceMock.Setup(x => x.Delete(It.IsAny<EmployeeDateDto>())).ThrowsAsync(new Exception("Exception message"));

        var result = await controller.DeleteEmployeeDate(employeeDateInput);

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("Exception message", notFoundResult.Value);
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
                "EmployeeNumber Test",
                "TaxNumber Test",
                new DateOnly(2023, 11, 20),
                new DateOnly(2023, 12, 23),
                1,
                false,
                "Disabilities false",
                3,
                new EmployeeTypeDto(1, "Type"),
                "Your notes here",
                1.5f,
                20.5f,
                15.0f,
                50000,
                "John",
                "J",
                "Doe",
                new DateOnly(1990, 1, 1),
                "Country",
                "Nationality",
                "ID123456",
                "AB123456",
                new DateOnly(2025, 12, 31),
                "PassportCountry",
                Race.White,
                Gender.Male,
                "path/to/photo.jpg",
                "test@example.com",
                "john.personal@example.com",
                "123456789",
                1,
                2,
                new EmployeeAddressDto(1, "UnitNumber Test", "ComplexName Test", "StreetNumber Test", "SuburbOrDistrict Test", "City Test", "Country Test", "Province Test", "PostalCode Test"),
                new EmployeeAddressDto(2, "UnitNumber Test", "ComplexName Test", "StreetNumber Test", "SuburbOrDistrict Test", "City Test", "Country Test", "Province Test", "PostalCode Test"),
                "123",
                "Emergency Name",
                "987654321"
            ),
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
                "EmployeeNumber Test",
                "TaxNumber Test",
                new DateOnly(2023, 11, 20),
                new DateOnly(2023, 12, 23),
                1,
                false,
                "Disabilities false",
                3,
                new EmployeeTypeDto(1, "Type"),
                "Your notes here",
                1.5f,
                20.5f,
                15.0f,
                50000,
                "John",
                "J",
                "Doe",
                new DateOnly(1990, 1, 1),
                "Country",
                "Nationality",
                "ID123456",
                "AB123456",
                new DateOnly(2025, 12, 31),
                "PassportCountry",
                Race.White,
                Gender.Male,
                "path/to/photo.jpg",
                "test@example.com",
                "john.personal@example.com",
                "123456789",
                1,
                2,
                new EmployeeAddressDto(1, "UnitNumber Test", "ComplexName Test", "StreetNumber Test", "SuburbOrDistrict Test", "City Test", "Country Test", "Province Test", "PostalCode Test"),
                new EmployeeAddressDto(2, "UnitNumber Test", "ComplexName Test", "StreetNumber Test", "SuburbOrDistrict Test", "City Test", "Country Test", "Province Test", "PostalCode Test"),
                "123",
                "Emergency Name",
                "987654321"
            ),
            "Test Subject",
            "Test Note",
            new DateOnly(2023, 1, 1)
        );

        employeeDateServiceMock.Setup(x => x.Update(It.IsAny<EmployeeDateDto>())).ThrowsAsync(new Exception("Exception message"));

        var result = await controller.UpdateEmployeeDate(employeeDateDto);

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("Exception message", notFoundResult.Value);
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
                    "EmployeeNumber Test",
                    "TaxNumber Test",
                    new DateOnly(2023, 11, 20),
                    new DateOnly(2023, 12, 23),
                    1,
                    false,
                    "Disabilities false",
                    3,
                    new EmployeeTypeDto(1, "Type"),
                    "Your notes here",
                    1.5f,
                    20.5f,
                    15.0f,
                    50000,
                    "John",
                    "J",
                    "Doe",
                    new DateOnly(1990, 1, 1),
                    "Country",
                    "Nationality",
                    "ID123456",
                    "AB123456",
                    new DateOnly(2025, 12, 31),
                    "PassportCountry",
                    Race.White,
                    Gender.Male,
                    "path/to/photo.jpg",
                    "test@example.com",
                    "john.personal@example.com",
                    "123456789",
                    1,
                    2,
                    new EmployeeAddressDto(1, "UnitNumber Test", "ComplexName Test", "StreetNumber Test", "SuburbOrDistrict Test", "City Test", "Country Test", "Province Test", "PostalCode Test"),
                    new EmployeeAddressDto(2, "UnitNumber Test", "ComplexName Test", "StreetNumber Test", "SuburbOrDistrict Test", "City Test", "Country Test", "Province Test", "PostalCode Test"),
                    "123",
                    "Emergency Name",
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
                    "EmployeeNumber Test",
                    "TaxNumber Test",
                    new DateOnly(2023, 11, 20),
                    new DateOnly(2023, 12, 23),
                    1,
                    false,
                    "Disabilities false",
                    3,
                    new EmployeeTypeDto(1, "Type"),
                    "Your notes here",
                    1.5f,
                    20.5f,
                    15.0f,
                    50000,
                    "John",
                    "J",
                    "Doe",
                    new DateOnly(1990, 1, 1),
                    "Country",
                    "Nationality",
                    "ID123456",
                    "AB123456",
                    new DateOnly(2025, 12, 31),
                    "PassportCountry",
                    Race.White,
                    Gender.Male,
                    "path/to/photo.jpg",
                    "test@example.com",
                    "john.personal@example.com",
                    "123456789",
                    1,
                    2,
                    new EmployeeAddressDto(1, "UnitNumber Test", "ComplexName Test", "StreetNumber Test", "SuburbOrDistrict Test", "City Test", "Country Test", "Province Test", "PostalCode Test"),
                    new EmployeeAddressDto(2, "UnitNumber Test", "ComplexName Test", "StreetNumber Test", "SuburbOrDistrict Test", "City Test", "Country Test", "Province Test", "PostalCode Test"),
                    "123",
                    "Emergency Name",
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

        var email = "test@example.com";
        var expectedEmployeeDates = new List<EmployeeDateDto>
        {
            new EmployeeDateDto
            (
                1,
                new EmployeeDto
                (
                    1,
                    "EmployeeNumber Test",
                    "TaxNumber Test",
                    new DateOnly(2023, 11, 20),
                    new DateOnly(2023, 12, 23),
                    1,
                    false,
                    "Disabilities false",
                    3,
                    new EmployeeTypeDto(1, "Type"),
                    "Your notes here",
                    1.5f,
                    20.5f,
                    15.0f,
                    50000,
                    "John",
                    "J",
                    "Doe",
                    new DateOnly(1990, 1, 1),
                    "Country",
                    "Nationality",
                    "ID123456",
                    "AB123456",
                    new DateOnly(2025, 12, 31),
                    "PassportCountry",
                    Race.White,
                    Gender.Male,
                    "path/to/photo.jpg",
                    "test@example.com",
                    "john.personal@example.com",
                    "123456789",
                    1,
                    2,
                    new EmployeeAddressDto(1, "UnitNumber Test", "ComplexName Test", "StreetNumber Test", "SuburbOrDistrict Test", "City Test", "Country Test", "Province Test", "PostalCode Test"),
                    new EmployeeAddressDto(2, "UnitNumber Test", "ComplexName Test", "StreetNumber Test", "SuburbOrDistrict Test", "City Test", "Country Test", "Province Test", "PostalCode Test"),
                    "123",
                    "Emergency Name",
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
                    "EmployeeNumber Test",
                    "TaxNumber Test",
                    new DateOnly(2023, 11, 20),
                    new DateOnly(2023, 12, 23),
                    1,
                    false,
                    "Disabilities false",
                    3,
                    new EmployeeTypeDto(1, "Type"),
                    "Your notes here",
                    1.5f,
                    20.5f,
                    15.0f,
                    50000,
                    "John",
                    "J",
                    "Doe",
                    new DateOnly(1990, 1, 1),
                    "Country",
                    "Nationality",
                    "ID123456",
                    "AB123456",
                    new DateOnly(2025, 12, 31),
                    "PassportCountry",
                    Race.White,
                    Gender.Male,
                    "path/to/photo.jpg",
                    "test@example.com",
                    "john.personal@example.com",
                    "123456789",
                    1,
                    2,
                    new EmployeeAddressDto(1, "UnitNumber Test", "ComplexName Test", "StreetNumber Test", "SuburbOrDistrict Test", "City Test", "Country Test", "Province Test", "PostalCode Test"),
                    new EmployeeAddressDto(2, "UnitNumber Test", "ComplexName Test", "StreetNumber Test", "SuburbOrDistrict Test", "City Test", "Country Test", "Province Test", "PostalCode Test"),
                    "123",
                    "Emergency Name",
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
                    "EmployeeNumber Test",
                    "TaxNumber Test",
                    new DateOnly(2023, 11, 20),
                    new DateOnly(2023, 12, 23),
                    1,
                    false,
                    "Disabilities false",
                    3,
                    new EmployeeTypeDto(1, "Type"),
                    "Your notes here",
                    1.5f,
                    20.5f,
                    15.0f,
                    50000,
                    "John",
                    "J",
                    "Doe",
                    new DateOnly(1990, 1, 1),
                    "Country",
                    "Nationality",
                    "ID123456",
                    "AB123456",
                    new DateOnly(2025, 12, 31),
                    "PassportCountry",
                    Race.White,
                    Gender.Male,
                    "path/to/photo.jpg",
                    "test@example.com",
                    "john.personal@example.com",
                    "123456789",
                    1,
                    2,
                    new EmployeeAddressDto(1, "UnitNumber Test", "ComplexName Test", "StreetNumber Test", "SuburbOrDistrict Test", "City Test", "Country Test", "Province Test", "PostalCode Test"),
                    new EmployeeAddressDto(2, "UnitNumber Test", "ComplexName Test", "StreetNumber Test", "SuburbOrDistrict Test", "City Test", "Country Test", "Province Test", "PostalCode Test"),
                    "123",
                    "Emergency Name",
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

        employeeDateServiceMock.Setup(x => x.GetAll()).Throws(new Exception("Exception message"));

        var result = await controller.GetAllEmployeeDate();

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("Exception message", notFoundResult.Value);
    }
}