using System.Linq.Expressions;
using HRIS.Models;
using HRIS.Models.Enums;
using HRIS.Services.Services;
using MockQueryable.Moq;
using Moq;
using RR.UnitOfWork;
using RR.UnitOfWork.Entities.HRIS;
using Xunit;

namespace RGO.Tests.Services;

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
        const string Email = "test@retrorabbit.co.za";

        EmployeeTypeDto employeeTypeDto = new(1, "Developer");
        var employeeAddressDto =
            new EmployeeAddressDto(1, "2", "Complex", "2", "Suburb/District", "City", "Country", "Province", "1620");

        EmployeeDto testEmployee = new(1, "001", "34434434", new DateTime(), new DateTime(),
                                       null, false, "None", 4, employeeTypeDto, "Notes", 1, 28, 128, 100000, "Dotty",
                                       "D",
                                       "Missile", new DateTime(), "South Africa", "South African", "0000080000000", " ",
                                       new DateTime(), null, Race.Black, Gender.Male, null,
                                       Email, "test.example@gmail.com", "0000000000", null, null, employeeAddressDto,
                                       employeeAddressDto, null, null, null);

        var roleList = new List<RoleDto>
        {
            new RoleDto{Id = 1, Description = "Admin" },
            new RoleDto { Id = 2, Description = "Manager" },
            new RoleDto { Id = 3, Description = "Employee" },
            new RoleDto { Id = 4, Description = "Intern" }
        };

        var employeeRoleList = new List<EmployeeRole>
        {
            new()
            {
                Id = 1,
                EmployeeId = testEmployee.Id,
                RoleId = roleList[0].Id,
                Employee = new Employee(testEmployee, employeeTypeDto),
                Role = new Role(roleList[0])
            },
            new()
            {
                Id = 2,
                EmployeeId = testEmployee.Id,
                RoleId = roleList[1].Id,
                Employee = new Employee(testEmployee, employeeTypeDto)
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

        Expression<Func<EmployeeRole, bool>>[] criteriAList =
        {
            e => e.Employee.Email == Email && e.Role.Description == roleList[0].Description,
            e => e.Employee.Email == Email && e.Role.Description == roleList[1].Description,
            e => e.Employee.Email == Email && e.Role.Description == roleList[2].Description,
            e => e.Employee.Email == Email && e.Role.Description == roleList[3].Description
        };

        _dbMock.SetupSequence(e => e.EmployeeRole.Any(It.IsAny<Expression<Func<EmployeeRole, bool>>>()))
               .Returns(Task.FromResult(true))
               .Returns(Task.FromResult(false))
               .Returns(Task.FromResult(false))
               .Returns(Task.FromResult(false))
               .Returns(Task.FromResult(false));

        _dbMock.SetupSequence(e => e.EmployeeRole.Add(It.IsAny<EmployeeRole>()))
               .Returns(Task.FromResult(employeeRoleList[0].ToDto()))
               .Returns(Task.FromResult(employeeRoleList[1].ToDto()))
               .Returns(Task.FromResult(employeeRoleList[2].ToDto()))
               .Returns(Task.FromResult(employeeRoleList[3].ToDto()));

        employeeRoleList[1].Role = new Role(roleList[1]);
        employeeRoleList[2].Employee = new Employee(testEmployee, employeeTypeDto);

        _dbMock.SetupSequence(e => e.EmployeeRole.GetById(It.IsAny<int>()))
               .Returns(Task.FromResult(employeeRoleList[1].ToDto()))
               .Returns(Task.FromResult(employeeRoleList[2].ToDto()))
               .Returns(Task.FromResult(employeeRoleList[3].ToDto()));

        await Assert.ThrowsAsync<Exception>(async () => await _employeeRoleService.SaveEmployeeRole(new EmployeeRoleDto
                                                (
                                                 employeeRoleList[0].Id,
                                                 employeeRoleList[0].Employee.ToDto(),
                                                 employeeRoleList[0].Role.ToDto()
                                                )));
        var result = await _employeeRoleService.SaveEmployeeRole(employeeRoleList[0].ToDto());
        Assert.Equivalent(employeeRoleList[0].ToDto(), result);
        result = await _employeeRoleService.SaveEmployeeRole(employeeRoleList[1].ToDto());
        Assert.Equivalent(employeeRoleList[1].ToDto(), result);
        result = await _employeeRoleService.SaveEmployeeRole(employeeRoleList[2].ToDto());
        Assert.Equivalent(employeeRoleList[2].ToDto(), result);
        Assert.ThrowsAsync<Exception>(() => _employeeRoleService.SaveEmployeeRole(employeeRoleList[3].ToDto()));
    }

    [Fact]
    public async Task DeleteEmployeeRoleTest()
    {
        const string Email = "test@retrorabbit.co.za";

        EmployeeTypeDto employeeTypeDto = new(1, "Developer");
        var employeeAddressDto =
            new EmployeeAddressDto(1, "2", "Complex", "2", "Suburb/District", "City", "Country", "Province", "1620");

        EmployeeDto testEmployee = new(1, "001", "34434434", new DateTime(), new DateTime(),
                                       null, false, "None", 4, employeeTypeDto, "Notes", 1, 28, 128, 100000, "Dotty",
                                       "D",
                                       "Missile", new DateTime(), "South Africa", "South African", "0000080000000", " ",
                                       new DateTime(), null, Race.Black, Gender.Male, null,
                                       Email, "test.example@gmail.com", "0000000000", null, null, employeeAddressDto,
                                       employeeAddressDto, null, null, null);

        var roleList = new List<RoleDto>
        {
            new RoleDto{Id = 1, Description = "Admin" },
            new RoleDto { Id = 2, Description = "Manager" },
            new RoleDto { Id = 3, Description = "Employee" },
            new RoleDto { Id = 4, Description = "Intern" }
        };

        var employeeRoleList = new List<EmployeeRole>
        {
            new()
            {
                Id = 1,
                EmployeeId = testEmployee.Id,
                RoleId = roleList[0].Id,
                Employee = new Employee(testEmployee, employeeTypeDto),
                Role = new Role(roleList[0])
            },
            new()
            {
                Id = 2,
                EmployeeId = testEmployee.Id,
                RoleId = roleList[1].Id,
                Employee = new Employee(testEmployee, employeeTypeDto),
                Role = new Role(roleList[1])
            },
            new()
            {
                Id = 3,
                EmployeeId = testEmployee.Id,
                RoleId = roleList[2].Id,
                Employee = new Employee(testEmployee, employeeTypeDto),
                Role = new Role(roleList[2])
            }
        };

        Expression<Func<EmployeeRole, bool>>[] criteriAList =
        {
            e => e.Employee.Email == Email && e.Role.Description == roleList[0].Description,
            e => e.Employee.Email == "" && e.Role.Description == ""
        };

        _dbMock.SetupSequence(e => e.EmployeeRole.Any(It.IsAny<Expression<Func<EmployeeRole, bool>>>()))
               .Returns(Task.FromResult(employeeRoleList.AsQueryable().BuildMock().Any(criteriAList[0])))
               .Returns(Task.FromResult(employeeRoleList.AsQueryable().BuildMock().Any(criteriAList[1])));

        _dbMock.Setup(e => e.EmployeeRole.Get(It.IsAny<Expression<Func<EmployeeRole, bool>>>()))
               .Returns(employeeRoleList.Where(e => e.Employee.Email == Email).AsQueryable().BuildMock());

        _dbMock.Setup(e => e.EmployeeRole.GetAll(It.IsAny<Expression<Func<EmployeeRole, bool>>>()))
               .Returns(Task.FromResult(employeeRoleList.Select(e => e.ToDto()).ToList()));

        _dbMock.SetupSequence(e => e.EmployeeRole.Delete(It.IsAny<int>()))
               .Returns(Task.FromResult(employeeRoleList.Where(e => e.Role.Description == roleList[0].Description)
                                                        .Select(e => e.ToDto()).FirstOrDefault()))
               .Returns(Task.FromResult(employeeRoleList.Where(e => e.Role.Description == roleList[1].Description)
                                                        .Select(e => e.ToDto()).FirstOrDefault()));

        var result1 = await _employeeRoleService.DeleteEmployeeRole(Email, roleList[0].Description);

        Assert.NotNull(result1);
        Assert.ThrowsAsync<Exception>(() => _employeeRoleService.DeleteEmployeeRole("", ""));
    }

    [Fact]
    public async Task UpdateEmployeeRoleTest()
    {
        const string Email = "test@retrorabbit.co.za";

        EmployeeTypeDto employeeTypeDto = new(1, "Developer");
        var employeeAddressDto =
            new EmployeeAddressDto(1, "2", "Complex", "2", "Suburb/District", "City", "Country", "Province", "1620");

        EmployeeDto testEmployee = new(1, "001", "34434434", new DateTime(), new DateTime(),
                                       null, false, "None", 4, employeeTypeDto, "Notes", 1, 28, 128, 100000, "Dotty",
                                       "D",
                                       "Missile", new DateTime(), "South Africa", "South African", "0000080000000", " ",
                                       new DateTime(), null, Race.Black, Gender.Male, null,
                                       Email, "test.example@gmail.com", "0000000000", null, null, employeeAddressDto,
                                       employeeAddressDto, null, null, null);

        var roleList = new List<RoleDto>
        {
            new RoleDto{Id = 1, Description = "Admin" },
            new RoleDto { Id = 2, Description = "Manager" },
            new RoleDto { Id = 3, Description = "Employee" },
            new RoleDto { Id = 4, Description = "Intern" }
        };

        var employeeRoleList = new List<EmployeeRole>
        {
            new()
            {
                Id = 1,
                EmployeeId = testEmployee.Id,
                RoleId = roleList[0].Id,
                Employee = new Employee(testEmployee, employeeTypeDto),
                Role = new Role(roleList[0])
            },
            new()
            {
                Id = 2,
                EmployeeId = testEmployee.Id,
                RoleId = roleList[1].Id,
                Employee = new Employee(testEmployee, employeeTypeDto),
                Role = new Role(roleList[1])
            },
            new()
            {
                Id = 3,
                EmployeeId = testEmployee.Id,
                RoleId = roleList[2].Id,
                Employee = new Employee(testEmployee, employeeTypeDto),
                Role = new Role(roleList[2])
            }
        };

        Expression<Func<EmployeeRole, bool>>[] criteriAList =
        {
            e => e.Employee.Email == Email && e.Role.Description == roleList[0].Description,
            e => e.Employee.Email == Email && e.Role.Description == roleList[1].Description,
            e => e.Employee.Email == Email && e.Role.Description == roleList[2].Description,
            e => e.Employee.Email == Email && e.Role.Description == "Made up Role"
        };

        _dbMock.SetupSequence(e => e.EmployeeRole.Any(It.IsAny<Expression<Func<EmployeeRole, bool>>>()))
               .Returns(Task.FromResult(employeeRoleList.AsQueryable().BuildMock().Any(criteriAList[0])))
               .Returns(Task.FromResult(employeeRoleList.AsQueryable().BuildMock().Any(criteriAList[1])))
               .Returns(Task.FromResult(employeeRoleList.AsQueryable().BuildMock().Any(criteriAList[2])))
               .Returns(Task.FromResult(employeeRoleList.AsQueryable().BuildMock().Any(criteriAList[3])));

        _dbMock.SetupSequence(e => e.EmployeeRole.Update(It.IsAny<EmployeeRole>()))
               .Returns(Task.FromResult(employeeRoleList[0].ToDto()))
               .Returns(Task.FromResult(employeeRoleList[1].ToDto()))
               .Returns(Task.FromResult(employeeRoleList[2].ToDto()));

        var result1 = await _employeeRoleService.UpdateEmployeeRole(employeeRoleList[0].ToDto());
        var result2 = await _employeeRoleService.UpdateEmployeeRole(employeeRoleList[1].ToDto());
        var result3 = await _employeeRoleService.UpdateEmployeeRole(employeeRoleList[2].ToDto());

        Assert.Equivalent(employeeRoleList[0].ToDto(), result1);
        Assert.Equivalent(employeeRoleList[1].ToDto(), result2);
        Assert.Equivalent(employeeRoleList[2].ToDto(), result3);

        _ = Assert.ThrowsAsync<Exception>(() => _employeeRoleService.UpdateEmployeeRole(new EmployeeRoleDto
                                          (
                                           4,
                                           employeeRoleList[0].Employee.ToDto(),
                                           new RoleDto { Id = 2, Description = "Made up Role" })));
    }

    [Fact]
    public async Task GetAllEmployeeRoles()
    {
        const string Email = "test@retrorabbit.co.za";

        EmployeeTypeDto employeeTypeDto = new(1, "Developer");
        var employeeAddressDto =
            new EmployeeAddressDto(1, "2", "Complex", "2", "Suburb/District", "City", "Country", "Province", "1620");

        EmployeeDto testEmployee = new(1, "001", "34434434", new DateTime(), new DateTime(),
                                       null, false, "None", 4, employeeTypeDto, "Notes", 1, 28, 128, 100000, "Dotty",
                                       "D",
                                       "Missile", new DateTime(), "South Africa", "South African", "0000080000000", " ",
                                       new DateTime(), null, Race.Black, Gender.Male, null,
                                       Email, "test.example@gmail.com", "0000000000", null, null, employeeAddressDto,
                                       employeeAddressDto, null, null, null);

        var roleList = new List<RoleDto>
        {
             new RoleDto{Id = 1, Description = "Admin" },
            new RoleDto { Id = 2, Description = "Manager" },
            new RoleDto { Id = 3, Description = "Employee" },
            new RoleDto { Id = 4, Description = "Intern" }
        };

        var employeeRoleList = new List<EmployeeRole>
        {
            new()
            {
                Id = 1,
                EmployeeId = testEmployee.Id,
                RoleId = roleList[0].Id,
                Employee = new Employee(testEmployee, employeeTypeDto),
                Role = new Role(roleList[0])
            },
            new()
            {
                Id = 2,
                EmployeeId = testEmployee.Id,
                RoleId = roleList[1].Id,
                Employee = new Employee(testEmployee, employeeTypeDto),
                Role = new Role(roleList[1])
            },
            new()
            {
                Id = 3,
                EmployeeId = testEmployee.Id,
                RoleId = roleList[2].Id,
                Employee = new Employee(testEmployee, employeeTypeDto),
                Role = new Role(roleList[2])
            }
        };

        Expression<Func<EmployeeRole, bool>>[] criteriAList =
        {
            e => e.Employee.Email == Email && e.Role.Description == roleList[0].Description,
            e => e.Employee.Email == Email && e.Role.Description == roleList[1].Description,
            e => e.Employee.Email == Email && e.Role.Description == roleList[2].Description,
            e => e.Employee.Email == Email && e.Role.Description == roleList[3].Description
        };

        _dbMock.Setup(e => e.EmployeeRole.GetAll(null))
               .Returns(Task.FromResult(employeeRoleList.Select(e => e.ToDto()).ToList()));

        var result = await _employeeRoleService.GetAllEmployeeRoles();

        Assert.NotNull(result);
        Assert.Equivalent(employeeRoleList.Select(e => e.ToDto()).ToList(), result);
    }

    [Fact]
    public async Task GetEmployeeRoleTest()
    {
        const string Email = "test@retrorabbit.co.za";

        EmployeeTypeDto employeeTypeDto = new(1, "Developer");
        var employeeAddressDto =
            new EmployeeAddressDto(1, "2", "Complex", "2", "Suburb/District", "City", "Country", "Province", "1620");

        EmployeeDto testEmployee = new(1, "001", "34434434", new DateTime(), new DateTime(),
                                       null, false, "None", 4, employeeTypeDto, "Notes", 1, 28, 128, 10000, "Dotty",
                                       "D",
                                       "Missile", new DateTime(), "South Africa", "South African", "0000080000000", " ",
                                       new DateTime(), null, Race.Black, Gender.Male, null,
                                       Email, "test.example@gmail.com", "0000000000", null, null, employeeAddressDto,
                                       employeeAddressDto, null, null, null);

        var roleList = new List<RoleDto>
        {
             new RoleDto{Id = 1, Description = "Admin" },
            new RoleDto { Id = 2, Description = "Manager" },
            new RoleDto { Id = 3, Description = "Employee" },
            new RoleDto { Id = 4, Description = "Intern" }
        };

        var employeeRoleList = new List<EmployeeRole>
        {
            new()
            {
                Id = 1,
                EmployeeId = testEmployee.Id,
                RoleId = roleList[0].Id,
                Employee = new Employee(testEmployee, employeeTypeDto),
                Role = new Role(roleList[0])
            }
        };

        Expression<Func<EmployeeRole, bool>>[] criteriAList =
        {
            e => e.Employee.Email == employeeRoleList[0].Employee.Email
        };

        _dbMock.SetupSequence(e => e.EmployeeRole.Get(It.IsAny<Expression<Func<EmployeeRole, bool>>>()))
               .Returns(employeeRoleList.AsQueryable().BuildMock().Where(criteriAList[0]));

        var result1 = await _employeeRoleService.GetEmployeeRole(employeeRoleList[0].Employee.Email);

        Assert.Equivalent(employeeRoleList[0].ToDto(), result1);
    }

    [Fact]
    public async Task CheckEmployeeRoleTest()
    {
        const string Email = "test@retrorabbit.co.za";

        EmployeeTypeDto employeeTypeDto = new(1, "Developer");
        var employeeAddressDto =
            new EmployeeAddressDto(1, "2", "Complex", "2", "Suburb/District", "City", "Country", "Province", "1620");

        EmployeeDto testEmployee = new(1, "001", "34434434", new DateTime(), new DateTime(),
                                       null, false, "None", 4, employeeTypeDto, "Notes", 1, 28, 128, 100000, "Dotty",
                                       "D",
                                       "Missile", new DateTime(), "South Africa", "South African", "0000080000000", " ",
                                       new DateTime(), null, Race.Black, Gender.Male, null,
                                       Email, "test.example@gmail.com", "0000000000", null, null, employeeAddressDto,
                                       employeeAddressDto, null, null, null);

        var roleList = new List<RoleDto>
        {
             new RoleDto{Id = 1, Description = "Admin" },
             new RoleDto { Id = 2, Description = "Manager" },
             new RoleDto { Id = 3, Description = "Employee" },
             new RoleDto { Id = 4, Description = "Intern" }
        };

        var employeeRoleList = new List<EmployeeRole>
        {
            new()
            {
                Id = 1,
                EmployeeId = testEmployee.Id,
                RoleId = roleList[0].Id,
                Employee = new Employee(testEmployee, employeeTypeDto),
                Role = new Role(roleList[0])
            },
            new()
            {
                Id = 2,
                EmployeeId = testEmployee.Id,
                RoleId = roleList[1].Id,
                Employee = new Employee(testEmployee, employeeTypeDto),
                Role = new Role(roleList[1])
            },
            new()
            {
                Id = 3,
                EmployeeId = testEmployee.Id,
                RoleId = roleList[2].Id,
                Employee = new Employee(testEmployee, employeeTypeDto),
                Role = new Role(roleList[2])
            }
        };

        Expression<Func<EmployeeRole, bool>>[] criteriAList =
        {
            e => e.Employee.Email == Email && e.Role.Description == roleList[0].Description,
            e => e.Employee.Email == Email && e.Role.Description == roleList[1].Description,
            e => e.Employee.Email == Email && e.Role.Description == roleList[2].Description,
            e => e.Employee.Email == Email && e.Role.Description == roleList[3].Description
        };

        _dbMock.SetupSequence(e => e.EmployeeRole.Any(It.IsAny<Expression<Func<EmployeeRole, bool>>>()))
               .Returns(Task.FromResult(employeeRoleList.AsQueryable().BuildMock().Any(criteriAList[0])))
               .Returns(Task.FromResult(employeeRoleList.AsQueryable().BuildMock().Any(criteriAList[1])))
               .Returns(Task.FromResult(employeeRoleList.AsQueryable().BuildMock().Any(criteriAList[2])))
               .Returns(Task.FromResult(employeeRoleList.AsQueryable().BuildMock().Any(criteriAList[3])));

        var result1 = await _employeeRoleService.CheckEmployeeRole(Email, roleList[0].Description);
        var result2 = await _employeeRoleService.CheckEmployeeRole(Email, roleList[1].Description);
        var result3 = await _employeeRoleService.CheckEmployeeRole(Email, roleList[2].Description);
        var result4 = await _employeeRoleService.CheckEmployeeRole(Email, roleList[3].Description);

        Assert.True(result1);
        Assert.True(result2);
        Assert.True(result3);
        Assert.False(result4);
    }

    [Fact]
    public async Task GetAllEmployeeOnRolesTest()
    {
        const string Email = "test@retrorabbit.co.za";

        EmployeeTypeDto employeeTypeDto = new(1, "Developer");
        var employeeAddressDto =
            new EmployeeAddressDto(1, "2", "Complex", "2", "Suburb/District", "City", "Country", "Province", "1620");

        EmployeeDto testEmployee = new(1, "001", "34434434", new DateTime(), new DateTime(),
                                       null, false, "None", 4, employeeTypeDto, "Notes", 1, 28, 128, 100000, "Dotty",
                                       "D",
                                       "Missile", new DateTime(), "South Africa", "South African", "0000080000000", " ",
                                       new DateTime(), null, Race.Black, Gender.Male, null,
                                       Email, "test.example@gmail.com", "0000000000", null, null, employeeAddressDto,
                                       employeeAddressDto, null, null, null);

        var roleList = new List<RoleDto>
        {
             new RoleDto{Id = 1, Description = "Admin" },
             new RoleDto { Id = 2, Description = "Manager" },
             new RoleDto { Id = 3, Description = "Employee" },
             new RoleDto { Id = 4, Description = "Intern" }
        };

        var employeeRoleList = new List<EmployeeRole>
        {
            new()
            {
                Id = 1,
                EmployeeId = testEmployee.Id,
                RoleId = roleList[0].Id,
                Employee = new Employee(testEmployee, employeeTypeDto),
                Role = new Role(roleList[0])
            }
        };

        Expression<Func<EmployeeRole, bool>>[] criteriAList =
        {
            e => e.Role.Id == employeeRoleList[0].Role.Id
        };

        _dbMock.SetupSequence(e => e.EmployeeRole.Get(It.IsAny<Expression<Func<EmployeeRole, bool>>>()))
               .Returns(employeeRoleList.AsQueryable().BuildMock().Where(criteriAList[0]));

        var result1 = await _employeeRoleService.GetAllEmployeeOnRoles(employeeRoleList[0].Role.Id);

        Assert.Equivalent(employeeRoleList[0].ToDto(), result1[0]);
    }
}