using Microsoft.EntityFrameworkCore;
using RGO.Models;
using RGO.Models.Enums;
using RGO.Services.Interfaces;
using RGO.UnitOfWork;
using RGO.UnitOfWork.Entities;

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
        List<EmployeeBankingDto> pendingDtos = new List<EmployeeBankingDto>();
        try
        {
            var pendingBankEntries = await _db.EmployeeBanking
                .Get(entry => entry.Status == BankApprovalStatus.PendingApproval)
                .AsNoTracking()
                .Include(entry => entry.Employee)
                .Include(entry => entry.Employee.EmployeeType)
                .Select(bankEntry => bankEntry.ToDto())
                .ToListAsync();
            return pendingBankEntries;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }


    public async Task<EmployeeBankingDto> UpdatePending(EmployeeBankingDto newEntry)
    {
        EmployeeBanking entry = new EmployeeBanking(newEntry);
        EmployeeDto employ = await _db.Employee.Get(x => x.Id == entry.EmployeeId).Select(x => x.ToDto()).FirstOrDefaultAsync();

        if (employ == null)
        {
            throw new Exception();
        }
        entry.Employee = new Employee(employ, employ.EmployeeType);
        await _db.EmployeeBanking.Update(entry);
        
        return newEntry;
    }
}
