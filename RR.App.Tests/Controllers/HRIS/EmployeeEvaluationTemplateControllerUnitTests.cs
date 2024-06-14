using HRIS.Models;
using HRIS.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RR.App.Controllers.HRIS;
using Xunit;

namespace RR.App.Tests.Controllers.HRIS;

public class EmployeeEvaluationTemplateControllerUnitTests
{
    private readonly Mock<IEmployeeEvaluationTemplateService> _employeeEvaluationTemplateServiceMock;
    private readonly EmployeeEvaluationTemplateController _controller;
    private readonly EmployeeEvaluationTemplateDto _employeeEvaluationTemplateDto;
    private readonly List<EmployeeEvaluationTemplateDto> _employeeEvaluationTemplateDtoList;

    public EmployeeEvaluationTemplateControllerUnitTests()
    {
        _employeeEvaluationTemplateServiceMock = new Mock<IEmployeeEvaluationTemplateService>();
        _controller = new EmployeeEvaluationTemplateController(_employeeEvaluationTemplateServiceMock.Object);

        _employeeEvaluationTemplateDto = new EmployeeEvaluationTemplateDto { Id = 1, Description = "Test Template Description" };

        _employeeEvaluationTemplateDtoList = new List<EmployeeEvaluationTemplateDto>
        {
            _employeeEvaluationTemplateDto,
            _employeeEvaluationTemplateDto
        };
    }

    [Fact]
    public async Task GetEmployeeEvaluationTemplateValidInputReturnsOkResult()
    {
        _employeeEvaluationTemplateServiceMock.Setup(x => x.Get("Template 1")).ReturnsAsync(_employeeEvaluationTemplateDto);

        var result = await _controller.GetEmployeeEvaluationTemplate("Template 1");

        var okResult = Assert.IsType<OkObjectResult>(result);
        var actualTemplate = Assert.IsType<EmployeeEvaluationTemplateDto>(okResult.Value);
        Assert.Equal(_employeeEvaluationTemplateDto.Id, actualTemplate.Id);
        Assert.Equal(_employeeEvaluationTemplateDto.Description, actualTemplate.Description);
    }

    [Fact]
    public async Task GetEmployeeEvaluationTemplateExceptionThrownReturnsNotFoundResultWithErrorMessage()
    {
        _employeeEvaluationTemplateServiceMock.Setup(x => x.Get("Template 1")).ThrowsAsync(new Exception("Error retrieving employee evaluation template"));

        var result = await _controller.GetEmployeeEvaluationTemplate("Template 1");

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        var actualErrorMessage = Assert.IsType<string>(notFoundResult.Value);
        Assert.Equal("Error retrieving employee evaluation template", actualErrorMessage);
    }

    [Fact]
    public async Task GetAllEmployeeEvaluationTemplatesValidInputReturnsOkResult()
    {
        _employeeEvaluationTemplateServiceMock.Setup(x => x.GetAll()).ReturnsAsync(_employeeEvaluationTemplateDtoList);

        var result = await _controller.GetAllEmployeeEvaluationTemplates();

        var okResult = Assert.IsType<OkObjectResult>(result);
        var actualTemplates = Assert.IsType<List<EmployeeEvaluationTemplateDto>>(okResult.Value);
        Assert.Equal(_employeeEvaluationTemplateDtoList.Count, actualTemplates.Count);
    }

    [Fact]
    public async Task GetAllEmployeeEvaluationTemplatesExceptionThrownReturnsNotFoundResultWithErrorMessage()
    {
        _employeeEvaluationTemplateServiceMock.Setup(x => x.GetAll()).ThrowsAsync(new Exception("Error retrieving employee evaluation templates"));

        var result = await _controller.GetAllEmployeeEvaluationTemplates();

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        var actualErrorMessage = Assert.IsType<string>(notFoundResult.Value);
        Assert.Equal("Error retrieving employee evaluation templates", actualErrorMessage);
    }

    [Fact]
    public async Task SaveEmployeeEvaluationTemplateValidInputReturnsOkResult()
    {
        _employeeEvaluationTemplateServiceMock.Setup(x => x.Save("Template Test 123")).ReturnsAsync(_employeeEvaluationTemplateDto);

        var result = await _controller.SaveEmployeeEvaluationTemplate("Template Test 123");

        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task SaveEmployeeEvaluationTemplateExceptionThrownReturnsNotFoundResultWithErrorMessage()
    {
        _employeeEvaluationTemplateServiceMock.Setup(x => x.Save("Template 1")).ThrowsAsync(new Exception("Error saving evaluation template"));

        var result = await _controller.SaveEmployeeEvaluationTemplate("Template 1");

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        var actualErrorMessage = Assert.IsType<string>(notFoundResult.Value);
        Assert.Equal("Error saving evaluation template", actualErrorMessage);
    }

    [Fact]
    public async Task UpdateEmployeeEvaluationTemplateValidInputReturnsOkResult()
    {
        _employeeEvaluationTemplateServiceMock.Setup(x => x.Update(It.IsAny<EmployeeEvaluationTemplateDto>())).ReturnsAsync(_employeeEvaluationTemplateDto);

        var result = await _controller.UpdateEmployeeEvaluationTemplate(_employeeEvaluationTemplateDto);

        Assert.IsType<OkResult>(result);
    }

    [Fact]
    public async Task UpdateEmployeeEvaluationTemplateExceptionThrownReturnsNotFoundResultWithErrorMessage()
    {
        _employeeEvaluationTemplateServiceMock.Setup(x => x.Update(It.IsAny<EmployeeEvaluationTemplateDto>())).ThrowsAsync(new Exception("Error updating evaluation template"));

        var result = await _controller.UpdateEmployeeEvaluationTemplate(_employeeEvaluationTemplateDto);

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        var actualErrorMessage = Assert.IsType<string>(notFoundResult.Value);
        Assert.Equal("Error updating evaluation template", actualErrorMessage);
    }

    [Fact]
    public async Task DeleteEmployeeEvaluationTemplateValidInputReturnsOkResult()
    {
        _employeeEvaluationTemplateServiceMock.Setup(x => x.Delete("Template To Delete")).ReturnsAsync(_employeeEvaluationTemplateDto);

        var result = await _controller.DeleteEmployeeEvaluationTemplate("Template To Delete");

        Assert.IsType<OkResult>(result);
    }

    [Fact]
    public async Task DeleteEmployeeEvaluationTemplateExceptionThrownReturnsNotFoundResultWithErrorMessage()
    {
        _employeeEvaluationTemplateServiceMock.Setup(x => x.Delete("Template To Delete")).ThrowsAsync(new Exception("Error deleting evaluation template"));

        var result = await _controller.DeleteEmployeeEvaluationTemplate("Template To Delete");

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        var actualErrorMessage = Assert.IsType<string>(notFoundResult.Value);
        Assert.Equal("Error deleting evaluation template", actualErrorMessage);
    }
}