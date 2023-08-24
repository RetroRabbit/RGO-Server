using RGO.Models;
using RGO.UnitOfWork.Entities;
using RGO.UnitOfWork.Interfaces;

namespace RGO.UnitOfWork.Repositories;

public class AuditLogRepository : BaseRepository<AuditLog, AuditLogDto>, IAuditLogRepository
{
    public AuditLogRepository(DatabaseContext db) : base(db)
    {
    }
}