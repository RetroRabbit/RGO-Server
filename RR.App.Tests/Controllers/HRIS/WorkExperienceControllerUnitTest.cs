using HRIS.Models;
using HRIS.Services.Interfaces;
using HRIS.Services.Services;
using HRIS.Services.Session;
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

    //[Fact(Skip = "Current user needs to be set for validations on endpoint")]
    //public async Task SaveWorkExperienceReturnsOk()
    //{
    //    _workExperienceServiceMock.Setup(x => x.Save(_workExperienceDto)).ReturnsAsync(_workExperienceDto);

    //    var controllerResult = await _controller.SaveWorkExperience(_workExperienceDto);

    //    var actionResult = Assert.IsType<CreatedAtActionResult>(controllerResult);
    //    Assert.NotNull(actionResult.Value);
    //    Assert.Equal(_workExperienceDto, actionResult.Value);
    //}

    //[Fact(Skip = "Needs fixing")]
    //public async Task SaveWorkExperienceFail()
    //{
    //    _workExperienceServiceMock.Setup(x => x.Save(_workExperienceDto)).ThrowsAsync(new Exception("unexpected error occurred"));

    //    var controllerResult = await _controller.SaveWorkExperience(_workExperienceDto);

    //    var actionResult = Assert.IsType<ObjectResult>(controllerResult);
    //    Assert.Equal(500, actionResult.StatusCode);

    //    var problemDetails = actionResult.Value as ProblemDetails;

    //    Assert.NotNull(problemDetails);
    //    Assert.Equal("Could not save data.", problemDetails.Detail);
    //}

    //[Fact(Skip = "Current user needs to be set for validations on endpoint")]
    //public async Task SaveWorkExperienceAlreadyExists()
    //{
    //    _workExperienceServiceMock.Setup(x => x.Save(_workExperienceDto)).ThrowsAsync(new Exception("work experience exists"));

    //    var controllerResult = await _controller.SaveWorkExperience(_workExperienceDto);
    //    var actionResult = Assert.IsType<BadRequestObjectResult>(controllerResult);

    //    Assert.Equal(400, actionResult.StatusCode);

    //    var errorMessage = actionResult.Value as string;

    //    Assert.NotNull(errorMessage);
    //    Assert.Equal("work experience exists", errorMessage);
    //}

    //[Fact(Skip = "Current user needs to be set for validations on endpoint")]
    //public async Task GetWorkExperienceByIdPass()
    //{
    //    var workExperienceList = new List<WorkExperienceDto> { _workExperienceDto };

    //    _workExperienceServiceMock.Setup(x => x.GetWorkExperienceByEmployeeId(_workExperienceDto.EmployeeId)).ReturnsAsync(workExperienceList);

    //    var controllerResult = await _controller.GetWorkExperienceById(_workExperienceDto.EmployeeId);

    //    var actionResult = Assert.IsType<OkObjectResult>(controllerResult);
    //    Assert.NotNull(actionResult.Value);
    //    Assert.Equal(workExperienceList, actionResult.Value);
    //}

    //[Fact(Skip = "Needs fixing")]
    //public async Task GetWorkExperienceByIdFail()
    //{
    //    _workExperienceServiceMock.Setup(x => x.GetWorkExperienceByEmployeeId(_workExperienceDto.EmployeeId)).ThrowsAsync(new Exception());

    //    var controllerResult = await _controller.GetWorkExperienceById(_workExperienceDto.EmployeeId);

    //    var actionResult = Assert.IsType<NotFoundObjectResult>(controllerResult);
    //    Assert.Equal(404, actionResult.StatusCode);
    //}

    //[Fact(Skip = "Current user needs to be set for validations on endpoint")]
    //public async Task UpdateWorkExperiencePass()
    //{
    //    _workExperienceServiceMock.Setup(x => x.Update(_workExperienceDto)).ReturnsAsync(_workExperienceDto);

    //    var controllerResult = await _controller.UpdateWorkExperience(_workExperienceDto);

    //    var actionResult = Assert.IsType<OkObjectResult>(controllerResult);
    //    Assert.NotNull(actionResult.Value);
    //    Assert.Equal(200, actionResult.StatusCode);
    //}

    //[Fact(Skip = "Needs fixing")]
    //public async Task UpdateWorkExperienceFail()
    //{
    //    _workExperienceServiceMock.Setup(x => x.Update(_workExperienceDto)).ThrowsAsync(new Exception());

    //    var controllerResult = await _controller.UpdateWorkExperience(_workExperienceDto);

    //    var actionResult = Assert.IsType<BadRequestObjectResult>(controllerResult);
    //    Assert.Equal(400, actionResult.StatusCode);
    //    Assert.Equal("Work experience could not be updated", actionResult.Value);
    //}

    //[Fact(Skip = "Current user needs to be set for validations on endpoint")]
    //public async Task DeleteWorkExperiencePass()
    //{
    //    _workExperienceServiceMock.Setup(x => x.Delete(_workExperienceDto.Id)).ReturnsAsync(_workExperienceDto);

    //    var controllerResult = await _controller.DeleteWorkExperience(_workExperienceDto.Id);

    //    var actionResult = Assert.IsType<OkResult>(controllerResult);
    //    Assert.NotNull(actionResult);
    //    Assert.Equal(200, actionResult.StatusCode);
    //}

    //[Fact(Skip = "Needs fixing")]
    //public async Task DeleteWorkExperienceFail()
    //{
    //    _workExperienceServiceMock.Setup(x => x.Delete(_workExperienceDto.Id)).ThrowsAsync(new Exception());

    //    var controllerResult = await _controller.DeleteWorkExperience(_workExperienceDto.Id);

    //    var actionResult = Assert.IsType<NotFoundObjectResult>(controllerResult);
    //    Assert.Equal(404, actionResult.StatusCode);
    //}

    [Fact]
    public async Task getter() { }

    //[Fact]
    //public async Task GetEmployeeWorkExperience_ReturnsOkResult()
    //{
    //    _identityMock.Setup(i => i.Role).Returns("Admin");
    //    _identityMock.SetupGet(i => i.EmployeeId).Returns(2);

    //    _workExperienceServiceMock.Setup(x => x.GetWorkExperienceByEmployeeId(_workExperienceDto.EmployeeId))
    //                           .Returns(WorkExperienceTestData.WorkExperienceTwo.ToDto);

    //    var result = await _controller.GetWorkExperienceById(_workExperienceDto.EmployeeId);

    //    var okResult = Assert.IsType<OkObjectResult>(result);

    //    _workExperienceServiceMock.Verify(service => service.GetWorkExperienceByEmployeeId(_workExperienceDto.EmployeeId), Times.Once);
    //}

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


    //[Fact(Skip = "fix the returns ok tests")]
    //public async Task CreateEmployeeWorkExperience_ReturnsOkResults()
    //{
    //    _identityMock.Setup(i => i.Role).Returns("SuperAdmin");
    //    _identityMock.SetupGet(i => i.EmployeeId).Returns(2);

    //    _workExperienceServiceMock.Setup(x => x.Save(_workExperienceDto))
    //                           .ReturnsAsync(_workExperienceDto);

    //    var result = await _controller.CreateWorkExperience(_workExperienceDto);

    //    var okResult = Assert.IsType<OkObjectResult>(result);
    //    var returnValue = Assert.IsType<EmployeeDataDto>(okResult.Value);
    //    Assert.Equivalent(_workExperienceDto, returnValue);
    //    _workExperienceServiceMock.Verify(service => service.Save(_workExperienceDto), Times.Once);
    //}



    //[Fact(Skip = "fix the returns ok tests")]
    //public async Task UpdateEmployeeWorkExperience_ReturnsOkResult()
    //{
    //    _identityMock.Setup(i => i.Role).Returns("SuperAdmin");
    //    _identityMock.SetupGet(i => i.EmployeeId).Returns(2);

    //    _workExperienceServiceMock.Setup(x => x.Update(_workExperienceDto))
    //                           .ReturnsAsync(_workExperienceDto);

    //    var result = await _controller.UpdateWorkExperience(_workExperienceDto);

    //    var okResult = Assert.IsType<OkObjectResult>(result);
    //    var returnValue = Assert.IsType<EmployeeDataDto>(okResult.Value);
    //    Assert.Equivalent(_workExperienceDto, returnValue);
    //    _workExperienceServiceMock.Verify(service => service.Update(_workExperienceDto), Times.Once);
    //}



    //[Fact(Skip = "fix the returns ok tests")]
    //public async Task DeleteEmployeeWorkExperience_ReturnsOkResult()
    //{
    //    _identityMock.Setup(i => i.Role).Returns("SuperAdmin");
    //    _identityMock.SetupGet(i => i.EmployeeId).Returns(2);

    //    _workExperienceServiceMock.Setup(service => service.Delete(_workExperienceDto.Id))
    //                           .ReturnsAsync(_workExperienceDto);

    //    var result = await _controller.DeleteWorkExperience(_workExperienceDto.Id);

    //    var okResult = Assert.IsType<OkObjectResult>(result);
    //    var returnValue = Assert.IsType<EmployeeDataDto>(okResult.Value);
    //    Assert.Equivalent(_workExperienceDto, returnValue);
    //    _workExperienceServiceMock.Verify(service => service.Delete(_workExperienceDto.Id), Times.Once);
    //}


}