using HRIS.Models;
using HRIS.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Amqp.Transaction;
using Moq;
using RR.App.Controllers.HRIS;
using RR.App.Tests.Helper;
using RR.Tests.Data;
using RR.Tests.Data.Models.HRIS;
using System.Security.Claims;
using Xunit;

namespace RR.App.Tests.Controllers.HRIS;

public class EmployeeCertificationControllerUnitTests
{

    private readonly EmployeeCertificationController _controller;
    private readonly Mock<IEmployeeCertificationService> _employeeCertificationServiceMock;

    private readonly EmployeeCertificationDto _employeeCertificationDto;
    private readonly List<EmployeeCertificationDto> _employeeCertificationDtoList;

    public EmployeeCertificationControllerUnitTests()
    {
        _employeeCertificationServiceMock = new Mock<IEmployeeCertificationService>();
        _controller = new EmployeeCertificationController(new AuthorizeIdentityMock("test@example.com", "TestUser", "SuperAdmin", 1), _employeeCertificationServiceMock.Object);

        _employeeCertificationDto =
            new EmployeeCertificationDto
            {
                Id = 1,
                IssueDate = DateTime.Now,
                IssueOrganization = "Amazon",
                CertificateDocument = "asd",
                CertificateName = "Name",
                EmployeeId = EmployeeTestData.EmployeeOne.Id,
            };

        _employeeCertificationDtoList = new List<EmployeeCertificationDto> { _employeeCertificationDto };
    }

    [Fact]
    public async Task GetAllEmployeeCertificatesPass()
    {
        _employeeCertificationServiceMock.Setup(s => s.GetAllEmployeeCertifications(EmployeeTestData.EmployeeOne.Id))
            .ReturnsAsync(_employeeCertificationDtoList);

        var result = await _controller.GetAllEmployeelCertiificates(EmployeeTestData.EmployeeOne.Id);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var model = Assert.IsAssignableFrom<IEnumerable<EmployeeCertificationDto>>(okResult.Value);
        Assert.Equal(model, _employeeCertificationDtoList);
    }

    [Fact]
    public async Task GetAllEmployeeCertificatesCurUserPass()
    {
        var newController = new EmployeeCertificationController(new AuthorizeIdentityMock("test@example.com", "TestUser", "User", 1), _employeeCertificationServiceMock.Object);

        _employeeCertificationServiceMock.Setup(s => s.GetAllEmployeeCertifications(EmployeeTestData.EmployeeOne.Id))
            .ReturnsAsync(_employeeCertificationDtoList);

        var result = await newController.GetAllEmployeelCertiificates(EmployeeTestData.EmployeeOne.Id);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var model = Assert.IsAssignableFrom<IEnumerable<EmployeeCertificationDto>>(okResult.Value);
        Assert.Equal(model, _employeeCertificationDtoList);
    }

    [Fact]
    public async Task GetAllEmployeeCertificatesFail()
    {
        var newController = new EmployeeCertificationController(new AuthorizeIdentityMock(), _employeeCertificationServiceMock.Object);

        _employeeCertificationServiceMock.Setup(s => s.GetAllEmployeeCertifications(EmployeeTestData.EmployeeOne.Id + 1))
            .ThrowsAsync(new Exception("User data being accessed does not match user making the request."));

        var result = await MiddlewareHelperUnitTests.SimulateHandlingExceptionMiddlewareAsync(async () => await newController.GetAllEmployeelCertiificates(EmployeeTestData.EmployeeOne.Id + 1));

        var objectResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("User data being accessed does not match user making the request.", objectResult.Value);
    }

    [Fact]
    public async Task SaveEmployeeCertificatesPass()
    {
        _employeeCertificationServiceMock.Setup(s => s.SaveEmployeeCertification(_employeeCertificationDto))
            .ReturnsAsync(_employeeCertificationDto);

        var result = await _controller.SaveEmployeeCertificate(_employeeCertificationDto);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(_employeeCertificationDto, okResult.Value);
    }

    [Fact]
    public async Task GetEmployeeCertificatesPass()
    {
        _employeeCertificationServiceMock.Setup(s => s.GetEmployeeCertification(_employeeCertificationDto.EmployeeId, _employeeCertificationDto.Id))
            .ReturnsAsync(_employeeCertificationDto);

        var result = await _controller.GetEmployeeCertificate(_employeeCertificationDto.EmployeeId, _employeeCertificationDto.Id);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(_employeeCertificationDto, okResult.Value);
    }

    [Fact]
    public async Task GetEmployeeCertificatesCurUserPass()
    {
        var newController = new EmployeeCertificationController(new AuthorizeIdentityMock("test@example.com", "TestUser", "User", 1), _employeeCertificationServiceMock.Object);

        _employeeCertificationServiceMock.Setup(s => s.GetEmployeeCertification(_employeeCertificationDto.EmployeeId, _employeeCertificationDto.Id))
            .ReturnsAsync(_employeeCertificationDto);

        var result = await newController.GetEmployeeCertificate(_employeeCertificationDto.EmployeeId, _employeeCertificationDto.Id);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(_employeeCertificationDto, okResult.Value);
    }

    [Fact]
    public async Task GetEmployeeCertificatesFail()
    {
        var newController = new EmployeeCertificationController(new AuthorizeIdentityMock(), _employeeCertificationServiceMock.Object);

        _employeeCertificationServiceMock.Setup(s => s.GetEmployeeCertification(_employeeCertificationDto.EmployeeId, _employeeCertificationDto.Id))
            .ThrowsAsync(new Exception("User data being accessed does not match user making the request."));


        var result = await MiddlewareHelperUnitTests.SimulateHandlingExceptionMiddlewareAsync(async () => await newController.GetEmployeeCertificate(_employeeCertificationDto.EmployeeId, _employeeCertificationDto.Id));

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("User data being accessed does not match user making the request.", notFoundResult.Value);
    }

    [Fact]
    public async Task DeleteEmployeeCertificatesPass()
    {
        _employeeCertificationServiceMock.Setup(s => s.DeleteEmployeeCertification(_employeeCertificationDto.Id))
            .ReturnsAsync(_employeeCertificationDto);

        var result = await _controller.DeleteEmployeeCertificate(_employeeCertificationDto.Id);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(_employeeCertificationDto, okResult.Value);
    }

    [Fact]
    public async Task UpdateEmployeeCertificatesPass()
    {
        _employeeCertificationServiceMock.Setup(s => s.UpdateEmployeeCertification(_employeeCertificationDto))
            .ReturnsAsync(_employeeCertificationDto);

        var result = await _controller.UpdateCertificate(_employeeCertificationDto);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(_employeeCertificationDto, okResult.Value);
    }
}