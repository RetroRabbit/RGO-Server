using ATS.Models;

namespace HRIS.Services.Interfaces
{
    public interface IErrorLoggingService
    {
        /// <summary>
        ///     Check if the error log exists in the system
        /// </summary>
        /// <param name="message"></param>
        /// <returns>Bool depending if on they exists or not</returns>
        Task<bool> CheckErrorLogExists(int Id);

        /// <summary>
        ///     Takes in a errorlog and adds it to the database
        /// </summary>
        /// <param name="errorLoggingDto"></param>
        /// <returns>The new ErrorLoggingDto</returns>
        Task SaveErrorLog(ErrorLoggingDto errorLoggingDto);

        /// <summary>
        ///     Returns a list of all errorlogs
        /// </summary>
        /// <returns>List<ErrorLoggingDto</returns>
        Task<List<ErrorLoggingDto>> GetAllErrorLogs();

        /// <summary>
        ///     Fetches a single errorlog by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Single ErrorLoggingDto</returns>
        Task<ErrorLoggingDto> GetErrorLogById(int id);

        /// <summary>
        ///     Deletes an errorlog
        /// </summary>
        /// <param name="id"></param>
        /// <returns>The deleted ErrorLoggingDto</returns>
        Task<ErrorLoggingDto> DeleteErrorLog(int id);

        /// <summary>
        ///     This Logs an exception
        /// </summary>
        /// <param name="exception"></param>
        /// <returns>A logged exception</returns>
        Exception LogException(Exception exception);
    }
}
