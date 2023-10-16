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
    public async Task<List<EmployeeBanking>> GetPending()
    {
         var pendingBankEntries = await _db.EmployeeBanking
                .Get(entry => entry.Status == BankApprovalStatus.PendingApproval)
                .AsNoTracking()
                .Include(entry => entry.Employee)
                .Include(entry => entry.Employee.EmployeeType)
                .Select(bankEntry => bankEntry)
                .ToListAsync();
            return pendingBankEntries;
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
