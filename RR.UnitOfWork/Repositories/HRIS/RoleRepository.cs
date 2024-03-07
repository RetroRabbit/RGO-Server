using HRIS.Models;
using RR.UnitOfWork.Entities.HRIS;
using RR.UnitOfWork.Interfaces.HRIS;

namespace RR.UnitOfWork.Repositories.HRIS;

public class RoleRepository : BaseRepository<Role, RoleDto>, IRoleRepository
{
    public RoleRepository(DatabaseContext databaseContext) : base(databaseContext)
    {
    }
}