using HRIS.Models;
using HRIS.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RR.App.Controllers.HRIS;
using RR.UnitOfWork.Entities.HRIS;
using Xunit;

namespace HRIS.Tests.Controllers
{
    public class ClientProjectsControllerTests
    {
        private Mock<IClientProjectService> _mockClientProjectService;
        private ClientProjectsController _controller;

        public ClientProjectsControllerTests()
        {
            _mockClientProjectService = new Mock<IClientProjectService>();
            _controller = new ClientProjectsController(_mockClientProjectService.Object);
        }

        [Fact]
        public async Task GetAllClientProjects_ReturnsOkResult()
        {
            var expectedClientProjects = new List<ClientProjectsDto>();
            _mockClientProjectService.Setup(service => service.GetAllClientProject())
                .ReturnsAsync(expectedClientProjects);

            var result = await _controller.GetAllClientProjects();
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult);
            Assert.Equal(expectedClientProjects, okResult.Value);
        }

        [Fact]
        public async Task GetClientProject_WithValidId_ReturnsOkResult()
        {
            var id = 1;
            var clientProjectDto = new ClientProjectsDto { Id = id };
            _mockClientProjectService.Setup(service => service.GetClientProject(id))
                .ReturnsAsync(clientProjectDto);

            var result = await _controller.GetClientProject(id);
            var okResult = Assert.IsType<ActionResult<ClientProjectsDto>>(result);
            Assert.NotNull(okResult);
            var objectResult = Assert.IsType<OkObjectResult>(okResult.Result);
            Assert.NotNull(objectResult);
            Assert.Equal(clientProjectDto, objectResult.Value);
        }

        [Fact]
        public async Task DeleteClientProject_WithExistingId_ReturnsNoContent()
        {
            var id = 1;
            _mockClientProjectService.Setup(service => service.GetClientProject(id))
                .ReturnsAsync(new ClientProjectsDto { Id = id });

            var result = await _controller.DeleteClientProject(id);
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task PutClientProject_WithMatchingId_ReturnsNoContent()
        {
            var id = 1;
            var clientProject = new ClientProject { Id = id };
            _mockClientProjectService.Setup(service => service.UpdateClientProject(clientProject))
                .ReturnsAsync(new ClientProjectsDto());

            var result = await _controller.PutClientProject(id, clientProject);
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task PostClientProject_WithValidInput_ReturnsCreatedAtAction()
        {
            var clientProject = new ClientProject
            {
                Id = 1,
            };
            var createdClientProjectDto = new ClientProjectsDto { Id = clientProject.Id };
            _mockClientProjectService.Setup(service => service.CreateClientProject(clientProject))
                .ReturnsAsync(createdClientProjectDto);

            var result = await _controller.PostClientProject(clientProject);
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(nameof(_controller.GetClientProject), createdAtActionResult.ActionName);
            Assert.Equal(clientProject.Id, createdAtActionResult.RouteValues["id"]);
            Assert.Equal(createdClientProjectDto, createdAtActionResult.Value);
        }
    }
}
