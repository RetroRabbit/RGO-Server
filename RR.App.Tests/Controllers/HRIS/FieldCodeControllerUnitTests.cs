using HRIS.Models;
using HRIS.Models.Enums;
using HRIS.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RR.App.Controllers.HRIS;
using Xunit;

namespace RR.App.Tests.Controllers.HRIS;

public class FieldCodeControllerTests
{
    private readonly FieldCodeController _controller;
    private readonly Mock<IFieldCodeService> _fieldCodeServiceMock;
    private readonly FieldCodeDto _fieldCodeDto;
    private readonly List<FieldCodeDto> _fieldCodeDtoList;

    public FieldCodeControllerTests()
    {
        _fieldCodeServiceMock = new Mock<IFieldCodeService>();
        _controller = new FieldCodeController(_fieldCodeServiceMock.Object);

        _fieldCodeDto = new FieldCodeDto
        {
            Id = 1,
            Code = "Code1",
            Name = "Name1",
            Description = "Description1",
            Regex = "Regex1",
            Type = FieldCodeType.String,
            Status = ItemStatus.Active,
            Internal = true,
            InternalTable = "InternalTable1",
            Category = 0,
            Required = false
        };

        _fieldCodeDtoList = new List<FieldCodeDto>
        {
            _fieldCodeDto,
        };

    }

    [Fact]
    public async Task GetAllFieldCodesReturnsOkResultWithListOfFieldCodes()
    {
        _fieldCodeServiceMock.Setup(s => s.GetAllFieldCodes()).ReturnsAsync(_fieldCodeDtoList);

        var result = await _controller.GetAllFieldCodes();

        var okResult = Assert.IsType<OkObjectResult>(result);
        var model = Assert.IsAssignableFrom<IEnumerable<FieldCodeDto>>(okResult.Value);
        Assert.Equal(_fieldCodeDtoList, model);
    }

    [Fact]
    public async Task GetAllFieldCodesReturnsNotFoundResultWhenNoFieldCodesFound()
    {
        _fieldCodeServiceMock.Setup(s => s.GetAllFieldCodes()).ReturnsAsync((List<FieldCodeDto>?)null);

        var result = await _controller.GetAllFieldCodes();

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("No field codes found", notFoundResult.Value);
    }

    [Fact]
    public async Task SaveFieldCodeReturnsOkResultWithSavedFieldCode()
    {
        _fieldCodeServiceMock.Setup(s => s.SaveFieldCode(_fieldCodeDto)).ReturnsAsync(_fieldCodeDto);

        var result = await _controller.SaveFieldCode(_fieldCodeDto);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var model = Assert.IsAssignableFrom<FieldCodeDto>(okResult.Value);
        Assert.Equal(_fieldCodeDto, model);
    }

    [Fact]
    public async Task SaveFieldCodeReturnsNotFoundResultWhenExceptionThrown()
    {
        _fieldCodeServiceMock.Setup(s => s.SaveFieldCode(_fieldCodeDto)).ThrowsAsync(new Exception("An error occurred"));

        var result = await _controller.SaveFieldCode(_fieldCodeDto);

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("An error occurred", notFoundResult.Value);
    }

    [Fact]
    public async Task UpdateFieldCodeReturnsOkResultWithUpdatedFieldCode()
    {
        _fieldCodeServiceMock.Setup(s => s.UpdateFieldCode(_fieldCodeDto)).ReturnsAsync(_fieldCodeDto);

        var result = await _controller.UpdateFieldCode(_fieldCodeDto);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var model = Assert.IsAssignableFrom<FieldCodeDto>(okResult.Value);
        Assert.Equal(_fieldCodeDto, model);
    }

    [Fact]
    public async Task UpdateFieldCodeReturnsNotFoundResultWhenExceptionThrown()
    {
        _fieldCodeServiceMock.Setup(s => s.UpdateFieldCode(_fieldCodeDto)).ThrowsAsync(new Exception("An error occurred"));

        var result = await _controller.UpdateFieldCode(_fieldCodeDto);

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("An error occurred", notFoundResult.Value);
    }

    [Fact]
    public async Task DeleteFieldCodeReturnsOkResultWithDeletedFieldCode()
    {
        _fieldCodeServiceMock.Setup(s => s.DeleteFieldCode(_fieldCodeDto)).ReturnsAsync(_fieldCodeDto);

        var result = await _controller.DeleteFieldCode(_fieldCodeDto);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var model = Assert.IsAssignableFrom<FieldCodeDto>(okResult.Value);
        Assert.Equal(_fieldCodeDto, model);
    }

    [Fact]
    public async Task DeleteFieldCodeReturnsNotFoundResultWhenExceptionThrown()
    {
        _fieldCodeServiceMock.Setup(s => s.DeleteFieldCode(_fieldCodeDto)).ThrowsAsync(new Exception("An error occurred"));

        var result = await _controller.DeleteFieldCode(_fieldCodeDto);

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("An error occurred", notFoundResult.Value);
    }

    [Fact]
    public async Task GetByCategoryValidCategoryReturnsOkResultWithCategoryCodes()
    {
        _fieldCodeServiceMock.Setup(service => service.GetByCategory(1))
                            .ReturnsAsync(_fieldCodeDtoList);

        var result = await _controller.GetByCategory(1);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var actualCategoryCodes = Assert.IsAssignableFrom<List<FieldCodeDto>>(okResult.Value);
        Assert.Equal(_fieldCodeDtoList, actualCategoryCodes);
    }

    [Fact]
    public async Task GetByCategoryInvalidCategoryReturnsNotFoundResult()
    {
        _fieldCodeServiceMock.Setup(s => s.GetByCategory(-1))
                            .ThrowsAsync(new Exception("Invalid Index"));

        var result = await _controller.GetByCategory(-1);

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("Invalid Index", notFoundResult.Value);
    }
}