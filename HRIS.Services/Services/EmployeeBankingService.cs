using HRIS.Models;
using HRIS.Models.Enums;
using HRIS.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using RR.UnitOfWork;
using RR.UnitOfWork.Entities.HRIS;

namespace HRIS.Services.Services;

public class EmployeeBankingService : IEmployeeBankingService
{
    private readonly IUnitOfWork _db;
    private readonly IErrorLoggingService _errorLoggingService;

    public EmployeeBankingService(IUnitOfWork db, IErrorLoggingService errorLoggingService)
    {
        _db = db;
        _errorLoggingService = errorLoggingService;
    }

    public async Task<List<EmployeeBanking>> Get(int approvalStatus)
    {
        var pendingBankEntries = await _db.EmployeeBanking
                                          .Get(entry => entry.Status == (BankApprovalStatus)approvalStatus)
                                          .AsNoTracking()
                                          .Include(entry => entry.Employee)
                                          .Include(entry => entry.Employee!.EmployeeType)
                                          .Select(bankEntry => bankEntry)
                                          .ToListAsync();

        return pendingBankEntries;
    }

    public async Task<EmployeeBankingDto> Delete(int addressId)
    {
        var address = await _db.EmployeeBanking.Delete(addressId);
        return address;
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

        if (empDto.Email == userEmail)
        {
            bankingDto = CreateEmployeeBankingDto(newEntry, empBankingDto);
        }
        else
        {
            if (await IsAdmin(userEmail))
                bankingDto = CreateEmployeeBankingDto(newEntry, empBankingDto);
            else
            {
                var exception = new Exception("Unauthorized access");
                throw _errorLoggingService.LogException(exception);
            }
        }

        var existingBankingRecords = await _db.EmployeeBanking
            .Get(b => b.EmployeeId == newEntry.EmployeeId)
            .OrderBy(b => b.LastUpdateDate)
            .ToListAsync();

        if (existingBankingRecords.Count > 2)
        {
            var oldestRecord = existingBankingRecords.First();
            _db.EmployeeBanking.Delete(oldestRecord.Id);
        }

        var newBankingDetails = new EmployeeBanking
        {
            EmployeeId = newEntry.EmployeeId,
            BankName = newEntry.BankName,
            Branch = newEntry.Branch,
            AccountNo = newEntry.AccountNo,
            AccountType = newEntry.AccountType,
            AccountHolderName = newEntry.AccountHolderName,
            Status = newEntry.Status,
            DeclineReason = newEntry.DeclineReason,
            File = newEntry.File,
            LastUpdateDate = DateOnly.FromDateTime(DateTime.Now),
            PendingUpdateDate = newEntry.PendingUpdateDate
        };

        await _db.EmployeeBanking.Add(newBankingDetails); 

        return newEntry;
    }

    public async Task<List<EmployeeBankingDto>> GetBanking(int employeeId)
    {
        try
        {
            var employeeBanking = await _db.EmployeeBanking
                                           .Get(employeeBanking => employeeBanking.EmployeeId == employeeId)
                                           .AsNoTracking()
                                           .Include(employeeBanking => employeeBanking.Employee)
                                           .Select(employeeBanking => employeeBanking.ToDto())
                                           .ToListAsync();

            return employeeBanking;
        }
        catch (Exception)
        {
            var exception = new Exception("Employee banking details not found");
            throw _errorLoggingService.LogException(exception);
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

        EmployeeBankingDto? newEntryDto = null;

        if (employee.Email == userEmail)
        {
            newEntryDto = await _db.EmployeeBanking.Add(bankingDetails);
        }
        else
        {
            if (await IsAdmin(userEmail))
                newEntryDto = await _db.EmployeeBanking.Add(bankingDetails);
            else
            {
                var exception = new Exception("Unauthorized access");
                throw _errorLoggingService.LogException(exception);
            }

        }

        bankingDetails.Employee = employee;
        return newEntryDto;
    }

    private async Task<bool> IsAdmin(string userEmail)
    {
        var employeeDto = await _db.Employee
                                   .Get(emp => emp.Email == userEmail)
                                   .Include(emp => emp.EmployeeType)
                                   .Select(emp => emp.ToDto())
                                   .FirstOrDefaultAsync();

        var empRole = await _db.EmployeeRole
                               .Get(role => role.EmployeeId == employeeDto!.Id)
                               .FirstOrDefaultAsync();

        var role = await _db.Role
                            .Get(role => role.Id == empRole!.RoleId)
                            .FirstOrDefaultAsync();

        return role!.Description is "Admin" or "SuperAdmin";
    }

    private EmployeeBankingDto CreateEmployeeBankingDto(EmployeeBankingDto newEntry,
                                                                    EmployeeBankingDto empBankingDto)
    {
        var Bankingdto = new EmployeeBankingDto
        {
            Id = newEntry.Id,
            EmployeeId = newEntry.EmployeeId,
            BankName = newEntry.BankName,
            Branch = newEntry.Branch,
            AccountNo = newEntry.AccountNo,
            AccountType = newEntry.AccountType,
            AccountHolderName = newEntry.AccountHolderName,
            Status = newEntry.Status,
            DeclineReason = newEntry.DeclineReason,
            File = newEntry.File,
            LastUpdateDate = empBankingDto.LastUpdateDate,
            PendingUpdateDate = newEntry.PendingUpdateDate
        };
        return Bankingdto;
    }
}
