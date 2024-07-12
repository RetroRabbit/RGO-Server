using Auth0.ManagementApi;
using Auth0.ManagementApi.Models;
using Auth0.ManagementApi.Paging;
using HRIS.Models;
using HRIS.Services.Helpers;
using HRIS.Services.Interfaces;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;

namespace HRIS.Services.Services;

public class AuthService : IAuthService
{
    private readonly IErrorLoggingService _errorLoggingService;
    private readonly AuthManagement _authManagement;
    private readonly HttpClient client = new();
    private static string _cachedAccessToken = string.Empty;
    private static string _clientId = string.Empty;
    private static string _clientSecret = string.Empty;
    private static string _issuer = string.Empty;
    private static string _audience = string.Empty;

    public AuthService(IErrorLoggingService errorLoggingService, IOptions<AuthManagement> options)
    {
        _errorLoggingService = errorLoggingService;
        _authManagement = options.Value;
        _clientId = _authManagement.ClientId ?? EnvironmentVariableHelper.AUTH_MANAGEMENT_CLIENT_ID;
        _clientSecret = _authManagement.ClientSecret ?? EnvironmentVariableHelper.AUTH_MANAGEMENT_CLIENT_SECRET;
        _issuer = _authManagement.Issuer ?? EnvironmentVariableHelper.AUTH_MANAGEMENT_ISSUER;
        _audience = _authManagement.Audience ?? EnvironmentVariableHelper.AUTH_MANAGEMENT_AUDIENCE;
    }

    private static bool IsTokenExpired(string token)
    {
        var jwtHandler = new JwtSecurityTokenHandler();
        var jwtToken = jwtHandler.ReadJwtToken(token);
        var expClaim = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "exp")?.Value;

        if (!string.IsNullOrEmpty(expClaim))
        {
            var exp = long.Parse(expClaim);
            var expDateTime = DateTimeOffset.FromUnixTimeSeconds(exp).UtcDateTime;

            return expDateTime <= DateTime.UtcNow;
        }
        return true;
    }

    public async Task<string?> GetAuth0ManagementAccessToken()
    {
        if (!string.IsNullOrEmpty(_cachedAccessToken))
        {
            var isExpired = IsTokenExpired(_cachedAccessToken);

            if (!isExpired)
            {
                return _cachedAccessToken;
            }
        }
        var tokenEndpoint = $"{_issuer}oauth/token";
        var requestBody = new Dictionary<string, string>
        {
            { "client_id",  _clientId},
            { "client_secret", _clientSecret },
            { "grant_type", "client_credentials" },
            { "audience", $"{_issuer}api/v2/" }
        };

        var response = await client.PostAsync(tokenEndpoint, new FormUrlEncodedContent(requestBody));
        var content = await response.Content.ReadAsStringAsync();

        if (response.IsSuccessStatusCode)
        {
            var jsonDoc = System.Text.Json.JsonDocument.Parse(content);
            _cachedAccessToken = jsonDoc.RootElement.GetProperty("access_token").GetString();
            return _cachedAccessToken;
        }
        else
        {
            throw new Exception($"Failed response from auth provider: {content}");
        }
    }

    public async Task<IPagedList<Role>> GetAllRolesAsync()
    {
        try
        {
            var token = await GetAuth0ManagementAccessToken();
            var client = new ManagementApiClient(token, new Uri($"{_issuer}api/v2"));
            var request = new GetRolesRequest();
            var pagination = new PaginationInfo(pageNo: 0, perPage: 50);
            return await client.Roles.GetAllAsync(request, pagination);
        }
        catch (Exception ex)
        {
            _errorLoggingService.LogException(ex);
            return null;
        }
    }

    public async Task<IPagedList<User>> GetAllUsersAsync()
    {
        try
        {
            var token = await GetAuth0ManagementAccessToken();
            var client = new ManagementApiClient(token, new Uri($"{_issuer}api/v2"));
            var request = new GetUsersRequest();
            var pagination = new PaginationInfo(pageNo: 0, perPage: 50);
            return await client.Users.GetAllAsync(request, pagination);
        }
        catch (Exception ex)
        {
            _errorLoggingService.LogException(ex);
            return null;
        }
    }

    public async Task<IList<User>> GetUsersByEmailAsync(string email)
    {
        try
        {
            var token = await GetAuth0ManagementAccessToken();
            var client = new ManagementApiClient(token, new Uri($"{_issuer}api/v2"));
            var user = await client.Users.GetUsersByEmailAsync(email);
            return user;
        }
        catch (Exception ex)
        {
            _errorLoggingService.LogException(ex);
            return null;
        }
    }

    public async Task<IPagedList<AssignedUser>> GetUsersByRoleAsync(string roleId)
    {
        var allRoles = await GetAllRolesAsync();
        bool roleFound = allRoles.Any(role => role.Id == roleId);

        if (roleFound)
        {
            try
            {
                var token = await GetAuth0ManagementAccessToken();
                var client = new ManagementApiClient(token, new Uri($"{_issuer}api/v2"));
                var users = await client.Roles.GetUsersAsync(roleId, new PaginationInfo(0, 50));
                return users;
            }
            catch (Exception ex)
            {
                _errorLoggingService.LogException(ex);
                return null;
            }
        }
        return null;
    }

    public async Task<IPagedList<Role>> GetUserRolesAsync(string userId)
    {
        var allUsers = await GetAllUsersAsync();
        bool userFound = allUsers.Any(user => user.UserId == userId);

        if (userFound)
        {
            try
            {
                var token = await GetAuth0ManagementAccessToken();
                var client = new ManagementApiClient(token, new Uri($"{_issuer}api/v2"));
                return await client.Users.GetRolesAsync(userId);
            }
            catch (Exception ex)
            {
                _errorLoggingService.LogException(ex);
                return null;
            }
        }
        return null;
    }

    public async Task<bool> AddRoleToUserAsync(string userId, string roleId)
    {
        var allUsers = await GetAllUsersAsync();
        bool userFound = allUsers.Any(user => user.UserId == userId);

        var allRoles = await GetAllRolesAsync();
        bool roleFound = allRoles.Any(role => role.Id == roleId);

        if (userFound && roleFound)
        {
            try
            {
                var token = await GetAuth0ManagementAccessToken();
                var client = new ManagementApiClient(token, new Uri($"{_issuer}api/v2"));
                await client.Users.AssignRolesAsync(userId, new AssignRolesRequest
                {
                    Roles = new[] { roleId }
                });
                return true;
            }
            catch (Exception ex)
            {
                _errorLoggingService.LogException(ex);
                return false;
            }
        }
        return false;
    }

    public async Task<bool> RemoveRoleFromUserAsync(string userId, string roleId)
    {
        var allUsers = await GetAllUsersAsync();
        bool userFound = allUsers.Any(user => user.UserId == userId);

        var allRoles = await GetAllRolesAsync();
        bool roleFound = allRoles.Any(role => role.Id == roleId);

        if (userFound && roleFound)
        {
            try
            {
                var token = await GetAuth0ManagementAccessToken();
                var client = new ManagementApiClient(token, new Uri($"{_issuer}api/v2"));
                await client.Users.RemoveRolesAsync(userId, new AssignRolesRequest
                {
                    Roles = new[] { roleId }
                });
                return true;
            }
            catch (Exception ex)
            {
                _errorLoggingService.LogException(ex);
                return false;
            }
        }
        return false;
    }

    public async Task<Role> CreateRoleAsync(string roleName, string description)
    {
        var temporaryRole = new Role();
        temporaryRole.Name = roleName;
        var allRoles = await GetAllRolesAsync();

        for (int roleCount = 0; roleCount < allRoles.Count(); roleCount++)
        {
            if (allRoles[roleCount].Name == roleName)
            {
                temporaryRole = allRoles[roleCount];
                return temporaryRole;
            }
        }

        try
        {
            var token = await GetAuth0ManagementAccessToken();
            var client = new ManagementApiClient(token, new Uri($"{_issuer}api/v2"));
            temporaryRole = await client.Roles.CreateAsync(new RoleCreateRequest
            {
                Name = roleName,
                Description = description
            });
            return temporaryRole;
        }
        catch (Exception ex)
        {
            _errorLoggingService.LogException(ex);
            return temporaryRole;
        }
    }

    public async Task<bool> DeleteRoleAsync(string roleId)
    {
        var allRoles = await GetAllRolesAsync();

        bool roleFound = allRoles.Any(role => role.Id == roleId);
        if (roleFound)
        {
            try
            {
                var token = await GetAuth0ManagementAccessToken();
                var client = new ManagementApiClient(token, new Uri($"{_issuer}api/v2"));
                await client.Roles.DeleteAsync(roleId);
                return true;
            }
            catch (Exception ex)
            {
                _errorLoggingService.LogException(ex);
                return false;
            };
        }
        return false;
    }

    public async Task<bool> UpdateRoleAsync(string roleId, string roleName, string description)
    {
        var allRoles = await GetAllRolesAsync();

        bool roleFound = allRoles.Any(role => role.Id == roleId);
        if (roleFound)
        {
            try
            {
                var token = await GetAuth0ManagementAccessToken();
                var client = new ManagementApiClient(token, new Uri($"{_issuer}api/v2"));
                var temporaryRole = await client.Roles.UpdateAsync(roleId, new RoleUpdateRequest
                {
                    Name = roleName,
                    Description = description
                });
                return true;
            }
            catch (Exception ex)
            {
                _errorLoggingService.LogException(ex);
                return false;
            }
        }
        return false;
    }

    public async Task<IPagedList<Permission>> GetPermissionsByRoleAsync(string roleId)
    {
        var allRoles = await GetAllRolesAsync();

        bool roleFound = allRoles.Any(role => role.Id == roleId);
        if (roleFound)
        {
            try
            {
                var token = await GetAuth0ManagementAccessToken();
                var client = new ManagementApiClient(token, new Uri($"{_issuer}api/v2"));
                var permissions = await client.Roles.GetPermissionsAsync(roleId, new PaginationInfo());
                return permissions;
            }
            catch (Exception ex)
            {
                _errorLoggingService.LogException(ex);
                return null;
            }
        }
        return null;
    }

    public async Task<bool> RemovePermissionsFromRoleAsync(string roleId, string permissionName)
    {
        var allRoles = await GetAllRolesAsync();

        bool roleFound = allRoles.Any(role => role.Id == roleId);
        if (roleFound)
        {
            try
            {
                var token = await GetAuth0ManagementAccessToken();
                var client = new ManagementApiClient(token, new Uri($"{_issuer}api/v2"));

                var permissions = new List<PermissionIdentity>
            {
                new PermissionIdentity { Name = permissionName, Identifier = _audience}
            };

                await client.Roles.RemovePermissionsAsync(roleId, new AssignPermissionsRequest
                {
                    Permissions = permissions,

                });

                return true;
            }
            catch (Exception ex)
            {
                _errorLoggingService.LogException(ex);
                return false;
            }
        }
        return false;
    }

    public async Task<bool> AddPermissionsToRoleAsync(string roleId, string permissionName)
    {
        var allRoles = await GetAllRolesAsync();

        bool roleFound = allRoles.Any(role => role.Id == roleId);
        if (roleFound)
        {
            try
            {
                var token = await GetAuth0ManagementAccessToken();
                var client = new ManagementApiClient(token, new Uri($"{_issuer}api/v2"));

                var permissions = new List<PermissionIdentity>
                {
                new PermissionIdentity { Name = permissionName, Identifier = _audience}
                };

                await client.Roles.AssignPermissionsAsync(roleId, new AssignPermissionsRequest
                {
                    Permissions = permissions
                });

                return true;
            }
            catch (Exception ex)
            {
                _errorLoggingService.LogException(ex);
                return false;
            }
        }
        return false;
    }

    public async Task<bool> CreateUser(UserCreateRequest request)
    {
        var allUsers = await GetAllUsersAsync();
        bool userFound = allUsers.Any(user => user.UserId == request.UserId);

        if (userFound || request == null)
        {
            return false;
        }
        try
        {
            var token = await GetAuth0ManagementAccessToken();
            var client = new ManagementApiClient(token, new Uri($"{_issuer}api/v2"));
            await client.Users.CreateAsync(request);
            return true;
        }
        catch (Exception ex)
        {
            _errorLoggingService.LogException(ex);
            return false;
        }
    }

    public async Task<User> GetUserById(string userId)
    {
        var allUsers = await GetAllUsersAsync();
        bool userFound = allUsers.Any(user => user.UserId == userId);

        if (userFound)
        {
            try
            {
                var token = await GetAuth0ManagementAccessToken();
                var client = new ManagementApiClient(token, new Uri($"{_issuer}api/v2"));
                var user = await client.Users.GetAsync(userId);
                return user;
            }
            catch (Exception ex)
            {
                _errorLoggingService.LogException(ex);
                return null;
            }
        }
        return null;
    }

    public async Task<bool> DeleteUser(string userId)
    {
        var token = await GetAuth0ManagementAccessToken();
        var client = new ManagementApiClient(token, new Uri($"{_issuer}api/v2"));
        var existingUser = GetUserById(userId);
        if (existingUser != null)
        {
            await client.Users.DeleteAsync(userId);
        }
        return true;
    }

    public async Task<bool> UpdateUser(string userId, UserUpdateRequest request)
    {
        var allUsers = await GetAllUsersAsync();
        bool userFound = allUsers.Any(user => user.UserName == request.UserName);

        if (!userFound || request != null)
        {
            return false;
        }
        try
        {
            var token = await GetAuth0ManagementAccessToken();
            var client = new ManagementApiClient(token, new Uri($"{_issuer}api/v2"));
            await client.Users.UpdateAsync(userId, request);
            return true;
        }
        catch (Exception ex)
        {
            _errorLoggingService.LogException(ex);
            return false;
        }
    }

}
