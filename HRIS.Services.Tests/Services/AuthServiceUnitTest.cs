using Auth0.ManagementApi;
using Auth0.ManagementApi.Models;
using Auth0.ManagementApi.Paging;
using HRIS.Models;
using HRIS.Services.Services;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Moq;
using Moq.Protected;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Xunit;
using System.Text;
using System.Net;
using System.Reflection;
using Auth0.ManagementApi.Clients;

namespace HRIS.Services.Tests.Services;

public class AuthServiceUnitTests
{
    private readonly Mock<HttpMessageHandler> _httpMessageHandlerMock;
    private readonly Mock<IManagementApiClient> _managementApiClientMock;
    private readonly Mock<IUsersClient> _usersClientMock;
    private readonly Mock<IRolesClient> _rolesClientMock;
    private readonly HttpClient _httpClient;
    private readonly AuthService _authService;

    public AuthServiceUnitTests()
    {
        _authService = CreateAuthServiceWithMocks(out _managementApiClientMock, out _httpMessageHandlerMock, out _httpClient, out _usersClientMock, out _rolesClientMock);
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

    private void SetPrivateField(object instance, string fieldName, object value)
    {
        var field = instance.GetType().GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);
        field?.SetValue(instance, value);
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

    private HttpClient CreateHttpClientMock(HttpStatusCode statusCode, string responseContent)
    {
        var handlerMock = new Mock<HttpMessageHandler>();
        handlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = statusCode,
                Content = new StringContent(responseContent, Encoding.UTF8, "application/json")
            });

        return new HttpClient(handlerMock.Object);
    }

    [Theory]
    [InlineData(false, true, true)] // ExpiredToken
    [InlineData(true, true, false)] // ValidToken
    [InlineData(true, false, true)] // TokenWithoutExpClaim
    public void IsTokenExpired_ReturnsExpectedResult(bool isValid, bool includeExpClaim, bool expectedResult)
    {
        var token = CreateJwtToken(isValid, includeExpClaim);
        var result = AuthService.IsTokenExpired(token);
        Assert.Equal(expectedResult, result);
    }

    [Theory]
    [InlineData(HttpStatusCode.OK, "{\"access_token\": \"new_token\"}", "validToken", "validToken")]
    [InlineData(HttpStatusCode.OK, "{\"access_token\": \"new_token\"}", "new_token", "expiredToken")]
    [InlineData(HttpStatusCode.BadRequest, "error", null, "")]
    [InlineData(HttpStatusCode.OK, "{\"invalid_key\": \"value\"}", null, "")]
    public async Task GetAuth0ManagementAccessToken_ReturnsExpectedResult(HttpStatusCode statusCode, string responseContent, string expectedToken, string cachedTokenState)
    {
        string? cachedToken = cachedTokenState switch
        {
            "validToken" => CreateJwtToken(),
            "expiredToken" => CreateJwtToken(false, true),
            _ => cachedTokenState
        };

        SetPrivateField(_authService, "_cachedAccessToken", cachedToken);

        var httpClient = CreateHttpClientMock(statusCode, responseContent);
        SetPrivateField(_authService, "_httpClient", httpClient);

        if (expectedToken == null)
        {
            await Assert.ThrowsAsync<CustomException>(() => _authService.GetAuth0ManagementAccessToken());
        }
        else
        {
            var result = await _authService.GetAuth0ManagementAccessToken();

            if (cachedTokenState == "validToken")
            {
                Assert.Equal(cachedToken, result);
            }
            else
            {
                Assert.Equal(expectedToken, result);
            }
        }
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task GetAllRolesAsync_Test(bool rolesExist)
    {
        var pagination = new PagingInformation(0, 50, 10, 10);
        IPagedList<Role>? roles = rolesExist ? new PagedList<Role>(new List<Role>
        {
            new Role { Id = "role1", Name = "Role 1" },
            new Role { Id = "role2", Name = "Role 2" }
        }, pagination) : null;

        var validToken = CreateJwtToken();
        SetPrivateField(_authService, "_cachedAccessToken", validToken);

        _managementApiClientMock.Setup(client => client.Roles.GetAllAsync(It.IsAny<GetRolesRequest>(), It.IsAny<PaginationInfo>(), It.IsAny<CancellationToken>()))
                                .ReturnsAsync(roles);

        if (rolesExist)
        {
            var result = await _authService.GetAllRolesAsync();

            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal("role1", result[0].Id);
            Assert.Equal("Role 1", result[0].Name);
        }
        else
        {
            await Assert.ThrowsAsync<CustomException>(() => _authService.GetAllRolesAsync());
        }
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task GetAllUsersAsync_Test(bool usersExist)
    {
        var pagination = new PagingInformation(0, 50, 10, 10);
        IPagedList<User>? users = usersExist ? new PagedList<User>(new List<User>
        {
            new User { UserId = "1", Email = "test1@retrorabbit.co.za" },
            new User { UserId = "2", Email = "test2@retrorabbit.co.za" }
        }, pagination) : null;

        var validToken = CreateJwtToken();
        SetPrivateField(_authService, "_cachedAccessToken", validToken);

        _usersClientMock.Setup(client => client.GetAllAsync(It.IsAny<GetUsersRequest>(), It.IsAny<PaginationInfo>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(users);

        if (usersExist)
        {
            var result = await _authService.GetAllUsersAsync();

            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal("1", result[0].UserId);
            Assert.Equal("test1@retrorabbit.co.za", result[0].Email);
        }
        else
        {
            await Assert.ThrowsAsync<CustomException>(() => _authService.GetAllUsersAsync());
        }
    }

    [Theory]
    [InlineData("test@example.com", true)]
    [InlineData("nonexistent@example.com", false)]
    public async Task GetUsersByEmailAsync_Test(string email, bool userExists)
    {
        var validToken = CreateJwtToken();
        SetPrivateField(_authService, "_cachedAccessToken", validToken);

        IList<User>? users = userExists ? new List<User>
        {
            new User { UserId = "user1", Email = email, FullName = "User 1" },
            new User { UserId = "user2", Email = email, FullName = "User 2" }
        } : null;

        _usersClientMock.Setup(client => client.GetUsersByEmailAsync(email, null, null, It.IsAny<CancellationToken>()))
                        .ReturnsAsync(users);

        if (userExists)
        {
            var result = await _authService.GetUsersByEmailAsync(email);

            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal("user1", result[0].UserId);
            Assert.Equal(email, result[0].Email);
            Assert.Equal("User 1", result[0].FullName);
        }
        else
        {
            await Assert.ThrowsAsync<CustomException>(() => _authService.GetUsersByEmailAsync(email));
        }
    }

    [Theory]
    [InlineData("role1", true, false)]
    [InlineData("nonexistentRole", false, false)]
    [InlineData("role1", true, true)]
    public async Task GetUsersByRoleAsync_Test(string roleId, bool roleExists, bool failsToRetrieveUsers)
    {
        var validToken = CreateJwtToken();
        SetPrivateField(_authService, "_cachedAccessToken", validToken);

        var roles = new PagedList<Role>(new List<Role>
        {
            new Role { Id = "role1", Name = "Role 1" },
            new Role { Id = "role2", Name = "Role 2" }
        }, new PagingInformation(0, 50, 2, 2));

        if (roleExists)
        {
            _managementApiClientMock.Setup(client => client.Roles.GetAllAsync(It.IsAny<GetRolesRequest>(), It.IsAny<PaginationInfo>(), It.IsAny<CancellationToken>()))
                                    .ReturnsAsync(roles);

            if (failsToRetrieveUsers)
            {
                _managementApiClientMock.Setup(client => client.Roles.GetUsersAsync(roleId, It.IsAny<PaginationInfo>(), It.IsAny<CancellationToken>()))
                                        .ReturnsAsync((IPagedList<AssignedUser>)null);
            }
            else
            {
                var assignedUsers = new PagedList<AssignedUser>(new List<AssignedUser>
                {
                    new AssignedUser { UserId = "user1", FullName = "User 1" },
                    new AssignedUser { UserId = "user2", FullName = "User 2" }
                }, new PagingInformation(0, 50, 2, 2));

                _managementApiClientMock.Setup(client => client.Roles.GetUsersAsync(roleId, It.IsAny<PaginationInfo>(), It.IsAny<CancellationToken>()))
                                        .ReturnsAsync(assignedUsers);
            }
        }
        else
        {
            _managementApiClientMock.Setup(client => client.Roles.GetAllAsync(It.IsAny<GetRolesRequest>(), It.IsAny<PaginationInfo>(), It.IsAny<CancellationToken>()))
                                    .ReturnsAsync(roles);
        }

        if (roleExists && !failsToRetrieveUsers)
        {
            var result = await _authService.GetUsersByRoleAsync(roleId);

            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal("user1", result[0].UserId);
            Assert.Equal("User 1", result[0].FullName);
        }
        else
        {
            await Assert.ThrowsAsync<CustomException>(() => _authService.GetUsersByRoleAsync(roleId));
        }
    }

    [Theory]
    [InlineData("user1", true, false)]
    [InlineData("nonexistentUser", false, false)]
    [InlineData("user1", true, true)]
    public async Task GetUserRolesAsync_Test(string userId, bool userExists, bool failsToRetrieveRoles)
    {
        var validToken = CreateJwtToken();
        SetPrivateField(_authService, "_cachedAccessToken", validToken);

        var users = new PagedList<User>(new List<User>
        {
            new User { UserId = "user1", FullName = "User 1" },
            new User { UserId = "user2", FullName = "User 2" }
        }, new PagingInformation(0, 50, 2, 2));

        var userRoles = new PagedList<Role>(new List<Role>
        {
            new Role { Id = "role1", Name = "Role 1" },
            new Role { Id = "role2", Name = "Role 2" }
        }, new PagingInformation(0, 50, 2, 2));

        if (userExists)
        {
            _managementApiClientMock.Setup(client => client.Users.GetAllAsync(It.IsAny<GetUsersRequest>(), It.IsAny<PaginationInfo>(), It.IsAny<CancellationToken>()))
                                    .ReturnsAsync(users);

            if (failsToRetrieveRoles)
            {
                _managementApiClientMock.Setup(client => client.Users.GetRolesAsync(userId, It.IsAny<PaginationInfo>(), It.IsAny<CancellationToken>()))
                                        .ReturnsAsync((IPagedList<Role>)null);
            }
            else
            {
                _managementApiClientMock.Setup(client => client.Users.GetRolesAsync(userId, It.IsAny<PaginationInfo>(), It.IsAny<CancellationToken>()))
                                        .ReturnsAsync(userRoles);
            }
        }
        else
        {
            _managementApiClientMock.Setup(client => client.Users.GetAllAsync(It.IsAny<GetUsersRequest>(), It.IsAny<PaginationInfo>(), It.IsAny<CancellationToken>()))
                                    .ReturnsAsync(users);
        }

        if (userExists && !failsToRetrieveRoles)
        {
            var result = await _authService.GetUserRolesAsync(userId);

            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal("role1", result[0].Id);
            Assert.Equal("Role 1", result[0].Name);
        }
        else
        {
            await Assert.ThrowsAsync<CustomException>(() => _authService.GetUserRolesAsync(userId));
        }
    }

    [Theory]
    [InlineData("userAndRoleExistsAssignmentSucceeds", "role1", true, true, true)]
    [InlineData("userAndRoleExistsAssignmentFails", "role1", true, false, true)]
    [InlineData("userDoesNotExistRoleExists", "role1", false, true, false)]
    public async Task AddRoleToUserAsync_Test(string userId, string roleId, bool userExists, bool roleExists, bool assignRoleSuccess)
    {
        var users = new PagedList<User>(new List<User>
        {
            new User { UserId = userId, FullName = "User 1" },
            new User { UserId = "user2", FullName = "User 2" }
        }, new PagingInformation(0, 50, 2, 2));

        var roles = roleExists
            ? new PagedList<Role>(new List<Role>
            {
                new Role { Id = roleId, Name = "Role 1" },
                new Role { Id = "role2", Name = "Role 2" }
            }, new PagingInformation(0, 50, 2, 2))
            : new PagedList<Role>(new List<Role>(), new PagingInformation(0, 50, 0, 0));

        var assignedRoles = new AssignRolesRequest
        {
            Roles = new[] { roleId }
        };

        var validToken = CreateJwtToken();
        SetPrivateField(_authService, "_cachedAccessToken", validToken);

        _usersClientMock.Setup(client => client.GetAllAsync(It.IsAny<GetUsersRequest>(), It.IsAny<PaginationInfo>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(userExists ? users : new PagedList<User>(new List<User>(), new PagingInformation(0, 50, 0, 0)));

        _rolesClientMock.Setup(client => client.GetAllAsync(It.IsAny<GetRolesRequest>(), It.IsAny<PaginationInfo>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(roles);

        if (assignRoleSuccess)
        {
            _usersClientMock.Setup(client => client.AssignRolesAsync(userId, assignedRoles, It.IsAny<CancellationToken>()))
                            .Returns(Task.CompletedTask);
        }
        else
        {
            _usersClientMock.Setup(client => client.AssignRolesAsync(userId, assignedRoles, It.IsAny<CancellationToken>()))
                            .ThrowsAsync(new CustomException("Failed to assign role"));
        }

        if (userExists && roleExists && assignRoleSuccess)
        {
            await _authService.AddRoleToUserAsync(userId, roleId);

            _usersClientMock.Verify(client => client.AssignRolesAsync(userId, It.Is<AssignRolesRequest>(r => r.Roles.Contains(roleId)), It.IsAny<CancellationToken>()), Times.Once);
        }
        else
        {
            await Assert.ThrowsAsync<CustomException>(async () => await _authService.AddRoleToUserAsync(userId, roleId));
        }
    }

    [Theory]
    [InlineData("userAndRoleExistsRemovalSucceeds", "role1", true, true, true)]
    [InlineData("userDoesNotExistRoleExists", "role1", false, true, false)]
    [InlineData("useExistRoleDoesNotExists", "nonexistentRole", true, false, false)]
    public async Task RemoveRoleFromUserAsync_Test(string userId, string roleId, bool userExists, bool roleExists, bool removeRoleSuccess)
    {
        var users = userExists
            ? new PagedList<User>(new List<User>
            {
                new User { UserId = userId, FullName = "User 1" },
                new User { UserId = "user2", FullName = "User 2" }
            }, new PagingInformation(0, 50, 2, 2))
            : new PagedList<User>(new List<User>(), new PagingInformation(0, 50, 0, 0));

        var roles = roleExists
            ? new PagedList<Role>(new List<Role>
            {
                new Role { Id = roleId, Name = "Role 1" },
                new Role { Id = "role2", Name = "Role 2" }
            }, new PagingInformation(0, 50, 2, 2))
            : new PagedList<Role>(new List<Role>(), new PagingInformation(0, 50, 0, 0));

        var removeRolesRequest = new AssignRolesRequest
        {
            Roles = new[] { roleId }
        };

        var validToken = CreateJwtToken();
        SetPrivateField(_authService, "_cachedAccessToken", validToken);

        _usersClientMock.Setup(client => client.GetAllAsync(It.IsAny<GetUsersRequest>(), It.IsAny<PaginationInfo>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(users);

        _rolesClientMock.Setup(client => client.GetAllAsync(It.IsAny<GetRolesRequest>(), It.IsAny<PaginationInfo>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(roles);

        if (removeRoleSuccess)
        {
            _usersClientMock.Setup(client => client.RemoveRolesAsync(userId, removeRolesRequest, It.IsAny<CancellationToken>()))
                            .Returns(Task.CompletedTask);
        }
        else
        {
            _usersClientMock.Setup(client => client.RemoveRolesAsync(userId, removeRolesRequest, It.IsAny<CancellationToken>()))
                            .ThrowsAsync(new CustomException("Failed to remove role"));
        }

        if (userExists && roleExists && removeRoleSuccess)
        {
            await _authService.RemoveRoleFromUserAsync(userId, roleId);

            _usersClientMock.Verify(client => client.RemoveRolesAsync(userId, It.Is<AssignRolesRequest>(req => req.Roles.Contains(roleId)), It.IsAny<CancellationToken>()), Times.Once);
        }
        else
        {
            await Assert.ThrowsAsync<CustomException>(async () => await _authService.RemoveRoleFromUserAsync(userId, roleId));
        }
    }

    [Theory]
    [InlineData("ExistingRole", "Existing Role Description", true)] 
    [InlineData("NewRole", "New Role Description", false)]
    public async Task CreateRoleAsync_Test(string roleName, string description, bool roleAlreadyExists)
    {
        var existingRoles = roleAlreadyExists
            ? new PagedList<Role>(new List<Role>
            {
                new Role { Name = roleName, Description = description },
                new Role { Name = "AnotherRole", Description = "Another Role Description" }
            }, new PagingInformation(0, 50, 2, 2))
            : new PagedList<Role>(new List<Role>
            {
                new Role { Name = "AnotherRole", Description = "Another Role Description" }
            }, new PagingInformation(0, 50, 1, 1));

        var validToken = CreateJwtToken();
        SetPrivateField(_authService, "_cachedAccessToken", validToken);

        var newRole = roleAlreadyExists
            ? null
            : new Role
            {
                Name = roleName,
                Description = description
            };

        if (roleAlreadyExists)
        {
            _managementApiClientMock.Setup(client => client.Roles.GetAllAsync(It.IsAny<GetRolesRequest>(), It.IsAny<PaginationInfo>(), It.IsAny<CancellationToken>()))
                                   .ReturnsAsync(existingRoles);
            _managementApiClientMock.Setup(client => client.Roles.CreateAsync(It.IsAny<RoleCreateRequest>(), It.IsAny<CancellationToken>()))
                                   .Returns(Task.FromResult<Role>(null));
        }
        else
        {
            _managementApiClientMock.Setup(client => client.Roles.GetAllAsync(It.IsAny<GetRolesRequest>(), It.IsAny<PaginationInfo>(), It.IsAny<CancellationToken>()))
                                   .ReturnsAsync(existingRoles);
            _managementApiClientMock.Setup(client => client.Roles.CreateAsync(It.Is<RoleCreateRequest>(req => req.Name == roleName && req.Description == description), It.IsAny<CancellationToken>()))
                                   .ReturnsAsync(newRole);
        }

        if (roleAlreadyExists)
        {
            var result = await _authService.CreateRoleAsync(roleName, description);
            Assert.Equal(roleName, result.Name);
            Assert.Equal(description, result.Description);
            _managementApiClientMock.Verify(client => client.Roles.CreateAsync(It.IsAny<RoleCreateRequest>(), It.IsAny<CancellationToken>()), Times.Never);
        }
        else
        {
            var result = await _authService.CreateRoleAsync(roleName, description);
            Assert.Equal(roleName, result.Name);
            Assert.Equal(description, result.Description);
            _managementApiClientMock.Verify(client => client.Roles.CreateAsync(It.Is<RoleCreateRequest>(req => req.Name == roleName && req.Description == description), It.IsAny<CancellationToken>()), Times.Once);
        }
    }

    [Theory]
    [InlineData("ExistingRole", true)]
    [InlineData("NewRole", false)]
    public async Task DeleteRoleAsync_Test(string roleId, bool roleExists)
    {
        var roles = roleExists
            ? new PagedList<Role>(new List<Role>
            {
                new Role { Id = roleId, Name = "Role 1" },
                new Role { Id = "role2", Name = "Role 2" }
            }, new PagingInformation(0, 50, 2, 2))
            : new PagedList<Role>(new List<Role>
            {
                new Role { Id = "role1", Name = "Role 1" },
                new Role { Id = "role2", Name = "Role 2" }
            }, new PagingInformation(0, 50, 2, 2));

        var validToken = CreateJwtToken();
        SetPrivateField(_authService, "_cachedAccessToken", validToken);

        _managementApiClientMock.Setup(client => client.Roles.GetAllAsync(It.IsAny<GetRolesRequest>(), It.IsAny<PaginationInfo>(), It.IsAny<CancellationToken>()))
                               .ReturnsAsync(roles);

        if (roleExists)
        {
            _managementApiClientMock.Setup(client => client.Roles.DeleteAsync(roleId, It.IsAny<CancellationToken>()))
                                    .Returns(Task.CompletedTask);
        }

        var result = await _authService.DeleteRoleAsync(roleId);

        if (roleExists)
        {
            Assert.True(result);
            _managementApiClientMock.Verify(client => client.Roles.DeleteAsync(roleId, It.IsAny<CancellationToken>()), Times.Once);
        }
        else
        {
            Assert.False(result);
            _managementApiClientMock.Verify(client => client.Roles.DeleteAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
        }
    }

    [Theory]
    [InlineData("ExistingRole", "Updated Role Name", "Updated Description", true)]
    [InlineData("NewRole", "Updated Role Name", "Updated Description", false)]
    public async Task UpdateRoleAsync_Test(string roleId, string newRoleName, string newDescription, bool roleExists)
    {
        var roles = roleExists
            ? new PagedList<Role>(new List<Role>
            {
                new Role { Id = roleId, Name = "Role 1" },
                new Role { Id = "role2", Name = "Role 2" }
            }, new PagingInformation(0, 50, 2, 2))
            : new PagedList<Role>(new List<Role>
            {
                new Role { Id = "role1", Name = "Role 1" },
                new Role { Id = "role2", Name = "Role 2" }
            }, new PagingInformation(0, 50, 2, 2));

        var validToken = CreateJwtToken();
        SetPrivateField(_authService, "_cachedAccessToken", validToken);

        _managementApiClientMock.Setup(client => client.Roles.GetAllAsync(It.IsAny<GetRolesRequest>(), It.IsAny<PaginationInfo>(), It.IsAny<CancellationToken>()))
                               .ReturnsAsync(roles);

        if (roleExists)
        {
            _managementApiClientMock.Setup(client => client.Roles.UpdateAsync(roleId, It.IsAny<RoleUpdateRequest>(), It.IsAny<CancellationToken>()))
                                    .ReturnsAsync(new Role { Id = roleId, Name = newRoleName, Description = newDescription });
        }

        var result = await _authService.UpdateRoleAsync(roleId, newRoleName, newDescription);

        if (roleExists)
        {
            Assert.True(result);
            _managementApiClientMock.Verify(client => client.Roles.UpdateAsync(roleId, It.Is<RoleUpdateRequest>(r => r.Name == newRoleName && r.Description == newDescription), It.IsAny<CancellationToken>()), Times.Once);
        }
        else
        {
            Assert.False(result);
            _managementApiClientMock.Verify(client => client.Roles.UpdateAsync(It.IsAny<string>(), It.IsAny<RoleUpdateRequest>(), It.IsAny<CancellationToken>()), Times.Never);
        }
    }

    [Theory]
    [InlineData("ExistingRole", true)]
    [InlineData("NewRole", false)]
    public async Task GetPermissionsByRoleAsync_Test(string roleId, bool roleExists)
    {
        var roles = roleExists
            ? new PagedList<Role>(new List<Role>
            {
                new Role { Id = roleId, Name = "Role 1" },
                new Role { Id = "role2", Name = "Role 2" }
            }, new PagingInformation(0, 50, 2, 2))
            : new PagedList<Role>(new List<Role>
            {
                new Role { Id = "role1", Name = "Role 1" },
                new Role { Id = "role2", Name = "Role 2" }
            }, new PagingInformation(0, 50, 2, 2));

        var permissions = roleExists
            ? new PagedList<Permission>(new List<Permission>
            {
                new Permission { Name = "perm1", Description = "Permission 1" },
                new Permission { Name = "perm2", Description = "Permission 2" }
            }, new PagingInformation(0, 50, 2, 2))
            : new PagedList<Permission>(new List<Permission>(), new PagingInformation(0, 50, 0, 0));

        var validToken = CreateJwtToken();
        SetPrivateField(_authService, "_cachedAccessToken", validToken);

        _managementApiClientMock.Setup(client => client.Roles.GetAllAsync(It.IsAny<GetRolesRequest>(), It.IsAny<PaginationInfo>(), It.IsAny<CancellationToken>()))
                               .ReturnsAsync(roles);

        if (roleExists)
        {
            _managementApiClientMock.Setup(client => client.Roles.GetPermissionsAsync(roleId, It.IsAny<PaginationInfo>(), It.IsAny<CancellationToken>()))
                                   .ReturnsAsync(permissions);
        }

        if (roleExists)
        {
            var result = await _authService.GetPermissionsByRoleAsync(roleId);

            Assert.Equal(2, result.Count);
            Assert.Contains(result, p => p.Name == "perm1");
            Assert.Contains(result, p => p.Name == "perm2");
            _managementApiClientMock.Verify(client => client.Roles.GetPermissionsAsync(roleId, It.IsAny<PaginationInfo>(), It.IsAny<CancellationToken>()), Times.Once);
        }
        else
        {
            await Assert.ThrowsAsync<CustomException>(async () => await _authService.GetPermissionsByRoleAsync(roleId));
        }
    }

    [Theory]
    [InlineData("ExistingRole", "read:profile", true)]
    [InlineData("NewRole", "read:profile", false)]
    public async Task RemovePermissionsFromRoleAsync_Test(string roleId, string permissionName, bool roleExists)
    {
        var roles = roleExists
            ? new PagedList<Role>(new List<Role>
            {
                new Role { Id = roleId, Name = "Role 1" },
                new Role { Id = "role2", Name = "Role 2" }
            }, new PagingInformation(0, 50, 2, 2))
            : new PagedList<Role>(new List<Role>
            {
                new Role { Id = "role1", Name = "Role 1" },
                new Role { Id = "role2", Name = "Role 2" }
            }, new PagingInformation(0, 50, 2, 2));

        var validToken = CreateJwtToken();
        SetPrivateField(_authService, "_cachedAccessToken", validToken);

        _managementApiClientMock.Setup(client => client.Roles.GetAllAsync(It.IsAny<GetRolesRequest>(), It.IsAny<PaginationInfo>(), It.IsAny<CancellationToken>()))
                               .ReturnsAsync(roles);

        if (roleExists)
        {
            _managementApiClientMock.Setup(client => client.Roles.RemovePermissionsAsync(roleId, It.Is<AssignPermissionsRequest>(req => req.Permissions.Any(p => p.Name == permissionName)), It.IsAny<CancellationToken>()))
                                   .Returns(Task.CompletedTask);
        }

        var result = await _authService.RemovePermissionsFromRoleAsync(roleId, permissionName);

        if (roleExists)
        {
            Assert.True(result);
            _managementApiClientMock.Verify(client => client.Roles.RemovePermissionsAsync(roleId, It.Is<AssignPermissionsRequest>(req => req.Permissions.Any(p => p.Name == permissionName)), It.IsAny<CancellationToken>()), Times.Once);
        }
        else
        {
            Assert.False(result);
            _managementApiClientMock.Verify(client => client.Roles.RemovePermissionsAsync(It.IsAny<string>(), It.IsAny<AssignPermissionsRequest>(), It.IsAny<CancellationToken>()), Times.Never);
        }
    }

    [Theory]
    [InlineData("ExistingRole", "read:profile", true)]
    [InlineData("NewRole", "read:profile", false)]
    public async Task AddPermissionsToRoleAsync_Test(string roleId, string permissionName, bool roleExists)
    {
        var roles = roleExists
            ? new PagedList<Role>(new List<Role>
            {
                new Role { Id = roleId, Name = "Role 1" },
                new Role { Id = "role2", Name = "Role 2" }
            }, new PagingInformation(0, 50, 2, 2))
            : new PagedList<Role>(new List<Role>
            {
                new Role { Id = "role1", Name = "Role 1" },
                new Role { Id = "role2", Name = "Role 2" }
            }, new PagingInformation(0, 50, 2, 2));

        var validToken = CreateJwtToken();
        SetPrivateField(_authService, "_cachedAccessToken", validToken);

        _managementApiClientMock.Setup(client => client.Roles.GetAllAsync(It.IsAny<GetRolesRequest>(), It.IsAny<PaginationInfo>(), It.IsAny<CancellationToken>()))
                               .ReturnsAsync(roles);

        if (roleExists)
        {
            _managementApiClientMock.Setup(client => client.Roles.AssignPermissionsAsync(roleId, It.IsAny<AssignPermissionsRequest>(), It.IsAny<CancellationToken>()))
                                   .Returns(Task.CompletedTask);
        }

        var result = await _authService.AddPermissionsToRoleAsync(roleId, permissionName);

        if (roleExists)
        {
            Assert.True(result);
            _managementApiClientMock.Verify(client => client.Roles.AssignPermissionsAsync(roleId, It.Is<AssignPermissionsRequest>(req => req.Permissions.Any(p => p.Name == permissionName)), It.IsAny<CancellationToken>()), Times.Once);
        }
        else
        {
            Assert.False(result);
            _managementApiClientMock.Verify(client => client.Roles.AssignPermissionsAsync(It.IsAny<string>(), It.IsAny<AssignPermissionsRequest>(), It.IsAny<CancellationToken>()), Times.Never);
        }
    }

    [Theory]
    [InlineData("existingUserId", false)]
    [InlineData("anotherUserId", false)]
    [InlineData("newUserId", true)]
    public async Task CreateUser_Test(string userIdToCreate, bool expectedResult)
    {
        var request = new UserCreateRequest { UserId = userIdToCreate };

        var existingUsers = new PagedList<User>(new List<User>
        {
            new User { UserId = "existingUserId", FullName = "Existing User" },
            new User { UserId = "anotherUserId", FullName = "Another User" }
        }, new PagingInformation(0, 50, 2, 2));

        var validToken = CreateJwtToken();
        SetPrivateField(_authService, "_cachedAccessToken", validToken);

        _usersClientMock.Setup(client => client.GetAllAsync(It.IsAny<GetUsersRequest>(), It.IsAny<PaginationInfo>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(existingUsers);

        var result = await _authService.CreateUser(request);

        Assert.Equal(expectedResult, result);
        if (!expectedResult)
        {
            _managementApiClientMock.Verify(client => client.Users.CreateAsync(It.IsAny<UserCreateRequest>(), It.IsAny<CancellationToken>()), Times.Never);
        }
        else
        {
            _managementApiClientMock.Verify(client => client.Users.CreateAsync(It.IsAny<UserCreateRequest>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }

    [Theory]
    [InlineData("existingUser", "existingUser", "test1@retrorabbit.co.za", true)]
    [InlineData("nonexistentUser", null, null, false)]
    public async Task GetUserById_Test(string userIdToGet, string expectedUserId, string expectedEmail, bool userExists)
    {
        var existingUsers = new PagedList<User>(new List<User>
    {
        new User { UserId = "existingUser", Email = "test1@retrorabbit.co.za" },
        new User { UserId = "user2", Email = "test2@retrorabbit.co.za" }
    }, new PagingInformation(0, 50, 2, 2));

        var validToken = CreateJwtToken();
        SetPrivateField(_authService, "_cachedAccessToken", validToken);

        _managementApiClientMock.Setup(client => client.Users.GetAllAsync(It.IsAny<GetUsersRequest>(), It.IsAny<PaginationInfo>(), It.IsAny<CancellationToken>()))
                                .ReturnsAsync(existingUsers);

        if (userExists)
        {
            _usersClientMock.Setup(client => client.GetAsync(userIdToGet, null, true, It.IsAny<CancellationToken>()))
                            .ReturnsAsync(new User { UserId = expectedUserId, Email = expectedEmail });
        }
        else
        {
            _usersClientMock.Setup(client => client.GetAsync(userIdToGet, null, true, It.IsAny<CancellationToken>()))
                            .ThrowsAsync(new KeyNotFoundException("User not found."));
        }

        _managementApiClientMock.Setup(client => client.Users).Returns(_usersClientMock.Object);

        if (userExists)
        {
            var result = await _authService.GetUserById(userIdToGet);
            Assert.NotNull(result);
            Assert.Equal(expectedUserId, result.UserId);
            Assert.Equal(expectedEmail, result.Email);
        }
        else
        {
            var exception = await Assert.ThrowsAsync<CustomException>(() => _authService.GetUserById(userIdToGet));
            Assert.Equal("User not found.", exception.Message);
        }
    }

    [Theory]
    [InlineData("existingUser", "test1@retrorabbit.co.za", true, true)]
    [InlineData("nonexistentUser", null, false, false)]
    [InlineData("", null, false, false)]
    [InlineData(null, null, false, false)]
    public async Task DeleteUser_Test(string userId, string userEmail, bool userExists, bool expectedResult)
    {
        var validToken = CreateJwtToken();
        SetPrivateField(_authService, "_cachedAccessToken", validToken);

        if (!string.IsNullOrEmpty(userId))
        {
            var expectedUser = userExists ? new User { UserId = userId, Email = userEmail } : null;

            _usersClientMock.Setup(client => client.GetAsync(userId, null, true, It.IsAny<CancellationToken>()))
                            .ReturnsAsync(expectedUser);

            if (userExists)
            {
                _usersClientMock.Setup(client => client.DeleteAsync(userId))
                                .Returns(Task.CompletedTask);
            }
        }
        else
        {
            _usersClientMock.Setup(client => client.GetAsync(It.IsAny<string>(), null, true, It.IsAny<CancellationToken>()))
                            .ThrowsAsync(new ArgumentException("Invalid userId"));
        }

        _managementApiClientMock.Setup(client => client.Users).Returns(_usersClientMock.Object);

        var result = await _authService.DeleteUser(userId);

        Assert.Equal(expectedResult, result);

        if (!string.IsNullOrEmpty(userId))
        {
            if (userExists)
            {
                _usersClientMock.Verify(client => client.DeleteAsync(userId), Times.Once);
            }
            else
            {
                _usersClientMock.Verify(client => client.DeleteAsync(userId), Times.Never);
            }
        }
        else
        {
            _usersClientMock.Verify(client => client.DeleteAsync(It.IsAny<string>()), Times.Never);
        }
    }

    [Theory]
    [InlineData("user1", "userNameToBeUpdated", "updatedEmail@retrorabbit.co.za", true, true)]
    [InlineData("nonexistentUser", "updatedUserName", "updatedEmail@retrorabbit.co.za", false, false)]
    public async Task UpdateUser_Test(string userId, string updatedUserName, string updatedEmail, bool userExists, bool expectedResult)
    {
        var request = new UserUpdateRequest
        {
            UserName = updatedUserName,
            Email = updatedEmail
        };

        var existingUsers = userExists
            ? new PagedList<User>(new List<User>
            {
            new User { UserId = userId, UserName = "oldUserName", Email = "oldEmail@retrorabbit.co.za" },
            new User { UserId = "user2", UserName = "userName2", Email = "email2@retrorabbit.co.za" }
            }, new PagingInformation(0, 50, 2, 2))
            : new PagedList<User>(new List<User>
            {
            new User { UserId = "user2", UserName = "userName2", Email = "email2@retrorabbit.co.za" }
            }, new PagingInformation(0, 50, 1, 1));

        var validToken = CreateJwtToken();
        SetPrivateField(_authService, "_cachedAccessToken", validToken);

        _usersClientMock.Setup(client => client.GetAllAsync(It.IsAny<GetUsersRequest>(), It.IsAny<PaginationInfo>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(existingUsers);

        if (userExists)
        {
            _usersClientMock.Setup(client => client.UpdateAsync(userId, It.Is<UserUpdateRequest>(r =>
                r.UserName == request.UserName &&
                r.Email == request.Email), It.IsAny<CancellationToken>()))
                            .ReturnsAsync(new User { UserId = userId, UserName = request.UserName, Email = request.Email });
        }

        var result = await _authService.UpdateUser(userId, request);

        Assert.Equal(expectedResult, result);

        if (userExists)
        {
            _usersClientMock.Verify(client => client.UpdateAsync(userId, It.Is<UserUpdateRequest>(r =>
                r.UserName == request.UserName &&
                r.Email == request.Email), It.IsAny<CancellationToken>()), Times.Once);
        }
        else
        {
            _usersClientMock.Verify(client => client.UpdateAsync(It.IsAny<string>(), It.IsAny<UserUpdateRequest>(), It.IsAny<CancellationToken>()), Times.Never);
        }
    }

}