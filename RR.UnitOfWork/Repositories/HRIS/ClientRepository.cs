using HRIS.Models;
using RR.UnitOfWork.Entities.HRIS;
using RR.UnitOfWork.Interfaces.HRIS;

namespace RR.UnitOfWork.Repositories.HRIS;

public class ClientRepository : BaseRepository<Client, ClientDto>, IClientRepository
{
    public ClientRepository(DatabaseContext db) : base(db)
    {
    }
}