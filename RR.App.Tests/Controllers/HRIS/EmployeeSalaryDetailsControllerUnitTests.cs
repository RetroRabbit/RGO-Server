using HRIS.Models;
using HRIS.Services.Interfaces;
using HRIS.Services.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RR.App.Controllers.HRIS;
using RR.Tests.Data;
using RR.Tests.Data.Models.HRIS;
using Xunit;

namespace RR.App.Tests.Controllers.HRIS;

public class EmployeeSalaryDetailsControllerUnitTest
{
    private readonly Mock<IEmployeeSalaryDetailsService> _employeeSalaryDetailsServiceMock;
    private readonly EmployeeSalaryDetailsController _controller;
    private readonly Mock<IEmployeeService> _employeeServiceMock;
    
    private readonly EmployeeSalaryDetailsDto _employeeSalaryDetailsDto;
    private readonly EmployeeDto _employeeDto;

    public EmployeeSalaryDetailsControllerUnitTest()
    {
        _employeeSalaryDetailsServiceMock = new Mock<IEmployeeSalaryDetailsService>();
        _employeeServiceMock = new Mock<IEmployeeService>();
        _controller = new EmployeeSalaryDetailsController(new AuthorizeIdentityMock("test@example.com", "TestUser", "SuperAdmin", 1), _employeeSalaryDetailsServiceMock.Object);
        _employeeSalaryDetailsDto = EmployeeSalaryDetailsTestData.EmployeeSalaryDetailsOne.ToDto();
        _employeeDto = EmployeeTestData.EmployeeOne.ToDto();

    }

    [Fact]
    public async Task SaveEmployeeSalaryValidInputReturnsOkResult()
    {
        // Mock service to return the expected employee DTO
        _employeeServiceMock.Setup(x => x.GetEmployeeById(_employeeSalaryDetailsDto.EmployeeId))
                            .ReturnsAsync(_employeeDto);

        // Mock the SaveEmployeeSalary method to return the test salary details DTO
        _employeeSalaryDetailsServiceMock.Setup(x => x.SaveEmployeeSalary(It.IsAny<EmployeeSalaryDetailsDto>()))
                                         .ReturnsAsync(_employeeSalaryDetailsDto);

        // Call the AddEmployeeSalary method on the controller
        var result = await _controller.AddEmployeeSalary(_employeeSalaryDetailsDto);

        // Verify the result is a CreatedAtActionResult
        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal("AddEmployeeSalary", createdAtActionResult.ActionName); // Directly specify the action name as string
        Assert.Equal(_employeeSalaryDetailsDto.EmployeeId, createdAtActionResult.RouteValues["employeeId"]);
        Assert.Equal(_employeeSalaryDetailsDto, createdAtActionResult.Value);
    }

    [Fact]
    public async Task AddEmployeeSalary_UnauthorizedRoleOrIdMismatch_ThrowsCustomException()
    {
        // Arrange: Create a mock identity with a role that doesn't have permissions and a mismatched employee ID
        var unauthorizedIdentity = new AuthorizeIdentityMock("unauthorized@example.com", "UnauthorizedUser", "User", 2);
        var controller = new EmployeeSalaryDetailsController(unauthorizedIdentity, _employeeSalaryDetailsServiceMock.Object);

        // Act & Assert: Expect a CustomException when the AddEmployeeSalary method is called
        var exception = await Assert.ThrowsAsync<CustomException>(() => controller.AddEmployeeSalary(_employeeSalaryDetailsDto));

        Assert.Equal("User data being accessed does not match user making the request.", exception.Message);
    }

    [Fact]
    public async Task AddEmployeeSalary_ValidRoleAndMatchingId_ReturnsCreatedAtActionResult()
    {
        // Arrange: Setup the mock service to return the expected DTO
        _employeeSalaryDetailsServiceMock.Setup(x => x.SaveEmployeeSalary(It.IsAny<EmployeeSalaryDetailsDto>()))
                                         .ReturnsAsync(_employeeSalaryDetailsDto);

        // Act: Call the AddEmployeeSalary method
        var result = await _controller.AddEmployeeSalary(_employeeSalaryDetailsDto);

        // Assert: Verify the result is a CreatedAtActionResult with expected values
        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal("AddEmployeeSalary", createdAtActionResult.ActionName);
        Assert.Equal(_employeeSalaryDetailsDto.EmployeeId, createdAtActionResult.RouteValues["employeeId"]);
        Assert.Equal(_employeeSalaryDetailsDto, createdAtActionResult.Value);
    }

    [Fact]
    public async Task DeleteEmployeeSalaryValidInputReturnsOkResult()
    {
        _employeeServiceMock.Setup(x => x.GetEmployeeById(_employeeSalaryDetailsDto.EmployeeId))
                           .ReturnsAsync(_employeeDto);

        _employeeSalaryDetailsServiceMock.Setup(x => x.DeleteEmployeeSalary(_employeeSalaryDetailsDto.Id))
                                        .ReturnsAsync(_employeeSalaryDetailsDto);

        var result = await _controller.DeleteSalary(_employeeSalaryDetailsDto.Id);

        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task UpdateEmployeeSalaryValidInputReturnsOkResult()
    {
        // Mock the GetEmployeeById to return the employee DTO
        _employeeServiceMock.Setup(x => x.GetEmployeeById(_employeeSalaryDetailsDto.EmployeeId))
                            .ReturnsAsync(_employeeDto);

        // Mock the UpdateEmployeeSalary method to return the test salary details DTO
        _employeeSalaryDetailsServiceMock.Setup(x => x.UpdateEmployeeSalary(_employeeSalaryDetailsDto))
                                         .ReturnsAsync(_employeeSalaryDetailsDto);

        // Call the UpdateSalary method on the controller
        var result = await _controller.UpdateSalary(_employeeSalaryDetailsDto);

        // Verify the result is an OkObjectResult
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task UpdateEmployeeSalary_UnauthorizedRoleOrIdMismatch_ThrowsCustomException()
    {
        // Arrange: Create a mock identity with a role that doesn't have permissions and a mismatched employee ID
        var unauthorizedIdentity = new AuthorizeIdentityMock("unauthorized@example.com", "UnauthorizedUser", "User", 2);
        var controller = new EmployeeSalaryDetailsController(unauthorizedIdentity, _employeeSalaryDetailsServiceMock.Object);

        // Act & Assert: Expect a CustomException when the UpdateSalary method is called
        var exception = await Assert.ThrowsAsync<CustomException>(() => controller.UpdateSalary(_employeeSalaryDetailsDto));

        Assert.Equal("User data being accessed does not match user making the request.", exception.Message);
    }

    [Fact]
    public async Task GetSalariesByEmployeePass()
    {
        // Mock the GetEmployeeSalary method to return the test salary details DTO
        _employeeSalaryDetailsServiceMock.Setup(x => x.GetEmployeeSalary(_employeeSalaryDetailsDto.EmployeeId))
                                         .ReturnsAsync(_employeeSalaryDetailsDto);

        // Call the GetEmployeeSalary method on the controller
        var result = await _controller.GetEmployeeSalary(_employeeSalaryDetailsDto.EmployeeId);

        // Verify the result is an OkObjectResult
        var okResult = Assert.IsType<OkObjectResult>(result);
        var actualSalaryDetailsDto = Assert.IsType<EmployeeSalaryDetailsDto>(okResult.Value);
        Assert.Equal(_employeeSalaryDetailsDto, actualSalaryDetailsDto);
    }

    [Fact]
    public async Task GetEmployeeSalary_ValidRoleAndMatchingId_ReturnsOkObjectResult()
    {
        // Arrange: Setup the mock service to return the expected DTO
        _employeeSalaryDetailsServiceMock.Setup(x => x.GetEmployeeSalary(_employeeSalaryDetailsDto.EmployeeId))
                                         .ReturnsAsync(_employeeSalaryDetailsDto);

        // Act: Call the GetEmployeeSalary method
        var result = await _controller.GetEmployeeSalary(_employeeSalaryDetailsDto.EmployeeId);

        // Assert: Verify the result is an OkObjectResult with expected value
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(_employeeSalaryDetailsDto, okResult.Value);
    }

    [Fact]
    public async Task GetEmployeeSalary_UnauthorizedRoleOrIdMismatch_ThrowsCustomException()
    {
        // Arrange: Create a mock identity with a role that doesn't have permissions and a mismatched employee ID
        var unauthorizedIdentity = new AuthorizeIdentityMock("unauthorized@example.com", "UnauthorizedUser", "User", 2);
        var controller = new EmployeeSalaryDetailsController(unauthorizedIdentity, _employeeSalaryDetailsServiceMock.Object);

        // Act & Assert: Expect a CustomException when the GetEmployeeSalary method is called
        var exception = await Assert.ThrowsAsync<CustomException>(() => controller.GetEmployeeSalary(_employeeSalaryDetailsDto.EmployeeId));

        Assert.Equal("User data being accessed does not match user making the request.", exception.Message);
    }

    [Fact]
    public async Task GetAllEmployeeSalariesNoFiltersReturnsOkResultWithList()
    {
        // Prepare the expected list of employee salary details DTOs
        var expectedSalaryDetailsList = new List<EmployeeSalaryDetailsDto>
    {
        _employeeSalaryDetailsDto,
        _employeeSalaryDetailsDto
    };

        // Mock the GetAllEmployeeSalaries method to return the expected list
        _employeeSalaryDetailsServiceMock.Setup(x => x.GetAllEmployeeSalaries())
                                         .ReturnsAsync(expectedSalaryDetailsList);

        // Call the GetAllEmployeeSalaries method on the controller
        var result = await _controller.GetAllEmployeeSalaries();

        // Verify the result is an OkObjectResult
        var okResult = Assert.IsType<OkObjectResult>(result);
        var actualSalaryDetailsDto = Assert.IsType<List<EmployeeSalaryDetailsDto>>(okResult.Value);

        // Verify the actual list matches the expected list
        Assert.Equal(expectedSalaryDetailsList, actualSalaryDetailsDto);
    }



}