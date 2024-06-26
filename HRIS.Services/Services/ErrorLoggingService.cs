using ATS.Models;
using HRIS.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
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

    public async Task SaveErrorLog(ErrorLoggingDto errorLog)
    {
        ErrorLogging newErroLog = new ErrorLogging(errorLog);
        await _db.ErrorLogging.Add(newErroLog);
    }

    public Exception LogException(Exception exception, int statusCode = 500, string errorDetails = null)
    {
        try
        {
            DateTime utcNow = DateTime.UtcNow;
            TimeZoneInfo targetTimeZone = TimeZoneInfo.FindSystemTimeZoneById("South Africa Standard Time");
            DateTime targetLocalTime = TimeZoneInfo.ConvertTimeFromUtc(utcNow, targetTimeZone);

            ErrorLoggingDto errorLog = new ErrorLoggingDto
            {
                DateOfIncident = targetLocalTime,
                Message = exception.Message,
                StackTrace = exception.StackTrace,  
                StatusCode = statusCode,
                ErrorDetails = errorDetails
            };
            Task.Run(async () => await SaveErrorLog(errorLog)).Wait();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error occurred while logging exception: {ex.Message}");
        }

        return exception;  
    }
}
