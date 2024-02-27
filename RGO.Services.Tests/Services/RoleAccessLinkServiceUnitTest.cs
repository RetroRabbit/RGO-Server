using System.Linq.Expressions;
using HRIS.Models;
using HRIS.Models.Enums;
using HRIS.Services.Interfaces;
using HRIS.Services.Services;
using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
using RR.UnitOfWork;
using RR.UnitOfWork.Entities.HRIS;
using Xunit;

namespace RGO.Tests.Services;

public class RoleAccessLinkServiceUnitTest
{
    private readonly Mock<IUnitOfWork> _dbMock;
    private readonly Mock<IEmployeeRoleService> _employeeRoleServiceMock;
    private readonly RoleAccessDto _roleAccessDto;
    private readonly RoleAccessLinkDto _roleAccessLinkDto;
    private readonly RoleAccessLinkService _roleAccessLinkService;
    private readonly RoleDto _roleDto;

    public RoleAccessLinkServiceUnitTest()
    {
        _dbMock = new Mock<IUnitOfWork>();
        _employeeRoleServiceMock = new Mock<IEmployeeRoleService>();
        _roleAccessLinkService = new RoleAccessLinkService(_dbMock.Object, _employeeRoleServiceMock.Object);
        _roleDto = new RoleDto {Id = 0, Description = "Employee"};
        _roleAccessDto = new RoleAccessDto(1, "ViewEmployee", "Employee Data");
        _roleAccessLinkDto = new RoleAccessLinkDto(1, _roleDto, _roleAccessDto);
    }

    [Fact]
    public async Task SaveTest()
    {
        _dbMock
            .Setup(r => r.RoleAccessLink.Add(It.IsAny<RoleAccessLink>()))
            .Returns(Task.FromResult(_roleAccessLinkDto));

        var result = await _roleAccessLinkService.Save(_roleAccessLinkDto);

        Assert.NotNull(result);
        Assert.Equal(_roleAccessLinkDto, result);
        _dbMock.Verify(r => r.RoleAccessLink.Add(It.IsAny<RoleAccessLink>()), Times.Once);
    }

    [Fact]
    public async Task DeleteTest()
    {
        var roleAccessLinks = new List<RoleAccessLink>
        {
            new()
            {
                Id = 1, Role = new Role { Id = 1, Description = "Employee" },
                RoleAccess = new RoleAccess { Id = 1, Permission = "ViewEmployee" }
            },
            new()
            {
                Id = 2, Role = new Role { Id = 1, Description = "Employee" },
                RoleAccess = new RoleAccess { Id = 2, Permission = "EditEmployee" }
            },
            new()
            {
                Id = 3, Role = new Role { Id = 2, Description = "Manager" },
                RoleAccess = new RoleAccess { Id = 1, Permission = "ViewEmployee" }
            },
            new()
            {
                Id = 4, Role = new Role { Id = 2, Description = "Manager" },
                RoleAccess = new RoleAccess { Id = 2, Permission = "EditEmployee" }
            }
        }.AsQueryable().BuildMock();

        var randLink = roleAccessLinks
                       .Where(r => r.Id == 2)
                       .Select(r => r.ToDto())
                       .First();

        Expression<Func<RoleAccessLink, bool>> criteria = r =>
            r.Role.Description == randLink.Role.Description &&
            r.RoleAccess.Permission == randLink.RoleAccess.Permission;

        var expect = await roleAccessLinks
                           .Where(criteria)
                           .Select(r => r.ToDto())
                           .FirstAsync();

        var malformed = await roleAccessLinks
                              .Where(criteria)
                              .Select(r => new RoleAccessLinkDto(r.Id, null, null))
                              .FirstAsync();

        _dbMock
            .SetupSequence(r => r.RoleAccessLink.Get(It.IsAny<Expression<Func<RoleAccessLink, bool>>>()))
            .Returns(roleAccessLinks.Where(criteria))
            .Returns(roleAccessLinks.Where(criteria));

        _dbMock
            .SetupSequence(r => r.RoleAccessLink.Delete(It.IsAny<int>()))
            .Returns(Task.FromResult(expect))
            .Returns(Task.FromResult(malformed));

        var result1 = await _roleAccessLinkService.Delete(_roleDto.Description, _roleAccessDto.Permission);

        Assert.NotNull(result1);
        Assert.Equal(expect.Id, result1.Id);

        var result2 = await _roleAccessLinkService.Delete(_roleDto.Description, _roleAccessDto.Permission);

        Assert.NotNull(result2.RoleAccess);
        Assert.NotNull(result2.Role);
        Assert.Equal(expect.Id, result2.Id);
    }

    [Fact]
    public async Task GetAllTest()
    {
        var roleAccessLinks = new List<RoleAccessLink>
        {
            new()
            {
                Id = 1, Role = new Role { Id = 1, Description = "Employee" },
                RoleAccess = new RoleAccess { Id = 1, Permission = "ViewEmployee" }
            },
            new()
            {
                Id = 2, Role = new Role { Id = 1, Description = "Employee" },
                RoleAccess = new RoleAccess { Id = 2, Permission = "EditEmployee" }
            },
            new()
            {
                Id = 3, Role = new Role { Id = 2, Description = "Manager" },
                RoleAccess = new RoleAccess { Id = 1, Permission = "ViewEmployee" }
            },
            new()
            {
                Id = 4, Role = new Role { Id = 2, Description = "Manager" },
                RoleAccess = new RoleAccess { Id = 2, Permission = "EditEmployee" }
            }
        }.AsQueryable().BuildMock();

        var expect = await roleAccessLinks
                           .GroupBy(r => r.Role.Description)
                           .ToDictionaryAsync(
                                              group => group.Key,
                                              group => group.Select(r => r.RoleAccess.Permission).ToList());

        _dbMock
            .Setup(r => r.RoleAccessLink.Get(null))
            .Returns(roleAccessLinks);

        var result = await _roleAccessLinkService.GetAll();

        Assert.Equal(2, result.Count);
        Assert.Equal(expect.Keys, result.Keys);
        Assert.Equal(expect.Values, result.Values);
        _dbMock.Verify(r => r.RoleAccessLink.Get(null), Times.Once);
    }

    [Fact]
    public async Task GetByRoleTest()
    {
        var roleAccessLinks = new List<RoleAccessLink>
        {
            new()
            {
                Id = 1, Role = new Role { Id = 1, Description = "Employee" },
                RoleAccess = new RoleAccess { Id = 1, Permission = "ViewEmployee" }
            },
            new()
            {
                Id = 2, Role = new Role { Id = 1, Description = "Employee" },
                RoleAccess = new RoleAccess { Id = 2, Permission = "EditEmployee" }
            },
            new()
            {
                Id = 3, Role = new Role { Id = 2, Description = "Manager" },
                RoleAccess = new RoleAccess { Id = 1, Permission = "ViewEmployee" }
            },
            new()
            {
                Id = 4, Role = new Role { Id = 2, Description = "Manager" },
                RoleAccess = new RoleAccess { Id = 2, Permission = "EditEmployee" }
            }
        }.AsQueryable().BuildMock();

        var randLink = roleAccessLinks
                       .Where(r => r.Role.Id == 1)
                       .Select(r => r.ToDto())
                       .First();

        Expression<Func<RoleAccessLink, bool>> criteria = r =>
            r.Role.Description == randLink.Role.Description;

        var expect = await roleAccessLinks
                           .Where(criteria)
                           .GroupBy(r => r.Role.Description)
                           .ToDictionaryAsync(
                                              group => group.Key,
                                              group => group.Select(r => r.RoleAccess.Permission).ToList());

        _dbMock
            .Setup(r => r.RoleAccessLink.Get(It.IsAny<Expression<Func<RoleAccessLink, bool>>>()))
            .Returns(roleAccessLinks.Where(criteria));

        var result = await _roleAccessLinkService.GetByRole(randLink.Role.Description);

        Assert.Equal(2, result.First().Value.Count);
        Assert.Equal(expect.Keys, result.Keys);
        Assert.Equal(expect.Values, result.Values);
        _dbMock.Verify(r => r.RoleAccessLink.Get(It.IsAny<Expression<Func<RoleAccessLink, bool>>>()), Times.Once);
    }

    [Fact]
    public async Task GetBypermission()
    {
        var roleAccessLinks = new List<RoleAccessLink>
        {
            new()
            {
                Id = 1, Role = new Role { Id = 1, Description = "Employee" },
                RoleAccess = new RoleAccess { Id = 1, Permission = "ViewEmployee" }
            },
            new()
            {
                Id = 2, Role = new Role { Id = 1, Description = "Employee" },
                RoleAccess = new RoleAccess { Id = 2, Permission = "EditEmployee" }
            },
            new()
            {
                Id = 3, Role = new Role { Id = 2, Description = "Manager" },
                RoleAccess = new RoleAccess { Id = 1, Permission = "ViewEmployee" }
            },
            new()
            {
                Id = 4, Role = new Role { Id = 2, Description = "Manager" },
                RoleAccess = new RoleAccess { Id = 2, Permission = "EditEmployee" }
            }
        }.AsQueryable().BuildMock();

        var randLink = roleAccessLinks
                       .Where(r => r.Role.Id == 1)
                       .Select(r => r.ToDto())
                       .First();

        Expression<Func<RoleAccessLink, bool>> criteria = r =>
            r.RoleAccess.Permission == randLink.RoleAccess.Permission;

        var expect = await roleAccessLinks
                           .Where(criteria)
                           .GroupBy(r => r.Role.Description)
                           .ToDictionaryAsync(
                                              group => group.Key,
                                              group => group.Select(r => r.RoleAccess.Permission).ToList());

        _dbMock
            .Setup(r => r.RoleAccessLink.Get(It.IsAny<Expression<Func<RoleAccessLink, bool>>>()))
            .Returns(roleAccessLinks.Where(criteria));

        var result = await _roleAccessLinkService.GetByPermission(randLink.RoleAccess.Permission);

        Assert.Equal(2, result.Count);
        Assert.Equal(expect.Keys, result.Keys);
        Assert.Equal(expect.Values, result.Values);
        _dbMock.Verify(r => r.RoleAccessLink.Get(It.IsAny<Expression<Func<RoleAccessLink, bool>>>()), Times.Once);
    }

    [Fact]
    public async Task GetRoleByEmployeeTest()
    {
        var email = "test@retrorabbit.co.za";
        var employeeTypeDto = new EmployeeTypeDto(1, "Developer");
        var employeeAddressDto =
            new EmployeeAddressDto(1, "2", "Complex", "2", "Suburb/District", "City", "Country", "Province", "1620");
        var testEmployee = new EmployeeDto(1, "001", "34434434", new DateTime(), new DateTime(),
                                           null, false, "None", 4, employeeTypeDto, "Notes", 1, 28, 128, 100000, "Test",
                                           "TD",
                                           "Dummy", new DateTime(), "South Africa", "South African", "0000000000000",
                                           " ",
                                           new DateTime(), null, Race.Black, Gender.Female, null,
                                           email, "tdummy@gmail.com", "0123456789", null, null, employeeAddressDto,
                                           employeeAddressDto, null, null, null);

        var employeeRoleDtos = new List<EmployeeRoleDto>
        {
            new(
                1,
                testEmployee,
                new RoleDto{ Id = 0, Description = "Employee" }),
            new(
                2,
                testEmployee,
                new RoleDto{Id = 0, Description = "Manager" }),
        };

        _employeeRoleServiceMock
            .Setup(r => r.GetEmployeeRoles(email))
            .ReturnsAsync(employeeRoleDtos);

        var rolesAssignedToEmplyee = employeeRoleDtos
                                     .Select(r => r.Role.Description)
                                     .ToList();

        var roleAccessLinks = new List<RoleAccessLink>
        {
            new()
            {
                Id = 1, Role = new Role { Id = 1, Description = "Employee" },
                RoleAccess = new RoleAccess { Id = 1, Permission = "ViewEmployee" }
            },
            new()
            {
                Id = 2, Role = new Role { Id = 1, Description = "Employee" },
                RoleAccess = new RoleAccess { Id = 2, Permission = "EditEmployee" }
            },
            new()
            {
                Id = 3, Role = new Role { Id = 2, Description = "Manager" },
                RoleAccess = new RoleAccess { Id = 3, Permission = "Oversee" }
            },
            new()
            {
                Id = 4, Role = new Role { Id = 2, Description = "Manager" },
                RoleAccess = new RoleAccess { Id = 4, Permission = "CalculateKPI" }
            }
        }.AsQueryable().BuildMock();

        var criteria = roleAccessLinks
                       .GroupBy(r => r.Role.Description)
                       .Select(r => r.Key)
                       .Distinct()
                       .Select(r => (Expression<Func<RoleAccessLink, bool>>)(x => x.Role.Description == r))
                       .ToList();

        var returns = _dbMock
                      .SetupSequence(r => r.RoleAccessLink.Get(It.IsAny<Expression<Func<RoleAccessLink, bool>>>()))
                      .Returns(roleAccessLinks.Where(criteria[0]));

        for (var i = 1; i < criteria.Count; i++)
            returns = returns.Returns(roleAccessLinks.Where(criteria[i]));

        IEnumerable<Dictionary<string, List<string>>> accessRoles = new List<Dictionary<string, List<string>>>
        {
            new()
            {
                { "Employee", new List<string> { "ViewEmployee", "EditEmployee" } }
            },
            new()
            {
                { "Manager", new List<string> { "Oversee", "CalculateKPI" } }
            }
        };

        var merged = new Dictionary<string, List<string>>
        {
            { "Employee", new List<string> { "ViewEmployee", "EditEmployee" } },
            { "Manager", new List<string> { "Oversee", "CalculateKPI" } }
        };

        var result = await _roleAccessLinkService.GetRoleByEmployee(email);

        Assert.Equal(merged.Keys, result.Keys);
        Assert.Equal(merged.Values, result.Values);
    }

    [Fact]
    public async Task Update()
    {
        var roleAccessLinkToUpdate = new RoleAccessLinkDto(
                                                           _roleAccessLinkDto.Id,
                                                           new RoleDto { Id = 0, Description = "Employee"},
                                                           new RoleAccessDto(2, "EditEmployee", "Employee Data"));

        _dbMock
            .Setup(r => r.RoleAccessLink.Update(It.IsAny<RoleAccessLink>()))
            .Returns(Task.FromResult(_roleAccessLinkDto));

        var result = await _roleAccessLinkService.Update(roleAccessLinkToUpdate);

        Assert.NotNull(result);
        Assert.NotNull(result.Role);
        Assert.NotNull(result.RoleAccess);
        Assert.Equal(_roleAccessLinkDto, result);
        _dbMock.Verify(r => r.RoleAccessLink.Update(It.IsAny<RoleAccessLink>()), Times.Once);
    }

    [Fact]
    public async Task GetAllRoleAccessLink_ReturnsRoleAccessLinks()
    {
        var roleAccessLinkData = new List<RoleAccessLink>
        {
            new()
            {
                Id = 1, Role = new Role { Id = 1, Description = "Employee" },
                RoleAccess = new RoleAccess { Id = 1, Permission = "ViewEmployee" }
            },
            new()
            {
                Id = 2, Role = new Role { Id = 1, Description = "Employee" },
                RoleAccess = new RoleAccess { Id = 2, Permission = "EditEmployee" }
            }
        }.AsQueryable().BuildMock();

        _dbMock
            .Setup(r => r.RoleAccessLink.Get(It.IsAny<Expression<Func<RoleAccessLink, bool>>>()))
            .Returns(roleAccessLinkData);

        var employeeEvaluationTemplateItems = await _roleAccessLinkService.GetAllRoleAccessLink();

        Assert.Equal("Employee", employeeEvaluationTemplateItems[0].Role.Description);
        Assert.Equal("ViewEmployee", employeeEvaluationTemplateItems[0].RoleAccess.Permission);


        Assert.Equal("Employee", employeeEvaluationTemplateItems[1].Role.Description);
        Assert.Equal("EditEmployee", employeeEvaluationTemplateItems[1].RoleAccess.Permission);
    }
}