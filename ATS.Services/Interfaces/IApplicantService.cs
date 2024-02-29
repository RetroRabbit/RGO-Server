using ATS.Models;

namespace ATS.Services.Interfaces;

public interface IApplicantService
{
    /// <summary>
    ///     Check if the applicant exists in the system
    /// </summary>
    /// <param name="applicantEmail"></param>
    /// <returns>Bool depending on they exists or not</returns>
    Task<bool> CheckApplicantExists(string applicantEmail);

    /// <summary>
    ///     Takes in an applicant and adds it to the database
    /// </summary>
    /// <param name="applicantDto"></param>
    /// <returns>The new ApplicantDto</returns>
    Task<ApplicantDto> SaveApplicant(ApplicantDto applicantDto);

    /// <summary>
    ///     Returns a list of all applicants
    /// </summary>
    /// <returns>List<ApplicantDto</returns>
    Task<List<ApplicantDto>> GetAllApplicants();

    /// <summary>
    ///     Fetches a single applicant by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns>Single ApplicantDto</returns>
    Task<ApplicantDto> GetApplicantById(int id);

    /// <summary>
    ///     Fetches a single applicant by email
    /// </summary>
    /// <param name="email"></param>
    /// <returns>Single ApplicantDto</returns>
    Task<ApplicantDto> GetApplicantByEmail(string email);

    /// <summary>
    ///     Updates an applicant
    /// </summary>
    /// <param name="applicantDto"></param>
    /// <returns>The updated applicantDto</returns>
    Task<ApplicantDto> UpdateApplicant(ApplicantDto applicantDto);

    /// <summary>
    ///     Deletes an applicant
    /// </summary>
    /// <param name="id"></param>
    /// <returns>The deleted applicantDto</returns>
    Task<ApplicantDto> DeleteApplicant(int id);
}
