using System.Security.Claims;
using HRIS.Models;
using HRIS.Models.Enums;
using HRIS.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RR.App.Controllers.HRIS;
using Xunit;

namespace RR.App.Tests.Controllers.HRIS;

public class AuthenticationControllerUnitTests
{
    [Fact]
    public async Task LoginUserValidInputReturnsOkResult()
    {
        var authServiceMock = new Mock<IAuthService>();
        var controller = new AuthenticationController(authServiceMock.Object);

        var email = "test@retrorabbit.co.za";
        var expectedToken = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

        authServiceMock.Setup(x => x.CheckUserExist(email)).ReturnsAsync(true);
        authServiceMock.Setup(x => x.Login(email)).ReturnsAsync(expectedToken);

        var result = await controller.LoginUser(email);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var actualToken = Assert.IsType<string>(okResult.Value);

        Assert.Equal(expectedToken, actualToken);

        authServiceMock.Verify(x => x.CheckUserExist(email), Times.Once);
        authServiceMock.Verify(x => x.Login(email), Times.Once);
    }

    [Fact]
    public async Task LoginUserUserNotFoundReturnsNotFoundResult()
    {
        var authServiceMock = new Mock<IAuthService>();
        var controller = new AuthenticationController(authServiceMock.Object);

        var userEmail = "nonexistent@retrorabbit.co.za";
        authServiceMock.Setup(x => x.CheckUserExist(userEmail)).ReturnsAsync(false);

        var result = await controller.LoginUser(userEmail);

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("User not found", notFoundResult.Value);
    }

    [Fact]
    public async Task RegisterEmployeeValidInputReturnsCreatedAtActionResult()
    {
        var authServiceMock = new Mock<IAuthService>();
        var controller = new AuthenticationController(authServiceMock.Object);

        var newEmployee = new EmployeeDto
            (1, "Emp123", "Tax123", new DateTime(2022, 1, 1), null, 1, false, "No disability", 2,
             new EmployeeTypeDto{ Id = 1, Name = "Full Time" }, "Notes", 20.0f, 15.0f, 50.0f, 50000, "John Doe", "JD", "Doe",
             new DateTime(1990, 1, 1),
             "South Africa", "South African", "123456789", "AB123456", new DateTime(2025, 1, 1), "South Africa",
             Race.White, Gender.Male, "photo.jpg",
             "test@retrorabbit.co.za", "john.doe.personal@example.com", "1234567890", 1, 1,
             new EmployeeAddressDto
                 (1, "Unit 1", "Complex A", "123", "Suburb", "City", "Country", "Province", "12345"),
             new EmployeeAddressDto
                 (2, "P.O. Box 123", "", "456", "Suburb", "City", "Country", "Province", "54321"),
             "12",
             "Emergency Contact",
             "987654321"
            );

        authServiceMock.Setup(x => x.CheckUserExist(newEmployee.Email!)).ReturnsAsync(false);
        authServiceMock.Setup(x => x.RegisterEmployee(newEmployee))
                       .ReturnsAsync("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789");

        var result = await controller.RegisterEmployee(newEmployee);

        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal(nameof(AuthenticationController.RegisterEmployee), createdAtActionResult.ActionName);
        Assert.Equal(newEmployee, createdAtActionResult.Value);
    }

    [Fact]
    public async Task RegisterEmployeeEmployeeAlreadyExistsReturnsNotFoundResult()
    {
        var authServiceMock = new Mock<IAuthService>();
        var controller = new AuthenticationController(authServiceMock.Object);

        var existingEmployee = new EmployeeDto
            (1, "Emp123", "Tax123", new DateTime(2022, 1, 1), null, 1, false, "No disability", 2,
             new EmployeeTypeDto{ Id = 1, Name = "Full Time" }, "Notes", 20.0f, 15.0f, 50.0f, 50000, "John Doe", "JD", "Doe",
             new DateTime(1990, 1, 1),
             "South Africa", "South African", "123456789", "AB123456", new DateTime(2025, 1, 1), "South Africa",
             Race.White, Gender.Male, "photo.jpg",
             "test@retrorabbit.co.za", "john.doe.personal@example.com", "1234567890", 1, 1,
             new EmployeeAddressDto
                 (1, "Unit 1", "Complex A", "123", "Suburb", "City", "Country", "Province", "12345"),
             new EmployeeAddressDto
                 (2, "P.O. Box 123", "", "456", "Suburb", "City", "Country", "Province", "54321"),
             "12",
             "Emergency Contact",
             "987654321"
            );

        authServiceMock.Setup(x => x.CheckUserExist(existingEmployee.Email!)).ReturnsAsync(true);

        var result = await controller.RegisterEmployee(existingEmployee);

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("Employee already exists", notFoundResult.Value);
    }

    [Fact]
    public async Task GetUserRolesSuccessReturnsOkResult()
    {
        var authServiceMock = new Mock<IAuthService>();
        var controller = new AuthenticationController(authServiceMock.Object);

        var httpContextMock = new Mock<HttpContext>();
        httpContextMock.SetupGet(c => c.User.Identity).Returns(new ClaimsIdentity(new List<Claim>
        {
            new(ClaimTypes.Email, "test@retrorabbit.co.za")
        }));

        controller.ControllerContext = new ControllerContext
        {
            HttpContext = httpContextMock.Object
        };

        authServiceMock.Setup(a => a.GetUserRoles(It.IsAny<string>()))
                       .ReturnsAsync(new Dictionary<string, List<string>>());

        var result = await controller.GetUserRoles(null);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var roles = Assert.IsType<Dictionary<string, List<string>>>(okResult.Value);

        authServiceMock.Verify(a => a.GetUserRoles("test@retrorabbit.co.za"), Times.Once);
    }

    [Fact]
    public async Task GetUserRoles_ReturnsOkResult_WithEmail()
    {
        var authServiceMock = new Mock<IAuthService>();
        var controller = new AuthenticationController(authServiceMock.Object);

        var httpContextMock = new Mock<HttpContext>();
        httpContextMock.SetupGet(c => c.User.Identity).Returns(new ClaimsIdentity(new List<Claim>
        {
            new(ClaimTypes.Email, "test@retrorabbit.co.za")
        }));

        controller.ControllerContext = new ControllerContext
        {
            HttpContext = httpContextMock.Object
        };

        authServiceMock.Setup(a => a.GetUserRoles("test@retrorabbit.co.za"))
                       .ReturnsAsync(new Dictionary<string, List<string>>());

        var result = await controller.GetUserRoles(null);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var roles = Assert.IsType<Dictionary<string, List<string>>>(okResult.Value);

        authServiceMock.Verify(a => a.GetUserRoles("test@retrorabbit.co.za"), Times.Once);
    }

    [Fact]
    public async Task GetUserRoles_ReturnsOkResult_WithoutEmail()
    {
        var authServiceMock = new Mock<IAuthService>();
        var controller = new AuthenticationController(authServiceMock.Object);

        var httpContextMock = new Mock<HttpContext>();
        httpContextMock.SetupGet(c => c.User.Identity).Returns(new ClaimsIdentity(new List<Claim>
        {
            new(ClaimTypes.Email, "test@retrorabbit.co.za")
        }));

        controller.ControllerContext = new ControllerContext
        {
            HttpContext = httpContextMock.Object
        };

        authServiceMock.Setup(a => a.GetUserRoles("test@retrorabbit.co.za"))
                       .ReturnsAsync(new Dictionary<string, List<string>>());

        var result = await controller.GetUserRoles(null);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var roles = Assert.IsType<Dictionary<string, List<string>>>(okResult.Value);

        authServiceMock.Verify(a => a.GetUserRoles("test@retrorabbit.co.za"), Times.Once);
    }


    [Fact]
    public async Task GetUserRolesReturnsNotFoundResult()
    {
        var authServiceMock = new Mock<IAuthService>();
        var controller = new AuthenticationController(authServiceMock.Object);

        var claims = new List<Claim>
        {
            new(ClaimTypes.Email, "test@retrorabbit.co.za")
        };
        var identity = new ClaimsIdentity(claims, "TestAuthentication");
        var claimsPrincipal = new ClaimsPrincipal(identity);

        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = claimsPrincipal }
        };

        authServiceMock.Setup(x => x.GetUserRoles(It.IsAny<string>()))
                       .ThrowsAsync(new Exception("Exception occurred while getting user roles"));

        var result = await controller.GetUserRoles("test@retrorabbit.co.za");

        Assert.IsType<NotFoundObjectResult>(result);

        if (result is NotFoundObjectResult notFoundResult)
        {
            Assert.NotNull(notFoundResult.Value);
            Assert.IsType<string>(notFoundResult.Value);

            var actualErrorMessage = notFoundResult.Value.ToString();

            Assert.Equal("Exception occurred while getting user roles", actualErrorMessage);
        }
    }
}
