using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
using RGO.Models;
using RGO.Services.Interfaces;
using RGO.Services.Services;
using RGO.UnitOfWork;
using RGO.UnitOfWork.Entities;
using System.Linq.Expressions;
using Xunit;

namespace RGO.Services.Tests.Services;

public class RoleAccessLinkServiceUnitTest
{
    private readonly Mock<IUnitOfWork> _dbMock;
    private readonly Mock<IEmployeeRoleService> _employeeRoleServiceMock;
    private readonly RoleAccessLinkService _roleAccessLinkService;
    private readonly RoleDto _roleDto;
    private readonly RoleAccessDto _roleAccessDto;
    private readonly RoleAccessLinkDto _roleAccessLinkDto;

    public RoleAccessLinkServiceUnitTest()
    {
        _dbMock = new Mock<IUnitOfWork>();
        _employeeRoleServiceMock = new Mock<IEmployeeRoleService>();
        _roleAccessLinkService = new RoleAccessLinkService(_dbMock.Object, _employeeRoleServiceMock.Object);
        _roleDto = new RoleDto(1, "Employee");
        _roleAccessDto = new RoleAccessDto(1, "ViewEmployee");
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
            new RoleAccessLink { Id = 1, Role = new Role { Id = 1, Description = "Employee" }, RoleAccess = new RoleAccess { Id = 1, Permission = "ViewEmployee" } },
            new RoleAccessLink { Id = 2, Role = new Role { Id = 1, Description = "Employee" }, RoleAccess = new RoleAccess { Id = 2, Permission = "EditEmployee" } },
            new RoleAccessLink { Id = 3, Role = new Role { Id = 2, Description = "Manager" }, RoleAccess = new RoleAccess { Id = 1, Permission = "ViewEmployee" } },
            new RoleAccessLink { Id = 4, Role = new Role { Id = 2, Description = "Manager" }, RoleAccess = new RoleAccess { Id = 2, Permission = "EditEmployee" } },
        }.AsQueryable().BuildMock();

        var randLink = roleAccessLinks
            .Where(r => r.Id == new Random().Next(1, 4))
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
            new RoleAccessLink { Id = 1, Role = new Role { Id = 1, Description = "Employee" }, RoleAccess = new RoleAccess { Id = 1, Permission = "ViewEmployee" } },
            new RoleAccessLink { Id = 2, Role = new Role { Id = 1, Description = "Employee" }, RoleAccess = new RoleAccess { Id = 2, Permission = "EditEmployee" } },
            new RoleAccessLink { Id = 3, Role = new Role { Id = 2, Description = "Manager" }, RoleAccess = new RoleAccess { Id = 1, Permission = "ViewEmployee" } },
            new RoleAccessLink { Id = 4, Role = new Role { Id = 2, Description = "Manager" }, RoleAccess = new RoleAccess { Id = 2, Permission = "EditEmployee" } },
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
            new RoleAccessLink { Id = 1, Role = new Role { Id = 1, Description = "Employee" }, RoleAccess = new RoleAccess { Id = 1, Permission = "ViewEmployee" } },
            new RoleAccessLink { Id = 2, Role = new Role { Id = 1, Description = "Employee" }, RoleAccess = new RoleAccess { Id = 2, Permission = "EditEmployee" } },
            new RoleAccessLink { Id = 3, Role = new Role { Id = 2, Description = "Manager" }, RoleAccess = new RoleAccess { Id = 1, Permission = "ViewEmployee" } },
            new RoleAccessLink { Id = 4, Role = new Role { Id = 2, Description = "Manager" }, RoleAccess = new RoleAccess { Id = 2, Permission = "EditEmployee" } },
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

        Assert.Equal(1, result.Count);
        Assert.Equal(expect.Keys, result.Keys);
        Assert.Equal(expect.Values, result.Values);
        _dbMock.Verify(r => r.RoleAccessLink.Get(It.IsAny<Expression<Func<RoleAccessLink, bool>>>()), Times.Once);
    }

    [Fact]
    public async Task GetRoleByEmployeeTest()
    {
        string email = "test@retrorabbit.co.za";
        var employeeTypeDto = new EmployeeTypeDto(1, "Developer");
        EmployeeDto testEmployee = new EmployeeDto(1, "001", "34434434", new DateOnly(), new DateOnly(),
                        null, false, "None", 4, employeeTypeDto, "Notes", 1, 28, 128, 100000, "Ms", "Test", "TD",
                        "Dummy", new DateOnly(), "South Africa", "South African", "9708180344086", " ",
                        new DateOnly(), null, Models.Enums.Race.Black, Models.Enums.Gender.Female, null,
                        email, "tdummy@gmail.com", "0858748117");

        var employeeRoleDtos = new List<EmployeeRoleDto>
        {
            new EmployeeRoleDto
            (
                1,
                testEmployee,
                new RoleDto(1, "Employee")),
            new EmployeeRoleDto
            (
                2,
                testEmployee,
                new RoleDto(2, "Manager"))
        };

        _employeeRoleServiceMock
            .Setup(r => r.GetEmployeeRoles(email))
            .ReturnsAsync(employeeRoleDtos);

        var rolesAssignedToEmplyee = employeeRoleDtos
            .Select(r => r.Role.Description)
            .ToList();

        var roleAccessLinks = new List<RoleAccessLink>
        {
            new RoleAccessLink { Id = 1, Role = new Role { Id = 1, Description = "Employee" }, RoleAccess = new RoleAccess { Id = 1, Permission = "ViewEmployee" } },
            new RoleAccessLink { Id = 2, Role = new Role { Id = 1, Description = "Employee" }, RoleAccess = new RoleAccess { Id = 2, Permission = "EditEmployee" } },
            new RoleAccessLink { Id = 3, Role = new Role { Id = 2, Description = "Manager" }, RoleAccess = new RoleAccess { Id = 3, Permission = "Oversee" } },
            new RoleAccessLink { Id = 4, Role = new Role { Id = 2, Description = "Manager" }, RoleAccess = new RoleAccess { Id = 4, Permission = "CalculateKPI" } },
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

        for (int i = 1; i < criteria.Count; i++)
            returns = returns.Returns(roleAccessLinks.Where(criteria[i]));

        IEnumerable<Dictionary<string, List<string>>> accessRoles = new List<Dictionary<string, List<string>>>
        {
            new Dictionary<string, List<string>>
            {
                { "Employee", new List<string> { "ViewEmployee", "EditEmployee" } }
            },
            new Dictionary<string, List<string>>
            {
                { "Manager", new List<string> { "Oversee", "CalculateKPI" } }
            }
        };

        Dictionary<string, List<string>> merged = new Dictionary<string, List<string>>()
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
            new RoleDto(1, "Employee"),
            new RoleAccessDto(2, "EditEmployee"));

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
}