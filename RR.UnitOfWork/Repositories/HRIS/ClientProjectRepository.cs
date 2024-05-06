using HRIS.Models;
using RR.UnitOfWork.Entities.HRIS;
using RR.UnitOfWork.Interfaces.HRIS;

namespace RR.UnitOfWork.Repositories.HRIS
{
    public class ClientProjectRepository : BaseRepository<ClientProject, ClientProjectsDto>, IClientProjectRepository
    {
        public ClientProjectRepository(DatabaseContext db) : base(db)
        {

        }
    }
}
