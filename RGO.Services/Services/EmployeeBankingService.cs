using RGO.Models;
using RGO.Services.Interfaces;
using RGO.UnitOfWork;

namespace RGO.Services.Services;

public class EmployeeBankingService : IEmployeeBankingService
{
    private readonly IEmployeeBankingService _employeeBankingService;
    private readonly IUnitOfWork _db;

    public EmployeeBankingService(IEmployeeBankingService employeeBankingService, IUnitOfWork db)
    {
        _employeeBankingService = employeeBankingService;
        _db = db;
    }

    public async Task<List<EmployeeBankingDto>> GetPending()
    {
        /*
            Fetch all entries that have a status of pending
            return it
        */
        try
        {
            var pendingBankEntries = _db.EmployeeBanking
        }catch(Exception ex)
        {

        }
    }
}
