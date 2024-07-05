using RR.UnitOfWork.Entities.HRIS;

namespace RR.UnitOfWork.Repositories.HRIS;

public interface IFieldCodeRepository : IRepository<FieldCode>
{
}

public class FieldCodeRepository : BaseRepository<FieldCode>, IFieldCodeRepository
{
    public FieldCodeRepository(DatabaseContext db) : base(db)
    {
    }
}