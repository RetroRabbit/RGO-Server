using System.Linq.Expressions;
using Auth0.ManagementApi.Paging;
using Auth0.ManagementApi;
using HRIS.Services.Interfaces;
using HRIS.Services.Services;
using Microsoft.EntityFrameworkCore;
using Moq;
using RR.Tests.Data;
using RR.UnitOfWork;
using RR.UnitOfWork.Entities.HRIS;
using Xunit;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Reflection;
using Auth0.ManagementApi.Clients;
using HRIS.Models;
using Microsoft.Extensions.Options;

namespace HRIS.Services.Tests.Services;

public class RoleServiceUnitTest
{
    private readonly Mock<IUnitOfWork> _dbMock;
    private readonly Role _role;
    private readonly RoleService _roleService;
    private readonly RoleService _roleService2;
    private readonly Mock<IAuthService> _authServiceMock;

    public RoleServiceUnitTest()
    {
        _dbMock = new Mock<IUnitOfWork>();
        _authServiceMock = new Mock<IAuthService>();
        _roleService = new RoleService(_dbMock.Object, _authServiceMock.Object, new AuthorizeIdentityMock("test@gmail.com", "test", "Admin", 1));
        _roleService2 = new RoleService(_dbMock.Object, _authServiceMock.Object, new AuthorizeIdentityMock("test@gmail.com", "test", "Employee", 2));
        _role = new Role { Id = 1, Description = "Employee" };
    }

    [Fact]
    public async Task CheckRoleTest()
    {
        _dbMock
            .Setup(r => r.Role.Any(It.IsAny<Expression<Func<Role, bool>>>()))
            .ReturnsAsync(true);

        var result = await _roleService.CheckRole(_role.Description!);

        Assert.True(result);
        _dbMock.Verify(r => r.Role.Any(It.IsAny<Expression<Func<Role, bool>>>()), Times.Once);
    }

    [Fact]
    public async Task SaveRoleTestPass()
    {
        _dbMock
            .Setup(r => r.Role.Add(It.IsAny<Role>()))
            .ReturnsAsync(_role);

        var result = await _roleService.CreateRole(_role.ToDto());

        Assert.NotNull(result);
        Assert.Equivalent(_role.ToDto(), result);
        _dbMock.Verify(r => r.Role.Add(It.IsAny<Role>()), Times.Once);
    }

    [Fact]
    public async Task SaveRoleTestUnauthorised()
    {
        await Assert.ThrowsAsync<CustomException>(() => _roleService2.CreateRole(_role.ToDto()));
    }

    [Fact]
    public async Task SaveRoleTestExistPass()
    {
        _dbMock
            .Setup(r => r.Role.Any(It.IsAny<Expression<Func<Role, bool>>>()))
            .ReturnsAsync(true);

        _dbMock
            .Setup(r => r.Role.Add(It.IsAny<Role>()))
            .ReturnsAsync(_role);

        var pagination = new PagingInformation(0, 50, 10, 10);
        IPagedList<Auth0.ManagementApi.Models.Role>? roles = new PagedList<Auth0.ManagementApi.Models.Role>(new List<Auth0.ManagementApi.Models.Role>
        {
            new Auth0.ManagementApi.Models.Role { Id = "role1", Name = "Role 1" },
            new Auth0.ManagementApi.Models.Role { Id = "role2", Name = "Role 2" }
        }, pagination);

        _authServiceMock.Setup(a => a.GetAllRolesAsync()).ReturnsAsync(roles);

        _dbMock.Setup(r => r.Role.Get(It.IsAny<Expression<Func<Role, bool>>>())).Returns(_role.ToMockIQueryable());

        var result = await _roleService.CreateRole(_role.ToDto());

        Assert.NotNull(result);
        Assert.Equivalent(_role.ToDto(), result);
    }

    [Fact]
    public async Task SaveRoleTestExistUpdatePass()
    {
        _dbMock
            .Setup(r => r.Role.Any(It.IsAny<Expression<Func<Role, bool>>>()))
            .ReturnsAsync(true);

        _dbMock
            .Setup(r => r.Role.Update(It.IsAny<Role>()))
            .ReturnsAsync(_role);

        var pagination = new PagingInformation(0, 50, 10, 10);
        IPagedList<Auth0.ManagementApi.Models.Role>? roles = new PagedList<Auth0.ManagementApi.Models.Role>(new List<Auth0.ManagementApi.Models.Role>
        {
            new Auth0.ManagementApi.Models.Role { Id = "role1", Name = "Role 1" },
            new Auth0.ManagementApi.Models.Role { Id = "role2", Name = "Role 2" },
            new Auth0.ManagementApi.Models.Role { Id = "employee", Name = "Employee" }
        }, pagination);

        _authServiceMock.Setup(a => a.GetAllRolesAsync()).ReturnsAsync(roles);

        _dbMock.Setup(r => r.Role.Get(It.IsAny<Expression<Func<Role, bool>>>())).Returns(_role.ToMockIQueryable());

        var result = await _roleService.CreateRole(_role.ToDto());

        Assert.NotNull(result);
        Assert.Equivalent(_role.ToDto(), result);
    }

    [Fact]
    public async Task DeleteRoleTestPass()
    {
        var roleQueryable = new List<Role>
        {
            new() { Id = 2, Description = "Manager" },
            new() { Id = 3, Description = "Admin" }
        }.ToMockIQueryable();

        Expression<Func<Role, bool>> criteria = r => r.Description == "Admin";
        var expect = await roleQueryable.Where(criteria).Select(r => r).FirstAsync();

        _dbMock
            .Setup(r => r.Role.Get(It.IsAny<Expression<Func<Role, bool>>>()))
            .Returns(roleQueryable.Where(criteria));

        _dbMock.Setup(x => x.Role.Any(It.IsAny<Expression<Func<Role, bool>>>()))
               .ReturnsAsync(true);

        _dbMock
            .Setup(r => r.Role.Delete(It.IsAny<int>()))
            .ReturnsAsync(expect);

        var result = await _roleService.DeleteRole(3);

        Assert.NotNull(result);
        Assert.Equivalent(expect, result);
        _dbMock.Verify(r => r.Role.Delete(It.IsAny<int>()), Times.Once);
    }

    [Fact]
    public async Task DeleteRoleTestUnauthorised()
    {
        _dbMock.Setup(x => x.Role.Any(It.IsAny<Expression<Func<Role, bool>>>()))
               .ReturnsAsync(true);

        await Assert.ThrowsAsync<CustomException>(() => _roleService2.DeleteRole(3));
    }

    [Fact]
    public async Task DeleteRoleTestFail()
    {
        _dbMock.Setup(x => x.Role.Any(It.IsAny<Expression<Func<Role, bool>>>()))
               .ReturnsAsync(false);

        await Assert.ThrowsAsync<CustomException>(() => _roleService2.DeleteRole(3));
    }

    [Fact]
    public async Task GetAllRolesTestPass()
    {
        var roles = new List<Role> { _role };

        _dbMock
            .Setup(r => r.Role.GetAll(null))
            .ReturnsAsync(roles);

        var result = await _roleService.GetAll();

        Assert.NotNull(result);
        Assert.Equivalent(roles.Select(x => x.ToDto()).ToList(), result);
        _dbMock.Verify(r => r.Role.GetAll(null), Times.Once);
    }

    [Fact]
    public async Task GetAllRolesTestUnauthorised()
    {
        await Assert.ThrowsAsync<CustomException>(() => _roleService2.GetAll());
    }

    [Fact]
    public async Task GetRoleTestPass()
    {
        _dbMock
            .Setup(r => r.Role.Get(It.IsAny<Expression<Func<Role, bool>>>()))
            .Returns(_role.ToMockIQueryable());

        _dbMock.Setup(x => x.Role.Any(It.IsAny<Expression<Func<Role, bool>>>()))
               .ReturnsAsync(true);

        var result = await _roleService.GetRole(_role.Description!);

        Assert.NotNull(result);
        Assert.Equivalent(_role.ToDto(), result);
        _dbMock.Verify(r => r.Role.Get(It.IsAny<Expression<Func<Role, bool>>>()), Times.Once);
    }

    [Fact]
    public async Task GetRoleTestUnauthorised()
    {
        _dbMock.Setup(x => x.Role.Any(It.IsAny<Expression<Func<Role, bool>>>()))
               .ReturnsAsync(true);

        await Assert.ThrowsAsync<CustomException>(() => _roleService2.GetRole(_role.Description!));
    }

    [Fact]
    public async Task GetRoleTestFail()
    {
        _dbMock.Setup(x => x.Role.Any(It.IsAny<Expression<Func<Role, bool>>>()))
               .ReturnsAsync(false);

        await Assert.ThrowsAsync<CustomException>(() => _roleService2.GetRole(_role.Description!));
    }

    [Fact]
    public async Task UpdateRoleTestPass()
    {
        var roleQueryable = new List<Role>
        {
            new() { Id = 2, Description = "Manager" },
            new() { Id = 3, Description = "Admin" }
        }.ToMockIQueryable();

        Expression<Func<Role, bool>> criteria = r => r.Description == "Admin";
        var expect = await roleQueryable.Where(criteria).FirstAsync();

        _dbMock
            .Setup(r => r.Role.Get(It.IsAny<Expression<Func<Role, bool>>>()))
            .Returns(roleQueryable.Where(criteria));

        _dbMock.Setup(x => x.Role.Any(It.IsAny<Expression<Func<Role, bool>>>()))
               .ReturnsAsync(true);

        _dbMock
            .Setup(r => r.Role.Update(It.IsAny<Role>()))
            .ReturnsAsync(expect);

        var result = await _roleService.UpdateRole("Admin");

        Assert.NotNull(result);
        Assert.Equivalent(expect.ToDto(), result);
        _dbMock.Verify(r => r.Role.Update(It.IsAny<Role>()), Times.Once);
    }

    [Fact]
    public async Task UpdateRoleTestUnauthorised()
    {
        _dbMock.Setup(x => x.Role.Any(It.IsAny<Expression<Func<Role, bool>>>()))
               .ReturnsAsync(true);

        await Assert.ThrowsAsync<CustomException>(() => _roleService2.UpdateRole("Admin"));
    }

    [Fact]
    public async Task UpdateRoleTestFail()
    {
        _dbMock.Setup(x => x.Role.Any(It.IsAny<Expression<Func<Role, bool>>>()))
               .ReturnsAsync(false);

        await Assert.ThrowsAsync<CustomException>(() => _roleService2.UpdateRole("Admin"));
    }

    private string CreateJwtToken(bool notExpired = true, bool hasExpClaim = true)
    {
        var securityKey = new SymmetricSecurityKey(System.Text.Encoding.Default.GetBytes("test_secret_key_which_must_be_a_longer_than_128bits"));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var claims = new List<Claim>();

        if (hasExpClaim)
        {
            if (notExpired)
            {
                claims.Add(new Claim("exp", DateTimeOffset.UtcNow.AddMinutes(10).ToUnixTimeSeconds().ToString()));
            }
            else
            {
                claims.Add(new Claim("exp", DateTimeOffset.UtcNow.AddMinutes(-10).ToUnixTimeSeconds().ToString()));
            }
        }
        else
        {
            new Claim("exp", string.Empty);
        }

        var token = new JwtSecurityToken(
            issuer: "https://test-issuer/",
            audience: "test-audience",
            claims: claims,
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private void SetPrivateField(object instance, string fieldName, object value)
    {
        var field = instance.GetType().GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);
        field?.SetValue(instance, value);
    }
    private AuthService CreateAuthServiceWithMocks(out Mock<IManagementApiClient> managementApiClientMock, out Mock<HttpMessageHandler> httpMessageHandlerMock, out HttpClient httpClient, out Mock<IUsersClient> usersClientMock, out Mock<IRolesClient> rolesClientMock)
    {
        var authManagementOptionsMock = new Mock<IOptions<AuthManagement>>();
        var authManagement = new AuthManagement
        {
            ClientId = "test-client-id",
            ClientSecret = "test-client-secret",
            Issuer = "https://test-issuer/",
            Audience = "test-audience"
        };
        authManagementOptionsMock.Setup(opt => opt.Value).Returns(authManagement);

        httpMessageHandlerMock = new Mock<HttpMessageHandler>();
        httpClient = new HttpClient(httpMessageHandlerMock.Object);

        var authService = new AuthService(authManagementOptionsMock.Object);
        SetPrivateField(authService, "_httpClient", httpClient);

        managementApiClientMock = new Mock<IManagementApiClient>();
        usersClientMock = new Mock<IUsersClient>();
        rolesClientMock = new Mock<IRolesClient>();

        managementApiClientMock.Setup(client => client.Users).Returns(usersClientMock.Object);
        managementApiClientMock.Setup(client => client.Roles).Returns(rolesClientMock.Object);

        SetPrivateField(authService, "_managementApiClient", managementApiClientMock.Object);

        return authService;
    }
}