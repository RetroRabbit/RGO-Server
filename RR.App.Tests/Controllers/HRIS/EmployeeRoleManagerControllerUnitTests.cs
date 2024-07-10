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

namespace RR.App.Tests.Controllers.HRIS;

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
    public async Task UpdateRole_AuthUserIsNull_ThrowsException()
    {
        var email = "test@example.com";
        var role = "Admin";

        var employee = new EmployeeDto { Email = email };

        _employeeMockService.Setup(service => service.GetEmployee(email)).ReturnsAsync(employee);
        _authMockService.Setup(service => service.GetUsersByEmailAsync(email)).ReturnsAsync((List<Auth0.ManagementApi.Models.User>)null);

        var result = await MiddlewareHelperUnitTests.SimulateHandlingExceptionMiddlewareAsync(() => _controller.UpdateRole(email, role));

        Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("User not found in authentication service.", (result as NotFoundObjectResult).Value);
    }

    [Fact]
    public async Task UpdateRole_UserRoleNotFoundInAuth0_ThrowsException()
    {
        var email = "test@example.com";
        var role = "Admin";

        var employee = new EmployeeDto { Email = email };
        var authUser = new List<Auth0.ManagementApi.Models.User>
            {
                 new Auth0.ManagementApi.Models.User { UserId = "1" }
            };

        _employeeMockService.Setup(service => service.GetEmployee(email)).ReturnsAsync(employee);
        _authMockService.Setup(service => service.GetUsersByEmailAsync(email)).ReturnsAsync(authUser);
        _roleMockService.Setup(service => service.CheckRole(role)).ReturnsAsync(true);
        var roleDto = new RoleDto { Id = 1, Description = role, AuthRoleId = "authRoleId" };
        _roleMockService.Setup(service => service.GetRole(role)).ReturnsAsync(roleDto);
        _authMockService.Setup(service => service.AddRoleToUserAsync("1", "authRoleId")).ReturnsAsync(false);

        var result = await MiddlewareHelperUnitTests.SimulateHandlingExceptionMiddlewareAsync(() => _controller.UpdateRole(email, role));

        Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("Auth role not found in the database.", (result as NotFoundObjectResult).Value);
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
        //var authRoles = new List<Role>
        //{
        //    RoleTestData.TalentRole
        //};

        _employeeMockService.Setup(service => service.GetEmployee(email)).ReturnsAsync(employee);
        _authMockService.Setup(service => service.GetUsersByEmailAsync(email)).ReturnsAsync(authUser);
        _roleMockService.Setup(service => service.GetRole(role)).ReturnsAsync(roleDto);
        //   _authMockService.Setup(service => service.GetUserRolesAsync("1")).ReturnsA(true);

        _employeeRoleMockService.Setup(service => service.GetEmployeeRole(email)).ReturnsAsync((EmployeeRoleDto)null);

        var exception = await Assert.ThrowsAsync<CustomException>(() => _controller.RemoveRole(email, role));

        Assert.Equal("User roles not found in authentication service.", exception.Message);
    }

    [Fact]
    public async Task GetEmployeeRole_ReturnsOkResult_WithRole()
    {
        var email = "test@example.com";
        var employeeRole = new EmployeeRoleDto
        {
            Role = new RoleDto
            {
                Description = "Admin"
            }
        };

        _employeeRoleMockService.Setup(service => service.GetEmployeeRole(email)).ReturnsAsync(employeeRole);

        var result = await _controller.GetEmployeeRole(email);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var roleArray = Assert.IsAssignableFrom<string[]>(okResult.Value);
        Assert.Single(roleArray);
        Assert.Equal("Admin", roleArray.First());
    }

    [Fact]
    public async Task GetEmployeeRole_ThrowsCustomException_WhenEmployeeRoleServiceIsNull()
    {
        var controller = new EmployeeRoleManageController(null, null, null, null);

        var exception = await Assert.ThrowsAsync<CustomException>(() => controller.GetEmployeeRole("test@example.com"));

        Assert.Equal("Invalid input", exception.Message);
    }

    [Fact]
    public async Task GetAllRoles_ReturnsOkResult_WithRolesDescriptions()
    {
        var roles = new List<RoleDto>
            {
               new RoleDto { Description = "Admin" },
               new RoleDto { Description = "User" }
            };

        _roleMockService.Setup(service => service.GetAll()).ReturnsAsync(roles);

        var result = await _controller.GetAllRoles();

        var okResult = Assert.IsType<OkObjectResult>(result);
        var rolesDescriptions = Assert.IsAssignableFrom<List<string>>(okResult.Value);
        Assert.Equal(2, rolesDescriptions.Count);
        Assert.Contains("Admin", rolesDescriptions);
        Assert.Contains("User", rolesDescriptions);
    }

    [Fact]
    public async Task GetAllRoles_ThrowsCustomException_WhenRoleServiceIsNull()
    {
        var controller = new EmployeeRoleManageController(null, null, null, null);

        var exception = await Assert.ThrowsAsync<CustomException>(() => controller.GetAllRoles());

        Assert.Equal("Invalid input", exception.Message);
    }

    [Fact]
    public async Task GetAllEmployeeOnRoles_ReturnsOkResult_WithEmployeeRoles()
    {
        var roleId = 1;
        var employeeRoles = new List<EmployeeRoleDto>
            {
               new EmployeeRoleDto { Id = 1 },
               new EmployeeRoleDto { Id = 2 }
            };

        _employeeRoleMockService.Setup(service => service.GetAllEmployeeOnRoles(roleId)).ReturnsAsync(employeeRoles);

        var result = await _controller.GetAllEmployeeOnRoles(roleId);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedEmployeeRoles = Assert.IsAssignableFrom<List<EmployeeRoleDto>>(okResult.Value);
        Assert.Equal(2, returnedEmployeeRoles.Count);
    }

    [Fact]
    public async Task GetAllEmployeeOnRoles_ThrowsCustomException_WhenEmployeeRoleServiceIsNull()
    {
        var controller = new EmployeeRoleManageController(null, null, null, null);

        var exception = await Assert.ThrowsAsync<CustomException>(() => controller.GetAllEmployeeOnRoles(1));

        Assert.Equal("Invalid input", exception.Message);
    }
}

