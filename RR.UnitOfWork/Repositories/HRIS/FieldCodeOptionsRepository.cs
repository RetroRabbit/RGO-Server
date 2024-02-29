using HRIS.Models;
using RR.UnitOfWork.Entities.HRIS;
using RR.UnitOfWork.Interfaces.HRIS;

namespace RR.UnitOfWork.Repositories.HRIS;

public class FieldCodeOptionsRepository : BaseRepository<FieldCodeOptions, FieldCodeOptionsDto>,
                                          IFieldCodeOptionsRepository
{
    public FieldCodeOptionsRepository(DatabaseContext db) : base(db)
    {
    }
}