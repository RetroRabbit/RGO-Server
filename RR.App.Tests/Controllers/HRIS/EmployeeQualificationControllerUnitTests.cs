using HRIS.Models;
using HRIS.Services.Interfaces;
using HRIS.Services.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RR.App.Controllers.HRIS;
using RR.Tests.Data;
using RR.Tests.Data.Models.HRIS;
using Xunit;

namespace RR.App.Tests.Controllers.HRIS;

public class EmployeeQualificationControllerUnitTests
{
    private readonly Mock<IEmployeeQualificationService> _mockEmployeeQualificationService;
    private readonly EmployeeQualificationController _employeeQualificationController;
    private readonly EmployeeQualificationDto _employeeQualificationDto;

    public EmployeeQualificationControllerUnitTests()
    {
        _mockEmployeeQualificationService = new Mock<IEmployeeQualificationService>();

        _employeeQualificationController = new EmployeeQualificationController(new AuthorizeIdentityMock(1), _mockEmployeeQualificationService.Object);

        _employeeQualificationDto = EmployeeQualificationTestData.EmployeeQualification.ToDto();
    }

    [Fact]
    public async Task SaveEmployeeQualificationReturnsOkObjectResultWithQualification()
    {
        _mockEmployeeQualificationService.Setup(x => x.SaveEmployeeQualification(_employeeQualificationDto, _employeeQualificationDto.EmployeeId))
            .ReturnsAsync(_employeeQualificationDto);

        var result = await _employeeQualificationController.SaveEmployeeQualification(_employeeQualificationDto);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsType<EmployeeQualificationDto>(okResult.Value);
        Assert.Equal(_employeeQualificationDto, returnValue);
    }

    [Fact]
    public async Task UpdateEmployeeQualificationReturnsOkObjectResultWithUpdatedQualification()
    {
        _mockEmployeeQualificationService.Setup(x => x.UpdateEmployeeQualification(_employeeQualificationDto))
            .ReturnsAsync(_employeeQualificationDto);

        var result = await _employeeQualificationController.UpdateEmployeeQualification(1, _employeeQualificationDto);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsType<EmployeeQualificationDto>(okResult.Value);
        Assert.Equal(_employeeQualificationDto, returnValue);
    }
    [Fact]
    public async Task UpdateEmployeeQualificationFail()
    {
        _mockEmployeeQualificationService.Setup(x => x.UpdateEmployeeQualification(_employeeQualificationDto))
            .ThrowsAsync(new CustomException("An error occurred while updating qualifications"));

        var exception = await Assert.ThrowsAsync<CustomException>(async () =>
            await _employeeQualificationController.UpdateEmployeeQualification(1, _employeeQualificationDto));

        Assert.Equal("An error occurred while updating qualifications", exception.Message);
    }

    [Fact(Skip = "Current user needs to be set for validations on endpoint")]
    public async Task GetEmployeeQualificationByEmployeeIdReturnsOkObjectResultWithQualifications()
    {
        _mockEmployeeQualificationService.Setup(x => x.GetAllEmployeeQualificationsByEmployeeId(_employeeQualificationDto.Id))
            .ReturnsAsync(EmployeeQualificationTestData.EmployeeQualification.ToDto);

        var result = await _employeeQualificationController.GetEmployeeQualificationByEmployeeId(_employeeQualificationDto.Id);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnValue = Assert.IsType<EmployeeQualificationDto>(okResult.Value);
        Assert.Equal(_employeeQualificationDto, returnValue);
    }

    [Fact]
    public async Task DeleteEmployeeQualificationReturnsOkObjectResultWithDeletedQualification()
    {
        _mockEmployeeQualificationService.Setup(x => x.DeleteEmployeeQualification(_employeeQualificationDto.Id))
            .ReturnsAsync(_employeeQualificationDto);

        var result = await _employeeQualificationController.DeleteEmployeeQualification(_employeeQualificationDto.Id);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsType<EmployeeQualificationDto>(okResult.Value);
        Assert.Equal(_employeeQualificationDto, returnValue);
    }

    [Fact]
    public async Task GetAllEmployeeQualifications()
    {
        var listOfEmployeeQualificationDto = new List<EmployeeQualificationDto>()
            {
               _employeeQualificationDto,
               _employeeQualificationDto
            };

        _mockEmployeeQualificationService.Setup(x => x.GetAllEmployeeQualifications())
            .ReturnsAsync(listOfEmployeeQualificationDto);

        var result = await _employeeQualificationController.GetAllEmployeeQualifications();

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnValue = Assert.IsType<List<EmployeeQualificationDto>>(okResult.Value);
        Assert.Equal(listOfEmployeeQualificationDto, returnValue);
    }
}
