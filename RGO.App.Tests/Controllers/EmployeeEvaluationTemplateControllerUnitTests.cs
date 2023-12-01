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
            var templateName = "Template1";
            var expectedTemplate = new EmployeeEvaluationTemplateDto
            (
                1,
                "Test Template Description"
            );

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
        }

        [Fact]
        public async Task GetAllEmployeeEvaluationTemplatesValidInputReturnsOkResult()
        {
        }

        [Fact]
        public async Task GetAllEmployeeEvaluationTemplatesExceptionThrownReturnsNotFoundResultWithErrorMessage()
        {
        }

        [Fact]
        public async Task SaveEmployeeEvaluationTemplateValidInputReturnsOkResult()
        {
        }

        [Fact]
        public async Task SaveEmployeeEvaluationTemplateExceptionThrownReturnsNotFoundResultWithErrorMessage()
        {
        }

        [Fact]
        public async Task UpdateEmployeeEvaluationTemplateValidInputReturnsOkResult()
        {
        }

        [Fact]
        public async Task UpdateEmployeeEvaluationTemplateExceptionThrownReturnsNotFoundResultWithErrorMessage()
        {
        }

        [Fact]
        public async Task DeleteEmployeeEvaluationTemplateValidInputReturnsOkResult()
        {
        }

        [Fact]
        public async Task DeleteEmployeeEvaluationTemplateExceptionThrownReturnsNotFoundResultWithErrorMessage()
        {
        }
    }
}
