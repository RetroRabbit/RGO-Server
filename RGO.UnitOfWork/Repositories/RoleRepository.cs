using RGO.Models;
using RGO.UnitOfWork.Entities;
using RGO.UnitOfWork.Interfaces;

namespace RGO.UnitOfWork.Repositories;

public class RoleRepository : BaseRepository<Role, RoleDto>, IRoleRepository
{
    public RoleRepository(DatabaseContext databaseContext) : base(databaseContext)
    {
    }
}
