using HRIS.Models;
using RR.UnitOfWork.Entities.HRIS;
using RR.UnitOfWork.Interfaces.HRIS;

namespace RR.UnitOfWork.Repositories.HRIS;

public class RoleAccessRepository : BaseRepository<RoleAccess, RoleAccessDto>, IRoleAccessRepository
{
    public RoleAccessRepository(DatabaseContext databaseContext) : base(databaseContext)
    {
    }
}