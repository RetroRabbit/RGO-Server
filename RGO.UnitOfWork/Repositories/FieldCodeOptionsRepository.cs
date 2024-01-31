using RGO.UnitOfWork.Entities;
using RGO.Models;
using RGO.UnitOfWork.Interfaces;

namespace RGO.UnitOfWork.Repositories;

public class FieldCodeOptionsRepository : BaseRepository<FieldCodeOptions, FieldCodeOptionsDto>, IFieldCodeOptionsRepository
{
    public FieldCodeOptionsRepository(DatabaseContext db): base(db)
    {

    }
}
