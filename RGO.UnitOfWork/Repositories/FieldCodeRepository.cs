using RGO.UnitOfWork.Entities;
using RGO.Models;
using RGO.UnitOfWork.Interfaces;

namespace RGO.UnitOfWork.Repositories;

public class FieldCodeRepository : BaseRepository<FieldCode, FieldCodeDto>, IFieldCodeRepository
{
    public FieldCodeRepository(DatabaseContext db): base(db) 
    {
    }
}
