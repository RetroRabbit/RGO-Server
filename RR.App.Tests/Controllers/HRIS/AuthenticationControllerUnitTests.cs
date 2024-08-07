using Auth0.ManagementApi.Paging;
using HRIS.Models;
using HRIS.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RR.App.Controllers.HRIS;
using System.Security.Claims;
using Xunit;

namespace RR.App.Tests.Controllers.HRIS
{
    public class AuthenticationControllerUnitTests
    {
        private readonly Mock<IAuthService> _authServiceMock;
        private readonly Mock<IEmployeeService> _employeeServiceMock;
        private readonly Mock<IRoleAccessLinkService> _roleAccessLinkServiceMock;
        private readonly Mock<ITerminationService> _terminationServiceMock;
        private readonly Mock<IErrorLoggingService> _errorLoggingServiceMock;
        private readonly AuthenticationController _controller;

        public AuthenticationControllerUnitTests()
        {
            _authServiceMock = new Mock<IAuthService>();
            _employeeServiceMock = new Mock<IEmployeeService>();
            _roleAccessLinkServiceMock = new Mock<IRoleAccessLinkService>();
            _terminationServiceMock = new Mock<ITerminationService>();
            _errorLoggingServiceMock = new Mock<IErrorLoggingService>();

            _controller = new AuthenticationController(_authServiceMock.Object, _employeeServiceMock.Object, _roleAccessLinkServiceMock.Object, _terminationServiceMock.Object, _errorLoggingServiceMock.Object);
        }

        [Fact]
        public async Task LoggingInUser_ReturnsOkResult()
        {
            var result = await _controller.LoggingInUser();
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Api connection works", okResult.Value);
        }

        [Fact]
        public async Task CheckUserExistence_UserFound_ReturnsOk()
        {
            var email = "test@example.com";
            var authId = "authId";
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.NameIdentifier, authId),
                new Claim(ClaimTypes.Role, "Employee")
            };
            var identity = new ClaimsIdentity(claims);
            var principal = new ClaimsPrincipal(identity);

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = principal }
            };

            _employeeServiceMock.Setup(x => x.CheckUserEmailExist(email)).ReturnsAsync(true);
            _employeeServiceMock.Setup(x => x.GetEmployeeByEmail(email)).ReturnsAsync(new EmployeeDto { Id = 1, AuthUserId = null });
            _terminationServiceMock.Setup(x => x.CheckTerminationExist(1)).ReturnsAsync(false);
            _roleAccessLinkServiceMock.Setup(x => x.GetRoleByEmployee(email)).ReturnsAsync(new Dictionary<string, List<string>>
            {
                { "Employee", new List<string> { "roleId" } }
            });
            _authServiceMock.Setup(x => x.GetAllRolesAsync()).ReturnsAsync(() =>
            {
                var roles = new List<Auth0.ManagementApi.Models.Role>
                {
                    new Auth0.ManagementApi.Models.Role { Name = "Employee", Id = "roleId" }
                };

                var pagedList = new PagedList<Auth0.ManagementApi.Models.Role>(roles);
                return (IPagedList<Auth0.ManagementApi.Models.Role>)pagedList;
            });

            _employeeServiceMock.Setup(x => x.UpdateEmployee(It.IsAny<EmployeeDto>()))
                                .ReturnsAsync((EmployeeDto employee) =>
                                {
                                    employee.AuthUserId = "updatedAuthId";
                                    return employee;
                                });

            var result = await _controller.CheckUserExistence();

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("User found.", okResult.Value);
        }

        [Fact(Skip = "Needs Fixing")]
        public async Task CheckUserExistence_UserNotFound_ReturnsNotFound()
        {
            var email = "test@example.com";
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, email)
            };
            var identity = new ClaimsIdentity(claims);
            var principal = new ClaimsPrincipal(identity);

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = principal }
            };

            _employeeServiceMock.Setup(x => x.CheckUserEmailExist(email)).ReturnsAsync(false);
            _authServiceMock.Setup(x => x.DeleteUser(email)).ReturnsAsync(false);

            var result = await _controller.CheckUserExistence();

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("User not found.", notFoundResult.Value);

            _authServiceMock.Verify(x => x.DeleteUser(email), Times.Once);
            _errorLoggingServiceMock.Verify(x => x.LogException(It.IsAny<Exception>()), Times.Once);
        }

        [Fact]
        public async Task CheckUserExistence_UserFoundButNoRole_ReturnsNotFound()
        {
            var email = "test@example.com";
            var authId = "authId";
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.NameIdentifier, authId)
            };
            var identity = new ClaimsIdentity(claims);
            var principal = new ClaimsPrincipal(identity);

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = principal }
            };

            _employeeServiceMock.Setup(x => x.CheckUserEmailExist(email)).ReturnsAsync(true);
            _employeeServiceMock.Setup(x => x.GetEmployeeByEmail(email)).ReturnsAsync(new EmployeeDto { Id = 1, AuthUserId = null });
            _terminationServiceMock.Setup(x => x.CheckTerminationExist(1)).ReturnsAsync(false);
            _roleAccessLinkServiceMock.Setup(x => x.GetRoleByEmployee(email)).ReturnsAsync(new Dictionary<string, List<string>>
            {
                { "Employee", new List<string> { "roleId" } }
            });
            _authServiceMock.Setup(x => x.GetAllRolesAsync()).ReturnsAsync(() =>
            {
                var roles = new List<Auth0.ManagementApi.Models.Role>();
                var pagedList = new PagedList<Auth0.ManagementApi.Models.Role>(roles);
                return (IPagedList<Auth0.ManagementApi.Models.Role>)pagedList;
            });

            var result = await _controller.CheckUserExistence();

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("User not found", notFoundResult.Value);
            _errorLoggingServiceMock.Verify(x => x.LogException(It.IsAny<Exception>()), Times.Once);
        }

        [Fact]
        public async Task CheckUserExistence_UserFoundWithRole_ReturnsOk()
        {
            var email = "test@example.com";
            var authId = "authId";
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.NameIdentifier, authId),
                new Claim(ClaimTypes.Role, "Employee")
            };
            var identity = new ClaimsIdentity(claims);
            var principal = new ClaimsPrincipal(identity);

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = principal }
            };

            _employeeServiceMock.Setup(x => x.CheckUserEmailExist(email)).ReturnsAsync(true);
            _terminationServiceMock.Setup(x => x.CheckTerminationExist(1)).ReturnsAsync(false);
            _roleAccessLinkServiceMock.Setup(x => x.GetRoleByEmployee(email)).ReturnsAsync(new Dictionary<string, List<string>>
            {
                { "Employee", new List<string> { "roleId" } }
            });

            var result = await _controller.CheckUserExistence();

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("User found.", okResult.Value);
        }

        [Fact(Skip = "Needs Fixing")]
        public async Task CheckUserExistence_ExceptionThrown_ReturnsInternalServerError()
        {
            var email = "test@example.com";
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, email)
            };
            var identity = new ClaimsIdentity(claims);
            var principal = new ClaimsPrincipal(identity);

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = principal }
            };

            _employeeServiceMock.Setup(x => x.CheckUserEmailExist(email)).ThrowsAsync(new Exception("Simulated error"));

            var result = await _controller.CheckUserExistence();

            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, statusCodeResult.StatusCode);
            Assert.Equal("Simulated error", statusCodeResult.Value);
            _errorLoggingServiceMock.Verify(x => x.LogException(It.IsAny<Exception>()), Times.Once);
        }
    }
}
