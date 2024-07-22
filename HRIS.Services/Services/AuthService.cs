using Auth0.ManagementApi;
using Auth0.ManagementApi.Models;
using Auth0.ManagementApi.Paging;
using HRIS.Models;
using HRIS.Services.Helpers;
using HRIS.Services.Interfaces;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;

namespace HRIS.Services.Services;

public class AuthService : IAuthService
{
    private readonly IManagementApiClient _managementApiClient;
    private readonly AuthManagement _authManagement;
    private string _cachedAccessToken;
    private string _clientId;
    private string _clientSecret;
    private string _issuer;
    private string _audience;
    private HttpClient _httpClient;

    public AuthService(IOptions<AuthManagement> options)
    {
        _authManagement = options.Value;
        _clientId = _authManagement.ClientId ?? EnvironmentVariableHelper.AUTH_MANAGEMENT_CLIENT_ID;
        _clientSecret = _authManagement.ClientSecret ?? EnvironmentVariableHelper.AUTH_MANAGEMENT_CLIENT_SECRET;
        _issuer = _authManagement.Issuer ?? EnvironmentVariableHelper.AUTH_MANAGEMENT_ISSUER;
        _audience = _authManagement.Audience ?? EnvironmentVariableHelper.AUTH_MANAGEMENT_AUDIENCE;
        _cachedAccessToken = string.Empty;
        _managementApiClient = new ManagementApiClient(_cachedAccessToken, new Uri($"{_issuer}api/v2"));
        _httpClient = new HttpClient();
    }

    public static bool IsTokenExpired(string token)
    {
        var jwtHandler = new JwtSecurityTokenHandler();
        var jwtToken = jwtHandler.ReadJwtToken(token);
        var expClaim = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "exp")?.Value;

        if (string.IsNullOrEmpty(expClaim))
            return true;

        var exp = long.Parse(expClaim);
        var expDateTime = DateTimeOffset.FromUnixTimeSeconds(exp).UtcDateTime;

        return expDateTime <= DateTime.UtcNow;
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
        var response = await _httpClient.PostAsync(tokenEndpoint, new FormUrlEncodedContent(requestBody));
        var content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
            throw new CustomException($"Failed response from auth provider: {content}");

        var jsonDoc = System.Text.Json.JsonDocument.Parse(content);

        if (jsonDoc.RootElement.TryGetProperty("access_token", out JsonElement accessTokenElement))
        {
            _cachedAccessToken = accessTokenElement.ToString();
            return _cachedAccessToken;
        }

        throw new CustomException($"Failed response from auth provider. access_token key not found.");
    }

    public async Task<IPagedList<Auth0.ManagementApi.Models.Role>> GetAllRolesAsync()
    {
        var token = await GetAuth0ManagementAccessToken();
        _managementApiClient.UpdateAccessToken(token);

        var request = new GetRolesRequest();
        var pagination = new PaginationInfo(pageNo: 0, perPage: 50);

        var allRoles = await _managementApiClient.Roles.GetAllAsync(request, pagination);
        if (allRoles == null)
            throw new CustomException("Failed to retrieve roles.");

        return allRoles;
    }

    public async Task<IPagedList<Auth0.ManagementApi.Models.User>> GetAllUsersAsync()
    {
        var token = await GetAuth0ManagementAccessToken();
        _managementApiClient.UpdateAccessToken(token);

        var request = new GetUsersRequest();
        var pagination = new PaginationInfo(pageNo: 0, perPage: 50);

        var allUsers = await _managementApiClient.Users.GetAllAsync(request, pagination);
        if (allUsers == null)
            throw new CustomException("Failed to retrieve roles.");

        return allUsers;
    }

    public async Task<IList<Auth0.ManagementApi.Models.User>> GetUsersByEmailAsync(string email)
    {
        var token = await GetAuth0ManagementAccessToken();
        _managementApiClient.UpdateAccessToken(token);

        var user = await _managementApiClient.Users.GetUsersByEmailAsync(email);
        if (user == null)
            throw new CustomException("Failed to retrieve user by email.");

        return user;
    }

    public async Task<IPagedList<Auth0.ManagementApi.Models.AssignedUser>> GetUsersByRoleAsync(string roleId)
    {
        var allRoles = await GetAllRolesAsync();

        var role = allRoles.FirstOrDefault(r => r.Id == roleId);
        if (role == null)
            throw new CustomException($"Role with id '{roleId}' not found.");

        var token = await GetAuth0ManagementAccessToken();
        _managementApiClient.UpdateAccessToken(token);

        var users = await _managementApiClient.Roles.GetUsersAsync(roleId, new PaginationInfo(0, 50));
        if (users == null)
            throw new CustomException("Failed to retrieve users.");

        return users;
    }

    public async Task<IPagedList<Auth0.ManagementApi.Models.Role>> GetUserRolesAsync(string userId)
    {
        var allUsers = await GetAllUsersAsync();

        var users = allUsers.FirstOrDefault(r => r.UserId == userId);
        if (users == null)
            throw new CustomException($"User with id '{userId}' not found.");

        var token = await GetAuth0ManagementAccessToken();
        _managementApiClient.UpdateAccessToken(token);

        var userRoles = await _managementApiClient.Users.GetRolesAsync(userId);
        if (userRoles == null)
            throw new CustomException($"User role found.");

        return userRoles;
    }

    public async Task<bool> AddRoleToUserAsync(string userId, string roleId)
    {
        var allUsers = await GetAllUsersAsync();

        var users = allUsers.FirstOrDefault(r => r.UserId == userId);
        if (users == null)
            throw new CustomException($"User with id '{userId}' not found.");

        var allRoles = await GetAllRolesAsync();
        var role = allRoles.FirstOrDefault(r => r.Id == roleId);
        if (role == null)
            throw new CustomException($"Role with id '{roleId}' not found.");

        var token = await GetAuth0ManagementAccessToken();
        _managementApiClient.UpdateAccessToken(token);

        await _managementApiClient.Users.AssignRolesAsync(userId, new AssignRolesRequest
        {
            Roles = new[] { roleId }
        });
        return true;
    }

    public async Task<bool> RemoveRoleFromUserAsync(string userId, string roleId)
    {
        var allUsers = await GetAllUsersAsync();

        var users = allUsers.FirstOrDefault(r => r.UserId == userId);
        if (users == null)
            throw new CustomException($"User with id '{userId}' not found.");

        var allRoles = await GetAllRolesAsync();
        var role = allRoles.FirstOrDefault(r => r.Id == roleId);
        if (role == null)
            throw new CustomException($"Role with id '{roleId}' not found.");

        var token = await GetAuth0ManagementAccessToken();
        _managementApiClient.UpdateAccessToken(token);

        await _managementApiClient.Users.RemoveRolesAsync(userId, new AssignRolesRequest
        {
            Roles = new[] { roleId }
        });
        return true;
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
        var token = await GetAuth0ManagementAccessToken();
        _managementApiClient.UpdateAccessToken(token);
        temporaryRole = await _managementApiClient.Roles.CreateAsync(new RoleCreateRequest
        {
            Name = roleName,
            Description = description
        });
        return temporaryRole;
    }

    public async Task<bool> DeleteRoleAsync(string roleId)
    {
        var allRoles = await GetAllRolesAsync();

        bool roleFound = allRoles.Any(role => role.Id == roleId);
        if (!roleFound)
            return false;

        var token = await GetAuth0ManagementAccessToken();
        _managementApiClient.UpdateAccessToken(token);

        await _managementApiClient.Roles.DeleteAsync(roleId);
        return true;
    }

    public async Task<bool> UpdateRoleAsync(string roleId, string roleName, string description)
    {
        var allRoles = await GetAllRolesAsync();

        bool roleFound = allRoles.Any(role => role.Id == roleId);
        if (!roleFound)
            return false;

        var token = await GetAuth0ManagementAccessToken();
        _managementApiClient.UpdateAccessToken(token);

        var temporaryRole = await _managementApiClient.Roles.UpdateAsync(roleId, new RoleUpdateRequest
        {
            Name = roleName,
            Description = description
        });
        return true;
    }

    public async Task<IPagedList<Permission>> GetPermissionsByRoleAsync(string roleId)
    {
        var allRoles = await GetAllRolesAsync();

        bool roleFound = allRoles.Any(role => role.Id == roleId);
        if (!roleFound)
            throw new CustomException($"Role not found.");

        var token = await GetAuth0ManagementAccessToken();
        _managementApiClient.UpdateAccessToken(token);

        var permissions = await _managementApiClient.Roles.GetPermissionsAsync(roleId, new PaginationInfo());
        return permissions;
    }

    public async Task<bool> RemovePermissionsFromRoleAsync(string roleId, string permissionName)
    {
        var allRoles = await GetAllRolesAsync();

        bool roleFound = allRoles.Any(role => role.Id == roleId);
        if (!roleFound)
            return false;

        var token = await GetAuth0ManagementAccessToken();
        _managementApiClient.UpdateAccessToken(token);

        var permissions = new List<PermissionIdentity>
            {
                new PermissionIdentity { Name = permissionName, Identifier = _audience}
            };

        await _managementApiClient.Roles.RemovePermissionsAsync(roleId, new AssignPermissionsRequest
        {
            Permissions = permissions,

        });

        return true;
    }

    public async Task<bool> AddPermissionsToRoleAsync(string roleId, string permissionName)
    {
        var allRoles = await GetAllRolesAsync();

        bool roleFound = allRoles.Any(role => role.Id == roleId);
        if (!roleFound)
            return false;

        var token = await GetAuth0ManagementAccessToken();
        _managementApiClient.UpdateAccessToken(token);

        var permissions = new List<PermissionIdentity>
                {
                new PermissionIdentity { Name = permissionName, Identifier = _audience}
                };

        await _managementApiClient.Roles.AssignPermissionsAsync(roleId, new AssignPermissionsRequest
        {
            Permissions = permissions
        });

        return true;
    }

    public async Task<bool> CreateUser(UserCreateRequest request)
    {
        var allUsers = await GetAllUsersAsync();
        bool userFound = allUsers.Any(user => user.UserId == request.UserId);

        if (userFound)
            return false;

        var token = await GetAuth0ManagementAccessToken();
        _managementApiClient.UpdateAccessToken(token);
        await _managementApiClient.Users.CreateAsync(request);
        return true;
    }

    public async Task<User> GetUserById(string userId)
    {
        var allUsers = await GetAllUsersAsync();
        bool userFound = allUsers.Any(user => user.UserId == userId);

        if (!userFound)
            throw new CustomException($"User not found.");

        var token = await GetAuth0ManagementAccessToken();
        _managementApiClient.UpdateAccessToken(token);
        var user = await _managementApiClient.Users.GetAsync(userId);
        return user;
    }

    public async Task<bool> DeleteUser(string userId)
    {
        var token = await GetAuth0ManagementAccessToken();
        _managementApiClient.UpdateAccessToken(token);
        var existingUser = GetUserById(userId);
        if (existingUser != null)
            await _managementApiClient.Users.DeleteAsync(userId);

        return true;
    }

    public async Task<bool> UpdateUser(string userId, UserUpdateRequest request)
    {
        var allUsers = await GetAllUsersAsync();
        bool userFound = allUsers.Any(user => user.UserName == request.UserName);

        if (!userFound)
            return false;

        var token = await GetAuth0ManagementAccessToken();
        _managementApiClient.UpdateAccessToken(token);
        await _managementApiClient.Users.UpdateAsync(userId, request);
        return true;
    }
}
