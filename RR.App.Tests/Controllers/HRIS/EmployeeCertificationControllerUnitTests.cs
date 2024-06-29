using HRIS.Models;
using HRIS.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RR.App.Controllers.HRIS;
using RR.Tests.Data;
using RR.Tests.Data.Models.HRIS;
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
        _controller = new EmployeeCertificationController(new AuthorizeIdentityMock(), _employeeCertificationServiceMock.Object);

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

    [Fact(Skip = "Current user needs to be set for validations on endpoint")]
    public async Task GetAllEmployeelCertiificatesPass()
    {
        _employeeCertificationServiceMock.Setup(s => s.GetAllEmployeeCertifications(EmployeeTestData.EmployeeOne.Id))
            .ReturnsAsync(_employeeCertificationDtoList);

        var result = await _controller.GetAllEmployeelCertiificates(EmployeeTestData.EmployeeOne.Id);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var model = Assert.IsAssignableFrom<IEnumerable<EmployeeCertificationDto>>(okResult.Value);
        Assert.Equal(model, _employeeCertificationDtoList);
    }

    [Fact(Skip = "Current user needs to be set for validations on endpoint")]
    public async Task GetAllEmployeelCertiificatesFail()
    {
        _employeeCertificationServiceMock.Setup(s => s.GetAllEmployeeCertifications(EmployeeTestData.EmployeeOne.Id + 1))
            .ThrowsAsync(new Exception("Employee not found"));

        var result = await _controller.GetAllEmployeelCertiificates(EmployeeTestData.EmployeeOne.Id + 1);

        var notFoundResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Employee not found", notFoundResult.Value);
    }

    [Fact]
    public async Task SaveEmployeelCertiificatesPass()
    {
        _employeeCertificationServiceMock.Setup(s => s.SaveEmployeeCertification(_employeeCertificationDto))
            .ReturnsAsync(_employeeCertificationDto);

        var result = await _controller.SaveEmployeeCertificate(_employeeCertificationDto);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(_employeeCertificationDto, okResult.Value);
    }

    [Fact]
    public async Task SaveEmployeelCertiificatesFail()
    {
        _employeeCertificationServiceMock.Setup(s => s.SaveEmployeeCertification(_employeeCertificationDto))
            .ThrowsAsync(new Exception("Employee not found"));

        var result = await _controller.SaveEmployeeCertificate(_employeeCertificationDto);

        var notFoundResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Employee not found", notFoundResult.Value);
    }

    [Fact(Skip = "Current user needs to be set for validations on endpoint")]
    public async Task GetEmployeelCertiificatesPass()
    {
        _employeeCertificationServiceMock.Setup(s => s.GetEmployeeCertification(_employeeCertificationDto.EmployeeId, _employeeCertificationDto.Id))
            .ReturnsAsync(_employeeCertificationDto);

        var result = await _controller.GetEmployeeCertificate(_employeeCertificationDto.EmployeeId, _employeeCertificationDto.Id);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(_employeeCertificationDto, okResult.Value);
    }

    [Fact(Skip = "Current user needs to be set for validations on endpoint")]
    public async Task GetEmployeelCertiificatesFail()
    {
        _employeeCertificationServiceMock.Setup(s => s.GetEmployeeCertification(_employeeCertificationDto.EmployeeId, _employeeCertificationDto.Id))
            .ThrowsAsync(new Exception("Employee not found"));

        var result = await _controller.GetEmployeeCertificate(_employeeCertificationDto.EmployeeId, _employeeCertificationDto.Id);

        var notFoundResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Employee not found", notFoundResult.Value);
    }

    [Fact]
    public async Task DeleteEmployeelCertiificatesPass()
    {
        _employeeCertificationServiceMock.Setup(s => s.DeleteEmployeeCertification(_employeeCertificationDto.Id))
            .ReturnsAsync(_employeeCertificationDto);

        var result = await _controller.DeleteEmployeeCertificate(_employeeCertificationDto.Id);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(_employeeCertificationDto, okResult.Value);
    }

    [Fact]
    public async Task DeleteEmployeelCertiificatesFail()
    {
        _employeeCertificationServiceMock.Setup(s => s.DeleteEmployeeCertification(_employeeCertificationDto.Id))
            .ThrowsAsync(new Exception("Employee not found"));

        var result = await _controller.DeleteEmployeeCertificate(_employeeCertificationDto.Id);

        var notFoundResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Employee not found", notFoundResult.Value);
    }

    [Fact]
    public async Task UpdateEmployeelCertiificatesPass()
    {
        _employeeCertificationServiceMock.Setup(s => s.UpdateEmployeeCertification(_employeeCertificationDto))
            .ReturnsAsync(_employeeCertificationDto);

        var result = await _controller.UpdateCertificate(_employeeCertificationDto);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(_employeeCertificationDto, okResult.Value);
    }

    [Fact]
    public async Task UpdateEmployeelCertiificatesFail()
    {
        _employeeCertificationServiceMock.Setup(s => s.UpdateEmployeeCertification(_employeeCertificationDto))
            .ThrowsAsync(new Exception("Employee not found"));

        var result = await _controller.UpdateCertificate(_employeeCertificationDto);

        var notFoundResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Employee not found", notFoundResult.Value);
    }
}