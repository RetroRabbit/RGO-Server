using HRIS.Models;
using HRIS.Models.Enums;
using HRIS.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RR.App.Controllers.HRIS;
using System.Security.Claims;
using Xunit;

namespace RR.App.Tests.Controllers.HRIS;

public class PropertyAccessControllerUnitTests
{

    private readonly Mock<IPropertyAccessService> _propertyAccessMockService;
    private readonly Mock<IEmployeeService> _employeeMockService;
    private readonly PropertyAccessController _propertyAccessController;

    public PropertyAccessControllerUnitTests() {
        _propertyAccessMockService = new Mock<IPropertyAccessService>();
        _employeeMockService = new Mock<IEmployeeService>();
        _propertyAccessController = new PropertyAccessController(_propertyAccessMockService.Object, _employeeMockService.Object);
    }

    [Fact]
    public async Task GeAllPropertyWithAccessReturnsOkResult()
    {
        _propertyAccessMockService.Setup(service => service.GetAll())
                                 .ReturnsAsync(new List<PropertyAccessDto>());

        var result = await _propertyAccessController.GetAllPropertyAccess();

        var okResult = Assert.IsType<OkObjectResult>(result);
        var model = Assert.IsAssignableFrom<List<PropertyAccessDto>>(okResult.Value);

        _propertyAccessMockService.Verify(service => service.GetAll(), Times.Once);
    }

    [Fact]
    public async Task GeAllPropertyWithAccessNotFoundResult()
    {
        _propertyAccessMockService.Setup(service => service.GetAll())
                                 .ThrowsAsync(new
                                                  Exception("Error retrieving properties with access for the specified user."));

        var result = await _propertyAccessController.GetAllPropertyAccess();

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        var errorMessage = Assert.IsType<string>(notFoundResult.Value);
        Assert.Equal("Error retrieving properties with access for the specified user.", errorMessage);

    }

    [Fact]
    public async Task GetPropertyWithAccessReturnsOkResult()
    {
        var userId = 0;

        _propertyAccessMockService.Setup(service => service.GetAccessListByEmployeeId(userId))
                                 .ReturnsAsync(new List<PropertyAccessDto>());

        var result = await _propertyAccessController.GetPropertiesWithAccess(userId);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var model = Assert.IsAssignableFrom<List<PropertyAccessDto>>(okResult.Value);

        _propertyAccessMockService.Verify(service => service.GetAccessListByEmployeeId(userId), Times.Once);
    }

    [Fact]
    public async Task GetPropertyWithAccessReturnsNotFoundResult()
    {
        var userId = 0;

        _propertyAccessMockService.Setup(service => service.GetAccessListByEmployeeId(userId))
                                 .ThrowsAsync(new
                                                  Exception("Error retrieving properties with access for the specified user."));

        var result = await _propertyAccessController.GetPropertiesWithAccess(userId);

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        var errorMessage = Assert.IsType<string>(notFoundResult.Value);
        Assert.Equal("Error retrieving properties with access for the specified user.", errorMessage);

        _propertyAccessMockService.Verify(service => service.GetAccessListByEmployeeId(userId), Times.Once);
    }

    [Fact]
    public async Task SeedPropertiesSuccess()
    {
        var result = await _propertyAccessController.Seed();

        Assert.IsType<OkResult>(result);
    }

    [Fact]
    public async Task SeedPropertiesFail()
    {
        _propertyAccessMockService.Setup(service => service.CreatePropertyAccessEntries())
                                .ThrowsAsync(new
                                                 Exception("Error retrieving properties with access for the specified user."));
        var result = await _propertyAccessController.Seed();

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        var errorMessage = Assert.IsType<string>(notFoundResult.Value);
    }

    [Fact]
    public async Task UpdatePropertyRoleAccessReturnsSuccess()
    {
        _propertyAccessMockService.Setup(service => service.UpdatePropertyAccess(0, PropertyAccessLevel.read));

        var result = await _propertyAccessController.UpdatePropertyRoleAccess(0, PropertyAccessLevel.read);

        Assert.IsType<OkResult>(result);

        _propertyAccessMockService.Verify(service => service.UpdatePropertyAccess(0, PropertyAccessLevel.read), Times.Once);
    }

    [Fact]
    public async Task UpdatePropertyRoleAccessReturnsFail()
    {
        _propertyAccessMockService.Setup(service => service.UpdatePropertyAccess(0, PropertyAccessLevel.read))
                                 .ThrowsAsync(new
                                                  Exception("Error updating properties with access for the specified user."));

        var result = await _propertyAccessController.UpdatePropertyRoleAccess(0, PropertyAccessLevel.read);

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        var errorMessage = Assert.IsType<string>(notFoundResult.Value);
        Assert.Equal("Error updating properties with access for the specified user.", errorMessage);

        _propertyAccessMockService.Verify(service => service.UpdatePropertyAccess(0, PropertyAccessLevel.read), Times.Once);
    }

    [Fact]
    public async Task GetUserIdReturnsOkResult()
    {
        var email = "test@retrorabbit.co.za";
        var employeeId = 1;
        var authUserId = "authUser123";

        var employee = new EmployeeDto
        {
            Id = employeeId,
            AuthUserId = null
        };

        var claims = new List<Claim>
        {
        new Claim(ClaimTypes.NameIdentifier, authUserId)
        };
        var identity = new ClaimsIdentity(claims, "TestAuthType");
        var claimsPrincipal = new ClaimsPrincipal(identity);

        _employeeMockService.Setup(service => service.GetEmployee(email))
                            .ReturnsAsync(employee);

        var httpContext = new DefaultHttpContext
        {
            User = claimsPrincipal
        };
        _propertyAccessController.ControllerContext = new ControllerContext
        {
            HttpContext = httpContext
        };

        var result = await _propertyAccessController.GetUserId(email);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(employeeId, okResult.Value);

        _employeeMockService.Verify(service => service.GetEmployee(email), Times.Once);

        _employeeMockService.Verify(service => service.UpdateEmployee(It.Is<EmployeeDto>(e =>
            e.Id == employeeId && e.AuthUserId == authUserId), email), Times.Once);
    }

    [Fact]
    public async Task GetUserIdReturnsNotFoundResultWhenExceptionThrown()
    {
        var email = "test@retrorabbit.co.za";

        _employeeMockService.Setup(service => service.GetEmployee(email))
                            .ThrowsAsync(new Exception("Employee not found"));

        var result = await _propertyAccessController.GetUserId(email);

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("Employee not found", notFoundResult.Value);

        _employeeMockService.Verify(service => service.GetEmployee(email), Times.Once);
    }

    [Fact]
    public async Task GetUserIdReturnsNotFoundResult()
    {
        var email = "test@retrorabbit.co.za";

        _employeeMockService.Setup(service => service.GetEmployee(email))
                                 .ThrowsAsync(new
                                                  Exception("Error retrieving properties with access for the specified user."));

        var result = await _propertyAccessController.GetUserId(email);

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        var errorMessage = Assert.IsType<string>(notFoundResult.Value);
        Assert.Equal("Error retrieving properties with access for the specified user.", errorMessage);

        _employeeMockService.Verify(service => service.GetEmployee(email), Times.Once);
    }
}