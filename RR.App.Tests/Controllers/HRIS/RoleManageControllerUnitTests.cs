using HRIS.Models;
using HRIS.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RR.App.Controllers.HRIS;
using Xunit;

namespace RR.App.Tests.Controllers.HRIS;

public class RoleManageControllerUnitTests
{
    private readonly Mock<IRoleAccessLinkService> _roleAccessLinkServiceMock;
    private readonly Mock<IRoleAccessService> _roleAccessServiceMock;
    private readonly Mock<IRoleService> _roleServiceMock;
    private readonly RoleManageController _controller;
    private readonly RoleDto _roleDto;
    private readonly RoleAccessDto _roleAccessDto;
    private readonly RoleAccessLinkDto _roleAccessLinkDto;

    public RoleManageControllerUnitTests()
    {
        _roleAccessLinkServiceMock = new Mock<IRoleAccessLinkService>();
        _roleServiceMock = new Mock<IRoleService>();
        _roleAccessServiceMock = new Mock<IRoleAccessService>();
        _controller = new RoleManageController(_roleAccessLinkServiceMock.Object, _roleServiceMock.Object, _roleAccessServiceMock.Object);
        _roleDto = new RoleDto { Id = 0, Description = "Super Admin" };
        _roleAccessDto = new RoleAccessDto { Id = 1, Permission = "Permission 1", Grouping = "Group 1" };
        _roleAccessLinkDto = new RoleAccessLinkDto { Id = 1, Role = _roleDto, RoleAccess = _roleAccessDto };
    }

    [Fact]
    public async Task AddPermissionReturnsCreatedAtAction()
    {
        _roleServiceMock.Setup(x => x.CheckRole(It.IsAny<string>())).ReturnsAsync(true);
        _roleServiceMock.Setup(x => x.GetRole(It.IsAny<string>())).ReturnsAsync(_roleDto);
        _roleServiceMock.Setup(x => x.SaveRole(It.IsAny<RoleDto>())).ReturnsAsync(_roleDto);
        _roleAccessServiceMock.Setup(x => x.CheckRoleAccess(It.IsAny<string>())).ReturnsAsync(true);
        _roleAccessServiceMock.Setup(x => x.GetRoleAccess(It.IsAny<string>())).ReturnsAsync(_roleAccessDto);
        _roleAccessServiceMock.Setup(x => x.SaveRoleAccess(It.IsAny<RoleAccessDto>())).ReturnsAsync(_roleAccessDto);
        _roleAccessLinkServiceMock.Setup(x => x.Save(It.IsAny<RoleAccessLinkDto>())).ReturnsAsync(_roleAccessLinkDto);

        var result = await _controller.AddPermission("Super Admin", "Permission 1", "Grouping 1");

        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
        var roleAccessLinkDto = Assert.IsType<RoleAccessLinkDto>(createdAtActionResult.Value);
        Assert.NotNull(roleAccessLinkDto);
        Assert.Equal("AddPermission", createdAtActionResult.ActionName);
    }

    [Fact]
    public async Task AddPermissionHandlesExceptionReturnsNotFound()
    {
        _roleAccessLinkServiceMock.Setup(x => x.Save(It.IsAny<RoleAccessLinkDto>())).ThrowsAsync(new Exception("Error adding permission"));

        var result = await _controller.AddPermission("Super Admin", "Permission 1", "Grouping 1");

        var notFoundObjectResult = Assert.IsType<NotFoundObjectResult>(result);
        var errorMessage = Assert.IsType<string>(notFoundObjectResult.Value);
        Assert.Equal("Error adding permission", errorMessage);
    }

    [Fact]
    public async Task RemovePermissionReturnsCreatedAtAction()
    {
        _roleServiceMock.Setup(x => x.CheckRole(It.IsAny<string>())).ReturnsAsync(true);
        _roleServiceMock.Setup(x => x.GetRole(It.IsAny<string>())).ReturnsAsync(_roleDto);
        _roleServiceMock.Setup(x => x.SaveRole(It.IsAny<RoleDto>())).ReturnsAsync(_roleDto);
        _roleAccessServiceMock.Setup(x => x.CheckRoleAccess(It.IsAny<string>())).ReturnsAsync(true);
        _roleAccessServiceMock.Setup(x => x.GetRoleAccess(It.IsAny<string>())).ReturnsAsync(_roleAccessDto);
        _roleAccessServiceMock.Setup(x => x.SaveRoleAccess(It.IsAny<RoleAccessDto>())).ReturnsAsync(_roleAccessDto);
        _roleAccessLinkServiceMock.Setup(x => x.Delete(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(_roleAccessLinkDto);

        var result = await _controller.RemovePermission("Super Admin", "Permission 2", "Grouping 1");

        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
        var roleAccessLinkDto = Assert.IsType<RoleAccessLinkDto>(createdAtActionResult.Value);
        Assert.NotNull(roleAccessLinkDto);
        Assert.Equal("RemovePermission", createdAtActionResult.ActionName);
    }

    [Fact]
    public async Task RemovePermissionHandlesExceptionReturnsBadRequest()
    {
        _roleAccessLinkServiceMock.Setup(x => x.Delete(It.IsAny<string>(), It.IsAny<string>())).ThrowsAsync(new Exception("Error removing permission"));

        var result = await _controller.RemovePermission("Super Admin", "Permission 1", "Grouping 1");

        var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result);
        var errorMessage = Assert.IsType<string>(badRequestObjectResult.Value);
        Assert.Equal("Error removing permission", errorMessage);
    }

    [Fact]
    public async Task GetRolePermissionsReturnsOk()
    {
        _roleAccessLinkServiceMock.Setup(x => x.GetByRole(It.IsAny<string>()))
                                 .ReturnsAsync(new Dictionary<string, List<string>>
                                 {
                                     { "Super Admin", new List<string> { "Permission 1", "Permission 2" } }
                                 });

        var result = await _controller.GetRolePermissions("Super Admin");

        var okObjectResult = Assert.IsType<OkObjectResult>(result);
        var roleAccessLinks = Assert.IsType<Dictionary<string, List<string>>>(okObjectResult.Value);
        Assert.NotNull(roleAccessLinks);
    }

    [Fact]
    public async Task GetRolePermissionsHandlesExceptionReturnsNotFound()
    {
        _roleAccessLinkServiceMock.Setup(x => x.GetByRole(It.IsAny<string>())).ThrowsAsync(new Exception("Error getting role permissions"));

        var result = await _controller.GetRolePermissions("Super Admin");

        var notFoundObjectResult = Assert.IsType<NotFoundObjectResult>(result);
        var errorMessage = Assert.IsType<string>(notFoundObjectResult.Value);
        Assert.Equal("Error getting role permissions", errorMessage);
    }


    [Fact]
    public async Task GetAllRoleAccessLinkReturnsOk()
    {
        _roleAccessLinkServiceMock.Setup(x => x.GetAll()).ReturnsAsync(new Dictionary<string, List<string>>
        {
            { "Super Admin", new List<string> { "Permission 1", "Permission 2" } }
        });

        var result = await _controller.GetAllRoleAccessLink();

        var okObjectResult = Assert.IsType<OkObjectResult>(result);
        var roleAccessLinks = Assert.IsType<Dictionary<string, List<string>>>(okObjectResult.Value);
        Assert.NotNull(roleAccessLinks);
    }

    [Fact]
    public async Task GetAllRoleAccessLinkHandlesExceptionReturnsNotFound()
    {
        _roleAccessLinkServiceMock.Setup(x => x.GetAll()).ThrowsAsync(new Exception("Error getting all role access link"));

        var result = await _controller.GetAllRoleAccessLink();

        var notFoundObjectResult = Assert.IsType<NotFoundObjectResult>(result);
        var errorMessage = Assert.IsType<string>(notFoundObjectResult.Value);
        Assert.Equal("Error getting all role access link", errorMessage);
    }

    [Fact]
    public async Task GetAllRoleAccessLinksReturnsOk()
    {
        _roleAccessLinkServiceMock.Setup(x => x.GetAllRoleAccessLink()).ReturnsAsync(new List<RoleAccessLinkDto> { _roleAccessLinkDto });

        var result = await _controller.GetAllRoleAccessLinks();

        var okObjectResult = Assert.IsType<OkObjectResult>(result);
        var roleAccessLinks = Assert.IsType<List<RoleAccessLinkDto>>(okObjectResult.Value);
        Assert.NotNull(roleAccessLinks);
    }

    [Fact]
    public async Task GetAllRoleAccessLinksHandlesExceptionReturnsNotFound()
    {
        _roleAccessLinkServiceMock.Setup(x => x.GetAllRoleAccessLink()).ThrowsAsync(new Exception("Error getting all role access link"));

        var result = await _controller.GetAllRoleAccessLinks();

        var notFoundObjectResult = Assert.IsType<NotFoundObjectResult>(result);
        var errorMessage = Assert.IsType<string>(notFoundObjectResult.Value);
        Assert.Equal("Error getting all role access link", errorMessage);
    }

    [Fact]
    public async Task GetAllRoleAccessesReturnsOk()
    {
        _roleAccessServiceMock.Setup(x => x.GetAllRoleAccess()).ReturnsAsync(new List<RoleAccessDto> { _roleAccessDto });

        var result = await _controller.GetAllRoleAccesses();

        var okObjectResult = Assert.IsType<OkObjectResult>(result);
        var roleAccesses = Assert.IsType<List<RoleAccessDto>>(okObjectResult.Value);
        Assert.NotNull(roleAccesses);
    }

    [Fact]
    public async Task GetAllRoleAccessesHandlesExceptionReturnsNotFound()
    {
        _roleAccessServiceMock.Setup(x => x.GetAllRoleAccess()).ThrowsAsync(new Exception("Error getting all role access"));

        var result = await _controller.GetAllRoleAccesses();

        var notFoundObjectResult = Assert.IsType<NotFoundObjectResult>(result);
        var errorMessage = Assert.IsType<string>(notFoundObjectResult.Value);
        Assert.Equal("Error getting all role access", errorMessage);
    }

    [Fact]
    public async Task GetAllRolesReturnsOk()
    {
        _roleServiceMock.Setup(x => x.GetAll()).ReturnsAsync(new List<RoleDto> { _roleDto });

        var result = await _controller.GetAllRoles();

        var okObjectResult = Assert.IsType<OkObjectResult>(result);
        var roles = Assert.IsType<List<RoleDto>>(okObjectResult.Value);
        Assert.NotNull(roles);
    }

    [Fact]
    public async Task GetAllRolesHandlesExceptionReturnsNotFound()
    {
        _roleServiceMock.Setup(x => x.GetAll()).ThrowsAsync(new Exception("Error getting all roles"));

        var result = await _controller.GetAllRoles();

        var notFoundObjectResult = Assert.IsType<NotFoundObjectResult>(result);
        var errorMessage = Assert.IsType<string>(notFoundObjectResult.Value);
        Assert.Equal("Error getting all roles", errorMessage);
    }
}