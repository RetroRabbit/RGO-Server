using HRIS.Models;
using RR.UnitOfWork.Entities;


namespace RR.UnitOfWork.Interfaces.HRIS;

public interface IAuditLogRepository : IRepository<AuditLog, AuditLogDto>
{
}