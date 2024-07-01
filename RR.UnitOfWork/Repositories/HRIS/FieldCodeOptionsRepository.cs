using RR.UnitOfWork.Entities.HRIS;

namespace RR.UnitOfWork.Repositories.HRIS;

public interface IFieldCodeOptionsRepository : IRepository<FieldCodeOptions>
{
}

public class FieldCodeOptionsRepository : BaseRepository<FieldCodeOptions>, IFieldCodeOptionsRepository
{
    public FieldCodeOptionsRepository(DatabaseContext db) : base(db)
    {
    }
}