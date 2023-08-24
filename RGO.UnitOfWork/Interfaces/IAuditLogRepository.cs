using RGO.Models;
using RGO.UnitOfWork.Entities;

namespace RGO.UnitOfWork.Interfaces;

public interface IAuditLogRepository : IRepository<AuditLog, AuditLogDto>
{
}
