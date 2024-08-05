using System.Linq.Expressions;
using HRIS.Models;
using HRIS.Services.Interfaces;
using HRIS.Services.Services;
using Microsoft.EntityFrameworkCore;
using Moq;
using RR.Tests.Data;
using RR.Tests.Data.Models.HRIS;
using RR.UnitOfWork;
using RR.UnitOfWork.Entities.HRIS;
using Xunit;

namespace HRIS.Services.Tests.Services;

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
        _roleAccessLinkService = new RoleAccessLinkService(_dbMock.Object, _employeeRoleServiceMock.Object, new AuthorizeIdentityMock("test@gmail.com", "test", "Admin", 1));
        _roleDto = new RoleDto { Id = 1, Description = "Employee" };
        _roleAccessDto = new RoleAccessDto { Id = 1, Permission = "ViewEmployee", Grouping = "Employee Data" };
        _roleAccessLinkDto = new RoleAccessLinkDto { Id = 1, Role = _roleDto, RoleAccess = _roleAccessDto };
    }

    [Fact]
    public async Task SaveTest()
    {
        _dbMock
            .Setup(r => r.RoleAccessLink.Add(It.IsAny<RoleAccessLink>()))
            .ReturnsAsync(new RoleAccessLink(_roleAccessLinkDto));

        var result = await _roleAccessLinkService.Create(_roleAccessLinkDto);

        Assert.NotNull(result);
        _roleAccessLinkDto.Role = null;
        _roleAccessLinkDto.RoleAccess = null;
        Assert.Equivalent(_roleAccessLinkDto, result);
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
        }.ToMockIQueryable();

        var randLink = roleAccessLinks
                       .Where(r => r.Id == 2)
                       .Select(r => r.ToDto())
                       .First();

        Expression<Func<RoleAccessLink, bool>> criteria = r =>
            r.Role!.Description == randLink.Role!.Description &&
            r.RoleAccess!.Permission == randLink.RoleAccess!.Permission;

        var expect = await roleAccessLinks
                           .Where(criteria)
                           .Select(r => r.ToDto())
                           .FirstAsync();

        var malformed = await roleAccessLinks
                              .Where(criteria)
                              .Select(r => new RoleAccessLinkDto { Id = r.Id, Role = null, RoleAccess = null })
                              .FirstAsync();

        _dbMock
            .SetupSequence(r => r.RoleAccessLink.Get(It.IsAny<Expression<Func<RoleAccessLink, bool>>>()))
            .Returns(roleAccessLinks.Where(criteria))
            .Returns(roleAccessLinks.Where(criteria));

        _dbMock
            .SetupSequence(r => r.RoleAccessLink.Delete(It.IsAny<int>()))
            .ReturnsAsync(new RoleAccessLink(expect))
            .ReturnsAsync(new RoleAccessLink(malformed));

        var result1 = await _roleAccessLinkService.Delete(_roleDto.Description!, _roleAccessDto.Permission);

        Assert.NotNull(result1);
        Assert.Equal(expect.Id, result1.Id);

        var result2 = await _roleAccessLinkService.Delete(_roleDto.Description!, _roleAccessDto.Permission);

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
        }.ToMockIQueryable();

        var expect = await roleAccessLinks
                           .GroupBy(r => r.Role!.Description)
                           .ToDictionaryAsync(
                                              group => group.Key!,
                                              group => group.Select(r => r.RoleAccess!.Permission).ToList());

        _dbMock
            .Setup(r => r.RoleAccessLink.Get(null))
            .Returns(roleAccessLinks);

        var result = await _roleAccessLinkService.GetAll();

        Assert.Equal(2, result.Count);
        Assert.Equal(expect.Keys!, result.Keys);
        Assert.Equal(expect.Values!, result.Values);
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
        }.ToMockIQueryable();

        var randLink = roleAccessLinks
                       .Where(r => r.Role!.Id == 1)
                       .Select(r => r.ToDto())
                       .First();

        Expression<Func<RoleAccessLink, bool>> criteria = r =>
            r.Role!.Description == randLink.Role!.Description;

        var expect = await roleAccessLinks
                           .Where(criteria)
                           .GroupBy(r => r.Role!.Description)
                           .ToDictionaryAsync(
                                              group => group.Key!,
                                              group => group.Select(r => r.RoleAccess!.Permission).ToList());

        _dbMock
            .Setup(r => r.RoleAccessLink.Get(It.IsAny<Expression<Func<RoleAccessLink, bool>>>()))
            .Returns(roleAccessLinks.Where(criteria));

        var result = await _roleAccessLinkService.GetByRole(randLink.Role!.Description!);

        Assert.Equal(2, result.First().Value.Count);
        Assert.Equal(expect.Keys!, result.Keys);
        Assert.Equal(expect.Values!, result.Values);
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
        }.ToMockIQueryable();

        var randLink = roleAccessLinks
                       .Where(r => r.Role!.Id == 1)
                       .Select(r => r.ToDto())
                       .First();

        Expression<Func<RoleAccessLink, bool>> criteria = r =>
            r.RoleAccess!.Permission == randLink.RoleAccess!.Permission;

        var expect = await roleAccessLinks
                           .Where(criteria)
                           .GroupBy(r => r.Role!.Description)
                           .ToDictionaryAsync(
                                              group => group.Key!,
                                              group => group.Select(r => r.RoleAccess!.Permission).ToList());

        _dbMock
            .Setup(r => r.RoleAccessLink.Get(It.IsAny<Expression<Func<RoleAccessLink, bool>>>()))
            .Returns(roleAccessLinks.Where(criteria));

        var result = await _roleAccessLinkService.GetByPermission(randLink.RoleAccess!.Permission);

        Assert.Equal(2, result.Count);
        Assert.Equal(expect.Keys!, result.Keys);
        Assert.Equal(expect.Values!, result.Values);
        _dbMock.Verify(r => r.RoleAccessLink.Get(It.IsAny<Expression<Func<RoleAccessLink, bool>>>()), Times.Once);
    }

    [Fact]
    public async Task GetRoleByEmployeeTest()
    {
        var email = "test@retrorabbit.co.za";

        var testEmployee = EmployeeTestData.EmployeeOne;

        var employeeRoleDtos = new List<EmployeeRoleDto>
        {
            new()
            {
                Id = 1,
                Employee = testEmployee.ToDto(),
                Role = new RoleDto { Id = 1, Description = "Employee" }
            },

            new()
            {
                Id = 2,
                Employee = testEmployee.ToDto(),
                Role = new RoleDto { Id = 1, Description = "Manager" }
            }
        };

        _employeeRoleServiceMock
            .Setup(r => r.GetEmployeeRoles(email))
            .ReturnsAsync(employeeRoleDtos);

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
        }.ToMockIQueryable();

        var criteria = roleAccessLinks
                       .GroupBy(r => r.Role!.Description)
                       .Select(r => r.Key)
                       .Distinct()
                       .Select(r => (Expression<Func<RoleAccessLink, bool>>)(x => x.Role!.Description == r))
                       .ToList();

        var returns = _dbMock
                      .SetupSequence(r => r.RoleAccessLink.Get(It.IsAny<Expression<Func<RoleAccessLink, bool>>>()))
                      .Returns(roleAccessLinks.Where(criteria[0]));

        for (var i = 1; i < criteria.Count; i++)
            returns = returns.Returns(roleAccessLinks.Where(criteria[i]));

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
        var roleAccessLinkToUpdate = new RoleAccessLinkDto
        {
            Id = _roleAccessLinkDto.Id,
            Role = new RoleDto { Id = 1, Description = "Employee" },
            RoleAccess = new RoleAccessDto { Id = 2, Permission = "EditEmployee", Grouping = "Employee Data" }
        };

        _dbMock
            .Setup(r => r.RoleAccessLink.Update(It.IsAny<RoleAccessLink>()))
            .ReturnsAsync(new RoleAccessLink(_roleAccessLinkDto));

        var result = await _roleAccessLinkService.Update(roleAccessLinkToUpdate);

        _roleAccessLinkDto.Role = null;
        _roleAccessLinkDto.RoleAccess = null;
        Assert.NotNull(result);
        Assert.Null(result.Role);
        Assert.Null(result.RoleAccess);
        Assert.Equivalent(_roleAccessLinkDto, result);
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
        }.ToMockIQueryable();

        _dbMock
            .Setup(r => r.RoleAccessLink.Get(It.IsAny<Expression<Func<RoleAccessLink, bool>>>()))
            .Returns(roleAccessLinkData);

        var employeeEvaluationTemplateItems = await _roleAccessLinkService.GetAllRoleAccessLink();

        Assert.Equal("Employee", employeeEvaluationTemplateItems[0].Role!.Description);
        Assert.Equal("ViewEmployee", employeeEvaluationTemplateItems[0].RoleAccess!.Permission);


        Assert.Equal("Employee", employeeEvaluationTemplateItems[1].Role!.Description);
        Assert.Equal("EditEmployee", employeeEvaluationTemplateItems[1].RoleAccess!.Permission);
    }
}
