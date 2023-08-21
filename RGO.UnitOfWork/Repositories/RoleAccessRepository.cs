using RGO.Models;
using RGO.UnitOfWork.Entities;
using RGO.UnitOfWork.Interfaces;

namespace RGO.UnitOfWork.Repositories;

public class RoleAccessRepository : BaseRepository<RoleAccess, RoleAccessDto>, IRoleAccessRepository
{
    public RoleAccessRepository(DatabaseContext databaseContext) : base(databaseContext)
    {
    }
}