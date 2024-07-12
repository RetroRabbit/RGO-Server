using System.Linq.Expressions;
using HRIS.Models;
using HRIS.Services.Services;
using Moq;
using RR.Tests.Data;
using RR.Tests.Data.Models.HRIS;
using RR.UnitOfWork;
using RR.UnitOfWork.Entities.HRIS;
using Xunit;

namespace HRIS.Services.Tests.Services;

public class EmployeeRoleServiceUnitTest
{
    private readonly Mock<IUnitOfWork> _dbMock;
    private readonly EmployeeRoleService _employeeRoleService;

    public EmployeeRoleServiceUnitTest()
    {
        _dbMock = new Mock<IUnitOfWork>();
        _employeeRoleService = new EmployeeRoleService(_dbMock.Object);
    }

    [Fact]
    public async Task SaveEmployeeRoleTest()
    {
        var testEmployee = EmployeeTestData.EmployeeOne;

        var roleList = new List<RoleDto>
        {
            new() { Id = 1, Description = "Admin" },
            new() { Id = 2, Description = "Manager" },
            new() { Id = 3, Description = "Employee" },
            new() { Id = 4, Description = "Intern" }
        };

        var employeeRoleList = new List<EmployeeRole>
        {
            new()
            {
                Id = 1,
                EmployeeId = testEmployee.Id,
                RoleId = roleList[0].Id,
                Employee = testEmployee,
                Role = new Role(roleList[0])
            },
            new()
            {
                Id = 2,
                EmployeeId = testEmployee.Id,
                RoleId = roleList[1].Id,
                Employee = testEmployee
            },
            new()
            {
                Id = 3,
                EmployeeId = testEmployee.Id,
                RoleId = roleList[2].Id,
                Role = new Role(roleList[2])
            },
            new()
            {
                Id = 4,
                EmployeeId = testEmployee.Id,
                RoleId = roleList[3].Id
            }
        };

        _dbMock.SetupSequence(e => e.EmployeeRole.Any(It.IsAny<Expression<Func<EmployeeRole, bool>>>()))
               .ReturnsAsync(true)
               .ReturnsAsync(false)
               .ReturnsAsync(false)
               .ReturnsAsync(false)
               .ReturnsAsync(false);

        _dbMock.SetupSequence(e => e.EmployeeRole.Add(It.IsAny<EmployeeRole>()))
               .ReturnsAsync(employeeRoleList[0])
               .ReturnsAsync(employeeRoleList[1])
               .ReturnsAsync(employeeRoleList[2])
               .ReturnsAsync(employeeRoleList[3]);

        employeeRoleList[1].Role = new Role(roleList[1]);
        employeeRoleList[2].Employee = testEmployee;

        _dbMock.SetupSequence(e => e.EmployeeRole.GetById(It.IsAny<int>()))
               .ReturnsAsync(employeeRoleList[1])
               .ReturnsAsync(employeeRoleList[2])
               .ReturnsAsync(employeeRoleList[3]);


        await Assert.ThrowsAsync<CustomException>(async () => await _employeeRoleService.SaveEmployeeRole(new EmployeeRoleDto
        {
            Id = employeeRoleList[0].Id,
            Employee = employeeRoleList[0].Employee!.ToDto(),
            Role = employeeRoleList[0].Role!.ToDto()
        }));
        var result = await _employeeRoleService.SaveEmployeeRole(employeeRoleList[0].ToDto());
        Assert.Equivalent(employeeRoleList[0].ToDto(), result);
        result = await _employeeRoleService.SaveEmployeeRole(employeeRoleList[1].ToDto());
        Assert.Equivalent(employeeRoleList[1].ToDto(), result);
        result = await _employeeRoleService.SaveEmployeeRole(employeeRoleList[2].ToDto());
        Assert.Equivalent(employeeRoleList[2].ToDto(), result);
        await Assert.ThrowsAsync<CustomException>(() => _employeeRoleService.SaveEmployeeRole(employeeRoleList[3].ToDto()));
    }

    [Fact]
    public async Task DeleteEmployeeRoleTest()
    {
        const string email = "test@retrorabbit.co.za";

        var testEmployee = EmployeeTestData.EmployeeOne;

        var roleList = new List<RoleDto>
        {
            new () {Id = 1, Description = "Admin" },
            new () { Id = 2, Description = "Manager" },
            new () { Id = 3, Description = "Employee" },
            new () { Id = 4, Description = "Intern" }
        };

        var employeeRoleList = new List<EmployeeRole>
        {
            new()
            {
                Id = 1,
                EmployeeId = testEmployee.Id,
                RoleId = roleList[0].Id,
                Employee = testEmployee,
                Role = new Role(roleList[0])
            },
            new()
            {
                Id = 2,
                EmployeeId = testEmployee.Id,
                RoleId = roleList[1].Id,
                Employee = testEmployee,
                Role = new Role(roleList[1])
            },
            new()
            {
                Id = 3,
                EmployeeId = testEmployee.Id,
                RoleId = roleList[2].Id,
                Employee = testEmployee,
                Role = new Role(roleList[2])
            }
        };

        Expression<Func<EmployeeRole, bool>>[] criteriaList =
        {
            e => e.Employee!.Email == email && e.Role!.Description == roleList[0].Description,
            e => e.Employee!.Email == "" && e.Role!.Description == ""
        };

        _dbMock.SetupSequence(e => e.EmployeeRole.Any(It.IsAny<Expression<Func<EmployeeRole, bool>>>()))
               .ReturnsAsync(employeeRoleList.ToMockIQueryable().Any(criteriaList[0]))
               .ReturnsAsync(employeeRoleList.ToMockIQueryable().Any(criteriaList[1]));

        _dbMock.Setup(e => e.EmployeeRole.Get(It.IsAny<Expression<Func<EmployeeRole, bool>>>()))
               .Returns(employeeRoleList.Where(e => e.Employee!.Email == email).ToMockIQueryable());

        _dbMock.Setup(e => e.EmployeeRole.GetAll(It.IsAny<Expression<Func<EmployeeRole, bool>>>()))
               .ReturnsAsync(employeeRoleList);

        _dbMock.SetupSequence(e => e.EmployeeRole.Delete(It.IsAny<int>()))
               .ReturnsAsync(employeeRoleList.Where(e => e.Role!.Description == roleList[0].Description).Select(e => e).FirstOrDefault()!)
               .ReturnsAsync(employeeRoleList.Where(e => e.Role!.Description == roleList[1].Description).Select(e => e).FirstOrDefault()!);

        var result1 = await _employeeRoleService.DeleteEmployeeRole(email, roleList[0].Description!);

        Assert.NotNull(result1);
        await Assert.ThrowsAsync<CustomException>(() => _employeeRoleService.DeleteEmployeeRole("", ""));
    }

    [Fact]
    public async Task UpdateEmployeeRoleTest()
    {
        const string email = "test@retrorabbit.co.za";

        var testEmployee = EmployeeTestData.EmployeeOne;
        var roleList = new List<RoleDto>
        {
            new() {Id = 1, Description = "Admin" },
            new() { Id = 2, Description = "Manager" },
            new() { Id = 3, Description = "Employee" },
            new() { Id = 4, Description = "Intern" }
        };

        var employeeRoleList = new List<EmployeeRole>
        {
            new()
            {
                Id = 1,
                EmployeeId = testEmployee.Id,
                RoleId = roleList[0].Id,
                Employee = testEmployee,
                Role = new Role(roleList[0])
            },
            new()
            {
                Id = 2,
                EmployeeId = testEmployee.Id,
                RoleId = roleList[1].Id,
                Employee = testEmployee,
                Role = new Role(roleList[1])
            },
            new()
            {
                Id = 3,
                EmployeeId = testEmployee.Id,
                RoleId = roleList[2].Id,
                Employee = testEmployee,
                Role = new Role(roleList[2])
            }
        };

        Expression<Func<EmployeeRole, bool>>[] criteriaList =
        {
            e => e.Employee!.Email == email && e.Role!.Description == roleList[0].Description,
            e => e.Employee!.Email == email && e.Role!.Description == roleList[1].Description,
            e => e.Employee!.Email == email && e.Role!.Description == roleList[2].Description,
            e => e.Employee!.Email == email && e.Role!.Description == "Made up Role"
        };

        _dbMock.SetupSequence(e => e.EmployeeRole.Any(It.IsAny<Expression<Func<EmployeeRole, bool>>>()))
               .ReturnsAsync(employeeRoleList.ToMockIQueryable().Any(criteriaList[0]))
               .ReturnsAsync(employeeRoleList.ToMockIQueryable().Any(criteriaList[1]))
               .ReturnsAsync(employeeRoleList.ToMockIQueryable().Any(criteriaList[2]))
               .ReturnsAsync(employeeRoleList.ToMockIQueryable().Any(criteriaList[3]));

        _dbMock.SetupSequence(e => e.EmployeeRole.Update(It.IsAny<EmployeeRole>()))
            .ReturnsAsync(employeeRoleList[0])
            .ReturnsAsync(employeeRoleList[1])
            .ReturnsAsync(employeeRoleList[2]);

        var result1 = await _employeeRoleService.UpdateEmployeeRole(employeeRoleList[0].ToDto());
        var result2 = await _employeeRoleService.UpdateEmployeeRole(employeeRoleList[1].ToDto());
        var result3 = await _employeeRoleService.UpdateEmployeeRole(employeeRoleList[2].ToDto());

        Assert.Equivalent(employeeRoleList[0].ToDto(), result1);
        Assert.Equivalent(employeeRoleList[1].ToDto(), result2);
        Assert.Equivalent(employeeRoleList[2].ToDto(), result3);

        Assert.ThrowsAsync<Exception>(() => _employeeRoleService.UpdateEmployeeRole(new EmployeeRoleDto
        {
            Id = 4,
            Employee = employeeRoleList[0].Employee!.ToDto(),
            Role = new RoleDto { Id = 2, Description = "Made up Role" }
        }));
    }

    [Fact]
    public async Task GetAllEmployeeRoles()
    {
        var testEmployee = EmployeeTestData.EmployeeOne;

        var roleList = new List<RoleDto>
        {
            new() {Id = 1, Description = "Admin" },
            new() { Id = 2, Description = "Manager" },
            new() { Id = 3, Description = "Employee" },
            new() { Id = 4, Description = "Intern" }
        };

        var employeeRoleList = new List<EmployeeRole>
        {
            new()
            {
                Id = 1,
                EmployeeId = testEmployee.Id,
                RoleId = roleList[0].Id,
                Employee = testEmployee,
                Role = new Role(roleList[0])
            },
            new()
            {
                Id = 2,
                EmployeeId = testEmployee.Id,
                RoleId = roleList[1].Id,
                Employee = testEmployee,
                Role = new Role(roleList[1])
            },
            new()
            {
                Id = 3,
                EmployeeId = testEmployee.Id,
                RoleId = roleList[2].Id,
                Employee = testEmployee,
                Role = new Role(roleList[2])
            }
        };

        _dbMock.Setup(e => e.EmployeeRole.GetAll(null))
               .ReturnsAsync(employeeRoleList.Select(e => e).ToList());

        var result = await _employeeRoleService.GetAllEmployeeRoles();

        Assert.NotNull(result);
        Assert.Equivalent(employeeRoleList.Select(e => e.ToDto()).ToList(), result);
    }

    [Fact]
    public async Task GetEmployeeRoleTest()
    {
        var testEmployee = EmployeeTestData.EmployeeOne;

        var roleList = new List<RoleDto>
        {
            new() {Id = 1, Description = "Admin" },
            new() { Id = 2, Description = "Manager" },
            new() { Id = 3, Description = "Employee" },
            new() { Id = 4, Description = "Intern" }
        };

        var employeeRoleList = new List<EmployeeRole>
        {
            new()
            {
                Id = 1,
                EmployeeId = testEmployee.Id,
                RoleId = roleList[0].Id,
                Employee = testEmployee,
                Role = new Role(roleList[0])
            }
        };

        Expression<Func<EmployeeRole, bool>>[] criteriaList =
        {
            e => e.Employee!.Email == employeeRoleList[0].Employee!.Email
        };

        _dbMock.SetupSequence(e => e.EmployeeRole.Get(It.IsAny<Expression<Func<EmployeeRole, bool>>>()))
               .Returns(employeeRoleList.ToMockIQueryable().Where(criteriaList[0]));

        var result1 = await _employeeRoleService.GetEmployeeRole(employeeRoleList[0].Employee!.Email!);

        Assert.Equivalent(employeeRoleList[0].ToDto(), result1);
    }

    [Fact]
    public async Task CheckEmployeeRoleTest()
    {
        const string email = "test@retrorabbit.co.za";

        var testEmployee = EmployeeTestData.EmployeeOne;

        var roleList = new List<RoleDto>
        {
            new() {Id = 1, Description = "Admin" },
            new() { Id = 2, Description = "Manager" },
            new() { Id = 3, Description = "Employee" },
            new() { Id = 4, Description = "Intern" }
        };

        var employeeRoleList = new List<EmployeeRole>
        {
            new()
            {
                Id = 1,
                EmployeeId = testEmployee.Id,
                RoleId = roleList[0].Id,
                Employee = testEmployee,
                Role = new Role(roleList[0])
            },
            new()
            {
                Id = 2,
                EmployeeId = testEmployee.Id,
                RoleId = roleList[1].Id,
                Employee = testEmployee,
                Role = new Role(roleList[1])
            },
            new()
            {
                Id = 3,
                EmployeeId = testEmployee.Id,
                RoleId = roleList[2].Id,
                Employee = testEmployee,
                Role = new Role(roleList[2])
            }
        };

        Expression<Func<EmployeeRole, bool>>[] criteriaList =
        {
            e => e.Employee!.Email == email && e.Role!.Description == roleList[0].Description,
            e => e.Employee!.Email == email && e.Role!.Description == roleList[1].Description,
            e => e.Employee!.Email == email && e.Role!.Description == roleList[2].Description,
            e => e.Employee!.Email == email && e.Role!.Description == roleList[3].Description
        };

        _dbMock.SetupSequence(e => e.EmployeeRole.Any(It.IsAny<Expression<Func<EmployeeRole, bool>>>()))
               .ReturnsAsync(employeeRoleList.ToMockIQueryable().Any(criteriaList[0]))
               .ReturnsAsync(employeeRoleList.ToMockIQueryable().Any(criteriaList[1]))
               .ReturnsAsync(employeeRoleList.ToMockIQueryable().Any(criteriaList[2]))
               .ReturnsAsync(employeeRoleList.ToMockIQueryable().Any(criteriaList[3]));

        var result1 = await _employeeRoleService.CheckEmployeeRole(email, roleList[0].Description!);
        var result2 = await _employeeRoleService.CheckEmployeeRole(email, roleList[1].Description!);
        var result3 = await _employeeRoleService.CheckEmployeeRole(email, roleList[2].Description!);
        var result4 = await _employeeRoleService.CheckEmployeeRole(email, roleList[3].Description!);

        Assert.True(result1);
        Assert.True(result2);
        Assert.True(result3);
        Assert.False(result4);
    }

    [Fact]
    public async Task GetAllEmployeeOnRolesTest()
    {
        var testEmployee = EmployeeTestData.EmployeeOne;

        var roleList = new List<RoleDto>
        {
             new() {Id = 1, Description = "Admin" },
             new() { Id = 2, Description = "Manager" },
             new() { Id = 3, Description = "Employee" },
             new() { Id = 4, Description = "Intern" }
        };

        var employeeRoleList = new EmployeeRole
        {
            Id = 1,
            EmployeeId = testEmployee.Id,
            RoleId = roleList[0].Id,
            Employee = testEmployee,
            Role = new Role(roleList[0])
        }.EntityToList();

        Expression<Func<EmployeeRole, bool>>[] criteriaList =
        {
            e => e.Role!.Id == employeeRoleList[0].Role!.Id
        };

        _dbMock.SetupSequence(e => e.EmployeeRole.Get(It.IsAny<Expression<Func<EmployeeRole, bool>>>()))
               .Returns(employeeRoleList.ToMockIQueryable().Where(criteriaList[0]));

        var result1 = await _employeeRoleService.GetAllEmployeeOnRoles(employeeRoleList[0].Role!.Id);

        Assert.Equivalent(employeeRoleList[0].ToDto(), result1[0]);
    }
}