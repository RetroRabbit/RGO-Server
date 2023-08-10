using RGO.Domain.Models;

namespace RGO.Domain.Interfaces.Repository;

public interface IUserGroupsRepository
{
    /// <summary>
    /// Retrieve User Groups
    /// </summary>
    /// <returns>List of User Groups</returns>
    public Task<List<UserGroupDto>> GetUserGroups();
}
