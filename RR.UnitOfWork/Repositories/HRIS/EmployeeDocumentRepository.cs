using HRIS.Models;
using RR.UnitOfWork.Entities.HRIS;
using RR.UnitOfWork.Interfaces.HRIS;

namespace RR.UnitOfWork.Repositories.HRIS;

public class EmployeeDocumentRepository : BaseRepository<EmployeeDocument, EmployeeDocumentDto>,
                                          IEmployeeDocumentRepository
{
    public EmployeeDocumentRepository(DatabaseContext db) : base(db)
    {
    }
}