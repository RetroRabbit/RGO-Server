using HRIS.Models;

namespace HRIS.Services.Interfaces;

public interface ITerminationService
{
    /// <summary>
    ///     Check if user exist
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    Task<bool> CheckTerminationExist(int Id);

    /// <summary>
    ///     Save a new Termination
    /// </summary>
    /// <param name="terminationDto">TerminationDto</param>
    /// <returns>TerminationDto</returns>
    Task<TerminationDto> SaveTermination(TerminationDto terminationDto);

    /// <summary>
    ///     Get Termination by employeeid
    /// </summary>
    /// <param name="employeeId">employeeId</param>
    /// <returns>TerminationDto</returns>
    Task<TerminationDto> GetTerminationByEmployeeId(int employeeId);

}
