using RR.UnitOfWork.Entities.HRIS;

namespace RR.UnitOfWork.Repositories.HRIS;

public interface IClientRepository : IRepository<Client>
{
}

public class ClientRepository : BaseRepository<Client>, IClientRepository
{
    public ClientRepository(DatabaseContext db) : base(db)
    {
    }
}