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
    public async Task<List<PendingBankDto>> GetPending()
    {
        List<EmployeeBankingDto> pendingDtos = new List<EmployeeBankingDto>();

        var pendingBankEntries = await _db.EmployeeBanking
            .Get(entry => entry.Status == BankApprovalStatus.PendingApproval)
            .AsNoTracking()
            .Select(bankEntry => bankEntry.ToDto())
            .ToListAsync();
        List<PendingBankDto> pendingEntries = new List<PendingBankDto>();
        foreach (var entry in pendingBankEntries)
        {
            var emp = await _db.Employee
            .Get(employee => employee.Id == entry.EmployeeId)
            .AsNoTracking()
            .Include(employee => employee.EmployeeType)
            .Select(employee => employee.ToDto())
            .FirstOrDefaultAsync();

            if (emp != null)
            {
                PendingBankDto newEntry = new PendingBankDto(
                    entry.Id,
                    entry.EmployeeId,
                    entry.BankName,
                    entry.Branch,
                    entry.AccountNo,
                    entry.AccountType,
                    entry.AccountHolderName,
                    entry.Status,
                    entry.Reason,
                    entry.File,
                    emp);

                pendingEntries.Add(newEntry);
            }
        }
        return pendingEntries;
    }


    public async Task<EmployeeBankingDto> UpdatePending(EmployeeBankingDto newEntry)
    {
        var empDto = await _db.Employee
            .Get(employee => employee.Id == newEntry.EmployeeId)
            .AsNoTracking()
            .Include(employee => employee.EmployeeType)
            .Select(employee => employee.ToDto())
            .FirstAsync();

        Employee newEmployee = new Employee(empDto, empDto.EmployeeType);
        EmployeeBanking entry = new EmployeeBanking(newEntry);
        entry.Employee = newEmployee;

        await _db.EmployeeBanking.Update(entry);

        return newEntry;
    }
}
