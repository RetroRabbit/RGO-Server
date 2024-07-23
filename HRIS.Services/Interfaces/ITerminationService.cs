using HRIS.Models;

namespace HRIS.Services.Interfaces;

public interface ITerminationService
{
    /// <summary>
    ///     Check if user exist
    /// </summary>
    /// <param name="email"></param>
    /// <returns>Boolean to check if the Termination exists</returns>
    Task<bool> CheckTerminationExist(int Id);

    /// <summary>
    ///     Save a new Termination
    /// </summary>
    /// <param name="terminationDto">TerminationDto</param>
    /// <returns>TerminationDto</returns>
    Task<TerminationDto> CreateTermination(TerminationDto terminationDto);

    /// <summary>
    ///     Get Termination by employeeid
    /// </summary>
    /// <param name="employeeId">employeeId</param>
    /// <returns>TerminationDto</returns>
    Task<TerminationDto> GetTerminationByEmployeeId(int employeeId);
    /// <summary>
    ///     Update an existing termination.
    /// </summary>
    /// <param name="terminationDto">The Termination DTO.</param>
    /// <returns>The updated Termination DTO.</returns>
    Task<TerminationDto> UpdateTermination(TerminationDto terminationDto);
}
