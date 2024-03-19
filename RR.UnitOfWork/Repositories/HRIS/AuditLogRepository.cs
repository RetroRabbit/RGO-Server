using HRIS.Models;
using RR.UnitOfWork.Entities;
using RR.UnitOfWork.Interfaces.HRIS;

namespace RR.UnitOfWork.Repositories.HRIS;

public class AuditLogRepository : BaseRepository<AuditLog, AuditLogDto>, IAuditLogRepository
{
    public AuditLogRepository(DatabaseContext db) : base(db)
    {
    }
}