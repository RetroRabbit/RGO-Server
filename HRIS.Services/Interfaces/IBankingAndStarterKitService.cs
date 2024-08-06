using HRIS.Models;

namespace HRIS.Services.Interfaces;

public interface IBankingAndStarterKitService
{
    /// <summary>
    ///     Check if employee exists
    /// </summary>
    /// <param name="employeeId"></param>
    /// <returns></returns>
    Task<bool> CheckEmployee(string email);

    /// <summary>
    ///     Get All Banking and Starter Kits
    /// </summary>
    /// <returns></returns>
    Task<List<BankingAndStarterKitDto>> GetBankingAndStarterKitAsync();

    /// <summary>
    ///     Get Banking and Starter Kit by employee email
    /// </summary>
    /// <param name="employeeId"></param>
    /// <returns></returns>
    Task<List<BankingAndStarterKitDto>> GetBankingAndStarterKitByEmployeeAsync(string email);
}
