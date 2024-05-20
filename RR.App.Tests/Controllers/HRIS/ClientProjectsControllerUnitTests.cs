using HRIS.Models;
using HRIS.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RR.App.Controllers.HRIS;
using Xunit;

namespace HRIS.Tests.Controllers;
    public class ClientProjectsControllerUnitTests
    {
        private Mock<IClientProjectService> _mockClientProjectService;
        private ClientProjectsController _controller;

        public ClientProjectsControllerUnitTests()
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
            var clientProjectDto = new ClientProjectsDto { Id = 1 };
            _mockClientProjectService.Setup(service => service.GetClientProject(1))
                .ReturnsAsync(clientProjectDto);

            var result = await _controller.GetClientProject(1);
            var okResult = Assert.IsType<ActionResult<ClientProjectsDto>>(result);
            Assert.NotNull(okResult);
            var objectResult = Assert.IsType<OkObjectResult>(okResult.Result);
            Assert.NotNull(objectResult);
            Assert.Equal(clientProjectDto, objectResult.Value);
        }

        [Fact]
        public async Task DeleteClientProject_WithExistingId_ReturnsNoContent()
        {
            _mockClientProjectService.Setup(service => service.GetClientProject(1))
                .ReturnsAsync(new ClientProjectsDto { Id = 1 });

            var result = await _controller.DeleteClientProject(1);
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task PutClientProject_WithMatchingId_ReturnsNoContent()
        {
            var clientProjectsDto = new ClientProjectsDto { Id = 1 };
            _mockClientProjectService.Setup(service => service.UpdateClientProject(clientProjectsDto))
                .ReturnsAsync(new ClientProjectsDto());

            var result = await _controller.PutClientProject(1, clientProjectsDto);
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task PostClientProject_WithValidInput_ReturnsCreatedAtAction()
        {
    
            var createdClientProjectDto = new ClientProjectsDto { Id = 1 };
            _mockClientProjectService.Setup(service => service.CreateClientProject(createdClientProjectDto))
                .ReturnsAsync(createdClientProjectDto);

            var result = await _controller.PostClientProject(createdClientProjectDto);
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(nameof(_controller.GetClientProject), createdAtActionResult.ActionName);
            Assert.Equal(createdClientProjectDto.Id, createdAtActionResult.RouteValues["id"]);
            Assert.Equal(createdClientProjectDto, createdAtActionResult.Value);
        }

        [Fact]
        public async Task PutClientProject_WithNonMatchingId_ReturnsBadRequest()
        {
            var clientProjectsDto = new ClientProjectsDto { Id = 2 };
            var result = await _controller.PutClientProject(1, clientProjectsDto);
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task PostClientProject_WithInvalidModel_ReturnsBadRequest()
        {
            var clientProjectsDto = new ClientProjectsDto { };
            _controller.ModelState.AddModelError("error", "sample error");

            var result = await _controller.PostClientProject(clientProjectsDto);
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Unable to create the project.", badRequestResult.Value);
        }

        [Fact]
        public async Task GetClientProjectNotFoundResult()
        {
            _mockClientProjectService.Setup(service => service.GetClientProject(99))
                .ReturnsAsync((ClientProjectsDto)null);

            var result = await _controller.GetClientProject(99);
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task UpdateClientProjectReturnsNotFoundResultOnException()
        {
            var clientProjectToUpdate = new ClientProjectsDto { Id = 99 };
            _mockClientProjectService.Setup(service => service.UpdateClientProject(clientProjectToUpdate))
                .ThrowsAsync(new Exception("An error occurred while updating the project."));

            var result = await _controller.PutClientProject(99, clientProjectToUpdate);
            var notFoundResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal("An error occurred while updating the project.", notFoundResult.Value);
        }

        [Fact]
        public async Task ClientProjectReturnsNotFoundResultOnException()
        {
            _mockClientProjectService.Setup(service => service.DeleteClientProject(999))
                .ThrowsAsync(new KeyNotFoundException("Project not found"));

            var result = await _controller.DeleteClientProject(999);
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task GetAllReturnsNotFoundResultWhenExceptionThrown()
        {
            _mockClientProjectService.Setup(service => service.GetAllClientProject())
        .   ThrowsAsync(new Exception("Error occurred while fetching client projects"));

            var result = await _controller.GetAllClientProjects();
            Assert.IsType<NotFoundResult>(result);
        }
    }

