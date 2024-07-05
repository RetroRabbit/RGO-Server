using ATS.Models;

namespace HRIS.Services.Interfaces
{
    public interface IErrorLoggingService
    {
        /// <summary>
        ///     This Logs an exception
        /// </summary>
        /// <param name="exception"></param>
        /// <returns>A logged exception</returns>
        Exception LogException(Exception exception);

        Task LogException(ErrorLoggingDto error);

    }
}
