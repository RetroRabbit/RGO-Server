using RGO.Models;

namespace RGO.Services.Interfaces;

public interface IEmployeeBankingService
{
    /// <summary>
    /// Fetches all pending banking entries
    /// </summary>
    /// <returns>List of pending EmployeeBanking objects</returns>
    Task<List<PendingBankDto>> GetPending();


    /// <summary>
    /// Updates a banking entry
    /// </summary>
    /// 
    /// <returns></returns>
    Task<EmployeeBankingDto> UpdatePending(EmployeeBankingDto newEntry);
}
