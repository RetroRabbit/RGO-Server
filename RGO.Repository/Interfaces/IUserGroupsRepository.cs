using RGO.Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RGO.Repository.Interfaces
{
    public interface IUserGroupsRepository
    {
        Task<UserGroup[]> getUserGroups();

        Task addUserGroup(UserGroup userGroup);
    }
}
