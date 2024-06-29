using System.Linq.Expressions;
using HRIS.Models;
using HRIS.Models.Enums;
using HRIS.Services.Interfaces;
using HRIS.Services.Services;
using Moq;
using RR.Tests.Data;
using RR.UnitOfWork;
using RR.UnitOfWork.Entities.HRIS;
using Xunit;

namespace RGO.Tests.Services;

public class EmployeeEvaluationRatingServiceUnitTests
{
    private readonly Mock<IUnitOfWork> _dbMock;
    private readonly Mock<IErrorLoggingService> _errorLoggingServiceMock;
    private readonly EmployeeDto _employee;
    private readonly EmployeeEvaluationRatingService _employeeEvaluationRatingService;
    private readonly Mock<IEmployeeEvaluationService> _employeeEvaluationServiceMock;
    private readonly EmployeeEvaluationTemplateDto _employeeEvaluationTemplate;
    private readonly Mock<IEmployeeService> _employeeServiceMock;

    public EmployeeEvaluationRatingServiceUnitTests()
    {
        _dbMock = new Mock<IUnitOfWork>();
        _employeeServiceMock = new Mock<IEmployeeService>();
        _employeeEvaluationServiceMock = new Mock<IEmployeeEvaluationService>();
        _errorLoggingServiceMock = new Mock<IErrorLoggingService>();

        _employeeEvaluationRatingService =
            new EmployeeEvaluationRatingService(_dbMock.Object, _employeeEvaluationServiceMock.Object,
                                                _employeeServiceMock.Object, _errorLoggingServiceMock.Object);

        var employeeTypeDto = new EmployeeTypeDto { Id = 1, Name = "Developer" };
        var employeeAddressDto =
            new EmployeeAddressDto { Id = 1, UnitNumber = "2", ComplexName = "Complex", StreetNumber = "2", SuburbOrDistrict = "Suburb/District", City = "City", Country = "Country", Province = "Province", PostalCode = "1620" };

        _employee = new EmployeeDto
        {
            Id = 1,
            EmployeeNumber = "001",
            TaxNumber = "34434434",
            EngagementDate = DateTime.Now,
            TerminationDate = DateTime.Now,
            PeopleChampion = null,
            Disability = false,
            DisabilityNotes = "None",
            Level = 4,
            EmployeeType = employeeTypeDto,
            Notes = "Notes",
            LeaveInterval = 1,
            SalaryDays = 28,
            PayRate = 128,
            Salary = 100000,
            Name = "Dorothy",
            Initials = "D",
            Surname = "Mahoko",
            DateOfBirth = DateTime.Now,
            CountryOfBirth = "South Africa",
            Nationality = "South African",
            IdNumber = "0000080000000",
            PassportNumber = " ",
            PassportExpirationDate = DateTime.Now,
            PassportCountryIssue = null,
            Race = Race.Black,
            Gender = Gender.Male,
            Email = "texample@retrorabbit.co.za",
            PersonalEmail = "test.example@gmail.com",
            CellphoneNo = "0000000000",
            PhysicalAddress = employeeAddressDto,
            PostalAddress = employeeAddressDto
        };

        _employeeEvaluationTemplate = new EmployeeEvaluationTemplateDto { Id = 1, Description = "template" };
    }

    private EmployeeEvaluation CreateEmployeeEvaluation(
        EmployeeDto? employee = null,
        EmployeeDto? owner = null,
        EmployeeEvaluationTemplateDto? template = null,
        string subject = "subject")
    {
        var employeeEvaluation = new EmployeeEvaluation
        {
            Id = 1,
            EmployeeId = employee?.Id ?? 0,
            OwnerId = owner?.Id ?? 0,
            TemplateId = template?.Id ?? 0,
            Subject = subject,
            StartDate = new DateOnly(2021, 1, 1)
        };

        if (employee != null)
            employeeEvaluation.Employee = new Employee(employee, employee.EmployeeType!);

        if (owner != null)
            employeeEvaluation.Owner = new Employee(owner, owner.EmployeeType!);

        if (template != null)
            employeeEvaluation.Template = new EmployeeEvaluationTemplate(template);

        return employeeEvaluation;
    }

    [Fact]
    public async Task CheckIfExistsTest()
    {
        var evaluation = CreateEmployeeEvaluation(_employee, _employee, _employeeEvaluationTemplate);
        var employeeEvaluationRating = CreateRating(evaluation, _employee);
        var employeeEvaluationInput = new EmployeeEvaluationInput
        {
            Id = evaluation.Id,
            OwnerEmail = evaluation.Owner.Email!,
            EmployeeEmail = evaluation.Employee.Email!,
            Template = evaluation.Template.Description,
            Subject = evaluation.Subject
        };

        var evaluationInput = new EvaluationRatingInput(
                                                        0,
                                                        employeeEvaluationRating.Employee.Email!,
                                                        employeeEvaluationInput,
                                                        employeeEvaluationRating.Description,
                                                        employeeEvaluationRating.Score,
                                                        employeeEvaluationRating.Comment);

        _dbMock
            .SetupSequence(x =>
                               x.EmployeeEvaluationRating
                                .Any(It.IsAny<Expression<Func<EmployeeEvaluationRating, bool>>>()))
            .ReturnsAsync(true)
            .ReturnsAsync(false);

        var exists = await _employeeEvaluationRatingService.CheckIfExists(evaluationInput);
        Assert.True(exists);

        exists = await _employeeEvaluationRatingService.CheckIfExists(evaluationInput);
        Assert.False(exists);
    }

    [Fact]
    public async Task GetRatingFailTest()
    {
        var employeeEvaluation = CreateEmployeeEvaluation(_employee, _employee, _employeeEvaluationTemplate);
        var employeeEvaluationRating = CreateRating(employeeEvaluation, _employee);
        var evaluationInput = new EmployeeEvaluationInput
        {
            Id = employeeEvaluation.Id,
            OwnerEmail = employeeEvaluation.Owner.Email!,
            EmployeeEmail = employeeEvaluation.Employee.Email!,
            Template = employeeEvaluation.Template.Description,
            Subject = employeeEvaluation.Subject
        };

        var evaluationRatingInput = new EvaluationRatingInput(
                                                              0,
                                                              employeeEvaluationRating.Employee.Email!,
                                                              evaluationInput,
                                                              employeeEvaluationRating.Description,
                                                              employeeEvaluationRating.Score,
                                                              employeeEvaluationRating.Comment);

        _employeeEvaluationServiceMock.SetupSequence(x => x.CheckIfExists(It.IsAny<EmployeeEvaluationInput>()))
                                      .ReturnsAsync(false)
                                      .ReturnsAsync(true);

        _employeeEvaluationServiceMock
            .Setup(x => x.Get(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(employeeEvaluation.ToDto());

        _dbMock.Setup(x => x.EmployeeEvaluationRating.Any(It.IsAny<Expression<Func<EmployeeEvaluationRating, bool>>>()))
               .ReturnsAsync(false);

        _errorLoggingServiceMock.Setup(r => r.LogException(It.IsAny<Exception>())).Throws(new Exception());

        await Assert.ThrowsAsync<Exception>(() => _employeeEvaluationRatingService.Get(evaluationRatingInput));
        await Assert.ThrowsAsync<Exception>(() => _employeeEvaluationRatingService.Get(evaluationRatingInput));
    }

    [Fact]
    public async Task GetRatingPassTest()
    {
        var employeeEvaluation = CreateEmployeeEvaluation(_employee, _employee, _employeeEvaluationTemplate);
        var employeeEvaluationRating = CreateRating(employeeEvaluation, _employee);
        var evaluationInput = new EmployeeEvaluationInput
        {
            Id = employeeEvaluation.Id,
            OwnerEmail = employeeEvaluation.Owner.Email!,
            EmployeeEmail = employeeEvaluation.Employee.Email!,
            Template = employeeEvaluation.Template.Description,
            Subject = employeeEvaluation.Subject
        };

        var evaluationRatingInput = new EvaluationRatingInput(
                                                              0,
                                                              employeeEvaluationRating.Employee.Email!,
                                                              evaluationInput,
                                                              employeeEvaluationRating.Description,
                                                              employeeEvaluationRating.Score,
                                                              employeeEvaluationRating.Comment);

        _employeeEvaluationServiceMock.Setup(x => x.CheckIfExists(It.IsAny<EmployeeEvaluationInput>()))
                                      .ReturnsAsync(true);

        _employeeEvaluationServiceMock
            .Setup(x => x.Get(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(employeeEvaluation.ToDto());

        var employeeEvaluationRatings = new EmployeeEvaluationRating
        {
            Id = 1,
            EmployeeEvaluationId = employeeEvaluation.Id,
            EmployeeId = employeeEvaluation.EmployeeId,
            Evaluation = employeeEvaluation,
            Score = 1,
            Comment = "comment"
        }.ToMockIQueryable();

        _dbMock.Setup(x => x.EmployeeEvaluationRating.Any(It.IsAny<Expression<Func<EmployeeEvaluationRating, bool>>>()))
               .ReturnsAsync(true);

        _dbMock.Setup(x => x.EmployeeEvaluationRating.Get(It.IsAny<Expression<Func<EmployeeEvaluationRating, bool>>>()))
               .Returns(employeeEvaluationRatings.Where(x => x.EmployeeEvaluationId == employeeEvaluation.Id));

        var result = await _employeeEvaluationRatingService.Get(evaluationRatingInput);

        Assert.Equal(employeeEvaluationRatings.First().Comment, result.Comment);
    }

    private EmployeeEvaluationRating CreateRating(
        EmployeeEvaluation? evaluation = null,
        EmployeeDto? employee = null,
        int score = 1,
        string comment = "comment",
        string description = "description")
    {
        var employeeEvaluationRating = new EmployeeEvaluationRating
        {
            Id = 1,
            EmployeeEvaluationId = evaluation?.Id ?? 0,
            EmployeeId = employee?.Id ?? 0,
            Description = description,
            Score = score,
            Comment = comment
        };

        if (evaluation != null) employeeEvaluationRating.Evaluation = evaluation;

        if (employee != null) employeeEvaluationRating.Employee = new Employee(employee, employee.EmployeeType!);

        return employeeEvaluationRating;
    }

    [Fact]
    public async Task SaveFailTest()
    {
        var employeeEvaluation = CreateEmployeeEvaluation(_employee, _employee, _employeeEvaluationTemplate);
        var employeeEvaluationRating = CreateRating(employeeEvaluation, _employee);
        var evaluationInput = new EmployeeEvaluationInput
        {
            Id = employeeEvaluation.Id,
            OwnerEmail = employeeEvaluation.Owner.Email!,
            EmployeeEmail = employeeEvaluation.Employee.Email!,
            Template = employeeEvaluation.Template.Description,
            Subject = employeeEvaluation.Subject
        };

        var ratingInput = new EvaluationRatingInput(
                                                    0,
                                                    employeeEvaluationRating.Employee.Email!,
                                                    evaluationInput,
                                                    employeeEvaluationRating.Description,
                                                    employeeEvaluationRating.Score,
                                                    employeeEvaluationRating.Comment);

        _employeeServiceMock.Setup(x => x.GetEmployee(It.IsAny<string>()))
                            .ReturnsAsync(_employee);

        _employeeEvaluationServiceMock
            .Setup(x => x.Get(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(employeeEvaluation.ToDto());

        _dbMock.Setup(x => x.EmployeeEvaluationRating.Any(It.IsAny<Expression<Func<EmployeeEvaluationRating, bool>>>()))
               .ReturnsAsync(true);

        _errorLoggingServiceMock.Setup(r => r.LogException(It.IsAny<Exception>())).Throws(new Exception());

        await Assert.ThrowsAsync<Exception>(() => _employeeEvaluationRatingService.Save(ratingInput));
    }

    [Fact]
    public async Task SavePassTest()
    {
        var employeeEvaluation = CreateEmployeeEvaluation(_employee, _employee, _employeeEvaluationTemplate);
        var employeeEvaluationRating = CreateRating(employeeEvaluation, _employee);
        var evaluationInput = new EmployeeEvaluationInput
        {
            Id = employeeEvaluation.Id,
            OwnerEmail = employeeEvaluation.Owner.Email!,
            EmployeeEmail = employeeEvaluation.Employee.Email!,
            Template = employeeEvaluation.Template.Description,
            Subject = employeeEvaluation.Subject
        };

        var ratingInput = new EvaluationRatingInput(
                                                    0,
                                                    employeeEvaluationRating.Employee.Email!,
                                                    evaluationInput,
                                                    employeeEvaluationRating.Description,
                                                    employeeEvaluationRating.Score,
                                                    employeeEvaluationRating.Comment);

        _employeeServiceMock.Setup(x => x.GetEmployee(It.IsAny<string>()))
                            .ReturnsAsync(_employee);

        _employeeEvaluationServiceMock
            .Setup(x => x.Get(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(employeeEvaluation.ToDto());

        _dbMock.Setup(x => x.EmployeeEvaluationRating.Any(It.IsAny<Expression<Func<EmployeeEvaluationRating, bool>>>()))
               .ReturnsAsync(false);

        _dbMock.Setup(x => x.EmployeeEvaluationRating.Add(It.IsAny<EmployeeEvaluationRating>()))
               .ReturnsAsync(employeeEvaluationRating);

        var result = await _employeeEvaluationRatingService.Save(ratingInput);

        Assert.Equal(employeeEvaluationRating.Comment, result.Comment);
    }

    [Fact]
    public async Task UpdateFailTest()
    {
        var employeeEvaluation = CreateEmployeeEvaluation(_employee, _employee, _employeeEvaluationTemplate);
        var employeeEvaluationRating = CreateRating(employeeEvaluation, _employee);
        var evaluationInput = new EmployeeEvaluationInput
        {
            Id = employeeEvaluation.Id,
            OwnerEmail = employeeEvaluation.Owner.Email!,
            EmployeeEmail = employeeEvaluation.Employee.Email!,
            Template = employeeEvaluation.Template.Description,
            Subject = employeeEvaluation.Subject
        };

        var ratingInput = new EvaluationRatingInput(
                                                    0,
                                                    employeeEvaluationRating.Employee.Email!,
                                                    evaluationInput,
                                                    employeeEvaluationRating.Description,
                                                    employeeEvaluationRating.Score,
                                                    employeeEvaluationRating.Comment);

        _dbMock.Setup(x => x.EmployeeEvaluationRating.Any(It.IsAny<Expression<Func<EmployeeEvaluationRating, bool>>>()))
               .ReturnsAsync(false);

        _errorLoggingServiceMock.Setup(r => r.LogException(It.IsAny<Exception>())).Throws(new Exception());

        await Assert.ThrowsAsync<Exception>(() => _employeeEvaluationRatingService.Update(ratingInput));
    }

    [Fact]
    public async Task UpdatePassTest()
    {
        var employeeEvaluation = CreateEmployeeEvaluation(_employee, _employee, _employeeEvaluationTemplate);
        var employeeEvaluationRating = CreateRating(employeeEvaluation, _employee);
        var evaluationInput = new EmployeeEvaluationInput
        {
            Id = employeeEvaluation.Id,
            OwnerEmail = employeeEvaluation.Owner.Email!,
            EmployeeEmail = employeeEvaluation.Employee.Email!,
            Template = employeeEvaluation.Template.Description,
            Subject = employeeEvaluation.Subject
        };

        var ratingInput = new EvaluationRatingInput(
                                                    0,
                                                    employeeEvaluationRating.Employee.Email!,
                                                    evaluationInput,
                                                    employeeEvaluationRating.Description,
                                                    employeeEvaluationRating.Score,
                                                    employeeEvaluationRating.Comment);

        _dbMock
            .SetupSequence(x =>
                               x.EmployeeEvaluationRating
                                .Any(It.IsAny<Expression<Func<EmployeeEvaluationRating, bool>>>()))
            .ReturnsAsync(true)
            .ReturnsAsync(true);

        _employeeEvaluationServiceMock
            .Setup(x => x.Get(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(employeeEvaluation.ToDto());

        _employeeEvaluationServiceMock.Setup(x => x.CheckIfExists(It.IsAny<EmployeeEvaluationInput>()))
                                      .ReturnsAsync(true);

        _employeeEvaluationServiceMock
            .Setup(x => x.Get(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(employeeEvaluation.ToDto());

        _dbMock.Setup(x => x.EmployeeEvaluationRating.Get(It.IsAny<Expression<Func<EmployeeEvaluationRating, bool>>>()))
               .Returns(employeeEvaluationRating.ToMockIQueryable()
                            .Where(x => x.EmployeeEvaluationId == employeeEvaluation.Id));

        _dbMock.Setup(x => x.EmployeeEvaluationRating.Update(It.IsAny<EmployeeEvaluationRating>()))
               .ReturnsAsync(employeeEvaluationRating);

        var result = await _employeeEvaluationRatingService.Update(ratingInput);

        Assert.Equal(employeeEvaluationRating.Comment, result.Comment);
    }

    [Fact]
    public async Task DeleteFailTest()
    {
        var employeeEvaluation = CreateEmployeeEvaluation(_employee, _employee, _employeeEvaluationTemplate);
        var employeeEvaluationRating = CreateRating(employeeEvaluation, _employee);
        var evaluationInput = new EmployeeEvaluationInput
        {
            Id = employeeEvaluation.Id,
            OwnerEmail = employeeEvaluation.Owner.Email!,
            EmployeeEmail = employeeEvaluation.Employee.Email!,
            Template = employeeEvaluation.Template.Description,
            Subject = employeeEvaluation.Subject
        };

        var ratingInput = new EvaluationRatingInput(
                                                    0,
                                                    employeeEvaluationRating.Employee.Email!,
                                                    evaluationInput,
                                                    employeeEvaluationRating.Description,
                                                    employeeEvaluationRating.Score,
                                                    employeeEvaluationRating.Comment);

        _employeeEvaluationServiceMock.SetupSequence(x => x.CheckIfExists(It.IsAny<EmployeeEvaluationInput>()))
                                      .ReturnsAsync(false)
                                      .ReturnsAsync(true);

        _employeeEvaluationServiceMock
            .Setup(x => x.Get(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(employeeEvaluation.ToDto());

        _dbMock.Setup(x => x.EmployeeEvaluationRating.Any(It.IsAny<Expression<Func<EmployeeEvaluationRating, bool>>>()))
               .ReturnsAsync(false);

        _errorLoggingServiceMock.Setup(r => r.LogException(It.IsAny<Exception>())).Throws(new Exception());

        await Assert.ThrowsAsync<Exception>(() => _employeeEvaluationRatingService.Delete(ratingInput));
        await Assert.ThrowsAsync<Exception>(() => _employeeEvaluationRatingService.Delete(ratingInput));
    }

    [Fact]
    public async Task DeletePassTest()
    {
        var employeeEvaluation = CreateEmployeeEvaluation(_employee, _employee, _employeeEvaluationTemplate);
        var employeeEvaluationRating = CreateRating(employeeEvaluation, _employee);
        var evaluationInput = new EmployeeEvaluationInput
        {
            Id = employeeEvaluation.Id,
            OwnerEmail = employeeEvaluation.Owner.Email!,
            EmployeeEmail = employeeEvaluation.Employee.Email!,
            Template = employeeEvaluation.Template.Description,
            Subject = employeeEvaluation.Subject
        };

        var ratingInput = new EvaluationRatingInput(
                                                    0,
                                                    employeeEvaluationRating.Employee.Email!,
                                                    evaluationInput,
                                                    employeeEvaluationRating.Description,
                                                    employeeEvaluationRating.Score,
                                                    employeeEvaluationRating.Comment);

        _employeeEvaluationServiceMock.SetupSequence(x => x.CheckIfExists(It.IsAny<EmployeeEvaluationInput>()))
                                      .ReturnsAsync(true)
                                      .ReturnsAsync(true);

        _employeeEvaluationServiceMock
            .SetupSequence(x => x.Get(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(employeeEvaluation.ToDto())
            .ReturnsAsync(employeeEvaluation.ToDto());

        _dbMock
            .SetupSequence(x =>
                               x.EmployeeEvaluationRating
                                .Any(It.IsAny<Expression<Func<EmployeeEvaluationRating, bool>>>()))
            .ReturnsAsync(true)
            .ReturnsAsync(true);

        _dbMock.Setup(x => x.EmployeeEvaluationRating.Get(It.IsAny<Expression<Func<EmployeeEvaluationRating, bool>>>()))
               .Returns(employeeEvaluationRating.ToMockIQueryable()
                            .Where(x => x.EmployeeEvaluationId == employeeEvaluation.Id));

        _dbMock.Setup(x => x.EmployeeEvaluationRating.Delete(It.IsAny<int>()))
               .ReturnsAsync(employeeEvaluationRating);

        var result = await _employeeEvaluationRatingService.Delete(ratingInput);

        Assert.Equal(employeeEvaluationRating.Comment, result.Comment);
    }

    [Fact]
    public async Task GetFailTest()
    {
        var employeeEvaluation = CreateEmployeeEvaluation(_employee, _employee, _employeeEvaluationTemplate);
        var employeeEvaluationRating = CreateRating(employeeEvaluation, _employee);
        var evaluationInput = new EmployeeEvaluationInput
        {
            Id = employeeEvaluation.Id,
            OwnerEmail = employeeEvaluation.Owner.Email!,
            EmployeeEmail = employeeEvaluation.Employee.Email!,
            Template = employeeEvaluation.Template.Description,
            Subject = employeeEvaluation.Subject
        };

        var evaluationRatingInput = new EvaluationRatingInput(
                                                              0,
                                                              employeeEvaluationRating.Employee.Email!,
                                                              evaluationInput,
                                                              employeeEvaluationRating.Description,
                                                              employeeEvaluationRating.Score,
                                                              employeeEvaluationRating.Comment);

        _employeeEvaluationServiceMock.SetupSequence(x => x.CheckIfExists(It.IsAny<EmployeeEvaluationInput>()))
                                      .ReturnsAsync(false)
                                      .ReturnsAsync(true);

        _employeeEvaluationServiceMock
            .Setup(x => x.Get(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(employeeEvaluation.ToDto());

        _dbMock.Setup(x => x.EmployeeEvaluationRating.Any(It.IsAny<Expression<Func<EmployeeEvaluationRating, bool>>>()))
               .ReturnsAsync(false);

        _errorLoggingServiceMock.Setup(r => r.LogException(It.IsAny<Exception>())).Throws(new Exception());

        await Assert.ThrowsAsync<Exception>(() => _employeeEvaluationRatingService.Get(evaluationRatingInput));
        await Assert.ThrowsAsync<Exception>(() => _employeeEvaluationRatingService.Get(evaluationRatingInput));
    }

    [Fact]
    public async Task GetPassTest()
    {
        var employeeEvaluation = CreateEmployeeEvaluation(_employee, _employee, _employeeEvaluationTemplate);
        var employeeEvaluationRating = CreateRating(employeeEvaluation, _employee);
        var evaluationInput = new EmployeeEvaluationInput
        {
            Id = employeeEvaluation.Id,
            OwnerEmail = employeeEvaluation.Owner.Email!,
            EmployeeEmail = employeeEvaluation.Employee.Email!,
            Template = employeeEvaluation.Template.Description,
            Subject = employeeEvaluation.Subject
        };

        var evaluationRatingInput = new EvaluationRatingInput(
                                                              0,
                                                              employeeEvaluationRating.Employee.Email!,
                                                              evaluationInput,
                                                              employeeEvaluationRating.Description,
                                                              employeeEvaluationRating.Score,
                                                              employeeEvaluationRating.Comment);

        _employeeEvaluationServiceMock.Setup(x => x.CheckIfExists(It.IsAny<EmployeeEvaluationInput>()))
                                      .ReturnsAsync(true);

        _employeeEvaluationServiceMock
            .Setup(x => x.Get(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(employeeEvaluation.ToDto());

        var employeeEvaluationRatings = new EmployeeEvaluationRating
        {
            Id = 1,
            EmployeeEvaluationId = employeeEvaluation.Id,
            EmployeeId = employeeEvaluation.EmployeeId,
            Evaluation = employeeEvaluation,
            Score = 1,
            Comment = "comment"
        }.ToMockIQueryable();

        _dbMock.Setup(x => x.EmployeeEvaluationRating.Any(It.IsAny<Expression<Func<EmployeeEvaluationRating, bool>>>()))
               .ReturnsAsync(true);

        _dbMock.Setup(x => x.EmployeeEvaluationRating.Get(It.IsAny<Expression<Func<EmployeeEvaluationRating, bool>>>()))
               .Returns(employeeEvaluationRatings.Where(x => x.EmployeeEvaluationId == employeeEvaluation.Id));

        var result = await _employeeEvaluationRatingService.Get(evaluationRatingInput);

        Assert.Equal(employeeEvaluationRatings.First().Comment, result.Comment);
    }

    [Fact]
    public async Task GetAllTest()
    {
        var employeeEvaluationRatings = new EmployeeEvaluationRating
        {
            Id = 1,
            EmployeeEvaluationId = 1,
            EmployeeId = 1,
            Evaluation = CreateEmployeeEvaluation(_employee, _employee, _employeeEvaluationTemplate),
            Score = 1,
            Comment = "comment"
        }.ToMockIQueryable();

        _dbMock.Setup(x => x.EmployeeEvaluationRating.Get(It.IsAny<Expression<Func<EmployeeEvaluationRating, bool>>>()))
               .Returns(employeeEvaluationRatings);

        var result = await _employeeEvaluationRatingService.GetAll();

        Assert.Equal(employeeEvaluationRatings.First().Comment, result.First().Comment);
    }

    [Fact]
    public async Task GetAllByEmployeeTest()
    {
        var employeeEvaluationRatings = CreateRating(CreateEmployeeEvaluation(_employee, _employee, _employeeEvaluationTemplate), _employee).ToMockIQueryable();

        var targetEmail = _employee.Email;

        Expression<Func<EmployeeEvaluationRating, bool>> criteria = x => x.Employee.Email == targetEmail;

        _dbMock.Setup(x => x.EmployeeEvaluationRating.Get(It.IsAny<Expression<Func<EmployeeEvaluationRating, bool>>>()))
               .Returns(employeeEvaluationRatings.Where(criteria));

        var result = await _employeeEvaluationRatingService.GetAllByEmployee(_employee.Email!);

        Assert.Equal(employeeEvaluationRatings.First().Comment, result.First().Comment);
    }

    [Fact]
    public async Task GetAllByEvaluationFailTest()
    {
        var employeeEvaluation = CreateEmployeeEvaluation(_employee, _employee, _employeeEvaluationTemplate);
        var evaluationInput = new EmployeeEvaluationInput
        {
            Id = employeeEvaluation.Id,
            OwnerEmail = employeeEvaluation.Owner.Email!,
            EmployeeEmail = employeeEvaluation.Employee.Email!,
            Template = employeeEvaluation.Template.Description,
            Subject = employeeEvaluation.Subject
        };

        _employeeEvaluationServiceMock.Setup(x => x.CheckIfExists(It.IsAny<EmployeeEvaluationInput>()))
                                      .ReturnsAsync(false);

        _errorLoggingServiceMock.Setup(r => r.LogException(It.IsAny<Exception>())).Throws(new Exception());

        await Assert.ThrowsAsync<Exception>(() => _employeeEvaluationRatingService.GetAllByEvaluation(evaluationInput));
    }

    [Fact]
    public async Task GetAllByEvaluationPassTest()
    {
        var employeeEvaluation = CreateEmployeeEvaluation(_employee, _employee, _employeeEvaluationTemplate);
        var evaluationInput = new EmployeeEvaluationInput
        {
            Id = employeeEvaluation.Id,
            OwnerEmail = employeeEvaluation.Owner.Email!,
            EmployeeEmail = employeeEvaluation.Employee.Email!,
            Template = employeeEvaluation.Template.Description,
            Subject = employeeEvaluation.Subject
        };

        _employeeEvaluationServiceMock.Setup(x => x.CheckIfExists(It.IsAny<EmployeeEvaluationInput>()))
                                      .ReturnsAsync(true);

        _employeeEvaluationServiceMock
            .Setup(x => x.Get(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(employeeEvaluation.ToDto());

        var employeeEvaluationRatings = CreateRating(employeeEvaluation, _employee).ToMockIQueryable();

        _dbMock.Setup(x => x.EmployeeEvaluationRating.Get(It.IsAny<Expression<Func<EmployeeEvaluationRating, bool>>>()))
               .Returns(employeeEvaluationRatings.Where(x => x.EmployeeEvaluationId == employeeEvaluation.Id));

        var result = await _employeeEvaluationRatingService.GetAllByEvaluation(evaluationInput);

        Assert.Equal(employeeEvaluationRatings.First().Comment, result.First().Comment);
    }
}