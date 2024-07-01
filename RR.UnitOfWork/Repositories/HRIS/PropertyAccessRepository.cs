using RR.UnitOfWork.Entities.HRIS;

namespace RR.UnitOfWork.Repositories.HRIS;

public interface IPropertyAccessRepository : IRepository<PropertyAccess>
{
}

public class PropertyAccessRepository : BaseRepository<PropertyAccess>, IPropertyAccessRepository
{
    public PropertyAccessRepository(DatabaseContext db) : base(db)
    {
    }
}