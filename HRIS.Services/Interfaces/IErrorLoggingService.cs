using ATS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRIS.Services.Interfaces
{
    public interface IErrorLoggingService
    {
        /// <summary>
        ///     Check if the applicant exists in the system
        /// </summary>
        /// <param name="message"></param>
        /// <returns>Bool depending on they exists or not</returns>
        Task<bool> CheckErrorLogExists(int Id);

        /// <summary>
        ///     Takes in an applicant and adds it to the database
        /// </summary>
        /// <param name="errorLoggingDto"></param>
        /// <returns>The new ApplicantDto</returns>
        Task<ErrorLoggingDto> SaveErrorLog(ErrorLoggingDto errorLoggingDto);

        /// <summary>
        ///     Returns a list of all applicants
        /// </summary>
        /// <returns>List<ApplicantDto</returns>
        Task<List<ErrorLoggingDto>> GetAllErrorLogs();

        /// <summary>
        ///     Fetches a single applicant by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Single ApplicantDto</returns>
        Task<ErrorLoggingDto> GetErrorLogById(int id);

        /// <summary>
        ///     Deletes an applicant
        /// </summary>
        /// <param name="id"></param>
        /// <returns>The deleted applicantDto</returns>
        Task<ErrorLoggingDto> DeleteErrorLog(int id);
    }
}
