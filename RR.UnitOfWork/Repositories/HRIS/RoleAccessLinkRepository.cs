using RR.UnitOfWork.Entities.HRIS;

namespace RR.UnitOfWork.Repositories.HRIS;

public interface IRoleAccessLinkRepository : IRepository<RoleAccessLink>
{
}

public class RoleAccessLinkRepository : BaseRepository<RoleAccessLink>, IRoleAccessLinkRepository
{
    public RoleAccessLinkRepository(DatabaseContext databaseContext) : base(databaseContext)
    {
    }
}