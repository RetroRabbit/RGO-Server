﻿using ATS.Models;
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

    [Fact]
    public async Task GetAllApplicantsFail()
    {
        _mockApplicantService
            .Setup(service => service.GetAllApplicants())
        .ThrowsAsync(new Exception());

        var controllerResult = await _controller.GetAll();

        var actionResult = Assert.IsType<BadRequestObjectResult>(controllerResult);

        Assert.Equal(400, actionResult.StatusCode);
    }

    [Fact]
    public async Task GetApplicantByIdPass()
    {
        _mockApplicantService
           .Setup(service => service
               .GetApplicantById(ApplicantDtoTestData.ApplicantDto.Id))
           .ReturnsAsync(ApplicantDtoTestData.ApplicantDto);
        
        var controllerResult = await _controller
            .GetById(ApplicantDtoTestData.ApplicantDto.Id);

        var actionResult = Assert.IsType<OkObjectResult>(controllerResult);

        Assert.NotNull(actionResult.Value);
        Assert.Equal(ApplicantDtoTestData.ApplicantDto, actionResult.Value);
    }

    [Fact]
    public async Task GetApplicantByIdFailNotFound()
    {
        _mockApplicantService
           .Setup(service => service
               .GetApplicantById(ApplicantDtoTestData.ApplicantDto.Id))
           .ThrowsAsync(new Exception());

        var controllerResult = await _controller
            .GetById(ApplicantDtoTestData.ApplicantDto.Id);

        var actionResult = Assert.IsType<NotFoundObjectResult>(controllerResult);

        Assert.Equal(404, actionResult.StatusCode);
    }
    
    [Fact]
    public async Task GetApplicantByEmailPass()
    {
        _mockApplicantService
           .Setup(service => service
               .GetApplicantByEmail(ApplicantDtoTestData.ApplicantDto.PersonalEmail))
           .ReturnsAsync(ApplicantDtoTestData.ApplicantDto);
        
        var controllerResult = await _controller
            .GetByEmail(ApplicantDtoTestData.ApplicantDto.PersonalEmail);

        var actionResult = Assert.IsType<OkObjectResult>(controllerResult);

        Assert.NotNull(actionResult.Value);
        Assert.Equal(ApplicantDtoTestData.ApplicantDto, actionResult.Value);
    }

    [Fact]
    public async Task GetApplicantByIdEmaiFaillNotFound()
    {
        _mockApplicantService
           .Setup(service => service
               .GetApplicantByEmail(ApplicantDtoTestData.ApplicantDto.PersonalEmail))
           .ThrowsAsync(new Exception());

        var controllerResult = await _controller
            .GetByEmail(ApplicantDtoTestData.ApplicantDto.PersonalEmail);

        var actionResult = Assert.IsType<NotFoundObjectResult>(controllerResult);

        Assert.Equal(404, actionResult.StatusCode);
    }

    [Fact]
    public async Task UpdateApplicantPass()
    {
        _mockApplicantService
           .Setup(service => service
                .UpdateApplicant(ApplicantDtoTestData.ApplicantDto))
           .ReturnsAsync(ApplicantDtoTestData.ApplicantDtoTwo);

        var controllerResult = await _controller
            .UpdateApplicant(ApplicantDtoTestData.ApplicantDto);

        var actionResult = Assert.IsType<OkObjectResult>(controllerResult);

        Assert.NotNull(actionResult.Value);
        Assert.Equal(ApplicantDtoTestData.ApplicantDtoTwo, actionResult.Value);
    }

    [Fact]
    public async Task UpdateApplicantFail()
    {
        _mockApplicantService
           .Setup(service => service
               .UpdateApplicant(ApplicantDtoTestData.ApplicantDto))
           .ThrowsAsync(new Exception());

        var controllerResult = await _controller
            .UpdateApplicant(ApplicantDtoTestData.ApplicantDto);

        var actionResult = Assert.IsType<BadRequestObjectResult>(controllerResult);

        Assert.Equal(400, actionResult.StatusCode);
    }

    [Fact]
    public async Task DeleteApplicantPass()
    {
        _mockApplicantService
           .Setup(service => service
                .DeleteApplicant(ApplicantDtoTestData.ApplicantDto.Id))
           .ReturnsAsync(ApplicantDtoTestData.ApplicantDto);

        var controllerResult = await _controller
            .DeleteApplicant(ApplicantDtoTestData.ApplicantDto.Id);

        var actionResult = Assert.IsType<OkObjectResult>(controllerResult);

        Assert.NotNull(actionResult.Value);
        Assert.Equal(ApplicantDtoTestData.ApplicantDto, actionResult.Value);
    }

    [Fact]
    public async Task DeleteApplicantFail()
    {
        _mockApplicantService
            .Setup(service =>
                service.DeleteApplicant(ApplicantDtoTestData.ApplicantDto.Id))
            .ThrowsAsync(new Exception());

        var controllerResult = await _controller
            .DeleteApplicant(ApplicantDtoTestData.ApplicantDto.Id);

        var actionResult = Assert.IsType<BadRequestObjectResult>(controllerResult);

        Assert.Equal(400, actionResult.StatusCode);
            
    }
}

