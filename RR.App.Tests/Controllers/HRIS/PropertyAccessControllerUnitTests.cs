using HRIS.Models;
using HRIS.Models.Update;
using HRIS.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RR.App.Controllers.HRIS;
using Xunit;

namespace RR.App.Tests.Controllers.HRIS;

public class PropertyAccessControllerUnitTests
{
    [Fact]
    public async Task GetPropertyWithAccessReturnsOkResult()
    {
        var email = "test@retrorabbit.co.za";
        var mockPropertyAccessService = new Mock<IPropertyAccessService>();
        mockPropertyAccessService.Setup(service => service.GetPropertiesWithAccess(email))
                                 .ReturnsAsync(new List<EmployeeAccessDto>());

        var controller = new PropertyAccessController(mockPropertyAccessService.Object);

        var result = await controller.GetPropertyWithAccess(email);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var model = Assert.IsAssignableFrom<List<EmployeeAccessDto>>(okResult.Value);

        mockPropertyAccessService.Verify(service => service.GetPropertiesWithAccess(email), Times.Once);
    }

    [Fact]
    public async Task GetPropertyWithAccessReturnsNotFoundResult()
    {
        var email = "test@retrorabbit.co.za";
        var mockPropertyAccessService = new Mock<IPropertyAccessService>();
        mockPropertyAccessService.Setup(service => service.GetPropertiesWithAccess(email))
                                 .ThrowsAsync(new
                                                  Exception("Error retrieving properties with access for the specified user."));

        var controller = new PropertyAccessController(mockPropertyAccessService.Object);

        var result = await controller.GetPropertyWithAccess(email);

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        var errorMessage = Assert.IsType<string>(notFoundResult.Value);
        Assert.Equal("Error retrieving properties with access for the specified user.", errorMessage);

        mockPropertyAccessService.Verify(service => service.GetPropertiesWithAccess(email), Times.Once);
    }

    [Fact]
    public async Task UpdatePropertyWithAccessReturnsOkResult()
    {
        var email = "test@retrorabbit.co.za";
        var fields = new List<UpdateFieldValueDto>();
        var mockPropertyAccessService = new Mock<IPropertyAccessService>();
        mockPropertyAccessService.Setup(service => service.UpdatePropertiesWithAccess(fields, email))
                                 .Returns(Task.CompletedTask);

        var controller = new PropertyAccessController(mockPropertyAccessService.Object);

        var result = await controller.UpdatePropertyWithAccess(fields, email);

        var okResult = Assert.IsType<OkResult>(result);

        mockPropertyAccessService.Verify(service => service.UpdatePropertiesWithAccess(fields, email), Times.Once);
    }

    [Fact]
    public async Task UpdatePropertyWithAccessReturnsNotFoundResult()
    {
        var email = "test@retrorabbit.co.za";
        var fields = new List<UpdateFieldValueDto>();
        var mockPropertyAccessService = new Mock<IPropertyAccessService>();
        mockPropertyAccessService.Setup(service => service.UpdatePropertiesWithAccess(fields, email))
                                 .ThrowsAsync(new
                                                  Exception("Error updating properties with access for the specified user."));

        var controller = new PropertyAccessController(mockPropertyAccessService.Object);

        var result = await controller.UpdatePropertyWithAccess(fields, email);

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        var errorMessage = Assert.IsType<string>(notFoundResult.Value);
        Assert.Equal("Error updating properties with access for the specified user.", errorMessage);

        mockPropertyAccessService.Verify(service => service.UpdatePropertiesWithAccess(fields, email), Times.Once);
    }
}