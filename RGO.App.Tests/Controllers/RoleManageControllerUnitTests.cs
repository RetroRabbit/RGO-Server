using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using RGO.App.Controllers;
using RGO.Models;
using RGO.Services.Interfaces;

namespace RGO.App.Tests.Controllers
{
    public class RoleManageControllerUnitTests
    {
        private readonly Mock<IRoleAccessLinkService> roleAccessLinkServiceMock;
        private readonly Mock<IRoleService> roleServiceMock;
        private readonly Mock<IRoleAccessService> roleAccessServiceMock;

        public RoleManageControllerUnitTests()
        {
            roleAccessLinkServiceMock = new Mock<IRoleAccessLinkService>();
            roleServiceMock = new Mock<IRoleService>();
            roleAccessServiceMock = new Mock<IRoleAccessService>();
        }

        public static RoleDto CreateRoleDto()
        {
            return new RoleDto(1, "Super Admin");
        }
    
        public static RoleAccessDto CreateRoleAccessDto()   
        {
            return new RoleAccessDto(1, "Permission 1", "Group 1");
        }

        public static RoleAccessLinkDto CreateRoleAccessLinkDto()
        {
            return new RoleAccessLinkDto(1, CreateRoleDto(), CreateRoleAccessDto());
        }

        [Fact]
        public async Task AddPermissionReturnsCreatedAtAction()
        {
            roleServiceMock.Setup(x => x.CheckRole(It.IsAny<string>())).ReturnsAsync(true);
            roleServiceMock.Setup(x => x.GetRole(It.IsAny<string>())).ReturnsAsync(CreateRoleDto());
            roleServiceMock.Setup(x => x.SaveRole(It.IsAny<RoleDto>())).ReturnsAsync(CreateRoleDto());

            roleAccessServiceMock.Setup(x => x.CheckRoleAccess(It.IsAny<string>())).ReturnsAsync(true);
            roleAccessServiceMock.Setup(x => x.GetRoleAccess(It.IsAny<string>())).ReturnsAsync(CreateRoleAccessDto());
            roleAccessServiceMock.Setup(x => x.SaveRoleAccess(It.IsAny<RoleAccessDto>())).ReturnsAsync(CreateRoleAccessDto());

            roleAccessLinkServiceMock.Setup(x => x.Save(It.IsAny<RoleAccessLinkDto>())).ReturnsAsync(CreateRoleAccessLinkDto());

            var controller = new RoleManageController(roleAccessLinkServiceMock.Object, roleServiceMock.Object, roleAccessServiceMock.Object);

            var result = await controller.AddPermission("Super Admin", "Permission 1", "Grouping 1");

            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            var roleAccessLinkDto = Assert.IsType<RoleAccessLinkDto>(createdAtActionResult.Value);
            Assert.NotNull(roleAccessLinkDto);
            Assert.Equal("AddPermission", createdAtActionResult.ActionName);
        }

        [Fact]
        public async Task AddPermissionHandlesExceptionReturnsNotFound()
        {
            var roleAccessLinkServiceMock = new Mock<IRoleAccessLinkService>();
            roleAccessLinkServiceMock.Setup(x => x.Save(It.IsAny<RoleAccessLinkDto>())).ThrowsAsync(new Exception("Error adding permission"));

            var roleServiceMock = new Mock<IRoleService>();
            var roleAccessServiceMock = new Mock<IRoleAccessService>();

            var controller = new RoleManageController(roleAccessLinkServiceMock.Object, roleServiceMock.Object, roleAccessServiceMock.Object);

            var result = await controller.AddPermission("Super Admin", "Permission 1", "Grouping 1");

            var notFoundObjectResult = Assert.IsType<NotFoundObjectResult>(result);
            var errorMessage = Assert.IsType<string>(notFoundObjectResult.Value);
            Assert.Equal("Error adding permission", errorMessage);
        }

        [Fact]
        public async Task RemovePermissionReturnsCreatedAtAction()
        {
            roleServiceMock.Setup(x => x.CheckRole(It.IsAny<string>())).ReturnsAsync(true);
            roleServiceMock.Setup(x => x.GetRole(It.IsAny<string>())).ReturnsAsync(CreateRoleDto());
            roleServiceMock.Setup(x => x.SaveRole(It.IsAny<RoleDto>())).ReturnsAsync(CreateRoleDto());

            roleAccessServiceMock.Setup(x => x.CheckRoleAccess(It.IsAny<string>())).ReturnsAsync(true);
            roleAccessServiceMock.Setup(x => x.GetRoleAccess(It.IsAny<string>())).ReturnsAsync(CreateRoleAccessDto());
            roleAccessServiceMock.Setup(x => x.SaveRoleAccess(It.IsAny<RoleAccessDto>())).ReturnsAsync(CreateRoleAccessDto());

            roleAccessLinkServiceMock.Setup(x => x.Delete(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(CreateRoleAccessLinkDto());

            var controller = new RoleManageController(roleAccessLinkServiceMock.Object, roleServiceMock.Object, roleAccessServiceMock.Object);

            var result = await controller.RemovePermission("Super Admin", "Permission 2", "Grouping 1");

            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            var roleAccessLinkDto = Assert.IsType<RoleAccessLinkDto>(createdAtActionResult.Value);
            Assert.NotNull(roleAccessLinkDto);
            Assert.Equal("RemovePermission", createdAtActionResult.ActionName);
        }

        [Fact]
        public async Task RemovePermissionHandlesExceptionReturnsBadRequest()
        {
            roleAccessLinkServiceMock.Setup(x => x.Delete(It.IsAny<string>(), It.IsAny<string>())).ThrowsAsync(new Exception("Error removing permission"));

            var roleServiceMock = new Mock<IRoleService>();
            var roleAccessServiceMock = new Mock<IRoleAccessService>();

            var controller = new RoleManageController(roleAccessLinkServiceMock.Object, roleServiceMock.Object, roleAccessServiceMock.Object);

            var result = await controller.RemovePermission("Super Admin", "Permission 1", "Grouping 1");

            var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result);
            var errorMessage = Assert.IsType<string>(badRequestObjectResult.Value);
            Assert.Equal("Error removing permission", errorMessage);
        }

        [Fact]
        public async Task GetRolePermissionsReturnsOk()
        {
            roleAccessLinkServiceMock.Setup(x => x.GetByRole(It.IsAny<string>())).ReturnsAsync(new Dictionary<string, List<string>> { { "Super Admin", new List<string> { "Permission 1", "Permission 2" } } });

            var controller = new RoleManageController(roleAccessLinkServiceMock.Object, null, null);

            var result = await controller.GetRolePermissions("Super Admin");

            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            var roleAccessLinks = Assert.IsType<Dictionary<string, List<string>>>(okObjectResult.Value);
            Assert.NotNull(roleAccessLinks);
        }

        [Fact]
        public async Task GetRolePermissionsHandlesExceptionReturnsNotFound()
        {;
            roleAccessLinkServiceMock.Setup(x => x.GetByRole(It.IsAny<string>())).ThrowsAsync(new Exception("Error getting role permissions"));

            var controller = new RoleManageController(roleAccessLinkServiceMock.Object, null, null);

            var result = await controller.GetRolePermissions("Super Admin");

            var notFoundObjectResult = Assert.IsType<NotFoundObjectResult>(result);
            var errorMessage = Assert.IsType<string>(notFoundObjectResult.Value);
            Assert.Equal("Error getting role permissions", errorMessage);
        }


        [Fact]
        public async Task GetAllRoleAccessLinkReturnsOk()
        {
            roleAccessLinkServiceMock.Setup(x => x.GetAll()).ReturnsAsync(new Dictionary<string, List<string>>
            {
                { "Super Admin", new List<string> { "Permission 1", "Permission 2" } }
            });

            var controller = new RoleManageController(roleAccessLinkServiceMock.Object, null, null);

            var result = await controller.GetAllRoleAccessLink();

            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            var roleAccessLinks = Assert.IsType<Dictionary<string, List<string>>>(okObjectResult.Value);
            Assert.NotNull(roleAccessLinks);
        }

        [Fact]
        public async Task GetAllRoleAccessLinkHandlesExceptionReturnsNotFound()
        {
            roleAccessLinkServiceMock.Setup(x => x.GetAll()).ThrowsAsync(new Exception("Error getting all role access link"));

            var controller = new RoleManageController(roleAccessLinkServiceMock.Object, null, null);

            var result = await controller.GetAllRoleAccessLink();

            var notFoundObjectResult = Assert.IsType<NotFoundObjectResult>(result);
            var errorMessage = Assert.IsType<string>(notFoundObjectResult.Value);
            Assert.Equal("Error getting all role access link", errorMessage);
        }

        [Fact]
        public async Task GetAllRoleAccessLinksReturnsOk()
        {
            roleAccessLinkServiceMock.Setup(x => x.GetAllRoleAccessLink()).ReturnsAsync(new List<RoleAccessLinkDto> { CreateRoleAccessLinkDto() });

            var controller = new RoleManageController(roleAccessLinkServiceMock.Object, null, null);

            var result = await controller.GetAllRoleAccessLinks();

            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            var roleAccessLinks = Assert.IsType<List<RoleAccessLinkDto>>(okObjectResult.Value);
            Assert.NotNull(roleAccessLinks);
        }

        [Fact]
        public async Task GetAllRoleAccessLinksHandlesExceptionReturnsNotFound()
        {
            roleAccessLinkServiceMock.Setup(x => x.GetAllRoleAccessLink()).ThrowsAsync(new Exception("Error getting all role access link"));

            var controller = new RoleManageController(roleAccessLinkServiceMock.Object, null, null);

            var result = await controller.GetAllRoleAccessLinks();

            var notFoundObjectResult = Assert.IsType<NotFoundObjectResult>(result);
            var errorMessage = Assert.IsType<string>(notFoundObjectResult.Value);
            Assert.Equal("Error getting all role access link", errorMessage);
        }

        [Fact]
        public async Task GetAllRoleAccessesReturnsOk()
        {
            roleAccessServiceMock.Setup(x => x.GetAllRoleAccess()).ReturnsAsync(new List<RoleAccessDto> { CreateRoleAccessDto() });

            var controller = new RoleManageController(null, null, roleAccessServiceMock.Object);

            var result = await controller.GetAllRoleAccesses();

            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            var roleAccesses = Assert.IsType<List<RoleAccessDto>>(okObjectResult.Value);
            Assert.NotNull(roleAccesses);
        }

        [Fact]
        public async Task GetAllRoleAccessesHandlesExceptionReturnsNotFound()
        {
            roleAccessServiceMock.Setup(x => x.GetAllRoleAccess()).ThrowsAsync(new Exception("Error getting all role access"));

            var controller = new RoleManageController(null, null, roleAccessServiceMock.Object);

            var result = await controller.GetAllRoleAccesses();

            var notFoundObjectResult = Assert.IsType<NotFoundObjectResult>(result);
            var errorMessage = Assert.IsType<string>(notFoundObjectResult.Value);
            Assert.Equal("Error getting all role access", errorMessage);
        }

        [Fact]
        public async Task GetAllRolesReturnsOk()
        {
            roleServiceMock.Setup(x => x.GetAll()).ReturnsAsync(new List<RoleDto> { CreateRoleDto() });

            var controller = new RoleManageController(null, roleServiceMock.Object, null);

            var result = await controller.GetAllRoles();

            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            var roles = Assert.IsType<List<RoleDto>>(okObjectResult.Value);
            Assert.NotNull(roles);
        }

        [Fact]
        public async Task GetAllRolesHandlesExceptionReturnsNotFound()
        {
            roleServiceMock.Setup(x => x.GetAll()).ThrowsAsync(new Exception("Error getting all roles"));

            var controller = new RoleManageController(null, roleServiceMock.Object, null);

            var result = await controller.GetAllRoles();

            var notFoundObjectResult = Assert.IsType<NotFoundObjectResult>(result);
            var errorMessage = Assert.IsType<string>(notFoundObjectResult.Value);
            Assert.Equal("Error getting all roles", errorMessage);
        }
    }
}
