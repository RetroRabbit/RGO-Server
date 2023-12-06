using Microsoft.AspNetCore.Mvc;
using Moq;
using RGO.App.Controllers;
using RGO.Models;
using RGO.Models.Enums;
using RGO.Services.Interfaces;
using Xunit;

namespace RGO.App.Tests.Controllers
{
    public class EmployeeEvaluationAudienceControllerUnitTests
    {
        [Fact]
        public async Task GetAllEmployeeEvaluationAudiencesValidInputReturnsOkResult()
        {
            var serviceMock = new Mock<IEmployeeEvaluationAudienceService>();
            var controller = new EmployeeEvaluationAudienceController(serviceMock.Object, null, null);

            var expectedAudiences = new List<EmployeeEvaluationAudienceDto>
            {
                new EmployeeEvaluationAudienceDto(
                    1,
                    new EmployeeEvaluationDto(
                        1,
                        new EmployeeDto(
                            1, "Emp123", "Tax123", new DateTime(2022, 1, 1), null, 1, false, "No disability", 2,
                            new EmployeeTypeDto(1, "Full Time"), "Notes", 20.0f, 15.0f, 50.0f, 50000, "John Doe", "JD", "Doe", new DateTime(1990, 1, 1),
                            "South Africa", "South African", "123456789", "AB123456", new DateTime(2025, 1, 1), "South Africa", Race.White, Gender.Male, "photo.jpg",
                            "test@retrorabbit.co.za", "john.doe.personal@example.com", "1234567890", 1, 1,
                            new EmployeeAddressDto(
                                1, "Unit 1", "Complex A", "123", "Suburb", "City", "Country", "Province", "12345"),
                            new EmployeeAddressDto(
                                2, "P.O. Box 123", "", "456", "Suburb", "City", "Country", "Province", "54321"),
                            "12",
                            "Emergency Contact",
                            "987654321"
                        ),
                        new EmployeeEvaluationTemplateDto(1, "Employee Evaluation Template 1"),
                        new EmployeeDto(
                            2, "Emp124", "Tax124", new DateTime(2022, 1, 1), null, 1, false, "No disability", 2,
                            new EmployeeTypeDto(1, "Full Time"), "Notes", 20.0f, 15.0f, 50.0f, 50000, "John Doe", "JD", "Doe", new DateTime(1990, 1, 1),
                            "South Africa", "South African", "123456789", "AB123456", new DateTime(2025, 1, 1), "South Africa", Race.White, Gender.Male, "photo.jpg",
                            "john.doe@example.com", "john.doe.personal@example.com", "1234567890", 1, 1,
                            new EmployeeAddressDto(
                                1, "Unit 1", "Complex A", "123", "Suburb", "City", "Country", "Province", "12345"),
                            new EmployeeAddressDto(
                                2, "P.O. Box 123", "", "456", "Suburb", "City", "Country", "Province", "54321"),
                            "12",
                            "Emergency Contact",
                            "987654321"
                        ),
                        "Employee Evaluation Subject",
                        new DateOnly(2022, 1, 1),
                        new DateOnly(2022, 2, 1)
                    ),
                    new EmployeeDto(
                        2, "Emp124", "Tax124", new DateTime(2022, 1, 1), null, 1, false, "No disability", 2,
                        new EmployeeTypeDto(1, "Full Time"), "Notes", 20.0f, 15.0f, 50.0f, 50000, "John Doe", "JD", "Doe", new DateTime(1990, 1, 1),
                        "South Africa", "South African", "123456789", "AB123456", new DateTime(2025, 1, 1), "South Africa", Race.White, Gender.Male, "photo.jpg",
                        "john.doe@example.com", "john.doe.personal@example.com", "1234567890", 1, 1,
                        new EmployeeAddressDto(
                            1, "Unit 1", "Complex A", "123", "Suburb", "City", "Country", "Province", "12345"),
                        new EmployeeAddressDto(
                            2, "P.O. Box 123", "", "456", "Suburb", "City", "Country", "Province", "54321"),
                        "12",
                        "Emergency Contact",
                        "987654321"
                    )
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
            var controller = new EmployeeEvaluationAudienceController(serviceMock.Object, null, null);

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
            var controller = new EmployeeEvaluationAudienceController(serviceMock.Object, null, null);

            var savedAudience = new EmployeeEvaluationAudienceDto
                (1, new EmployeeEvaluationDto(1, new EmployeeDto
                    (1, "Emp123", "Tax123", new DateTime(2022, 1, 1), null, 1, false, "No disability", 2,
                        new EmployeeTypeDto(1, "Full Time"), "Notes", 20.0f, 15.0f, 50.0f, 50000, "John Doe", "JD", "Doe", new DateTime(1990, 1, 1),
                        "South Africa", "South African", "123456789", "AB123456", new DateTime(2025, 1, 1), "South Africa", Race.White, Gender.Male, "photo.jpg",
                        "test@retrorabbit.co.za", "john.doe.personal@example.com", "1234567890", 1, 1,
                        new EmployeeAddressDto
                        (1, "Unit 1", "Complex A", "123", "Suburb", "City", "Country", "Province", "12345"),
                        new EmployeeAddressDto
                        (2, "P.O. Box 123", "", "456", "Suburb", "City", "Country", "Province", "54321"),
                        "12",
                        "Emergency Contact",
                        "987654321"),

                    new EmployeeEvaluationTemplateDto(1, "Employee Evaluation Template 1"),

                    new EmployeeDto
                    (2, "Emp124", "Tax124", new DateTime(2022, 1, 1), null, 1, false, "No disability", 2,
                        new EmployeeTypeDto(1, "Full Time"), "Notes", 20.0f, 15.0f, 50.0f, 50000, "John Doe", "JD", "Doe", new DateTime(1990, 1, 1),
                        "South Africa", "South African", "123456789", "AB123456", new DateTime(2025, 1, 1), "South Africa", Race.White, Gender.Male, "photo.jpg",
                        "john.doe@example.com", "john.doe.personal@example.com", "1234567890", 1, 1,
                        new EmployeeAddressDto
                        (1, "Unit 1", "Complex A", "123", "Suburb", "City", "Country", "Province", "12345"),
                        new EmployeeAddressDto
                        (2, "P.O. Box 123", "", "456", "Suburb", "City", "Country", "Province", "54321"),
                        "12",
                        "Emergency Contact",
                        "987654321"
                        ),
                        "Employee Evaluation Subject",
                        new DateOnly(2022, 1, 1),
                        new DateOnly(2022, 2, 1)),

                    new EmployeeDto
                    (2, "Emp124", "Tax124", new DateTime(2022, 1, 1), null, 1, false, "No disability", 2,
                        new EmployeeTypeDto(1, "Full Time"), "Notes", 20.0f, 15.0f, 50.0f, 50000, "John Doe", "JD", "Doe", new DateTime(1990, 1, 1),
                        "South Africa", "South African", "123456789", "AB123456", new DateTime(2025, 1, 1), "South Africa", Race.White, Gender.Male, "photo.jpg",
                        "john.doe@example.com", "john.doe.personal@example.com", "1234567890", 1, 1,
                        new EmployeeAddressDto
                        (1, "Unit 1", "Complex A", "123", "Suburb", "City", "Country", "Province", "12345"),
                        new EmployeeAddressDto
                        (2, "P.O. Box 123", "", "456", "Suburb", "City", "Country", "Province", "54321"),
                        "12",
                        "Emergency Contact",
                        "987654321"
                        )
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
            var controller = new EmployeeEvaluationAudienceController(serviceMock.Object, null, null);

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
            var controller = new EmployeeEvaluationAudienceController(serviceMock.Object, null, null);

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
            var controller = new EmployeeEvaluationAudienceController(serviceMock.Object, null, null);

            var result = await controller.DeleteEmployeeEvaluationAudience("test@retrorabbit.co.za", 
                new EmployeeEvaluationInput(1, "owner@retrorabbit.co.za", "employee@retrorabbit.co.za", "Test Template", "Test Subject"));

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            var exceptionMessage = Assert.IsType<string>(notFoundResult.Value);
            Assert.Equal("Exception occurred while deleting employee evaluation audience.", exceptionMessage);
        }
    }
}

