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
using HRIS.Services.Interfaces;
using HRIS.Services.Tests.Helpers;
using System.Reflection;
using Auth0.ManagementApi.Clients;

namespace HRIS.Services.Tests.Services;

public class AuthServiceUnitTests
{
    private readonly Mock<IOptions<AuthManagement>> _authManagementOptionsMock;
    private readonly Mock<HttpMessageHandler> _httpMessageHandlerMock;
    private readonly Mock<IRolesClient> _rolesClientMock;
    private readonly Mock<IAuthService> _authServiceMock;
    private readonly HttpClient _httpClient;
    private AuthService _authService;

    public AuthServiceUnitTests()
    {
        var authManagement = new AuthManagement
        {
            ClientId = "test-client-id",
            ClientSecret = "test-client-secret",
            Issuer = "https://test-issuer/",
            Audience = "test-audience"
        };

        _authManagementOptionsMock = new Mock<IOptions<AuthManagement>>();
        _authManagementOptionsMock.Setup(opt => opt.Value).Returns(authManagement);

        _httpMessageHandlerMock = new Mock<HttpMessageHandler>();
        _httpClient = new HttpClient(_httpMessageHandlerMock.Object);

        _rolesClientMock = new Mock<IRolesClient>();
        _authServiceMock = new Mock<IAuthService>();
        _authService = new AuthService(_authManagementOptionsMock.Object);

        var managementApiClientField = typeof(AuthService).GetField("_managementApiClient", BindingFlags.NonPublic | BindingFlags.Instance);
        managementApiClientField?.SetValue(_authService, new Mock<IManagementApiClient>().Object);

        var httpClientField = typeof(AuthService).GetField("_httpClient", BindingFlags.NonPublic | BindingFlags.Instance);
        httpClientField?.SetValue(_authService, _httpClient);
    }

    [Fact]
    public async Task GetToken_Successful()
    {
        _authServiceMock.Setup(svc => svc.GetAuth0ManagementAccessToken())
                        .ReturnsAsync("mocked_token");

        var authService = _authServiceMock.Object;
        var result = await _authServiceMock.Object.GetAuth0ManagementAccessToken();
        Assert.Equal("mocked_token", result);
    }

    [Fact]
    public async Task GetToken_Failure()
    {
        _authServiceMock.Setup(svc => svc.GetAuth0ManagementAccessToken())
                        .ThrowsAsync(new Exception("Token retrieval failed"));

        var authService = _authServiceMock.Object;
        await Assert.ThrowsAsync<Exception>(() => authService.GetAuth0ManagementAccessToken());
    }

    private string CreateJwtToken(bool notExpired = true, bool hasExpClaim = true)
    {
        var securityKey = new SymmetricSecurityKey(System.Text.Encoding.Default.GetBytes("test_secret_key_which_must_be_a_longer_than_128bits"));

        // Note for developer: We use an asymmetric key, but for testing we can simply use a symmetric key!
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

    [Fact]
    public void IsTokenExpired_ReturnsTrue_ForExpiredToken()
    {
        var expired = CreateJwtToken(false);
        var result = AuthService.IsTokenExpired(expired);
        Assert.True(result);
    }

    [Fact]
    public void IsTokenExpired_ReturnsFalse_ForValidToken()
    {
        var valid = CreateJwtToken();
        var result = AuthService.IsTokenExpired(valid);
        Assert.False(result);
    }

    [Fact]
    public void IsTokenExpired_ReturnsTrue_WhenExpClaimIsNullOrEmpty()
    {
        var tokenWithoutExpClaim = CreateJwtToken(true, false);
        var result = AuthService.IsTokenExpired(tokenWithoutExpClaim);
        Assert.True(result);
    }

    [Fact]
    public async Task GetAuth0ManagementAccessToken_ReturnsCachedToken_IfNotExpired()
    {
        var cachedTokenField = typeof(AuthService).GetField("_cachedAccessToken", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var valid = CreateJwtToken();
        cachedTokenField?.SetValue(_authService, valid);
        var result = await _authService.GetAuth0ManagementAccessToken();
        Assert.Equal(valid, result);
    }

    [Fact]
    public async Task GetAuth0ManagementAccessToken_FetchesNewToken_IfCachedTokenIsExpired()
    {
        var expired = CreateJwtToken(false, true);
        var expiredTokenField = typeof(AuthService).GetField("_cachedAccessToken", BindingFlags.NonPublic | BindingFlags.Instance);
        expiredTokenField?.SetValue(_authService, expired);

        var httpClient = CreateHttpClientMock(HttpStatusCode.OK, "{\"access_token\": \"new_token\"}");
        var httpClientField = typeof(AuthService).GetField("_httpClient", BindingFlags.NonPublic | BindingFlags.Instance);
        httpClientField?.SetValue(_authService, httpClient);

        var result = await _authService.GetAuth0ManagementAccessToken();

        Assert.Equal("new_token", result);
    }

    [Fact]
    public async Task GetAuth0ManagementAccessToken_ThrowsException_IfTokenRequestFails()
    {
        var expiredTokenField = typeof(AuthService).GetField("_cachedAccessToken", BindingFlags.NonPublic | BindingFlags.Instance);
        expiredTokenField?.SetValue(_authService, string.Empty);

        var httpClient = CreateHttpClientMock(HttpStatusCode.BadRequest, "error");
        var httpClientField = typeof(AuthService).GetField("_httpClient", BindingFlags.NonPublic | BindingFlags.Instance);
        httpClientField?.SetValue(_authService, httpClient);

        await Assert.ThrowsAsync<CustomException>(() => _authService.GetAuth0ManagementAccessToken());
    }

    [Fact]
    public async Task GetAuth0ManagementAccessToken_ThrowsException_IfAccessTokenNotFound()
    {
        var httpClient = CreateHttpClientMock(HttpStatusCode.OK, "{\"invalid_key\": \"value\"}");
        var httpClientField = typeof(AuthService).GetField("_httpClient", BindingFlags.NonPublic | BindingFlags.Instance);
        httpClientField?.SetValue(_authService, httpClient);

        await Assert.ThrowsAsync<CustomException>(() => _authService.GetAuth0ManagementAccessToken());
    }

    [Fact]
    public async Task GetAllRolesAsync_ReturnsRoles()
    {
        var pagination = new PagingInformation(start: 0, limit: 50, length: 10, total: 10);
        var roles = new PagedList<Role>(new List<Role>
        {
            new Role { Id = "role1", Name = "Role 1" },
            new Role { Id = "role2", Name = "Role 2" }
        }, pagination);

        var validToken = CreateJwtToken();
        var cachedTokenField = typeof(AuthService).GetField("_cachedAccessToken", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        cachedTokenField?.SetValue(_authService, validToken);

        var managementApiClientMock = new Mock<IManagementApiClient>();
        managementApiClientMock.Setup(client => client.Roles.GetAllAsync(It.IsAny<GetRolesRequest>(), It.IsAny<PaginationInfo>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(roles);

        var managementApiClientField = typeof(AuthService).GetField("_managementApiClient", BindingFlags.NonPublic | BindingFlags.Instance);
        managementApiClientField?.SetValue(_authService, managementApiClientMock.Object);

        var result = await _authService.GetAllRolesAsync();

        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.Equal("role1", result[0].Id);
        Assert.Equal("Role 1", result[0].Name);
    }

    [Fact]
    public async Task GetAllRolesAsync_FailsToRetrieveRoles()
    {
        var validToken = CreateJwtToken();
        var cachedTokenField = typeof(AuthService).GetField("_cachedAccessToken", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        cachedTokenField?.SetValue(_authService, validToken);

        var managementApiClientMock = new Mock<IManagementApiClient>();
        managementApiClientMock.Setup(client => client.Roles.GetAllAsync(It.IsAny<GetRolesRequest>(), It.IsAny<PaginationInfo>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync((IPagedList<Role>)null);

        var managementApiClientField = typeof(AuthService).GetField("_managementApiClient", BindingFlags.NonPublic | BindingFlags.Instance);
        managementApiClientField?.SetValue(_authService, managementApiClientMock.Object);

        await Assert.ThrowsAsync<CustomException>(async () => await _authService.GetAllRolesAsync());
    }

    [Fact]
    public async Task GetAllUsersAsync_ReturnsUsers()
    {
        var pagination = new PagingInformation(start: 0, limit: 50, length: 10, total: 10);
        var users = new PagedList<User>(new List<User>
        {
            new User { UserId = "1", Email = "test1@retrorabbit.co.za" },
            new User { UserId = "2", Email = "test2@retrorabbit.co.za" }
        }, pagination);

        var validToken = CreateJwtToken();
        var cachedTokenField = typeof(AuthService).GetField("_cachedAccessToken", BindingFlags.NonPublic | BindingFlags.Instance);
        cachedTokenField?.SetValue(_authService, validToken);

        var usersClientMock = new Mock<IUsersClient>();
        usersClientMock.Setup(client => client.GetAllAsync(It.IsAny<GetUsersRequest>(), It.IsAny<PaginationInfo>(), It.IsAny<CancellationToken>()))
                       .ReturnsAsync(users);

        var managementApiClientMock = new Mock<IManagementApiClient>();
        managementApiClientMock.Setup(client => client.Users).Returns(usersClientMock.Object);

        var managementApiClientField = typeof(AuthService).GetField("_managementApiClient", BindingFlags.NonPublic | BindingFlags.Instance);
        managementApiClientField?.SetValue(_authService, managementApiClientMock.Object);

        var result = await _authService.GetAllUsersAsync();

        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.Equal("1", result[0].UserId);
        Assert.Equal("test1@retrorabbit.co.za", result[0].Email);
    }

    [Fact]
    public async Task GetAllUsersAsync_FailsToRetrieveUsers()
    {
        var validToken = CreateJwtToken();
        var cachedTokenField = typeof(AuthService).GetField("_cachedAccessToken", BindingFlags.NonPublic | BindingFlags.Instance);
        cachedTokenField?.SetValue(_authService, validToken);

        var usersClientMock = new Mock<IUsersClient>();
        usersClientMock.Setup(client => client.GetAllAsync(It.IsAny<GetUsersRequest>(), It.IsAny<PaginationInfo>(), It.IsAny<CancellationToken>()))
                       .ReturnsAsync((IPagedList<User>)null);

        var managementApiClientMock = new Mock<IManagementApiClient>();
        managementApiClientMock.Setup(client => client.Users).Returns(usersClientMock.Object);

        var managementApiClientField = typeof(AuthService).GetField("_managementApiClient", BindingFlags.NonPublic | BindingFlags.Instance);
        managementApiClientField?.SetValue(_authService, managementApiClientMock.Object);

        await Assert.ThrowsAsync<CustomException>(async () => await _authService.GetAllUsersAsync());
    }

    [Fact]
    public async Task GetUsersByEmailAsync_ReturnsUsers()
    {
        var email = "test@example.com";
        var users = new List<User>
        {
            new User { UserId = "user1", Email = email, FullName = "User 1" },
            new User { UserId = "user2", Email = email, FullName = "User 2" }
        };

        var validToken = CreateJwtToken();
        var cachedTokenField = typeof(AuthService).GetField("_cachedAccessToken", BindingFlags.NonPublic | BindingFlags.Instance);
        cachedTokenField?.SetValue(_authService, validToken);

        var usersClientMock = new Mock<IUsersClient>();
        usersClientMock.Setup(client => client.GetUsersByEmailAsync(email, null, null, It.IsAny<CancellationToken>()))
                       .ReturnsAsync(users);

        var managementApiClientMock = new Mock<IManagementApiClient>();
        managementApiClientMock.Setup(client => client.Users).Returns(usersClientMock.Object);

        var managementApiClientField = typeof(AuthService).GetField("_managementApiClient", BindingFlags.NonPublic | BindingFlags.Instance);
        managementApiClientField?.SetValue(_authService, managementApiClientMock.Object);

        var result = await _authService.GetUsersByEmailAsync(email);

        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.Equal("user1", result[0].UserId);
        Assert.Equal(email, result[0].Email);
        Assert.Equal("User 1", result[0].FullName);
    }

    [Fact]
    public async Task GetUsersByEmailAsync_UserNotFound_ThrowsCustomException()
    {
        var email = "nonexistent@example.com";

        var validToken = CreateJwtToken();
        var cachedTokenField = typeof(AuthService).GetField("_cachedAccessToken", BindingFlags.NonPublic | BindingFlags.Instance);
        cachedTokenField?.SetValue(_authService, validToken);

        var usersClientMock = new Mock<IUsersClient>();
        usersClientMock.Setup(client => client.GetUsersByEmailAsync(email, null, null, It.IsAny<CancellationToken>()))
                       .ReturnsAsync((IList<User>)null);

        var managementApiClientMock = new Mock<IManagementApiClient>();
        managementApiClientMock.Setup(client => client.Users).Returns(usersClientMock.Object);

        var managementApiClientField = typeof(AuthService).GetField("_managementApiClient", BindingFlags.NonPublic | BindingFlags.Instance);
        managementApiClientField?.SetValue(_authService, managementApiClientMock.Object);

        await Assert.ThrowsAsync<CustomException>(() => _authService.GetUsersByEmailAsync(email));
    }

    [Fact]
    public async Task GetUsersByRoleAsync_ReturnsUsers()
    {
        var roleId = "role1";
        var pagination = new PagingInformation(start: 0, limit: 50, length: 10, total: 10);

        var roles = new PagedList<Role>(new List<Role>
        {
            new Role { Id = roleId, Name = "Role 1" },
            new Role { Id = "role2", Name = "Role 2" }
        }, pagination);

        var assignedUsers = new PagedList<AssignedUser>(new List<AssignedUser>
        {
            new AssignedUser { UserId = "user1", FullName = "User 1" },
            new AssignedUser { UserId = "user2", FullName = "User 2" }
        }, new PagingInformation(0, 50, 2, 2));

        var validToken = CreateJwtToken();
        var cachedTokenField = typeof(AuthService).GetField("_cachedAccessToken", BindingFlags.NonPublic | BindingFlags.Instance);
        cachedTokenField?.SetValue(_authService, validToken);

        var managementApiClientMock = new Mock<IManagementApiClient>();

        managementApiClientMock.Setup(client => client.Roles.GetAllAsync(It.IsAny<GetRolesRequest>(), It.IsAny<PaginationInfo>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(roles);

        managementApiClientMock.Setup(client => client.Roles.GetUsersAsync(roleId, It.IsAny<PaginationInfo>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(assignedUsers);

        var managementApiClientField = typeof(AuthService).GetField("_managementApiClient", BindingFlags.NonPublic | BindingFlags.Instance);
        managementApiClientField?.SetValue(_authService, managementApiClientMock.Object);

        var result = await _authService.GetUsersByRoleAsync(roleId);

        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.Equal("user1", result[0].UserId);
        Assert.Equal("User 1", result[0].FullName);
    }

    [Fact]
    public async Task GetUsersByRoleAsync_RoleNotFound()
    {
        var roleId = "nonexistentRole";
        var roles = new PagedList<Role>(new List<Role>
        {
            new Role { Id = "role1", Name = "Role 1" },
            new Role { Id = "role2", Name = "Role 2" }
        }, new PagingInformation(0, 50, 2, 2));

        var validToken = CreateJwtToken();
        var cachedTokenField = typeof(AuthService).GetField("_cachedAccessToken", BindingFlags.NonPublic | BindingFlags.Instance);
        cachedTokenField?.SetValue(_authService, validToken);

        var managementApiClientMock = new Mock<IManagementApiClient>();
        managementApiClientMock.Setup(client => client.Roles.GetAllAsync(It.IsAny<GetRolesRequest>(), It.IsAny<PaginationInfo>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(roles);

        var managementApiClientField = typeof(AuthService).GetField("_managementApiClient", BindingFlags.NonPublic | BindingFlags.Instance);
        managementApiClientField?.SetValue(_authService, managementApiClientMock.Object);

        await Assert.ThrowsAsync<CustomException>(async () => await _authService.GetUsersByRoleAsync(roleId));
    }
    
    [Fact]
    public async Task GetUsersByRoleAsync_FailsToRetrieveUsers()
    {
        var roleId = "role1";
        var pagination = new PagingInformation(start: 0, limit: 50, length: 10, total: 10);

        var roles = new PagedList<Role>(new List<Role>
        {
            new Role { Id = roleId, Name = "Role 1" },
            new Role { Id = "role2", Name = "Role 2" }
        }, pagination);

        var validToken = CreateJwtToken();
        var cachedTokenField = typeof(AuthService).GetField("_cachedAccessToken", BindingFlags.NonPublic | BindingFlags.Instance);
        cachedTokenField?.SetValue(_authService, validToken);

        var managementApiClientMock = new Mock<IManagementApiClient>();

        managementApiClientMock.Setup(client => client.Roles.GetAllAsync(It.IsAny<GetRolesRequest>(), It.IsAny<PaginationInfo>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(roles);

        managementApiClientMock.Setup(client => client.Roles.GetUsersAsync(roleId, It.IsAny<PaginationInfo>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync((IPagedList<AssignedUser>)null);

        var managementApiClientField = typeof(AuthService).GetField("_managementApiClient", BindingFlags.NonPublic | BindingFlags.Instance);
        managementApiClientField?.SetValue(_authService, managementApiClientMock.Object);

        await Assert.ThrowsAsync<CustomException>(async () => await _authService.GetUsersByRoleAsync(roleId));
    }

    [Fact]
    public async Task GetUserRolesAsync_ReturnsRoles()
    {
        var userId = "user1";
        var pagination = new PagingInformation(start: 0, limit: 50, length: 10, total: 10);

        var users = new PagedList<User>(new List<User>
    {
        new User { UserId = userId, FullName = "User 1" },
        new User { UserId = "user2", FullName = "User 2" }
    }, pagination);

        var userRoles = new PagedList<Role>(new List<Role>
    {
        new Role { Id = "role1", Name = "Role 1" },
        new Role { Id = "role2", Name = "Role 2" }
    }, new PagingInformation(0, 50, 2, 2));

        var validToken = CreateJwtToken();
        var cachedTokenField = typeof(AuthService).GetField("_cachedAccessToken", BindingFlags.NonPublic | BindingFlags.Instance);
        cachedTokenField?.SetValue(_authService, validToken);

        var managementApiClientMock = new Mock<IManagementApiClient>();

        managementApiClientMock.Setup(client => client.Users.GetAllAsync(It.IsAny<GetUsersRequest>(), It.IsAny<PaginationInfo>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(users);

        managementApiClientMock.Setup(client => client.Users.GetRolesAsync(userId, It.IsAny<PaginationInfo>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(userRoles);

        var managementApiClientField = typeof(AuthService).GetField("_managementApiClient", BindingFlags.NonPublic | BindingFlags.Instance);
        managementApiClientField?.SetValue(_authService, managementApiClientMock.Object);

        var result = await _authService.GetUserRolesAsync(userId);

        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.Equal("role1", result[0].Id);
        Assert.Equal("Role 1", result[0].Name);
    }

    [Fact]
    public async Task GetUserRolesAsync_UserNotFound()
    {
        var userId = "nonexistentUser";
        var users = new PagedList<User>(new List<User>
    {
        new User { UserId = "user1", FullName = "User 1" },
        new User { UserId = "user2", FullName = "User 2" }
    }, new PagingInformation(0, 50, 2, 2));

        var validToken = CreateJwtToken();
        var cachedTokenField = typeof(AuthService).GetField("_cachedAccessToken", BindingFlags.NonPublic | BindingFlags.Instance);
        cachedTokenField?.SetValue(_authService, validToken);

        var managementApiClientMock = new Mock<IManagementApiClient>();
        managementApiClientMock.Setup(client => client.Users.GetAllAsync(It.IsAny<GetUsersRequest>(), It.IsAny<PaginationInfo>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(users);

        var managementApiClientField = typeof(AuthService).GetField("_managementApiClient", BindingFlags.NonPublic | BindingFlags.Instance);
        managementApiClientField?.SetValue(_authService, managementApiClientMock.Object);

        await Assert.ThrowsAsync<CustomException>(async () => await _authService.GetUserRolesAsync(userId));
    }

    [Fact]
    public async Task GetUserRolesAsync_FailsToRetrieveRoles()
    {
        var userId = "user1";
        var pagination = new PagingInformation(start: 0, limit: 50, length: 10, total: 10);

        var users = new PagedList<User>(new List<User>
    {
        new User { UserId = userId, FullName = "User 1" },
        new User { UserId = "user2", FullName = "User 2" }
    }, pagination);

        var validToken = CreateJwtToken();
        var cachedTokenField = typeof(AuthService).GetField("_cachedAccessToken", BindingFlags.NonPublic | BindingFlags.Instance);
        cachedTokenField?.SetValue(_authService, validToken);

        var managementApiClientMock = new Mock<IManagementApiClient>();

        managementApiClientMock.Setup(client => client.Users.GetAllAsync(It.IsAny<GetUsersRequest>(), It.IsAny<PaginationInfo>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(users);

        managementApiClientMock.Setup(client => client.Users.GetRolesAsync(userId, It.IsAny<PaginationInfo>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync((IPagedList<Role>)null);

        var managementApiClientField = typeof(AuthService).GetField("_managementApiClient", BindingFlags.NonPublic | BindingFlags.Instance);
        managementApiClientField?.SetValue(_authService, managementApiClientMock.Object);

        await Assert.ThrowsAsync<CustomException>(async () => await _authService.GetUserRolesAsync(userId));
    }

    [Fact]
    public async Task AddRoleToUserAsync_Success()
    {
        var userId = "user1";
        var roleId = "role1";

        var users = new PagedList<User>(new List<User>
        {
            new User { UserId = userId, FullName = "User 1" },
            new User { UserId = "user2", FullName = "User 2" }
        }, new PagingInformation(0, 50, 2, 2));

        var roles = new PagedList<Role>(new List<Role>
        {
            new Role { Id = roleId, Name = "Role 1" },
            new Role { Id = "role2", Name = "Role 2" }
        }, new PagingInformation(0, 50, 2, 2));

        var AssignedRoles = new AssignRolesRequest()
        {
           Roles = new string[] { "role1" },

        };

        var validToken = CreateJwtToken();
        var cachedTokenField = typeof(AuthService).GetField("_cachedAccessToken", BindingFlags.NonPublic | BindingFlags.Instance);
        cachedTokenField?.SetValue(_authService, validToken);

        var managementApiClientMock = new Mock<IManagementApiClient>();

        managementApiClientMock.Setup(client => client.Users.GetAllAsync(It.IsAny<GetUsersRequest>(), It.IsAny<PaginationInfo>(), It.IsAny<CancellationToken>()))
                               .ReturnsAsync(users);

        managementApiClientMock.Setup(client => client.Roles.GetAllAsync(It.IsAny<GetRolesRequest>(), It.IsAny<PaginationInfo>(), It.IsAny<CancellationToken>()))
                               .ReturnsAsync(roles);

        managementApiClientMock.Setup(client => client.Users.AssignRolesAsync(userId, AssignedRoles, It.IsAny<CancellationToken>()))
                               .Returns(Task.CompletedTask);

        var managementApiClientField = typeof(AuthService).GetField("_managementApiClient", BindingFlags.NonPublic | BindingFlags.Instance);
        managementApiClientField?.SetValue(_authService, managementApiClientMock.Object);

        await _authService.AddRoleToUserAsync(userId, roleId);

        managementApiClientMock.Verify(client => client.Users.AssignRolesAsync(userId, It.Is<AssignRolesRequest>(roles => roles.Roles.Contains(roleId)), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task AddRoleToUserAsync_UserNotFound()
    {
        var userId = "nonexistentUser";
        var roleId = "role1";

        var roles = new PagedList<Role>(new List<Role>
        {
            new Role { Id = roleId, Name = "Role 1" },
            new Role { Id = "role2", Name = "Role 2" }
        }, new PagingInformation(0, 50, 2, 2));

        var validToken = CreateJwtToken();
        var cachedTokenField = typeof(AuthService).GetField("_cachedAccessToken", BindingFlags.NonPublic | BindingFlags.Instance);
        cachedTokenField?.SetValue(_authService, validToken);

        var managementApiClientMock = new Mock<IManagementApiClient>();
        managementApiClientMock.Setup(client => client.Users.GetAllAsync(It.IsAny<GetUsersRequest>(), It.IsAny<PaginationInfo>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(new PagedList<User>(new List<User>(), new PagingInformation(0, 50, 0, 0)));

        managementApiClientMock.Setup(client => client.Roles.GetAllAsync(It.IsAny<GetRolesRequest>(), It.IsAny<PaginationInfo>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(roles);

        var managementApiClientField = typeof(AuthService).GetField("_managementApiClient", BindingFlags.NonPublic | BindingFlags.Instance);
        managementApiClientField?.SetValue(_authService, managementApiClientMock.Object);

        await Assert.ThrowsAsync<CustomException>(async () => await _authService.AddRoleToUserAsync(userId, roleId));
    }

    [Fact]
    public async Task AddRoleToUserAsync_RoleNotFound()
    {
        var userId = "user1";
        var roleId = "nonexistentRole";

        var users = new PagedList<User>(new List<User>
        {
            new User { UserId = userId, FullName = "User 1" },
            new User { UserId = "user2", FullName = "User 2" }
        }, new PagingInformation(0, 50, 2, 2));

        var validToken = CreateJwtToken();
        var cachedTokenField = typeof(AuthService).GetField("_cachedAccessToken", BindingFlags.NonPublic | BindingFlags.Instance);
        cachedTokenField?.SetValue(_authService, validToken);

        var managementApiClientMock = new Mock<IManagementApiClient>();

        managementApiClientMock.Setup(client => client.Users.GetAllAsync(It.IsAny<GetUsersRequest>(), It.IsAny<PaginationInfo>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(users);

        managementApiClientMock.Setup(client => client.Roles.GetAllAsync(It.IsAny<GetRolesRequest>(), It.IsAny<PaginationInfo>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(new PagedList<Role>(new List<Role>(), new PagingInformation(0, 50, 0, 0)));

        var managementApiClientField = typeof(AuthService).GetField("_managementApiClient", BindingFlags.NonPublic | BindingFlags.Instance);
        managementApiClientField?.SetValue(_authService, managementApiClientMock.Object);

        await Assert.ThrowsAsync<CustomException>(async () => await _authService.AddRoleToUserAsync(userId, roleId));
    }

    [Fact]
    public async Task RemoveRoleFromUserAsync_Success()
    {
        var userId = "user1";
        var roleId = "role1";

        var users = new PagedList<User>(new List<User>
        {
            new User { UserId = userId, FullName = "User 1" },
            new User { UserId = "user2", FullName = "User 2" }
        }, new PagingInformation(0, 50, 2, 2));

        var roles = new PagedList<Role>(new List<Role>
        {
            new Role { Id = roleId, Name = "Role 1" },
            new Role { Id = "role2", Name = "Role 2" }
        }, new PagingInformation(0, 50, 2, 2));

        var removeRolesRequest = new AssignRolesRequest
        {
            Roles = new[] { roleId }
        };

        var validToken = CreateJwtToken();
        var cachedTokenField = typeof(AuthService).GetField("_cachedAccessToken", BindingFlags.NonPublic | BindingFlags.Instance);
        cachedTokenField?.SetValue(_authService, validToken);

        var managementApiClientMock = new Mock<IManagementApiClient>();

        managementApiClientMock.Setup(client => client.Users.GetAllAsync(It.IsAny<GetUsersRequest>(), It.IsAny<PaginationInfo>(), It.IsAny<CancellationToken>()))
                               .ReturnsAsync(users);

        managementApiClientMock.Setup(client => client.Roles.GetAllAsync(It.IsAny<GetRolesRequest>(), It.IsAny<PaginationInfo>(), It.IsAny<CancellationToken>()))
                               .ReturnsAsync(roles);

        managementApiClientMock.Setup(client => client.Users.RemoveRolesAsync(userId, removeRolesRequest, It.IsAny<CancellationToken>()))
                               .Returns(Task.CompletedTask);

        var managementApiClientField = typeof(AuthService).GetField("_managementApiClient", BindingFlags.NonPublic | BindingFlags.Instance);
        managementApiClientField?.SetValue(_authService, managementApiClientMock.Object);

        await _authService.RemoveRoleFromUserAsync(userId, roleId);

        managementApiClientMock.Verify(client => client.Users.RemoveRolesAsync(userId, It.Is<AssignRolesRequest>(req => req.Roles.Contains(roleId)), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task RemoveRoleToUserAsync_UserNotFound()
    {
        var userId = "nonexistentUser";
        var roleId = "role1";

        var roles = new PagedList<Role>(new List<Role>
        {
            new Role { Id = roleId, Name = "Role 1" },
            new Role { Id = "role2", Name = "Role 2" }
        }, new PagingInformation(0, 50, 2, 2));

        var validToken = CreateJwtToken();
        var cachedTokenField = typeof(AuthService).GetField("_cachedAccessToken", BindingFlags.NonPublic | BindingFlags.Instance);
        cachedTokenField?.SetValue(_authService, validToken);

        var managementApiClientMock = new Mock<IManagementApiClient>();
        managementApiClientMock.Setup(client => client.Users.GetAllAsync(It.IsAny<GetUsersRequest>(), It.IsAny<PaginationInfo>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(new PagedList<User>(new List<User>(), new PagingInformation(0, 50, 0, 0)));

        managementApiClientMock.Setup(client => client.Roles.GetAllAsync(It.IsAny<GetRolesRequest>(), It.IsAny<PaginationInfo>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(roles);

        var managementApiClientField = typeof(AuthService).GetField("_managementApiClient", BindingFlags.NonPublic | BindingFlags.Instance);
        managementApiClientField?.SetValue(_authService, managementApiClientMock.Object);

        await Assert.ThrowsAsync<CustomException>(async () => await _authService.RemoveRoleFromUserAsync(userId, roleId));
    }

    [Fact]
    public async Task RemoveRoleToUserAsync_RoleNotFound()
    {
        var userId = "user1";
        var roleId = "nonexistentRole";

        var users = new PagedList<User>(new List<User>
        {
            new User { UserId = userId, FullName = "User 1" },
            new User { UserId = "user2", FullName = "User 2" }
        }, new PagingInformation(0, 50, 2, 2));

        var validToken = CreateJwtToken();
        var cachedTokenField = typeof(AuthService).GetField("_cachedAccessToken", BindingFlags.NonPublic | BindingFlags.Instance);
        cachedTokenField?.SetValue(_authService, validToken);

        var managementApiClientMock = new Mock<IManagementApiClient>();

        managementApiClientMock.Setup(client => client.Users.GetAllAsync(It.IsAny<GetUsersRequest>(), It.IsAny<PaginationInfo>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(users);

        managementApiClientMock.Setup(client => client.Roles.GetAllAsync(It.IsAny<GetRolesRequest>(), It.IsAny<PaginationInfo>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(new PagedList<Role>(new List<Role>(), new PagingInformation(0, 50, 0, 0)));

        var managementApiClientField = typeof(AuthService).GetField("_managementApiClient", BindingFlags.NonPublic | BindingFlags.Instance);
        managementApiClientField?.SetValue(_authService, managementApiClientMock.Object);

        await Assert.ThrowsAsync<CustomException>(async () => await _authService.RemoveRoleFromUserAsync(userId, roleId));
    }

    [Fact]
    public async Task CreateRoleAsync_RoleAlreadyExists()
    {
        var roleName = "ExistingRole";
        var description = "Existing Role Description";

        var existingRole = new Role
        {
            Name = roleName,
            Description = description
        };

        var roles = new PagedList<Role>(new List<Role>
        {
            existingRole,
            new Role { Name = "AnotherRole", Description = "Another Role Description" }
        }, new PagingInformation(0, 50, 2, 2));

        var validToken = CreateJwtToken();
        var cachedTokenField = typeof(AuthService).GetField("_cachedAccessToken", BindingFlags.NonPublic | BindingFlags.Instance);
        cachedTokenField?.SetValue(_authService, validToken);

        var managementApiClientMock = new Mock<IManagementApiClient>();

        managementApiClientMock.Setup(client => client.Roles.GetAllAsync(It.IsAny<GetRolesRequest>(), It.IsAny<PaginationInfo>(), It.IsAny<CancellationToken>()))
                               .ReturnsAsync(roles);

        var managementApiClientField = typeof(AuthService).GetField("_managementApiClient", BindingFlags.NonPublic | BindingFlags.Instance);
        managementApiClientField?.SetValue(_authService, managementApiClientMock.Object);

        var result = await _authService.CreateRoleAsync(roleName, description);

        Assert.Equal(roleName, result.Name);
        Assert.Equal(description, result.Description);
        managementApiClientMock.Verify(client => client.Roles.CreateAsync(It.IsAny<RoleCreateRequest>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task CreateRoleAsync_RoleDoesNotExist()
    {
        var roleName = "NewRole";
        var description = "New Role Description";

        var newRole = new Role
        {
            Name = roleName,
            Description = description
        };

        var existingRoles = new PagedList<Role>(new List<Role>
        {
            new Role { Name = "AnotherRole", Description = "Another Role Description" }
        }, new PagingInformation(0, 50, 1, 1));

        var validToken = CreateJwtToken();
        var cachedTokenField = typeof(AuthService).GetField("_cachedAccessToken", BindingFlags.NonPublic | BindingFlags.Instance);
        cachedTokenField?.SetValue(_authService, validToken);

        var managementApiClientMock = new Mock<IManagementApiClient>();

        managementApiClientMock.Setup(client => client.Roles.GetAllAsync(It.IsAny<GetRolesRequest>(), It.IsAny<PaginationInfo>(), It.IsAny<CancellationToken>()))
                               .ReturnsAsync(existingRoles);

        managementApiClientMock.Setup(client => client.Roles.CreateAsync(It.Is<RoleCreateRequest>(req => req.Name == roleName && req.Description == description), It.IsAny<CancellationToken>()))
                               .ReturnsAsync(newRole);

        var managementApiClientField = typeof(AuthService).GetField("_managementApiClient", BindingFlags.NonPublic | BindingFlags.Instance);
        managementApiClientField?.SetValue(_authService, managementApiClientMock.Object);

        var result = await _authService.CreateRoleAsync(roleName, description);

        Assert.Equal(roleName, result.Name);
        Assert.Equal(description, result.Description);
        managementApiClientMock.Verify(client => client.Roles.CreateAsync(It.Is<RoleCreateRequest>(req => req.Name == roleName && req.Description == description), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteRoleAsync_RoleExists()
    {
        var roleId = "role1";

        var roles = new PagedList<Role>(new List<Role>
        {
            new Role { Id = roleId, Name = "Role 1" },
            new Role { Id = "role2", Name = "Role 2" }
        }, new PagingInformation(0, 50, 2, 2));

        var validToken = CreateJwtToken();
        var cachedTokenField = typeof(AuthService).GetField("_cachedAccessToken", BindingFlags.NonPublic | BindingFlags.Instance);
        cachedTokenField?.SetValue(_authService, validToken);

        var managementApiClientMock = new Mock<IManagementApiClient>();

        managementApiClientMock.Setup(client => client.Roles.GetAllAsync(It.IsAny<GetRolesRequest>(), It.IsAny<PaginationInfo>(), It.IsAny<CancellationToken>()))
                               .ReturnsAsync(roles);

        managementApiClientMock.Setup(client => client.Roles.DeleteAsync(roleId, It.IsAny<CancellationToken>()))
                               .Returns(Task.CompletedTask);

        var managementApiClientField = typeof(AuthService).GetField("_managementApiClient", BindingFlags.NonPublic | BindingFlags.Instance);
        managementApiClientField?.SetValue(_authService, managementApiClientMock.Object);

        var result = await _authService.DeleteRoleAsync(roleId);

        Assert.True(result);
        managementApiClientMock.Verify(client => client.Roles.DeleteAsync(roleId, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteRoleAsync_RoleDoesNotExist()
    {
        var roleId = "nonexistentRole";

        var roles = new PagedList<Role>(new List<Role>
    {
        new Role { Id = "role1", Name = "Role 1" },
        new Role { Id = "role2", Name = "Role 2" }
    }, new PagingInformation(0, 50, 2, 2));

        var validToken = CreateJwtToken();
        var cachedTokenField = typeof(AuthService).GetField("_cachedAccessToken", BindingFlags.NonPublic | BindingFlags.Instance);
        cachedTokenField?.SetValue(_authService, validToken);

        var managementApiClientMock = new Mock<IManagementApiClient>();

        managementApiClientMock.Setup(client => client.Roles.GetAllAsync(It.IsAny<GetRolesRequest>(), It.IsAny<PaginationInfo>(), It.IsAny<CancellationToken>()))
                               .ReturnsAsync(roles);

        var managementApiClientField = typeof(AuthService).GetField("_managementApiClient", BindingFlags.NonPublic | BindingFlags.Instance);
        managementApiClientField?.SetValue(_authService, managementApiClientMock.Object);

        var result = await _authService.DeleteRoleAsync(roleId);

        Assert.False(result);
        managementApiClientMock.Verify(client => client.Roles.DeleteAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task UpdateRoleAsync_RoleExists()
    {
        var roleId = "role1";
        var newRoleName = "Updated Role Name";
        var newDescription = "Updated Description";

        var roles = new PagedList<Role>(new List<Role>
    {
        new Role { Id = roleId, Name = "Role 1" },
        new Role { Id = "role2", Name = "Role 2" }
    }, new PagingInformation(0, 50, 2, 2));

        var validToken = CreateJwtToken();
        var cachedTokenField = typeof(AuthService).GetField("_cachedAccessToken", BindingFlags.NonPublic | BindingFlags.Instance);
        cachedTokenField?.SetValue(_authService, validToken);

        var managementApiClientMock = new Mock<IManagementApiClient>();

        managementApiClientMock.Setup(client => client.Roles.GetAllAsync(It.IsAny<GetRolesRequest>(), It.IsAny<PaginationInfo>(), It.IsAny<CancellationToken>()))
                               .ReturnsAsync(roles);

        managementApiClientMock.Setup(client => client.Roles.UpdateAsync(roleId, It.IsAny<RoleUpdateRequest>(), It.IsAny<CancellationToken>()))
                               .ReturnsAsync(new Role { Id = roleId, Name = newRoleName, Description = newDescription });

        var managementApiClientField = typeof(AuthService).GetField("_managementApiClient", BindingFlags.NonPublic | BindingFlags.Instance);
        managementApiClientField?.SetValue(_authService, managementApiClientMock.Object);

        var result = await _authService.UpdateRoleAsync(roleId, newRoleName, newDescription);

        Assert.True(result);
        managementApiClientMock.Verify(client => client.Roles.UpdateAsync(roleId, It.Is<RoleUpdateRequest>(r => r.Name == newRoleName && r.Description == newDescription), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UpdateRoleAsync_RoleDoesNotExist()
    {
        var roleId = "nonexistentRole";
        var newRoleName = "Updated Role Name";
        var newDescription = "Updated Description";

        var roles = new PagedList<Role>(new List<Role>
    {
        new Role { Id = "role1", Name = "Role 1" },
        new Role { Id = "role2", Name = "Role 2" }
    }, new PagingInformation(0, 50, 2, 2));

        var validToken = CreateJwtToken();
        var cachedTokenField = typeof(AuthService).GetField("_cachedAccessToken", BindingFlags.NonPublic | BindingFlags.Instance);
        cachedTokenField?.SetValue(_authService, validToken);

        var managementApiClientMock = new Mock<IManagementApiClient>();

        managementApiClientMock.Setup(client => client.Roles.GetAllAsync(It.IsAny<GetRolesRequest>(), It.IsAny<PaginationInfo>(), It.IsAny<CancellationToken>()))
                               .ReturnsAsync(roles);

        var managementApiClientField = typeof(AuthService).GetField("_managementApiClient", BindingFlags.NonPublic | BindingFlags.Instance);
        managementApiClientField?.SetValue(_authService, managementApiClientMock.Object);

        var result = await _authService.UpdateRoleAsync(roleId, newRoleName, newDescription);

        Assert.False(result);
        managementApiClientMock.Verify(client => client.Roles.UpdateAsync(It.IsAny<string>(), It.IsAny<RoleUpdateRequest>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task GetPermissionsByRoleAsync_RoleExists()
    {
        var roleId = "role1";

        var roles = new PagedList<Role>(new List<Role>
    {
        new Role { Id = roleId, Name = "Role 1" },
        new Role { Id = "role2", Name = "Role 2" }
    }, new PagingInformation(0, 50, 2, 2));

        var permissions = new PagedList<Permission>(new List<Permission>
    {
        new Permission { Name = "perm1", Description = "Permission 1" },
        new Permission { Name = "perm2", Description = "Permission 2" }
    }, new PagingInformation(0, 50, 2, 2));

        var validToken = CreateJwtToken();
        var cachedTokenField = typeof(AuthService).GetField("_cachedAccessToken", BindingFlags.NonPublic | BindingFlags.Instance);
        cachedTokenField?.SetValue(_authService, validToken);

        var managementApiClientMock = new Mock<IManagementApiClient>();

        managementApiClientMock.Setup(client => client.Roles.GetAllAsync(It.IsAny<GetRolesRequest>(), It.IsAny<PaginationInfo>(), It.IsAny<CancellationToken>()))
                               .ReturnsAsync(roles);

        managementApiClientMock.Setup(client => client.Roles.GetPermissionsAsync(roleId, It.IsAny<PaginationInfo>(), It.IsAny<CancellationToken>()))
                               .ReturnsAsync(permissions);

        var managementApiClientField = typeof(AuthService).GetField("_managementApiClient", BindingFlags.NonPublic | BindingFlags.Instance);
        managementApiClientField?.SetValue(_authService, managementApiClientMock.Object);

        var result = await _authService.GetPermissionsByRoleAsync(roleId);

        Assert.Equal(2, result.Count);
        Assert.Contains(result, p => p.Name == "perm1");
        Assert.Contains(result, p => p.Name == "perm2");
        managementApiClientMock.Verify(client => client.Roles.GetPermissionsAsync(roleId, It.IsAny<PaginationInfo>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetPermissionsByRoleAsync_RoleDoesNotExist()
    {
        var roleId = "nonexistentRole";

        var roles = new PagedList<Role>(new List<Role>
    {
        new Role { Id = "role1", Name = "Role 1" },
        new Role { Id = "role2", Name = "Role 2" }
    }, new PagingInformation(0, 50, 2, 2));

        var validToken = CreateJwtToken();
        var cachedTokenField = typeof(AuthService).GetField("_cachedAccessToken", BindingFlags.NonPublic | BindingFlags.Instance);
        cachedTokenField?.SetValue(_authService, validToken);

        var managementApiClientMock = new Mock<IManagementApiClient>();

        managementApiClientMock.Setup(client => client.Roles.GetAllAsync(It.IsAny<GetRolesRequest>(), It.IsAny<PaginationInfo>(), It.IsAny<CancellationToken>()))
                               .ReturnsAsync(roles);

        var managementApiClientField = typeof(AuthService).GetField("_managementApiClient", BindingFlags.NonPublic | BindingFlags.Instance);
        managementApiClientField?.SetValue(_authService, managementApiClientMock.Object);

        await Assert.ThrowsAsync<CustomException>(async () => await _authService.GetPermissionsByRoleAsync(roleId));
    }

    [Fact]
    public async Task RemovePermissionsFromRoleAsync_RoleExists()
    {
        var roleId = "role1";
        var permissionName = "read:profile";

        var roles = new PagedList<Role>(new List<Role>
    {
        new Role { Id = roleId, Name = "Role 1" },
        new Role { Id = "role2", Name = "Role 2" }
    }, new PagingInformation(0, 50, 2, 2));

        var validToken = CreateJwtToken();
        var cachedTokenField = typeof(AuthService).GetField("_cachedAccessToken", BindingFlags.NonPublic | BindingFlags.Instance);
        cachedTokenField?.SetValue(_authService, validToken);

        var managementApiClientMock = new Mock<IManagementApiClient>();

        managementApiClientMock.Setup(client => client.Roles.GetAllAsync(It.IsAny<GetRolesRequest>(), It.IsAny<PaginationInfo>(), It.IsAny<CancellationToken>()))
                               .ReturnsAsync(roles);

        managementApiClientMock.Setup(client => client.Roles.RemovePermissionsAsync(roleId, It.IsAny<AssignPermissionsRequest>(), It.IsAny<CancellationToken>()))
                               .Returns(Task.CompletedTask);

        var managementApiClientField = typeof(AuthService).GetField("_managementApiClient", BindingFlags.NonPublic | BindingFlags.Instance);
        managementApiClientField?.SetValue(_authService, managementApiClientMock.Object);

        var result = await _authService.RemovePermissionsFromRoleAsync(roleId, permissionName);

        Assert.True(result);
        managementApiClientMock.Verify(client => client.Roles.RemovePermissionsAsync(roleId, It.Is<AssignPermissionsRequest>(req => req.Permissions.Any(p => p.Name == permissionName)), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task RemovePermissionsFromRoleAsync_RoleDoesNotExist()
    {
        var roleId = "nonexistentRole";
        var permissionName = "read:profile";

        var roles = new PagedList<Role>(new List<Role>
    {
        new Role { Id = "role1", Name = "Role 1" },
        new Role { Id = "role2", Name = "Role 2" }
    }, new PagingInformation(0, 50, 2, 2));

        var validToken = CreateJwtToken();
        var cachedTokenField = typeof(AuthService).GetField("_cachedAccessToken", BindingFlags.NonPublic | BindingFlags.Instance);
        cachedTokenField?.SetValue(_authService, validToken);

        var managementApiClientMock = new Mock<IManagementApiClient>();

        managementApiClientMock.Setup(client => client.Roles.GetAllAsync(It.IsAny<GetRolesRequest>(), It.IsAny<PaginationInfo>(), It.IsAny<CancellationToken>()))
                               .ReturnsAsync(roles);

        var managementApiClientField = typeof(AuthService).GetField("_managementApiClient", BindingFlags.NonPublic | BindingFlags.Instance);
        managementApiClientField?.SetValue(_authService, managementApiClientMock.Object);

        var result = await _authService.RemovePermissionsFromRoleAsync(roleId, permissionName);

        Assert.False(result);
        managementApiClientMock.Verify(client => client.Roles.RemovePermissionsAsync(It.IsAny<string>(), It.IsAny<AssignPermissionsRequest>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task AddPermissionsToRoleAsync_RoleExists()
    {
        var roleId = "role1";
        var permissionName = "read:profile";

        var roles = new PagedList<Role>(new List<Role>
    {
        new Role { Id = roleId, Name = "Role 1" },
        new Role { Id = "role2", Name = "Role 2" }
    }, new PagingInformation(0, 50, 2, 2));

        var validToken = CreateJwtToken();
        var cachedTokenField = typeof(AuthService).GetField("_cachedAccessToken", BindingFlags.NonPublic | BindingFlags.Instance);
        cachedTokenField?.SetValue(_authService, validToken);

        var managementApiClientMock = new Mock<IManagementApiClient>();

        managementApiClientMock.Setup(client => client.Roles.GetAllAsync(It.IsAny<GetRolesRequest>(), It.IsAny<PaginationInfo>(), It.IsAny<CancellationToken>()))
                               .ReturnsAsync(roles);

        managementApiClientMock.Setup(client => client.Roles.AssignPermissionsAsync(roleId, It.IsAny<AssignPermissionsRequest>(), It.IsAny<CancellationToken>()))
                               .Returns(Task.CompletedTask);

        var managementApiClientField = typeof(AuthService).GetField("_managementApiClient", BindingFlags.NonPublic | BindingFlags.Instance);
        managementApiClientField?.SetValue(_authService, managementApiClientMock.Object);

        var result = await _authService.AddPermissionsToRoleAsync(roleId, permissionName);

        Assert.True(result);
        managementApiClientMock.Verify(client => client.Roles.AssignPermissionsAsync(roleId, It.Is<AssignPermissionsRequest>(req => req.Permissions.Any(p => p.Name == permissionName)), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task AddPermissionsToRoleAsync_RoleDoesNotExist()
    {
        var roleId = "nonexistentRole";
        var permissionName = "read:profile";

        var roles = new PagedList<Role>(new List<Role>
    {
        new Role { Id = "role1", Name = "Role 1" },
        new Role { Id = "role2", Name = "Role 2" }
    }, new PagingInformation(0, 50, 2, 2));

        var validToken = CreateJwtToken();
        var cachedTokenField = typeof(AuthService).GetField("_cachedAccessToken", BindingFlags.NonPublic | BindingFlags.Instance);
        cachedTokenField?.SetValue(_authService, validToken);

        var managementApiClientMock = new Mock<IManagementApiClient>();

        managementApiClientMock.Setup(client => client.Roles.GetAllAsync(It.IsAny<GetRolesRequest>(), It.IsAny<PaginationInfo>(), It.IsAny<CancellationToken>()))
                               .ReturnsAsync(roles);

        var managementApiClientField = typeof(AuthService).GetField("_managementApiClient", BindingFlags.NonPublic | BindingFlags.Instance);
        managementApiClientField?.SetValue(_authService, managementApiClientMock.Object);

        var result = await _authService.AddPermissionsToRoleAsync(roleId, permissionName);

        Assert.False(result);
        managementApiClientMock.Verify(client => client.Roles.AssignPermissionsAsync(It.IsAny<string>(), It.IsAny<AssignPermissionsRequest>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task CreateUser_UserAlreadyExists()
    {
        var request = new UserCreateRequest { UserId = "existingUserId" };

        var existingUsers = new PagedList<User>(new List<User>
    {
        new User { UserId = "existingUserId", FullName = "Existing User" },
        new User { UserId = "anotherUserId", FullName = "Another User" }
    }, new PagingInformation(0, 50, 2, 2));

        var validToken = CreateJwtToken();
        var cachedTokenField = typeof(AuthService).GetField("_cachedAccessToken", BindingFlags.NonPublic | BindingFlags.Instance);
        cachedTokenField?.SetValue(_authService, validToken);

        var managementApiClientMock = new Mock<IManagementApiClient>();

        managementApiClientMock.Setup(client => client.Users.GetAllAsync(It.IsAny<GetUsersRequest>(), It.IsAny<PaginationInfo>(), It.IsAny<CancellationToken>()))
                               .ReturnsAsync(existingUsers);

        var managementApiClientField = typeof(AuthService).GetField("_managementApiClient", BindingFlags.NonPublic | BindingFlags.Instance);
        managementApiClientField?.SetValue(_authService, managementApiClientMock.Object);

        var result = await _authService.CreateUser(request);

        Assert.False(result);
        managementApiClientMock.Verify(client => client.Users.CreateAsync(It.IsAny<UserCreateRequest>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task CreateUser_Success()
    {
        var userCreateRequest = new UserCreateRequest
        {
            UserId = "newUserId",
            UserName = "New User",
            Email = "newuser@example.com"
        };

        var existingUsers = new PagedList<User>(new List<User>
    {
        new User { UserId = "existingUserId", FullName = "Existing User" }
    }, new PagingInformation(0, 50, 1, 1));

        var validToken = CreateJwtToken();
        var cachedTokenField = typeof(AuthService).GetField("_cachedAccessToken", BindingFlags.NonPublic | BindingFlags.Instance);
        cachedTokenField?.SetValue(_authService, validToken);

        var managementApiClientMock = new Mock<IManagementApiClient>();

        managementApiClientMock.Setup(client => client.Users.GetAllAsync(It.IsAny<GetUsersRequest>(), It.IsAny<PaginationInfo>(), It.IsAny<CancellationToken>()))
                               .ReturnsAsync(existingUsers);

        managementApiClientMock.Setup(client => client.Users.CreateAsync(It.Is<UserCreateRequest>(req => req.UserId == userCreateRequest.UserId), It.IsAny<CancellationToken>()))
                               .ReturnsAsync(new Auth0.ManagementApi.Models.User
                               {
                                   UserId = userCreateRequest.UserId
                               });

        var managementApiClientField = typeof(AuthService).GetField("_managementApiClient", BindingFlags.NonPublic | BindingFlags.Instance);
        managementApiClientField?.SetValue(_authService, managementApiClientMock.Object);

        var result = await _authService.CreateUser(userCreateRequest);

        Assert.True(result);
        managementApiClientMock.Verify(client => client.Users.CreateAsync(It.Is<UserCreateRequest>(req => req.UserId == userCreateRequest.UserId), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task CreateUser_ApiException()
    {
        var userCreateRequest = new UserCreateRequest
        {
            UserId = "newUserId",
            UserName = "New User",
            Email = "newuser@example.com"
        };

        var existingUsers = new PagedList<User>(new List<User>(), new PagingInformation(0, 50, 0, 0));

        var validToken = CreateJwtToken();
        var cachedTokenField = typeof(AuthService).GetField("_cachedAccessToken", BindingFlags.NonPublic | BindingFlags.Instance);
        cachedTokenField?.SetValue(_authService, validToken);

        var managementApiClientMock = new Mock<IManagementApiClient>();

        managementApiClientMock.Setup(client => client.Users.GetAllAsync(It.IsAny<GetUsersRequest>(), It.IsAny<PaginationInfo>(), It.IsAny<CancellationToken>()))
                               .ReturnsAsync(existingUsers);

        managementApiClientMock.Setup(client => client.Users.CreateAsync(It.IsAny<UserCreateRequest>(), It.IsAny<CancellationToken>()))
                               .ThrowsAsync(new Exception("API error"));

        var managementApiClientField = typeof(AuthService).GetField("_managementApiClient", BindingFlags.NonPublic | BindingFlags.Instance);
        managementApiClientField?.SetValue(_authService, managementApiClientMock.Object);

        await Assert.ThrowsAsync<Exception>(async () => await _authService.CreateUser(userCreateRequest));
    }

    [Fact]
    public async Task GetUserById_UserExists_ReturnsUser()
    {
        var userId = "user1";

        var existingUsers = new PagedList<User>(new List<User>()
        {
            new User { UserId = userId, Email = "test1@retrorabbit.co.za" },
            new User { UserId = "user2", Email = "test2@retrorabbit.co.za" }
        }, new PagingInformation(0, 50, 0, 0));

        var expectedUser = new Auth0.ManagementApi.Models.User { UserId = userId, Email = "test1@retrorabbit.co.za" };
        var allUsers = new List<Auth0.ManagementApi.Models.User>
        {
            expectedUser,
            new Auth0.ManagementApi.Models.User { UserId = "user2", FullName = "User 2" }
        };

        var validToken = CreateJwtToken();
        var cachedTokenField = typeof(AuthService).GetField("_cachedAccessToken", BindingFlags.NonPublic | BindingFlags.Instance);
        cachedTokenField?.SetValue(_authService, validToken);

        var usersClientMock = new Mock<IUsersClient>();
        usersClientMock.Setup(client => client.GetAsync(userId, null, true, It.IsAny<CancellationToken>()))
                       .ReturnsAsync(expectedUser);

        var managementApiClientMock = new Mock<IManagementApiClient>();
        managementApiClientMock.Setup(client => client.Users).Returns(usersClientMock.Object);

        managementApiClientMock.Setup(client => client.Users.GetAllAsync(It.IsAny<GetUsersRequest>(), It.IsAny<PaginationInfo>(), It.IsAny<CancellationToken>()))
                       .ReturnsAsync(existingUsers);

        var managementApiClientField = typeof(AuthService).GetField("_managementApiClient", BindingFlags.NonPublic | BindingFlags.Instance);
        managementApiClientField?.SetValue(_authService, managementApiClientMock.Object);

        var result = await _authService.GetUserById(userId);

        Assert.NotNull(result);
        Assert.Equal("user1", result.UserId);
        Assert.Equal("test1@retrorabbit.co.za", result.Email);
    }

    [Fact]
    public async Task GetUserById_UserNotFound_ThrowsCustomException()
    {
        var userId = "nonexistentUser";

        var existingUsers = new PagedList<User>(new List<User>()
    {
        new User { UserId = "user1", Email = "test1@retrorabbit.co.za" },
        new User { UserId = "user2", Email = "test2@retrorabbit.co.za" }
    }, new PagingInformation(0, 50, 0, 0));

        var validToken = CreateJwtToken();
        var cachedTokenField = typeof(AuthService).GetField("_cachedAccessToken", BindingFlags.NonPublic | BindingFlags.Instance);
        cachedTokenField?.SetValue(_authService, validToken);

        var usersClientMock = new Mock<IUsersClient>();
        var managementApiClientMock = new Mock<IManagementApiClient>();
        managementApiClientMock.Setup(client => client.Users).Returns(usersClientMock.Object);

        managementApiClientMock.Setup(client => client.Users.GetAllAsync(It.IsAny<GetUsersRequest>(), It.IsAny<PaginationInfo>(), It.IsAny<CancellationToken>()))
                       .ReturnsAsync(existingUsers);

        var managementApiClientField = typeof(AuthService).GetField("_managementApiClient", BindingFlags.NonPublic | BindingFlags.Instance);
        managementApiClientField?.SetValue(_authService, managementApiClientMock.Object);

        await Assert.ThrowsAsync<CustomException>(() => _authService.GetUserById(userId));
    }

    [Fact]
    public async Task DeleteUser_UserExists_DeletesUser()
    {
        var userId = "user1";

        var validToken = CreateJwtToken();
        var cachedTokenField = typeof(AuthService).GetField("_cachedAccessToken", BindingFlags.NonPublic | BindingFlags.Instance);
        cachedTokenField?.SetValue(_authService, validToken);

        var expectedUser = new Auth0.ManagementApi.Models.User { UserId = userId, Email = "test1@retrorabbit.co.za" };

        var usersClientMock = new Mock<IUsersClient>();
        usersClientMock.Setup(client => client.GetAsync(userId, null, true, It.IsAny<CancellationToken>()))
                       .ReturnsAsync(expectedUser);
        usersClientMock.Setup(client => client.DeleteAsync(userId))
                       .Returns(Task.CompletedTask);

        var managementApiClientMock = new Mock<IManagementApiClient>();
        managementApiClientMock.Setup(client => client.Users).Returns(usersClientMock.Object);

        var managementApiClientField = typeof(AuthService).GetField("_managementApiClient", BindingFlags.NonPublic | BindingFlags.Instance);
        managementApiClientField?.SetValue(_authService, managementApiClientMock.Object);

        var result = await _authService.DeleteUser(userId);

        Assert.True(result);
        usersClientMock.Verify(client => client.DeleteAsync(userId));
    }

    [Fact]
    public async Task DeleteUser_UserNotFound_ReturnsTrue()
    {
        var userId = "nonexistentUser";

        var validToken = CreateJwtToken();
        var cachedTokenField = typeof(AuthService).GetField("_cachedAccessToken", BindingFlags.NonPublic | BindingFlags.Instance);
        cachedTokenField?.SetValue(_authService, validToken);

        var usersClientMock = new Mock<IUsersClient>();
        usersClientMock.Setup(client => client.GetAsync(userId, null, true, It.IsAny<CancellationToken>()))
                       .ReturnsAsync((Auth0.ManagementApi.Models.User)null);

        var managementApiClientMock = new Mock<IManagementApiClient>();
        managementApiClientMock.Setup(client => client.Users).Returns(usersClientMock.Object);

        var managementApiClientField = typeof(AuthService).GetField("_managementApiClient", BindingFlags.NonPublic | BindingFlags.Instance);
        managementApiClientField?.SetValue(_authService, managementApiClientMock.Object);

        var result = await _authService.DeleteUser(userId);

        Assert.True(result);
        usersClientMock.Verify(client => client.DeleteAsync(userId));
    }

    [Fact]
    public async Task UpdateUser_UserExists_UpdatesUser()
    {
        var userId = "user1";
        var request = new UserUpdateRequest
        {
            UserName = "userNameToBeUpdated",
            Email = "updatedEmail@retrorabbit.co.za"
        };

        var existingUsers = new PagedList<User>(new List<User>
    {
        new User { UserId = userId, UserName = "userNameToBeUpdated", Email = "oldEmail@retrorabbit.co.za" },
        new User { UserId = "user2", UserName = "userName2", Email = "email2@retrorabbit.co.za" }
    }, new PagingInformation(0, 50, 2, 2));

        var validToken = CreateJwtToken();
        var cachedTokenField = typeof(AuthService).GetField("_cachedAccessToken", BindingFlags.NonPublic | BindingFlags.Instance);
        cachedTokenField?.SetValue(_authService, validToken);

        var managementApiClientMock = new Mock<IManagementApiClient>();

        managementApiClientMock.Setup(client => client.Users.GetAllAsync(It.IsAny<GetUsersRequest>(), It.IsAny<PaginationInfo>(), It.IsAny<CancellationToken>()))
                               .ReturnsAsync(existingUsers);

        managementApiClientMock.Setup(client => client.Users.UpdateAsync(userId, It.Is<UserUpdateRequest>(r =>
        r.UserName == request.UserName &&
        r.Email == request.Email), It.IsAny<CancellationToken>()))
                           .ReturnsAsync(new User { UserId = userId, UserName = request.UserName, Email = request.Email });


        managementApiClientMock.Setup(client => client.Users.UpdateAsync(userId, It.IsAny<UserUpdateRequest>(), It.IsAny<CancellationToken>()))
                               .ReturnsAsync(new User { UserId = userId, UserName = request.UserName, Email = request.Email });

        var managementApiClientField = typeof(AuthService).GetField("_managementApiClient", BindingFlags.NonPublic | BindingFlags.Instance);
        managementApiClientField?.SetValue(_authService, managementApiClientMock.Object);

        var result = await _authService.UpdateUser(userId, request);

        Assert.True(result);
        managementApiClientMock.Verify(client => client.Users.UpdateAsync(userId, It.Is<UserUpdateRequest>(r =>
            r.UserName == request.UserName &&
            r.Email == request.Email), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UpdateUser_UserNotFound_ReturnsFalse()
    {
        var userId = "nonexistentUser";
        var request = new UserUpdateRequest
        {
            UserName = "updatedUserName",
            Email = "updatedEmail@retrorabbit.co.za"
        };

        var existingUsers = new PagedList<User>(new List<User>
    {
        new User { UserId = "user2", UserName = "userName2", Email = "email2@retrorabbit.co.za" }
    }, new PagingInformation(0, 50, 1, 1));

        var validToken = CreateJwtToken();
        var cachedTokenField = typeof(AuthService).GetField("_cachedAccessToken", BindingFlags.NonPublic | BindingFlags.Instance);
        cachedTokenField?.SetValue(_authService, validToken);

        var managementApiClientMock = new Mock<IManagementApiClient>();

        managementApiClientMock.Setup(client => client.Users.GetAllAsync(It.IsAny<GetUsersRequest>(), It.IsAny<PaginationInfo>(), It.IsAny<CancellationToken>()))
                               .ReturnsAsync(existingUsers);

        var managementApiClientField = typeof(AuthService).GetField("_managementApiClient", BindingFlags.NonPublic | BindingFlags.Instance);
        managementApiClientField?.SetValue(_authService, managementApiClientMock.Object);

        var result = await _authService.UpdateUser(userId, request);

        Assert.False(result);
        managementApiClientMock.Verify(client => client.Users.UpdateAsync(It.IsAny<string>(), It.IsAny<UserUpdateRequest>(), It.IsAny<CancellationToken>()), Times.Never);
    }

}
