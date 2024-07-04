using ATS.Models;
using HRIS.Services.Interfaces;
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

    public async Task SaveErrorLog(ErrorLoggingDto errorLog)
    {
        await _db.ErrorLogging.Add(new ErrorLogging(errorLog));
    }

    public Exception LogException(Exception exception)
    {
        try
        {
            var errorLog = new ErrorLoggingDto
            {
                DateOfIncident = DateTime.Now,
                Message = exception.Message,
                StackTrace = exception.ToString(),
                IpAddress = string.Empty,
                RequestUrl = string.Empty,
                RequestMethod = string.Empty,
            };
            Task.Run(async () => await SaveErrorLog(errorLog)).Wait();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error occurred while logging exception: {ex.Message}");
        }

        return exception;
    }

    public async Task LogException(ErrorLoggingDto error)
    {
        await SaveErrorLog(error);
    }
}
