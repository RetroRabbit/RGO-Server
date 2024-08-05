using HRIS.Models.Employee.Commons;
using HRIS.Services.Interfaces;
using HRIS.Services.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RR.App.Controllers.HRIS;
using RR.App.Tests.Helper;
using RR.Tests.Data;
using RR.Tests.Data.Models.HRIS;
using Xunit;

namespace RR.App.Tests.Controllers.HRIS;

public class WorkExperienceControllerUnitTest
{
    private readonly Mock<IWorkExperienceService> _workExperienceServiceMock;
    private readonly WorkExperienceController _controller;
    private readonly WorkExperienceDto _workExperienceDto;
    private readonly Mock<AuthorizeIdentityMock> _identityMock;
    private readonly List<WorkExperienceDto> _workExperienceDtoList;

    public WorkExperienceControllerUnitTest()
    {
        _workExperienceServiceMock = new Mock<IWorkExperienceService>();
        _identityMock = new Mock<AuthorizeIdentityMock>();
        _controller = new WorkExperienceController(_identityMock.Object, _workExperienceServiceMock.Object);
        _workExperienceDto = WorkExperienceTestData.WorkExperienceOne.ToDto();
        _workExperienceDtoList = new List<WorkExperienceDto>
        {
            WorkExperienceTestData.WorkExperienceOne.ToDto(),
            WorkExperienceTestData.WorkExperienceOne.ToDto()
        };
    }

    [Fact]
    public async Task CreateWorkExperience_Success()
    {
        _identityMock.Setup(i => i.Role).Returns("Admin");
        _identityMock.SetupGet(i => i.EmployeeId).Returns(1);

        _workExperienceDto.EmployeeId = 1;

        _workExperienceServiceMock.Setup(service => service.Save(_workExperienceDto))
                                  .ReturnsAsync(_workExperienceDto);

        var result = await _controller.CreateWorkExperience(_workExperienceDto);

        var createdResult = Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal(StatusCodes.Status201Created, createdResult.StatusCode);
        Assert.Equal(_workExperienceDto, createdResult.Value);
    }

    [Fact]
    public async Task CreateEmployeeWorkExperience_UnauthorizedAccess()
    {
        _identityMock.Setup(i => i.Role).Returns("Employee");
        _identityMock.SetupGet(i => i.EmployeeId).Returns(5);

        _workExperienceServiceMock.Setup(service => service.Save(_workExperienceDto))
                              .ThrowsAsync(new CustomException("User data being accessed does not match user making the request."));

        var result = await MiddlewareHelperUnitTests.SimulateHandlingExceptionMiddlewareAsync(async () => await _controller.CreateWorkExperience(_workExperienceDto));

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("User data being accessed does not match user making the request.", notFoundResult.Value);
    }

    [Fact]
    public async Task GetWorkExperienceById_Success()
    {
        _identityMock.Setup(i => i.Role).Returns("Admin");
        _identityMock.SetupGet(i => i.EmployeeId).Returns(1);

        _workExperienceServiceMock.Setup(service => service.GetWorkExperienceByEmployeeId(1))
                                  .ReturnsAsync(_workExperienceDtoList);

        var result = await _controller.GetWorkExperienceById(1);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
        Assert.Equal(_workExperienceDtoList, okResult.Value);
    }

    [Fact]
    public async Task GetEmployeeWorkExperience_UnauthorizedAccess()
    {
        _identityMock.Setup(i => i.Role).Returns("Developer");
        _identityMock.SetupGet(i => i.EmployeeId).Returns(5);

        var result = await MiddlewareHelperUnitTests.SimulateHandlingExceptionMiddlewareAsync(async () => await _controller.GetWorkExperienceById(2));

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
        Assert.Equal("User data being accessed does not match user making the request.", notFoundResult.Value);
    }

    [Fact]
    public async Task UpdateWorkExperience_Success()
    {
        _identityMock.Setup(i => i.Role).Returns("Admin");
        _identityMock.SetupGet(i => i.EmployeeId).Returns(1);

        _workExperienceDto.Id = 1;

        _workExperienceServiceMock.Setup(service => service.Update(_workExperienceDto))
                                  .ReturnsAsync(_workExperienceDto);

        var result = await _controller.UpdateWorkExperience(_workExperienceDto);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
        Assert.Equal(_workExperienceDto, okResult.Value);
    }

    [Fact]
    public async Task UpdateEmployeeWorkExperience_Unauthorized()
    {
        _workExperienceServiceMock.Setup(service => service.Update(_workExperienceDto))
                              .ThrowsAsync(new CustomException("User data being accessed does not match user making the request."));

        var result = await MiddlewareHelperUnitTests.SimulateHandlingExceptionMiddlewareAsync(async () => await _controller.UpdateWorkExperience(_workExperienceDto));

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
        Assert.Equal("User data being accessed does not match user making the request.", notFoundResult.Value);
    }

    [Fact]
    public async Task DeleteWorkExperience_Success()
    {
        _identityMock.Setup(i => i.Role).Returns("Admin");
        _identityMock.SetupGet(i => i.EmployeeId).Returns(1);

        _workExperienceServiceMock.Setup(service => service.Delete(1))
                                  .ReturnsAsync(_workExperienceDto);

        var result = await _controller.DeleteWorkExperience(1);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
        Assert.Equal(_workExperienceDto, okResult.Value);
    }

    [Fact]
    public async Task DeleteEmployeeWorkExperience_Unauthorized()
    {
        _workExperienceServiceMock.Setup(service => service.Delete(_workExperienceDto.Id))
                              .ThrowsAsync(new CustomException("Unauthorized Access."));

        var result = await MiddlewareHelperUnitTests.SimulateHandlingExceptionMiddlewareAsync(async () => await _controller.DeleteWorkExperience(1));

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
    }
}