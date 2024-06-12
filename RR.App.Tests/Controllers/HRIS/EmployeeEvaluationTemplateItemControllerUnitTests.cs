using HRIS.Models;
using HRIS.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RR.App.Controllers.HRIS;
using RR.Tests.Data.Models.HRIS;
using Xunit;

namespace RR.App.Tests.Controllers.HRIS;
public class EmployeeEvaluationTemplateItemControllerUnitTests
{
    private readonly Mock<IEmployeeEvaluationTemplateItemService> _employeeEvaluationTemplateItemServiceMock;
    private readonly EmployeeEvaluationTemplateItemController _controller;
    private readonly List<EmployeeEvaluationTemplateItemDto> _employeeEvaluationTemplateItemDtoList;
    public EmployeeEvaluationTemplateItemControllerUnitTests()
    {
        _employeeEvaluationTemplateItemServiceMock = new Mock<IEmployeeEvaluationTemplateItemService>();
        _controller = new EmployeeEvaluationTemplateItemController(_employeeEvaluationTemplateItemServiceMock.Object);

        _employeeEvaluationTemplateItemDtoList = new List<EmployeeEvaluationTemplateItemDto>
        {
          EmployeeEvaluationTemplateItemTestData.employeeEvaluationTemplateItemDto,
          EmployeeEvaluationTemplateItemTestData.employeeEvaluationTemplateItemDto2,
        };
    }

    [Fact]
    public async Task GetAllEmployeeEvaluationTemplateItemsValidSectionReturnsOkResult()
    {
        _employeeEvaluationTemplateItemServiceMock.Setup(x => x.GetAllBySection("Example Test Section")).ReturnsAsync(_employeeEvaluationTemplateItemDtoList);

        var result = await _controller.GetAllEmployeeEvaluationTemplateItems("Example Test Section", null);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var actualTemplateItems = Assert.IsAssignableFrom<List<EmployeeEvaluationTemplateItemDto>>(okResult.Value);
        Assert.Equal(_employeeEvaluationTemplateItemDtoList.Count, actualTemplateItems.Count);
        Assert.All(actualTemplateItems, item => { Assert.Equal("Example Test Section", item.Section); });
    }

    [Fact]
    public async Task GetAllEmployeeEvaluationTemplateItemsValidTemplateReturnsOkResult()
    {
        _employeeEvaluationTemplateItemServiceMock.Setup(x => x.GetAllByTemplate(EmployeeEvaluationTemplateItemTestData.employeeEvaluationTemplateItemDto2.Template.Description))
            .ReturnsAsync(_employeeEvaluationTemplateItemDtoList);

        var result = await _controller.GetAllEmployeeEvaluationTemplateItems(null, EmployeeEvaluationTemplateItemTestData.employeeEvaluationTemplateItemDto2.Template.Description);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var actualTemplateItems = Assert.IsAssignableFrom<List<EmployeeEvaluationTemplateItemDto>>(okResult.Value);
        Assert.Equal(_employeeEvaluationTemplateItemDtoList.Count, actualTemplateItems.Count);
    }

    [Fact]
    public async Task GetAllEmployeeEvaluationTemplateItemsNoSectionOrTemplateReturnsOkResult()
    {
        _employeeEvaluationTemplateItemServiceMock.Setup(x => x.GetAll())
            .ReturnsAsync(_employeeEvaluationTemplateItemDtoList);

        var result = await _controller.GetAllEmployeeEvaluationTemplateItems(null, null);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var actualTemplateItems = Assert.IsAssignableFrom<List<EmployeeEvaluationTemplateItemDto>>(okResult.Value);
        Assert.Equal(_employeeEvaluationTemplateItemDtoList.Count, actualTemplateItems.Count);
    }

    [Fact]
    public async Task GetAllEmployeeEvaluationTemplateItemsExceptionThrownReturnsNotFoundResult()
    {
        _employeeEvaluationTemplateItemServiceMock.Setup(x => x.GetAll())
            .ThrowsAsync(new Exception("An error occurred while retrieving employee evaluation template items."));

        var result = await _controller.GetAllEmployeeEvaluationTemplateItems(null, null);

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

        _employeeEvaluationTemplateItemServiceMock.Setup(x => x.Save(template, section, question)).ReturnsAsync(EmployeeEvaluationTemplateItemTestData.employeeEvaluationTemplateItemDto);

        var result = await _controller.SaveEmployeeEvaluationTemplateItem(template, section, question);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var actualSavedItem = Assert.IsAssignableFrom<EmployeeEvaluationTemplateItemDto>(okResult.Value);
        Assert.Equal(EmployeeEvaluationTemplateItemTestData.employeeEvaluationTemplateItemDto, actualSavedItem);
    }

    [Fact]
    public async Task SaveEmployeeEvaluationTemplateItemExceptionThrownReturnsNotFoundResult()
    {
        var template = "Example Test Template";
        var section = "Example Test Section";
        var question = "Example Test Question";

        _employeeEvaluationTemplateItemServiceMock.Setup(x => x.Save(template, section, question)).ThrowsAsync(new Exception("An error occurred while saving the employee evaluation template item."));

        var result = await _controller.SaveEmployeeEvaluationTemplateItem(template, section, question);

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        var actualExceptionMessage = Assert.IsType<string>(notFoundResult.Value);
        Assert.Equal("An error occurred while saving the employee evaluation template item.", actualExceptionMessage);
    }

    [Fact]
    public async Task UpdateEmployeeEvaluationTemplateItemValidDtoReturnsOkResult()
    {
        _employeeEvaluationTemplateItemServiceMock.Setup(x => x.Update(EmployeeEvaluationTemplateItemTestData.employeeEvaluationTemplateItemDto)).ReturnsAsync(EmployeeEvaluationTemplateItemTestData.employeeEvaluationTemplateItemDto);

        var result = await _controller.UpdateEmployeeEvaluationTemplateItem(EmployeeEvaluationTemplateItemTestData.employeeEvaluationTemplateItemDto);

        var okResult = Assert.IsType<OkResult>(result);
        Assert.Equal(200, okResult.StatusCode);
    }

    [Fact]
    public async Task UpdateEmployeeEvaluationTemplateItemExceptionThrownReturnsNotFoundResult()
    {
        _employeeEvaluationTemplateItemServiceMock.Setup(x => x.Update(EmployeeEvaluationTemplateItemTestData.employeeEvaluationTemplateItemDto2)).ThrowsAsync(new Exception("An error occurred while updating the employee evaluation template item."));

        var result = await _controller.UpdateEmployeeEvaluationTemplateItem(EmployeeEvaluationTemplateItemTestData.employeeEvaluationTemplateItemDto2);

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        var actualExceptionMessage = Assert.IsType<string>(notFoundResult.Value);
        Assert.Equal("An error occurred while updating the employee evaluation template item.", actualExceptionMessage);
    }

    [Fact]
    public async Task DeleteEmployeeEvaluationTemplateItemValidParametersReturnsOkResult()
    {
        var template = "Example Test Template";
        var section = "Example Section";
        var question = "Example Question";

        _employeeEvaluationTemplateItemServiceMock.Setup(x => x.Delete(template, section, question))
                   .ReturnsAsync(EmployeeEvaluationTemplateItemTestData.employeeEvaluationTemplateItemDto);

        var result = await _controller.DeleteEmployeeEvaluationTemplateItem(template, section, question);

        var okResult = Assert.IsType<OkResult>(result);
        Assert.Equal(200, okResult.StatusCode);
    }

    [Fact]
    public async Task DeleteEmployeeEvaluationTemplateItemExceptionThrownReturnsNotFoundResult()
    {
        var template = "Example Test Template";
        var section = "Example Section";
        var question = "Example Question";

        _employeeEvaluationTemplateItemServiceMock.Setup(x => x.Delete(template, section, question)).ThrowsAsync(new Exception("An error occurred while deleting the employee evaluation template item."));

        var result = await _controller.DeleteEmployeeEvaluationTemplateItem(template, section, question);

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        var actualExceptionMessage = Assert.IsType<string>(notFoundResult.Value);
        Assert.Equal("An error occurred while deleting the employee evaluation template item.", actualExceptionMessage);
    }
}