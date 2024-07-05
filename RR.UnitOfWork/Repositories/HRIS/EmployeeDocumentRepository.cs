using RR.UnitOfWork.Entities.HRIS;

namespace RR.UnitOfWork.Repositories.HRIS;

public interface IEmployeeDocumentRepository : IRepository<EmployeeDocument>
{
}

public class EmployeeDocumentRepository : BaseRepository<EmployeeDocument>, IEmployeeDocumentRepository
{
    public EmployeeDocumentRepository(DatabaseContext db) : base(db)
    {
    }
}