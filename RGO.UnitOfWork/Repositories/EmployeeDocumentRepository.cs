using Microsoft.EntityFrameworkCore;
using RGO.Models;
using RGO.UnitOfWork.Entities;
using RGO.UnitOfWork.Interfaces;

namespace RGO.UnitOfWork.Repositories;

public class EmployeeDocumentRepository : BaseRepository<EmployeeDocument, EmployeeDocumentDto>, IEmployeeDocumentRepository
{
    public EmployeeDocumentRepository(DatabaseContext db) : base(db)
    {
    }
}
