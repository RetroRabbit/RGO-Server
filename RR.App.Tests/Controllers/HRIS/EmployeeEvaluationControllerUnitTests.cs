using HRIS.Models;
using HRIS.Models.Enums;
using HRIS.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RR.App.Controllers.HRIS;
using Xunit;

namespace RR.App.Tests.Controllers.HRIS;

public class EmployeeEvaluationControllerUnitTests
{
    [Fact]
    public async Task GetAllEmployeeEvaluationsValidEmailReturnsOkResultWithEvaluations()
    {
        var email = "test@retrorabbit.co.za";
        var expectedEvaluations = new List<EmployeeEvaluationDto>
        {
            new(
                1,
                new EmployeeDto
                    (1, "Emp123", "Tax123", new DateTime(2022, 1, 1), null, 1, false, "No disability", 2,
                     new EmployeeTypeDto{ Id = 1, Name = "Full Time" }, "Notes", 20.0f, 15.0f, 50.0f, 50000, "John Doe", "JD", "Doe",
                     new DateTime(1990, 1, 1),
                     "South Africa", "South African", "123456789", "AB123456", new DateTime(2025, 1, 1), "South Africa",
                     Race.White, Gender.Male, "photo.jpg",
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
                    (2, "Emp124", "Tax124", new DateTime(2022, 1, 1), null, 1, false, "No disability", 2,
                     new EmployeeTypeDto{ Id = 1, Name = "Full Time" }, "Notes", 20.0f, 15.0f, 50.0f, 50000, "John Doe", "JD", "Doe",
                     new DateTime(1990, 1, 1),
                     "South Africa", "South African", "123456789", "AB123456", new DateTime(2025, 1, 1), "South Africa",
                     Race.White, Gender.Male, "photo.jpg",
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
               )
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
                 (1, "Emp123", "Tax123", new DateTime(2022, 1, 1), null, 1, false, "No disability", 2,
                  new EmployeeTypeDto{ Id = 1, Name = "Full Time" }, "Notes", 20.0f, 15.0f, 50.0f, 50000, "John Doe", "JD", "Doe",
                  new DateTime(1990, 1, 1),
                  "South Africa", "South African", "123456789", "AB123456", new DateTime(2025, 1, 1), "South Africa",
                  Race.White, Gender.Male, "photo.jpg",
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
                 (2, "Emp124", "Tax124", new DateTime(2022, 1, 1), null, 1, false, "No disability", 2,
                  new EmployeeTypeDto{ Id = 1, Name = "Full Time" }, "Notes", 20.0f, 15.0f, 50.0f, 50000, "John Doe", "JD", "Doe",
                  new DateTime(1990, 1, 1),
                  "South Africa", "South African", "123456789", "AB123456", new DateTime(2025, 1, 1), "South Africa",
                  Race.White, Gender.Male, "photo.jpg",
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


        Assert.Equal(expectedEvaluation.Employee?.Id, actualEvaluation.Employee?.Id);
        Assert.Equal(expectedEvaluation.Employee?.EmployeeNumber, actualEvaluation.Employee?.EmployeeNumber);
    }

    [Fact]
    public async Task GetEmployeeEvaluationInvalidParametersReturnsNotFoundResultWithErrorMessage()
    {
        var employeeEmail = "employee@retrorabbit.co.za";
        var ownerEmail = "owner@retrorabbit.co.za";
        var template = "Employee Evaluation Template";
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

    [Fact]
    public async Task SaveEmployeeEvaluationValidInputReturnsOkResultWithSavedEvaluation()
    {
        var mockService = new Mock<IEmployeeEvaluationService>();
        var controller = new EmployeeEvaluationController(mockService.Object);

        var input = new EmployeeEvaluationInput
            (1, "owner@retrorabbit.co.za", "employee@retrorabbit.co.za", "Template 1", "Subject 1");

        var expectedSavedEvaluation = new EmployeeEvaluationDto
            (
             1,
             new EmployeeDto
                 (1, "Emp123", "Tax123", new DateTime(2022, 1, 1), null, 1, false, "No disability", 2,
                  new EmployeeTypeDto{ Id = 1, Name = "Full Time" }, "Notes", 20.0f, 15.0f, 50.0f, 50000, "John Doe", "JD", "Doe",
                  new DateTime(1990, 1, 1),
                  "South Africa", "South African", "123456789", "AB123456", new DateTime(2025, 1, 1), "South Africa",
                  Race.White, Gender.Male, "photo.jpg",
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
                 (2, "Emp124", "Tax124", new DateTime(2022, 1, 1), null, 1, false, "No disability", 2,
                  new EmployeeTypeDto{ Id = 1, Name = "Full Time" }, "Notes", 20.0f, 15.0f, 50.0f, 50000, "John Doe", "JD", "Doe",
                  new DateTime(1990, 1, 1),
                  "South Africa", "South African", "123456789", "AB123456", new DateTime(2025, 1, 1), "South Africa",
                  Race.White, Gender.Male, "photo.jpg",
                  "john.doe@example.com", "john.doe.personal@example.com", "1234567890", 1, 1,
                  new EmployeeAddressDto
                      (1, "Unit 1", "Complex A", "123", "Suburb", "City", "Country", "Province", "12345"),
                  new EmployeeAddressDto
                      (2, "P.O. Box 123", "", "456", "Suburb", "City", "Country", "Province", "54321"),
                  "12",
                  "Emergency Contact",
                  "987654321"
                 ),
             "Subject 1",
             new DateOnly(2022, 1, 1),
             new DateOnly(2022, 2, 1)
            );

        mockService.Setup(x => x.Save(It.IsAny<EmployeeEvaluationInput>())).ReturnsAsync(expectedSavedEvaluation);

        var result = await controller.SaveEmployeeEvaluation(input);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var actualSavedEvaluation = Assert.IsType<EmployeeEvaluationDto>(okResult.Value);

        Assert.Equal(expectedSavedEvaluation.Id, actualSavedEvaluation.Id);
        Assert.Equal(expectedSavedEvaluation.Subject, actualSavedEvaluation.Subject);
        Assert.Equal(expectedSavedEvaluation.StartDate, actualSavedEvaluation.StartDate);
        Assert.Equal(expectedSavedEvaluation.EndDate, actualSavedEvaluation.EndDate);

        Assert.Equal(expectedSavedEvaluation.Employee?.Id, actualSavedEvaluation.Employee?.Id);
        Assert.Equal(expectedSavedEvaluation.Employee?.EmployeeNumber, actualSavedEvaluation.Employee?.EmployeeNumber);

        Assert.Equal(expectedSavedEvaluation.Template?.Id, actualSavedEvaluation.Template?.Id);
        Assert.Equal(expectedSavedEvaluation.Template?.Description, actualSavedEvaluation.Template?.Description);

        Assert.Equal(expectedSavedEvaluation.Owner?.Id, actualSavedEvaluation.Owner?.Id);
        Assert.Equal(expectedSavedEvaluation.Owner?.EmployeeNumber, actualSavedEvaluation.Owner?.EmployeeNumber);
    }

    [Fact]
    public async Task SaveEmployeeEvaluationInvalidInputReturnsNotFoundResultWithErrorMessage()
    {
        var mockService = new Mock<IEmployeeEvaluationService>();
        var controller = new EmployeeEvaluationController(mockService.Object);

        var invalidInput = new EmployeeEvaluationInput
            (1, null, "employee@retrorabbit.co.za", "", "Evaluation Subject 1");

        mockService.Setup(x => x.Save(invalidInput))
                   .ThrowsAsync(new Exception("Invalid input error message"));

        var result = await controller.SaveEmployeeEvaluation(invalidInput);

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        var actualErrorMessage = Assert.IsType<string>(notFoundResult.Value);

        Assert.Equal("Invalid input error message", actualErrorMessage);
    }


    [Fact]
    public async Task UpdateEmployeeEvaluationValidInputReturnsOkResult()
    {
        var mockService = new Mock<IEmployeeEvaluationService>();
        var controller = new EmployeeEvaluationController(mockService.Object);

        var evaluationInputList = new List<EmployeeEvaluationInput>
        {
            new(
                1,
                "owner@retrorabbit.co.za",
                "employee@retrorabbit.co.za",
                "Template 1",
                "Subject 1"
               ),
            new(
                2,
                "owner@retrorabbit.co.za",
                "employee@retrorabbit.co.za",
                "Template 2",
                "Subject 2"
               )
        };

        mockService.Setup(x => x.Update(It.IsAny<EmployeeEvaluationInput>(), It.IsAny<EmployeeEvaluationInput>()))
                   .ReturnsAsync(new EmployeeEvaluationDto
                                     (
                                      1,
                                      new EmployeeDto
                                          (1, "Emp123", "Tax123", new DateTime(2022, 1, 1), null, 1, false,
                                           "No disability", 2,
                                           new EmployeeTypeDto{ Id = 1, Name = "Full Time" }, "Notes", 20.0f, 15.0f, 50.0f, 50000,
                                           "John Doe", "JD", "Doe", new DateTime(1990, 1, 1),
                                           "South Africa", "South African", "123456789", "AB123456",
                                           new DateTime(2025, 1, 1), "South Africa", Race.White, Gender.Male,
                                           "photo.jpg",
                                           "test@retrorabbit.co.za", "john.doe.personal@example.com", "1234567890", 1,
                                           1,
                                           new EmployeeAddressDto
                                               (1, "Unit 1", "Complex A", "123", "Suburb", "City", "Country",
                                                "Province", "12345"),
                                           new EmployeeAddressDto
                                               (2, "P.O. Box 123", "", "456", "Suburb", "City", "Country", "Province",
                                                "54321"),
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
                                          (2, "Emp124", "Tax124", new DateTime(2022, 1, 1), null, 1, false,
                                           "No disability", 2,
                                           new EmployeeTypeDto{ Id = 1, Name = "Full Time" }, "Notes", 20.0f, 15.0f, 50.0f, 50000,
                                           "John Doe", "JD", "Doe", new DateTime(1990, 1, 1),
                                           "South Africa", "South African", "123456789", "AB123456",
                                           new DateTime(2025, 1, 1), "South Africa", Race.White, Gender.Male,
                                           "photo.jpg",
                                           "john.doe@example.com", "john.doe.personal@example.com", "1234567890", 1, 1,
                                           new EmployeeAddressDto
                                               (1, "Unit 1", "Complex A", "123", "Suburb", "City", "Country",
                                                "Province", "12345"),
                                           new EmployeeAddressDto
                                               (2, "P.O. Box 123", "", "456", "Suburb", "City", "Country", "Province",
                                                "54321"),
                                           "12",
                                           "Emergency Contact",
                                           "987654321"
                                          ),
                                      "Subject 1",
                                      new DateOnly(2022, 1, 1),
                                      new DateOnly(2022, 2, 1)
                                     ));

        var result = await controller.UpdateEmployeeEvaluation(evaluationInputList);

        Assert.IsType<OkResult>(result);
    }

    [Fact]
    public async Task UpdateEmployeeEvaluationInvalidInputReturnsNotFoundResultWithErrorMessage()
    {
        var mockService = new Mock<IEmployeeEvaluationService>();
        var controller = new EmployeeEvaluationController(mockService.Object);

        var invalidInputList = new List<EmployeeEvaluationInput>
        {
            new(0, null, "invalidemail", "", null),
            new(-1, "owner@retrorabbit.co.za", "employee@retrorabbit.co.za", "Template 1", "Subject 1")
        };

        var errorMessage = "Invalid input error message";
        mockService.Setup(x => x.Update(It.IsAny<EmployeeEvaluationInput>(), It.IsAny<EmployeeEvaluationInput>()))
                   .ThrowsAsync(new Exception(errorMessage));

        var result = await controller.UpdateEmployeeEvaluation(invalidInputList);

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        var actualErrorMessage = Assert.IsType<string>(notFoundResult.Value);

        Assert.Equal(errorMessage, actualErrorMessage);
    }

    [Fact]
    public async Task DeleteEmployeeEvaluationValidInputReturnsOkResult()
    {
        var mockService = new Mock<IEmployeeEvaluationService>();
        var controller = new EmployeeEvaluationController(mockService.Object);

        var evaluationInput = new EmployeeEvaluationInput
            (1, "owner@retrorabbit.co.za", "employee@retrorabbit.co.za", "Template 1", "Subject 1");

        mockService.Setup(x => x.Delete(evaluationInput))
                   .ReturnsAsync(new EmployeeEvaluationDto
                                     (
                                      1,
                                      new EmployeeDto
                                          (1, "Emp123", "Tax123", new DateTime(2022, 1, 1), null, 1, false,
                                           "No disability", 2,
                                           new EmployeeTypeDto{ Id = 1, Name = "Full Time" }, "Notes", 20.0f, 15.0f, 50.0f, 50000,
                                           "John Doe", "JD", "Doe", new DateTime(1990, 1, 1),
                                           "South Africa", "South African", "123456789", "AB123456",
                                           new DateTime(2025, 1, 1), "South Africa", Race.White, Gender.Male,
                                           "photo.jpg",
                                           "test@retrorabbit.co.za", "john.doe.personal@example.com", "1234567890", 1,
                                           1,
                                           new EmployeeAddressDto
                                               (1, "Unit 1", "Complex A", "123", "Suburb", "City", "Country",
                                                "Province", "12345"),
                                           new EmployeeAddressDto
                                               (2, "P.O. Box 123", "", "456", "Suburb", "City", "Country", "Province",
                                                "54321"),
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
                                          (2, "Emp124", "Tax124", new DateTime(2022, 1, 1), null, 1, false,
                                           "No disability", 2,
                                           new EmployeeTypeDto{ Id = 1, Name = "Full Time" }, "Notes", 20.0f, 15.0f, 50.0f, 50000,
                                           "John Doe", "JD", "Doe", new DateTime(1990, 1, 1),
                                           "South Africa", "South African", "123456789", "AB123456",
                                           new DateTime(2025, 1, 1), "South Africa", Race.White, Gender.Male,
                                           "photo.jpg",
                                           "john.doe@example.com", "john.doe.personal@example.com", "1234567890", 1, 1,
                                           new EmployeeAddressDto
                                               (1, "Unit 1", "Complex A", "123", "Suburb", "City", "Country",
                                                "Province", "12345"),
                                           new EmployeeAddressDto
                                               (2, "P.O. Box 123", "", "456", "Suburb", "City", "Country", "Province",
                                                "54321"),
                                           "12",
                                           "Emergency Contact",
                                           "987654321"
                                          ),
                                      "Subject 1",
                                      new DateOnly(2022, 1, 1),
                                      new DateOnly(2022, 2, 1)
                                     ));

        var result = await controller.DeleteEmployeeEvaluation(evaluationInput);

        Assert.IsType<OkResult>(result);
    }

    [Fact]
    public async Task DeleteEmployeeEvaluationInvalidInputReturnsNotFoundResultWithErrorMessage()
    {
        var mockService = new Mock<IEmployeeEvaluationService>();
        var controller = new EmployeeEvaluationController(mockService.Object);

        var invalidEvaluationInput = new EmployeeEvaluationInput
            (0, null, "invalidemail", "", null);

        var errorMessage = "Invalid input error message";
        mockService.Setup(x => x.Delete(invalidEvaluationInput))
                   .ThrowsAsync(new Exception(errorMessage));

        var result = await controller.DeleteEmployeeEvaluation(invalidEvaluationInput);

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        var actualErrorMessage = Assert.IsType<string>(notFoundResult.Value);

        Assert.Equal(errorMessage, actualErrorMessage);
    }
}
