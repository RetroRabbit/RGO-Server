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


    public async Task<List<UserGroupDto>> GetUserGroups()
    {
        return await _databaseContext.usergroups
            .Select(group => group.ToDTO())
            .ToListAsync();
    }
}
