using HRIS.Models;
using HRIS.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RR.App.Controllers.HRIS;
using Xunit;

namespace RR.App.Tests.Controllers.HRIS;

public class TerminationControllerUnitTests
{
    private readonly Mock<ITerminationService> _terminationServiceMock;
    private readonly TerminationController _controller;
    private readonly TerminationDto _terminationDto;

    public TerminationControllerUnitTests()
    {
        _terminationServiceMock = new Mock<ITerminationService>();
        _controller = new TerminationController(_terminationServiceMock.Object);

        _terminationDto = new TerminationDto
        {
            Id = 1,
            EmployeeId = 1,
            TerminationOption = 0,
            DayOfNotice = new DateTime(),
            LastDayOfEmployment = new DateTime(),
            ReemploymentStatus = false,
            EquipmentStatus = true,
            AccountsStatus = true,
            TerminationDocument = "document",
            DocumentName = "document name",
            TerminationComments = "termination comment",
        };
    }

    [Fact]
    public async Task saveTerminationReturnsOk()
    {
        _terminationServiceMock.Setup(x => x.SaveTermination(_terminationDto)).ReturnsAsync(_terminationDto);

        var controllerResult = await _controller.AddTermination(_terminationDto);

        var actionResult = Assert.IsType<CreatedAtActionResult>(controllerResult);
        Assert.NotNull(actionResult.Value);
        Assert.Equal(_terminationDto, actionResult.Value);
    }

    [Fact]
    public async Task saveTerminationReturnsBadRequest()
    {
        _terminationServiceMock.Setup(x => x.SaveTermination(_terminationDto)).ThrowsAsync(new Exception("exists"));

        var controllerResult = await _controller.AddTermination(_terminationDto);

        var actionResult = Assert.IsType<BadRequestObjectResult>(controllerResult);
    }

    [Fact]
    public async Task saveTerminationReturnsProblem()
    {
        _terminationServiceMock.Setup(x => x.SaveTermination(_terminationDto)).ThrowsAsync(new Exception());

        var controllerResult = await _controller.AddTermination(_terminationDto);

        var actionResult = Assert.IsType<ObjectResult>(controllerResult);
    }

    [Fact]
    public async Task GetTerminationByEmployeeIdPass()
    {
        _terminationServiceMock.Setup(x => x.GetTerminationByEmployeeId(_terminationDto.EmployeeId)).ReturnsAsync(_terminationDto);

        var controllerResult = await _controller.GetTerminationByEmployeeId(_terminationDto.EmployeeId);

        var actionResult = Assert.IsType<OkObjectResult>(controllerResult);
        Assert.NotNull(actionResult.Value);
        Assert.Equal(_terminationDto, actionResult.Value);
    }

    [Fact]
    public async Task GetTerminationByEmployeeIdReturnsNotFound()
    {
        _terminationServiceMock.Setup(x => x.GetTerminationByEmployeeId(_terminationDto.EmployeeId)).ThrowsAsync(new Exception());

        var controllerResult = await _controller.GetTerminationByEmployeeId(_terminationDto.EmployeeId);

        var actionResult = Assert.IsType<NotFoundObjectResult>(controllerResult);
    }
}