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
    private readonly RoleAccessLinkService _roleAccessLinkService2;
    private readonly RoleDto _roleDto;

    public RoleAccessLinkServiceUnitTest()
    {
        _dbMock = new Mock<IUnitOfWork>();
        _employeeRoleServiceMock = new Mock<IEmployeeRoleService>();
        _roleAccessLinkService = new RoleAccessLinkService(_dbMock.Object, _employeeRoleServiceMock.Object, new AuthorizeIdentityMock("test@gmail.com", "test", "Admin", 1));
        _roleAccessLinkService2 = new RoleAccessLinkService(_dbMock.Object, _employeeRoleServiceMock.Object, new AuthorizeIdentityMock("test@gmail.com", "test", "Employee", 2));
        _roleDto = new RoleDto { Id = 1, Description = "Employee" };
        _roleAccessDto = new RoleAccessDto { Id = 1, Permission = "ViewEmployee", Grouping = "Employee Data" };
        _roleAccessLinkDto = new RoleAccessLinkDto { Id = 1, Role = _roleDto, RoleAccess = _roleAccessDto };
    }

    [Fact]
    public async Task CreateTestPass()
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
    public async Task CreateTestUnauthorised()
    {
        _dbMock
            .Setup(r => r.RoleAccessLink.Any(It.IsAny<Expression<Func<RoleAccessLink, bool>>>()))
            .ReturnsAsync(false);

        await Assert.ThrowsAsync<CustomException>(() => _roleAccessLinkService2.Create(_roleAccessLinkDto));
    }

    [Fact]
    public async Task CreateTestFail()
    {
        _dbMock
            .Setup(r => r.RoleAccessLink.Any(It.IsAny<Expression<Func<RoleAccessLink, bool>>>()))
            .ReturnsAsync(true);

        await Assert.ThrowsAsync<CustomException>(() => _roleAccessLinkService2.Create(_roleAccessLinkDto));
    }

    [Fact]
    public async Task DeleteTestPass()
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

        _dbMock.Setup(x => x.RoleAccessLink.Any(It.IsAny<Expression<Func<RoleAccessLink, bool>>>()))
               .ReturnsAsync(true);
        _dbMock
            .Setup(r => r.RoleAccessLink.Any(It.IsAny<Expression<Func<RoleAccessLink, bool>>>()))
            .ReturnsAsync(true);

        var result1 = await _roleAccessLinkService.Delete(_roleDto.Description!, _roleAccessDto.Permission);

        Assert.NotNull(result1);
        Assert.Equal(expect.Id, result1.Id);

        var result2 = await _roleAccessLinkService.Delete(_roleDto.Description!, _roleAccessDto.Permission);

        Assert.NotNull(result2.RoleAccess);
        Assert.NotNull(result2.Role);
        Assert.Equal(expect.Id, result2.Id);
    }

    [Fact]
    public async Task DeleteTestUnauthorised()
    {
        _dbMock
            .Setup(r => r.RoleAccessLink.Any(It.IsAny<Expression<Func<RoleAccessLink, bool>>>()))
            .ReturnsAsync(true);

        await Assert.ThrowsAsync<CustomException>(() => _roleAccessLinkService2.Delete(_roleDto.Description!, _roleAccessDto.Permission));
    }

    [Fact]
    public async Task DeleteTestFail()
    {
        _dbMock
            .Setup(r => r.RoleAccessLink.Any(It.IsAny<Expression<Func<RoleAccessLink, bool>>>()))
            .ReturnsAsync(false);

        await Assert.ThrowsAsync<CustomException>(() => _roleAccessLinkService2.Delete(_roleDto.Description!, _roleAccessDto.Permission));
    }

    [Fact]
    public async Task GetAllTestPass()
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

        _dbMock
            .Setup(r => r.RoleAccessLink.Any(It.IsAny<Expression<Func<RoleAccessLink, bool>>>()))
            .ReturnsAsync(true);

        var result = await _roleAccessLinkService.GetAll();

        Assert.Equal(2, result.Count);
        Assert.Equal(expect.Keys!, result.Keys);
        Assert.Equal(expect.Values!, result.Values);
        _dbMock.Verify(r => r.RoleAccessLink.Get(null), Times.Once);
    }

    [Fact]
    public async Task GetAllTestUnauthorised()
    {
        _dbMock
            .Setup(r => r.RoleAccessLink.Any(It.IsAny<Expression<Func<RoleAccessLink, bool>>>()))
            .ReturnsAsync(true);

        await Assert.ThrowsAsync<CustomException>(() => _roleAccessLinkService2.GetAll());
    }

    [Fact]
    public async Task GetByRoleTestPass()
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

        _dbMock.Setup(x => x.Role.Any(It.IsAny<Expression<Func<Role, bool>>>()))
               .ReturnsAsync(true);

        var result = await _roleAccessLinkService.GetByRole(randLink.Role!.Description!);

        Assert.Equal(2, result.First().Value.Count);
        Assert.Equal(expect.Keys!, result.Keys);
        Assert.Equal(expect.Values!, result.Values);
        _dbMock.Verify(r => r.RoleAccessLink.Get(It.IsAny<Expression<Func<RoleAccessLink, bool>>>()), Times.Once);
    }

    [Fact]
    public async Task GetByRoleTestUnauthorised()
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

        _dbMock.Setup(x => x.Role.Any(It.IsAny<Expression<Func<Role, bool>>>()))
               .ReturnsAsync(true);

        await Assert.ThrowsAsync<CustomException>(() => _roleAccessLinkService2.GetByRole(randLink.Role!.Description!));
    }

    [Fact]
    public async Task GetByRoleTestFail()
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

        _dbMock.Setup(x => x.Role.Any(It.IsAny<Expression<Func<Role, bool>>>()))
               .ReturnsAsync(false);

        await Assert.ThrowsAsync<CustomException>(() => _roleAccessLinkService2.GetByRole(randLink.Role!.Description!));
    }

    [Fact]
    public async Task GetBypermissionTestPass()
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

        _dbMock.Setup(x => x.RoleAccess.Any(It.IsAny<Expression<Func<RoleAccess, bool>>>()))
               .ReturnsAsync(true);

        var result = await _roleAccessLinkService.GetByPermission(randLink.RoleAccess!.Permission);

        Assert.Equal(2, result.Count);
        Assert.Equal(expect.Keys!, result.Keys);
        Assert.Equal(expect.Values!, result.Values);
        _dbMock.Verify(r => r.RoleAccessLink.Get(It.IsAny<Expression<Func<RoleAccessLink, bool>>>()), Times.Once);
    }

    [Fact]
    public async Task GetBypermissionTestUnauthorised()
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

        _dbMock.Setup(x => x.RoleAccess.Any(It.IsAny<Expression<Func<RoleAccess, bool>>>()))
               .ReturnsAsync(true);

        await Assert.ThrowsAsync<CustomException>(() => _roleAccessLinkService2.GetByPermission(randLink.RoleAccess!.Permission));
    }

    [Fact]
    public async Task GetBypermissionTestFail()
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

        _dbMock.Setup(x => x.RoleAccess.Any(It.IsAny<Expression<Func<RoleAccess, bool>>>()))
               .ReturnsAsync(false);

        await Assert.ThrowsAsync<CustomException>(() => _roleAccessLinkService2.GetByPermission(randLink.RoleAccess!.Permission));
    }

    [Fact]
    public async Task GetRoleByEmployeeTestPass()
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

        _dbMock.Setup(x => x.Employee.Any(It.IsAny<Expression<Func<Employee, bool>>>()))
               .ReturnsAsync(true);

        _dbMock.Setup(x => x.Role.Any(It.IsAny<Expression<Func<Role, bool>>>()))
               .ReturnsAsync(true);

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
    public async Task GetRoleByEmployeeTestUnauthorised()
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

        _dbMock.Setup(x => x.Employee.Any(It.IsAny<Expression<Func<Employee, bool>>>()))
               .ReturnsAsync(true);

        await Assert.ThrowsAsync<CustomException>(() => _roleAccessLinkService2.GetRoleByEmployee(email));
    }

    [Fact]
    public async Task GetRoleByEmployeeTestFail()
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

        _dbMock.Setup(x => x.Employee.Any(It.IsAny<Expression<Func<Employee, bool>>>()))
               .ReturnsAsync(false);

        await Assert.ThrowsAsync<CustomException>(() => _roleAccessLinkService2.GetRoleByEmployee(email));
    }

    [Fact]
    public async Task UpdateTestPass()
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

        _dbMock.Setup(x => x.RoleAccessLink.Any(It.IsAny<Expression<Func<RoleAccessLink, bool>>>()))
               .ReturnsAsync(true);

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
    public async Task UpdateTestPassUnauthorised()
    {
        var roleAccessLinkToUpdate = new RoleAccessLinkDto
        {
            Id = _roleAccessLinkDto.Id,
            Role = new RoleDto { Id = 1, Description = "Employee" },
            RoleAccess = new RoleAccessDto { Id = 2, Permission = "EditEmployee", Grouping = "Employee Data" }
        };

        _dbMock
            .Setup(r => r.RoleAccessLink.Any(It.IsAny<Expression<Func<RoleAccessLink, bool>>>()))
            .ReturnsAsync(true);

        await Assert.ThrowsAsync<CustomException>(() => _roleAccessLinkService2.Update(roleAccessLinkToUpdate));
    }

    [Fact]
    public async Task UpdateTestPassFail()
    {
        var roleAccessLinkToUpdate = new RoleAccessLinkDto
        {
            Id = _roleAccessLinkDto.Id,
            Role = new RoleDto { Id = 1, Description = "Employee" },
            RoleAccess = new RoleAccessDto { Id = 2, Permission = "EditEmployee", Grouping = "Employee Data" }
        };

        _dbMock
            .Setup(r => r.RoleAccessLink.Any(It.IsAny<Expression<Func<RoleAccessLink, bool>>>()))
            .ReturnsAsync(false);

        await Assert.ThrowsAsync<CustomException>(() => _roleAccessLinkService2.Update(roleAccessLinkToUpdate));
    }

    [Fact]
    public async Task GetAllRoleAccessLink_ReturnsRoleAccessLinksTestPass()
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

    [Fact]
    public async Task GetAllRoleAccessLink_ReturnsRoleAccessLinksTestUnauthorised()
    {
        await Assert.ThrowsAsync<CustomException>(() => _roleAccessLinkService2.GetAllRoleAccessLink());
    }
}
