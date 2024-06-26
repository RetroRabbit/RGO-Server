using ATS.Models;
using HRIS.Models;
using HRIS.Services.Interfaces;
using RR.UnitOfWork;
using RR.UnitOfWork.Entities;

namespace HRIS.Services.Services;

public class AuditLogService : IAuditLogService
{
    private readonly IUnitOfWork _db;
    private readonly IErrorLoggingService _errorLoggingService;

    public AuditLogService(IUnitOfWork db, IErrorLoggingService errorLoggingService)
    {
        _db = db;
        _errorLoggingService = errorLoggingService;
    }

    public async Task<AuditLogDto> SaveAuditLog(AuditLogDto auditLogDto)
    {
        var auditLog = new AuditLog(auditLogDto);
        var newAuditLog = await _db.AuditLog.Add(auditLog);
        return newAuditLog;
    }

    public async Task<List<AuditLogDto>> GetAuditLogByEditedById(int editedById)
    {
        return await _db.AuditLog.GetAll(auditLog => auditLog.CreatedById == editedById);
    }

    public async Task<List<AuditLogDto>> GetAuditLogByEditedForId(int editedForId)
    {
        return await _db.AuditLog.GetAll(auditLog => auditLog.CreatedById == editedForId);
    }

    public async Task<List<AuditLogDto>> GetAllAuditLogs()
    {
        return await _db.AuditLog.GetAll();
    }

    public async Task<AuditLogDto> UpdateAuditLog(AuditLogDto auditLogDto)
    {
        var ifAuditLog = await CheckAuditLog(auditLogDto.Id);
        if (!ifAuditLog)
        {
            var exception = new Exception("Audit Log not found");

           // throw _errorLoggingService.LogException(exception);
        }                                                            

        var auditLog = new AuditLog(auditLogDto);
        var updatedAuditLog = await _db.AuditLog.Update(auditLog);

        return updatedAuditLog;
    }

    public async Task<AuditLogDto> DeleteAuditLog(AuditLogDto auditLogDto)
    {
        var ifAuditLog = await CheckAuditLog(auditLogDto.Id);
        if (!ifAuditLog) 
        { 
            var exception = new Exception("Audit Log not found");
           // throw _errorLoggingService.LogException(exception);
        } 

        var auditLog = new AuditLog(auditLogDto);
        var deletedAuditLog = await _db.AuditLog.Delete(auditLog.Id);

        return deletedAuditLog;
    }

    private async Task<bool> CheckAuditLog(int auditLogId)
    {
        var auditLog = await _db.AuditLog.GetById(auditLogId);
        return auditLog != null;
    }
}
