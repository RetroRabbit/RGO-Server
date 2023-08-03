using RGO.Domain.Models;

namespace RGO.Domain.Interfaces.Repository;

public interface IUserGroupsRepository
{
    Task<UserGroupDto[]> GetUserGroups();
}
