using HRIS.Models.Employee.Commons;
using HRIS.Models.Enums;
using HRIS.Services.Interfaces;
using HRIS.Services.Session;
using Microsoft.EntityFrameworkCore;
using RR.UnitOfWork;
using RR.UnitOfWork.Entities.HRIS;

namespace HRIS.Services.Services;

public class EmployeeBankingService : IEmployeeBankingService
{
    private readonly IUnitOfWork _db;
    private readonly AuthorizeIdentity _identity;

    public EmployeeBankingService(IUnitOfWork db, AuthorizeIdentity identity)
    {
        _db = db;
        _identity = identity;
    }

    public async Task<List<EmployeeBanking>> Get(int approvalStatus)
    {
        if (_identity.IsAdmin == false)
            throw new CustomException("Unauthorized Access");

        var pendingBankEntries = await _db.EmployeeBanking
                                          .Get(entry => entry.Status == (BankApprovalStatus)approvalStatus)
                                          .AsNoTracking()
                                          .Include(entry => entry.Employee)
                                          .Include(entry => entry.Employee!.EmployeeType)
                                          .Select(bankEntry => bankEntry)
                                          .ToListAsync();

        return pendingBankEntries;
    }

    public async Task<EmployeeBankingDto> Delete(int id)
    {
        var exists = await EmployeeBankingDetailsExist(id);

        if (!exists)
            throw new CustomException("Employee Banking Not Found");

        var bankingDetails = await _db.EmployeeBanking.Delete(id);
        return bankingDetails.ToDto();
    }

    public async Task<EmployeeBankingDto> Update(EmployeeBankingDto newEntry)
    {
        var exists = await CheckEmployee(newEntry.EmployeeId);

        if (!exists)
            throw new CustomException("Employee Not Found");

        if (_identity.IsAdmin == false && _identity.EmployeeId != newEntry.EmployeeId)
            throw new CustomException("Unauthorized Access");

        var empDto = await _db.Employee
                              .Get(employee => employee.Id == newEntry.EmployeeId)
                              .AsNoTracking()
                              .Include(employee => employee.EmployeeType)
                              .Select(employee => employee.ToDto())
                              .FirstOrDefaultAsync();

        var empBankingDto = await _db.EmployeeBanking
                                         .Get(banking => banking.Id == newEntry.Id)
                                         .AsNoTracking()
                                         .Select(banking => banking.ToDto())
                                         .FirstOrDefaultAsync();

        var existingBankingRecords = await _db.EmployeeBanking
                .Get(b => b.EmployeeId == newEntry.EmployeeId)
                .OrderBy(b => b.LastUpdateDate)
                .ToListAsync();

        var oldestRecord = existingBankingRecords.First();
        await _db.EmployeeBanking.Delete(oldestRecord.Id);

        var newBankingDetails = new EmployeeBanking
        {
            EmployeeId = newEntry.EmployeeId,
            BankName = newEntry.BankName,
            Branch = newEntry.Branch,
            AccountNo = newEntry.AccountNo,
            AccountType = newEntry.AccountType,
            Status = newEntry.Status,
            DeclineReason = newEntry.DeclineReason,
            File = newEntry.File,
            LastUpdateDate = DateOnly.FromDateTime(DateTime.Now),
            PendingUpdateDate = newEntry.PendingUpdateDate
        };

        await _db.EmployeeBanking.Add(newBankingDetails);
        return newBankingDetails.ToDto();

    }

    public async Task<List<EmployeeBankingDto>> GetBankingById(int id)
    {
        var exists = await CheckEmployee(id);

        if (!exists)
            throw new CustomException("Employee Not Found");

        if (_identity.IsAdmin == false && _identity.EmployeeId != id)
            throw new CustomException("Unauthorized Access");

        var employeeBanking = await _db.EmployeeBanking
                                       .Get(employeeBanking => employeeBanking.EmployeeId == id)
                                       .AsNoTracking()
                                       .Include(employeeBanking => employeeBanking.Employee)
                                       .Select(employeeBanking => employeeBanking.ToDto())
                                       .ToListAsync();

        return employeeBanking;
    }

    public async Task<EmployeeBankingDto> Create(EmployeeBankingDto newEntry)
    {
        var exists = await EmployeeBankingDetailsExist(newEntry.Id);

        if (exists)
            throw new CustomException("Employee Banking Already Exists");

        if (_identity.IsAdmin == false && _identity.EmployeeId != newEntry.EmployeeId)
            throw new CustomException("Unauthorized Access");

        var employee = await _db.Employee
                                .Get(employee => employee.Id == newEntry.EmployeeId)
                                .Include(newEntry => newEntry.EmployeeType)
                                .Select(employee => employee)
                                .FirstAsync();

        EmployeeBankingDto? newEntryDto;

        EmployeeBanking employeeBanking = new EmployeeBanking(newEntry);
        newEntryDto = (await _db.EmployeeBanking.Add(employeeBanking)).ToDto();

        employeeBanking.Employee = employee;
        return newEntryDto;
    }

    public async Task<bool> CheckEmployee(int employeeId)
    {
        return await _db.Employee.Any(x => x.Id == employeeId);
    }

    public async Task<bool> EmployeeBankingDetailsExist(int id)
    {
        return await _db.EmployeeBanking.Any(x => x.Id == id);
    }
}
