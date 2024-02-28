using HRIS.Models;
using RR.UnitOfWork.Entities.HRIS;
using RR.UnitOfWork.Interfaces.HRIS;

namespace RR.UnitOfWork.Repositories.HRIS;

public class RoleAccessLinkRepository : BaseRepository<RoleAccessLink, RoleAccessLinkDto>, IRoleAccessLinkRepository
{
    public RoleAccessLinkRepository(DatabaseContext databaseContext) : base(databaseContext)
    {
    }
}