using RR.UnitOfWork.Entities.HRIS;

namespace RR.UnitOfWork.Repositories.HRIS;

public interface IRoleAccessRepository : IRepository<RoleAccess>
{
}

public class RoleAccessRepository : BaseRepository<RoleAccess>, IRoleAccessRepository
{
    public RoleAccessRepository(DatabaseContext databaseContext) : base(databaseContext)
    {
    }
}