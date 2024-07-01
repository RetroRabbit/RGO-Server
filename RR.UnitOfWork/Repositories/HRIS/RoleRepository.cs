using RR.UnitOfWork.Entities.HRIS;

namespace RR.UnitOfWork.Repositories.HRIS;

public interface IRoleRepository : IRepository<Role>
{
}

public class RoleRepository : BaseRepository<Role>, IRoleRepository
{
    public RoleRepository(DatabaseContext databaseContext) : base(databaseContext)
    {
    }
}