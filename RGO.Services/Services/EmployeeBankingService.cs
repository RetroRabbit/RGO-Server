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
        //await _db.EmployeeBanking.Update(entry);
        try
        {
            EmployeeDto empDto = await _db.Employee
            .Get(employee => employee.Id == newEntry.EmployeeId)
            .AsNoTracking()
            .Include(employee => employee.EmployeeType)
            .Select(employee => employee.ToDto())
            .FirstAsync();
            Employee newEmployee = new Employee(empDto, empDto.EmployeeType);

            EmployeeBanking entry = new EmployeeBanking(newEntry);

            entry.Employee = newEmployee;
        }catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
        }


        //Console.WriteLine(entry);
        return newEntry;
    }
}
