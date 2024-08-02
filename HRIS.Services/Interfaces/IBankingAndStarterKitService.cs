using HRIS.Models;

namespace HRIS.Services.Interfaces;

public interface IBankingAndStarterKitService
{
    Task<List<BankingAndStarterKitDto>> GetBankingAndStarterKitAsync();
    Task<List<BankingAndStarterKitDto>> GetBankingAndStarterKitByIdAsync(int employeeId);
}
