using HRIS.Models;
using HRIS.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RR.App.Controllers.HRIS;
using RR.Tests.Data.Models.HRIS;
using Xunit;

namespace RR.App.Tests.Controllers.HRIS;

public class EmployeeAddressControllerUnitTests
{
    [Fact]
    public async Task GetAllReturnsOkResultWithAddresses()
    {
        var expectedAddresses = new List<EmployeeAddressDto>
        {
            EmployeeAddressTestData.EmployeeAddressDto2,
            EmployeeAddressTestData.EmployeeAddressDto3
        };

        var mockEmployeeAddressService = new Mock<IEmployeeAddressService>();
        mockEmployeeAddressService.Setup(s => s.GetAll()).ReturnsAsync(expectedAddresses);

        var controller = new EmployeeAddressController(mockEmployeeAddressService.Object);

        var result = await controller.GetAll();

        var okResult = Assert.IsType<OkObjectResult>(result);
        var actualAddresses = Assert.IsAssignableFrom<List<EmployeeAddressDto>>(okResult.Value);
        Assert.Equal(expectedAddresses, actualAddresses);
    }

    [Fact]
    public async Task GetAllReturnsNotFoundResultWhenExceptionThrown()
    {
        var exceptionMessage = "An error occurred while retrieving addresses";

        var mockEmployeeAddressService = new Mock<IEmployeeAddressService>();
        mockEmployeeAddressService.Setup(s => s.GetAll()).ThrowsAsync(new Exception(exceptionMessage));

        var controller = new EmployeeAddressController(mockEmployeeAddressService.Object);
        var result = await controller.GetAll();

        var noFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal(exceptionMessage, noFoundResult.Value);
    }

    [Fact]
    public async Task SaveEmployeeAddressReturnsOkResultWithSavedAddress()
    {
        var addressToSave = EmployeeAddressTestData.EmployeeAddressDto2;
        var savedAddress = EmployeeAddressTestData.EmployeeAddressDto2;

        var mockEmployeeAddressService = new Mock<IEmployeeAddressService>();
        mockEmployeeAddressService.Setup(s => s.Save(addressToSave)).ReturnsAsync(savedAddress);

        var controller = new EmployeeAddressController(mockEmployeeAddressService.Object);

        var result = await controller.SaveEmployeeAddress(addressToSave);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var actualAddress = Assert.IsAssignableFrom<EmployeeAddressDto>(okResult.Value);
        Assert.Equal(savedAddress, actualAddress);
    }

    [Fact]
    public async Task SaveEmployeeAddressThrowsExceptionReturnsNotFoundResult()
    {
        var addressToSave = EmployeeAddressTestData.EmployeeAddressDto2;
        var exceptionMessage = "An error occurred while saving the address.";

        var mockEmployeeAddressService = new Mock<IEmployeeAddressService>();
        mockEmployeeAddressService.Setup(s => s.Save(addressToSave)).ThrowsAsync(new Exception(exceptionMessage));

        var controller = new EmployeeAddressController(mockEmployeeAddressService.Object);

        var result = await controller.SaveEmployeeAddress(addressToSave);

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal(exceptionMessage, notFoundResult.Value);
    }

    [Fact]
    public async Task UpdateEmployeeAddressReturnsOkResultWithUpdatedAddress()
    {
        var addressToUpdate = EmployeeAddressTestData.EmployeeAddressDto2;
        var updatedAddress = new EmployeeAddressDto(1, "Updated 1", "Updated Complex Name 1", "Updated Street Number 1",
                                                    "Updated Suburb or District 1", "Updated City 1",
                                                    "Updated Country 1", "Updated Province 1", "0002");

        var mockEmployeeAddressService = new Mock<IEmployeeAddressService>();
        mockEmployeeAddressService.Setup(s => s.Update(addressToUpdate)).ReturnsAsync(updatedAddress);

        var controller = new EmployeeAddressController(mockEmployeeAddressService.Object);

        var result = await controller.UpdateEmployeeAddress(addressToUpdate);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var actualAddress = Assert.IsAssignableFrom<EmployeeAddressDto>(okResult.Value);
        Assert.Equal(updatedAddress, actualAddress);
    }

    [Fact]
    public async Task UpdateEmployeeAddressReturnsNotFoundResultWhenExceptionThrown()
    {
        var addressToUpdate = EmployeeAddressTestData.EmployeeAddressDto2;
        var exceptionMessage = "An error occurred while updating the address.";

        var mockEmployeeAddressService = new Mock<IEmployeeAddressService>();
        mockEmployeeAddressService.Setup(s => s.Update(addressToUpdate)).ThrowsAsync(new Exception(exceptionMessage));

        var controller = new EmployeeAddressController(mockEmployeeAddressService.Object);

        var result = await controller.UpdateEmployeeAddress(addressToUpdate);

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal(exceptionMessage, notFoundResult.Value);
    }

    [Fact]
    public async Task DeleteEmployeeAddressReturnsOkResultWithDeletedAddress()
    {
        var addressToDelete = EmployeeAddressTestData.EmployeeAddressDto2;
        var deletedAddress = new EmployeeAddressDto(1, "Deleted 1", "Deleted Complex Name 1", "Deleted Street Number 1",
                                                    "Deleted Suburb or District 1", "Deleted City 1",
                                                    "Deleted Country 1", "Deleted Province 1", "0002");

        var mockEmployeeAddressService = new Mock<IEmployeeAddressService>();
        mockEmployeeAddressService.Setup(s => s.Delete(addressToDelete.Id)).ReturnsAsync(deletedAddress);

        var controller = new EmployeeAddressController(mockEmployeeAddressService.Object);

        var result = await controller.DeleteEmployeeAddress(addressToDelete.Id);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var actualAddress = Assert.IsAssignableFrom<EmployeeAddressDto>(okResult.Value);
        Assert.Equal(deletedAddress, actualAddress);
    }

    [Fact]
    public async Task DeleteEmployeeAddressReturnsNotFoundResultWhenExceptionThrown()
    {
        var addressToDelete = EmployeeAddressTestData.EmployeeAddressDto2;
        var exceptionMessage = "An error occurred while deleting the address.";

        var mockEmployeeAddressService = new Mock<IEmployeeAddressService>();
        mockEmployeeAddressService.Setup(s => s.Delete(addressToDelete.Id))
                                  .ThrowsAsync(new Exception(exceptionMessage));

        var controller = new EmployeeAddressController(mockEmployeeAddressService.Object);

        var result = await controller.DeleteEmployeeAddress(addressToDelete.Id);

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal(exceptionMessage, notFoundResult.Value);
    }
}