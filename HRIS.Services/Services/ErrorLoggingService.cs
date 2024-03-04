using ATS.Models;
using HRIS.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using RR.UnitOfWork;
using RR.UnitOfWork.Entities;
using RR.UnitOfWork.Entities.ATS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRIS.Services.Services;

internal class ErrorLoggingService : IErrorLoggingService
{
    private readonly IUnitOfWork _db;

    public ErrorLoggingService(IUnitOfWork db)
    {
        _db = db;
    }

    public async Task<bool> CheckErrorLogExists(int Id)
    {
        return await _db.ErrorLogging
            .Any(errorLog => errorLog.Id == Id);
    }

    public async Task<ErrorLoggingDto> DeleteErrorLog(int id) => await
    _db.ErrorLogging.Delete(id);

    public async Task<List<ErrorLoggingDto>> GetAllErrorLogs() =>
        await _db.ErrorLogging.GetAll();

    public async Task<ErrorLoggingDto> GetErrorLogById(int id) =>
        await _db.ErrorLogging.Get(errorLog => errorLog.Id == id)
        .Select(errorLog => errorLog.ToDto())
        .FirstAsync();

    public async Task<ErrorLoggingDto> SaveErrorLog(ErrorLoggingDto errorLog)
    {
        if (await CheckErrorLogExists(errorLog.Id))
            throw new Exception("Error Log Already exists");

        ErrorLogging newErroLog = new ErrorLogging(errorLog);
        return await _db.ErrorLogging.Add(newErroLog);
    }
}
