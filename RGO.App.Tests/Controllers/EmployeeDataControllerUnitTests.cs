using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RGO.App.Controllers;
using RGO.Models;
using RGO.Services.Interfaces;
using Xunit;

namespace RGO.App.Tests.Controllers
{
    public class EmployeeDataControllerUnitTests
    {
        [Fact]
        public async Task GetEmployeeDataReturnsOkResult()
        {
            var id = 1;
            var mockEmployeeDataService = new Mock<IEmployeeDataService>();
            mockEmployeeDataService.Setup(service => service.GetAllEmployeeData(id))
                .ReturnsAsync(new List<EmployeeDataDto>
                {
                new EmployeeDataDto(1, 1, 1, "example 1"),
                new EmployeeDataDto(2, 2, 1, "example 1"),
                });

            var controller = new EmployeeDataController(mockEmployeeDataService.Object);

            var result = await controller.GetEmployeeData(id);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsType<List<EmployeeDataDto>>(okResult.Value);

            mockEmployeeDataService.Verify(service => service.GetAllEmployeeData(id), Times.Once);
        }

        [Fact]
        public async Task GetEmployeeDataReturnsNotFoundResult()
        {
            var id = 1;
            var mockEmployeeDataService = new Mock<IEmployeeDataService>();
            mockEmployeeDataService.Setup(service => service.GetAllEmployeeData(id))
                .ReturnsAsync((List<EmployeeDataDto>)null);

            var controller = new EmployeeDataController(mockEmployeeDataService.Object);

            var result = await controller.GetEmployeeData(id);

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            var errorMessage = Assert.IsType<string>(notFoundResult.Value);

            Assert.Equal("Employee data not found", errorMessage);

            mockEmployeeDataService.Verify(service => service.GetAllEmployeeData(id), Times.Once);
        }

        [Fact]
        public async Task SaveEmployeeDataReturnsOkResult()
        {
            var employeeDataDto = new EmployeeDataDto(1, 1, 1, "example 1");
            var mockEmployeeDataService = new Mock<IEmployeeDataService>();
            mockEmployeeDataService.Setup(service => service.SaveEmployeeData(employeeDataDto))
                .ReturnsAsync(employeeDataDto);

            var controller = new EmployeeDataController(mockEmployeeDataService.Object);

            var result = await controller.SaveEmployeeData(employeeDataDto);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsType<EmployeeDataDto>(okResult.Value);

            mockEmployeeDataService.Verify(service => service.SaveEmployeeData(employeeDataDto), Times.Once);
        }

        [Fact]
        public async Task SaveEmployeeDataReturnsNotFoundResultOnException()
        {
            var employeeDataDto = new EmployeeDataDto(1, 1, 1, "example 1");
            var mockEmployeeDataService = new Mock<IEmployeeDataService>();
            mockEmployeeDataService.Setup(service => service.SaveEmployeeData(employeeDataDto))
                .ThrowsAsync(new Exception("Error saving employee data."));

            var controller = new EmployeeDataController(mockEmployeeDataService.Object);

            var result = await controller.SaveEmployeeData(employeeDataDto);

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            var errorMessage = Assert.IsType<string>(notFoundResult.Value);

            Assert.Equal("Error saving employee data.", errorMessage);

            mockEmployeeDataService.Verify(service => service.SaveEmployeeData(employeeDataDto), Times.Once);
        }

        [Fact]
        public async Task UpdateEmployeeDataReturnsOkResult()
        {
            var employeeDataDto = new EmployeeDataDto(1, 1, 1, "example 1");
            var mockEmployeeDataService = new Mock<IEmployeeDataService>();
            mockEmployeeDataService.Setup(service => service.UpdateEmployeeData(employeeDataDto))
                .ReturnsAsync(employeeDataDto);

            var controller = new EmployeeDataController(mockEmployeeDataService.Object);

            var result = await controller.UpdateEmployeeData(employeeDataDto);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsType<EmployeeDataDto>(okResult.Value);

            mockEmployeeDataService.Verify(service => service.UpdateEmployeeData(employeeDataDto), Times.Once);
        }

        [Fact]
        public async Task UpdateEmployeeDataReturnsNotFoundResultOnException()
        {
            var employeeDataDto = new EmployeeDataDto(1, 1, 1, "example 1");
            var mockEmployeeDataService = new Mock<IEmployeeDataService>();
            mockEmployeeDataService.Setup(service => service.UpdateEmployeeData(employeeDataDto))
                .ThrowsAsync(new Exception("Error updating employee data."));

            var controller = new EmployeeDataController(mockEmployeeDataService.Object);

            var result = await controller.UpdateEmployeeData(employeeDataDto);

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            var errorMessage = Assert.IsType<string>(notFoundResult.Value);

            Assert.Equal("Error updating employee data.", errorMessage);

            mockEmployeeDataService.Verify(service => service.UpdateEmployeeData(employeeDataDto), Times.Once);
        }


        [Fact]
        public async Task DeleteEmployeeDataReturnsOkResult()
        {
            var employeeDataDto = new EmployeeDataDto(1, 1, 1, "example 1");
            var mockEmployeeDataService = new Mock<IEmployeeDataService>();
            mockEmployeeDataService.Setup(service => service.DeleteEmployeeData(employeeDataDto.Id))
                .ReturnsAsync(employeeDataDto);

            var controller = new EmployeeDataController(mockEmployeeDataService.Object);

            var result = await controller.DeleteEmployeeData(employeeDataDto.Id);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsType<EmployeeDataDto>(okResult.Value);

            mockEmployeeDataService.Verify(service => service.DeleteEmployeeData(employeeDataDto.Id), Times.Once);
        }

        [Fact]
        public async Task DeleteEmployeeDataReturnsNotFoundResultOnException()
        {
            var employeeDataDto = new EmployeeDataDto(1, 1, 1, "example 1");
            var mockEmployeeDataService = new Mock<IEmployeeDataService>();
            mockEmployeeDataService.Setup(service => service.DeleteEmployeeData(employeeDataDto.Id))
                .ThrowsAsync(new Exception("Error deleting employee data."));

            var controller = new EmployeeDataController(mockEmployeeDataService.Object);

            var result = await controller.DeleteEmployeeData(employeeDataDto.Id);

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            var errorMessage = Assert.IsType<string>(notFoundResult.Value);

            Assert.Equal("Error deleting employee data.", errorMessage);

            mockEmployeeDataService.Verify(service => service.DeleteEmployeeData(employeeDataDto.Id), Times.Once);
        }
    }
}