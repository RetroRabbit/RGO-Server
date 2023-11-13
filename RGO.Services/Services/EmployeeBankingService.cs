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
    public async Task<List<EmployeeBanking>> Get(int approvalStatus)
    {
        var pendingBankEntries = new List<EmployeeBanking>();

        switch (approvalStatus)
        {
            case 0:
                {
                    pendingBankEntries = await _db.EmployeeBanking
                       .Get(entry => entry.Status == BankApprovalStatus.Approved)
                       .AsNoTracking()
                       .Include(entry => entry.Employee)
                       .Include(entry => entry.Employee.EmployeeType)
                       .Select(bankEntry => bankEntry)
                       .ToListAsync();
                    break;
                }

            case 1:
                {

                    pendingBankEntries = await _db.EmployeeBanking
                        .Get(entry => entry.Status == BankApprovalStatus.PendingApproval)
                        .AsNoTracking()
                        .Include(entry => entry.Employee)
                        .Include(entry => entry.Employee.EmployeeType)
                        .Select(bankEntry => bankEntry)
                        .ToListAsync();
                    break;
                }
            case 2:
                {
                    pendingBankEntries = await _db.EmployeeBanking
                        .Get(entry => entry.Status == BankApprovalStatus.Declined)
                        .AsNoTracking()
                        .Include(entry => entry.Employee)
                        .Include(entry => entry.Employee.EmployeeType)
                        .Select(bankEntry => bankEntry)
                        .ToListAsync();
                    break;
                }
        }
        return pendingBankEntries;
    }


    public async Task<EmployeeBankingDto> Update(EmployeeBankingDto newEntry)
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

    public async Task<EmployeeBankingDto> GetBanking(int employeeId)
    {
        try
        {
            var employeeBanking = await _db.EmployeeBanking
            .Get(employeeBanking => employeeBanking.EmployeeId == employeeId)
            .AsNoTracking()
            .Include(employeeBanking => employeeBanking.Employee)
            .Select(employeeBanking => employeeBanking.ToDto())
            .FirstOrDefaultAsync();

            return employeeBanking;
        }
        catch (Exception ex)
        {
            throw new Exception("Employee certification record not found");
        }
    }

    public async Task<EmployeeBankingDto> Save(EmployeeBankingDto newEntry)
    {
        EmployeeBanking bankingDetails;

        bankingDetails = new EmployeeBanking(newEntry);

        var employee = await _db.Employee
            .Get(employee => employee.Id == newEntry.EmployeeId)
            .Include(newEntry => newEntry.EmployeeType)
            .Select(employee => employee)
            .FirstAsync();

        bankingDetails.Employee = employee;

        EmployeeBankingDto newEntryDto = await _db.EmployeeBanking.Add(bankingDetails);

        return newEntryDto;
    }
}
