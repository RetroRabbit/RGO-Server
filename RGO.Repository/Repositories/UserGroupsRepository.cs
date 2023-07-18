using Microsoft.EntityFrameworkCore;
using RGO.Repository.Entities;
using RGO.Repository.Interfaces;

namespace RGO.Repository.Repositories
{
    public class UserGroupsRepository : IUserGroupsRepository
    {
        private readonly DatabaseContext _databaseContext;

        public UserGroupsRepository(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task addUserGroup(UserGroup userGroup)
        {
            await _databaseContext.usergroups.AddAsync(userGroup);
            await _databaseContext.SaveChangesAsync();
        }

        public async Task<UserGroup[]> getUserGroups()
        {
            return await _databaseContext.usergroups.ToArrayAsync();
        }
    }
}
