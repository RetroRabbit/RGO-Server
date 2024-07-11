using System.Linq.Expressions;
using HRIS.Models;
using HRIS.Models.Enums;
using HRIS.Services.Interfaces;
using HRIS.Services.Services;
using MockQueryable.Moq;
using Moq;
using RR.Tests.Data;
using RR.UnitOfWork;
using RR.UnitOfWork.Entities.HRIS;
using Xunit;

namespace HRIS.Services.Tests.Services;

public class EmployeeEvaluationServiceUnitTests
{
    private readonly Mock<IUnitOfWork> _dbMock;
    private readonly EmployeeDto _employee;
    private readonly EmployeeEvaluationService _employeeEvaluationService;
    private readonly EmployeeEvaluationTemplateDto _employeeEvaluationTemplate;
    private readonly Mock<IEmployeeEvaluationTemplateService> _employeeEvaluationTemplateServiceMock;
    private readonly Mock<IEmployeeService> _employeeServiceMock;

    public EmployeeEvaluationServiceUnitTests()
    {
        _dbMock = new Mock<IUnitOfWork>();
        _employeeServiceMock = new Mock<IEmployeeService>();
        _employeeEvaluationTemplateServiceMock = new Mock<IEmployeeEvaluationTemplateService>();
        _employeeEvaluationService = new EmployeeEvaluationService(
                                                                   _dbMock.Object,
                                                                   _employeeServiceMock.Object,
                                                                   _employeeEvaluationTemplateServiceMock.Object);

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
        var evaluationInput = new EmployeeEvaluationInput { Id = 0, OwnerEmail = "owner", EmployeeEmail = "employee", Template = "template", Subject = "subject" };

        _dbMock.Setup(x => x.EmployeeEvaluation.Any(It.IsAny<Expression<Func<EmployeeEvaluation, bool>>>()))
               .ReturnsAsync(true);

        var result = await _employeeEvaluationService.CheckIfExists(evaluationInput);

        Assert.True(result);
    }

    [Fact]
    public async Task GetFailTest()
    {
        var evaluation = CreateEmployeeEvaluation(_employee, _employee, _employeeEvaluationTemplate);
        var evaluationInput = new EmployeeEvaluationInput
        {
            Id = evaluation.Id,
            OwnerEmail = evaluation.Owner.Email!,
            EmployeeEmail = evaluation.Employee.Email!,
            Template = evaluation.Template.Description,
            Subject = evaluation.Subject
        };

        _dbMock.Setup(x => x.EmployeeEvaluation.Any(It.IsAny<Expression<Func<EmployeeEvaluation, bool>>>()))
               .ReturnsAsync(false);

        await Assert.ThrowsAsync<CustomException>(() => _employeeEvaluationService.Get(
                                             evaluationInput.EmployeeEmail,
                                             evaluationInput.OwnerEmail,
                                             evaluationInput.Template,
                                             evaluationInput.Subject));
    }

    [Fact]
    public async Task GetPassTest()
    {
        var evaluation = CreateEmployeeEvaluation(_employee, _employee, _employeeEvaluationTemplate);
        var evaluationInput = new EmployeeEvaluationInput
        {
            Id = evaluation.Id,
            OwnerEmail = evaluation.Owner.Email!,
            EmployeeEmail = evaluation.Employee.Email!,
            Template = evaluation.Template.Description,
            Subject = evaluation.Subject
        };

        _dbMock.Setup(x => x.EmployeeEvaluation.Any(It.IsAny<Expression<Func<EmployeeEvaluation, bool>>>()))
               .ReturnsAsync(true);

        _dbMock.Setup(x => x.EmployeeEvaluation.Get(It.IsAny<Expression<Func<EmployeeEvaluation, bool>>>()))
               .Returns(evaluation.ToMockIQueryable());

        var result = await _employeeEvaluationService.Get(
                                                          evaluationInput.EmployeeEmail,
                                                          evaluationInput.OwnerEmail,
                                                          evaluationInput.Template,
                                                          evaluationInput.Subject);

        Assert.Equivalent(evaluation.ToDto(), result);
    }

    [Fact]
    public async Task DeleteFailTest()
    {
        var evaluationInput = new EmployeeEvaluationInput { Id = 0, OwnerEmail = "owner", EmployeeEmail = "employee", Template = "template", Subject = "subject" };
        _ = CreateEmployeeEvaluation(_employee, _employee, _employeeEvaluationTemplate);

        _dbMock.Setup(x => x.EmployeeEvaluation.Any(It.IsAny<Expression<Func<EmployeeEvaluation, bool>>>()))
               .ReturnsAsync(false);

        await Assert.ThrowsAsync<CustomException>(() => _employeeEvaluationService.Delete(evaluationInput));
    }

    [Fact]
    public async Task DeletePassTest()
    {
        var evaluationInput = new EmployeeEvaluationInput { Id = 0, OwnerEmail = "owner", EmployeeEmail = "employee", Template = "template", Subject = "subject" };
        var employeeEvaluation = CreateEmployeeEvaluation(_employee, _employee, _employeeEvaluationTemplate);

        _dbMock.Setup(x => x.EmployeeEvaluation.Any(It.IsAny<Expression<Func<EmployeeEvaluation, bool>>>()))
               .ReturnsAsync(true);

        _dbMock.Setup(x => x.EmployeeEvaluation.Get(It.IsAny<Expression<Func<EmployeeEvaluation, bool>>>()))
               .Returns(employeeEvaluation.ToMockIQueryable());

        _dbMock.Setup(x => x.EmployeeEvaluation.Delete(It.IsAny<int>()))
               .ReturnsAsync(employeeEvaluation);

        var result = await _employeeEvaluationService.Delete(evaluationInput);

        Assert.Equivalent(employeeEvaluation.ToDto(), result);
    }

    [Fact]
    public async Task SaveFailTest()
    {
        var evaluationInput = new EmployeeEvaluationInput { Id = 0, OwnerEmail = "owner", EmployeeEmail = "employee", Template = "template", Subject = "subject" };
        _ = CreateEmployeeEvaluation(_employee, _employee, _employeeEvaluationTemplate);

        _dbMock.SetupSequence(x => x.EmployeeEvaluation.Any(It.IsAny<Expression<Func<EmployeeEvaluation, bool>>>()))
               .ReturnsAsync(true);

        await Assert.ThrowsAsync<CustomException>(() => _employeeEvaluationService.Save(evaluationInput));
    }

    [Fact]
    public async Task SavePassTest()
    {
        var evaluationInput = new EmployeeEvaluationInput { Id = 0, OwnerEmail = "owner", EmployeeEmail = "employee", Template = "template", Subject = "subject" };
        var employeeEvaluation = CreateEmployeeEvaluation(_employee, _employee, _employeeEvaluationTemplate);

        _dbMock.Setup(x => x.EmployeeEvaluation.Any(It.IsAny<Expression<Func<EmployeeEvaluation, bool>>>()))
               .ReturnsAsync(false);

        _employeeServiceMock.SetupSequence(x => x.GetEmployee(It.IsAny<string>()))
                            .ReturnsAsync(_employee)
                            .ReturnsAsync(_employee);

        _employeeEvaluationTemplateServiceMock.Setup(x => x.Get(It.IsAny<string>()))
                                              .ReturnsAsync(_employeeEvaluationTemplate);

        _dbMock.Setup(x => x.EmployeeEvaluation.Add(It.IsAny<EmployeeEvaluation>()))
               .ReturnsAsync(employeeEvaluation);

        var result = await _employeeEvaluationService.Save(evaluationInput);

        Assert.Equivalent(employeeEvaluation.ToDto(), result);
    }

    [Fact]
    public async Task UpdateFailTest()
    {
        var employeeEvaluation = CreateEmployeeEvaluation(_employee, _employee, _employeeEvaluationTemplate);

        var oldEvaluationInput = new EmployeeEvaluationInput
        {
            Id = 0,
            OwnerEmail = employeeEvaluation.Owner.Email!,
            EmployeeEmail = employeeEvaluation.Employee.Email!,
            Template = employeeEvaluation.Template.Description,
            Subject = employeeEvaluation.Subject
        };

        var newEvaluationInput = new EmployeeEvaluationInput
        {
            Id = 0,
            OwnerEmail = employeeEvaluation.Owner.Email!,
            EmployeeEmail = employeeEvaluation.Employee.Email!,
            Template = employeeEvaluation.Template.Description,
            Subject = "new subject"
        };

        _dbMock.Setup(x => x.EmployeeEvaluation.Any(It.IsAny<Expression<Func<EmployeeEvaluation, bool>>>()))
               .ReturnsAsync(false);

        await Assert.ThrowsAsync<CustomException>(() =>
                                                _employeeEvaluationService.Update(oldEvaluationInput,
                                                 newEvaluationInput));
    }

    [Fact]
    public async Task UpdatePassTest()
    {
        var employeeEvaluation = CreateEmployeeEvaluation(_employee, _employee, _employeeEvaluationTemplate);
        var newEmployeeEvaluation =
            CreateEmployeeEvaluation(_employee, _employee, _employeeEvaluationTemplate, "new subject");

        var oldEvaluationInput = new EmployeeEvaluationInput
        {
            Id = 0,
            OwnerEmail = employeeEvaluation.Owner.Email!,
            EmployeeEmail = employeeEvaluation.Employee.Email!,
            Template = employeeEvaluation.Template.Description,
            Subject = employeeEvaluation.Subject
        };

        var newEvaluationInput = new EmployeeEvaluationInput
        {
            Id = 0,
            OwnerEmail = employeeEvaluation.Owner.Email!,
            EmployeeEmail = employeeEvaluation.Employee.Email!,
            Template = employeeEvaluation.Template.Description,
            Subject = employeeEvaluation.Subject
        };

        _dbMock.SetupSequence(x => x.EmployeeEvaluation.Any(It.IsAny<Expression<Func<EmployeeEvaluation, bool>>>()))
               .ReturnsAsync(true)
               .ReturnsAsync(true);

        _dbMock.Setup(x => x.EmployeeEvaluation.Get(It.IsAny<Expression<Func<EmployeeEvaluation, bool>>>()))
               .Returns(employeeEvaluation.ToMockIQueryable());

        _employeeServiceMock.SetupSequence(x => x.GetEmployee(It.IsAny<string>()))
                            .ReturnsAsync(_employee)
                            .ReturnsAsync(_employee);

        _employeeEvaluationTemplateServiceMock.Setup(x => x.Get(It.IsAny<string>()))
                                              .ReturnsAsync(_employeeEvaluationTemplate);

        _dbMock.Setup(x => x.EmployeeEvaluation.Update(It.IsAny<EmployeeEvaluation>()))
               .ReturnsAsync(newEmployeeEvaluation);

        var result = await _employeeEvaluationService.Update(oldEvaluationInput, newEvaluationInput);
        var expected = newEmployeeEvaluation.ToDto();
        Assert.Equivalent(expected, result);
    }

    [Fact]
    public async Task GetAllTest()
    {
        var employeeEvaluations = new List<EmployeeEvaluation>
        {
            CreateEmployeeEvaluation(_employee, _employee, _employeeEvaluationTemplate),
            CreateEmployeeEvaluation(_employee, _employee, _employeeEvaluationTemplate),
            CreateEmployeeEvaluation(_employee, _employee, _employeeEvaluationTemplate)
        };
        employeeEvaluations[0].Owner.Email = "subject1@retrorabbit.co.za";
        employeeEvaluations[1].Owner.Email = "demo@retrorabbit.co.za";

        Expression<Func<EmployeeEvaluation, bool>> criteria = x => x.Owner.Email == _employee.Email
                                                                   || x.Employee.Email == _employee.Email;

        _dbMock.Setup(x => x.EmployeeEvaluation.Get(It.IsAny<Expression<Func<EmployeeEvaluation, bool>>>()))
               .Returns(employeeEvaluations.AsQueryable().Where(criteria).BuildMock());

        var result = await _employeeEvaluationService.GetAllEvaluationsByEmail(_employee.Email!);

        Assert.Equal(employeeEvaluations.Count, result.Count);
    }

    [Fact]
    public async Task GetAllByEmployeeFailTest()
    {
        _employeeServiceMock.Setup(x => x.CheckUserExist(It.IsAny<string>()))
                            .ReturnsAsync(false);

        await Assert.ThrowsAsync<CustomException>(() => _employeeEvaluationService.GetAllByEmployee(_employee.Email!));
    }

    [Fact]
    public async Task GetAllByEmployeePassTest()
    {
        var employeeEvaluations = new List<EmployeeEvaluation>
        {
            CreateEmployeeEvaluation(_employee, _employee, _employeeEvaluationTemplate)
        };

        _employeeServiceMock.Setup(x => x.CheckUserExist(It.IsAny<string>()))
                            .ReturnsAsync(true);

        Expression<Func<EmployeeEvaluation, bool>> criteria = x => x.Employee.Email == _employee.Email;

        _dbMock.Setup(x => x.EmployeeEvaluation.Get(It.IsAny<Expression<Func<EmployeeEvaluation, bool>>>()))
               .Returns(employeeEvaluations.AsQueryable().Where(criteria).BuildMock());

        var result = await _employeeEvaluationService.GetAllByEmployee(_employee.Email!);

        Assert.Equal(employeeEvaluations.Count, result.Count);
    }

    [Fact]
    public async Task GetAllByOwnerFailTest()
    {
        _employeeServiceMock.Setup(x => x.CheckUserExist(It.IsAny<string>()))
                            .ReturnsAsync(false);

        await Assert.ThrowsAsync<CustomException>(() => _employeeEvaluationService.GetAllByOwner(_employee.Email!));
    }

    [Fact]
    public async Task GetAllByOwnerPassTest()
    {
        var employeeEvaluations = new List<EmployeeEvaluation>
        {
            CreateEmployeeEvaluation(_employee, _employee, _employeeEvaluationTemplate)
        };

        _employeeServiceMock.Setup(x => x.CheckUserExist(It.IsAny<string>()))
                            .ReturnsAsync(true);

        Expression<Func<EmployeeEvaluation, bool>> criteria = x => x.Employee.Email == _employee.Email;

        _dbMock.Setup(x => x.EmployeeEvaluation.Get(It.IsAny<Expression<Func<EmployeeEvaluation, bool>>>()))
               .Returns(employeeEvaluations.AsQueryable().Where(criteria).BuildMock());

        var result = await _employeeEvaluationService.GetAllByOwner(_employee.Email!);

        Assert.Equal(employeeEvaluations.Count, result.Count);
    }

    [Fact]
    public async Task GetAllFailTest()
    {
        _employeeEvaluationTemplateServiceMock.Setup(x => x.CheckIfExists(It.IsAny<string>()))
                                              .ReturnsAsync(false);

        await Assert.ThrowsAsync<CustomException>(() =>
                                                _employeeEvaluationService.GetAllByTemplate(_employeeEvaluationTemplate
                                                    .Description));
    }

    [Fact]
    public async Task GetAllPassTest()
    {
        var employeeEvaluations = new List<EmployeeEvaluation>
        {
            CreateEmployeeEvaluation(_employee, _employee, _employeeEvaluationTemplate)
        };

        _employeeEvaluationTemplateServiceMock.Setup(x => x.CheckIfExists(It.IsAny<string>()))
                                              .ReturnsAsync(true);

        Expression<Func<EmployeeEvaluation, bool>> criteria = x => x.Employee.Email == _employee.Email;

        _dbMock.Setup(x => x.EmployeeEvaluation.Get(It.IsAny<Expression<Func<EmployeeEvaluation, bool>>>()))
               .Returns(employeeEvaluations.AsQueryable().Where(criteria).BuildMock());

        var result = await _employeeEvaluationService.GetAllByTemplate(_employeeEvaluationTemplate.Description);

        Assert.Equal(employeeEvaluations.Count, result.Count);
    }
}