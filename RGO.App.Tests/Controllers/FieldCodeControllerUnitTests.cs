using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using Moq;
using RGO.App.Controllers;
using RGO.Models;
using RGO.Models.Enums;
using RGO.Services.Interfaces;
using Xunit;

namespace RGO.App.Tests.Controllers
{
    public class FieldCodeControllerTests
    {
        private readonly Mock<IFieldCodeService> _fieldCodeServiceMock;
        private readonly FieldCodeController _controller;

        public FieldCodeControllerTests()
        {
            _fieldCodeServiceMock = new Mock<IFieldCodeService>();
            _controller = new FieldCodeController(_fieldCodeServiceMock.Object);
        }

        [Fact]
        public async Task GetAllFieldCodesReturnsOkResultWithListOfFieldCodes()
        {
            var fieldCodes = new List<FieldCodeDto>
            {
                new FieldCodeDto(1,"Code1", "Name1", "Description1", "Regex1", FieldCodeType.String, ItemStatus.Active, true, "InternalTable1", 0),
                new FieldCodeDto(2, "Code2", "Name2", "Description2", "Regex2", FieldCodeType.String, ItemStatus.Active, true, "InternalTable2", 0)
            };

            _fieldCodeServiceMock.Setup(s => s.GetAllFieldCodes()).ReturnsAsync(fieldCodes);

            var result = await _controller.GetAllFieldCodes();

            var okResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<FieldCodeDto>>(okResult.Value);
            Assert.Equal(fieldCodes, model);
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
            var fieldCodeDto = new FieldCodeDto(1, "Code1", "Name1", "Description1", "Regex1", FieldCodeType.String, ItemStatus.Active, true, "InternalTable1", 0);
            var savedFieldCode = new FieldCodeDto(2, "Code2", "Name2", "Description2", "Regex2", FieldCodeType.String, ItemStatus.Active, true, "InternalTable2", 0);
            _fieldCodeServiceMock.Setup(s => s.SaveFieldCode(fieldCodeDto)).ReturnsAsync(savedFieldCode);

            var result = await _controller.SaveFieldCode(fieldCodeDto);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<FieldCodeDto>(okResult.Value);
            Assert.Equal(savedFieldCode, model);
        }

        [Fact]
        public async Task SaveFieldCodeReturnsNotFoundResultWhenExceptionThrown()
        {
            var fieldCodeDto = new FieldCodeDto(1, "Code1", "Name1", "Description1", "Regex1", FieldCodeType.String, ItemStatus.Active, true, "InternalTable1", 0);
            var exceptionMessage = "An error occurred";
            _fieldCodeServiceMock.Setup(s => s.SaveFieldCode(fieldCodeDto)).ThrowsAsync(new Exception(exceptionMessage));

            var result = await _controller.SaveFieldCode(fieldCodeDto);

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(exceptionMessage, notFoundResult.Value);
        }

        [Fact]
        public async Task UpdateFieldCodeReturnsOkResultWithUpdatedFieldCode()
        {
            var fieldCodeDto = new FieldCodeDto(1, "Code1", "Name1", "Description1", "Regex1", FieldCodeType.String, ItemStatus.Active, true, "InternalTable1", 0);
            var updatedFieldCode = new FieldCodeDto(2, "Code2", "Name2", "Description2", "Regex2", FieldCodeType.String, ItemStatus.Active, true, "InternalTable2", 0);
            _fieldCodeServiceMock.Setup(s => s.UpdateFieldCode(fieldCodeDto)).ReturnsAsync(updatedFieldCode);

            var result = await _controller.UpdateFieldCode(fieldCodeDto);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<FieldCodeDto>(okResult.Value);
            Assert.Equal(updatedFieldCode, model);
        }

        [Fact]
        public async Task UpdateFieldCodeReturnsNotFoundResultWhenExceptionThrown()
        {
            var fieldCodeDto = new FieldCodeDto(1, "Code1", "Name1", "Description1", "Regex1", FieldCodeType.String, ItemStatus.Active, true, "InternalTable1", 0);
            var exceptionMessage = "An error occurred";
            _fieldCodeServiceMock.Setup(s => s.UpdateFieldCode(fieldCodeDto)).ThrowsAsync(new Exception(exceptionMessage));

            var result = await _controller.UpdateFieldCode(fieldCodeDto);

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(exceptionMessage, notFoundResult.Value);
        }

        [Fact]
        public async Task DeleteFieldCodeReturnsOkResultWithDeletedFieldCode()
        {
            var fieldCodeDto = new FieldCodeDto(1, "Code1", "Name1", "Description1", "Regex1", FieldCodeType.String, ItemStatus.Active, true, "InternalTable1", 0);
            var deletedFieldCode = new FieldCodeDto(1, "Code1", "Name1", "Description1", "Regex1", FieldCodeType.String, ItemStatus.Active, true, "InternalTable1", 0);
            _fieldCodeServiceMock.Setup(s => s.DeleteFieldCode(fieldCodeDto)).ReturnsAsync(deletedFieldCode);

            var result = await _controller.DeleteFieldCode(fieldCodeDto);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<FieldCodeDto>(okResult.Value);
            Assert.Equal(deletedFieldCode, model);
        }

        [Fact]
        public async Task DeleteFieldCodeReturnsNotFoundResultWhenExceptionThrown()
        {
            var fieldCodeDto = new FieldCodeDto(1, "Code1", "Name1", "Description1", "Regex1", FieldCodeType.String, ItemStatus.Active, true, "InternalTable1", 0);
            var exceptionMessage = "An error occurred";
            _fieldCodeServiceMock.Setup(s => s.DeleteFieldCode(fieldCodeDto)).ThrowsAsync(new Exception(exceptionMessage));

            var result = await _controller.DeleteFieldCode(fieldCodeDto);

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(exceptionMessage, notFoundResult.Value);
        }

        [Fact]
        public async Task GetByCategoryValidCategoryReturnsOkResultWithCategoryCodes()
        {
            var validCategory = 1;
            var expectedCategoryCodes = new List<FieldCodeDto>
            {
                new FieldCodeDto(1, "Code1", "Name1", "Description1", "Regex1", FieldCodeType.String, ItemStatus.Active, true, "InternalTable1", FieldCodeCategory.Banking)
            };

            var mockFieldCodeService = new Mock<IFieldCodeService>();
            mockFieldCodeService.Setup(service => service.GetByCategory(validCategory))
                .ReturnsAsync(expectedCategoryCodes);

            var controller = new FieldCodeController(mockFieldCodeService.Object);

            var result = await controller.GetByCategory(validCategory);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var actualCategoryCodes = Assert.IsAssignableFrom<List<FieldCodeDto>>(okResult.Value);
            Assert.Equal(expectedCategoryCodes, actualCategoryCodes);
        }

        [Fact]
        public async Task GetByCategoryInvalidCategoryReturnsNotFoundResult()
        {
            var invalidCategory = -1;

            var mockFieldCodeService = new Mock<IFieldCodeService>();
            mockFieldCodeService.Setup(s => s.GetByCategory(invalidCategory))
                .ThrowsAsync(new Exception("Invalid Index"));

            var controller = new FieldCodeController(mockFieldCodeService.Object);

            var result = await controller.GetByCategory(invalidCategory);

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Invalid Index", notFoundResult.Value);
        }
    }
}