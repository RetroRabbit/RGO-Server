using MockQueryable.Moq;
using Moq;
using RGO.Models;
using RGO.Services.Services;
using RGO.UnitOfWork;
using RGO.UnitOfWork.Entities;
using System.Linq.Expressions;
using Xunit;

namespace RGO.Tests.Services;

public class EmployeeEvaluationTemplateServiceUnitTests
{
    private readonly Mock<IUnitOfWork> _dbMock;
    private readonly EmployeeEvaluationTemplateService _employeeEvaluationTemplateService;

    public EmployeeEvaluationTemplateServiceUnitTests()
    {
        _dbMock = new Mock<IUnitOfWork>();
        _employeeEvaluationTemplateService = new EmployeeEvaluationTemplateService(_dbMock.Object);
    }

    [Fact]
    public async Task CheckIfExistsTest()
    {
        _dbMock.SetupSequence(x => x.EmployeeEvaluationTemplate.Any(It.IsAny<Expression<Func<EmployeeEvaluationTemplate, bool>>>()))
            .ReturnsAsync(true)
            .ReturnsAsync(false);

        bool exists = await _employeeEvaluationTemplateService.CheckIfExists("template");
        Assert.True(exists);

        exists = await _employeeEvaluationTemplateService.CheckIfExists("template");
        Assert.False(exists);
    }

    [Fact]
    public async Task GetEmployeeEvaluationTemplateFailTest()
    {
        _dbMock.Setup(x => x.EmployeeEvaluationTemplate.Any(It.IsAny<Expression<Func<EmployeeEvaluationTemplate, bool>>>()))
            .ReturnsAsync(false);

        await Assert.ThrowsAsync<Exception>(() => _employeeEvaluationTemplateService.GetEmployeeEvaluationTemplate("template"));
    }

    [Fact]
    public async Task GetEmployeeEvaluationTemplatePassTest()
    {
        var evaluationTemplates = new List<EmployeeEvaluationTemplate>
        {
            new EmployeeEvaluationTemplate
            {
                Id = 1,
                Description = "template"
            }
        }.AsQueryable().BuildMock();

        _dbMock.Setup(x => x.EmployeeEvaluationTemplate.Any(It.IsAny<Expression<Func<EmployeeEvaluationTemplate, bool>>>()))
            .ReturnsAsync(true);

        _dbMock.Setup(x => x.EmployeeEvaluationTemplate.Get(It.IsAny<Expression<Func<EmployeeEvaluationTemplate, bool>>>()))
            .Returns(evaluationTemplates.Where(x => x.Description == evaluationTemplates.First().Description));

        var result = await _employeeEvaluationTemplateService.GetEmployeeEvaluationTemplate("template");
        
        Assert.Equal(evaluationTemplates.First().Description, result.Description);
    }

    [Fact]
    public async Task SaveEmployeeEvaluationTemplateFailTest()
    {
        _dbMock.Setup(x => x.EmployeeEvaluationTemplate.Any(It.IsAny<Expression<Func<EmployeeEvaluationTemplate, bool>>>()))
            .ReturnsAsync(true);

        await Assert.ThrowsAsync<Exception>(() => _employeeEvaluationTemplateService.SaveEmployeeEvaluationTemplate("template"));
    }

    [Fact]
    public async Task SaveEmployeeEvaluationTemplatePassTest()
    {
        var evaluationTemplate = new EmployeeEvaluationTemplate
        {
            Id = 1,
            Description = "template"
        };

        _dbMock.Setup(x => x.EmployeeEvaluationTemplate.Any(It.IsAny<Expression<Func<EmployeeEvaluationTemplate, bool>>>()))
            .ReturnsAsync(false);

        _dbMock.Setup(x => x.EmployeeEvaluationTemplate.Add(It.IsAny<EmployeeEvaluationTemplate>()))
            .ReturnsAsync(evaluationTemplate.ToDto());

        var result = await _employeeEvaluationTemplateService.SaveEmployeeEvaluationTemplate("template");

        Assert.Equal(evaluationTemplate.Description, result.Description);
    }

    [Fact]
    public async Task UpdateEmployeeEvaluationTemplateFailTest()
    {
        var evaluationTemplate = new EmployeeEvaluationTemplate
        {
            Id = 1,
            Description = "template"
        };

        _dbMock.Setup(x => x.EmployeeEvaluationTemplate.Any(It.IsAny<Expression<Func<EmployeeEvaluationTemplate, bool>>>()))
            .ReturnsAsync(false);

        await Assert.ThrowsAsync<Exception>(() =>
            _employeeEvaluationTemplateService.UpdateEmployeeEvaluationTemplate(evaluationTemplate.ToDto()));
    }

    [Fact]
    public async Task UpdateEmployeeEvaluationTemplatePassTest()
    {
        var evaluationTemplate = new EmployeeEvaluationTemplate
        {
            Id = 1,
            Description = "template"
        };

        _dbMock.Setup(x => x.EmployeeEvaluationTemplate.Any(It.IsAny<Expression<Func<EmployeeEvaluationTemplate, bool>>>()))
            .ReturnsAsync(true);

        _dbMock.Setup(x => x.EmployeeEvaluationTemplate.Update(It.IsAny<EmployeeEvaluationTemplate>()))
            .ReturnsAsync(evaluationTemplate.ToDto());

        var result = await _employeeEvaluationTemplateService.UpdateEmployeeEvaluationTemplate(evaluationTemplate.ToDto());

        Assert.Equal(evaluationTemplate.Description, result.Description);
    }

    [Fact]
    public async Task DeleteEmployeeEvaluationTemplateFailTest()
    {
        _dbMock.Setup(x => x.EmployeeEvaluationTemplate.Any(It.IsAny<Expression<Func<EmployeeEvaluationTemplate, bool>>>()))
            .ReturnsAsync(false);

        await Assert.ThrowsAsync<Exception>(() => _employeeEvaluationTemplateService.DeleteEmployeeEvaluationTemplate("template"));
    }

    [Fact]
    public async Task DeleteEmployeeEvaluationTemplatePassTest()
    {
        var evaluationTemplates = new List<EmployeeEvaluationTemplate>
        {
            new EmployeeEvaluationTemplate
            {
                Id = 1,
                Description = "template"
            }
        }.AsQueryable().BuildMock();

        _dbMock.SetupSequence(x => x.EmployeeEvaluationTemplate.Any(It.IsAny<Expression<Func<EmployeeEvaluationTemplate, bool>>>()))
            .ReturnsAsync(true)
            .ReturnsAsync(true);

        _dbMock.Setup(x => x.EmployeeEvaluationTemplate.Get(It.IsAny<Expression<Func<EmployeeEvaluationTemplate, bool>>>()))
            .Returns(evaluationTemplates.Where(x => x.Description == evaluationTemplates.First().Description));

        _dbMock.Setup(x => x.EmployeeEvaluationTemplate.Delete(It.IsAny<int>()))
            .ReturnsAsync(evaluationTemplates.First().ToDto());

        var result = await _employeeEvaluationTemplateService.DeleteEmployeeEvaluationTemplate("template");

        Assert.Equal(evaluationTemplates.First().Description, result.Description);
    }

    [Fact]
    public async Task GetAllEmployeeEvaluationTemplatesTest()
    {
        var evaluationTemplates = new List<EmployeeEvaluationTemplateDto>
        {
            new EmployeeEvaluationTemplate
            {
                Id = 1,
                Description = "template"
            }.ToDto()
        };

        _dbMock.Setup(x => x.EmployeeEvaluationTemplate.GetAll(It.IsAny<Expression<Func<EmployeeEvaluationTemplate, bool>>>()))
            .ReturnsAsync(evaluationTemplates);

        var result = await _employeeEvaluationTemplateService.GetAllEmployeeEvaluationTemplates();

        Assert.Equal(evaluationTemplates.First().Description, result.First().Description);
    }
}
