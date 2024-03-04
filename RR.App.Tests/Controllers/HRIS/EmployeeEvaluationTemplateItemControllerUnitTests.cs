using HRIS.Models;
using HRIS.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RR.App.Controllers.HRIS;
using Xunit;
using static System.Collections.Specialized.BitVector32;

namespace RR.App.Tests.Controllers.HRIS;

public class EmployeeEvaluationTemplateItemControllerUnitTests
{
    [Fact]
    public async Task GetAllEmployeeEvaluationTemplateItemsValidSectionReturnsOkResult()
    {
        var section = "Example Test Section";
        var expectedTemplateItems = new List<EmployeeEvaluationTemplateItemDto>
        {
            new EmployeeEvaluationTemplateItemDto{Id = 1, Template = new EmployeeEvaluationTemplateDto{Id = 101, Description = "Template 1" }, Section = "Example Test Section", Question = "Question 1" },
            new EmployeeEvaluationTemplateItemDto{Id = 2, Template = new EmployeeEvaluationTemplateDto{ Id = 102, Description = "Template 2" }, Section = "Example Test Section", Question = "Question 2" },
        };

        var serviceMock = new Mock<IEmployeeEvaluationTemplateItemService>();
        serviceMock.Setup(x => x.GetAllBySection(section)).ReturnsAsync(expectedTemplateItems);

        var controller = new EmployeeEvaluationTemplateItemController(serviceMock.Object);

        var result = await controller.GetAllEmployeeEvaluationTemplateItems(section, null);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var actualTemplateItems = Assert.IsAssignableFrom<List<EmployeeEvaluationTemplateItemDto>>(okResult.Value);
        Assert.Equal(expectedTemplateItems.Count, actualTemplateItems.Count);
        Assert.All(actualTemplateItems, item => { Assert.Equal(section, item.Section); });
    }

    [Fact]
    public async Task GetAllEmployeeEvaluationTemplateItemsValidTemplateReturnsOkResult()
    {
        var template = "Example Test Template";
        var expectedTemplateItems = new List<EmployeeEvaluationTemplateItemDto>
        {
            new EmployeeEvaluationTemplateItemDto{Id = 1, Template = new EmployeeEvaluationTemplateDto{ Id = 101, Description = template }, Section = "Section 1", Question = "Question 1" },
            new EmployeeEvaluationTemplateItemDto{Id = 2, Template = new EmployeeEvaluationTemplateDto{ Id = 101, Description = template }, Section = "Section 1", Question = "Question 1" }
        };

        var serviceMock = new Mock<IEmployeeEvaluationTemplateItemService>();
        serviceMock.Setup(x => x.GetAllByTemplate(template)).ReturnsAsync(expectedTemplateItems);

        var controller = new EmployeeEvaluationTemplateItemController(serviceMock.Object);

        var result = await controller.GetAllEmployeeEvaluationTemplateItems(null, template);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var actualTemplateItems = Assert.IsAssignableFrom<List<EmployeeEvaluationTemplateItemDto>>(okResult.Value);
        Assert.Equal(expectedTemplateItems.Count, actualTemplateItems.Count);
        Assert.All(actualTemplateItems, item => { Assert.Equal(template, item.Template?.Description); });
    }

    [Fact]
    public async Task GetAllEmployeeEvaluationTemplateItemsNoSectionOrTemplateReturnsOkResult()
    {
        var expectedTemplateItems = new List<EmployeeEvaluationTemplateItemDto>
        {
            new EmployeeEvaluationTemplateItemDto{Id = 1, Template = new EmployeeEvaluationTemplateDto{ Id = 101, Description = "Example Test Template" }, Section = "Section 1", Question = "Question 1" },
            new EmployeeEvaluationTemplateItemDto{Id = 2, Template = new EmployeeEvaluationTemplateDto{ Id = 101, Description = "Example Test Template" }, Section = "Section 1", Question = "Question 1" },
        };

        var serviceMock = new Mock<IEmployeeEvaluationTemplateItemService>();
        serviceMock.Setup(x => x.GetAll()).ReturnsAsync(expectedTemplateItems);

        var controller = new EmployeeEvaluationTemplateItemController(serviceMock.Object);

        var result = await controller.GetAllEmployeeEvaluationTemplateItems(null, null);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var actualTemplateItems = Assert.IsAssignableFrom<List<EmployeeEvaluationTemplateItemDto>>(okResult.Value);
        Assert.Equal(expectedTemplateItems.Count, actualTemplateItems.Count);
    }

    [Fact]
    public async Task GetAllEmployeeEvaluationTemplateItemsExceptionThrownReturnsNotFoundResult()
    {
        var serviceMock = new Mock<IEmployeeEvaluationTemplateItemService>();
        serviceMock.Setup(x => x.GetAll())
                   .ThrowsAsync(new Exception("An error occurred while retrieving employee evaluation template items."));

        var controller = new EmployeeEvaluationTemplateItemController(serviceMock.Object);

        var result = await controller.GetAllEmployeeEvaluationTemplateItems(null, null);

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        var exceptionMessage = Assert.IsType<string>(notFoundResult.Value);
        Assert.Equal("An error occurred while retrieving employee evaluation template items.", exceptionMessage);
    }

    [Fact]
    public async Task SaveEmployeeEvaluationTemplateItemValidParametersReturnsOkResult()
    {
        var template = "Example Test Template";
        var section = "Example Test Section";
        var question = "Example Test Question";
        var savedEmployeeEvaluationTemplateItem =
            new EmployeeEvaluationTemplateItemDto { Id = 1, Template = new EmployeeEvaluationTemplateDto { Id = 101, Description = template }, Section = "Example Test Section",
                Question = "Example Test Question"
            };

        var serviceMock = new Mock<IEmployeeEvaluationTemplateItemService>();
        serviceMock.Setup(x => x.Save(template, section, question)).ReturnsAsync(savedEmployeeEvaluationTemplateItem);

        var controller = new EmployeeEvaluationTemplateItemController(serviceMock.Object);

        var result = await controller.SaveEmployeeEvaluationTemplateItem(template, section, question);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var actualSavedItem = Assert.IsAssignableFrom<EmployeeEvaluationTemplateItemDto>(okResult.Value);
        Assert.Equal(savedEmployeeEvaluationTemplateItem, actualSavedItem);
    }

    [Fact]
    public async Task SaveEmployeeEvaluationTemplateItemExceptionThrownReturnsNotFoundResult()
    {
        var template = "Example Test Template";
        var section = "Example Test Section";
        var question = "Example Test Question";

        var serviceMock = new Mock<IEmployeeEvaluationTemplateItemService>();
        var exceptionMessage = "An error occurred while saving the employee evaluation template item.";
        serviceMock.Setup(x => x.Save(template, section, question)).ThrowsAsync(new Exception(exceptionMessage));

        var controller = new EmployeeEvaluationTemplateItemController(serviceMock.Object);

        var result = await controller.SaveEmployeeEvaluationTemplateItem(template, section, question);

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        var actualExceptionMessage = Assert.IsType<string>(notFoundResult.Value);
        Assert.Equal(exceptionMessage, actualExceptionMessage);
    }

    [Fact]
    public async Task UpdateEmployeeEvaluationTemplateItemValidDtoReturnsOkResult()
    {
        var validDto =
            new EmployeeEvaluationTemplateItemDto
            {
                Id = 1,
                Template = new EmployeeEvaluationTemplateDto { Id = 101, Description = "Example Test Template" },
                Section = "Example Section",
                Question = "Example Question"
            };

        var serviceMock = new Mock<IEmployeeEvaluationTemplateItemService>();
        serviceMock.Setup(x => x.Update(validDto)).ReturnsAsync(validDto);

        var controller = new EmployeeEvaluationTemplateItemController(serviceMock.Object);

        var result = await controller.UpdateEmployeeEvaluationTemplateItem(validDto);

        var okResult = Assert.IsType<OkResult>(result);
        Assert.Equal(200, okResult.StatusCode);
    }

    [Fact]
    public async Task UpdateEmployeeEvaluationTemplateItemExceptionThrownReturnsNotFoundResult()
    {
        var invalidDto =
            new EmployeeEvaluationTemplateItemDto
            {
                Id = 1,
                Template = new EmployeeEvaluationTemplateDto { Id = 101, Description = "Example Test Template" },
                Section = "Example Section",
                Question = "Example Question"
            };

        var serviceMock = new Mock<IEmployeeEvaluationTemplateItemService>();
        var exceptionMessage = "An error occurred while updating the employee evaluation template item.";
        serviceMock.Setup(x => x.Update(invalidDto)).ThrowsAsync(new Exception(exceptionMessage));

        var controller = new EmployeeEvaluationTemplateItemController(serviceMock.Object);

        var result = await controller.UpdateEmployeeEvaluationTemplateItem(invalidDto);

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        var actualExceptionMessage = Assert.IsType<string>(notFoundResult.Value);
        Assert.Equal(exceptionMessage, actualExceptionMessage);
    }

    [Fact]
    public async Task DeleteEmployeeEvaluationTemplateItemValidParametersReturnsOkResult()
    {
        var template = "Example Test Template";
        var section = "Example Section";
        var question = "Example Question";

        var serviceMock = new Mock<IEmployeeEvaluationTemplateItemService>();
        serviceMock.Setup(x => x.Delete(template, section, question))
                   .ReturnsAsync(new EmployeeEvaluationTemplateItemDto
                   {
                       Id = 1,
                       Template = new EmployeeEvaluationTemplateDto
                       {
                           Id = 101,
                           Description = "Example Test Template"
                       },
                       Section = " Example Section",
                       Question = "Example Question"
                   });

        var controller = new EmployeeEvaluationTemplateItemController(serviceMock.Object);

        var result = await controller.DeleteEmployeeEvaluationTemplateItem(template, section, question);

        var okResult = Assert.IsType<OkResult>(result);
        Assert.Equal(200, okResult.StatusCode);
    }

    [Fact]
    public async Task DeleteEmployeeEvaluationTemplateItemExceptionThrownReturnsNotFoundResult()
    {
        var template = "Example Test Template";
        var section = "Example Section";
        var question = "Example Question";

        var serviceMock = new Mock<IEmployeeEvaluationTemplateItemService>();
        var exceptionMessage = "An error occurred while deleting the employee evaluation template item.";
        serviceMock.Setup(x => x.Delete(template, section, question)).ThrowsAsync(new Exception(exceptionMessage));

        var controller = new EmployeeEvaluationTemplateItemController(serviceMock.Object);

        var result = await controller.DeleteEmployeeEvaluationTemplateItem(template, section, question);

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        var actualExceptionMessage = Assert.IsType<string>(notFoundResult.Value);
        Assert.Equal(exceptionMessage, actualExceptionMessage);
    }
}
