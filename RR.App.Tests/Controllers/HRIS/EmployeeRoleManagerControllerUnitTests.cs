using HRIS.Models;
using HRIS.Services.Interfaces;
using Moq;
using RR.App.Controllers.HRIS;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using RR.App.Tests.Helper;
using HRIS.Services.Services;
using Auth0.ManagementApi.Paging;
using Auth0.ManagementApi.Models;

namespace RR.App.Tests.Controllers.HRIS
{
    public class EmployeeRoleManageControllerUnitTests
    {
        private readonly Mock<IEmployeeRoleService> _employeeRoleMockService;
        private readonly Mock<IEmployeeService> _employeeMockService;
        private readonly Mock<IRoleService> _roleMockService;
        private readonly Mock<IAuthService> _authMockService;
        private readonly EmployeeRoleManageController _controller;

        public EmployeeRoleManageControllerUnitTests()
        {
            _employeeRoleMockService = new Mock<IEmployeeRoleService>();
            _employeeMockService = new Mock<IEmployeeService>();
            _roleMockService = new Mock<IRoleService>();
            _authMockService = new Mock<IAuthService>();
            _controller = new EmployeeRoleManageController(_employeeRoleMockService.Object, _employeeMockService.Object, _roleMockService.Object, _authMockService.Object);
        }

        [Fact]
        public async Task AddRole_ReturnsCreatedResult()
        {
            var email = "test@example.com";
            var role = "Admin";

            var employee = new EmployeeDto { Email = email };
            var authUser = new List<Auth0.ManagementApi.Models.User>
    {
        new Auth0.ManagementApi.Models.User { UserId = "1" }
    };

            var roleDto = new RoleDto { Id = 1, Description = role, AuthRoleId = "authRoleId" };
            var employeeRoleDto = new EmployeeRoleDto { Id = 1, Employee = employee, Role = roleDto };

            _employeeMockService.Setup(service => service.GetEmployee(email)).ReturnsAsync(employee);
            _authMockService.Setup(service => service.GetUsersByEmailAsync(email)).ReturnsAsync(authUser);
            _roleMockService.Setup(service => service.CheckRole(role)).ReturnsAsync(true);
            _roleMockService.Setup(service => service.GetRole(role)).ReturnsAsync(roleDto);
            _authMockService.Setup(service => service.AddRoleToUserAsync("1", "authRoleId")).ReturnsAsync(true);
            _employeeRoleMockService.Setup(service => service.SaveEmployeeRole(It.IsAny<EmployeeRoleDto>())).ReturnsAsync(employeeRoleDto);

            var result = await _controller.AddRole(email, role);

            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var model = Assert.IsAssignableFrom<EmployeeRoleDto>(createdResult.Value);
            Assert.Equal(employeeRoleDto.Id, model.Id);
        }

        [Fact]
        public async Task AddRole_InvalidInput_ThrowsException()
        {
            var email = "";
            var role = "";

            var result = await MiddlewareHelperUnitTests.SimulateHandlingExceptionMiddlewareAsync(() => _controller.AddRole(email, role));
            Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Invalid input", (result as NotFoundObjectResult).Value);
        }

        [Fact]
        public async Task AddRole_EmployeeNotFound_ThrowsException()
        {
            var email = "test@example.com";
            var role = "Admin";

            _employeeMockService.Setup(service => service.GetEmployee(email)).ReturnsAsync((EmployeeDto)null);

            var result = await MiddlewareHelperUnitTests.SimulateHandlingExceptionMiddlewareAsync(() => _controller.AddRole(email, role));

            Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Employee not found.", (result as NotFoundObjectResult).Value);
        }

        [Fact]
        public async Task AddRole_AuthServiceFails_ThrowsException()
        {
            var email = "test@example.com";
            var role = "Admin";

            var employee = new EmployeeDto { Email = email };

            _employeeMockService.Setup(service => service.GetEmployee(email)).ReturnsAsync(employee);
            _authMockService.Setup(service => service.GetUsersByEmailAsync(email)).ReturnsAsync((List<Auth0.ManagementApi.Models.User>)null);

            var result = await MiddlewareHelperUnitTests.SimulateHandlingExceptionMiddlewareAsync(() => _controller.AddRole(email, role));

            Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("User not found in authentication service.", (result as NotFoundObjectResult).Value);
        }

        [Fact]
        public async Task UpdateRole_ReturnsCreatedResult()
        {
            var email = "test@example.com";
            var role = "Admin";

            var employee = new EmployeeDto { Email = email };
            var authUser = new List<Auth0.ManagementApi.Models.User>
    {
        new Auth0.ManagementApi.Models.User { UserId = "1" }
    };
            var roleDto = new RoleDto { Id = 1, Description = role, AuthRoleId = "authRoleId" };
            var employeeRoleDto = new EmployeeRoleDto { Id = 1, Employee = employee, Role = roleDto };

            _employeeMockService.Setup(service => service.GetEmployee(email)).ReturnsAsync(employee);
            _authMockService.Setup(service => service.GetUsersByEmailAsync(email)).ReturnsAsync(authUser);
            _roleMockService.Setup(service => service.CheckRole(role)).ReturnsAsync(true);
            _roleMockService.Setup(service => service.GetRole(role)).ReturnsAsync(roleDto);
            _authMockService.Setup(service => service.AddRoleToUserAsync("1", "authRoleId")).ReturnsAsync(true);
            _employeeRoleMockService.Setup(service => service.GetEmployeeRole(email)).ReturnsAsync(employeeRoleDto);

            // Simulate successful update
            _employeeRoleMockService.Setup(service => service.UpdateEmployeeRole(It.IsAny<EmployeeRoleDto>())).ReturnsAsync(employeeRoleDto);

            var result = await MiddlewareHelperUnitTests.SimulateHandlingExceptionMiddlewareAsync(() => _controller.UpdateRole(email, role));

            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var model = Assert.IsAssignableFrom<EmployeeRoleDto>(createdResult.Value);
            Assert.Equal(employeeRoleDto.Id, model.Id);
        }

        [Fact]
        public async Task UpdateRole_EmployeeNotFound_ThrowsException()
        {
            var email = "test@example.com";
            var role = "Admin";

            _employeeMockService.Setup(service => service.GetEmployee(email)).ReturnsAsync((EmployeeDto)null);

            var result = await MiddlewareHelperUnitTests.SimulateHandlingExceptionMiddlewareAsync(() => _controller.UpdateRole(email, role));

            Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Employee not found.", (result as NotFoundObjectResult).Value);
        }

        [Fact]
        public async Task RemoveRole_ReturnsOkResult()
        {
            var email = "test@example.com";
            var role = "Admin";

            var employee = new EmployeeDto { Email = email };
            var authUser = new List<Auth0.ManagementApi.Models.User>
    {
        new Auth0.ManagementApi.Models.User { UserId = "1" }
    };

            var roleDto = new RoleDto { Id = 1, Description = role, AuthRoleId = "authRoleId" };
            var employeeRoleDto = new EmployeeRoleDto { Id = 1, Employee = employee, Role = roleDto };

            _employeeMockService.Setup(service => service.GetEmployee(email)).ReturnsAsync(employee);
            _authMockService.Setup(service => service.GetUsersByEmailAsync(email)).ReturnsAsync(authUser);
            _roleMockService.Setup(service => service.GetRole(role)).ReturnsAsync(roleDto);
            _employeeRoleMockService.Setup(service => service.GetEmployeeRole(email)).ReturnsAsync(employeeRoleDto);
            _employeeRoleMockService.Setup(service => service.DeleteEmployeeRole(email, role)).ReturnsAsync(employeeRoleDto);

            // Mocking auth service to return empty roles list
            _authMockService.Setup(x => x.GetUserRolesAsync(It.IsAny<string>())).ReturnsAsync(() =>
            {
                var roles = new List<Auth0.ManagementApi.Models.Role>
                {
                    new Auth0.ManagementApi.Models.Role { Name = "Employee", Id = "roleId" }
                };

                var pagedList = new PagedList<Auth0.ManagementApi.Models.Role>(roles);
                return (IPagedList<Auth0.ManagementApi.Models.Role>)pagedList;
            });

            var result = await _controller.RemoveRole(email, role);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedEmployeeRole = Assert.IsAssignableFrom<EmployeeRoleDto>(okResult.Value);

            Assert.Equal(employeeRoleDto.Id, returnedEmployeeRole.Id);
            Assert.Equal(employeeRoleDto.Employee.Email, returnedEmployeeRole.Employee.Email);
            Assert.Equal(employeeRoleDto.Role.Description, returnedEmployeeRole.Role.Description);
        }

        [Fact]
        public async Task RemoveRole_EmployeeNotFound_ThrowsException()
        {
            var email = "test@example.com";
            var role = "Admin";

            _employeeMockService.Setup(service => service.GetEmployee(email)).ReturnsAsync((EmployeeDto)null);

            var result = await MiddlewareHelperUnitTests.SimulateHandlingExceptionMiddlewareAsync(() => _controller.RemoveRole(email, role));

            Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Employee not found.", (result as NotFoundObjectResult).Value);
        }

        [Fact]
        public async Task RemoveRole_RoleNotFound_ThrowsException()
        {
            var email = "test@example.com";
            var role = "Admin";

            _employeeMockService.Setup(service => service.GetEmployee(email)).ReturnsAsync(new EmployeeDto { Email = email });
            _roleMockService.Setup(service => service.GetRole(role)).ReturnsAsync((RoleDto)null);

            var result = await MiddlewareHelperUnitTests.SimulateHandlingExceptionMiddlewareAsync(() => _controller.RemoveRole(email, role));

            Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Role not found.", (result as NotFoundObjectResult).Value);
        }

        [Fact]
        public async Task RemoveRole_AuthServiceFails_ThrowsException()
        {
            var email = "test@example.com";
            var role = "Admin";

            var employee = new EmployeeDto { Email = email };
            var roleDto = new RoleDto { Id = 1, Description = role, AuthRoleId = "authRoleId" };

            _employeeMockService.Setup(service => service.GetEmployee(email)).ReturnsAsync(employee);
            _roleMockService.Setup(service => service.GetRole(role)).ReturnsAsync(roleDto);
            _authMockService.Setup(service => service.GetUsersByEmailAsync(email)).ReturnsAsync((List<Auth0.ManagementApi.Models.User>)null);

            var result = await MiddlewareHelperUnitTests.SimulateHandlingExceptionMiddlewareAsync(() => _controller.RemoveRole(email, role));

            Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("User not found in authentication service.", (result as NotFoundObjectResult).Value);
        }

        [Fact]
        public async Task RemoveRole_EmployeeRoleNotFound_ThrowsException()
        {
            var email = "test@example.com";
            var role = "Admin";

            var employee = new EmployeeDto { Email = email };
            var authUser = new List<Auth0.ManagementApi.Models.User>
    {
        new Auth0.ManagementApi.Models.User { UserId = "1" }
    };

            var roleDto = new RoleDto { Id = 1, Description = role, AuthRoleId = "authRoleId" };

            _employeeMockService.Setup(service => service.GetEmployee(email)).ReturnsAsync(employee);
            _authMockService.Setup(service => service.GetUsersByEmailAsync(email)).ReturnsAsync(authUser);
            _roleMockService.Setup(service => service.GetRole(role)).ReturnsAsync(roleDto);

            // Ensure this returns null to simulate employee role not found
            _employeeRoleMockService.Setup(service => service.GetEmployeeRole(email)).ReturnsAsync((EmployeeRoleDto)null);

            var exception = await Assert.ThrowsAsync<CustomException>(() => _controller.RemoveRole(email, role));

            Assert.Equal("User roles not found in authentication service.", exception.Message);
        }

    }
}