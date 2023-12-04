using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RGO.App.Controllers;
using RGO.Models;
using RGO.Services.Interfaces;
using System.Security.Claims;
using Xunit;

namespace RGO.App.Tests.Controllers
{
    public class EmployeeEvaluationTemplateControllerUnitTests
    {
        [Fact]
        public async Task GetEmployeeEvaluationTemplateValidInputReturnsOkResult()
        {
            var templateName = "Template 1";
            var expectedTemplate = new EmployeeEvaluationTemplateDto
            (1, "Test Template Description");

            var mockService = new Mock<IEmployeeEvaluationTemplateService>();
            mockService.Setup(x => x.Get(templateName)).ReturnsAsync(expectedTemplate);

            var controller = new EmployeeEvaluationTemplateController(mockService.Object);

            var result = await controller.GetEmployeeEvaluationTemplate(templateName);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var actualTemplate = Assert.IsType<EmployeeEvaluationTemplateDto>(okResult.Value);

            Assert.Equal(expectedTemplate.Id, actualTemplate.Id);
            Assert.Equal(expectedTemplate.Description, actualTemplate.Description);
        }

        [Fact]
        public async Task GetEmployeeEvaluationTemplateExceptionThrownReturnsNotFoundResultWithErrorMessage()
        {
            var templateName = "Template 1";
            var errorMessage = "Error retrieving employee evaluation template";

            var mockService = new Mock<IEmployeeEvaluationTemplateService>();
            mockService.Setup(x => x.Get(templateName)).ThrowsAsync(new Exception(errorMessage));

            var controller = new EmployeeEvaluationTemplateController(mockService.Object);

            var result = await controller.GetEmployeeEvaluationTemplate(templateName);


            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            var actualErrorMessage = Assert.IsType<string>(notFoundResult.Value);

            Assert.Equal(errorMessage, actualErrorMessage);
        }

        [Fact]
        public async Task GetAllEmployeeEvaluationTemplatesValidInputReturnsOkResult()
        {
            var expectedTemplates = new List<EmployeeEvaluationTemplateDto>
            {
                new EmployeeEvaluationTemplateDto(1, "Template 1"),
                new EmployeeEvaluationTemplateDto(2, "Template 2"),
                new EmployeeEvaluationTemplateDto(2, "Template 3")
            };

            var mockService = new Mock<IEmployeeEvaluationTemplateService>();
            mockService.Setup(x => x.GetAll()).ReturnsAsync(expectedTemplates);

            var controller = new EmployeeEvaluationTemplateController(mockService.Object);

            var result = await controller.GetAllEmployeeEvaluationTemplates();

            var okResult = Assert.IsType<OkObjectResult>(result);
            var actualTemplates = Assert.IsType<List<EmployeeEvaluationTemplateDto>>(okResult.Value);

            Assert.Equal(expectedTemplates.Count, actualTemplates.Count);
        }

        [Fact]
        public async Task GetAllEmployeeEvaluationTemplatesExceptionThrownReturnsNotFoundResultWithErrorMessage()
        {
            var mockService = new Mock<IEmployeeEvaluationTemplateService>();
            var controller = new EmployeeEvaluationTemplateController(mockService.Object);
            var errorMessage = "Error retrieving employee evaluation templates";

            mockService.Setup(x => x.GetAll()).ThrowsAsync(new Exception(errorMessage));

            var result = await controller.GetAllEmployeeEvaluationTemplates();

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            var actualErrorMessage = Assert.IsType<string>(notFoundResult.Value);

            Assert.Equal(errorMessage, actualErrorMessage);
        }

        [Fact]
        public async Task SaveEmployeeEvaluationTemplateValidInputReturnsOkResult()
        {
            var mockService = new Mock<IEmployeeEvaluationTemplateService>();
            var controller = new EmployeeEvaluationTemplateController(mockService.Object);

            var templateName = "Template Test 123";

            mockService.Setup(x => x.Save(templateName))
                .ReturnsAsync(new EmployeeEvaluationTemplateDto
                (
                    1,
                    "Template Test 123"
                ));

            var result = await controller.SaveEmployeeEvaluationTemplate(templateName);

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task SaveEmployeeEvaluationTemplateExceptionThrownReturnsNotFoundResultWithErrorMessage()
        {
            var mockService = new Mock<IEmployeeEvaluationTemplateService>();
            var controller = new EmployeeEvaluationTemplateController(mockService.Object);

            var templateName = "Template 1";

            mockService.Setup(x => x.Save(templateName))
                .ThrowsAsync(new Exception("Error saving evaluation template"));

            var result = await controller.SaveEmployeeEvaluationTemplate(templateName);

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            var actualErrorMessage = Assert.IsType<string>(notFoundResult.Value);

            Assert.Equal("Error saving evaluation template", actualErrorMessage);
        }

        [Fact]
        public async Task UpdateEmployeeEvaluationTemplateValidInputReturnsOkResult()
        {
            var mockService = new Mock<IEmployeeEvaluationTemplateService>();
            var controller = new EmployeeEvaluationTemplateController(mockService.Object);

            var templateDto = new EmployeeEvaluationTemplateDto
            (1, "Updated Template");

            mockService.Setup(x => x.Update(It.IsAny<EmployeeEvaluationTemplateDto>()))
                .ReturnsAsync(templateDto);

            var result = await controller.UpdateEmployeeEvaluationTemplate(templateDto);

            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task UpdateEmployeeEvaluationTemplateExceptionThrownReturnsNotFoundResultWithErrorMessage()
        {
            var mockService = new Mock<IEmployeeEvaluationTemplateService>();
            var controller = new EmployeeEvaluationTemplateController(mockService.Object);

            var templateDto = new EmployeeEvaluationTemplateDto
            (1, "Updated Template");

            mockService.Setup(x => x.Update(It.IsAny<EmployeeEvaluationTemplateDto>()))
                .ThrowsAsync(new Exception("Error updating evaluation template")); 

            var result = await controller.UpdateEmployeeEvaluationTemplate(templateDto);

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            var actualErrorMessage = Assert.IsType<string>(notFoundResult.Value);

            Assert.Equal("Error updating evaluation template", actualErrorMessage);
        }

        [Fact]
        public async Task DeleteEmployeeEvaluationTemplateValidInputReturnsOkResult()
        {
            var mockService = new Mock<IEmployeeEvaluationTemplateService>();
            var controller = new EmployeeEvaluationTemplateController(mockService.Object);

            var template = "Template To Delete";

            mockService.Setup(x => x.Delete(template)).ReturnsAsync(new EmployeeEvaluationTemplateDto
            (1, "Deleted Template"));

            var result = await controller.DeleteEmployeeEvaluationTemplate(template);

            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task DeleteEmployeeEvaluationTemplateExceptionThrownReturnsNotFoundResultWithErrorMessage()
        {
            var mockService = new Mock<IEmployeeEvaluationTemplateService>();
            var controller = new EmployeeEvaluationTemplateController(mockService.Object);

            var template = "Template To Delete";
            var errorMessage = "Error deleting evaluation template";

            mockService.Setup(x => x.Delete(template)).ThrowsAsync(new Exception(errorMessage));

            var result = await controller.DeleteEmployeeEvaluationTemplate(template);

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            var actualErrorMessage = Assert.IsType<string>(notFoundResult.Value);

            Assert.Equal(errorMessage, actualErrorMessage);
        }
    }
}
