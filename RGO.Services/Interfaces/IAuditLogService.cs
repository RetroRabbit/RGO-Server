using HRIS.Models;

namespace HRIS.Services.Interfaces;

public interface IAuditLogService
{
    /// <summary>
    ///     Get Audit Log by Edited By Id
    /// </summary>
    /// <param name="editedById"></param>
    /// <returns>List of audits</returns>
    Task<List<AuditLogDto>> GetAuditLogByEditedById(int editedById);

    /// <summary>
    ///     Get Audit Log by Edited For Id
    /// </summary>
    /// <param name="editedForId"></param>
    /// <returns>List of audits</returns>
    Task<List<AuditLogDto>> GetAuditLogByEditedForId(int editedForId);

    /// <summary>
    ///     Get All Audit Logs
    /// </summary>
    /// <returns>List of Audits</returns>
    Task<List<AuditLogDto>> GetAllAuditLogs();

    /// <summary>
    ///     Save Audit Log
    /// </summary>
    /// <param name="auditLogDto"></param>
    /// <returns></returns>
    Task<AuditLogDto> SaveAuditLog(AuditLogDto auditLogDto);

    /// <summary>
    ///     Update Audit Log
    /// </summary>
    /// <param name="auditLogDto"></param>
    /// <returns>Audit Log</returns>
    Task<AuditLogDto> UpdateAuditLog(AuditLogDto auditLogDto);

    /// <summary>
    ///     Delete Audit Log
    /// </summary>
    /// <param name="auditLogDto"></param>
    /// <returns>AuditLog</returns>
    Task<AuditLogDto> DeleteAuditLog(AuditLogDto auditLogDto);
}