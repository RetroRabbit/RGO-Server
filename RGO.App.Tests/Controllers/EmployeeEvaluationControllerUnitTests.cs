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
                        "test@retrorabbit.co.za", "john.doe.personal@example.com", "1234567890", 1, 1,
                        new EmployeeAddressDto
                        (1, "Unit 1", "Complex A", "123", "Suburb", "City", "Country", "Province", "12345"),
                        new EmployeeAddressDto
                        (2, "P.O. Box 123", "", "456", "Suburb", "City", "Country", "Province", "54321"),
                        "12",
                        "Emergency Contact",
                        "987654321"
                    ),
                    new EmployeeEvaluationTemplateDto(1, "Employee Evaluation Template 1"),
                    new EmployeeDto
                    (2, "Emp124", "Tax124", new DateOnly(2022, 1, 1), null, 1, false, "No disability", 2,
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
                    "Employee Evaluation Subject",
                    new DateOnly(2022, 1, 1),
                    new DateOnly(2022, 2, 1)
                ),
            };

            var mockService = new Mock<IEmployeeEvaluationService>();
            mockService.Setup(x => x.GetAllEvaluationsByEmail(email)).ReturnsAsync(expectedEvaluations);

            var controller = new EmployeeEvaluationController(mockService.Object);

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

        [Fact]
        public async Task GetEmployeeEvaluation_ValidParameters_ReturnsOkResultWithEvaluation()
        {
            var employeeEmail = "test.employee@example.com";
            var ownerEmail = "test.owner@example.com";
            var template = "SampleTemplate";
            var subject = "SampleSubject";

            var expectedEvaluation = new EmployeeEvaluationDto
            (
                1,
                new EmployeeDto
                    (1, "Emp123", "Tax123", new DateOnly(2022, 1, 1), null, 1, false, "No disability", 2,
                        new EmployeeTypeDto(1, "Full Time"), "Notes", 20.0f, 15.0f, 50.0f, 50000, "John Doe", "JD", "Doe", new DateOnly(1990, 1, 1),
                        "South Africa", "South African", "123456789", "AB123456", new DateOnly(2025, 1, 1), "South Africa", Race.White, Gender.Male, "photo.jpg",
                        "test@retrorabbit.co.za", "john.doe.personal@example.com", "1234567890", 1, 1,
                        new EmployeeAddressDto
                        (1, "Unit 1", "Complex A", "123", "Suburb", "City", "Country", "Province", "12345"),
                        new EmployeeAddressDto
                        (2, "P.O. Box 123", "", "456", "Suburb", "City", "Country", "Province", "54321"),
                        "12",
                        "Emergency Contact",
                        "987654321"
                    ),
                new EmployeeEvaluationTemplateDto
                (
                    1,
                    "Sample Description"
                ),
                new EmployeeDto
                    (2, "Emp124", "Tax124", new DateOnly(2022, 1, 1), null, 1, false, "No disability", 2,
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
                "Employee Evaluation Subject",
                new DateOnly(2022, 1, 1),
                new DateOnly(2022, 2, 1)
            );

            var mockService = new Mock<IEmployeeEvaluationService>();
            mockService.Setup(x => x.Get(employeeEmail, ownerEmail, template, subject)).ReturnsAsync(expectedEvaluation);

            var controller = new EmployeeEvaluationController(mockService.Object);

            var result = await controller.GetEmployeeEvaluation(employeeEmail, ownerEmail, template, subject);


            var okResult = Assert.IsType<OkObjectResult>(result);
            var actualEvaluation = Assert.IsType<EmployeeEvaluationDto>(okResult.Value);


            Assert.Equal(expectedEvaluation.Id, actualEvaluation.Id);
            Assert.Equal(expectedEvaluation.Subject, actualEvaluation.Subject);
            Assert.Equal(expectedEvaluation.StartDate, actualEvaluation.StartDate);
            Assert.Equal(expectedEvaluation.EndDate, actualEvaluation.EndDate);


            Assert.Equal(expectedEvaluation.Employee.Id, actualEvaluation.Employee.Id);
            Assert.Equal(expectedEvaluation.Employee.EmployeeNumber, actualEvaluation.Employee.EmployeeNumber);
        }

        [Fact]
        public async Task GetEmployeeEvaluationInvalidParametersReturnsNotFoundResultWithErrorMessage()
        {
            var employeeEmail = "test.employee@retrorabbit.co.za";
            var ownerEmail = "test.owner@retrorabbit.co.za";
            var template = "Employee Evaluation Teamplte";
            var subject = "Employee Evaluation Subject";
            var errorMessage = "Error retrieving employee evaluation";

            var mockService = new Mock<IEmployeeEvaluationService>();
            mockService.Setup(x => x.Get(employeeEmail, ownerEmail, template, subject))
               .ThrowsAsync(new Exception(errorMessage));

            var controller = new EmployeeEvaluationController(mockService.Object);

            var result = await controller.GetEmployeeEvaluation(employeeEmail, ownerEmail, template, subject);

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            var actualErrorMessage = Assert.IsType<string>(notFoundResult.Value);
        }

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
