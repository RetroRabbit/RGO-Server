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
}
