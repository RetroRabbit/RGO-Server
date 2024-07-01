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
    private readonly Mock<IEmployeeEvaluationService> _employeeEvaluationServiceMock;
    private readonly EmployeeEvaluationController _controller;
    private readonly EmployeeEvaluationDto _employeeEvaluationDto;
    private readonly List<EmployeeEvaluationDto> _employeeEvaluationDtoList;
    private readonly EmployeeEvaluationInput _employeeEvaluationInput;
    private readonly List<EmployeeEvaluationInput> _employeeEvaluationInputList;
    public EmployeeEvaluationControllerUnitTests()
    {
        _employeeEvaluationServiceMock = new Mock<IEmployeeEvaluationService>();
        _controller = new EmployeeEvaluationController(_employeeEvaluationServiceMock.Object);

        _employeeEvaluationDto = new EmployeeEvaluationDto
        {
            Id = 1,
            Employee = new EmployeeDto
            {
                Id = 1,
                EmployeeNumber = "Emp123",
                TaxNumber = "Tax123",
                EngagementDate = new DateTime(2022, 1, 1),
                TerminationDate = null,
                PeopleChampion = 1,
                Disability = false,
                DisabilityNotes = "No disability",
                Level = 2,
                EmployeeType = new EmployeeTypeDto { Id = 1, Name = "Full Time" },
                Notes = "Notes",
                LeaveInterval = 20.0f,
                SalaryDays = 15.0f,
                PayRate = 50.0f,
                Salary = 50000,
                Name = "John Doe",
                Initials = "JD",
                Surname = "Doe",
                DateOfBirth = new DateTime(1990, 1, 1),
                CountryOfBirth = "South Africa",
                Nationality = "South African",
                IdNumber = "123456789",
                PassportNumber = "AB123456",
                PassportExpirationDate = new DateTime(2025, 1, 1),
                PassportCountryIssue = "South Africa",
                Race = Race.White,
                Gender = Gender.Male,
                Photo = "photo.jpg",
                Email = "test@retrorabbit.co.za",
                PersonalEmail = "john.doe.personal@example.com",
                CellphoneNo = "1234567890",
                ClientAllocated = 1,
                TeamLead = 1,

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
                HouseNo = "12",
                EmergencyContactName = "Emergency Contact",
                EmergencyContactNo = "987654321"
            },
            Template = new EmployeeEvaluationTemplateDto
            {
                Id = 1,
                Description = "Sample Description"
            },
            Owner = new EmployeeDto
            {
                Id = 2,
                EmployeeNumber = "Emp124",
                TaxNumber = "Tax124",
                EngagementDate = new DateTime(2022, 1, 1),
                TerminationDate = null,
                PeopleChampion = 1,
                Disability = false,
                DisabilityNotes = "No disability",
                Level = 2,
                EmployeeType = new EmployeeTypeDto { Id = 1, Name = "Full Time" },
                Notes = "Notes",
                LeaveInterval = 20.0f,
                SalaryDays = 15.0f,
                PayRate = 50.0f,
                Salary = 50000,
                Name = "John Doe",
                Initials = "JD",
                Surname = "Doe",
                DateOfBirth = new DateTime(1990, 1, 1),
                CountryOfBirth = "South Africa",
                Nationality = "South African",
                IdNumber = "123456789",
                PassportNumber = "AB123456",
                PassportExpirationDate = new DateTime(2025, 1, 1),
                PassportCountryIssue = "South Africa",
                Race = Race.White,
                Gender = Gender.Male,
                Photo = "photo.jpg",
                Email = "john.doe@example.com",
                PersonalEmail = "john.doe.personal@example.com",
                CellphoneNo = "1234567890",
                ClientAllocated = 1,
                TeamLead = 1,
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
                HouseNo = "12",
                EmergencyContactName = "Emergency Contact",
                EmergencyContactNo = "987654321"
            },
            Subject = "Employee Evaluation Subject",
            StartDate = new DateOnly(2022, 1, 1),
            EndDate = new DateOnly(2022, 2, 1)
        };

        _employeeEvaluationDtoList = new List<EmployeeEvaluationDto>
        {
            _employeeEvaluationDto,
            _employeeEvaluationDto
        };

        _employeeEvaluationInput = new EmployeeEvaluationInput
        {
            Id = 1,
            OwnerEmail = "owner@retrorabbit.co.za",
            EmployeeEmail = "employee@retrorabbit.co.za",
            Template = "Template 1",
            Subject = "Subject 1"
        };

        _employeeEvaluationInputList = new List<EmployeeEvaluationInput>
        {
            new EmployeeEvaluationInput
            {
              Id = 1,
              OwnerEmail = "owner@retrorabbit.co.za",
              EmployeeEmail = "employee@retrorabbit.co.za",
              Template = "Template 1",
              Subject = "Subject 1"
            },

            new EmployeeEvaluationInput
            {
              Id = 2,
              OwnerEmail = "owner@retrorabbit.co.za",
              EmployeeEmail = "employee@retrorabbit.co.za",
              Template = "Template 2",
              Subject = "Subject 2"
            }
        };
    }

    [Fact]
    public async Task GetAllEmployeeEvaluationsValidEmailReturnsOkResultWithEvaluations()
    {
        _employeeEvaluationServiceMock.Setup(x => x.GetAllEvaluationsByEmail("test@retrorabbit.co.za")).ReturnsAsync(_employeeEvaluationDtoList);

        var result = await _controller.GetAllEmployeeEvaluations("test@retrorabbit.co.za");
        var okResult = Assert.IsType<OkObjectResult>(result);
        var actualEvaluations = Assert.IsType<List<EmployeeEvaluationDto>>(okResult.Value);

        Assert.Equal(_employeeEvaluationDtoList.Count, actualEvaluations.Count);
    }

    [Fact]
    public async Task GetAllEmployeeEvaluationsExceptionThrownReturnsNotFoundResultWithErrorMessage()
    {
        _employeeEvaluationServiceMock.Setup(x => x.GetAllEvaluationsByEmail("test@retrorabbit.co.za"))
                   .ThrowsAsync(new Exception("Error retrieving employee evaluations"));

        var result = await _controller.GetAllEmployeeEvaluations("test@retrorabbit.co.za");
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        var actualErrorMessage = Assert.IsType<string>(notFoundResult.Value);

        Assert.Equal("Error retrieving employee evaluations", actualErrorMessage);
    }

    [Fact]
    public async Task GetEmployeeEvaluation_ValidParameters_ReturnsOkResultWithEvaluation()
    {
        var employeeEmail = "test.employee@example.com";
        var ownerEmail = "test.owner@example.com";
        var template = "SampleTemplate";
        var subject = "SampleSubject";

        _employeeEvaluationServiceMock.Setup(x => x.Get(employeeEmail, ownerEmail, template, subject)).ReturnsAsync(_employeeEvaluationDto);

        var result = await _controller.GetEmployeeEvaluation(employeeEmail, ownerEmail, template, subject);
        var okResult = Assert.IsType<OkObjectResult>(result);
        var actualEvaluation = Assert.IsType<EmployeeEvaluationDto>(okResult.Value);

        Assert.Equal(_employeeEvaluationDto.Id, actualEvaluation.Id);
        Assert.Equal(_employeeEvaluationDto.Subject, actualEvaluation.Subject);
        Assert.Equal(_employeeEvaluationDto.StartDate, actualEvaluation.StartDate);
        Assert.Equal(_employeeEvaluationDto.EndDate, actualEvaluation.EndDate);
        Assert.Equal(_employeeEvaluationDto.Employee?.Id, actualEvaluation.Employee?.Id);
        Assert.Equal(_employeeEvaluationDto.Employee?.EmployeeNumber, actualEvaluation.Employee?.EmployeeNumber);
    }

    [Fact]
    public async Task GetEmployeeEvaluationInvalidParametersReturnsNotFoundResultWithErrorMessage()
    {
        var employeeEmail = "employee@retrorabbit.co.za";
        var ownerEmail = "owner@retrorabbit.co.za";
        var template = "Employee Evaluation Template";
        var subject = "Employee Evaluation Subject";
        var errorMessage = "Error retrieving employee evaluation";

        _employeeEvaluationServiceMock.Setup(x => x.Get(employeeEmail, ownerEmail, template, subject))
                   .ThrowsAsync(new Exception(errorMessage));

        var result = await _controller.GetEmployeeEvaluation(employeeEmail, ownerEmail, template, subject);

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        var actualErrorMessage = Assert.IsType<string>(notFoundResult.Value);
    }

    [Fact]
    public async Task SaveEmployeeEvaluationValidInputReturnsOkResultWithSavedEvaluation()
    {
        _employeeEvaluationServiceMock.Setup(x => x.Save(It.IsAny<EmployeeEvaluationInput>())).ReturnsAsync(_employeeEvaluationDto);

        var result = await _controller.SaveEmployeeEvaluation(_employeeEvaluationInput);
        var okResult = Assert.IsType<OkObjectResult>(result);
        var actualSavedEvaluation = Assert.IsType<EmployeeEvaluationDto>(okResult.Value);

        Assert.Equal(_employeeEvaluationDto.Id, actualSavedEvaluation.Id);
        Assert.Equal(_employeeEvaluationDto.Subject, actualSavedEvaluation.Subject);
        Assert.Equal(_employeeEvaluationDto.StartDate, actualSavedEvaluation.StartDate);
        Assert.Equal(_employeeEvaluationDto.EndDate, actualSavedEvaluation.EndDate);
        Assert.Equal(_employeeEvaluationDto.Employee?.Id, actualSavedEvaluation.Employee?.Id);
        Assert.Equal(_employeeEvaluationDto.Employee?.EmployeeNumber, actualSavedEvaluation.Employee?.EmployeeNumber);
        Assert.Equal(_employeeEvaluationDto.Template?.Id, actualSavedEvaluation.Template?.Id);
        Assert.Equal(_employeeEvaluationDto.Template?.Description, actualSavedEvaluation.Template?.Description);
        Assert.Equal(_employeeEvaluationDto.Owner?.Id, actualSavedEvaluation.Owner?.Id);
        Assert.Equal(_employeeEvaluationDto.Owner?.EmployeeNumber, actualSavedEvaluation.Owner?.EmployeeNumber);
    }

    [Fact]
    public async Task SaveEmployeeEvaluationInvalidInputReturnsNotFoundResultWithErrorMessage()
    {
        _employeeEvaluationServiceMock.Setup(x => x.Save(_employeeEvaluationInput))
                   .ThrowsAsync(new Exception("Invalid input error message"));

        var result = await _controller.SaveEmployeeEvaluation(_employeeEvaluationInput);
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        var actualErrorMessage = Assert.IsType<string>(notFoundResult.Value);

        Assert.Equal("Invalid input error message", actualErrorMessage);
    }


    [Fact]
    public async Task UpdateEmployeeEvaluationValidInputReturnsOkResult()
    {
        _employeeEvaluationServiceMock.Setup(x => x.Update(It.IsAny<EmployeeEvaluationInput>(), It.IsAny<EmployeeEvaluationInput>()))
                   .ReturnsAsync(_employeeEvaluationDto);

        var result = await _controller.UpdateEmployeeEvaluation(_employeeEvaluationInputList);

        Assert.IsType<OkResult>(result);
    }

    [Fact]
    public async Task UpdateEmployeeEvaluationInvalidInputReturnsNotFoundResultWithErrorMessage()
    {
        _employeeEvaluationServiceMock.Setup(x => x.Update(It.IsAny<EmployeeEvaluationInput>(), It.IsAny<EmployeeEvaluationInput>()))
                   .ThrowsAsync(new Exception("Invalid input error message"));

        var result = await _controller.UpdateEmployeeEvaluation(_employeeEvaluationInputList);
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        var actualErrorMessage = Assert.IsType<string>(notFoundResult.Value);

        Assert.Equal("Invalid input error message", actualErrorMessage);
    }

    [Fact]
    public async Task DeleteEmployeeEvaluationValidInputReturnsOkResult()
    {
        _employeeEvaluationServiceMock.Setup(x => x.Delete(_employeeEvaluationInput))
                   .ReturnsAsync(_employeeEvaluationDto);

        var result = await _controller.DeleteEmployeeEvaluation(_employeeEvaluationInput);

        Assert.IsType<OkResult>(result);
    }

    [Fact]
    public async Task DeleteEmployeeEvaluationInvalidInputReturnsNotFoundResultWithErrorMessage()
    {
        var errorMessage = "Invalid input error message";
        _employeeEvaluationServiceMock.Setup(x => x.Delete(_employeeEvaluationInput))
                   .ThrowsAsync(new Exception(errorMessage));

        var result = await _controller.DeleteEmployeeEvaluation(_employeeEvaluationInput);
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        var actualErrorMessage = Assert.IsType<string>(notFoundResult.Value);

        Assert.Equal(errorMessage, actualErrorMessage);
    }
}