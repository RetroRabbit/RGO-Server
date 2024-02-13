using Microsoft.AspNetCore.Mvc;
using Moq;
using RGO.App.Controllers;
using RGO.Models;
using RGO.Models.Enums;
using RGO.Services.Interfaces;
using RGO.Tests.Data.Models;
using Xunit;

namespace RGO.App.Tests.Controllers
{
    public class EmployeeEvaluationAudienceControllerUnitTests
    {
        [Fact]
        public async Task GetAllEmployeeEvaluationAudiencesValidInputReturnsOkResult()
        {
            var serviceMock = new Mock<IEmployeeEvaluationAudienceService>();
            var controller = new EmployeeEvaluationAudienceController(serviceMock.Object);

            var expectedAudiences = new List<EmployeeEvaluationAudienceDto>
            {
                new EmployeeEvaluationAudienceDto(
                    1,
                    new EmployeeEvaluationDto(
                        1,
                        EmployeeTestData.EmployeeDto,
                        new EmployeeEvaluationTemplateDto(1, "Employee Evaluation Template 1"),
                        EmployeeTestData.EmployeeDto2,
                        "Employee Evaluation Subject",
                        new DateOnly(2022, 1, 1),
                        new DateOnly(2022, 2, 1)
                    ),
                    EmployeeTestData.EmployeeDto3
                )
            };


            serviceMock.Setup(x => x.GetAllbyEvaluation(It.IsAny<EmployeeEvaluationInput>())).ReturnsAsync(expectedAudiences);

            var result = await controller.GetAll(new EmployeeEvaluationInput(1, "owner@retrorabbit.co.za", "employee@retrorabbit.co.za", "Test Template", "Test Subject"));

            var okResult = Assert.IsType<OkObjectResult>(result);
            var actualAudiences = Assert.IsType<List<EmployeeEvaluationAudienceDto>>(okResult.Value);
            Assert.Equal(expectedAudiences, actualAudiences);
        }

        [Fact]
        public async Task GetAllEmployeeEvaluationAudiencesExceptionThrownReturnsNotFoundResult()
        {
            var serviceMock = new Mock<IEmployeeEvaluationAudienceService>();
            var controller = new EmployeeEvaluationAudienceController(serviceMock.Object);

            serviceMock.Setup(x => x.GetAllbyEvaluation(It.IsAny<EmployeeEvaluationInput>())).ThrowsAsync(new Exception("Error retrieving employee evaluation audiences."));

            var result = await controller.GetAll(new EmployeeEvaluationInput(1, "owner@retrorabbit.co.za", "employee@retrorabbit.co.za", "Test Template", "Test Subject"));

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            var exceptionMessage = Assert.IsType<string>(notFoundResult.Value);

            Assert.Equal("Error retrieving employee evaluation audiences.", exceptionMessage);
        }


        [Fact]
        public async Task SaveEmployeeEvaluationAudienceValidInputReturnsOkResult()
        {
            var serviceMock = new Mock<IEmployeeEvaluationAudienceService>();
            var controller = new EmployeeEvaluationAudienceController(serviceMock.Object);

            var savedAudience = new EmployeeEvaluationAudienceDto
                (1, new EmployeeEvaluationDto(1, EmployeeTestData.EmployeeDto,

                    new EmployeeEvaluationTemplateDto(1, "Employee Evaluation Template 1"),

                    EmployeeTestData.EmployeeDto2,
                        "Employee Evaluation Subject",
                        new DateOnly(2022, 1, 1),
                        new DateOnly(2022, 2, 1)),

                    EmployeeTestData.EmployeeDto3
                    );

            serviceMock.Setup(x => x.Save(It.IsAny<string>(), It.IsAny<EmployeeEvaluationInput>())).ReturnsAsync(savedAudience);

            var result = await controller.SaveEmployeeEvaluationAudience("test@retrorabbit.co.za",
                new EmployeeEvaluationInput(1, "owner@retrorabbit.co.za", "employee@retrorabbit.co.za", "Test Template", "Test Subject"));

            var okResult = Assert.IsType<OkObjectResult>(result);
            var actualSavedAudience = Assert.IsType<EmployeeEvaluationAudienceDto>(okResult.Value);

            Assert.Equal(savedAudience, actualSavedAudience);
        }

        [Fact]
        public async Task SaveEmployeeEvaluationAudienceExceptionThrownReturnsNotFoundResult()
        {
            var serviceMock = new Mock<IEmployeeEvaluationAudienceService>();
            serviceMock.Setup(x => x.Save(It.IsAny<string>(), It.IsAny<EmployeeEvaluationInput>()))
                .ThrowsAsync(new Exception("Exception occurred while saving employee evaluation audience."));
            var controller = new EmployeeEvaluationAudienceController(serviceMock.Object);

            var result = await controller.SaveEmployeeEvaluationAudience("test@retrorabbit.co.za", 
                new EmployeeEvaluationInput(1, "owner@retrorabbit.co.za", "employee@retrorabbit.co.za", "Test Template", "Test Subject"));

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            var exceptionMessage = Assert.IsType<string>(notFoundResult.Value);
            Assert.Equal("Exception occurred while saving employee evaluation audience.", exceptionMessage);
        }

        [Fact]
        public async Task DeleteEmployeeEvaluationAudienceValidInputReturnsOkResult()
        {
            var serviceMock = new Mock<IEmployeeEvaluationAudienceService>();
            serviceMock.Setup(x => x.Delete(It.IsAny<string>(), It.IsAny<EmployeeEvaluationInput>()));
            var controller = new EmployeeEvaluationAudienceController(serviceMock.Object);

            var result = await controller.DeleteEmployeeEvaluationAudience("test@retrorabbit.co.za", 
                new EmployeeEvaluationInput(1, "owner@retrorabbit.co.za", "employee@retrorabbit.co.za", "Test Template", "Test Subject"));

            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task DeleteEmployeeEvaluationAudienceExceptionThrownReturnsNotFoundResult()
        {
            var serviceMock = new Mock<IEmployeeEvaluationAudienceService>();
            serviceMock.Setup(x => x.Delete(It.IsAny<string>(), It.IsAny<EmployeeEvaluationInput>()))
                .ThrowsAsync(new Exception("Exception occurred while deleting employee evaluation audience."));
            var controller = new EmployeeEvaluationAudienceController(serviceMock.Object);

            var result = await controller.DeleteEmployeeEvaluationAudience("test@retrorabbit.co.za", 
                new EmployeeEvaluationInput(1, "owner@retrorabbit.co.za", "employee@retrorabbit.co.za", "Test Template", "Test Subject"));

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            var exceptionMessage = Assert.IsType<string>(notFoundResult.Value);
            Assert.Equal("Exception occurred while deleting employee evaluation audience.", exceptionMessage);
        }
    }
}

