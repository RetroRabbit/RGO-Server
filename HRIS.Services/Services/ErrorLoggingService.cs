using ATS.Models;
using HRIS.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using RR.UnitOfWork;
using RR.UnitOfWork.Entities;


namespace HRIS.Services.Services;

public class ErrorLoggingService : IErrorLoggingService
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

    public async void SaveErrorLog(ErrorLoggingDto errorLog)
    {
        ErrorLogging newErroLog = new ErrorLogging(errorLog);
        await _db.ErrorLogging.Add(newErroLog);
    }

    public void LogException(Exception exception)
    {
        DateTime localDateTime = DateTime.Now;
        DateTime utcDateTime = localDateTime.ToUniversalTime();

        ErrorLoggingDto errorLog = new ErrorLoggingDto
        {
            dateOfIncident = utcDateTime,
            message = exception.Message,
            exceptionType = exception.GetType().FullName,
        };
        SaveErrorLog(errorLog);
        throw exception;
    }
}
