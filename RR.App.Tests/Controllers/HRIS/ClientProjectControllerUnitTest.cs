using HRIS.Models;
using HRIS.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RR.App.Controllers.HRIS;
using Xunit;

namespace RR.App.Tests.Controllers.HRIS;
    public class ClientProjectControllerUnitTest
    {
        private readonly ClientProjectsController _controller;
        private readonly Mock<IClientProjectService> _mockClientProjectService;

        public ClientProjectControllerUnitTest()
        {
          _mockClientProjectService = new Mock<IClientProjectService>();
          _controller = new ClientProjectsController(_mockClientProjectService.Object);

        }

        [Fact]
        public async Task GetAllClientProjects_ReturnsOkResult_WithListOfClientProjects()
        {
            var clientProjects = new List<ClientProjectsDto> 
            { new ClientProjectsDto
               { 
                  Id = 1,
                  EmployeeId = 1,
                  ClientName = "ClientName1",
                  ProjectName = "ProjectName1",
                  StartDate = DateTime.Now,
                  EndDate = DateTime.Now,
                  ProjectURL = "URL"
               }
            };

              _mockClientProjectService.Setup(ex => ex.GetAllClientProjects())
                .ReturnsAsync(clientProjects);

             var result = await _controller.GetAllClientProjects();

             var okResult = Assert.IsType<OkObjectResult>(result);
             Assert.Equal(clientProjects, okResult.Value);
        }

        [Fact]
        public async Task GetAllClientProjects_ReturnsNotFound_WhenExceptionIsThrown()
        {
          _mockClientProjectService.Setup(Exception => Exception.GetAllClientProjects())
            .ThrowsAsync(new Exception("Test exception"));

          var result = await _controller.GetAllClientProjects();

          var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
          Assert.Equal("Test exception", notFoundResult.Value);
        }

        [Fact]
        public async Task GetClientProjectById_ReturnsOkResult_WithClientProjectsDto()
        {
          var clientProjectDto = new ClientProjectsDto { Id = 1, 
                                                         EmployeeId = 1, 
                                                         ClientName = "ClientName2",  
                                                         ProjectName = "Project2", 
                                                         StartDate = DateTime.Now,
                                                         EndDate = DateTime.Now, 
                                                         ProjectURL = "URL"
          };

           _mockClientProjectService.Setup(ex => ex.GetClientProjectById(1))
             .ReturnsAsync(clientProjectDto);

           var result = await _controller.GetClientProjectById(1);

           var okResult = result.Result as OkObjectResult;
           Assert.NotNull(okResult);
           var returnValue = Assert.IsType<ClientProjectsDto>(okResult.Value);
           Assert.Equal(clientProjectDto, returnValue);
        }

        [Fact]
        public async Task GetClientProjectById_ReturnsNotFound_WhenExceptionIsThrown()
        {
          _mockClientProjectService.Setup(ex => ex.GetClientProjectById(1))
          .ThrowsAsync(new Exception("Test exception"));

          var result = await _controller.GetClientProjectById(1);

          var notFoundResult = result.Result as NotFoundObjectResult;
          Assert.NotNull(notFoundResult);
          Assert.Equal("Test exception", notFoundResult.Value);
        }

        [Fact]
        public async Task SaveClientProject_ReturnsOkResult_WithCreatedClientProject()
        {
            var clientProjectsDto = new ClientProjectsDto { Id = 1, 
                                                            EmployeeId = 1, 
                                                            ClientName = "New Project", 
                                                            ProjectName = "Project2", 
                                                            StartDate = DateTime.Now, 
                                                            EndDate = DateTime.Now,
                                                            ProjectURL = "URL" 
            };

            _mockClientProjectService.Setup(ex => ex.CreateClientProject(clientProjectsDto))
             .ReturnsAsync(clientProjectsDto);

            var result = await _controller.SaveClientProject(clientProjectsDto);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<ClientProjectsDto>(okResult.Value);
            Assert.Equal(clientProjectsDto, returnValue);
        }

        [Fact]
        public async Task SaveClientProject_ReturnsNotFound_WhenExceptionIsThrown()
        {
           var clientProjectsDto = new ClientProjectsDto { Id = 1,
                                                           EmployeeId = 1,
                                                           ClientName = "New Project", 
                                                           ProjectName = "Project2",
                                                           StartDate = DateTime.Now,
                                                           EndDate = DateTime.Now, 
                                                           ProjectURL = "URL" 
           };

             _mockClientProjectService.Setup(ex => ex.CreateClientProject(clientProjectsDto))
             .ThrowsAsync(new Exception("Test exception"));

             var result = await _controller.SaveClientProject(clientProjectsDto);

           var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
           Assert.Equal("Test exception", notFoundResult.Value);
        }

        [Fact]
        public async Task UpdateClientProject_ReturnsOkResult_WithUpdatedClientProject()
        {
           var clientProjectsDto = new ClientProjectsDto { Id = 1, 
                                                           EmployeeId = 1, 
                                                           ClientName = "Updated Project", 
                                                           ProjectName = "Project2",  
                                                           StartDate = DateTime.Now, 
                                                           EndDate = DateTime.Now, 
                                                           ProjectURL = "URL"
           };

            _mockClientProjectService.Setup(ex => ex.UpdateClientProject(clientProjectsDto))
             .ReturnsAsync(clientProjectsDto);

            var result = await _controller.UpdateClientProject(clientProjectsDto);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<ClientProjectsDto>(okResult.Value);
            Assert.Equal(clientProjectsDto, returnValue);
        }

        [Fact]
        public async Task UpdateClientProject_ReturnsNotFound_WhenExceptionIsThrown()
        {
            var clientProjectsDto = new ClientProjectsDto { Id = 1, 
                                                            EmployeeId = 1,
                                                            ClientName = "Updated Project", 
                                                            ProjectName = "Project2", 
                                                            StartDate = DateTime.Now, 
                                                            EndDate = DateTime.Now,
                                                            ProjectURL = "URL" 
            };

             _mockClientProjectService.Setup(ex => ex.UpdateClientProject(clientProjectsDto))
              .ThrowsAsync(new Exception("Test exception"));

            var result = await _controller.UpdateClientProject(clientProjectsDto);

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Test exception", notFoundResult.Value);
        }

        [Fact]
        public async Task DeleteClientProject_ReturnsOkResult_WhenProjectIsDeleted()
        {

            var clientProjectsDto = new ClientProjectsDto { Id = 1, 
                                                            EmployeeId = 1,
                                                            ClientName = "Project to Delete",
                                                            ProjectName = "Project2", 
                                                            StartDate = DateTime.Now, 
                                                            EndDate = DateTime.Now, 
                                                            ProjectURL = "URL"
            };

             _mockClientProjectService.Setup(service => service.GetClientProjectById(1))
              .ReturnsAsync(clientProjectsDto);
             _mockClientProjectService.Setup(ex => ex.DeleteClientProject(1))
              .ReturnsAsync(clientProjectsDto);
 
            var result = await _controller.DeleteClientProject(1);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<ClientProjectsDto>(okResult.Value);
            Assert.Equal(clientProjectsDto, returnValue);
        }

        [Fact]
        public async Task DeleteClientProject_ReturnsNotFound_WhenProjectDoesNotExist()
        {
           _mockClientProjectService.Setup(ex => ex.GetClientProjectById(1))
             .ReturnsAsync((ClientProjectsDto)null);

           var result = await _controller.DeleteClientProject(1);

           var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
           Assert.Equal("Client Project not found", notFoundResult.Value);
        }
    }
