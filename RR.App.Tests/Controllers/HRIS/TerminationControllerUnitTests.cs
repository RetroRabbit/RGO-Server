using HRIS.Models;
using HRIS.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RR.App.Controllers.HRIS;
using RR.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace RR.App.Tests.Controllers.HRIS;

public class TerminationControllerUnitTests
{
    private readonly Mock<ITerminationService> _terminationServiceMock;
    private readonly TerminationController _controller;
    private readonly Mock<IUnitOfWork> _db;

    public TerminationControllerUnitTests()
    {
        _terminationServiceMock = new Mock<ITerminationService>();
        _controller = new TerminationController(_terminationServiceMock.Object);
    }

    TerminationDto terminationDto = new TerminationDto
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

    [Fact]
    public async Task saveTerminationReturnsOk()
    {
        _terminationServiceMock
           .Setup(x => x
               .SaveTermination(terminationDto))
           .ReturnsAsync(terminationDto);

        var controllerResult = await _controller.AddTermination(terminationDto);
        var actionResult = Assert.IsType<CreatedAtActionResult>(controllerResult);

        TerminationDto newTermination = actionResult.Value as TerminationDto;

        Assert.NotNull(newTermination);
        Assert.Equal(terminationDto, newTermination);
    }

    [Fact]
    public async Task saveTerminationReturnsBadRequest()
    {
        _terminationServiceMock
           .Setup(x => x
               .SaveTermination(terminationDto))
           .ThrowsAsync(new Exception("exists"));

        var controllerResult = await _controller.AddTermination(terminationDto);

        var actionResult = Assert.IsType<BadRequestObjectResult>(controllerResult);
    }

    [Fact]
    public async Task saveTerminationReturnsProblem()
    {
        _terminationServiceMock
           .Setup(x => x
               .SaveTermination(terminationDto))
           .ThrowsAsync(new Exception());

        var controllerResult = await _controller.AddTermination(terminationDto);

        var actionResult = Assert.IsType<ObjectResult>(controllerResult);
    }


}
