using RR.UnitOfWork.Entities.HRIS;

namespace RR.UnitOfWork.Repositories;

public interface IClientProjectRepository : IRepository<ClientProject>
{
}

public class ClientProjectRepository : BaseRepository<ClientProject>, IClientProjectRepository
{
    public ClientProjectRepository(DatabaseContext db) : base(db)
    {

    }
}
