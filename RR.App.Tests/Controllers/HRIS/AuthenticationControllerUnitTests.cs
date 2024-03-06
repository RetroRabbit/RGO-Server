using System.Security.Claims;
using HRIS.Models;
using HRIS.Models.Enums;
using HRIS.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RR.App.Controllers.HRIS;
using Xunit;

namespace RR.App.Tests.Controllers.HRIS;

public class AuthenticationControllerUnitTests
{
    [Fact]
    public async Task LoginUserValidInputReturnsOkResult()
    {
        var authServiceMock = new Mock<IAuthService>();
        var controller = new AuthenticationController(authServiceMock.Object);

        var email = "test@retrorabbit.co.za";
        var expectedToken = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

        authServiceMock.Setup(x => x.CheckUserExist(email)).ReturnsAsync(true);
        authServiceMock.Setup(x => x.Login(email)).ReturnsAsync(expectedToken);

        var result = await controller.LoginUser(email);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var actualToken = Assert.IsType<string>(okResult.Value);

        Assert.Equal(expectedToken, actualToken);

        authServiceMock.Verify(x => x.CheckUserExist(email), Times.Once);
        authServiceMock.Verify(x => x.Login(email), Times.Once);
    }

    [Fact]
    public async Task LoginUserUserNotFoundReturnsNotFoundResult()
    {
        var authServiceMock = new Mock<IAuthService>();
        var controller = new AuthenticationController(authServiceMock.Object);

        var userEmail = "nonexistent@retrorabbit.co.za";
        authServiceMock.Setup(x => x.CheckUserExist(userEmail)).ReturnsAsync(false);

        var result = await controller.LoginUser(userEmail);

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("User not found", notFoundResult.Value);
    }

    [Fact]
    public async Task RegisterEmployeeValidInputReturnsCreatedAtActionResult()
    {
        var authServiceMock = new Mock<IAuthService>();
        var controller = new AuthenticationController(authServiceMock.Object);

        var newEmployee = new EmployeeDto
        {
            Id = 1,
            EmployeeNumber = "Emp123",
            TaxNumber = "Tax123",
            EngagementDate = new DateTime(2022, 1, 1),
            TerminationDate = null,
            PeopleChampion = 1,
            Disability = false,
            DisabilityNotes = "No disability",
            Level = 2,
            EmployeeType = new EmployeeTypeDto { Id = 1, Name = "Full Time" },
            Notes = "Notes",
            LeaveInterval = 20.0f,
            SalaryDays = 15.0f,
            PayRate = 50.0f,
            Salary = 50000,
            Name = "John Doe",
            Initials = "JD",
            Surname = "Doe",
            DateOfBirth = new DateTime(1990, 1, 1),
            CountryOfBirth = "South Africa",
            Nationality = "South African",
            IdNumber = "123456789",
            PassportNumber = "AB123456",
            PassportExpirationDate = new DateTime(2025, 1, 1),
            PassportCountryIssue = "South Africa",
            Race = Race.White,
            Gender = Gender.Male,
            Photo = "photo.jpg",
            Email = "test@retrorabbit.co.za",
            PersonalEmail = "john.doe.personal@example.com",
            CellphoneNo = "1234567890",
            ClientAllocated = 1,
            TeamLead = 1,
            PhysicalAddress = new EmployeeAddressDto
            {
                Id = 1,
                UnitNumber = "Unit 1",
                ComplexName = "Complex A",
                StreetNumber = "123",
                SuburbOrDistrict = "Suburb",
                City = "City",
                Country = "Country",
                Province = "Province",
                PostalCode = "12345"
            },
            PostalAddress = new EmployeeAddressDto
            {
                Id = 2,
                UnitNumber = "P.O. Box 123",
                StreetNumber = "456",
                SuburbOrDistrict = "Suburb",
                City = "City",
                Country = "Country",
                Province = "Province",
                PostalCode = "54321"
            },
            HouseNo = "12",
            EmergencyContactName = "Emergency Contact",
            EmergencyContactNo = "987654321"
        };

        authServiceMock.Setup(x => x.CheckUserExist(newEmployee.Email!)).ReturnsAsync(false);
        authServiceMock.Setup(x => x.RegisterEmployee(newEmployee))
                       .ReturnsAsync("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789");

        var result = await controller.RegisterEmployee(newEmployee);

        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal(nameof(AuthenticationController.RegisterEmployee), createdAtActionResult.ActionName);
        Assert.Equal(newEmployee, createdAtActionResult.Value);
    }

    [Fact]
    public async Task RegisterEmployeeEmployeeAlreadyExistsReturnsNotFoundResult()
    {
        var authServiceMock = new Mock<IAuthService>();
        var controller = new AuthenticationController(authServiceMock.Object);

        var existingEmployee = new EmployeeDto
        {
            Id = 1,
            EmployeeNumber = "Emp123",
            TaxNumber = "Tax123",
            EngagementDate = new DateTime(2022, 1, 1),
            TerminationDate = null,
            PeopleChampion = 1,
            Disability = false,
            DisabilityNotes = "No disability",
            Level = 2,
            EmployeeType = new EmployeeTypeDto { Id = 1, Name = "Full Time" },
            Notes = "Notes",
            LeaveInterval = 20.0f,
            SalaryDays = 15.0f,
            PayRate = 50.0f,
            Salary = 50000,
            Name = "John Doe",
            Initials = "JD",
            Surname = "Doe",
            DateOfBirth = new DateTime(1990, 1, 1),
            CountryOfBirth = "South Africa",
            Nationality = "South African",
            IdNumber = "123456789",
            PassportNumber = "AB123456",
            PassportExpirationDate = new DateTime(2025, 1, 1),
            PassportCountryIssue = "South Africa",
            Race = Race.White,
            Gender = Gender.Male,
            Photo = "photo.jpg",
            Email = "test@retrorabbit.co.za",
            PersonalEmail = "john.doe.personal@example.com",
            CellphoneNo = "1234567890",
            ClientAllocated = 1,
            TeamLead = 1,
            PhysicalAddress = new EmployeeAddressDto
            {
                Id = 1,
                UnitNumber = "Unit 1",
                ComplexName = "Complex A",
                StreetNumber = "123",
                SuburbOrDistrict = "Suburb",
                City = "City",
                Country = "Country",
                Province = "Province",
                PostalCode = "12345"
            },
            PostalAddress = new EmployeeAddressDto
            {
                Id = 2,
                UnitNumber = "P.O. Box 123",
                StreetNumber = "456",
                SuburbOrDistrict = "Suburb",
                City = "City",
                Country = "Country",
                Province = "Province",
                PostalCode = "54321"
            },
            HouseNo = "12",
            EmergencyContactName = "Emergency Contact",
            EmergencyContactNo = "987654321"
        };

        authServiceMock.Setup(x => x.CheckUserExist(existingEmployee.Email!)).ReturnsAsync(true);

        var result = await controller.RegisterEmployee(existingEmployee);

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("Employee already exists", notFoundResult.Value);
    }

    [Fact]
    public async Task GetUserRolesSuccessReturnsOkResult()
    {
        var authServiceMock = new Mock<IAuthService>();
        var controller = new AuthenticationController(authServiceMock.Object);

        var httpContextMock = new Mock<HttpContext>();
        httpContextMock.SetupGet(c => c.User.Identity).Returns(new ClaimsIdentity(new List<Claim>
        {
            new(ClaimTypes.Email, "test@retrorabbit.co.za")
        }));

        controller.ControllerContext = new ControllerContext
        {
            HttpContext = httpContextMock.Object
        };

        authServiceMock.Setup(a => a.GetUserRoles(It.IsAny<string>()))
                       .ReturnsAsync(new Dictionary<string, List<string>>());

        var result = await controller.GetUserRoles(null);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var roles = Assert.IsType<Dictionary<string, List<string>>>(okResult.Value);

        authServiceMock.Verify(a => a.GetUserRoles("test@retrorabbit.co.za"), Times.Once);
    }

    [Fact]
    public async Task GetUserRoles_ReturnsOkResult_WithEmail()
    {
        var authServiceMock = new Mock<IAuthService>();
        var controller = new AuthenticationController(authServiceMock.Object);

        var httpContextMock = new Mock<HttpContext>();
        httpContextMock.SetupGet(c => c.User.Identity).Returns(new ClaimsIdentity(new List<Claim>
        {
            new(ClaimTypes.Email, "test@retrorabbit.co.za")
        }));

        controller.ControllerContext = new ControllerContext
        {
            HttpContext = httpContextMock.Object
        };

        authServiceMock.Setup(a => a.GetUserRoles("test@retrorabbit.co.za"))
                       .ReturnsAsync(new Dictionary<string, List<string>>());

        var result = await controller.GetUserRoles(null);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var roles = Assert.IsType<Dictionary<string, List<string>>>(okResult.Value);

        authServiceMock.Verify(a => a.GetUserRoles("test@retrorabbit.co.za"), Times.Once);
    }

    [Fact]
    public async Task GetUserRoles_ReturnsOkResult_WithoutEmail()
    {
        var authServiceMock = new Mock<IAuthService>();
        var controller = new AuthenticationController(authServiceMock.Object);

        var httpContextMock = new Mock<HttpContext>();
        httpContextMock.SetupGet(c => c.User.Identity).Returns(new ClaimsIdentity(new List<Claim>
        {
            new(ClaimTypes.Email, "test@retrorabbit.co.za")
        }));

        controller.ControllerContext = new ControllerContext
        {
            HttpContext = httpContextMock.Object
        };

        authServiceMock.Setup(a => a.GetUserRoles("test@retrorabbit.co.za"))
                       .ReturnsAsync(new Dictionary<string, List<string>>());

        var result = await controller.GetUserRoles(null);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var roles = Assert.IsType<Dictionary<string, List<string>>>(okResult.Value);

        authServiceMock.Verify(a => a.GetUserRoles("test@retrorabbit.co.za"), Times.Once);
    }


    [Fact]
    public async Task GetUserRolesReturnsNotFoundResult()
    {
        var authServiceMock = new Mock<IAuthService>();
        var controller = new AuthenticationController(authServiceMock.Object);

        var claims = new List<Claim>
        {
            new(ClaimTypes.Email, "test@retrorabbit.co.za")
        };
        var identity = new ClaimsIdentity(claims, "TestAuthentication");
        var claimsPrincipal = new ClaimsPrincipal(identity);

        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = claimsPrincipal }
        };

        authServiceMock.Setup(x => x.GetUserRoles(It.IsAny<string>()))
                       .ThrowsAsync(new Exception("Exception occurred while getting user roles"));

        var result = await controller.GetUserRoles("test@retrorabbit.co.za");

        Assert.IsType<NotFoundObjectResult>(result);

        if (result is NotFoundObjectResult notFoundResult)
        {
            Assert.NotNull(notFoundResult.Value);
            Assert.IsType<string>(notFoundResult.Value);

            var actualErrorMessage = notFoundResult.Value.ToString();

            Assert.Equal("Exception occurred while getting user roles", actualErrorMessage);
        }
    }
}
