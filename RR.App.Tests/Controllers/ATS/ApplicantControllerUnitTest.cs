using ATS.Models;
using ATS.Services.Interfaces;
using Castle.Components.DictionaryAdapter.Xml;
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
    public async Task AddAppliantPass()
    {
        _mockCandidateService
            .Setup(service => service
                .SaveCandidate(CandidateDtoTestData.CandidateDto))
            .ReturnsAsync(CandidateDtoTestData.CandidateDto);

        var controllerResult = await _controller.AddCandidate(CandidateDtoTestData.CandidateDto);
        var actionResult = Assert.IsType<OkObjectResult>(controllerResult);

        CandidateDto? newCandidate = actionResult.Value as CandidateDto;

        Assert.NotNull(newCandidate);
        Assert.Equal(CandidateDtoTestData.CandidateDto, newCandidate);
    }

    [Fact]
    public async Task AddCandidateFailUserExists()
    {
        _mockCandidateService
           .Setup(service => service
               .SaveCandidate(CandidateDtoTestData.CandidateDto))
           .ThrowsAsync(new Exception("User Exists"));

        var controllerResult = await _controller
            .AddCandidate(CandidateDtoTestData.CandidateDto);

        var objectResult = Assert.IsType<ConflictObjectResult>(controllerResult);

        Assert.Equal(objectResult.StatusCode, 409);
        Assert.Equal(objectResult.Value, "User Exists");
    }
    
    [Fact]
    public async Task AddCandidateFailUserInternalError()
    {
        _mockCandidateService
           .Setup(service => service
               .SaveCandidate(CandidateDtoTestData.CandidateDto))
           .ThrowsAsync(new Exception());

        var controllerResult = await _controller
            .AddCandidate(CandidateDtoTestData.CandidateDto);

        var objectResult = Assert.IsType<BadRequestObjectResult>(controllerResult);

        Assert.Equal(objectResult.StatusCode, 400);
    }

    [Fact]
    public async Task GetAllCandidatesPass()
    {
        _mockCandidateService
            .Setup(service => service.GetAllCandidates())
        .ReturnsAsync(new List<CandidateDto> { 
            CandidateDtoTestData.CandidateDto,
            CandidateDtoTestData.CandidateDtoTwo
        });

        var controllerResult = await _controller.GetAll();
        var actionResult = Assert.IsType<OkObjectResult>(controllerResult);

        List<CandidateDto>? fetchedCandidates = actionResult.Value as List<CandidateDto>;

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
               .GetCandidateById(CandidateDtoTestData.CandidateDto.Id))
           .ReturnsAsync(CandidateDtoTestData.CandidateDto);
        
        var controllerResult = await _controller
            .GetById(CandidateDtoTestData.CandidateDto.Id);

        var actionResult = Assert.IsType<OkObjectResult>(controllerResult);

        Assert.NotNull(actionResult.Value);
        Assert.Equal(CandidateDtoTestData.CandidateDto, actionResult.Value);
    }

    [Fact]
    public async Task GetCandidateByIdFailNotFound()
    {
        _mockCandidateService
           .Setup(service => service
               .GetCandidateById(CandidateDtoTestData.CandidateDto.Id))
           .ThrowsAsync(new Exception());

        var controllerResult = await _controller
            .GetById(CandidateDtoTestData.CandidateDto.Id);

        var actionResult = Assert.IsType<NotFoundObjectResult>(controllerResult);

        Assert.Equal(404, actionResult.StatusCode);
    }
    
    [Fact]
    public async Task GetCandidateByEmailPass()
    {
        _mockCandidateService
           .Setup(service => service
               .GetCandidateByEmail(CandidateDtoTestData.CandidateDto.PersonalEmail))
           .ReturnsAsync(CandidateDtoTestData.CandidateDto);
        
        var controllerResult = await _controller
            .GetByEmail(CandidateDtoTestData.CandidateDto.PersonalEmail);

        var actionResult = Assert.IsType<OkObjectResult>(controllerResult);

        Assert.NotNull(actionResult.Value);
        Assert.Equal(CandidateDtoTestData.CandidateDto, actionResult.Value);
    }

    [Fact]
    public async Task GetCandidateByIdEmaiFaillNotFound()
    {
        _mockCandidateService
           .Setup(service => service
               .GetCandidateByEmail(CandidateDtoTestData.CandidateDto.PersonalEmail))
           .ThrowsAsync(new Exception());

        var controllerResult = await _controller
            .GetByEmail(CandidateDtoTestData.CandidateDto.PersonalEmail);

        var actionResult = Assert.IsType<NotFoundObjectResult>(controllerResult);

        Assert.Equal(404, actionResult.StatusCode);
    }

    [Fact]
    public async Task UpdateCandidatePass()
    {
        _mockCandidateService
           .Setup(service => service
                .UpdateCandidate(CandidateDtoTestData.CandidateDto))
           .ReturnsAsync(CandidateDtoTestData.CandidateDtoTwo);

        var controllerResult = await _controller
            .UpdateCandidate(CandidateDtoTestData.CandidateDto);

        var actionResult = Assert.IsType<OkObjectResult>(controllerResult);

        Assert.NotNull(actionResult.Value);
        Assert.Equal(CandidateDtoTestData.CandidateDtoTwo, actionResult.Value);
    }

    [Fact]
    public async Task UpdateCandidateFail()
    {
        _mockCandidateService
           .Setup(service => service
               .UpdateCandidate(CandidateDtoTestData.CandidateDto))
           .ThrowsAsync(new Exception());

        var controllerResult = await _controller
            .UpdateCandidate(CandidateDtoTestData.CandidateDto);

        var actionResult = Assert.IsType<BadRequestObjectResult>(controllerResult);

        Assert.Equal(400, actionResult.StatusCode);
    }

    [Fact]
    public async Task DeleteCandidatePass()
    {
        _mockCandidateService
           .Setup(service => service
                .DeleteCandidate(CandidateDtoTestData.CandidateDto.Id))
           .ReturnsAsync(CandidateDtoTestData.CandidateDto);

        var controllerResult = await _controller
            .DeleteCandidate(CandidateDtoTestData.CandidateDto.Id);

        var actionResult = Assert.IsType<OkObjectResult>(controllerResult);

        Assert.NotNull(actionResult.Value);
        Assert.Equal(CandidateDtoTestData.CandidateDto, actionResult.Value);
    }

    [Fact]
    public async Task DeleteCandidateFail()
    {
        _mockCandidateService
            .Setup(service =>
                service.DeleteCandidate(CandidateDtoTestData.CandidateDto.Id))
            .ThrowsAsync(new Exception());

        var controllerResult = await _controller
            .DeleteCandidate(CandidateDtoTestData.CandidateDto.Id);

        var actionResult = Assert.IsType<BadRequestObjectResult>(controllerResult);

        Assert.Equal(400, actionResult.StatusCode);
            
    }
}

