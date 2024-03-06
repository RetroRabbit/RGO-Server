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

public class ApplicantControllerUnitTest
{
    private readonly Mock<IApplicantService> _mockApplicantService;
    private readonly ApplicantController _controller;
    private readonly Mock<IUnitOfWork> _dbMock;

    public ApplicantControllerUnitTest()
    {
        _dbMock = new Mock<IUnitOfWork>();
        _mockApplicantService = new Mock<IApplicantService>();
        _controller = new ApplicantController(_mockApplicantService.Object);
    }

    [Fact]
    public async Task AddAppliantPass()
    {
        _mockApplicantService
            .Setup(service => service
                .SaveApplicant(ApplicantDtoTestData.ApplicantDto))
            .ReturnsAsync(ApplicantDtoTestData.ApplicantDto);

        var controllerResult = await _controller.AddApplicant(ApplicantDtoTestData.ApplicantDto);
        var actionResult = Assert.IsType<OkObjectResult>(controllerResult);
        ApplicantDto? newApplicant = actionResult.Value as ApplicantDto;
        Assert.NotNull(newApplicant);
        Assert.Equal(ApplicantDtoTestData.ApplicantDto, newApplicant);
    }

    [Fact]
    public async Task AddApplicantFailUserExists()
    {
        _mockApplicantService
           .Setup(service => service
               .SaveApplicant(ApplicantDtoTestData.ApplicantDto))
           .ThrowsAsync(new Exception("User Exists"));

        var controllerResult = await _controller
            .AddApplicant(ApplicantDtoTestData.ApplicantDto);
        var objectResult = Assert.IsType<ConflictObjectResult>(controllerResult);
        Assert.Equal(objectResult.StatusCode, 409);
        Assert.Equal(objectResult.Value, "User Exists");
    }
    
    [Fact]
    public async Task AddApplicantFailUserInternalError()
    {
        _mockApplicantService
           .Setup(service => service
               .SaveApplicant(ApplicantDtoTestData.ApplicantDto))
           .ThrowsAsync(new Exception());

        var controllerResult = await _controller
            .AddApplicant(ApplicantDtoTestData.ApplicantDto);

        var objectResult = Assert.IsType<BadRequestObjectResult>(controllerResult);
        Assert.Equal(objectResult.StatusCode, 400);
    }

    [Fact]
    public async Task GetAllApplicantsPass()
    {
        _mockApplicantService
            .Setup(service => service.GetAllApplicants())
        .ReturnsAsync(new List<ApplicantDto> { 
            ApplicantDtoTestData.ApplicantDto,
            ApplicantDtoTestData.ApplicantDtoTwo
        });

        var controllerResult = await _controller.GetAll();
        var actionResult = Assert.IsType<OkObjectResult>(controllerResult);
        List<ApplicantDto>? fetchedApplicants = actionResult.Value as List<ApplicantDto>;
        Assert.NotNull(fetchedApplicants);
        Assert.Equal(2, fetchedApplicants.Count);
    }
}

