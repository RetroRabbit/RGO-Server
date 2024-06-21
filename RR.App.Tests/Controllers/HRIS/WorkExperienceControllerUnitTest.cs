using HRIS.Models;
using HRIS.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RR.App.Controllers.HRIS;
using System.Security.Claims;
using Xunit;

namespace RR.App.Tests.Controllers.HRIS;

public class WorkExperienceControllerUnitTest
{
    private readonly Mock<IWorkExperienceService> _workExperienceServiceMock;
    private readonly WorkExperienceController _controller;
    private readonly WorkExperienceDto _workExperienceDto;

    public WorkExperienceControllerUnitTest()
    {
        _workExperienceServiceMock = new Mock<IWorkExperienceService>();
        _controller = new WorkExperienceController(_workExperienceServiceMock.Object);

        _workExperienceDto = new WorkExperienceDto
        {
            Id = 1,
            ClientName = "Capitec",
            ProjectName = "Project1",
            SkillSet = new List<string> { "front-end", "back-end" },
            Software = new List<string> { "c#", "java" },
            EmployeeId = 1,
            StartDate = new DateTime(2022, 1, 1),
            EndDate = new DateTime(2024, 1, 1),
        };
    }

    [Fact]
    public async Task saveWorkExperienceReturnsOk()
    {
        _workExperienceServiceMock.Setup(x => x.Save(_workExperienceDto)).ReturnsAsync(_workExperienceDto);

        var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] {
                                        new Claim(ClaimTypes.NameIdentifier, "TestUser"),
                                        new Claim(ClaimTypes.Email, "test@example.com"),
                                        new Claim(ClaimTypes.Role, "SuperAdmin")
                                   }, "TestAuthentication"));
        _controller.ControllerContext = new ControllerContext();
        _controller.ControllerContext.HttpContext = new DefaultHttpContext { User = user };

        var controllerResult = await _controller.SaveWorkExperience(_workExperienceDto);

        var actionResult = Assert.IsType<CreatedAtActionResult>(controllerResult);
        Assert.NotNull(actionResult.Value);
        Assert.Equal(_workExperienceDto, actionResult.Value);
    }

    [Fact]
    public async Task saveWorkExperienceFail()
    {
        _workExperienceServiceMock.Setup(x => x.Save(_workExperienceDto)).ThrowsAsync(new Exception("unexpected error occurred"));

        var controllerResult = await _controller.SaveWorkExperience(_workExperienceDto);

        var actionResult = Assert.IsType<ObjectResult>(controllerResult);
        Assert.Equal(500, actionResult.StatusCode);

        var problemDetails = actionResult.Value as ProblemDetails;
        
        Assert.NotNull(problemDetails);
        Assert.Equal("Could not save data.", problemDetails.Detail);
    }

    [Fact]
    public async Task saveWorkExperienceAlreadyExists()
    {
        _workExperienceServiceMock.Setup(x => x.Save(_workExperienceDto)).ThrowsAsync(new Exception("work experience exists"));

        var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] {
                                        new Claim(ClaimTypes.NameIdentifier, "TestUser"),
                                        new Claim(ClaimTypes.Email, "test@example.com"),
                                        new Claim(ClaimTypes.Role, "SuperAdmin")
                                   }, "TestAuthentication"));
        _controller.ControllerContext = new ControllerContext();
        _controller.ControllerContext.HttpContext = new DefaultHttpContext { User = user };

        var controllerResult = await _controller.SaveWorkExperience(_workExperienceDto);
        var actionResult = Assert.IsType<BadRequestObjectResult>(controllerResult);

        Assert.Equal(400, actionResult.StatusCode);

        var errorMessage = actionResult.Value as string;

        Assert.NotNull(errorMessage);
        Assert.Equal("work experience exists", errorMessage);
    }

    [Fact]
    public async Task GetWorkExperienceByIdPass()
    {
        var workExperienceList = new List<WorkExperienceDto> { _workExperienceDto };

        _workExperienceServiceMock.Setup(x => x.GetWorkExperienceByEmployeeId(_workExperienceDto.EmployeeId)).ReturnsAsync(workExperienceList);

        var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] {
                                        new Claim(ClaimTypes.NameIdentifier, "TestUser"),
                                        new Claim(ClaimTypes.Email, "test@example.com"),
                                        new Claim(ClaimTypes.Role, "SuperAdmin")
                                   }, "TestAuthentication"));
        _controller.ControllerContext = new ControllerContext();
        _controller.ControllerContext.HttpContext = new DefaultHttpContext { User = user };

        var controllerResult = await _controller.GetWorkExperienceById(_workExperienceDto.EmployeeId);

        var actionResult = Assert.IsType<OkObjectResult>(controllerResult);
        Assert.NotNull(actionResult.Value);
        Assert.Equal(workExperienceList, actionResult.Value);
    }

    [Fact]
    public async Task GetWorkExperienceByIdFail()
    {
        _workExperienceServiceMock.Setup(x => x.GetWorkExperienceByEmployeeId(_workExperienceDto.EmployeeId)).ThrowsAsync(new Exception());

        var controllerResult = await _controller.GetWorkExperienceById(_workExperienceDto.EmployeeId);

        var actionResult = Assert.IsType<NotFoundObjectResult>(controllerResult);
        Assert.Equal(404, actionResult.StatusCode);
    }

    [Fact]
    public async Task UpdateWorkExperiencePass()
    {
        _workExperienceServiceMock.Setup(x => x.Update(_workExperienceDto)).ReturnsAsync(_workExperienceDto);

        var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] {
                                        new Claim(ClaimTypes.NameIdentifier, "TestUser"),
                                        new Claim(ClaimTypes.Email, "test@example.com"),
                                        new Claim(ClaimTypes.Role, "SuperAdmin")
                                   }, "TestAuthentication"));
        _controller.ControllerContext = new ControllerContext();
        _controller.ControllerContext.HttpContext = new DefaultHttpContext { User = user };

        var controllerResult = await _controller.UpdateWorkExperience(_workExperienceDto);

        var actionResult = Assert.IsType<OkObjectResult>(controllerResult);
        Assert.NotNull(actionResult.Value);
        Assert.Equal(200, actionResult.StatusCode);
    }

    [Fact]
    public async Task UpdateWorkExperienceFail()
    {
        _workExperienceServiceMock.Setup(x => x.Update(_workExperienceDto)).ThrowsAsync(new Exception());

        var controllerResult = await _controller.UpdateWorkExperience(_workExperienceDto);

        var actionResult = Assert.IsType<BadRequestObjectResult>(controllerResult);
        Assert.Equal(400, actionResult.StatusCode);
        Assert.Equal("Work experience could not be updated", actionResult.Value);
    }

    [Fact]
    public async Task DeleteWorkExperiencePass()
    {
        _workExperienceServiceMock.Setup(x => x.Delete(_workExperienceDto.Id)).ReturnsAsync(_workExperienceDto);

        var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] {
                                        new Claim(ClaimTypes.NameIdentifier, "TestUser"),
                                        new Claim(ClaimTypes.Email, "test@example.com"),
                                        new Claim(ClaimTypes.Role, "SuperAdmin")
                                   }, "TestAuthentication"));
        _controller.ControllerContext = new ControllerContext();
        _controller.ControllerContext.HttpContext = new DefaultHttpContext { User = user };

        var controllerResult = await _controller.DeleteWorkExperience(_workExperienceDto.Id);

        var actionResult = Assert.IsType<OkResult>(controllerResult);
        Assert.NotNull(actionResult);
        Assert.Equal(200, actionResult.StatusCode);
    }

    [Fact]
    public async Task DeleteWorkExperienceFail()
    {
        _workExperienceServiceMock.Setup(x => x.Delete(_workExperienceDto.Id)).ThrowsAsync(new Exception());

        var controllerResult = await _controller.DeleteWorkExperience(_workExperienceDto.Id);

        var actionResult = Assert.IsType<NotFoundObjectResult>(controllerResult);
        Assert.Equal(404, actionResult.StatusCode);
    }
}