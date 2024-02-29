using System.Linq.Expressions;
using HRIS.Models;
using HRIS.Models.Enums;
using HRIS.Services.Interfaces;
using HRIS.Services.Services;
using MockQueryable.Moq;
using Moq;
using RR.UnitOfWork;
using RR.UnitOfWork.Entities.HRIS;
using Xunit;

namespace RGO.Tests.Services;

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

        EmployeeTypeDto employeeTypeDto = new EmployeeTypeDto { Id = 1, Name = "Developer" };
        var employeeAddressDto =
            new EmployeeAddressDto(1, "2", "Complex", "2", "Suburb/District", "City", "Country", "Province", "1620");
        _employee = new EmployeeDto(1, "001", "34434434", new DateTime(), new DateTime(),
                                    null, false, "None", 4, employeeTypeDto, "Notes", 1, 28, 128, 100000, "Dotty", "D",
                                    "Missile", new DateTime(), "South Africa", "South African", "1234457899", " ",
                                    new DateTime(), null, Race.Black, Gender.Female, null!,
                                    "dm@retrorabbit.co.za", "test@gmail.com", "0123456789", null, null,
                                    employeeAddressDto, employeeAddressDto, null, null, null);
        _employeeEvaluationTemplate = new EmployeeEvaluationTemplateDto(1, "template");
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
        var evaluationInput = new EmployeeEvaluationInput(0, "owner", "employee", "template", "subject");

        _dbMock.Setup(x => x.EmployeeEvaluation.Any(It.IsAny<Expression<Func<EmployeeEvaluation, bool>>>()))
               .ReturnsAsync(true);

        var result = await _employeeEvaluationService.CheckIfExists(evaluationInput);

        Assert.True(result);
    }

    [Fact]
    public async Task GetFailTest()
    {
        var evaluation = CreateEmployeeEvaluation(_employee, _employee, _employeeEvaluationTemplate);
        var evaluationInput = new EmployeeEvaluationInput(
                                                          evaluation.Id,
                                                          evaluation.Owner.Email!,
                                                          evaluation.Employee.Email!,
                                                          evaluation.Template.Description,
                                                          evaluation.Subject);

        _dbMock.Setup(x => x.EmployeeEvaluation.Any(It.IsAny<Expression<Func<EmployeeEvaluation, bool>>>()))
               .ReturnsAsync(false);

        await Assert.ThrowsAsync<Exception>(() => _employeeEvaluationService.Get(
                                             evaluationInput.EmployeeEmail,
                                             evaluationInput.OwnerEmail,
                                             evaluationInput.Template,
                                             evaluationInput.Subject));
    }

    [Fact]
    public async Task GetPassTest()
    {
        var evaluation = CreateEmployeeEvaluation(_employee, _employee, _employeeEvaluationTemplate);
        var evaluationInput = new EmployeeEvaluationInput(
                                                          evaluation.Id,
                                                          evaluation.Owner.Email!,
                                                          evaluation.Employee.Email!,
                                                          evaluation.Template.Description,
                                                          evaluation.Subject);

        _dbMock.Setup(x => x.EmployeeEvaluation.Any(It.IsAny<Expression<Func<EmployeeEvaluation, bool>>>()))
               .ReturnsAsync(true);

        _dbMock.Setup(x => x.EmployeeEvaluation.Get(It.IsAny<Expression<Func<EmployeeEvaluation, bool>>>()))
               .Returns(new List<EmployeeEvaluation> { evaluation }.AsQueryable().BuildMock());

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
        var evaluationInput = new EmployeeEvaluationInput(0, "owner", "employee", "template", "subject");
        _ = CreateEmployeeEvaluation(_employee, _employee, _employeeEvaluationTemplate);

        _dbMock.Setup(x => x.EmployeeEvaluation.Any(It.IsAny<Expression<Func<EmployeeEvaluation, bool>>>()))
               .ReturnsAsync(false);

        await Assert.ThrowsAsync<Exception>(() => _employeeEvaluationService.Delete(evaluationInput));
    }

    [Fact]
    public async Task DeletePassTest()
    {
        var evaluationInput = new EmployeeEvaluationInput(0, "owner", "employee", "template", "subject");
        var employeeEvaluation = CreateEmployeeEvaluation(_employee, _employee, _employeeEvaluationTemplate);

        _dbMock.Setup(x => x.EmployeeEvaluation.Any(It.IsAny<Expression<Func<EmployeeEvaluation, bool>>>()))
               .ReturnsAsync(true);

        _dbMock.Setup(x => x.EmployeeEvaluation.Get(It.IsAny<Expression<Func<EmployeeEvaluation, bool>>>()))
               .Returns(new List<EmployeeEvaluation> { employeeEvaluation }.AsQueryable().BuildMock());

        _dbMock.Setup(x => x.EmployeeEvaluation.Delete(It.IsAny<int>()))
               .ReturnsAsync(employeeEvaluation.ToDto());

        var result = await _employeeEvaluationService.Delete(evaluationInput);

        Assert.Equivalent(employeeEvaluation.ToDto(), result);
    }

    [Fact]
    public async Task SaveFailTest()
    {
        var evaluationInput = new EmployeeEvaluationInput(0, "owner", "employee", "template", "subject");
        _ = CreateEmployeeEvaluation(_employee, _employee, _employeeEvaluationTemplate);

        _dbMock.SetupSequence(x => x.EmployeeEvaluation.Any(It.IsAny<Expression<Func<EmployeeEvaluation, bool>>>()))
               .ReturnsAsync(true);

        await Assert.ThrowsAsync<Exception>(() => _employeeEvaluationService.Save(evaluationInput));
    }

    [Fact]
    public async Task SavePassTest()
    {
        var evaluationInput = new EmployeeEvaluationInput(0, "owner", "employee", "template", "subject");
        var employeeEvaluation = CreateEmployeeEvaluation(_employee, _employee, _employeeEvaluationTemplate);

        _dbMock.Setup(x => x.EmployeeEvaluation.Any(It.IsAny<Expression<Func<EmployeeEvaluation, bool>>>()))
               .ReturnsAsync(false);

        _employeeServiceMock.SetupSequence(x => x.GetEmployee(It.IsAny<string>()))
                            .ReturnsAsync(_employee)
                            .ReturnsAsync(_employee);

        _employeeEvaluationTemplateServiceMock.Setup(x => x.Get(It.IsAny<string>()))
                                              .ReturnsAsync(_employeeEvaluationTemplate);

        _dbMock.Setup(x => x.EmployeeEvaluation.Add(It.IsAny<EmployeeEvaluation>()))
               .ReturnsAsync(employeeEvaluation.ToDto());

        var result = await _employeeEvaluationService.Save(evaluationInput);

        Assert.Equivalent(employeeEvaluation.ToDto(), result);
    }

    [Fact]
    public async Task UpdateFailTest()
    {
        var employeeEvaluation = CreateEmployeeEvaluation(_employee, _employee, _employeeEvaluationTemplate);

        var oldEvaluationInput = new EmployeeEvaluationInput(
                                                             0,
                                                             employeeEvaluation.Owner.Email!,
                                                             employeeEvaluation.Employee.Email!,
                                                             employeeEvaluation.Template.Description,
                                                             employeeEvaluation.Subject);

        var newEvaluationInput = new EmployeeEvaluationInput(
                                                             0,
                                                             employeeEvaluation.Owner.Email!,
                                                             employeeEvaluation.Employee.Email!,
                                                             employeeEvaluation.Template.Description,
                                                             "new subject");

        _dbMock.Setup(x => x.EmployeeEvaluation.Any(It.IsAny<Expression<Func<EmployeeEvaluation, bool>>>()))
               .ReturnsAsync(false);

        await Assert.ThrowsAsync<Exception>(() =>
                                                _employeeEvaluationService.Update(oldEvaluationInput,
                                                 newEvaluationInput));
    }

    [Fact]
    public async Task UpdatePassTest()
    {
        var employeeEvaluation = CreateEmployeeEvaluation(_employee, _employee, _employeeEvaluationTemplate);
        var newEmployeeEvaluation =
            CreateEmployeeEvaluation(_employee, _employee, _employeeEvaluationTemplate, "new subject");

        var oldEvaluationInput = new EmployeeEvaluationInput(
                                                             0,
                                                             employeeEvaluation.Owner.Email!,
                                                             employeeEvaluation.Employee.Email!,
                                                             employeeEvaluation.Template.Description,
                                                             employeeEvaluation.Subject);
        var newEvaluationInput = new EmployeeEvaluationInput(
                                                             0,
                                                             newEmployeeEvaluation.Owner.Email!,
                                                             newEmployeeEvaluation.Employee.Email!,
                                                             newEmployeeEvaluation.Template.Description,
                                                             newEmployeeEvaluation.Subject);

        _dbMock.SetupSequence(x => x.EmployeeEvaluation.Any(It.IsAny<Expression<Func<EmployeeEvaluation, bool>>>()))
               .ReturnsAsync(true)
               .ReturnsAsync(true);

        _dbMock.Setup(x => x.EmployeeEvaluation.Get(It.IsAny<Expression<Func<EmployeeEvaluation, bool>>>()))
               .Returns(new List<EmployeeEvaluation> { employeeEvaluation }.AsQueryable().BuildMock());

        _employeeServiceMock.SetupSequence(x => x.GetEmployee(It.IsAny<string>()))
                            .ReturnsAsync(_employee)
                            .ReturnsAsync(_employee);

        _employeeEvaluationTemplateServiceMock.Setup(x => x.Get(It.IsAny<string>()))
                                              .ReturnsAsync(_employeeEvaluationTemplate);

        _dbMock.Setup(x => x.EmployeeEvaluation.Update(It.IsAny<EmployeeEvaluation>()))
               .ReturnsAsync(newEmployeeEvaluation.ToDto());

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

        await Assert.ThrowsAsync<Exception>(() => _employeeEvaluationService.GetAllByEmployee(_employee.Email!));
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

        await Assert.ThrowsAsync<Exception>(() => _employeeEvaluationService.GetAllByOwner(_employee.Email!));
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

        await Assert.ThrowsAsync<Exception>(() =>
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
