using RGO.Models;
using RGO.UnitOfWork.Entities;
using RGO.UnitOfWork.Interfaces;

namespace RGO.UnitOfWork.Repositories
{
    public class FieldCodeRepository : BaseRepository<FieldCode, FieldCodeDto>, IFieldCodeRepository
    {
        public FieldCodeRepository(DatabaseContext databaseContext) : base(databaseContext)
        {
        }
    }
}
