using RGO.Models;
using RGO.Models.Enums;
using RGO.Services.Interfaces;
using RGO.UnitOfWork;

namespace RGO.Services.Services;

public class EmployeeBankingService : IEmployeeBankingService
{
    private readonly IUnitOfWork _db;

    public EmployeeBankingService(IUnitOfWork db)
    {
        _db = db;
    }
    public async Task<List<EmployeeBankingDto>> GetPending()
    {
        try
        {
            var pendingBankEntries = _db.EmployeeBanking
                .Get(entry => entry.Status == BankApprovalStatus.Pending)
                .Select(bankEntry => bankEntry.ToDto())
                .ToList();
            return pendingBankEntries;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}
