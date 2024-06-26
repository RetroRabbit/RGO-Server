﻿using System.Linq.Expressions;
using HRIS.Models;
using HRIS.Models.Enums;
using HRIS.Services.Interfaces;
using HRIS.Services.Services;
using MockQueryable.Moq;
using Moq;
using RR.Tests.Data.Models.HRIS;
using RR.UnitOfWork;
using RR.UnitOfWork.Entities.HRIS;
using Xunit;

namespace RGO.Tests.Services;

public class EmployeeEvaluationAudienceServiceUnitTests
{
    private readonly Mock<IUnitOfWork> _dbMock;
    private readonly Mock<IErrorLoggingService> _errorLoggingServiceMock;
    private readonly EmployeeDto _employee;
    private readonly EmployeeEvaluationAudienceService _employeeEvaluationAudienceService;
    private readonly Mock<IEmployeeEvaluationService> _employeeEvaluationServiceMock;
    private readonly EmployeeEvaluationTemplateDto _employeeEvaluationTemplate;
    private readonly Mock<IEmployeeService> _employeeServiceMock;

    public EmployeeEvaluationAudienceServiceUnitTests()
    {
        _dbMock = new Mock<IUnitOfWork>();
        _employeeServiceMock = new Mock<IEmployeeService>();
        _employeeEvaluationServiceMock = new Mock<IEmployeeEvaluationService>();
        _errorLoggingServiceMock = new Mock<IErrorLoggingService>();
        _employeeEvaluationAudienceService = new EmployeeEvaluationAudienceService(
         _dbMock.Object,
         _employeeServiceMock.Object,
         _employeeEvaluationServiceMock.Object,_errorLoggingServiceMock.Object);

        EmployeeTypeDto employeeTypeDto = new EmployeeTypeDto
        {
            Id = 1,
            Name = "Developer"
        };

        var employeeAddressDto = EmployeeAddressTestData.EmployeeAddressDto;

        _employee = EmployeeTestData.EmployeeDto;
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

    private EmployeeEvaluationAudience CreateEmployeeEvaluationAudience(
        EmployeeEvaluation? evaluation = null,
        EmployeeDto? employee = null)
    {
        var employeeEvaluationAudience = new EmployeeEvaluationAudience
        {
            Id = 1,
            EmployeeEvaluationId = evaluation?.Id ?? 0,
            EmployeeId = employee?.Id ?? 0
        };

        if (evaluation != null)
            employeeEvaluationAudience.Evaluation = evaluation;

        if (employee != null)
            employeeEvaluationAudience.Employee = new Employee(employee, employee.EmployeeType!);

        return employeeEvaluationAudience;
    }

    [Fact]
    public async Task CheckIfExistsTest()
    {
        var evaluation = CreateEmployeeEvaluation(_employee, _employee, _employeeEvaluationTemplate);

        _dbMock
            .SetupSequence(x =>
                               x.EmployeeEvaluationAudience.Any(It.IsAny<Expression<
                                                                    Func<EmployeeEvaluationAudience, bool>>>()))
            .ReturnsAsync(true)
            .ReturnsAsync(false);

        var exists = await _employeeEvaluationAudienceService.CheckIfExists(evaluation.ToDto(), "email");
        Assert.True(exists);

        exists = await _employeeEvaluationAudienceService.CheckIfExists(evaluation.ToDto(), "email");
        Assert.False(exists);
    }

    [Fact]
    public async Task GetFailTest()
    {
        var evaluation = CreateEmployeeEvaluation(_employee, _employee, _employeeEvaluationTemplate);

        _dbMock
            .Setup(x =>
                       x.EmployeeEvaluationAudience.Any(It.IsAny<Expression<Func<EmployeeEvaluationAudience, bool>>>()))
            .ReturnsAsync(false);

        _errorLoggingServiceMock.Setup(r => r.LogException(It.IsAny<Exception>())).Throws(new Exception());

        await Assert.ThrowsAsync<Exception>(() => _employeeEvaluationAudienceService.Get(evaluation.ToDto(), "email"));
    }

    [Fact]
    public async Task GetPassTest()
    {
        var evaluation = CreateEmployeeEvaluation(_employee, _employee, _employeeEvaluationTemplate);
        var evaluationAudience = CreateEmployeeEvaluationAudience(evaluation, _employee);

        var targetEmail = _employee.Email;

        Expression<Func<EmployeeEvaluationAudience, bool>> criteria = x => x.Employee.Email == targetEmail
                                                                           && x.Evaluation.Id == evaluation.Id;

        _dbMock
            .Setup(x =>
                       x.EmployeeEvaluationAudience.Any(It.IsAny<Expression<Func<EmployeeEvaluationAudience, bool>>>()))
            .ReturnsAsync(true);

        _dbMock
            .Setup(x =>
                       x.EmployeeEvaluationAudience.Get(It.IsAny<Expression<Func<EmployeeEvaluationAudience, bool>>>()))
            .Returns(new List<EmployeeEvaluationAudience> { evaluationAudience }.AsQueryable().BuildMock()
                         .Where(criteria));

        var result = await _employeeEvaluationAudienceService.Get(evaluation.ToDto(), targetEmail!);

        Assert.Equal(evaluationAudience.Id, result.Id);
    }

    [Fact]
    public async Task DeleteFailTest()
    {
        var evaluation = CreateEmployeeEvaluation(_employee, _employee, _employeeEvaluationTemplate);
        var evaluationInput = new EmployeeEvaluationInput
        {
            Id = 0,
            OwnerEmail = evaluation.Owner.Email!,
            EmployeeEmail = evaluation.Employee.Email!,
            Template = evaluation.Template.Description,
            Subject = evaluation.Subject
        };

        

        _dbMock
            .Setup(x =>
                       x.EmployeeEvaluationAudience.Any(It.IsAny<Expression<Func<EmployeeEvaluationAudience, bool>>>()))
            .ReturnsAsync(false);

        _errorLoggingServiceMock.Setup(r => r.LogException(It.IsAny<Exception>())).Throws(new Exception());

        await Assert.ThrowsAsync<Exception>(() => _employeeEvaluationAudienceService.Delete("email", evaluationInput));
    }

    [Fact]
    public async Task DeletePassTest()
    {
        var evaluation = CreateEmployeeEvaluation(_employee, _employee, _employeeEvaluationTemplate);
        var evaluationInput = new EmployeeEvaluationInput
        {
            Id = 0,
            OwnerEmail = evaluation.Owner.Email!,
            EmployeeEmail = evaluation.Employee.Email!,
            Template = evaluation.Template.Description,
            Subject = evaluation.Subject
        };
        var evaluationAudience = CreateEmployeeEvaluationAudience(evaluation, _employee);

        var targetEmail = _employee.Email;

        Expression<Func<EmployeeEvaluationAudience, bool>> criteria = x => x.Employee.Email == targetEmail
                                                                           && x.Evaluation.Id == evaluation.Id;

        _dbMock
            .Setup(x =>
                       x.EmployeeEvaluationAudience.Any(It.IsAny<Expression<Func<EmployeeEvaluationAudience, bool>>>()))
            .ReturnsAsync(true);

        _dbMock
            .Setup(x =>
                       x.EmployeeEvaluationAudience.Any(It.IsAny<Expression<Func<EmployeeEvaluationAudience, bool>>>()))
            .ReturnsAsync(true);

        _dbMock
            .Setup(x =>
                       x.EmployeeEvaluationAudience.Get(It.IsAny<Expression<Func<EmployeeEvaluationAudience, bool>>>()))
            .Returns(new List<EmployeeEvaluationAudience> { evaluationAudience }.AsQueryable().BuildMock()
                         .Where(criteria));

        _dbMock.Setup(x => x.EmployeeEvaluationAudience.Delete(It.IsAny<int>()))
               .ReturnsAsync(evaluationAudience.ToDto());

        var result = await _employeeEvaluationAudienceService.Delete(targetEmail!, evaluationInput);

        Assert.Equal(evaluationAudience.Id, result.Id);
    }

    [Fact]
    public async Task SaveFailTest()
    {
        var evaluation = CreateEmployeeEvaluation(_employee, _employee, _employeeEvaluationTemplate);
        var evaluationAudience = CreateEmployeeEvaluationAudience(evaluation, _employee);
        var evaluationInput = new EmployeeEvaluationInput
        {
            Id = 0,
            OwnerEmail = evaluation.Owner.Email!,
            EmployeeEmail = evaluation.Employee.Email!,
            Template = evaluation.Template.Description,
            Subject = evaluation.Subject
        };
                                                          

        _dbMock
            .Setup(x =>
                       x.EmployeeEvaluationAudience.Any(It.IsAny<Expression<Func<EmployeeEvaluationAudience, bool>>>()))
            .ReturnsAsync(true);

        _errorLoggingServiceMock.Setup(r => r.LogException(It.IsAny<Exception>())).Throws(new Exception());

        await Assert.ThrowsAsync<Exception>(() => _employeeEvaluationAudienceService.Save(_employee.Email!,
                                             evaluationInput));
    }

    [Fact]
    public async Task SavePassTest()
    {
        var evaluation = CreateEmployeeEvaluation(_employee, _employee, _employeeEvaluationTemplate);
        var evaluationAudience = CreateEmployeeEvaluationAudience(evaluation, _employee);
        var evaluationInput = new EmployeeEvaluationInput
        {
            Id = 0,
            OwnerEmail = evaluation.Owner.Email!,
            EmployeeEmail = evaluation.Employee.Email!,
            Template = evaluation.Template.Description,
            Subject = evaluation.Subject
        };

        _employeeEvaluationServiceMock.Setup(x => x.Get(
                                                        It.IsAny<string>(),
                                                        It.IsAny<string>(),
                                                        It.IsAny<string>(),
                                                        It.IsAny<string>()))
                                      .ReturnsAsync(evaluation.ToDto());

        _employeeServiceMock.Setup(x => x.GetEmployee(It.IsAny<string>()))
                            .ReturnsAsync(_employee);

        _dbMock
            .Setup(x =>
                       x.EmployeeEvaluationAudience.Any(It.IsAny<Expression<Func<EmployeeEvaluationAudience, bool>>>()))
            .ReturnsAsync(false);

        _dbMock.Setup(x => x.EmployeeEvaluationAudience.Add(It.IsAny<EmployeeEvaluationAudience>()))
               .ReturnsAsync(evaluationAudience.ToDto());

        var result = await _employeeEvaluationAudienceService.Save(_employee.Email!, evaluationInput);

        Assert.Equal(evaluationAudience.Employee.Email, result.Employee!.Email);
    }

    [Fact]
    public async Task UpdateFailTest()
    {
        var evaluation = CreateEmployeeEvaluation(_employee, _employee, _employeeEvaluationTemplate);
        var evaluationAudience = CreateEmployeeEvaluationAudience(evaluation, _employee);

        _dbMock
            .Setup(x =>
                       x.EmployeeEvaluationAudience.Any(It.IsAny<Expression<Func<EmployeeEvaluationAudience, bool>>>()))
            .ReturnsAsync(false);

        _errorLoggingServiceMock.Setup(r => r.LogException(It.IsAny<Exception>())).Throws(new Exception());

        await Assert.ThrowsAsync<Exception>(() => _employeeEvaluationAudienceService
                                                .Update(evaluationAudience.ToDto()));
    }

    [Fact]
    public async Task UpdatePassTest()
    {
        var evaluation = CreateEmployeeEvaluation(_employee, _employee, _employeeEvaluationTemplate);
        var evaluationAudience = CreateEmployeeEvaluationAudience(evaluation, _employee);

        _dbMock
            .Setup(x =>
                       x.EmployeeEvaluationAudience.Any(It.IsAny<Expression<Func<EmployeeEvaluationAudience, bool>>>()))
            .ReturnsAsync(true);

        _dbMock.Setup(x => x.EmployeeEvaluationAudience.Update(It.IsAny<EmployeeEvaluationAudience>()))
               .ReturnsAsync(evaluationAudience.ToDto());

        var result = await _employeeEvaluationAudienceService.Update(evaluationAudience.ToDto());

        Assert.Equal(evaluationAudience.Employee.Email, result.Employee!.Email);
    }

    [Fact]
    public async Task GetAllTest()
    {
        var evaluation = CreateEmployeeEvaluation(_employee, _employee, _employeeEvaluationTemplate);
        var evaluationAudience = CreateEmployeeEvaluationAudience(evaluation, _employee);

        _dbMock
            .Setup(x =>
                       x.EmployeeEvaluationAudience.Get(It.IsAny<Expression<Func<EmployeeEvaluationAudience, bool>>>()))
            .Returns(new List<EmployeeEvaluationAudience> { evaluationAudience }.AsQueryable().BuildMock());

        var result = await _employeeEvaluationAudienceService.GetAll();

        Assert.Single(result);
    }

    [Fact]
    public async Task GetAllByEmployeeFailTest()
    {
        _employeeServiceMock.Setup(x => x.CheckUserExist(It.IsAny<string>()))
                            .ReturnsAsync(false);
        _errorLoggingServiceMock.Setup(r => r.LogException(It.IsAny<Exception>())).Throws(new Exception());

        await Assert.ThrowsAsync<Exception>(() => _employeeEvaluationAudienceService.GetAllbyEmployee("email"));
    }

    [Fact]
    public async Task GetAllByEmployeePassTest()
    {
        var evaluation = CreateEmployeeEvaluation(_employee, _employee, _employeeEvaluationTemplate);
        var evaluationAudience = CreateEmployeeEvaluationAudience(evaluation, _employee);

        var targetEmail = _employee.Email;
        Expression<Func<EmployeeEvaluationAudience, bool>> criteria = x => x.Employee.Email == targetEmail;

        _employeeServiceMock.Setup(x => x.CheckUserExist(It.IsAny<string>()))
                            .ReturnsAsync(true);

        _dbMock
            .Setup(x =>
                       x.EmployeeEvaluationAudience.Get(It.IsAny<Expression<Func<EmployeeEvaluationAudience, bool>>>()))
            .Returns(new List<EmployeeEvaluationAudience> { evaluationAudience }.AsQueryable().BuildMock()
                         .Where(criteria));

        var result = await _employeeEvaluationAudienceService.GetAllbyEmployee(targetEmail!);

        Assert.Single(result);
    }

    [Fact]
    public async Task GetAllByEvaluationFailTest()
    {
        var evaluation = CreateEmployeeEvaluation(_employee, _employee, _employeeEvaluationTemplate);
        var evaluationInput = new EmployeeEvaluationInput
        {
            Id = 0,
            OwnerEmail = evaluation.Owner.Email!,
            EmployeeEmail = evaluation.Employee.Email!,
            Template = evaluation.Template.Description,
            Subject = evaluation.Subject
        };
                                                          
        _employeeEvaluationServiceMock.Setup(x => x.CheckIfExists(It.IsAny<EmployeeEvaluationInput>()))
                                      .ReturnsAsync(false);
        _errorLoggingServiceMock.Setup(r => r.LogException(It.IsAny<Exception>())).Throws(new Exception());

        await Assert.ThrowsAsync<Exception>(() => _employeeEvaluationAudienceService
                                                .GetAllbyEvaluation(evaluationInput));
    }

    [Fact]
    public async Task GetAllByEvaluationPassTest()
    {
        var evaluation = CreateEmployeeEvaluation(_employee, _employee, _employeeEvaluationTemplate);
        var evaluationAudience = CreateEmployeeEvaluationAudience(evaluation, _employee);
        var evaluationInput = new EmployeeEvaluationInput
        {
            Id = 0,
            OwnerEmail = evaluation.Owner.Email!,
            EmployeeEmail = evaluation.Employee.Email!,
            Template = evaluation.Template.Description,
            Subject = evaluation.Subject
        };
                                                          
        _employeeEvaluationServiceMock.Setup(x => x.CheckIfExists(It.IsAny<EmployeeEvaluationInput>()))
                                      .ReturnsAsync(true);

        _dbMock
            .Setup(x =>
                       x.EmployeeEvaluationAudience.Get(It.IsAny<Expression<Func<EmployeeEvaluationAudience, bool>>>()))
            .Returns(new List<EmployeeEvaluationAudience> { evaluationAudience }.AsQueryable().BuildMock());

        var result = await _employeeEvaluationAudienceService.GetAllbyEvaluation(evaluationInput);

        Assert.Single(result);
    }
}