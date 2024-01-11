using Microsoft.AspNetCore.Mvc;
using Moq;
using RGO.App.Controllers;
using RGO.Models;
using RGO.Models.Enums;
using RGO.Services.Interfaces;
using RGO.UnitOfWork.Entities;
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

            employeeRoleServiceMock.Setup(x => x.SaveEmployeeRole(It.IsAny<EmployeeRoleDto>())).ReturnsAsync(new EmployeeRoleDto(1, new EmployeeDto(1, "Emp123", "Tax123", new DateTime(2022, 1, 1), null, 1, false, "No disability", 2,
                        new EmployeeTypeDto(1, "Full Time"), "Notes", 20.0f, 15.0f, 50.0f, 50000, "John Doe", "JD", "Doe", new DateTime(1990, 1, 1),
                        "South Africa", "South African", "123456789", "AB123456", new DateTime(2025, 1, 1), "South Africa", Race.White, Gender.Male, "photo.jpg",
                        "test@retrorabbit.co.za", "john.doe.personal@example.com", "1234567890", 1, 1,
                        new EmployeeAddressDto
                        (1, "Unit 1", "Complex A", "123", "Suburb", "City", "Country", "Province", "12345"),
                        new EmployeeAddressDto
                        (2, "P.O. Box 123", "", "456", "Suburb", "City", "Country", "Province", "54321"),
                        "12",
                        "Emergency Contact",
                        "987654321"),
                        new RoleDto(1, "Role Description")));

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
            var employeeServiceMock = new Mock<IEmployeeService>();
            var employeeRoleServiceMock = new Mock<IEmployeeRoleService>();
            var roleServiceMock = new Mock<IRoleService>();

            var controller = new EmployeeRoleManageController(employeeRoleServiceMock.Object, employeeServiceMock.Object, roleServiceMock.Object);

            var email = "test@retrorabbit.co.za";
            var role = "ExistingRole";

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

            employeeRoleServiceMock.Setup(x => x.SaveEmployeeRole(It.IsAny<EmployeeRoleDto>())).ReturnsAsync(new EmployeeRoleDto(1, new EmployeeDto(1, "Emp123", "Tax123", new DateTime(2022, 1, 1), null, 1, false, "No disability", 2,
                        new EmployeeTypeDto(1, "Full Time"), "Notes", 20.0f, 15.0f, 50.0f, 50000, "John Doe", "JD", "Doe", new DateTime(1990, 1, 1),
                        "South Africa", "South African", "123456789", "AB123456", new DateTime(2025, 1, 1), "South Africa", Race.White, Gender.Male, "photo.jpg",
                        "test@retrorabbit.co.za", "john.doe.personal@example.com", "1234567890", 1, 1,
                        new EmployeeAddressDto
                        (1, "Unit 1", "Complex A", "123", "Suburb", "City", "Country", "Province", "12345"),
                        new EmployeeAddressDto
                        (2, "P.O. Box 123", "", "456", "Suburb", "City", "Country", "Province", "54321"),
                        "12",
                        "Emergency Contact",
                        "987654321"),
                        new RoleDto(1, "Role Description")));

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
        public async Task AddRoleUnknownRoleReturnsCreatedAtAction()
        {
            var employeeServiceMock = new Mock<IEmployeeService>();
            var employeeRoleServiceMock = new Mock<IEmployeeRoleService>();
            var roleServiceMock = new Mock<IRoleService>();

            var controller = new EmployeeRoleManageController(employeeRoleServiceMock.Object, employeeServiceMock.Object, roleServiceMock.Object);

            var email = "test@retrorabbit.co.za";
            var role = "UnknownRole";

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
            roleServiceMock.Setup(x => x.CheckRole(role)).ReturnsAsync(false);
            employeeRoleServiceMock.Setup(x => x.SaveEmployeeRole(It.IsAny<EmployeeRoleDto>())).ReturnsAsync(new EmployeeRoleDto(1, new EmployeeDto(1, "Emp123", "Tax123", new DateTime(2022, 1, 1), null, 1, false, "No disability", 2,
                        new EmployeeTypeDto(1, "Full Time"), "Notes", 20.0f, 15.0f, 50.0f, 50000, "John Doe", "JD", "Doe", new DateTime(1990, 1, 1),
                        "South Africa", "South African", "123456789", "AB123456", new DateTime(2025, 1, 1), "South Africa", Race.White, Gender.Male, "photo.jpg",
                        "test@retrorabbit.co.za", "john.doe.personal@example.com", "1234567890", 1, 1,
                        new EmployeeAddressDto
                        (1, "Unit 1", "Complex A", "123", "Suburb", "City", "Country", "Province", "12345"),
                        new EmployeeAddressDto
                        (2, "P.O. Box 123", "", "456", "Suburb", "City", "Country", "Province", "54321"),
                        "12",
                        "Emergency Contact",
                        "987654321"),
                        new RoleDto(1, "Role Description")));

            try
            {
                var result = await controller.AddRole(email, role);

                var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
                Assert.Equal(nameof(EmployeeRoleManageController.AddRole), createdAtActionResult.ActionName);

                employeeServiceMock.Verify(x => x.GetEmployee(email), Times.Once);
                roleServiceMock.Verify(x => x.CheckRole(role), Times.Once);
                employeeRoleServiceMock.Verify(x => x.SaveEmployeeRole(It.IsAny<EmployeeRoleDto>()), Times.Once);
            }
            catch (Exception ex)
            {
                Assert.True(false, $"Exception thrown: {ex.Message}");
            }
        }

        [Fact]
        public async Task AddRoleInvalidInputReturnsBadRequest()
        {
            var employeeServiceMock = new Mock<IEmployeeService>();
            var employeeRoleServiceMock = new Mock<IEmployeeRoleService>();
            var roleServiceMock = new Mock<IRoleService>();

            var controller = new EmployeeRoleManageController(employeeRoleServiceMock.Object, employeeServiceMock.Object, roleServiceMock.Object);

            string? invalidEmail = string.Empty;
            string? invalidRole = null;
            var result = await controller.AddRole(invalidEmail, invalidRole);

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

            var controller = new EmployeeRoleManageController(employeeRoleServiceMock.Object, employeeServiceMock.Object, roleServiceMock.Object);

            var email = "test@retrorabbit.co.za";
            var role = "TestRole";

            employeeServiceMock.Setup(x => x.GetEmployee(email)).ThrowsAsync(new Exception("An error occurred adding role"));

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
                "987654321"));

            roleServiceMock.Setup(x => x.CheckRole(role)).ReturnsAsync(true);
            roleServiceMock.Setup(x => x.GetRole(role)).ReturnsAsync(new RoleDto(1, "Role Description"));

            var existingEmployeeRole = new EmployeeRoleDto(1, new EmployeeDto(1, "Emp123", "Tax123", new DateTime(2022, 1, 1), null, 1, false, "No disability", 2,
                new EmployeeTypeDto(1, "Full Time"), "Notes", 20.0f, 15.0f, 50.0f, 50000, "John Doe", "JD", "Doe", new DateTime(1990, 1, 1),
                "South Africa", "South African", "123456789", "AB123456", new DateTime(2025, 1, 1), "South Africa", Race.White, Gender.Male, "photo.jpg",
                "test@retrorabbit.co.za", "john.doe.personal@example.com", "1234567890", 1, 1,
                new EmployeeAddressDto
                (1, "Unit 1", "Complex A", "123", "Suburb", "City", "Country", "Province", "12345"),
                new EmployeeAddressDto
                (2, "P.O. Box 123", "", "456", "Suburb", "City", "Country", "Province", "54321"),
                "12",
                "Emergency Contact",
                "987654321"),
                new RoleDto(1, "Role Description"));

            employeeRoleServiceMock.Setup(x => x.GetEmployeeRole(email)).ReturnsAsync(existingEmployeeRole);
            employeeRoleServiceMock.Setup(x => x.UpdateEmployeeRole(It.IsAny<EmployeeRoleDto>())).ReturnsAsync(existingEmployeeRole);

            var result = await controller.UpdateRole(email, role);

            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(nameof(EmployeeRoleManageController.UpdateRole), createdAtActionResult.ActionName);

            employeeServiceMock.Verify(x => x.GetEmployee(email), Times.Once);
            roleServiceMock.Verify(x => x.CheckRole(role), Times.Once);
            roleServiceMock.Verify(x => x.GetRole(role), Times.Once);
            employeeRoleServiceMock.Verify(x => x.GetEmployeeRole(email), Times.Once);
            employeeRoleServiceMock.Verify(x => x.UpdateEmployeeRole(It.IsAny<EmployeeRoleDto>()), Times.Once);
        }

        [Fact]
        public async Task UpdateRoleExceptionReturnsBadRequest()
        {
            var employeeServiceMock = new Mock<IEmployeeService>();
            var employeeRoleServiceMock = new Mock<IEmployeeRoleService>();
            var roleServiceMock = new Mock<IRoleService>();

            var controller = new EmployeeRoleManageController(employeeRoleServiceMock.Object, employeeServiceMock.Object, roleServiceMock.Object);

            var email = "test@retrorabbit.co.za";
            var role = "TestRole";

            employeeServiceMock.Setup(x => x.GetEmployee(email)).ThrowsAsync(new Exception("An error occurred updating role"));

            var result = await controller.UpdateRole(email, role);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("An error occurred updating role", badRequestResult.Value);

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
            var role = "TestRole";

            //EmployeeRoleDto

            var employeeServiceMock = new Mock<IEmployeeService>();
            var employeeRoleServiceMock = new Mock<IEmployeeRoleService>();
            var roleServiceMock = new Mock<IRoleService>();

            var controller = new EmployeeRoleManageController(employeeRoleServiceMock.Object, employeeServiceMock.Object, roleServiceMock.Object);

            var employeeDto = new EmployeeDto
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
                );

            var roleDto = new RoleDto(1, "TestRole");

            employeeRoleServiceMock.Setup(x => x.DeleteEmployeeRole(email, role)).ReturnsAsync(new EmployeeRoleDto(1, employeeDto, roleDto));

            var result = await controller.RemoveRole(email, role);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedDto = Assert.IsType<EmployeeRoleDto>(okResult.Value);

            employeeServiceMock.Verify(x => x.GetEmployee(email), Times.Once);
            roleServiceMock.Verify(x => x.GetRole(role), Times.Once);
            employeeRoleServiceMock.Verify(x => x.GetEmployeeRole(email), Times.Once);
            employeeRoleServiceMock.Verify(x => x.DeleteEmployeeRole(email, role), Times.Once);
        }

        [Fact]
        public async Task RemoveRoleExceptionReturnsBadRequest()
        {
            var employeeServiceMock = new Mock<IEmployeeService>();
            var employeeRoleServiceMock = new Mock<IEmployeeRoleService>();
            var roleServiceMock = new Mock<IRoleService>();

            var controller = new EmployeeRoleManageController(employeeRoleServiceMock.Object, employeeServiceMock.Object, roleServiceMock.Object);

            var email = "test@retrorabbit.co.za";
            var role = "TestRole";

            employeeServiceMock.Setup(x => x.GetEmployee(email)).ThrowsAsync(new Exception("Simulated exception"));

            var result = await controller.RemoveRole(email, role);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Simulated exception", badRequestResult.Value);

            employeeServiceMock.Verify(x => x.GetEmployee(email), Times.Once);
            roleServiceMock.Verify(x => x.GetRole(role), Times.Never);
            employeeRoleServiceMock.Verify(x => x.GetEmployeeRole(email), Times.Never);
            employeeRoleServiceMock.Verify(x => x.DeleteEmployeeRole(email, role), Times.Never);
        }
    }
}
