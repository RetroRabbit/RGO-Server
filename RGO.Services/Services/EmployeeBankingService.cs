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
        var pendingBankEntries = await _db.EmployeeBanking
            .Get(entry => entry.Status == (BankApprovalStatus)approvalStatus)
            .AsNoTracking()
            .Include(entry => entry.Employee)
            .Include(entry => entry.Employee.EmployeeType)
            .Select(bankEntry => bankEntry)
            .ToListAsync();

        return pendingBankEntries;
    }


    public async Task<EmployeeBankingDto> Update(EmployeeBankingDto newEntry, string userEmail)
    {
        var empDto = await _db.Employee
            .Get(employee => employee.Id == newEntry.EmployeeId)
            .AsNoTracking()
            .Include(employee => employee.EmployeeType)
            .Select(employee => employee.ToDto())
            .FirstAsync();

        EmployeeBankingDto bankingDto;
        var empBankingDto = await _db.EmployeeBanking
            .Get(employee => employee.Id == newEntry.Id)
            .AsNoTracking()
            .Select(employee => employee.ToDto())
            .FirstAsync();

        if(empDto.Email ==  userEmail)
            employeeBankingDto = await CreateEmployeeBankingDto(newEntry, empBankingDto);
        else
        {
            if(await IsAdmin(userEmail))
                employeeBankingDto = await CreateEmployeeBankingDto(newEntry, empBankingDto);          
            else       
                throw new Exception("Unauthorized access");           
        }

        Employee newEmployee = new Employee(empDto, empDto.EmployeeType);
        EmployeeBanking entry = new EmployeeBanking(bankingDto);
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
            throw new Exception("Employee banking details not found");
        }
    }

    public async Task<EmployeeBankingDto> Save(EmployeeBankingDto newEntry, string userEmail)
    {
        EmployeeBanking bankingDetails;

        bankingDetails = new EmployeeBanking(newEntry);

        var employee = await _db.Employee
            .Get(employee => employee.Id == newEntry.EmployeeId)
            .Include(newEntry => newEntry.EmployeeType)
            .Select(employee => employee)
            .FirstAsync();

        EmployeeBankingDto newEntryDto = null;

        if (employee.Email == userEmail)
            newEntryDto = await _db.EmployeeBanking.Add(bankingDetails);
        else
        {
            if (await IsAdmin(userEmail))
                newEntryDto = await _db.EmployeeBanking.Add(bankingDetails);
            else
                throw new Exception("Unauthorized access");        
        }

        bankingDetails.Employee = employee;
        return newEntryDto;
    }

    private async Task<bool> IsAdmin(string userEmail)
    {
        EmployeeDto employeeDto = await _db.Employee
            .Get(emp => emp.Email == userEmail)
            .Include(emp => emp.EmployeeType)
            .Select(emp => emp.ToDto())
            .FirstOrDefaultAsync();

        EmployeeRole empRole = await _db.EmployeeRole
            .Get(role => role.EmployeeId == employeeDto.Id)
            .FirstOrDefaultAsync();

        Role role = await _db.Role
            .Get(role => role.Id == empRole.RoleId)
            .FirstOrDefaultAsync();

        return role.Description is "Admin" or "SuperAdmin";
    }

    private async Task<EmployeeBankingDto> CreateEmployeeBankingDto(EmployeeBankingDto newEntry, EmployeeBankingDto empBankingDto)
    {
        EmployeeBankingDto Bankingdto = new EmployeeBankingDto
        (
              newEntry.Id,
              newEntry.EmployeeId,
              newEntry.BankName,
              newEntry.Branch,
              newEntry.AccountNo,
              newEntry.AccountType,
              newEntry.AccountHolderName,
              newEntry.Status,
              newEntry.DeclineReason,
              newEntry.File,
              empBankingDto.LastUpdateDate,
              newEntry.PendingUpdateDate
              );
        return Bankingdto;
    }
}
