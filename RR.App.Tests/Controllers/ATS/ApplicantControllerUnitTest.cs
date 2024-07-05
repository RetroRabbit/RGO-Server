using ATS.Models;
using ATS.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RR.App.Controllers.ATS;
using RR.Tests.Data.Models.ATS;
using RR.UnitOfWork;
using Xunit;

namespace RR.App.Tests.Controllers.ATS;

public class CandidateControllerUnitTest
{
    private readonly Mock<ICandidateService> _mockCandidateService;
    private readonly CandidateController _controller;
    private readonly Mock<IUnitOfWork> _dbMock;

    public CandidateControllerUnitTest()
    {
        _dbMock = new Mock<IUnitOfWork>();
        _mockCandidateService = new Mock<ICandidateService>();
        _controller = new CandidateController(_mockCandidateService.Object);
    }

    [Fact]
    public async Task AddApplicantPass()
    {
        _mockCandidateService
            .Setup(service => service
                .SaveCandidate(It.IsAny<CandidateDto>()))
            .ReturnsAsync(CandidateTestData.CandidateOne.ToDto());

        var controllerResult = await _controller.AddCandidate(CandidateTestData.CandidateOne.ToDto());
        var actionResult = Assert.IsType<OkObjectResult>(controllerResult);

        var newCandidate = actionResult.Value as CandidateDto;

        Assert.NotNull(newCandidate);
        Assert.Equivalent(CandidateTestData.CandidateOne.ToDto(), newCandidate);
    }

    [Fact]
    public async Task AddCandidateFailUserExists()
    {
        _mockCandidateService
           .Setup(service => service
               .SaveCandidate(It.IsAny<CandidateDto>()))
           .ThrowsAsync(new Exception("User Exists"));

        var controllerResult = await _controller
            .AddCandidate(CandidateTestData.CandidateOne.ToDto());

        var objectResult = Assert.IsType<ConflictObjectResult>(controllerResult);

        Assert.Equal(objectResult.StatusCode, 409);
        Assert.Equal(objectResult.Value, "User Exists");
    }

    [Fact]
    public async Task AddCandidateFailUserInternalError()
    {
        _mockCandidateService
           .Setup(service => service
               .SaveCandidate(It.IsAny<CandidateDto>()))
           .ThrowsAsync(new Exception());

        var controllerResult = await _controller
            .AddCandidate(CandidateTestData.CandidateOne.ToDto());

        var objectResult = Assert.IsType<BadRequestObjectResult>(controllerResult);

        Assert.Equal(objectResult.StatusCode, 400);
    }

    [Fact]
    public async Task GetAllCandidatesPass()
    {
        _mockCandidateService
            .Setup(service => service.GetAllCandidates())
        .ReturnsAsync(new List<CandidateDto> {
            CandidateTestData.CandidateOne.ToDto(),
            CandidateTestData.CandidateTwo.ToDto()
        });

        var controllerResult = await _controller.GetAll();
        var actionResult = Assert.IsType<OkObjectResult>(controllerResult);

        var fetchedCandidates = actionResult.Value as List<CandidateDto>;

        Assert.NotNull(fetchedCandidates);
        Assert.Equal(2, fetchedCandidates.Count);
    }

    [Fact]
    public async Task GetAllCandidatesFail()
    {
        _mockCandidateService
            .Setup(service => service.GetAllCandidates())
        .ThrowsAsync(new Exception());

        var controllerResult = await _controller.GetAll();

        var actionResult = Assert.IsType<BadRequestObjectResult>(controllerResult);

        Assert.Equal(400, actionResult.StatusCode);
    }

    [Fact]
    public async Task GetCandidateByIdPass()
    {
        _mockCandidateService
           .Setup(service => service
               .GetCandidateById(It.IsAny<int>()))
           .ReturnsAsync(CandidateTestData.CandidateOne.ToDto());

        var controllerResult = await _controller
            .GetById(CandidateTestData.CandidateOne.Id);

        var actionResult = Assert.IsType<OkObjectResult>(controllerResult);

        Assert.NotNull(actionResult.Value);
        Assert.Equivalent(CandidateTestData.CandidateOne, actionResult.Value);
    }

    [Fact]
    public async Task GetCandidateByIdFailNotFound()
    {
        _mockCandidateService
           .Setup(service => service
               .GetCandidateById(It.IsAny<int>()))
           .ThrowsAsync(new Exception());

        var controllerResult = await _controller
            .GetById(CandidateTestData.CandidateOne.Id);

        var actionResult = Assert.IsType<NotFoundObjectResult>(controllerResult);

        Assert.Equal(404, actionResult.StatusCode);
    }

    [Fact]
    public async Task GetCandidateByEmailPass()
    {
        _mockCandidateService
           .Setup(service => service
               .GetCandidateByEmail(It.IsAny<string>()))
           .ReturnsAsync(CandidateTestData.CandidateOne.ToDto());

        var controllerResult = await _controller
            .GetByEmail(CandidateTestData.CandidateOne.PersonalEmail);

        var actionResult = Assert.IsType<OkObjectResult>(controllerResult);

        Assert.NotNull(actionResult.Value);
        Assert.Equivalent(CandidateTestData.CandidateOne, actionResult.Value);
    }

    [Fact]
    public async Task GetCandidateByIdEmaiFaillNotFound()
    {
        _mockCandidateService
           .Setup(service => service
               .GetCandidateByEmail(It.IsAny<string>()))
           .ThrowsAsync(new Exception());

        var controllerResult = await _controller
            .GetByEmail(CandidateTestData.CandidateOne.PersonalEmail);

        var actionResult = Assert.IsType<NotFoundObjectResult>(controllerResult);

        Assert.Equal(404, actionResult.StatusCode);
    }

    [Fact]
    public async Task UpdateCandidatePass()
    {
        _mockCandidateService
           .Setup(service => service
                .UpdateCandidate(It.IsAny<CandidateDto>()))
           .ReturnsAsync(CandidateTestData.CandidateTwo.ToDto());

        var controllerResult = await _controller
            .UpdateCandidate(CandidateTestData.CandidateOne.ToDto());

        var actionResult = Assert.IsType<OkObjectResult>(controllerResult);

        Assert.NotNull(actionResult.Value);
        Assert.Equivalent(CandidateTestData.CandidateTwo, actionResult.Value);
    }

    [Fact]
    public async Task UpdateCandidateFail()
    {
        _mockCandidateService
           .Setup(service => service
               .UpdateCandidate(It.IsAny<CandidateDto>()))
           .ThrowsAsync(new Exception());

        var controllerResult = await _controller
            .UpdateCandidate(CandidateTestData.CandidateOne.ToDto());

        var actionResult = Assert.IsType<BadRequestObjectResult>(controllerResult);

        Assert.Equal(400, actionResult.StatusCode);
    }

    [Fact]
    public async Task DeleteCandidatePass()
    {
        _mockCandidateService
           .Setup(service => service
                .DeleteCandidate(It.IsAny<int>()))
           .ReturnsAsync(CandidateTestData.CandidateOne.ToDto());

        var controllerResult = await _controller
            .DeleteCandidate(CandidateTestData.CandidateOne.Id);

        var actionResult = Assert.IsType<OkObjectResult>(controllerResult);

        Assert.NotNull(actionResult.Value);
        Assert.Equivalent(CandidateTestData.CandidateOne, actionResult.Value);
    }

    [Fact]
    public async Task DeleteCandidateFail()
    {
        _mockCandidateService
            .Setup(service =>
                service.DeleteCandidate(It.IsAny<int>()))
            .ThrowsAsync(new Exception());

        var controllerResult = await _controller
            .DeleteCandidate(CandidateTestData.CandidateOne.Id);

        var actionResult = Assert.IsType<BadRequestObjectResult>(controllerResult);

        Assert.Equal(400, actionResult.StatusCode);

    }
}

