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
    //private readonly Mock<IEmployeeService> _employeeServiceMock;
    //private readonly Mock<IEmployee
    //> _employeeRoleServiceMock;
    //private readonly Mock<IRoleService> _roleServiceMock;
    //private readonly EmployeeRoleManageController _controller;
    //private readonly EmployeeDto _employeeDto;
    //private readonly EmployeeRoleDto _employeeRoleDto;
    //public EmployeeRoleManagerControllerUnitTests()
    //{
    //    _employeeServiceMock = new Mock<IEmployeeService>();
    //    _employeeRoleServiceMock = new Mock<IEmployeeRoleService>();
    //    _roleServiceMock = new Mock<IRoleService>();
    //    _controller = new EmployeeRoleManageController(_employeeRoleServiceMock.Object, _employeeServiceMock.Object, _roleServiceMock.Object);
    //    _employeeDto = EmployeeTestData.EmployeeDto;

    //    _employeeRoleDto = new EmployeeRoleDto
    //    {
    //        Id = 1,
    //        Employee = _employeeDto,
    //        Role = new RoleDto { Id = 1, Description = "Admin" }
    //    };
    //}




    ////pARAMETERISE THESE

    //[Fact]
    //public async Task AddRoleValidInputReturnsCreatedAtAction()
    //{
    //    _employeeServiceMock.Setup(x => x.GetEmployee("test@retrorabbit.co.za")).ReturnsAsync(_employeeDto);
    //    _roleServiceMock.Setup(x => x.CheckRole("Admin")).ReturnsAsync(true);
    //    _roleServiceMock.Setup(x => x.GetRole("Admin")).ReturnsAsync(new RoleDto { Id = 1, Description = "Admin" });
    //    _employeeRoleServiceMock.Setup(x => x.SaveEmployeeRole(It.IsAny<EmployeeRoleDto>())).ReturnsAsync(_employeeRoleDto);

    //    var result = await _controller.AddRole("test@retrorabbit.co.za", "Admin");

    //    var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
    //    Assert.Equal(nameof(EmployeeRoleManageController.AddRole), createdAtActionResult.ActionName);
    //    _employeeServiceMock.Verify(x => x.GetEmployee("test@retrorabbit.co.za"), Times.Once);
    //    _roleServiceMock.Verify(x => x.CheckRole("Admin"), Times.Once);
    //    _roleServiceMock.Verify(x => x.GetRole("Admin"), Times.Once);
    //    _employeeRoleServiceMock.Verify(x => x.SaveEmployeeRole(It.IsAny<EmployeeRoleDto>()), Times.Once);
    //}

    //[Fact]
    //public async Task AddRoleExistingRoleReturnsCreatedAtAction()
    //{
    //    _employeeServiceMock.Setup(x => x.GetEmployee("test@retrorabbit.co.za")).ReturnsAsync(_employeeDto);
    //    _roleServiceMock.Setup(x => x.CheckRole("Super Admin")).ReturnsAsync(true);
    //    _roleServiceMock.Setup(x => x.GetRole("Super Admin")).ReturnsAsync(new RoleDto { Id = 1, Description = "Super Admin" });
    //    _employeeRoleServiceMock.Setup(x => x.SaveEmployeeRole(It.IsAny<EmployeeRoleDto>())).ReturnsAsync(_employeeRoleDto);

    //    var result = await _controller.AddRole("test@retrorabbit.co.za", "Super Admin");

    //    var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
    //    Assert.Equal(nameof(EmployeeRoleManageController.AddRole), createdAtActionResult.ActionName);
    //    _employeeServiceMock.Verify(x => x.GetEmployee("test@retrorabbit.co.za"), Times.Once);
    //    _roleServiceMock.Verify(x => x.CheckRole("Super Admin"), Times.Once);
    //    _roleServiceMock.Verify(x => x.GetRole("Super Admin"), Times.Once);
    //    _employeeRoleServiceMock.Verify(x => x.SaveEmployeeRole(It.IsAny<EmployeeRoleDto>()), Times.Once);
    //}


    //// Here 



    //[Fact]
    //public async Task AddRoleUnknownRoleReturnsCreatedAtAction()
    //{
    //    _employeeServiceMock.Setup(x => x.GetEmployee("test@retrorabbit.co.za")).ReturnsAsync(_employeeDto);
    //    _roleServiceMock.Setup(x => x.CheckRole("Unknown Role")).ReturnsAsync(false);
    //    _employeeRoleServiceMock.Setup(x => x.SaveEmployeeRole(It.IsAny<EmployeeRoleDto>())).ReturnsAsync(_employeeRoleDto);

    //    var result = await _controller.AddRole("test@retrorabbit.co.za", "Unknown Role");

    //    var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
    //    Assert.Equal(nameof(EmployeeRoleManageController.AddRole), createdAtActionResult.ActionName);
    //    _employeeServiceMock.Verify(x => x.GetEmployee("test@retrorabbit.co.za"), Times.Once);
    //    _roleServiceMock.Verify(x => x.CheckRole("Unknown Role"), Times.Once);
    //    _employeeRoleServiceMock.Verify(x => x.SaveEmployeeRole(It.IsAny<EmployeeRoleDto>()), Times.Once);
    //}

    //[Fact]
    //public async Task AddRoleInvalidInputReturnsBadRequest()
    //{
    //    var invalidEmail = string.Empty;
    //    string? invalidRole = null;

    //    var result = await _controller.AddRole(invalidEmail, invalidRole!);

    //    Assert.IsType<BadRequestObjectResult>(result);
    //    _employeeServiceMock.Verify(x => x.GetEmployee(It.IsAny<string>()), Times.Never);
    //    _roleServiceMock.Verify(x => x.CheckRole(It.IsAny<string>()), Times.Never);
    //    _roleServiceMock.Verify(x => x.GetRole(It.IsAny<string>()), Times.Never);
    //    _employeeRoleServiceMock.Verify(x => x.SaveEmployeeRole(It.IsAny<EmployeeRoleDto>()), Times.Never);
    //}

    //[Fact]
    //public async Task AddRoleExceptionReturnsBadRequest()
    //{
    //    _employeeServiceMock.Setup(x => x.GetEmployee("test@retrorabbit.co.za"))
    //                       .ThrowsAsync(new Exception("An error occurred adding role"));

    //    var result = await _controller.AddRole("test@retrorabbit.co.za", "Employee");

    //    var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
    //    Assert.Equal("An error occurred adding role", badRequestResult.Value);
    //    _employeeServiceMock.Verify(x => x.GetEmployee("test@retrorabbit.co.za"), Times.Once);
    //    _roleServiceMock.Verify(x => x.CheckRole("Employee"), Times.Never);
    //    _roleServiceMock.Verify(x => x.GetRole("Employee"), Times.Never);
    //    _employeeRoleServiceMock.Verify(x => x.SaveEmployeeRole(It.IsAny<EmployeeRoleDto>()), Times.Never);
    //}

    //[Fact]
    //public async Task UpdateRoleValidInputReturnsCreatedAtAction()
    //{
    //    _employeeServiceMock.Setup(x => x.GetEmployee("test@retrorabbit.co.za")).ReturnsAsync(_employeeDto);
    //    _roleServiceMock.Setup(x => x.CheckRole("Employee")).ReturnsAsync(true);
    //    _roleServiceMock.Setup(x => x.GetRole("Employee")).ReturnsAsync(new RoleDto { Id = 0, Description = "Employee" });
    //    _employeeRoleServiceMock.Setup(x => x.GetEmployeeRole("test@retrorabbit.co.za")).ReturnsAsync(_employeeRoleDto);
    //    _employeeRoleServiceMock.Setup(x => x.UpdateEmployeeRole(It.IsAny<EmployeeRoleDto>())).ReturnsAsync(_employeeRoleDto);

    //    var result = await _controller.UpdateRole("test@retrorabbit.co.za", "Employee");

    //    var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
    //    Assert.Equal(nameof(EmployeeRoleManageController.UpdateRole), createdAtActionResult.ActionName);
    //    _employeeServiceMock.Verify(x => x.GetEmployee("test@retrorabbit.co.za"), Times.Once);
    //    _roleServiceMock.Verify(x => x.CheckRole("Employee"), Times.Once);
    //    _roleServiceMock.Verify(x => x.GetRole("Employee"), Times.Once);
    //    _employeeRoleServiceMock.Verify(x => x.GetEmployeeRole("test@retrorabbit.co.za"), Times.Once);
    //    _employeeRoleServiceMock.Verify(x => x.UpdateEmployeeRole(It.IsAny<EmployeeRoleDto>()), Times.Once);
    //}

    //[Fact]
    //public async Task UpdateRoleExceptionReturnsBadRequest()
    //{
    //    _employeeServiceMock.Setup(x => x.GetEmployee("test@retrorabbit.co.za")).ThrowsAsync(new Exception("An error occurred updating role"));

    //    var result = await _controller.UpdateRole("test@retrorabbit.co.za", "Journey");

    //    var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
    //    Assert.Equal("An error occurred updating role", badRequestResult.Value);
    //    _employeeServiceMock.Verify(x => x.GetEmployee("test@retrorabbit.co.za"), Times.Once);
    //    _roleServiceMock.Verify(x => x.CheckRole("Journey"), Times.Never);
    //    _roleServiceMock.Verify(x => x.GetRole("Journey"), Times.Never);
    //    _employeeRoleServiceMock.Verify(x => x.GetEmployeeRole("test@retrorabbit.co.za"), Times.Never);
    //    _employeeRoleServiceMock.Verify(x => x.UpdateEmployeeRole(It.IsAny<EmployeeRoleDto>()), Times.Never);
    //}

    //[Fact]
    //public async Task RemoveRoleValidInputReturnsOk()
    //{
    //    _employeeRoleServiceMock.Setup(x => x.DeleteEmployeeRole("test@retrorabbit.co.za", "Journey")).ReturnsAsync(_employeeRoleDto);

    //    var result = await _controller.RemoveRole("test@retrorabbit.co.za", "Journey");

    //    var okResult = Assert.IsType<OkObjectResult>(result);
    //    Assert.IsType<EmployeeRoleDto>(okResult.Value);
    //    _employeeServiceMock.Verify(x => x.GetEmployee("test@retrorabbit.co.za"), Times.Once);
    //    _roleServiceMock.Verify(x => x.GetRole("Journey"), Times.Once);
    //    _employeeRoleServiceMock.Verify(x => x.GetEmployeeRole("test@retrorabbit.co.za"), Times.Once);
    //    _employeeRoleServiceMock.Verify(x => x.DeleteEmployeeRole("test@retrorabbit.co.za", "Journey"), Times.Once);
    //}

    //[Fact]
    //public async Task RemoveRoleExceptionReturnsBadRequest()
    //{
    //    _employeeServiceMock.Setup(x => x.GetEmployee("test@retrorabbit.co.za")).ThrowsAsync(new Exception("An error occurred getting roles"));

    //    var result = await _controller.RemoveRole("test@retrorabbit.co.za", "Employee");

    //    var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
    //    Assert.Equal("An error occurred getting roles", badRequestResult.Value);
    //    _employeeServiceMock.Verify(x => x.GetEmployee("test@retrorabbit.co.za"), Times.Once);
    //    _roleServiceMock.Verify(x => x.GetRole("Employee"), Times.Never);
    //    _employeeRoleServiceMock.Verify(x => x.GetEmployeeRole("test@retrorabbit.co.za"), Times.Never);
    //    _employeeRoleServiceMock.Verify(x => x.DeleteEmployeeRole("test@retrorabbit.co.za", "Employee"), Times.Never);
    //}

    //[Fact]
    //public async Task GetEmployeeRoleValidInputReturnsOk()
    //{
    //    _employeeRoleServiceMock.Setup(x => x.GetEmployeeRole("test@retrorabbit.co.za")).ReturnsAsync(new EmployeeRoleDto
    //    {
    //        Id = 1,
    //        Employee = _employeeDto,
    //        Role = new RoleDto { Id = 1, Description = "Employee Role" }
    //    });

    //    var result = await _controller.GetEmployeeRole("test@retrorabbit.co.za");

    //    var okResult = Assert.IsType<OkObjectResult>(result);
    //    var returnedRole = Assert.IsType<string[]>(okResult.Value);
    //    Assert.Single(returnedRole);
    //    Assert.Equal("Employee Role", returnedRole[0]);
    //    _employeeRoleServiceMock.Verify(x => x.GetEmployeeRole("test@retrorabbit.co.za"), Times.Once);
    //}

    //[Fact]
    //public async Task GetAllRolesReturnsOk()
    //{
    //    var roles = new List<RoleDto>
    //    {
    //        new RoleDto{ Id = 1, Description = "Super Admin" },
    //        new RoleDto{Id = 2, Description = "Admin"},
    //    };

    //    _roleServiceMock.Setup(x => x.GetAll()).ReturnsAsync(roles);

    //    var result = await _controller.GetAllRoles();

    //    var okResult = Assert.IsType<OkObjectResult>(result);
    //    var returnedRoles = Assert.IsType<List<string>>(okResult.Value);
    //    Assert.Equal(roles.Count, returnedRoles.Count);
    //    Assert.All(returnedRoles, roleDescription => Assert.Contains(roleDescription, roles.Select(role => role.Description)));
    //    _roleServiceMock.Verify(x => x.GetAll(), Times.Once);
    //}

    //[Fact]
    //public async Task GetAllRolesReturnsNotFoundOnError()
    //{
    //    _roleServiceMock.Setup(x => x.GetAll()).ThrowsAsync(new Exception("An error occurred getting all roles."));

    //    var result = await _controller.GetAllRoles();

    //    var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
    //    Assert.Equal("An error occurred getting all roles.", notFoundResult.Value?.ToString());
    //    _roleServiceMock.Verify(x => x.GetAll(), Times.Once);
    //}

    //[Fact]
    //public async Task GetAllEmployeeOnRolesReturnsOk()
    //{
    //    var employeeRoles = new List<EmployeeRoleDto>
    //    {
    //       new EmployeeRoleDto
    //       {
    //            Id = 1,
    //            Employee = null,
    //            Role = new RoleDto { Id = 0, Description = "Super Admin" }
    //       },
    //       new EmployeeRoleDto
    //       {
    //            Id = 2,
    //            Employee = null,
    //            Role = new RoleDto { Id = 0, Description = "Admin" }
    //       }
    //    };

    //    _employeeRoleServiceMock.Setup(x => x.GetAllEmployeeOnRoles(1)).ReturnsAsync(employeeRoles);

    //    var result = await _controller.GetAllEmployeeOnRoles(1);

    //    var okResult = Assert.IsType<OkObjectResult>(result);
    //    var returnedEmployeeRoles = Assert.IsType<List<EmployeeRoleDto>>(okResult.Value);
    //    Assert.Equal(employeeRoles.Count, returnedEmployeeRoles.Count);
    //    _employeeRoleServiceMock.Verify(x => x.GetAllEmployeeOnRoles(1), Times.Once);
    //}

    //[Fact]
    //public async Task GetAllEmployeeOnRolesReturnsNotFoundOnError()
    //{
    //    _employeeRoleServiceMock.Setup(x => x.GetAllEmployeeOnRoles(1))
    //                           .ThrowsAsync(new Exception("An error occurred getting role."));

    //    var result = await _controller.GetAllEmployeeOnRoles(1);

    //    var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
    //    Assert.Equal("An error occurred getting role.", notFoundResult.Value?.ToString());
    //    _employeeRoleServiceMock.Verify(x => x.GetAllEmployeeOnRoles(1), Times.Once);
    //}

    //[Fact]
    //public async Task GetEmployeeRoleExceptionReturnsNotFound()
    //{
    //    _employeeRoleServiceMock.Setup(x => x.GetEmployeeRole("test@retrorabbit.co.za")).ThrowsAsync(new Exception("An error occurred while processing the request."));

    //    var result = await _controller.GetEmployeeRole("test@retrorabbit.co.za");

    //    var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
    //    Assert.NotNull(notFoundResult);
    //    Assert.Equal("An error occurred while processing the request.", notFoundResult.Value as string);
    //    _employeeRoleServiceMock.Verify(x => x.GetEmployeeRole("test@retrorabbit.co.za"), Times.Once);
    //}
}