using HRIS.Models;
using HRIS.Models.Enums;
using HRIS.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RR.App.Controllers.HRIS;
using RR.Tests.Data.Models.HRIS;
using Xunit;

namespace RR.App.Tests.Controllers.HRIS;

public class EmployeeEvaluationRatingControllerUnitTests
{
    private readonly Mock<IEmployeeEvaluationRatingService> _employeeEvaluationRatingServiceMock;
    private readonly EmployeeEvaluationRatingController _controller;
    private readonly EmployeeEvaluationInput _employeeEvaluationInput;
    private readonly List<EmployeeEvaluationRatingDto> _employeeEvaluationRatingDtoList;
    private readonly EvaluationRatingInput _evaluationRatingInput;
    private readonly EmployeeDto _employeeDto;
    private readonly EmployeeEvaluationTemplateDto _employeeEvaluationTemplateDto;
    private readonly EmployeeEvaluationRatingDto _employeeEvaluationRatingDto;
    public EmployeeEvaluationRatingControllerUnitTests()
    {
        _employeeEvaluationRatingServiceMock = new Mock<IEmployeeEvaluationRatingService>();
        _controller = new EmployeeEvaluationRatingController(_employeeEvaluationRatingServiceMock.Object);

        _employeeEvaluationInput = new EmployeeEvaluationInput
        {
            Id = null,
            OwnerEmail = "owner@retrorabbit.co.za",
            EmployeeEmail = "employee@test.com",
            Template = "Template 1",
            Subject = "Subject 1"
        };

        _employeeEvaluationRatingDtoList = new List<EmployeeEvaluationRatingDto>
        {
             new EmployeeEvaluationRatingDto
                {
                    Id = 1,
                    Evaluation = new EmployeeEvaluationDto
                    {
                        Id = 101,

                    Employee =new EmployeeDto
                    {
                    Id = 201,
                    EmployeeNumber = "EMP123",
                    TaxNumber = "123456",
                    EngagementDate = DateTime.Now,
                    TerminationDate = DateTime.Now.AddMonths(6),
                    PeopleChampion = 1,
                    Disability = false,
                    DisabilityNotes = "No disability",
                    Level = 2,
                    EmployeeType = new EmployeeTypeDto { Id = 1, Name = "Regular" },
                    Notes = "Some notes",
                    LeaveInterval = 25.0f,
                    SalaryDays = 20.0f,
                    PayRate = 50.0f,
                    Salary = 50000,
                    Name = "John Doe",
                    Initials = "JD",
                    Surname = "Doe",
                    DateOfBirth = DateTime.Parse("1990-01-01"),
                    CountryOfBirth = "South Africa",
                    Nationality = "South African",
                    IdNumber = "ID123456",
                    PassportNumber = "PP789012",
                    PassportExpirationDate = DateTime.Now.AddYears(5),
                    PassportCountryIssue = "South Africa",
                    Race = Race.Black,
                    Gender = Gender.Male,
                    Photo = "photo.jpg",
                    Email = "john.doe@example.com",
                    PersonalEmail = "john.doe.personal@example.com",
                    CellphoneNo = "+1234567890",
                    ClientAllocated = 3,
                    TeamLead = 2,
                    PhysicalAddress = new EmployeeAddressDto
                    {
                        Id = 1,
                        UnitNumber = "Unit 1",
                        ComplexName = "Complex A",
                        StreetNumber = "123",
                        SuburbOrDistrict = "Suburb",
                        City = "City",
                        Country = "Country",
                        Province = "Province",
                        PostalCode = "12345"
                    },
                    PostalAddress = new EmployeeAddressDto
                    {
                        Id = 2,
                        UnitNumber = "P.O. Box 123",
                        StreetNumber = "456",
                        SuburbOrDistrict = "Suburb",
                        City = "City",
                        Country = "Country",
                        Province = "Province",
                        PostalCode = "54321"
                    },
                    HouseNo = "123 Main St",
                    EmergencyContactName = "Emergency Contact",
                    EmergencyContactNo = "+9876543210" },

                        Template = new EmployeeEvaluationTemplateDto{ Id = 301, Description = "Template1" },

                    Owner = new EmployeeDto
                    {
                    Id = 1,
                    EmployeeNumber = "EMP123",
                    TaxNumber = "123456",
                    EngagementDate = DateTime.Now,
                    TerminationDate = DateTime.Now.AddMonths(6),
                    PeopleChampion = 1,
                    Disability = false,
                    DisabilityNotes = "No disability",
                    Level = 2,
                    EmployeeType = new EmployeeTypeDto { Id = 1, Name = "Regular" },
                    Notes = "Some notes",
                    LeaveInterval = 25.0f,
                    SalaryDays = 20.0f,
                    PayRate = 50.0f,
                    Salary = 50000,
                    Name = "John Doe",
                    Initials = "JD",
                    Surname = "Doe",
                    DateOfBirth = DateTime.Parse("1990-01-01"),
                    CountryOfBirth = "South Africa",
                    Nationality = "South African",
                    IdNumber = "ID123456",
                    PassportNumber = "PP789012",
                    PassportExpirationDate = DateTime.Now.AddYears(5),
                    PassportCountryIssue = "South Africa",
                    Race = Race.Black,
                    Gender = Gender.Male,
                    Photo = "photo.jpg",
                    Email = "john.doe@example.com",
                    PersonalEmail = "john.doe.personal@example.com",
                    CellphoneNo = "+1234567890",
                    ClientAllocated = 3,
                    TeamLead = 2,
                    PhysicalAddress = new EmployeeAddressDto
                    {
                        Id = 1,
                        UnitNumber = "Unit 1",
                        ComplexName = "Complex A",
                        StreetNumber = "123",
                        SuburbOrDistrict = "Suburb",
                        City = "City",
                        Country = "Country",
                        Province = "Province",
                        PostalCode = "12345"
                    },
                    PostalAddress = new EmployeeAddressDto
                    {
                        Id = 2,
                        UnitNumber = "P.O. Box 123",
                        StreetNumber = "456",
                        SuburbOrDistrict = "Suburb",
                        City = "City",
                        Country = "Country",
                        Province = "Province",
                        PostalCode = "54321"
                    },
                    HouseNo = "123 Main St",
                    EmergencyContactName = "Emergency Contact",
                    EmergencyContactNo = "+9876543210"
                    },
                        Subject = "Subject1",
                        StartDate = DateOnly.FromDateTime(DateTime.Now),
                        EndDate = null
                    },
                    Employee = EmployeeTestData.EmployeeOne.ToDto(),
                    Description = "exampleDescription",
                    Score = 4.5f,
                    Comment = "exampleComment"
                }
        };

        _evaluationRatingInput = new EvaluationRatingInput(1, "test@retrorabbit.co.za", _employeeEvaluationInput, "Test Description",5.0f, "Test Comment");

        _employeeDto = EmployeeTestData.EmployeeOne.ToDto();

        _employeeEvaluationTemplateDto = new EmployeeEvaluationTemplateDto { Id = 301, Description = "Template 1" };

        _employeeEvaluationRatingDto = new EmployeeEvaluationRatingDto
        {
            Id = 1,
            Evaluation = new EmployeeEvaluationDto
            {
                Id = 201,
                Employee = _employeeDto,
                Template = _employeeEvaluationTemplateDto,
                Owner = _employeeDto,
                Subject = "Subject 1",
                StartDate = DateOnly.FromDateTime(DateTime.Now),
                EndDate = null
            },
            Employee = _employeeDto,
            Description = "Test Description",
            Score = 5.0f,
            Comment = "Test Comment"
        };
    }

    [Fact]
    public async Task GetAllEmployeeEvaluationRatingsValidInputReturnsOkResult()
    {
        _employeeEvaluationRatingServiceMock.Setup(x => x.GetAllByEvaluation(_employeeEvaluationInput)).ReturnsAsync(_employeeEvaluationRatingDtoList);

        var result = await _controller.GetAllEmployeeEvaluationRatings(_employeeEvaluationInput);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var actualRatings = Assert.IsType<List<EmployeeEvaluationRatingDto>>(okResult.Value);
        Assert.Equal(_employeeEvaluationRatingDtoList, actualRatings);
    }

    [Fact]
    public async Task GetAllEmployeeEvaluationRatingsExceptionThrownReturnsNotFoundResult()
    {
        _employeeEvaluationRatingServiceMock.Setup(x => x.GetAllByEvaluation(_employeeEvaluationInput)).ThrowsAsync(new Exception("An error occurred while fetching employee evaluation ratings."));

        var result = await _controller.GetAllEmployeeEvaluationRatings(_employeeEvaluationInput);

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        var actualExceptionMessage = Assert.IsType<string>(notFoundResult.Value);
        Assert.Equal("An error occurred while fetching employee evaluation ratings.", actualExceptionMessage);
    }

    [Fact]
    public async Task SaveEmployeeEvaluationRatingValidInputReturnsOkResult()
    {
        _employeeEvaluationRatingServiceMock.Setup(x => x.Save(_evaluationRatingInput)).ReturnsAsync(_employeeEvaluationRatingDto);

        var result = await _controller.SaveEmployeeEvaluationRating(_evaluationRatingInput);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var actualSavedRating = Assert.IsType<EmployeeEvaluationRatingDto>(okResult.Value);
        Assert.Equal(_employeeEvaluationRatingDto.Id, actualSavedRating.Id);
        Assert.Equal(_employeeEvaluationRatingDto.Description, actualSavedRating.Description);
        Assert.Equal(_employeeEvaluationRatingDto.Score, actualSavedRating.Score);
        Assert.Equal(_employeeEvaluationRatingDto.Comment, actualSavedRating.Comment);
        Assert.Equal(_employeeEvaluationRatingDto.Employee?.Id, actualSavedRating.Employee?.Id);
        Assert.Equal(_employeeEvaluationRatingDto.Employee?.EmployeeNumber, actualSavedRating.Employee?.EmployeeNumber);
        Assert.Equal(_employeeEvaluationRatingDto.Evaluation?.Id, actualSavedRating.Evaluation?.Id);
        Assert.Equal(_employeeEvaluationRatingDto.Evaluation?.Subject, actualSavedRating.Evaluation?.Subject);
    }

    [Fact]
    public async Task SaveEmployeeEvaluationRatingExceptionThrownReturnsNotFoundResult()
    {
        _employeeEvaluationRatingServiceMock.Setup(x => x.Save(_evaluationRatingInput))
                   .ThrowsAsync(new Exception("An error occurred while saving the employee evaluation rating."));

        var result = await _controller.SaveEmployeeEvaluationRating(_evaluationRatingInput);

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        var exceptionMessage = Assert.IsType<string>(notFoundResult.Value);
        Assert.Equal("An error occurred while saving the employee evaluation rating.", exceptionMessage);
    }

    [Fact]
    public async Task UpdateEmployeeEvaluationRatingValidInputReturnsOkResult()
    {
        _employeeEvaluationRatingServiceMock.Setup(x => x.Update(_evaluationRatingInput)).ReturnsAsync(_employeeEvaluationRatingDto);

        var result = await _controller.UpdateEmployeeEvaluationRating(_evaluationRatingInput);

        var okResult = Assert.IsType<OkResult>(result);
        _employeeEvaluationRatingServiceMock.Verify(x => x.Update(It.IsAny<EvaluationRatingInput>()), Times.Once);
    }

    [Fact]
    public async Task UpdateEmployeeEvaluationRatingExceptionThrownReturnsNotFoundResult()
    {
        _employeeEvaluationRatingServiceMock.Setup(x => x.Update(_evaluationRatingInput))
                   .ThrowsAsync(new Exception("An error occurred while updating the employee evaluation rating."));

        var result = await _controller.UpdateEmployeeEvaluationRating(_evaluationRatingInput);

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("An error occurred while updating the employee evaluation rating.", notFoundResult.Value);
    }

    [Fact]
    public async Task DeleteEmployeeEvaluationRatingValidInputReturnsOkResult()
    {
        _employeeEvaluationRatingServiceMock.Setup(x => x.Delete(_evaluationRatingInput)).ReturnsAsync(_employeeEvaluationRatingDto);

        var result = await _controller.DeleteEmployeeEvaluationRating(_evaluationRatingInput);

        var okResult = Assert.IsType<OkResult>(result);
        Assert.Equal(200, okResult.StatusCode);
    }

    [Fact]
    public async Task DeleteEmployeeEvaluationRatingExceptionThrownReturnsNotFoundResult()
    {
        _employeeEvaluationRatingServiceMock.Setup(x => x.Delete(_evaluationRatingInput))
                   .ThrowsAsync(new Exception("An error occurred while deleting the employee evaluation rating."));

        var result = await _controller.DeleteEmployeeEvaluationRating(_evaluationRatingInput);

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("An error occurred while deleting the employee evaluation rating.", notFoundResult.Value);
    }
}