using HRIS.Models;
using HRIS.Models.Enums;
using HRIS.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RR.App.Controllers.HRIS;
using Xunit;

namespace RR.App.Tests.Controllers.HRIS;

public class EmployeeEvaluationRatingControllerUnitTests
{
    [Fact]
    public async Task GetAllEmployeeEvaluationRatingsValidInputReturnsOkResult()
    {
        var evaluationInput =
            new EmployeeEvaluationInput
            {
                Id = null,
                OwnerEmail = "owner@retrorabbit.co.za",
                EmployeeEmail = "employee@test.com",
                Template = "Template 1",
                Subject = "Subject 1"
            };

        var serviceMock = new Mock<IEmployeeEvaluationRatingService>();
        var expectedRatings = new List<EmployeeEvaluationRatingDto>
        {
            new(
                1,
                new EmployeeEvaluationDto(
                                          101,
                                          new EmployeeDto(201, "EMP123", "123456", DateTime.Now,
                                                          DateTime.Now.AddMonths(6), 1, false, "No disability", 2,
                                                          new EmployeeTypeDto(1, "Regular"), "Some notes", 25.0f, 20.0f,
                                                          50.0f, 50000, "John Doe", "JD", "Doe",
                                                          DateTime.Parse("1990-01-01"), "South Africa", "South African",
                                                          "ID123456", "PP789012",
                                                          DateTime.Now.AddYears(5), "South Africa", Race.Black,
                                                          Gender.Male, "photo.jpg",
                                                          "john.doe@example.com", "john.doe.personal@example.com",
                                                          "+1234567890", 3, 2,
                                                          new EmployeeAddressDto(401, "Unit123", "ComplexXYZ", "1234",
                                                           "SuburbABC", "City123", "South Africa",
                                                           "ProvinceXYZ", "12345"),
                                                          new EmployeeAddressDto(402, "Unit456", "Complex123", "5678",
                                                           "SuburbXYZ", "City456", "South Africa",
                                                           "ProvinceABC", "67890"), "123 Main St",
                                                          "Emergency Contact", "+9876543210"),
                                          new EmployeeEvaluationTemplateDto(301, "Template1"),
                                          new EmployeeDto(1, "EMP123", "123456", DateTime.Now,
                                                          DateTime.Now.AddMonths(6), 1, false, "No disability", 2,
                                                          new EmployeeTypeDto(1, "Regular"),
                                                          "Some notes", 25.0f, 20.0f, 50.0f, 50000, "John Doe", "JD",
                                                          "Doe", DateTime.Parse("1990-01-01"),
                                                          "South Africa", "South African", "ID123456", "PP789012",
                                                          DateTime.Now.AddYears(5), "South Africa",
                                                          Race.Black, Gender.Male, "photo.jpg", "john.doe@example.com",
                                                          "john.doe.personal@example.com",
                                                          "+1234567890", 3, 2,
                                                          new EmployeeAddressDto(401, "Unit123", "ComplexXYZ", "1234",
                                                           "SuburbABC",
                                                           "City123", "South Africa", "ProvinceXYZ", "12345"),
                                                          new EmployeeAddressDto(402, "Unit456", "Complex123", "5678",
                                                           "SuburbXYZ", "City456", "South Africa",
                                                           "ProvinceABC", "67890"), "123 Main St",
                                                          "Emergency Contact", "+9876543210"), "Subject1",
                                          DateOnly.FromDateTime(DateTime.Now), null),
                new EmployeeDto(1, "EMP123", "123456", DateTime.Now, DateTime.Now.AddMonths(6), 1, false,
                                "No disability",
                                2, new EmployeeTypeDto(1, "Regular"), "Some notes", 25.0f, 20.0f, 50.0f, 50000,
                                "John Doe", "JD",
                                "Doe", DateTime.Parse("1990-01-01"), "South Africa", "South African", "ID123456",
                                "PP789012",
                                DateTime.Now.AddYears(5), "South Africa", Race.Black, Gender.Male, "photo.jpg",
                                "john.doe@example.com", "john.doe.personal@example.com", "+1234567890", 3, 2,
                                new EmployeeAddressDto(401, "Unit123", "ComplexXYZ", "1234", "SuburbABC", "City123",
                                                       "South Africa",
                                                       "ProvinceXYZ", "12345"),
                                new EmployeeAddressDto(402, "Unit456", "Complex123", "5678", "SuburbXYZ", "City456",
                                                       "South Africa",
                                                       "ProvinceABC", "67890"), "123 Main St", "Emergency Contact",
                                "+9876543210"),
                "exampleDescription",
                4.5f,
                "exampleComment")
        };

        serviceMock.Setup(x => x.GetAllByEvaluation(evaluationInput)).ReturnsAsync(expectedRatings);

        var controller = new EmployeeEvaluationRatingController(serviceMock.Object);

        var result = await controller.GetAllEmployeeEvaluationRatings(evaluationInput);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var actualRatings = Assert.IsType<List<EmployeeEvaluationRatingDto>>(okResult.Value);
        Assert.Equal(expectedRatings, actualRatings);
    }


    [Fact]
    public async Task GetAllEmployeeEvaluationRatingsExceptionThrownReturnsNotFoundResult()
    {
        var evaluationInput =
            new EmployeeEvaluationInput
            {
                Id = null,
                OwnerEmail = "owner@retrorabbit.co.za",
                EmployeeEmail = "employee@test.com",
                Template = "Template 1",
                Subject = "Subject 1"
            };

        var serviceMock = new Mock<IEmployeeEvaluationRatingService>();
        var exceptionMessage = "An error occurred while fetching employee evaluation ratings.";
        serviceMock.Setup(x => x.GetAllByEvaluation(evaluationInput)).ThrowsAsync(new Exception(exceptionMessage));

        var controller = new EmployeeEvaluationRatingController(serviceMock.Object);

        var result = await controller.GetAllEmployeeEvaluationRatings(evaluationInput);

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        var actualExceptionMessage = Assert.IsType<string>(notFoundResult.Value);
        Assert.Equal(exceptionMessage, actualExceptionMessage);
    }

    [Fact]
    public async Task SaveEmployeeEvaluationRatingValidInputReturnsOkResult()
    {
        var evaluationInput = new EmployeeEvaluationInput
        {
            Id = null,
            OwnerEmail = "owner@retrorabbit.co.za",
            EmployeeEmail = "employee@retrorabbit.co.za",
            Template = "Template 1",
            Subject = "Subject 1"
        };

        var ratingInput = new EvaluationRatingInput(1, "test@retrorabbit.co.za", evaluationInput, "Test Description",
                                                    5.0f, "Test Comment");

        var serviceMock = new Mock<IEmployeeEvaluationRatingService>();

        var employeeDto = new EmployeeDto(201, "EMP123", "123456", DateTime.Now, DateTime.Now.AddMonths(6), 1, false,
                                          "No disability", 2,
                                          new EmployeeTypeDto(1, "Regular"), "Some notes", 25.0f, 20.0f, 50.0f, 50000,
                                          "John Doe", "JD", "Doe",
                                          DateTime.Parse("1990-01-01"), "South Africa", "South African", "ID123456",
                                          "PP789012",
                                          DateTime.Now.AddYears(5), "South Africa", Race.Black, Gender.Male,
                                          "photo.jpg",
                                          "john.doe@example.com", "john.doe.personal@example.com", "+1234567890", 3, 2,
                                          new EmployeeAddressDto(401, "Unit123", "ComplexXYZ", "1234", "SuburbABC",
                                                                 "City123", "South Africa",
                                                                 "ProvinceXYZ", "12345"),
                                          new EmployeeAddressDto(402, "Unit456", "Complex123", "5678", "SuburbXYZ",
                                                                 "City456", "South Africa",
                                                                 "ProvinceABC", "67890"), "123 Main St",
                                          "Emergency Contact", "+9876543210");

        var templateDto = new EmployeeEvaluationTemplateDto(301, "Template 1");

        var savedRating = new EmployeeEvaluationRatingDto(1,
                                                          new EmployeeEvaluationDto(201, employeeDto, templateDto,
                                                           employeeDto, "Subject 1",
                                                           DateOnly.FromDateTime(DateTime.Now), null),
                                                          employeeDto, "Test Description", 5.0f, "Test Comment");

        serviceMock.Setup(x => x.Save(ratingInput)).ReturnsAsync(savedRating);

        var controller = new EmployeeEvaluationRatingController(serviceMock.Object);

        var result = await controller.SaveEmployeeEvaluationRating(ratingInput);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var actualSavedRating = Assert.IsType<EmployeeEvaluationRatingDto>(okResult.Value);

        Assert.Equal(savedRating.Id, actualSavedRating.Id);
        Assert.Equal(savedRating.Description, actualSavedRating.Description);
        Assert.Equal(savedRating.Score, actualSavedRating.Score);
        Assert.Equal(savedRating.Comment, actualSavedRating.Comment);

        Assert.Equal(savedRating.Employee?.Id, actualSavedRating.Employee?.Id);
        Assert.Equal(savedRating.Employee?.EmployeeNumber, actualSavedRating.Employee?.EmployeeNumber);

        Assert.Equal(savedRating.Evaluation?.Id, actualSavedRating.Evaluation?.Id);
        Assert.Equal(savedRating.Evaluation?.Subject, actualSavedRating.Evaluation?.Subject);
    }

    [Fact]
    public async Task SaveEmployeeEvaluationRatingExceptionThrownReturnsNotFoundResult()
    {
        var ratingInput =
            new EvaluationRatingInput(1, "test@retrorabbit.co.za", null, "Test Description", 5.0f, "Test Comment");

        var serviceMock = new Mock<IEmployeeEvaluationRatingService>();
        serviceMock.Setup(x => x.Save(ratingInput))
                   .ThrowsAsync(new Exception("An error occurred while saving the employee evaluation rating."));

        var controller = new EmployeeEvaluationRatingController(serviceMock.Object);

        var result = await controller.SaveEmployeeEvaluationRating(ratingInput);

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        var exceptionMessage = Assert.IsType<string>(notFoundResult.Value);
        Assert.Equal("An error occurred while saving the employee evaluation rating.", exceptionMessage);
    }

    [Fact]
    public async Task UpdateEmployeeEvaluationRatingValidInputReturnsOkResult()
    {
        var evaluationInput = new EmployeeEvaluationInput { Id = 101,
            OwnerEmail = "owner@retrorabbit.co.za",
            EmployeeEmail = "employee@retrorabbit.co.za",
            Template = "Template 1",
            Subject = "Subject 1" };

        var ratingInput = new EvaluationRatingInput(1, "test@retrorabbit.co.za", evaluationInput, "Updated Description",
                                                    4.5f, "Updated Comment");

        var serviceMock = new Mock<IEmployeeEvaluationRatingService>();

        var employeeDto = new EmployeeDto(201, "EMP123", "123456", DateTime.Now, DateTime.Now.AddMonths(6), 1, false,
                                          "No disability", 2,
                                          new EmployeeTypeDto(1, "Regular"), "Some notes", 25.0f, 20.0f, 50.0f, 50000,
                                          "John Doe", "JD", "Doe",
                                          DateTime.Parse("1990-01-01"), "South Africa", "South African", "ID123456",
                                          "PP789012",
                                          DateTime.Now.AddYears(5), "South Africa", Race.Black, Gender.Male,
                                          "photo.jpg",
                                          "john.doe@example.com", "john.doe.personal@example.com", "+1234567890", 3, 2,
                                          new EmployeeAddressDto(401, "Unit123", "ComplexXYZ", "1234", "SuburbABC",
                                                                 "City123", "South Africa",
                                                                 "ProvinceXYZ", "12345"),
                                          new EmployeeAddressDto(402, "Unit456", "Complex123", "5678", "SuburbXYZ",
                                                                 "City456", "South Africa",
                                                                 "ProvinceABC", "67890"), "123 Main St",
                                          "Emergency Contact", "+9876543210");

        var evaluationDto = new EmployeeEvaluationDto(
                                                      101,
                                                      new EmployeeDto(201, "EMP123", "123456", DateTime.Now,
                                                                      DateTime.Now.AddMonths(6), 1, false,
                                                                      "No disability", 2,
                                                                      new EmployeeTypeDto(1, "Regular"), "Some notes",
                                                                      25.0f, 20.0f, 50.0f, 50000, "John Doe", "JD",
                                                                      "Doe",
                                                                      DateTime.Parse("1990-01-01"), "South Africa",
                                                                      "South African", "ID123456", "PP789012",
                                                                      DateTime.Now.AddYears(5), "South Africa",
                                                                      Race.Black, Gender.Male, "photo.jpg",
                                                                      "john.doe@example.com",
                                                                      "john.doe.personal@example.com", "+1234567890", 3,
                                                                      2,
                                                                      new EmployeeAddressDto(401, "Unit123",
                                                                       "ComplexXYZ", "1234", "SuburbABC", "City123",
                                                                       "South Africa",
                                                                       "ProvinceXYZ", "12345"),
                                                                      new EmployeeAddressDto(402, "Unit456",
                                                                       "Complex123", "5678", "SuburbXYZ", "City456",
                                                                       "South Africa",
                                                                       "ProvinceABC", "67890"), "123 Main St",
                                                                      "Emergency Contact", "+9876543210"),
                                                      new EmployeeEvaluationTemplateDto(301, "Template1"),
                                                      new EmployeeDto(1, "EMP123", "123456", DateTime.Now,
                                                                      DateTime.Now.AddMonths(6), 1, false,
                                                                      "No disability", 2,
                                                                      new EmployeeTypeDto(1, "Regular"),
                                                                      "Some notes", 25.0f, 20.0f, 50.0f, 50000,
                                                                      "John Doe", "JD", "Doe",
                                                                      DateTime.Parse("1990-01-01"),
                                                                      "South Africa", "South African", "ID123456",
                                                                      "PP789012", DateTime.Now.AddYears(5),
                                                                      "South Africa",
                                                                      Race.Black, Gender.Male, "photo.jpg",
                                                                      "john.doe@example.com",
                                                                      "john.doe.personal@example.com",
                                                                      "+1234567890", 3, 2,
                                                                      new EmployeeAddressDto(401, "Unit123",
                                                                       "ComplexXYZ", "1234", "SuburbABC",
                                                                       "City123", "South Africa", "ProvinceXYZ",
                                                                       "12345"),
                                                                      new EmployeeAddressDto(402, "Unit456",
                                                                       "Complex123", "5678", "SuburbXYZ", "City456",
                                                                       "South Africa",
                                                                       "ProvinceABC", "67890"), "123 Main St",
                                                                      "Emergency Contact", "+9876543210"), "Subject1",
                                                      DateOnly.FromDateTime(DateTime.Now), null);

        var originalRatingDto = new EmployeeEvaluationRatingDto(
                                                                1,
                                                                new EmployeeEvaluationDto(
                                                                 101,
                                                                 new EmployeeDto(201, "EMP123", "123456",
                                                                  DateTime.Now, DateTime.Now.AddMonths(6), 1,
                                                                  false, "No disability", 2,
                                                                  new EmployeeTypeDto(1, "Regular"),
                                                                  "Some notes", 25.0f, 20.0f, 50.0f, 50000,
                                                                  "John Doe", "JD", "Doe",
                                                                  DateTime.Parse("1990-01-01"), "South Africa",
                                                                  "South African", "ID123456", "PP789012",
                                                                  DateTime.Now.AddYears(5), "South Africa",
                                                                  Race.Black, Gender.Male, "photo.jpg",
                                                                  "john.doe@example.com",
                                                                  "john.doe.personal@example.com",
                                                                  "+1234567890", 3, 2,
                                                                  new EmployeeAddressDto(401, "Unit123",
                                                                   "ComplexXYZ", "1234", "SuburbABC",
                                                                   "City123", "South Africa",
                                                                   "ProvinceXYZ", "12345"),
                                                                  new EmployeeAddressDto(402, "Unit456",
                                                                   "Complex123", "5678", "SuburbXYZ",
                                                                   "City456", "South Africa",
                                                                   "ProvinceABC", "67890"), "123 Main St",
                                                                  "Emergency Contact", "+9876543210"),
                                                                 new EmployeeEvaluationTemplateDto(301,
                                                                  "Template1"),
                                                                 new EmployeeDto(1, "EMP123", "123456",
                                                                  DateTime.Now,
                                                                  DateTime.Now.AddMonths(6), 1, false,
                                                                  "No disability", 2,
                                                                  new EmployeeTypeDto(1, "Regular"),
                                                                  "Some notes", 25.0f, 20.0f, 50.0f, 50000,
                                                                  "John Doe", "JD", "Doe",
                                                                  DateTime.Parse("1990-01-01"),
                                                                  "South Africa", "South African", "ID123456",
                                                                  "PP789012", DateTime.Now.AddYears(5),
                                                                  "South Africa",
                                                                  Race.Black, Gender.Male, "photo.jpg",
                                                                  "john.doe@example.com",
                                                                  "john.doe.personal@example.com",
                                                                  "+1234567890", 3, 2,
                                                                  new EmployeeAddressDto(401, "Unit123",
                                                                   "ComplexXYZ", "1234", "SuburbABC",
                                                                   "City123", "South Africa", "ProvinceXYZ",
                                                                   "12345"),
                                                                  new EmployeeAddressDto(402, "Unit456",
                                                                   "Complex123", "5678", "SuburbXYZ",
                                                                   "City456", "South Africa",
                                                                   "ProvinceABC", "67890"), "123 Main St",
                                                                  "Emergency Contact", "+9876543210"),
                                                                 "Subject1",
                                                                 DateOnly.FromDateTime(DateTime.Now), null),
                                                                new EmployeeDto(1, "EMP123", "123456", DateTime.Now,
                                                                                DateTime.Now.AddMonths(6), 1, false,
                                                                                "No disability",
                                                                                2, new EmployeeTypeDto(1, "Regular"),
                                                                                "Some notes", 25.0f, 20.0f, 50.0f,
                                                                                50000, "John Doe", "JD",
                                                                                "Doe", DateTime.Parse("1990-01-01"),
                                                                                "South Africa", "South African",
                                                                                "ID123456", "PP789012",
                                                                                DateTime.Now.AddYears(5),
                                                                                "South Africa", Race.Black, Gender.Male,
                                                                                "photo.jpg",
                                                                                "john.doe@example.com",
                                                                                "john.doe.personal@example.com",
                                                                                "+1234567890", 3, 2,
                                                                                new EmployeeAddressDto(401, "Unit123",
                                                                                 "ComplexXYZ", "1234", "SuburbABC",
                                                                                 "City123", "South Africa",
                                                                                 "ProvinceXYZ", "12345"),
                                                                                new EmployeeAddressDto(402, "Unit456",
                                                                                 "Complex123", "5678", "SuburbXYZ",
                                                                                 "City456", "South Africa",
                                                                                 "ProvinceABC", "67890"),
                                                                                "123 Main St", "Emergency Contact",
                                                                                "+9876543210"),
                                                                "exampleDescription",
                                                                4.5f,
                                                                "exampleComment");

        serviceMock.Setup(x => x.Update(ratingInput)).ReturnsAsync(originalRatingDto);

        var controller = new EmployeeEvaluationRatingController(serviceMock.Object);

        var result = await controller.UpdateEmployeeEvaluationRating(ratingInput);

        var okResult = Assert.IsType<OkResult>(result);

        serviceMock.Verify(x => x.Update(It.IsAny<EvaluationRatingInput>()), Times.Once);
    }


    [Fact]
    public async Task UpdateEmployeeEvaluationRatingExceptionThrownReturnsNotFoundResult()
    {
        var evaluationInput = new EmployeeEvaluationInput
        {
            Id = 101,
            OwnerEmail = "owner@retrorabbit.co.za",
            EmployeeEmail = "employee@retrorabbit.co.za",
            Template = "Template 1",
            Subject = "Subject 1"
        };

        var ratingInput = new EvaluationRatingInput(1, "test@retrorabbit.co.za", evaluationInput, "Updated Description",
                                                    4.5f, "Updated Comment");

        var serviceMock = new Mock<IEmployeeEvaluationRatingService>();
        serviceMock.Setup(x => x.Update(ratingInput))
                   .ThrowsAsync(new Exception("An error occurred while updating the employee evaluation rating."));

        var controller = new EmployeeEvaluationRatingController(serviceMock.Object);

        var result = await controller.UpdateEmployeeEvaluationRating(ratingInput);

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("An error occurred while updating the employee evaluation rating.", notFoundResult.Value);
    }

    [Fact]
    public async Task DeleteEmployeeEvaluationRatingValidInputReturnsOkResult()
    {
        var evaluationInput = new EmployeeEvaluationInput
        {
            Id = 101,
            OwnerEmail = "owner@retrorabbit.co.za",
            EmployeeEmail = "employee@retrorabbit.co.za",
            Template = "Template 1",
            Subject = "Subject 1"
        };

        var ratingInput = new EvaluationRatingInput(1, "test@retrorabbit.co.za", evaluationInput, "Test Description",
                                                    5.0f, "Test Comment");

        var employeeDto = new EmployeeDto(201, "EMP123", "123456", DateTime.Now, DateTime.Now.AddMonths(6), 1, false,
                                          "No disability", 2,
                                          new EmployeeTypeDto(1, "Regular"), "Some notes", 25.0f, 20.0f, 50.0f, 50000,
                                          "John Doe", "JD", "Doe",
                                          DateTime.Parse("1990-01-01"), "South Africa", "South African", "ID123456",
                                          "PP789012",
                                          DateTime.Now.AddYears(5), "South Africa", Race.Black, Gender.Male,
                                          "photo.jpg",
                                          "john.doe@example.com", "john.doe.personal@example.com", "+1234567890", 3, 2,
                                          new EmployeeAddressDto(401, "Unit123", "ComplexXYZ", "1234", "SuburbABC",
                                                                 "City123", "South Africa",
                                                                 "ProvinceXYZ", "12345"),
                                          new EmployeeAddressDto(402, "Unit456", "Complex123", "5678", "SuburbXYZ",
                                                                 "City456", "South Africa",
                                                                 "ProvinceABC", "67890"), "123 Main St",
                                          "Emergency Contact", "+9876543210");

        var templateDto = new EmployeeEvaluationTemplateDto(301, "Template 1");

        var serviceMock = new Mock<IEmployeeEvaluationRatingService>();
        serviceMock.Setup(x => x.Delete(ratingInput)).ReturnsAsync(new EmployeeEvaluationRatingDto(1,
                                                                    new EmployeeEvaluationDto(201, employeeDto,
                                                                     templateDto, employeeDto, "Subject 1",
                                                                     DateOnly.FromDateTime(DateTime.Now), null),
                                                                    employeeDto, "Test Description", 5.0f,
                                                                    "Test Comment"));

        var controller = new EmployeeEvaluationRatingController(serviceMock.Object);

        var result = await controller.DeleteEmployeeEvaluationRating(ratingInput);

        var okResult = Assert.IsType<OkResult>(result);
        Assert.Equal(200, okResult.StatusCode);
    }

    [Fact]
    public async Task DeleteEmployeeEvaluationRatingExceptionThrownReturnsNotFoundResult()
    {
        var evaluationInput = new EmployeeEvaluationInput
        {
            Id = 101,
            OwnerEmail = "owner@retrorabbit.co.za",
            EmployeeEmail = "employee@retrorabbit.co.za",
            Template = "Template 1",
            Subject = "Subject 1"
        };

        var ratingInput = new EvaluationRatingInput(1, "test@retrorabbit.co.za", evaluationInput, "Test Description",
                                                    5.0f, "Test Comment");

        var serviceMock = new Mock<IEmployeeEvaluationRatingService>();
        serviceMock.Setup(x => x.Delete(ratingInput))
                   .ThrowsAsync(new Exception("An error occurred while deleting the employee evaluation rating."));

        var controller = new EmployeeEvaluationRatingController(serviceMock.Object);

        var result = await controller.DeleteEmployeeEvaluationRating(ratingInput);

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("An error occurred while deleting the employee evaluation rating.", notFoundResult.Value);
    }
}