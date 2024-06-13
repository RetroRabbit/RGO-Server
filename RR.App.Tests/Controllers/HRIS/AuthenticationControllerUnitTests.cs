using Microsoft.AspNetCore.Mvc;
using RR.App.Controllers.HRIS;
using Xunit;

namespace RR.App.Tests.Controllers.HRIS;

public class AuthenticationControllerUnitTests
{
    private readonly AuthenticationController _controller;

    public AuthenticationControllerUnitTests()
    {
        _controller = new AuthenticationController();
    }

    [Fact]
    public async Task LoggingInUser_ReturnsOkResult()
    {
        var result = await _controller.LoggingInUser();
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal("Server online", okResult.Value);
    }
}