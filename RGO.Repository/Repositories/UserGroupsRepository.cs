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
        var userGroups = await _databaseContext.usergroups.ToListAsync();
        var userGroupsDto = new List<UserGroupDto>();
        foreach (var userGroup in userGroups)
        {
            userGroupsDto.Add(userGroup.ToDTO());
        }
        
        return userGroupsDto;
    }
}
