using RGO.Models;
using RGO.UnitOfWork.Entities;
using RGO.UnitOfWork.Interfaces;

namespace RGO.UnitOfWork.Repositories
{
    public class StackRepository : BaseRepository<Stacks, StacksDto>, IStackRepository
    {
        public StackRepository(DatabaseContext databaseContext) : base(databaseContext)
        {
        }
    }
}
