using RGO.Models;
using RGO.UnitOfWork.Entities;
using RGO.UnitOfWork.Interfaces;

namespace RGO.UnitOfWork.Repositories
{
    public class GradEventsRepository : BaseRepository<GradEvents, GradEventsDto>, IGradEventsRepository
    {
        public GradEventsRepository(DatabaseContext databaseContext) : base(databaseContext)
        {
        }
    }
}
