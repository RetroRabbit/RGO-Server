using Microsoft.EntityFrameworkCore;
using RGO.Domain.Interfaces.Repository;
using RGO.Domain.Models;
using RGO.Repository.Entities;

namespace RGO.Repository.Repositories;

public class UserGroupsRepository : IUserGroupsRepository
{
    private readonly DatabaseContext _databaseContext;

    public UserGroupsRepository(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public async Task addUserGroup(UserGroupDTO userGroup)
    {
        await _databaseContext.usergroups.AddAsync(userGroup);
        await _databaseContext.SaveChangesAsync();
    }

    public async Task<UserGroupDTO[]> getUserGroups()
    {
        return await _databaseContext.usergroups
            .Select(group => new UserGroupDTO(group.title))
            .ToArrayAsync();
    }
}
