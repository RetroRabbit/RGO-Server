using HRIS.Models;
using RR.UnitOfWork.Entities.HRIS;

namespace HRIS.Services.Interfaces;

public interface IEmployeeBankingService
{
    /// <summary>
    ///     Fetches all banking entries by approval status
    /// </summary>
    /// <param name="approvalStatus"></param>
    /// <returns>List of pending EmployeeBanking objects</returns>
    Task<List<EmployeeBanking>> Get(int approvalStatus);

    /// <summary>
    ///     Delete Employee Banking
    /// </summary>
    /// <param name="employeeBankingDto"></param>
    /// <returns>Employee Banking</returns>
    Task<EmployeeBankingDto> Delete(int addressId);

    /// <summary>
    ///     Updates a banking entry
    /// </summary>
    /// <param name="newEntry"></param>
    /// <returns></returns>
    Task<EmployeeBankingDto> Update(EmployeeBankingDto newEntry, string userEmail);

    /// <summary>
    ///     Fetch banking of Employee
    /// </summary>
    /// <param id="employeeId"></param>
    /// <returns>EmployeeBankingDto</returns>
    Task<List<EmployeeBankingDto>> GetBanking(int id);

    /// <summary>
    ///     Save a new EmployeeBankingDto for an Employee
    /// </summary>
    /// <param name="newEntry"></param>
    /// <returns>EmployeeBankingDto</returns>
    Task<EmployeeBankingDto> Save(EmployeeBankingDto newEntry, string userEmail);
}