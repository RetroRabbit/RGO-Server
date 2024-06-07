using HRIS.Models;
using HRIS.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RR.App.Controllers.HRIS;
using RR.Tests.Data.Models.HRIS;
using RR.UnitOfWork;
using Xunit;

namespace RR.App.Tests.Controllers.HRIS;

public class EmployeeCertificationControllerUnitTests
{ 

    private readonly EmployeeCertificationController _controller;
    private readonly Mock<IUnitOfWork> _dbMock;
    private readonly Mock<IEmployeeCertificationService> _mockService;

    private readonly EmployeeCertificationDto employeeCertificationDto;
    private readonly List<EmployeeCertificationDto> certificates;
    
    public EmployeeCertificationControllerUnitTests()
    {
        _dbMock = new Mock<IUnitOfWork>();
        _mockService = new Mock<IEmployeeCertificationService>();
        _controller = new EmployeeCertificationController(_mockService.Object);
        employeeCertificationDto =
            new EmployeeCertificationDto
            {
                Id = 1,
                IssueDate = DateTime.Now,
                IssueOrganization = "Amazon",
                CertificateDocument = "asd",
                CertificateName = "Name",
                EmployeeId = EmployeeTestData.EmployeeDto.Id,
            };

        certificates = new List<EmployeeCertificationDto> { employeeCertificationDto };
    }

    [Fact]
    public async Task GetAllEmployeelCertiificatesPass()
    {
        _mockService.Setup(s => s.GetAllEmployeeCertifications(EmployeeTestData.EmployeeDto.Id))
            .ReturnsAsync(certificates);

        var result = await _controller.GetAllEmployeelCertiificates(EmployeeTestData.EmployeeDto.Id);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var model = Assert.IsAssignableFrom<IEnumerable<EmployeeCertificationDto>>(okResult.Value);
        Assert.Equal(model, certificates);
    }
    
    [Fact]
    public async Task GetAllEmployeelCertiificatesFail()
    {
        _mockService.Setup(s => s.GetAllEmployeeCertifications(EmployeeTestData.EmployeeDto.Id + 1))
            .ThrowsAsync(new Exception("Employee not found"));

        var result = await _controller.GetAllEmployeelCertiificates(EmployeeTestData.EmployeeDto.Id + 1);

        var notFoundResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Employee not found", notFoundResult.Value);
    }
    
    [Fact]
    public async Task SaveEmployeelCertiificatesPass()
    {
        _mockService.Setup(s => s.SaveEmployeeCertification(employeeCertificationDto))
            .ReturnsAsync(employeeCertificationDto);

        var result = await _controller.SaveEmployeeCertificate(employeeCertificationDto);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(employeeCertificationDto, okResult.Value);
    }
    
    [Fact]
    public async Task SaveEmployeelCertiificatesFail()
    {
        _mockService.Setup(s => s.SaveEmployeeCertification(employeeCertificationDto))
            .ThrowsAsync(new Exception("Employee not found"));

        var result = await _controller.SaveEmployeeCertificate(employeeCertificationDto);

        var notFoundResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Employee not found", notFoundResult.Value);
    }
    
    [Fact]
    public async Task GetEmployeelCertiificatesPass()
    {
        _mockService.Setup(s => s.GetEmployeeCertification(employeeCertificationDto.EmployeeId,employeeCertificationDto.Id))
            .ReturnsAsync(employeeCertificationDto);

        var result = await _controller.GetEmployeeCertificate(employeeCertificationDto.EmployeeId,employeeCertificationDto.Id);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(employeeCertificationDto, okResult.Value);
    }
    
    [Fact]
    public async Task GetEmployeelCertiificatesFail()
    {
        _mockService.Setup(s => s.GetEmployeeCertification(employeeCertificationDto.EmployeeId, employeeCertificationDto.Id))
            .ThrowsAsync(new Exception("Employee not found"));

        var result = await _controller.GetEmployeeCertificate(employeeCertificationDto.EmployeeId, employeeCertificationDto.Id);

        var notFoundResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Employee not found", notFoundResult.Value);
    }
    
    [Fact]
    public async Task DeleteEmployeelCertiificatesPass()
    {
        _mockService.Setup(s => s.DeleteEmployeeCertification(employeeCertificationDto.Id))
            .ReturnsAsync(employeeCertificationDto);

        var result = await _controller.DeleteEmployeeCertificate(employeeCertificationDto.Id);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(employeeCertificationDto, okResult.Value);
    }
    
    [Fact]
    public async Task DeleteEmployeelCertiificatesFail()
    {
        _mockService.Setup(s => s.DeleteEmployeeCertification(employeeCertificationDto.Id))
            .ThrowsAsync(new Exception("Employee not found"));

        var result = await _controller.DeleteEmployeeCertificate(employeeCertificationDto.Id);

        var notFoundResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Employee not found", notFoundResult.Value);
    }
    
    [Fact]
    public async Task UpdateEmployeelCertiificatesPass()
    {
        _mockService.Setup(s => s.UpdateEmployeeCertification(employeeCertificationDto))
            .ReturnsAsync(employeeCertificationDto);

        var result = await _controller.UpdateCertificate(employeeCertificationDto);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(employeeCertificationDto, okResult.Value);
    }
    
    [Fact]
    public async Task UpdateEmployeelCertiificatesFail()
    {
        _mockService.Setup(s => s.UpdateEmployeeCertification(employeeCertificationDto))
            .ThrowsAsync(new Exception("Employee not found"));

        var result = await _controller.UpdateCertificate(employeeCertificationDto);

        var notFoundResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Employee not found", notFoundResult.Value);
    }
}