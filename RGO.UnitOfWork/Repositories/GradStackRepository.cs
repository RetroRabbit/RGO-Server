using RGO.Models;
using RGO.UnitOfWork.Entities;
using RGO.UnitOfWork.Interfaces;

namespace RGO.UnitOfWork.Repositories
{
    public class GradStackRepository : BaseRepository<GradStacks, GradStackDto>, IGradStackRepository
    {
        public GradStackRepository(DatabaseContext databaseContext) : base(databaseContext)
        {
        }
    }
}
