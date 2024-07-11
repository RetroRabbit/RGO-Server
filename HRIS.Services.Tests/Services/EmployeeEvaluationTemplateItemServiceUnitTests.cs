using System.Linq.Expressions;
using HRIS.Services.Interfaces;
using HRIS.Services.Services;
using Moq;
using RR.Tests.Data;
using RR.UnitOfWork;
using RR.UnitOfWork.Entities.HRIS;
using Xunit;

namespace HRIS.Services.Tests.Services;

public class EmployeeEvaluationTemplateItemServiceUnitTests
{
    private readonly Mock<IUnitOfWork> _dbMock;
    private readonly EmployeeEvaluationTemplateItemService _employeeEvaluationTemplateItemService;
    private readonly Mock<IEmployeeEvaluationTemplateService> _employeeEvaluationTemplateServiceMock;

    public EmployeeEvaluationTemplateItemServiceUnitTests()
    {
        _dbMock = new Mock<IUnitOfWork>();
        _employeeEvaluationTemplateServiceMock = new Mock<IEmployeeEvaluationTemplateService>();
        _employeeEvaluationTemplateItemService = new EmployeeEvaluationTemplateItemService(
         _dbMock.Object,
         _employeeEvaluationTemplateServiceMock.Object);
    }

    [Fact]
    public async Task CheckIfExistsTest()
    {
        _dbMock
            .SetupSequence(x =>
                               x.EmployeeEvaluationTemplateItem.Any(It.IsAny<Expression<
                                                                        Func<EmployeeEvaluationTemplateItem, bool>>>()))
            .ReturnsAsync(true)
            .ReturnsAsync(false);

        var exists = await _employeeEvaluationTemplateItemService.CheckIfExists("template", "section", "question");
        Assert.True(exists);

        exists = await _employeeEvaluationTemplateItemService.CheckIfExists("template", "section", "question");
        Assert.False(exists);
    }

    [Fact]
    public async Task GetFailTest()
    {
        _dbMock
            .Setup(x =>
                       x.EmployeeEvaluationTemplateItem.Any(It.IsAny<Expression<
                                                                Func<EmployeeEvaluationTemplateItem, bool>>>()))
            .ReturnsAsync(false);

        await Assert.ThrowsAsync<CustomException>(() =>
                                                _employeeEvaluationTemplateItemService.Get("template", "section",
                                                 "question"));
    }

    [Fact]
    public async Task GetPassTest()
    {
        var evaluationTemplateItems = new EmployeeEvaluationTemplateItem
        {
            Id = 1,
            Template = new EmployeeEvaluationTemplate
            {
                Description = "template"
            },
            Section = "section",
            Question = "question"
        }.ToMockIQueryable();

        _dbMock
            .Setup(x =>
                       x.EmployeeEvaluationTemplateItem.Any(It.IsAny<Expression<
                                                                Func<EmployeeEvaluationTemplateItem, bool>>>()))
            .ReturnsAsync(true);

        _dbMock
            .Setup(x =>
                       x.EmployeeEvaluationTemplateItem.Get(It.IsAny<Expression<
                                                                Func<EmployeeEvaluationTemplateItem, bool>>>()))
            .Returns(evaluationTemplateItems);

        var employeeEvaluationTemplateItem =
            await _employeeEvaluationTemplateItemService.Get("template", "section", "question");

        Assert.Equal("template", employeeEvaluationTemplateItem.Template!.Description);
        Assert.Equal("section", employeeEvaluationTemplateItem.Section);
        Assert.Equal("question", employeeEvaluationTemplateItem.Question);
    }

    [Fact]
    public async Task SaveFailTest()
    {
        _dbMock
            .Setup(x =>
                       x.EmployeeEvaluationTemplateItem.Any(It.IsAny<Expression<
                                                                Func<EmployeeEvaluationTemplateItem, bool>>>()))
            .ReturnsAsync(true);

        await Assert.ThrowsAsync<CustomException>(() =>
                                                _employeeEvaluationTemplateItemService.Save("template", "section",
                                                 "question"));
    }

    [Fact]
    public async Task SavePassTest()
    {
        var evaluationTemplateItem = new EmployeeEvaluationTemplateItem
        {
            Id = 1,
            Template = new EmployeeEvaluationTemplate
            {
                Description = "template"
            },
            Section = "section",
            Question = "question"
        };

        _dbMock
            .Setup(x =>
                       x.EmployeeEvaluationTemplateItem.Any(It.IsAny<Expression<
                                                                Func<EmployeeEvaluationTemplateItem, bool>>>()))
            .ReturnsAsync(false);

        _employeeEvaluationTemplateServiceMock.Setup(x => x.Get(It.IsAny<string>()))
                                              .ReturnsAsync(evaluationTemplateItem.Template!.ToDto());

        _dbMock.Setup(x => x.EmployeeEvaluationTemplateItem.Add(It.IsAny<EmployeeEvaluationTemplateItem>()))
               .ReturnsAsync(evaluationTemplateItem);

        var employeeEvaluationTemplateItem =
            await _employeeEvaluationTemplateItemService.Save("template", "section", "question");

        Assert.Equal("template", employeeEvaluationTemplateItem.Template!.Description);
        Assert.Equal("section", employeeEvaluationTemplateItem.Section);
        Assert.Equal("question", employeeEvaluationTemplateItem.Question);
    }

    [Fact]
    public async Task UpdateFailTest()
    {
        var evaluationTemplateItem = new EmployeeEvaluationTemplateItem
        {
            Id = 1,
            Template = new EmployeeEvaluationTemplate
            {
                Description = "template"
            },
            Section = "section",
            Question = "question"
        };

        _dbMock
            .Setup(x =>
                       x.EmployeeEvaluationTemplateItem.Any(It.IsAny<Expression<
                                                                Func<EmployeeEvaluationTemplateItem, bool>>>()))
            .ReturnsAsync(false);

        _employeeEvaluationTemplateServiceMock.Setup(x => x.Get(It.IsAny<string>()))
                                              .ReturnsAsync(evaluationTemplateItem.Template!.ToDto());

        await Assert.ThrowsAsync<CustomException>(() =>
                                                _employeeEvaluationTemplateItemService.Update(evaluationTemplateItem
                                                    .ToDto()));
    }

    [Fact]
    public async Task UpdatePassTest()
    {
        var evaluationTemplateItem = new EmployeeEvaluationTemplateItem
        {
            Id = 1,
            Template = new EmployeeEvaluationTemplate
            {
                Description = "template"
            },
            Section = "section",
            Question = "question"
        };

        _dbMock
            .Setup(x =>
                       x.EmployeeEvaluationTemplateItem.Any(It.IsAny<Expression<
                                                                Func<EmployeeEvaluationTemplateItem, bool>>>()))
            .ReturnsAsync(true);

        _dbMock.Setup(x => x.EmployeeEvaluationTemplateItem.Update(It.IsAny<EmployeeEvaluationTemplateItem>()))
               .ReturnsAsync(evaluationTemplateItem);

        var employeeEvaluationTemplateItem =
            await _employeeEvaluationTemplateItemService.Update(evaluationTemplateItem.ToDto());

        Assert.Equal("template", employeeEvaluationTemplateItem.Template!.Description);
        Assert.Equal("section", employeeEvaluationTemplateItem.Section);
        Assert.Equal("question", employeeEvaluationTemplateItem.Question);
    }

    [Fact]
    public async Task DeleteFailTest()
    {
        _dbMock
            .Setup(x =>
                       x.EmployeeEvaluationTemplateItem.Any(It.IsAny<Expression<
                                                                Func<EmployeeEvaluationTemplateItem, bool>>>()))
            .ReturnsAsync(false);

        await Assert.ThrowsAsync<CustomException>(() =>
                                                _employeeEvaluationTemplateItemService.Delete("template", "section",
                                                 "question"));
    }

    [Fact]
    public async Task DeletePassTest()
    {
        var evaluationTemplateItem = new EmployeeEvaluationTemplateItem
        {
            Id = 1,
            Template = new EmployeeEvaluationTemplate
            {
                Description = "template"
            },
            Section = "section",
            Question = "question"
        };

        _dbMock
            .Setup(x =>
                       x.EmployeeEvaluationTemplateItem.Any(It.IsAny<Expression<
                                                                Func<EmployeeEvaluationTemplateItem, bool>>>()))
            .ReturnsAsync(true);

        _dbMock
            .Setup(x =>
                       x.EmployeeEvaluationTemplateItem.Get(It.IsAny<Expression<
                                                                Func<EmployeeEvaluationTemplateItem, bool>>>()))
            .Returns(evaluationTemplateItem.ToMockIQueryable());

        _dbMock.Setup(x => x.EmployeeEvaluationTemplateItem.Delete(It.IsAny<int>()))
               .ReturnsAsync(evaluationTemplateItem);

        var employeeEvaluationTemplateItem =
            await _employeeEvaluationTemplateItemService.Delete("template", "section", "question");

        Assert.Equal("template", employeeEvaluationTemplateItem.Template!.Description);
        Assert.Equal("section", employeeEvaluationTemplateItem.Section);
        Assert.Equal("question", employeeEvaluationTemplateItem.Question);
    }

    [Fact]
    public async Task GetAllTest()
    {
        var evaluationTemplateItems = new EmployeeEvaluationTemplateItem
            {
                Id = 1,
                Template = new EmployeeEvaluationTemplate
                {
                    Description = "template"
                },
                Section = "section",
                Question = "question"
            }.ToMockIQueryable();

        _dbMock
            .Setup(x =>
                       x.EmployeeEvaluationTemplateItem.Get(It.IsAny<Expression<
                                                                Func<EmployeeEvaluationTemplateItem, bool>>>()))
            .Returns(evaluationTemplateItems);

        var employeeEvaluationTemplateItems = await _employeeEvaluationTemplateItemService.GetAll();

        Assert.Equal("template", employeeEvaluationTemplateItems.First().Template!.Description);
        Assert.Equal("section", employeeEvaluationTemplateItems.First().Section);
        Assert.Equal("question", employeeEvaluationTemplateItems.First().Question);
    }

    [Fact]
    public async Task GetAllBySectionTest()
    {
        var evaluationTemplateItems = new EmployeeEvaluationTemplateItem
            {
                Id = 1,
                Template = new EmployeeEvaluationTemplate
                {
                    Description = "template"
                },
                Section = "section",
                Question = "question"
            }.ToMockIQueryable();

        _dbMock
            .Setup(x =>
                       x.EmployeeEvaluationTemplateItem.Get(It.IsAny<Expression<
                                                                Func<EmployeeEvaluationTemplateItem, bool>>>()))
            .Returns(evaluationTemplateItems);

        var employeeEvaluationTemplateItems = await _employeeEvaluationTemplateItemService.GetAllBySection("section");

        Assert.Equal("template", employeeEvaluationTemplateItems.First().Template!.Description);
        Assert.Equal("section", employeeEvaluationTemplateItems.First().Section);
        Assert.Equal("question", employeeEvaluationTemplateItems.First().Question);
    }

    [Fact]
    public async Task GetAllByTemplateFailTest()
    {
        _employeeEvaluationTemplateServiceMock.Setup(x => x.CheckIfExists(It.IsAny<string>()))
                                              .ReturnsAsync(false);

        await Assert.ThrowsAsync<CustomException>(() => _employeeEvaluationTemplateItemService.GetAllByTemplate("template"));
    }

    [Fact]
    public async Task GetAllByTemplatePassTest()
    {
        var evaluationTemplateItems = new EmployeeEvaluationTemplateItem
        {
            Id = 1,
            Template = new EmployeeEvaluationTemplate
            {
                Description = "template"
            },
            Section = "section",
            Question = "question"
        }.ToMockIQueryable();

        _employeeEvaluationTemplateServiceMock.Setup(x => x.CheckIfExists(It.IsAny<string>()))
                                              .ReturnsAsync(true);

        _dbMock
            .Setup(x =>
                       x.EmployeeEvaluationTemplateItem.Get(It.IsAny<Expression<
                                                                Func<EmployeeEvaluationTemplateItem, bool>>>()))
            .Returns(evaluationTemplateItems);

        var employeeEvaluationTemplateItems = await _employeeEvaluationTemplateItemService.GetAllByTemplate("template");

        Assert.Equal("template", employeeEvaluationTemplateItems.First().Template!.Description);
        Assert.Equal("section", employeeEvaluationTemplateItems.First().Section);
        Assert.Equal("question", employeeEvaluationTemplateItems.First().Question);
    }
}