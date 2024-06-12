using System.Security.Claims;
using HRIS.Models;
using HRIS.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RR.App.Controllers.HRIS;
using RR.Tests.Data.Models.HRIS;
using Xunit;

namespace RR.App.Tests.Controllers.HRIS;

public class AuthenticationControllerUnitTests
{
    private readonly Mock<IAuthService> _authServiceMock;
    private readonly Mock<HttpContext> _httpContextMock;
    private readonly AuthenticationController _controller;
    private readonly EmployeeDto _employeeDto;
    private readonly List<Claim> _claimList;
    private readonly ClaimsIdentity _claimsIdentity;

    public AuthenticationControllerUnitTests()
    {
        _authServiceMock = new Mock<IAuthService>();
        _httpContextMock = new Mock<HttpContext>();
        _controller = new AuthenticationController(_authServiceMock.Object);
        _employeeDto = EmployeeTestData.EmployeeDto;

        _claimList = new List<Claim>
        {
            new(ClaimTypes.Email, "test@retrorabbit.co.za")
        };

        _claimsIdentity = new ClaimsIdentity( _claimList );

        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = _httpContextMock.Object
        };
    }

    [Fact]
    public async Task LoginUserValidInputReturnsOkResult()
    {
        _authServiceMock.Setup(x => x.CheckUserExist("test@retrorabbit.co.za"))
            .ReturnsAsync(true);

        _authServiceMock.Setup(x => x.Login("test@retrorabbit.co.za"))
            .ReturnsAsync("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789");

        var result = await _controller.LoginUser("test@retrorabbit.co.za");

        var okResult = Assert.IsType<OkObjectResult>(result);
        var actualToken = Assert.IsType<string>(okResult.Value);
        Assert.Equal("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789", actualToken);
        _authServiceMock.Verify(x => x.CheckUserExist("test@retrorabbit.co.za"), Times.Once);
        _authServiceMock.Verify(x => x.Login("test@retrorabbit.co.za"), Times.Once);
    }

    [Fact]
    public async Task LoginUserUserNotFoundReturnsNotFoundResult()
    {
        _authServiceMock.Setup(x => x.CheckUserExist("nonexistent@retrorabbit.co.za"))
            .ReturnsAsync(false);

        var result = await _controller.LoginUser("nonexistent@retrorabbit.co.za");

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("User not found", notFoundResult.Value);
    }

    [Fact]
    public async Task RegisterEmployeeValidInputReturnsCreatedAtActionResult()
    {
        _authServiceMock.Setup(x => x.CheckUserExist(_employeeDto.Email!))
            .ReturnsAsync(false);

        _authServiceMock.Setup(x => x.RegisterEmployee(_employeeDto))
            .ReturnsAsync("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789");

        var result = await _controller.RegisterEmployee(_employeeDto);

        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal(nameof(AuthenticationController.RegisterEmployee), createdAtActionResult.ActionName);
        Assert.Equal(_employeeDto, createdAtActionResult.Value);
    }

    [Fact]
    public async Task RegisterEmployeeEmployeeAlreadyExistsReturnsNotFoundResult()
    {
        _authServiceMock.Setup(x => x.CheckUserExist(_employeeDto.Email!))
            .ReturnsAsync(true);

        var result = await _controller.RegisterEmployee(_employeeDto);

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("Employee already exists", notFoundResult.Value);
    }

    [Fact]
    public async Task GetUserRolesSuccessReturnsOkResult()
    {
        _httpContextMock.SetupGet(c => c.User.Identity)
            .Returns(_claimsIdentity);

        _authServiceMock.Setup(a => a.GetUserRoles(It.IsAny<string>()))
            .ReturnsAsync(new Dictionary<string, List<string>>());

        var result = await _controller.GetUserRoles(null);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var roles = Assert.IsType<Dictionary<string, List<string>>>(okResult.Value);
        _authServiceMock.Verify(a => a.GetUserRoles("test@retrorabbit.co.za"), Times.Once);
    }

    [Fact]
    public async Task GetUserRoles_ReturnsOkResult_WithEmail()
    {
        _httpContextMock.SetupGet(c => c.User.Identity)
            .Returns(_claimsIdentity);

        _authServiceMock.Setup(a => a.GetUserRoles("test@retrorabbit.co.za"))
            .ReturnsAsync(new Dictionary<string, List<string>>());

        var result = await _controller.GetUserRoles(null);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var roles = Assert.IsType<Dictionary<string, List<string>>>(okResult.Value);
        _authServiceMock.Verify(a => a.GetUserRoles("test@retrorabbit.co.za"), Times.Once);
    }

    [Fact]
    public async Task GetUserRoles_ReturnsOkResult_WithoutEmail()
    {
        _httpContextMock.SetupGet(c => c.User.Identity)
            .Returns(_claimsIdentity);

        _authServiceMock.Setup(a => a.GetUserRoles("test@retrorabbit.co.za"))
            .ReturnsAsync(new Dictionary<string, List<string>>());

        var result = await _controller.GetUserRoles(null);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var roles = Assert.IsType<Dictionary<string, List<string>>>(okResult.Value);
        _authServiceMock.Verify(a => a.GetUserRoles("test@retrorabbit.co.za"), Times.Once);
    }

    [Fact]
    public async Task GetUserRolesReturnsNotFoundResult()
    {
        _authServiceMock.Setup(x => x.GetUserRoles(It.IsAny<string>()))
            .ThrowsAsync(new Exception("Exception occurred while getting user roles"));

        var identity = new ClaimsIdentity(_claimList, "TestAuthentication");
        var claimsPrincipal = new ClaimsPrincipal(identity);

        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = claimsPrincipal }
        };

        var result = await _controller.GetUserRoles("test@retrorabbit.co.za");

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.NotNull(notFoundResult.Value);
        Assert.IsType<string>(notFoundResult.Value);
        Assert.Equal("Exception occurred while getting user roles", notFoundResult.Value.ToString());
    }
}