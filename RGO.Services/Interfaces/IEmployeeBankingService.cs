using RGO.Models;
using RGO.UnitOfWork.Entities;

namespace RGO.Services.Interfaces;

public interface IEmployeeBankingService
{
    /// <summary>
    /// Fetches all banking entries by approval status
    /// </summary>
   /// <param name="approvalStatus"></param>
    /// <returns>List of pending EmployeeBanking objects</returns>
    Task<List<EmployeeBanking>> Get(int approvalStatus);


    /// <summary>
    /// Updates a banking entry
    /// </summary>
    /// <param name="newEntry"></param>
    /// <returns></returns>
    Task<EmployeeBankingDto> Update(EmployeeBankingDto newEntry);

    /// <summary>
    /// Fetch banking of Employee
    /// </summary>
    /// <param id="employeeId"></param>
    /// <returns>EmployeeBankingDto</returns>
    Task<EmployeeBankingDto> GetBanking(int id);

    /// <summary>
    /// Save a new EmployeeBankingDto for an Employee
    /// </summary>
    /// <param name="newEntry"></param>
    /// <returns>EmployeeBankingDto</returns>
    Task<EmployeeBankingDto> Save(EmployeeBankingDto newEntry);
}
