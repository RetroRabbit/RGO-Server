using ATS.Models;
using ATS.Services.Interfaces;
using HRIS.Models;
using HRIS.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RR.App.Controllers.ATS;
using RR.App.Controllers.HRIS;
using RR.Tests.Data.Models.ATS;
using RR.Tests.Data.Models.HRIS;
using RR.UnitOfWork;
using Xunit;

namespace RR.App.Tests.Controllers.HRIS;

public class WorkExperienceControllerUnitTest
{
    private readonly Mock<IWorkExperienceService> _workExperienceServiceMock;
    private readonly WorkExperienceController _controller;
    private readonly Mock<IUnitOfWork> _dbMock;

    public WorkExperienceControllerUnitTest()
    {
        _workExperienceServiceMock = new Mock<IWorkExperienceService>();
        _controller = new WorkExperienceController(_workExperienceServiceMock.Object);
    }

    WorkExperienceDto workExperience = new WorkExperienceDto
    {
        Id = 1,
        Title = "Senior Developer",
        EmploymentType = "Permanent",
        CompanyName = "Retro Rabbit",
        Location = "Pretoria",
        EmployeeId = 1,
        StartDate = new DateOnly(2022, 1, 1),
        EndDate = new DateOnly(2024, 1, 1),
    };

    [Fact]
    public async Task saveWorkExperienceReturnsOk()
    {
        _workExperienceServiceMock
           .Setup(x => x
               .Save(workExperience))
           .ReturnsAsync(workExperience);

        var controllerResult = await _controller.SaveWorkExperience(workExperience);
        var actionResult = Assert.IsType<CreatedAtActionResult>(controllerResult);

        WorkExperienceDto? newWorkExperience = actionResult.Value as WorkExperienceDto;

        Assert.NotNull(newWorkExperience);
        Assert.Equal(workExperience, newWorkExperience);
    }

    [Fact]
    public async Task saveWorkExperienceFail()
    {
        _workExperienceServiceMock
           .Setup(x => x.Save(workExperience))
           .ThrowsAsync(new Exception("unexpected error occurred"));

        var controllerResult = await _controller.SaveWorkExperience(workExperience);

        var actionResult = Assert.IsType<ObjectResult>(controllerResult);
        Assert.Equal(500, actionResult.StatusCode);

        var problemDetails = actionResult.Value as ProblemDetails;
        Assert.NotNull(problemDetails);
        Assert.Equal("Could not save data.", problemDetails.Detail);
    }

    [Fact]
    public async Task saveWorkExperienceAlreadyExists()
    {
        _workExperienceServiceMock
            .Setup(x => x.Save(workExperience))
            .ThrowsAsync(new Exception("work experience exists"));

        var controllerResult = await _controller.SaveWorkExperience(workExperience);

        var actionResult = Assert.IsType<BadRequestObjectResult>(controllerResult);
        Assert.Equal(400, actionResult.StatusCode);

        var errorMessage = actionResult.Value as string;
        Assert.NotNull(errorMessage);
        Assert.Equal("work experience exists", errorMessage);
    }

    [Fact]
    public async Task GetWorkExperienceByIdPass()
    {
        _workExperienceServiceMock
           .Setup(x => x
               .GetWorkExperienceByEmployeeId(workExperience.Id))
           .ReturnsAsync(workExperience);

        var controllerResult = await _controller
            .GetWorkExperienceById(workExperience.Id);

        var actionResult = Assert.IsType<OkObjectResult>(controllerResult);

        Assert.NotNull(actionResult.Value);
        Assert.Equal(workExperience, actionResult.Value);
    }

    [Fact]
    public async Task GetWorkExperienceByIdFail()
    {
        _workExperienceServiceMock
           .Setup(x => x
               .GetWorkExperienceByEmployeeId(workExperience.Id))
           .ThrowsAsync(new Exception());

        var controllerResult = await _controller
            .GetWorkExperienceById(workExperience.Id);

        var actionResult = Assert.IsType<NotFoundObjectResult>(controllerResult);

        Assert.Equal(404, actionResult.StatusCode);
    }

    [Fact]
    public async Task UpdateWorkExperiencePass()
    {
        _workExperienceServiceMock
           .Setup(x => x
                .Update(workExperience))
           .ReturnsAsync(workExperience);

        var controllerResult = await _controller
            .UpdateWorkExperience(workExperience);

        var actionResult = Assert.IsType<OkObjectResult>(controllerResult);

        Assert.NotNull(actionResult.Value);
        Assert.Equal(200, actionResult.StatusCode);
    }

    [Fact]
    public async Task UpdateWorkExperienceFail()
    {
        _workExperienceServiceMock
           .Setup(x => x
                .Update(workExperience))
           .ThrowsAsync(new Exception());

        var controllerResult = await _controller
            .UpdateWorkExperience(workExperience);

        var actionResult = Assert.IsType<BadRequestObjectResult>(controllerResult);


        Assert.Equal(400, actionResult.StatusCode);
        Assert.Equal("Work experience could not be updated", actionResult.Value);
    }

    [Fact]
    public async Task DeleteWorkExperiencePass()
    {
        _workExperienceServiceMock
           .Setup(x => x
                .Delete(workExperience.Id))
           .ReturnsAsync(workExperience);

        var controllerResult = await _controller
            .DeleteWorkExperience(workExperience.Id);

        var actionResult = Assert.IsType<OkResult>(controllerResult);


        Assert.NotNull(actionResult);
        Assert.Equal(200, actionResult.StatusCode);
    }

    [Fact]
    public async Task DeleteWorkExperienceFail()
    {
        _workExperienceServiceMock.Setup(x => x.Delete(workExperience.Id))
            .ThrowsAsync(new Exception());

        var controllerResult = await _controller
            .DeleteWorkExperience(workExperience.Id);

        var actionResult = Assert.IsType<NotFoundObjectResult>(controllerResult);

        Assert.Equal(404, actionResult.StatusCode);
    }
}
