using ATS.Models;

namespace ATS.Services.Interfaces;

public interface ICandidateService
{
    /// <summary>
    ///     Check if the candidate exists in the system
    /// </summary>
    /// <param name="candidateEmail"></param>
    /// <returns>Bool depending on they exists or not</returns>
    Task<bool> CheckCandidateExists(string candidateEmail);

    /// <summary>
    ///     Takes in an candidate and adds it to the database
    /// </summary>
    /// <param name="candidateDto"></param>
    /// <returns>The new CandidateDto</returns>
    Task<CandidateDto> SaveCandidate(CandidateDto candidateDto);

    /// <summary>
    ///     Returns a list of all candidates
    /// </summary>
    /// <returns>List<CandidateDto</returns>
    Task<List<CandidateDto>> GetAllCandidates();

    /// <summary>
    ///     Fetches a single candidate by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns>Single CandidateDto</returns>
    Task<CandidateDto> GetCandidateById(int id);

    /// <summary>
    ///     Fetches a single candidate by email
    /// </summary>
    /// <param name="email"></param>
    /// <returns>Single CandidateDto</returns>
    Task<CandidateDto> GetCandidateByEmail(string email);

    /// <summary>
    ///     Updates an candidate
    /// </summary>
    /// <param name="candidateDto"></param>
    /// <returns>The updated candidateDto</returns>
    Task<CandidateDto> UpdateCandidate(CandidateDto candidateDto);

    /// <summary>
    ///     Deletes an candidate
    /// </summary>
    /// <param name="id"></param>
    /// <returns>The deleted candidateDto</returns>
    Task<CandidateDto> DeleteCandidate(int id);
}
