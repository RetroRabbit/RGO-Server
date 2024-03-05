using HRIS.Models;
using HRIS.Models.Enums;
using HRIS.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Org.BouncyCastle.Utilities;
using RR.App.Controllers.HRIS;
using RR.UnitOfWork.Entities.HRIS;
using System.Net.NetworkInformation;
using System.Xml.Linq;
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

                    Employee = new EmployeeDto
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

                    Description = "exampleDescription",
                    Score = 4.5f,
                    Comment = "exampleComment"
                }
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

        var employeeDto = new EmployeeDto
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
            EmergencyContactNo = "+9876543210"
        };


        var templateDto = new EmployeeEvaluationTemplateDto { Id = 301, Description = "Template 1" };

        var savedRating = new EmployeeEvaluationRatingDto
        {
            Id = 1,
            Evaluation = new EmployeeEvaluationDto
            {
                Id = 201,
                Employee = employeeDto,
                Template = templateDto,
                Owner = employeeDto,
                Subject = "Subject 1",
                StartDate = DateOnly.FromDateTime(DateTime.Now),
                EndDate = null
            },
            Employee = employeeDto,
            Description = "Test Description",
            Score = 5.0f,
            Comment = "Test Comment"
        };

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

        var employeeDto = new EmployeeDto
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
            EmergencyContactNo = "+9876543210"
        };


        var evaluationDto = new EmployeeEvaluationDto
        {
            Id = 101,
            Employee = new EmployeeDto
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
                EmergencyContactNo = "+9876543210"
            },

            Template = new EmployeeEvaluationTemplateDto { Id = 301, Description = "Template1" },
            Owner = new EmployeeDto
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
                EmergencyContactNo = "+9876543210"
            },

            Subject = "Subject1",
            StartDate = DateOnly.FromDateTime(DateTime.Now),
            EndDate = null
        };

        var originalRatingDto = new EmployeeEvaluationRatingDto
        {
            Id = 1,
            Evaluation = new EmployeeEvaluationDto
            {
                Id = 101,
                Employee = new EmployeeDto
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
                    EmergencyContactNo = "+9876543210"
                },

                Template = new EmployeeEvaluationTemplateDto
                {
                    Id = 301,
                    Description = "Template1"
                },
                Owner = new EmployeeDto
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
                    EmergencyContactNo = "+9876543210"
                },

                Subject = "Subject1",
                StartDate = DateOnly.FromDateTime(DateTime.Now),
                EndDate = null
            },

            Employee = new EmployeeDto
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
        Description = "exampleDescription",
            Score = 4.5f,
            Comment = "exampleComment"
        };

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

        var employeeDto = new EmployeeDto
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
        };

        var templateDto = new EmployeeEvaluationTemplateDto { Id = 301, Description = "Template 1" };

        var serviceMock = new Mock<IEmployeeEvaluationRatingService>();
        serviceMock.Setup(x => x.Delete(ratingInput)).ReturnsAsync(new EmployeeEvaluationRatingDto
        {
            Id = 1,
            Evaluation = new EmployeeEvaluationDto
            {
                Id = 201,
                Employee = employeeDto,
                Template = templateDto,
                Owner = employeeDto,
                Subject = "Subject 1",
                StartDate = DateOnly.FromDateTime(DateTime.Now),
                EndDate = null
            },
            Employee = employeeDto,
            Description = "Test Description",
            Score = 5.0f,
            Comment = "Test Comment"
        });

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
