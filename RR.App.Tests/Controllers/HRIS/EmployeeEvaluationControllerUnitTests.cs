using HRIS.Models;
using HRIS.Models.Enums;
using HRIS.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RR.App.Controllers.HRIS;
using RR.UnitOfWork.Entities.HRIS;
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
        new EmployeeEvaluationDto
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


        Template = new EmployeeEvaluationTemplateDto{ Id = 1, Description = "Employee Evaluation Template 1" },
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
                EndDate = new DateOnly(2022, 2, 1),
            }
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
        {
            Id = 1,
            OwnerEmail = "owner@retrorabbit.co.za",
            EmployeeEmail = "employee@retrorabbit.co.za",
            Template = "Template 1",
            Subject = "Subject 1"
        };

        var expectedSavedEvaluation = new EmployeeEvaluationDto
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
        { Id = 1, OwnerEmail = string.Empty, EmployeeEmail = "employee@retrorabbit.co.za", Template = "template", Subject = "Evaluation Subject 1" };

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

        mockService.Setup(x => x.Update(It.IsAny<EmployeeEvaluationInput>(), It.IsAny<EmployeeEvaluationInput>()))
                   .ReturnsAsync(new EmployeeEvaluationDto
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
                   });

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
            new EmployeeEvaluationInput{ Id = 0, OwnerEmail = string.Empty, EmployeeEmail = "invalidemail", Template = "template", Subject = string.Empty },
            new EmployeeEvaluationInput{Id = -1, OwnerEmail = "owner@retrorabbit.co.za", EmployeeEmail = "employee@retrorabbit.co.za", Template ="Template 1", Subject="Subject 1" }
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
        { Id = 1, OwnerEmail = "owner@retrorabbit.co.za", EmployeeEmail = "employee@retrorabbit.co.za", Template = "Template 1", Subject = "Subject 1" };

        mockService.Setup(x => x.Delete(evaluationInput))
                   .ReturnsAsync(new EmployeeEvaluationDto
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
                          Id =  1,
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
                   });

        var result = await controller.DeleteEmployeeEvaluation(evaluationInput);

        Assert.IsType<OkResult>(result);
    }

    [Fact]
    public async Task DeleteEmployeeEvaluationInvalidInputReturnsNotFoundResultWithErrorMessage()
    {
        var mockService = new Mock<IEmployeeEvaluationService>();
        var controller = new EmployeeEvaluationController(mockService.Object);

        var invalidEvaluationInput = new EmployeeEvaluationInput
        { Id = 0, OwnerEmail = string.Empty, EmployeeEmail="invalidemail", Template = "template", Subject = string.Empty};

        var errorMessage = "Invalid input error message";
        mockService.Setup(x => x.Delete(invalidEvaluationInput))
                   .ThrowsAsync(new Exception(errorMessage));

        var result = await controller.DeleteEmployeeEvaluation(invalidEvaluationInput);

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        var actualErrorMessage = Assert.IsType<string>(notFoundResult.Value);

        Assert.Equal(errorMessage, actualErrorMessage);
    }
}
