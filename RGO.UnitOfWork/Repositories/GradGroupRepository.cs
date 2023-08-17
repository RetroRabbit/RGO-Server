using RGO.Models;
using RGO.UnitOfWork.Entities;
using RGO.UnitOfWork.Interfaces;

namespace RGO.UnitOfWork.Repositories;

public class GradGroupRepository : BaseRepository<GradGroup, GradGroupDto>, IGradGroupRepository
{
    public GradGroupRepository(DatabaseContext databaseContext) : base(databaseContext)
    {
    }
}
