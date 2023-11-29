using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RGO.App.Controllers;
using RGO.Models;
using RGO.Models.Enums;
using RGO.Services.Interfaces;
using RGO.UnitOfWork.Entities;
using Xunit;


namespace RGO.App.Tests.Controllers
{

    public class EmployeeEvaluationControllerUnitTests
    {
        [Fact]
        public async Task GetAllEmployeeEvaluationsValidEmailReturnsOkResultWithEvaluations()
        {
            var mockService = new Mock<IEmployeeEvaluationService>();
            var controller = new EmployeeEvaluationController(mockService.Object);

            var email = "test@retrorabbit.co.za";
            var expectedEvaluations = new List<EmployeeEvaluationDto>
            {
                new EmployeeEvaluationDto
                (
                    1,
                    new EmployeeDto
                    (1, "Emp123", "Tax123", new DateOnly(2022, 1, 1), null, 1, false, "No disability", 2,
                        new EmployeeTypeDto(1, "Full Time"), "Notes", 20.0f, 15.0f, 50.0f, 50000, "John Doe", "JD", "Doe", new DateOnly(1990, 1, 1),
                        "South Africa", "South African", "123456789", "AB123456", new DateOnly(2025, 1, 1), "South Africa", Race.White, Gender.Male, "photo.jpg",
                        "john.doe@example.com", "john.doe.personal@example.com", "1234567890", 1, 1,
                        new EmployeeAddressDto
                        (1, "Unit 1", "Complex A", "123", "Suburb", "City", "Country", "Province", "12345"),
                        new EmployeeAddressDto
                        (2, "P.O. Box 123", "", "456", "Suburb", "City", "Country", "Province", "54321"),
                        "12",
                        "Emergency Contact",
                        "987654321"
                    ),
                    new EmployeeEvaluationTemplateDto(1, "Template 1"),
                    new EmployeeDto
                    (1, "Emp123", "Tax123", new DateOnly(2022, 1, 1), null, 1, false, "No disability", 2,
                        new EmployeeTypeDto(1, "Full Time"), "Notes", 20.0f, 15.0f, 50.0f, 50000, "John Doe", "JD", "Doe", new DateOnly(1990, 1, 1),
                        "South Africa", "South African", "123456789", "AB123456", new DateOnly(2025, 1, 1), "South Africa", Race.White, Gender.Male, "photo.jpg",
                        "john.doe@example.com", "john.doe.personal@example.com", "1234567890", 1, 1,
                        new EmployeeAddressDto
                        (1, "Unit 1", "Complex A", "123", "Suburb", "City", "Country", "Province", "12345"),
                        new EmployeeAddressDto
                        (2, "P.O. Box 123", "", "456", "Suburb", "City", "Country", "Province", "54321"),
                        "12",
                        "Emergency Contact",
                        "987654321"
                        ),
                    "Evaluation Subject",
                    new DateOnly(2022, 1, 1),
                    new DateOnly(2022, 2, 1)
                    )
            };

            mockService.Setup(x => x.GetAllEvaluationsByEmail(email)).ReturnsAsync(expectedEvaluations);

            var result = await controller.GetAllEmployeeEvaluations(email);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var actualEvaluations = Assert.IsType<List<EmployeeEvaluationDto>>(okResult.Value);

            Assert.Equal(expectedEvaluations.Count, actualEvaluations.Count);
        }

        [Fact]
        public async Task GetAllEmployeeEvaluationsExceptionThrownReturnsNotFoundResultWithErrorMessage()
        {
            var mockService = new Mock<IEmployeeEvaluationService>();
            var controller = new EmployeeEvaluationController(mockService.Object);
            var email = "test@retrorabbit.co.za";
            var errorMessage = "Error retrieving employee evaluations";

            mockService.Setup(x => x.GetAllEvaluationsByEmail(email))
                .ThrowsAsync(new Exception(errorMessage));

            var result = await controller.GetAllEmployeeEvaluations(email);

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            var actualErrorMessage = Assert.IsType<string>(notFoundResult.Value);

            Assert.Equal(errorMessage, actualErrorMessage);
        }

        //[Fact]
        //public async Task GetEmployeeEvaluationValidParametersReturnsOkResultWithEvaluation()
        //{

        //}

        //[Fact]
        //public async Task GetEmployeeEvaluationInvalidParametersReturnsNotFoundResultWithErrorMessage()
        //{

        //}

        //[Fact]
        //public async Task SaveEmployeeEvaluationValidInputReturnsOkResultWithSavedEvaluation()
        //{

        //}

        //[Fact]
        //public async Task SaveEmployeeEvaluationInvalidInputReturnsNotFoundResultWithErrorMessage()
        //{

        //}

        //[Fact]
        //public async Task UpdateEmployeeEvaluationValidInputReturnsOkResult()
        //{

        //}

        //[Fact]
        //public async Task UpdateEmployeeEvaluationInvalidInputReturnsNotFoundResultWithErrorMessage()
        //{

        //}

        //[Fact]
        //public async Task DeleteEmployeeEvaluationValidInputReturnsOkResult()
        //{

        //}

        //[Fact]
        //public async Task DeleteEmployeeEvaluationInvalidInputReturnsNotFoundResultWithErrorMessage()
        //{

        //}
    }
}
