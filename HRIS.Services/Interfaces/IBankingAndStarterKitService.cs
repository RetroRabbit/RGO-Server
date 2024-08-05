using HRIS.Models;

namespace HRIS.Services.Interfaces;

public interface IBankingAndStarterKitService
{
    Task<bool> CheckEmployee(int employeeId);
    Task<List<BankingAndStarterKitDto>> GetBankingAndStarterKitAsync();
    Task<List<BankingAndStarterKitDto>> GetBankingAndStarterKitByIdAsync(int employeeId);
}
