using HRIS.Models;
using RR.UnitOfWork.Entities.HRIS;
using RR.UnitOfWork.Interfaces.HRIS;

namespace RR.UnitOfWork.Repositories.HRIS;

public class FieldCodeRepository : BaseRepository<FieldCode, FieldCodeDto>, IFieldCodeRepository
{
    public FieldCodeRepository(DatabaseContext db) : base(db)
    {
    }
}