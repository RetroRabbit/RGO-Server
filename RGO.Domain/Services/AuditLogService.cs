using Microsoft.EntityFrameworkCore;
using RGO.Models;
using RGO.Services.Interfaces;
using RGO.UnitOfWork;
using RGO.UnitOfWork.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RGO.Services.Services
{
    public class AuditLogService : IAuditLogService
    {
        private readonly IUnitOfWork _db;

        public AuditLogService(IUnitOfWork db)
        {
            _db = db;
        }

        public async Task SaveAuditLog(AuditLogDto auditLogDto)
        {
            AuditLog auditLog = new AuditLog(auditLogDto);
            await _db.AuditLog.Add(auditLog);
        }

        public async Task<List<AuditLogDto>> GetAuditLogByEditedById(int editedById)
        {
           return await _db.AuditLog.GetAll(auditLog => auditLog.EditBy == editedById);
        }

        public async Task<List<AuditLogDto>> GetAuditLogByEditedForId(int editedForId)
        {
            return await _db.AuditLog.GetAll(auditLog => auditLog.EditFor == editedForId);
        }

        public async Task<List<AuditLogDto>> GetAllAuditLogs()
        {
            return await _db.AuditLog.GetAll();
        }

        public async Task UpdateAuditLog(AuditLogDto auditLogDto)
        {
            var ifAuditLog = await CheckAuditLog(auditLogDto.Id);
            if (!ifAuditLog) { throw new Exception("Audit Log not found"); }

            AuditLog auditLog = new AuditLog(auditLogDto);
            await _db.AuditLog.Update(auditLog);
        }

        public async Task DeleteAuditLog(AuditLogDto auditLogDto)
        {
            var ifAuditLog = await CheckAuditLog(auditLogDto.Id);
            if (!ifAuditLog) { throw new Exception("Audit Log not found"); }

            AuditLog auditLog = new AuditLog(auditLogDto);
            await _db.AuditLog.Delete(auditLog.Id);
        }

        private async Task<bool> CheckAuditLog(int auditLogId)
        {
            var auditLog = await _db.AuditLog
            .Get(auditLog => auditLog.Id == auditLogId)
            .FirstOrDefaultAsync();

            if (auditLog == null) { return false; }
            else { return true; }
        }
    }
}
