using Microsoft.AspNetCore.Mvc;
using Moq;
using RGO.App.Controllers;
using RGO.Models;
using RGO.Models.Enums;
using RGO.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace RGO.App.Tests.Controllers
{
    public class EmployeeRoleManagerControllerUnitTests
    {
        [Fact]
        public async Task AddRoleValidInputReturnsCreatedAtAction()
        {
            var employeeServiceMock = new Mock<IEmployeeService>();
            var employeeRoleServiceMock = new Mock<IEmployeeRoleService>();
            var roleServiceMock = new Mock<IRoleService>();

            var controller = new EmployeeRoleManageController(employeeRoleServiceMock.Object, employeeServiceMock.Object, roleServiceMock.Object);

            var email = "test@retrorabbit.co.za";
            var role = "TestRole";

            employeeServiceMock.Setup(x => x.GetEmployee(email)).ReturnsAsync(new EmployeeDto
                    (1, "Emp123", "Tax123", new DateTime(2022, 1, 1), null, 1, false, "No disability", 2,
                        new EmployeeTypeDto(1, "Full Time"), "Notes", 20.0f, 15.0f, 50.0f, 50000, "John Doe", "JD", "Doe", new DateTime(1990, 1, 1),
                        "South Africa", "South African", "123456789", "AB123456", new DateTime(2025, 1, 1), "South Africa", Race.White, Gender.Male, "photo.jpg",
                        "test@retrorabbit.co.za", "john.doe.personal@example.com", "1234567890", 1, 1,
                        new EmployeeAddressDto
                        (1, "Unit 1", "Complex A", "123", "Suburb", "City", "Country", "Province", "12345"),
                        new EmployeeAddressDto
                        (2, "P.O. Box 123", "", "456", "Suburb", "City", "Country", "Province", "54321"),
                        "12",
                        "Emergency Contact",
                        "987654321"
                    ));
            roleServiceMock.Setup(x => x.CheckRole(role)).ReturnsAsync(true);
            roleServiceMock.Setup(x => x.GetRole(role)).ReturnsAsync(new RoleDto(1, "Role Description"));
            employeeRoleServiceMock.Setup(x => x.SaveEmployeeRole(It.IsAny<EmployeeRoleDto>())).ReturnsAsync(new EmployeeRoleDto());

            try
            {
                var result = await controller.AddRole(email, role);

                var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);

                Assert.Equal(nameof(EmployeeRoleManageController.AddRole), createdAtActionResult.ActionName);

                employeeServiceMock.Verify(x => x.GetEmployee(email), Times.Once);
                roleServiceMock.Verify(x => x.CheckRole(role), Times.Once);
                roleServiceMock.Verify(x => x.GetRole(role), Times.Once);
                employeeRoleServiceMock.Verify(x => x.SaveEmployeeRole(It.IsAny<EmployeeRoleDto>()), Times.Once);
            }
            catch (Exception ex)
            {
                Assert.True(false, $"Exception thrown: {ex.Message}");
            }
        }

        [Fact]
        public async Task AddRoleExistingRoleReturnsCreatedAtAction()
        {
        }

        [Fact]
        public async Task AddRoleUnknownRoleReturnsCreatedAtAction()
        {
        }

        [Fact]
        public async Task AddRoleInvalidInputReturnsBadRequest()
        {
        }

        [Fact]
        public async Task UpdateRoleValidInputReturnsCreatedAtAction()
        {
        }

        [Fact]
        public async Task UpdateRoleExistingRoleReturnsCreatedAtAction()
        {
        }

        [Fact]
        public async Task UpdateRoleUnknownRoleReturnsCreatedAtAction()
        {
        }

        [Fact]
        public async Task UpdateRoleInvalidInputReturnsBadRequest()
        {
        }

        [Fact]
        public async Task RemoveRoleValidInputReturnsOk()
        {
        }

        [Fact]
        public async Task RemoveRoleInvalidInputReturnsBadRequest()
        {
        }

        [Fact]
        public async Task GetEmployeeRoleValidInputReturnsOk()
        {
        }

        [Fact]
        public async Task GetEmployeeRoleInvalidInputReturnsNotFound()
        {
        }

        [Fact]
        public async Task GetAllRolesValidInputReturnsOk()
        {
        }

        [Fact]
        public async Task GetAllRolesInvalidInputReturnsNotFound()
        {
        }

        [Fact]
        public async Task GetAllEmployeeOnRolesValidInputReturnsOk()
        {
        }

        [Fact]
        public async Task GetAllEmployeeOnRolesInvalidInputReturnsNotFound()
        {
        }
    }
}
