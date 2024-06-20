using HRIS.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RR.App.Controllers.HRIS;
using Xunit;

namespace RR.App.Tests.Controllers.HRIS
{
    public class AuthenticationControllerUnitTests
    {
        private readonly Mock<IAuthService> _authServiceMock;
        private readonly AuthenticationController _controller;

        public AuthenticationControllerUnitTests()
        {
            _authServiceMock = new Mock<IAuthService>();
            _controller = new AuthenticationController(_authServiceMock.Object);
        }

        [Fact]
        public async Task LoggingInUser_ReturnsOkResult()
        {
            var result = await _controller.LoggingInUser();
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Api connection works", okResult.Value);
        }
    }
}
