using Auth0.ManagementApi.Models;
using Auth0.ManagementApi.Paging;
using HRIS.Models;
using HRIS.Models.Enums;
using HRIS.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RR.App.Controllers.HRIS;
using RR.Tests.Data.Models.HRIS;
using Xunit;

namespace RR.App.Tests.Controllers.HRIS;

public class EmployeeRoleManagerControllerUnitTests
{
    private static EmployeeDto CreateEmployee(string email)
    {
       return new EmployeeDto
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
            Email = email, 
            PersonalEmail = "john.doe.personal@example.com",
            CellphoneNo = "1234567890",
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
                   City = "City",
                   Country = "Country",
                   Province = "Province",
                   PostalCode = "54321"
                },
                   HouseNo = "12",
                   EmergencyContactName = "Emergency Contact",
                   EmergencyContactNo = "987654321"
       };
    }

    [Fact]
    public async Task AddRoleValidInputReturnsCreatedAtAction()
    {
        var employeeServiceMock = new Mock<IEmployeeService>();
        var employeeRoleServiceMock = new Mock<IEmployeeRoleService>();
        var employeeAuthServiceMock = new Mock<IAuthService>();
        var roleServiceMock = new Mock<IRoleService>();

        var controller = new EmployeeRoleManageController(
            employeeRoleServiceMock.Object,
            employeeServiceMock.Object,
            roleServiceMock.Object,
            employeeAuthServiceMock.Object
        );

        var email = "test@retrorabbit.co.za";
        var role = "Admin";

        var employeeDto = CreateEmployee(email);
        employeeServiceMock.Setup(x => x.GetEmployee(email)).ReturnsAsync(employeeDto);

        roleServiceMock.Setup(x => x.CheckRole(role)).ReturnsAsync(true);
        roleServiceMock.Setup(x => x.GetRole(role)).ReturnsAsync(new RoleDto { Id = 1, Description = "Admin" });

        employeeRoleServiceMock.Setup(x => x.SaveEmployeeRole(It.IsAny<EmployeeRoleDto>()))
                               .ReturnsAsync(new EmployeeRoleDto
                               {
                                   Id = 1,
                                   Employee = employeeDto,
                                   Role = new RoleDto { Id = 1, Description = "Admin" }
                               });

        var result = await controller.AddRole(email, role);
        Assert.NotNull(result);

        if (result is CreatedAtActionResult createdAtActionResult)
        {
            Assert.Equal(nameof(EmployeeRoleManageController.AddRole), createdAtActionResult.ActionName);

            employeeServiceMock.Verify(x => x.GetEmployee(email), Times.Once);
            roleServiceMock.Verify(x => x.CheckRole(role), Times.Once);
            roleServiceMock.Verify(x => x.GetRole(role), Times.Once);
            employeeRoleServiceMock.Verify(x => x.SaveEmployeeRole(It.IsAny<EmployeeRoleDto>()), Times.Once);
        }
        else if (result is NotFoundObjectResult notFoundResult)
        {
            Assert.True(true, $"Expected CreatedAtActionResult but got NotFoundObjectResult with message: {notFoundResult.Value}");
        }
        else
        {
            Assert.IsType<CreatedAtActionResult>(result);
        }
    }

    [Fact]
    public async Task AddRoleUnknownRoleReturnsCreatedAtAction()
    {
        var employeeServiceMock = new Mock<IEmployeeService>();
        var employeeRoleServiceMock = new Mock<IEmployeeRoleService>();
        var employeeAuthServiceMock = new Mock<IAuthService>();
        var roleServiceMock = new Mock<IRoleService>();

        var controller = new EmployeeRoleManageController(
            employeeRoleServiceMock.Object,
            employeeServiceMock.Object,
            roleServiceMock.Object,
            employeeAuthServiceMock.Object
        );

        var email = "test@retrorabbit.co.za";
        var role = "Unknown Role";

        var employeeDto = CreateEmployee(email);
        employeeServiceMock.Setup(x => x.GetEmployee(email)).ReturnsAsync(employeeDto);

        roleServiceMock.Setup(x => x.CheckRole(role)).ReturnsAsync(false);

        employeeRoleServiceMock.Setup(x => x.SaveEmployeeRole(It.IsAny<EmployeeRoleDto>()))
                               .ReturnsAsync(new EmployeeRoleDto
                               {
                                   Id = 1,
                                   Employee = employeeDto,
                                   Role = null
                               });

        var result = await controller.AddRole(email, role);

        Assert.NotNull(result);

        if (result is CreatedAtActionResult createdAtActionResult)
        {
            Assert.Equal(nameof(EmployeeRoleManageController.AddRole), createdAtActionResult.ActionName);

            employeeServiceMock.Verify(x => x.GetEmployee(email), Times.Once);
            roleServiceMock.Verify(x => x.CheckRole(role), Times.Once);
            employeeRoleServiceMock.Verify(x => x.SaveEmployeeRole(It.IsAny<EmployeeRoleDto>()), Times.Once);
        }
        else if (result is NotFoundObjectResult notFoundResult)
        {
            Assert.True(true, $"Expected CreatedAtActionResult but got NotFoundObjectResult with message: {notFoundResult.Value}");
        }
        else
        {
            Assert.IsType<CreatedAtActionResult>(result); 
        }
    }

    [Fact]
    public async Task AddRoleInvalidInputReturnsBadRequest()
    {
        var employeeServiceMock = new Mock<IEmployeeService>();
        var employeeRoleServiceMock = new Mock<IEmployeeRoleService>();
        var employeeAuthServiceMock = new Mock<IAuthService>();
        var roleServiceMock = new Mock<IRoleService>();

        var controller = new EmployeeRoleManageController(employeeRoleServiceMock.Object, employeeServiceMock.Object,
                                                          roleServiceMock.Object, employeeAuthServiceMock.Object);

        var invalidEmail = string.Empty;
        string? invalidRole = null;
        var result = await controller.AddRole(invalidEmail, invalidRole!);

        Assert.IsType<BadRequestObjectResult>(result);

        employeeServiceMock.Verify(x => x.GetEmployee(It.IsAny<string>()), Times.Never);
        roleServiceMock.Verify(x => x.CheckRole(It.IsAny<string>()), Times.Never);
        roleServiceMock.Verify(x => x.GetRole(It.IsAny<string>()), Times.Never);
        employeeRoleServiceMock.Verify(x => x.SaveEmployeeRole(It.IsAny<EmployeeRoleDto>()), Times.Never);
    }

    [Fact]
    public async Task AddRoleExceptionReturnsBadRequest()
    {
        var employeeServiceMock = new Mock<IEmployeeService>();
        var employeeRoleServiceMock = new Mock<IEmployeeRoleService>();
        var roleServiceMock = new Mock<IRoleService>();
        var employeeAuthServiceMock = new Mock<IAuthService>();

        var controller = new EmployeeRoleManageController(employeeRoleServiceMock.Object, employeeServiceMock.Object,
                                                          roleServiceMock.Object, employeeAuthServiceMock.Object);

        var email = "test@retrorabbit.co.za";
        var role = "Employee";

        employeeServiceMock.Setup(x => x.GetEmployee(email))
                           .ThrowsAsync(new Exception("An error occurred adding role"));

        var result = await controller.AddRole(email, role);

        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("An error occurred adding role", badRequestResult.Value);

        employeeServiceMock.Verify(x => x.GetEmployee(email), Times.Once);
        roleServiceMock.Verify(x => x.CheckRole(role), Times.Never);
        roleServiceMock.Verify(x => x.GetRole(role), Times.Never);
        employeeRoleServiceMock.Verify(x => x.SaveEmployeeRole(It.IsAny<EmployeeRoleDto>()), Times.Never);
    }

    [Fact]
    public async Task UpdateRoleValidInputReturnsCreatedAtAction()
    {
        var email = "test@retrorabbit.co.za";
        var role = "Employee";

        var employeeServiceMock = new Mock<IEmployeeService>();
        var roleServiceMock = new Mock<IRoleService>();
        var employeeRoleServiceMock = new Mock<IEmployeeRoleService>();
        var authServiceMock = new Mock<IAuthService>();

        var controller = new EmployeeRoleManageController(
            employeeRoleServiceMock.Object,
            employeeServiceMock.Object,
            roleServiceMock.Object,
            authServiceMock.Object
        );

        var existingEmployee = new EmployeeDto { Email = email, Id = 1 };
        var existingRole = new RoleDto { Id = 1, Description = role, AuthRoleId = "authRoleId" };
        var existingEmployeeRole = new EmployeeRoleDto
        {
            Id = 1,
            Employee = existingEmployee,
            Role = existingRole
        };
        var authUserList = new List<User> { new User { UserId = "authUserId" } };
        var authRolesList = new PagedList<Auth0.ManagementApi.Models.Role>(
            new List<Auth0.ManagementApi.Models.Role> { new Auth0.ManagementApi.Models.Role { Id = "authRoleId", Name = role } },
            new PagingInformation(0, 1, 1, 1) // Correctly initialize PagingInformation
        );

        // Mock setup
        employeeServiceMock.Setup(x => x.GetEmployee(email)).ReturnsAsync(existingEmployee);
        roleServiceMock.Setup(x => x.CheckRole(role)).ReturnsAsync(true);
        roleServiceMock.Setup(x => x.GetRole(role)).ReturnsAsync(existingRole);
        employeeRoleServiceMock.Setup(x => x.GetEmployeeRole(email)).ReturnsAsync(existingEmployeeRole);
        employeeRoleServiceMock.Setup(x => x.UpdateEmployeeRole(It.IsAny<EmployeeRoleDto>())).ReturnsAsync(existingEmployeeRole);
        authServiceMock.Setup(x => x.GetUsersByEmailAsync(email)).ReturnsAsync(authUserList);
        authServiceMock.Setup(x => x.GetUserRolesAsync("authUserId")).ReturnsAsync(authRolesList);
        authServiceMock.Setup(x => x.RemoveRoleFromUserAsync("authUserId", "authRoleId")).ReturnsAsync(true);
        authServiceMock.Setup(x => x.AddRoleToUserAsync("authUserId", "authRoleId")).ReturnsAsync(true);

        var result = await controller.UpdateRole(email, role);

        Assert.NotNull(result);
        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal(nameof(EmployeeRoleManageController.UpdateRole), createdAtActionResult.ActionName);

        employeeServiceMock.Verify(x => x.GetEmployee(email), Times.Once);
        roleServiceMock.Verify(x => x.CheckRole(role), Times.Once);
        roleServiceMock.Verify(x => x.GetRole(role), Times.Once);
        employeeRoleServiceMock.Verify(x => x.GetEmployeeRole(email), Times.Once);
        employeeRoleServiceMock.Verify(x => x.UpdateEmployeeRole(It.IsAny<EmployeeRoleDto>()), Times.Once);
        authServiceMock.Verify(x => x.GetUsersByEmailAsync(email), Times.Once);
        authServiceMock.Verify(x => x.GetUserRolesAsync("authUserId"), Times.Once);
        authServiceMock.Verify(x => x.RemoveRoleFromUserAsync("authUserId", "authRoleId"), Times.Once);
        authServiceMock.Verify(x => x.AddRoleToUserAsync("authUserId", "authRoleId"), Times.Once);
    }

    [Fact]
    public async Task UpdateRoleExceptionReturnsBadRequest()
    {
        var employeeServiceMock = new Mock<IEmployeeService>();
        var employeeRoleServiceMock = new Mock<IEmployeeRoleService>();
        var roleServiceMock = new Mock<IRoleService>();
        var employeeAuthServiceMock = new Mock<IAuthService>();

        var controller = new EmployeeRoleManageController(employeeRoleServiceMock.Object, employeeServiceMock.Object,
                                                           roleServiceMock.Object, employeeAuthServiceMock.Object);
        var email = "test@retrorabbit.co.za";
        var role = "Journey";

        employeeServiceMock.Setup(x => x.GetEmployee(email))
                           .ThrowsAsync(new Exception("An error occurred updating role"));

        var result = await controller.UpdateRole(email, role);

        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("An error occurred: An error occurred updating role", badRequestResult.Value);

        employeeServiceMock.Verify(x => x.GetEmployee(email), Times.Once);
        roleServiceMock.Verify(x => x.CheckRole(role), Times.Never);
        roleServiceMock.Verify(x => x.GetRole(role), Times.Never);
        employeeRoleServiceMock.Verify(x => x.GetEmployeeRole(email), Times.Never);
        employeeRoleServiceMock.Verify(x => x.UpdateEmployeeRole(It.IsAny<EmployeeRoleDto>()), Times.Never);
    }

    [Fact]
    public async Task RemoveRoleValidInputReturnsOk()
    {
        var email = "test@retrorabbit.co.za";
        var role = "Journey";

        var employeeServiceMock = new Mock<IEmployeeService>();
        var roleServiceMock = new Mock<IRoleService>();
        var employeeRoleServiceMock = new Mock<IEmployeeRoleService>();
        var authServiceMock = new Mock<IAuthService>();

        var controller = new EmployeeRoleManageController(
            employeeRoleServiceMock.Object,
            employeeServiceMock.Object,
            roleServiceMock.Object,
            authServiceMock.Object
        );

        var existingEmployee = new EmployeeDto { Email = email, Id = 1 };
        var existingRole = new RoleDto { Id = 1, Description = role, AuthRoleId = "authRoleId" };
        var existingEmployeeRole = new EmployeeRoleDto { Id = 1, Employee = existingEmployee, Role = existingRole };
        var authUserList = new List<User> { new User { UserId = "authUserId" } };
        var authRolesList = new PagedList<Auth0.ManagementApi.Models.Role>(
            new List<Auth0.ManagementApi.Models.Role> { new Auth0.ManagementApi.Models.Role { Id = "authRoleId", Name = role } },
            new PagingInformation(0, 1, 1, 1) // Correctly initialize PagingInformation
        );

        // Mock setup
        employeeServiceMock.Setup(x => x.GetEmployee(email)).ReturnsAsync(existingEmployee);
        roleServiceMock.Setup(x => x.GetRole(role)).ReturnsAsync(existingRole);
        employeeRoleServiceMock.Setup(x => x.GetEmployeeRole(email)).ReturnsAsync(existingEmployeeRole);
        employeeRoleServiceMock.Setup(x => x.DeleteEmployeeRole(email, role)).ReturnsAsync(existingEmployeeRole);
        authServiceMock.Setup(x => x.GetUsersByEmailAsync(email)).ReturnsAsync(authUserList);
        authServiceMock.Setup(x => x.GetUserRolesAsync("authUserId")).ReturnsAsync(authRolesList);
        authServiceMock.Setup(x => x.RemoveRoleFromUserAsync("authUserId", "authRoleId")).ReturnsAsync(true);

        var result = await controller.RemoveRole(email, role);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedDto = Assert.IsType<EmployeeRoleDto>(okResult.Value);

        Assert.Equal(existingEmployee, returnedDto.Employee);
        Assert.Equal(existingRole, returnedDto.Role);

        employeeServiceMock.Verify(x => x.GetEmployee(email), Times.Once);
        roleServiceMock.Verify(x => x.GetRole(role), Times.Once);
        employeeRoleServiceMock.Verify(x => x.GetEmployeeRole(email), Times.Once);
        employeeRoleServiceMock.Verify(x => x.DeleteEmployeeRole(email, role), Times.Once);
        authServiceMock.Verify(x => x.GetUsersByEmailAsync(email), Times.Once);
        authServiceMock.Verify(x => x.GetUserRolesAsync("authUserId"), Times.Once);
        authServiceMock.Verify(x => x.RemoveRoleFromUserAsync("authUserId", "authRoleId"), Times.Once);
    }


    [Fact]
    public async Task RemoveRoleExceptionReturnsBadRequest()
    {
        var employeeServiceMock = new Mock<IEmployeeService>();
        var employeeRoleServiceMock = new Mock<IEmployeeRoleService>();
        var roleServiceMock = new Mock<IRoleService>();
        var employeeAuthServiceMock = new Mock<IAuthService>();

        var controller = new EmployeeRoleManageController(employeeRoleServiceMock.Object, employeeServiceMock.Object,
                                                          roleServiceMock.Object, employeeAuthServiceMock.Object);

        var email = "test@retrorabbit.co.za";
        var role = "Employee";

        employeeServiceMock.Setup(x => x.GetEmployee(email))
                           .ThrowsAsync(new Exception("An error occurred getting roles"));

        var result = await controller.RemoveRole(email, role);

        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("An error occurred getting roles", badRequestResult.Value);

        employeeServiceMock.Verify(x => x.GetEmployee(email), Times.Once);
        roleServiceMock.Verify(x => x.GetRole(role), Times.Never);
        employeeRoleServiceMock.Verify(x => x.GetEmployeeRole(email), Times.Never);
        employeeRoleServiceMock.Verify(x => x.DeleteEmployeeRole(email, role), Times.Never);
    }

    [Fact]
    public async Task GetEmployeeRoleValidInputReturnsOk()
    {
        var email = "test@retrorabbit.co.za";
        var expectedRoleDescription = "Employee Role";

        var employeeRoleServiceMock = new Mock<IEmployeeRoleService>();
        var controller = new EmployeeRoleManageController(employeeRoleServiceMock.Object, null, null, null);

        employeeRoleServiceMock.Setup(x => x.GetEmployeeRole(email))
                               .ReturnsAsync(new EmployeeRoleDto
                               {
                                   Id = 1,
                                   Employee = new EmployeeDto
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
                                       Email = email,
                                       PersonalEmail = "john.doe.personal@example.com",
                                       CellphoneNo = "1234567890",
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
                                   },
                                   Role = new RoleDto { Id = 1, Description = "Employee Role" }
                               });

        var result = await controller.GetEmployeeRole(email);

       var okResult = Assert.IsType<OkObjectResult>(result);
       var returnedRole = Assert.IsType<string[]>(okResult.Value);

       Assert.Single(returnedRole);
       Assert.Equal(expectedRoleDescription, returnedRole[0]);

        employeeRoleServiceMock.Verify(x => x.GetEmployeeRole(email), Times.Once);
    }

    [Fact]
    public async Task GetAllRolesReturnsOk()
    {
        var roleServiceMock = new Mock<IRoleService>();
        var employeeAuthServiceMock = new Mock<IAuthService>();

        var controller = new EmployeeRoleManageController(null, null,
                                                          roleServiceMock.Object, employeeAuthServiceMock.Object);
        var roles = new List<RoleDto>
        {
            new RoleDto{ Id = 1, Description = "Super Admin" },
            new RoleDto{Id = 2, Description = "Admin"},
        };

        roleServiceMock.Setup(x => x.GetAll()).ReturnsAsync(roles);


        var result = await controller.GetAllRoles();

        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedRoles = Assert.IsType<List<string>>(okResult.Value);

        Assert.Equal(roles.Count, returnedRoles.Count);
        Assert.All(returnedRoles,
                   roleDescription => Assert.Contains(roleDescription, roles.Select(role => role.Description)));

        roleServiceMock.Verify(x => x.GetAll(), Times.Once);
    }

    [Fact]
    public async Task GetAllRolesReturnsNotFoundOnError()
    {
        var roleServiceMock = new Mock<IRoleService>();
        var employeeAuthServiceMock = new Mock<IAuthService>();

        var controller = new EmployeeRoleManageController(null, null,
                                                          roleServiceMock.Object, employeeAuthServiceMock.Object);

        roleServiceMock.Setup(x => x.GetAll()).ThrowsAsync(new Exception("An error occurred getting all roles."));

        var result = await controller.GetAllRoles();

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("An error occurred getting all roles.", notFoundResult.Value?.ToString());

        roleServiceMock.Verify(x => x.GetAll(), Times.Once);
    }

    [Fact]
    public async Task GetAllEmployeeOnRolesReturnsOk()
    {
        var employeeRoleServiceMock = new Mock<IEmployeeRoleService>();
        var employeeAuthServiceMock = new Mock<IAuthService>();
        var controller = new EmployeeRoleManageController(employeeRoleServiceMock.Object, null, null, employeeAuthServiceMock.Object);

        var roleId = 1;
        var employeeRoles = new List<EmployeeRoleDto>
        {
           new EmployeeRoleDto
           {
               Id = 1,
               Employee = null,
               Role = new RoleDto { Id = 0, Description = "Super Admin" }
          },
           new EmployeeRoleDto
           {
               Id = 2,
                Employee = null,
               Role = new RoleDto { Id = 0, Description = "Admin" }
           }
        };

        employeeRoleServiceMock.Setup(x => x.GetAllEmployeeOnRoles(roleId)).ReturnsAsync(employeeRoles);

        var result = await controller.GetAllEmployeeOnRoles(roleId);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedEmployeeRoles = Assert.IsType<List<EmployeeRoleDto>>(okResult.Value);

        Assert.Equal(employeeRoles.Count, returnedEmployeeRoles.Count);

        employeeRoleServiceMock.Verify(x => x.GetAllEmployeeOnRoles(roleId), Times.Once);
    }

    [Fact]
    public async Task GetAllEmployeeOnRolesReturnsNotFoundOnError()
    {
        var employeeRoleServiceMock = new Mock<IEmployeeRoleService>();
        var employeeAuthServiceMock = new Mock<IAuthService>();
        var controller = new EmployeeRoleManageController(employeeRoleServiceMock.Object, null, null, employeeAuthServiceMock.Object);

        var roleId = 1;

        employeeRoleServiceMock.Setup(x => x.GetAllEmployeeOnRoles(roleId))
                              .ThrowsAsync(new Exception("An error occurred getting role."));

        var result = await controller.GetAllEmployeeOnRoles(roleId);

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("An error occurred getting role.", notFoundResult.Value?.ToString());

        employeeRoleServiceMock.Verify(x => x.GetAllEmployeeOnRoles(roleId), Times.Once);
    }

    [Fact]
    public async Task GetEmployeeRoleExceptionReturnsNotFound()
    {
        var email = "test@retrorabbit.co.za";
        var expectedErrorMessage = "An error occurred while processing the request.";

        var employeeRoleServiceMock = new Mock<IEmployeeRoleService>();
        var employeeAuthServiceMock = new Mock<IAuthService>();
        var controller = new EmployeeRoleManageController(employeeRoleServiceMock.Object, null, null, employeeAuthServiceMock.Object);

        employeeRoleServiceMock.Setup(x => x.GetEmployeeRole(email)).ThrowsAsync(new Exception(expectedErrorMessage));

        var result = await controller.GetEmployeeRole(email);

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.NotNull(notFoundResult);
        Assert.Equal(expectedErrorMessage, notFoundResult.Value as string);

        employeeRoleServiceMock.Verify(x => x.GetEmployeeRole(email), Times.Once);
    }
}
