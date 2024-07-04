using HRIS.Models;
using HRIS.Models.Enums;
using HRIS.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RR.App.Controllers.HRIS;
using Xunit;
using HRIS.Services.Services;
using Microsoft.AspNetCore.Http;
using HRIS.Services.Session;
using RR.App.Tests.Helper;

namespace RR.App.Tests.Controllers.HRIS;

public class PropertyAccessControllerUnitTests
{
    private readonly Mock<IPropertyAccessService> _propertyAccessMockService;
    private readonly Mock<IEmployeeService> _employeeMockService;
    private readonly PropertyAccessController _propertyAccessController;
    private readonly Mock<AuthorizeIdentity> _authorizeIdentityMock;

    public PropertyAccessControllerUnitTests()
    {
        _authorizeIdentityMock = new Mock<AuthorizeIdentity>(MockBehavior.Strict, null, null);
        _propertyAccessMockService = new Mock<IPropertyAccessService>();
        _employeeMockService = new Mock<IEmployeeService>();
        _propertyAccessController = new PropertyAccessController(_authorizeIdentityMock.Object, _propertyAccessMockService.Object, _employeeMockService.Object);
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
    public async Task GetAllPropertyAccessNotFoundResult()
    {
        _propertyAccessMockService.Setup(service => service.GetAll())
                                  .ThrowsAsync(new CustomException("Error retrieving properties with access for the specified user."));

        var result = await MiddlewareHelperUnitTests.SimulateHandlingExceptionMiddlewareAsync(async () => await _propertyAccessController.GetAllPropertyAccess());

        var objectResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal(StatusCodes.Status404NotFound, objectResult.StatusCode);
        Assert.Equal("Error retrieving properties with access for the specified user.", objectResult.Value);
    }

    [Fact]
    public async Task GetPropertiesWithAccessReturnsOkResult()
    {
        var employeeId = 1;
        _authorizeIdentityMock.SetupGet(x => x.Role).Returns("Admin");
        _authorizeIdentityMock.SetupGet(x => x.EmployeeId).Returns(employeeId);

        var controller = new PropertyAccessController(_authorizeIdentityMock.Object, _propertyAccessMockService.Object, _employeeMockService.Object);

        _propertyAccessMockService.Setup(service => service.GetAccessListByEmployeeId(employeeId))
                                  .ReturnsAsync(new List<PropertyAccessDto>());

        var result = await controller.GetPropertiesWithAccess(employeeId);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var model = Assert.IsAssignableFrom<List<PropertyAccessDto>>(okResult.Value);
        _propertyAccessMockService.Verify(service => service.GetAccessListByEmployeeId(employeeId), Times.Once);
    }

    [Fact]
    public async Task GetPropertiesWithAccessRoleOrEmployeeIdConditionFails()
    {
        var employeeId = 2;
        _authorizeIdentityMock.SetupGet(x => x.Role).Returns("Inactive");
        _authorizeIdentityMock.SetupGet(x => x.EmployeeId).Returns(1);

        var controller = new PropertyAccessController(_authorizeIdentityMock.Object, _propertyAccessMockService.Object, _employeeMockService.Object);

        var result = await MiddlewareHelperUnitTests.SimulateHandlingExceptionMiddlewareAsync(async () => await controller.GetPropertiesWithAccess(employeeId));

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
        Assert.Equal("Error retrieving employee.", notFoundResult.Value);

        _propertyAccessMockService.Verify(service => service.GetAccessListByEmployeeId(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task GetPropertiesWithAccessServiceThrowsException()
    {
        var employeeId = 2;
        _authorizeIdentityMock.SetupGet(x => x.Role).Returns("Admin");
        _authorizeIdentityMock.SetupGet(x => x.EmployeeId).Returns(employeeId);

        var controller = new PropertyAccessController(_authorizeIdentityMock.Object, _propertyAccessMockService.Object, _employeeMockService.Object);

        _propertyAccessMockService.Setup(service => service.GetAccessListByEmployeeId(employeeId))
                                  .ThrowsAsync(new CustomException("Error retrieving employee."));

        var result = await MiddlewareHelperUnitTests.SimulateHandlingExceptionMiddlewareAsync(async () => await controller.GetPropertiesWithAccess(employeeId));

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
        Assert.Equal("Error retrieving employee.", notFoundResult.Value);

        _propertyAccessMockService.Verify(service => service.GetAccessListByEmployeeId(employeeId), Times.Once);
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
                                  .ThrowsAsync(new CustomException("Error creating property access entries."));

        var result = await MiddlewareHelperUnitTests.SimulateHandlingExceptionMiddlewareAsync(async () => await _propertyAccessController.Seed());

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
        Assert.Equal("Error creating property access entries.", notFoundResult.Value);
    }

    [Fact]
    public async Task UpdatePropertyRoleAccessReturnsSuccess()
    {
        _propertyAccessMockService.Setup(service => service.UpdatePropertyAccess(0, PropertyAccessLevel.read))
                                  .Returns(Task.CompletedTask);

        var result = await _propertyAccessController.UpdatePropertyRoleAccess(0, PropertyAccessLevel.read);

        Assert.IsType<OkResult>(result);

        _propertyAccessMockService.Verify(service => service.UpdatePropertyAccess(0, PropertyAccessLevel.read), Times.Once);
    }

    [Fact]
    public async Task UpdatePropertyRoleAccessReturnsFail()
    {
        _propertyAccessMockService.Setup(service => service.UpdatePropertyAccess(0, PropertyAccessLevel.read))
                                  .ThrowsAsync(new CustomException("Error updating property access."));

        var result = await MiddlewareHelperUnitTests.SimulateHandlingExceptionMiddlewareAsync(async () => await _propertyAccessController.UpdatePropertyRoleAccess(0, PropertyAccessLevel.read));

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
        Assert.Equal("Error updating property access.", notFoundResult.Value);

        _propertyAccessMockService.Verify(service => service.UpdatePropertyAccess(0, PropertyAccessLevel.read), Times.Once);
    }

    [Fact]
    public async Task GetUserIdReturnsEmployeeIdForMatchingEmail()
    {
        var email = "test@example.com";
        _authorizeIdentityMock.SetupGet(x => x.Email).Returns(email);
        _authorizeIdentityMock.SetupGet(x => x.EmployeeId).Returns(1);

        var controller = new PropertyAccessController(_authorizeIdentityMock.Object, _propertyAccessMockService.Object, _employeeMockService.Object);
        var result = await controller.GetUserId(email);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var employeeId = Assert.IsType<int>(okResult.Value);
        Assert.Equal(1, employeeId);
    }

    [Fact]
    public async Task GetUserIdReturnsEmployeeIdForSuperAdminOrAdmin()
    {
        var email = "test@example.com";
        var employeeId = 2;
        _authorizeIdentityMock.SetupGet(x => x.Email).Returns("admin@example.com");
        _authorizeIdentityMock.SetupGet(x => x.Role).Returns("Admin");

        _employeeMockService.Setup(service => service.GetEmployee(email))
                            .ReturnsAsync(new EmployeeDto { Id = employeeId });

        var controller = new PropertyAccessController(_authorizeIdentityMock.Object, _propertyAccessMockService.Object, _employeeMockService.Object);
        var result = await controller.GetUserId(email);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedEmployeeId = Assert.IsType<int>(okResult.Value);
        Assert.Equal(employeeId, returnedEmployeeId);

        _employeeMockService.Verify(service => service.GetEmployee(email), Times.Once);
    }

    [Fact]
    public async Task GetUserIdThrowsExceptionForUnauthorizedAccess()
    {
        var email = "test@example.com";
        _authorizeIdentityMock.SetupGet(x => x.Email).Returns("another@example.com");
        _authorizeIdentityMock.SetupGet(x => x.Role).Returns("User");

        var controller = new PropertyAccessController(_authorizeIdentityMock.Object, _propertyAccessMockService.Object, _employeeMockService.Object);
        var result = await MiddlewareHelperUnitTests.SimulateHandlingExceptionMiddlewareAsync(async () => await controller.GetUserId(email));

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
        Assert.Equal("Error retrieving employee.", notFoundResult.Value);
    }

    [Fact]
    public async Task GetPropertiesWithAccessThrowsExceptionForUnauthorizedRole()
    {
        var employeeId = 2;
        _authorizeIdentityMock.SetupGet(x => x.Role).Returns("Inactive");
        _authorizeIdentityMock.SetupGet(x => x.EmployeeId).Returns(1);

        var controller = new PropertyAccessController(_authorizeIdentityMock.Object, _propertyAccessMockService.Object, _employeeMockService.Object);

        var result = await MiddlewareHelperUnitTests.SimulateHandlingExceptionMiddlewareAsync(async () => await controller.GetPropertiesWithAccess(employeeId));

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
        Assert.Equal("Error retrieving employee.", notFoundResult.Value);

        _propertyAccessMockService.Verify(service => service.GetAccessListByEmployeeId(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task GetPropertiesWithAccessThrowsExceptionForUnauthorizedRoleNoEmployeeIdMatch()
    {
        var employeeId = 2;
        _authorizeIdentityMock.SetupGet(x => x.Role).Returns("Inactive");
        _authorizeIdentityMock.SetupGet(x => x.EmployeeId).Returns(1);

        var controller = new PropertyAccessController(_authorizeIdentityMock.Object, _propertyAccessMockService.Object, _employeeMockService.Object);

        var result = await MiddlewareHelperUnitTests.SimulateHandlingExceptionMiddlewareAsync(async () => await controller.GetPropertiesWithAccess(employeeId));

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
        Assert.Equal("Error retrieving employee.", notFoundResult.Value);

        _propertyAccessMockService.Verify(service => service.GetAccessListByEmployeeId(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task GetUserIdThrowsExceptionForEmptyEmail()
    {
        var email = "";
        _authorizeIdentityMock.SetupGet(x => x.Email).Returns(email);

        var controller = new PropertyAccessController(_authorizeIdentityMock.Object, _propertyAccessMockService.Object, _employeeMockService.Object);

        var result = await Assert.ThrowsAsync<CustomException>(() => controller.GetUserId(email));
        Assert.Equal("Error retrieving employee.", result.Message);
    }
}
