using MockQueryable.Moq;
using Moq;
using RGO.Models;
using RGO.Services.Interfaces;
using RGO.Services.Services;
using RGO.UnitOfWork;
using RGO.UnitOfWork.Entities;
using System.Linq.Expressions;
using Xunit;

namespace RGO.Tests.Services;

public class EmployeeEvaluationTemplateItemServiceUnitTests
{
    private readonly Mock<IUnitOfWork> _dbMock;
    private readonly Mock<IEmployeeEvaluationTemplateService> _employeeEvaluationTemplateServiceMock;
    private readonly EmployeeEvaluationTemplateItemService _employeeEvaluationTemplateItemService;

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
        _dbMock.SetupSequence(x => x.EmployeeEvaluationTemplateItem.Any(It.IsAny<Expression<Func<EmployeeEvaluationTemplateItem, bool>>>()))
            .ReturnsAsync(true)
            .ReturnsAsync(false);

        bool exists = await _employeeEvaluationTemplateItemService.CheckIfExists("template", "section", "question");
        Assert.True(exists);

        exists = await _employeeEvaluationTemplateItemService.CheckIfExists("template", "section", "question");
        Assert.False(exists);
    }

    [Fact]
    public async Task GetEmployeeEvaluationTemplateItemFailTest()
    {
        _dbMock.Setup(x => x.EmployeeEvaluationTemplateItem.Any(It.IsAny<Expression<Func<EmployeeEvaluationTemplateItem, bool>>>()))
            .ReturnsAsync(false);

        await Assert.ThrowsAsync<Exception>(() => _employeeEvaluationTemplateItemService.GetEmployeeEvaluationTemplateItem("template", "section", "question"));
    }

    [Fact]
    public async Task GetEmployeeEvaluationTemplateItemPassTest()
    {
        var evaluationTemplateItems = new List<EmployeeEvaluationTemplateItem>
        {
            new EmployeeEvaluationTemplateItem
            {
                Id = 1,
                Template = new EmployeeEvaluationTemplate
                {
                    Description = "template"
                },
                Section = "section",
                Question = "question"
            }
        }.AsQueryable().BuildMock();

        _dbMock.Setup(x => x.EmployeeEvaluationTemplateItem.Any(It.IsAny<Expression<Func<EmployeeEvaluationTemplateItem, bool>>>()))
            .ReturnsAsync(true);

        _dbMock.Setup(x => x.EmployeeEvaluationTemplateItem.Get(It.IsAny<Expression<Func<EmployeeEvaluationTemplateItem, bool>>>()))
            .Returns(evaluationTemplateItems);

        EmployeeEvaluationTemplateItemDto employeeEvaluationTemplateItem = await _employeeEvaluationTemplateItemService.GetEmployeeEvaluationTemplateItem("template", "section", "question");

        Assert.Equal("template", employeeEvaluationTemplateItem.Template!.Description);
        Assert.Equal("section", employeeEvaluationTemplateItem.Section);
        Assert.Equal("question", employeeEvaluationTemplateItem.Question);
    }

    [Fact]
    public async Task SaveEmployeeEvaluationTemplateItemFailTest()
    {
        _dbMock.Setup(x => x.EmployeeEvaluationTemplateItem.Any(It.IsAny<Expression<Func<EmployeeEvaluationTemplateItem, bool>>>()))
            .ReturnsAsync(true);

        await Assert.ThrowsAsync<Exception>(() => _employeeEvaluationTemplateItemService.SaveEmployeeEvaluationTemplateItem("template", "section", "question"));
    }

    [Fact]
    public async Task SaveEmployeeEvaluationTemplateItemPassTest()
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

        _dbMock.Setup(x => x.EmployeeEvaluationTemplateItem.Any(It.IsAny<Expression<Func<EmployeeEvaluationTemplateItem, bool>>>()))
            .ReturnsAsync(false);

        _employeeEvaluationTemplateServiceMock.Setup(x => x.GetEmployeeEvaluationTemplate(It.IsAny<string>()))
            .ReturnsAsync(evaluationTemplateItem.Template!.ToDto());

        _dbMock.Setup(x => x.EmployeeEvaluationTemplateItem.Add(It.IsAny<EmployeeEvaluationTemplateItem>()))
            .ReturnsAsync(evaluationTemplateItem.ToDto());

        EmployeeEvaluationTemplateItemDto employeeEvaluationTemplateItem = await _employeeEvaluationTemplateItemService.SaveEmployeeEvaluationTemplateItem("template", "section", "question");

        Assert.Equal("template", employeeEvaluationTemplateItem.Template!.Description);
        Assert.Equal("section", employeeEvaluationTemplateItem.Section);
        Assert.Equal("question", employeeEvaluationTemplateItem.Question);
    }

    [Fact]
    public async Task UpdateEmployeeEvaluationTemplateItemFailTest()
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

        _dbMock.Setup(x => x.EmployeeEvaluationTemplateItem.Any(It.IsAny<Expression<Func<EmployeeEvaluationTemplateItem, bool>>>()))
            .ReturnsAsync(false);

        _employeeEvaluationTemplateServiceMock.Setup(x => x.GetEmployeeEvaluationTemplate(It.IsAny<string>()))
            .ReturnsAsync(evaluationTemplateItem.Template!.ToDto());

        await Assert.ThrowsAsync<Exception>(() => _employeeEvaluationTemplateItemService.UpdateEmployeeEvaluationTemplateItem(evaluationTemplateItem.ToDto()));
    }

    [Fact]
    public async Task UpdateEmployeeEvaluationTemplateItemPassTest()
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

        _dbMock.Setup(x => x.EmployeeEvaluationTemplateItem.Any(It.IsAny<Expression<Func<EmployeeEvaluationTemplateItem, bool>>>()))
            .ReturnsAsync(true);

        _dbMock.Setup(x => x.EmployeeEvaluationTemplateItem.Update(It.IsAny<EmployeeEvaluationTemplateItem>()))
            .ReturnsAsync(evaluationTemplateItem.ToDto());

        EmployeeEvaluationTemplateItemDto employeeEvaluationTemplateItem = await _employeeEvaluationTemplateItemService.UpdateEmployeeEvaluationTemplateItem(evaluationTemplateItem.ToDto());

        Assert.Equal("template", employeeEvaluationTemplateItem.Template!.Description);
        Assert.Equal("section", employeeEvaluationTemplateItem.Section);
        Assert.Equal("question", employeeEvaluationTemplateItem.Question);
    }

    [Fact]
    public async Task DeleteEmployeeEvaluationTemplateItemFailTest()
    {
        _dbMock.Setup(x => x.EmployeeEvaluationTemplateItem.Any(It.IsAny<Expression<Func<EmployeeEvaluationTemplateItem, bool>>>()))
            .ReturnsAsync(false);

        await Assert.ThrowsAsync<Exception>(() => _employeeEvaluationTemplateItemService.DeleteEmployeeEvaluationTemplateItem("template", "section", "question"));
    }

    [Fact]
    public async Task DeleteEmployeeEvaluationTemplateItemPassTest()
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

        _dbMock.Setup(x => x.EmployeeEvaluationTemplateItem.Any(It.IsAny<Expression<Func<EmployeeEvaluationTemplateItem, bool>>>()))
            .ReturnsAsync(true);

        _dbMock.Setup(x => x.EmployeeEvaluationTemplateItem.Get(It.IsAny<Expression<Func<EmployeeEvaluationTemplateItem, bool>>>()))
            .Returns(new List<EmployeeEvaluationTemplateItem> { evaluationTemplateItem }.AsQueryable().BuildMock());

        _dbMock.Setup(x => x.EmployeeEvaluationTemplateItem.Delete(It.IsAny<int>()))
            .ReturnsAsync(evaluationTemplateItem.ToDto());

        EmployeeEvaluationTemplateItemDto employeeEvaluationTemplateItem = await _employeeEvaluationTemplateItemService.DeleteEmployeeEvaluationTemplateItem("template", "section", "question");

        Assert.Equal("template", employeeEvaluationTemplateItem.Template!.Description);
        Assert.Equal("section", employeeEvaluationTemplateItem.Section);
        Assert.Equal("question", employeeEvaluationTemplateItem.Question);
    }

    [Fact]
    public async Task GetAllEmployeeEvaluationTemplateItemsTest()
    {
        var evaluationTemplateItems = new List<EmployeeEvaluationTemplateItem>
        {
            new EmployeeEvaluationTemplateItem
            {
                Id = 1,
                Template = new EmployeeEvaluationTemplate
                {
                    Description = "template"
                },
                Section = "section",
                Question = "question"
            }
        }.AsQueryable().BuildMock();

        _dbMock.Setup(x => x.EmployeeEvaluationTemplateItem.Get(It.IsAny<Expression<Func<EmployeeEvaluationTemplateItem, bool>>>()))
            .Returns(evaluationTemplateItems);

        List<EmployeeEvaluationTemplateItemDto> employeeEvaluationTemplateItems = await _employeeEvaluationTemplateItemService.GetAllEmployeeEvaluationTemplateItems();

        Assert.Equal("template", employeeEvaluationTemplateItems.First().Template!.Description);
        Assert.Equal("section", employeeEvaluationTemplateItems.First().Section);
        Assert.Equal("question", employeeEvaluationTemplateItems.First().Question);
    }

    [Fact]
    public async Task GetAllEmployeeEvaluationTemplateItemsBySectionTest()
    {
        var evaluationTemplateItems = new List<EmployeeEvaluationTemplateItem>
        {
            new EmployeeEvaluationTemplateItem
            {
                Id = 1,
                Template = new EmployeeEvaluationTemplate
                {
                    Description = "template"
                },
                Section = "section",
                Question = "question"
            }
        }.AsQueryable().BuildMock();

        _dbMock.Setup(x => x.EmployeeEvaluationTemplateItem.Get(It.IsAny<Expression<Func<EmployeeEvaluationTemplateItem, bool>>>()))
            .Returns(evaluationTemplateItems);

        List<EmployeeEvaluationTemplateItemDto> employeeEvaluationTemplateItems = await _employeeEvaluationTemplateItemService.GetAllEmployeeEvaluationTemplateItemsBySection("section");

        Assert.Equal("template", employeeEvaluationTemplateItems.First().Template!.Description);
        Assert.Equal("section", employeeEvaluationTemplateItems.First().Section);
        Assert.Equal("question", employeeEvaluationTemplateItems.First().Question);
    }

    [Fact]
    public async Task GetAllEmployeeEvaluationTemplateItemsByTemplateFailTest()
    {
        var evaluationTemplateItems = new List<EmployeeEvaluationTemplateItem>
        {
            new EmployeeEvaluationTemplateItem
            {
                Id = 1,
                Template = new EmployeeEvaluationTemplate
                {
                    Description = "template"
                },
                Section = "section",
                Question = "question"
            }
        }.AsQueryable().BuildMock();

        _employeeEvaluationTemplateServiceMock.Setup(x => x.CheckIfExists(It.IsAny<string>()))
            .ReturnsAsync(false);

        await Assert.ThrowsAsync<Exception>(() => _employeeEvaluationTemplateItemService.GetAllEmployeeEvaluationTemplateItemsByTemplate("template"));
    }

    [Fact]
    public async Task GetAllEmployeeEvaluationTemplateItemsByTemplatePassTest()
    {
        var evaluationTemplateItems = new List<EmployeeEvaluationTemplateItem>
        {
            new EmployeeEvaluationTemplateItem
            {
                Id = 1,
                Template = new EmployeeEvaluationTemplate
                {
                    Description = "template"
                },
                Section = "section",
                Question = "question"
            }
        }.AsQueryable().BuildMock();

        _employeeEvaluationTemplateServiceMock.Setup(x => x.CheckIfExists(It.IsAny<string>()))
            .ReturnsAsync(true);

        _dbMock.Setup(x => x.EmployeeEvaluationTemplateItem.Get(It.IsAny<Expression<Func<EmployeeEvaluationTemplateItem, bool>>>()))
            .Returns(evaluationTemplateItems);

        List<EmployeeEvaluationTemplateItemDto> employeeEvaluationTemplateItems = await _employeeEvaluationTemplateItemService.GetAllEmployeeEvaluationTemplateItemsByTemplate("template");

        Assert.Equal("template", employeeEvaluationTemplateItems.First().Template!.Description);
        Assert.Equal("section", employeeEvaluationTemplateItems.First().Section);
        Assert.Equal("question", employeeEvaluationTemplateItems.First().Question);
    }
}
