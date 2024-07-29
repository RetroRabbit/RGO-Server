using HRIS.Models;
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

    public EmployeeBankingService(AuthorizeIdentity identity, IUnitOfWork db)
    {
        _identity = identity;
        _db = db;
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
        var exists = await CheckIfExists(id);

        if (!exists)
            throw new CustomException("Employee Bankin Does Not Exist");

        if (_identity.IsAdmin == false && _identity.EmployeeId != id)
            throw new CustomException("Unauthorized Access");

        var bankingDetails = await _db.EmployeeBanking.Delete(id);
        return bankingDetails.ToDto();
    }

    public async Task<EmployeeBankingDto> Update(EmployeeBankingDto newEntry, string userEmail)
    {
        var exists = await CheckIfExists(newEntry.EmployeeId);

        if (exists)
            throw new CustomException("Employee Banking Does Not Exist");

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

        if (empDto?.Email == userEmail)
        {
            var bankingDto = CreateEmployeeBankingDto(newEntry, empBankingDto);
            var existingBankingRecords = await _db.EmployeeBanking
                .Get(b => b.EmployeeId == newEntry.EmployeeId)
                .OrderBy(b => b.LastUpdateDate)
                .ToListAsync();

            if (existingBankingRecords.Count > 1)
            {
                var oldestRecord = existingBankingRecords.First();
                await _db.EmployeeBanking.Delete(oldestRecord.Id);
            }

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

            return bankingDto;
        }

        throw new CustomException("Unauthorized access.");
    }

    public async Task<List<EmployeeBankingDto>> GetBanking(int id)
    {
        var exists = await CheckIfExists(id);

        if (!exists)
            throw new CustomException("Employee Banking Does Not Exist");

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

    public async Task<EmployeeBankingDto> Create(EmployeeBankingDto newEntry, string userEmail)
    {
        var exists = await CheckIfExists(newEntry.Id);

        if (exists)
            throw new CustomException("Employee Banking Record Already Exists");

        if (_identity.IsAdmin == false && _identity.EmployeeId != newEntry.EmployeeId)
            throw new CustomException("Unauthorized Access");

        var bankingDetails = new EmployeeBanking(newEntry);

        return bankingDetails.ToDto();
    }

    public async Task<bool> CheckIfExists(int id)
    {
        return await _db.EmployeeBanking.Any(x => x.Id == id);
    }

    private EmployeeBankingDto CreateEmployeeBankingDto(EmployeeBankingDto newEntry, EmployeeBankingDto empBankingDto)
    {
        var banking = new EmployeeBankingDto
        {
            Id = newEntry.Id,
            EmployeeId = newEntry.EmployeeId,
            BankName = newEntry.BankName,
            Branch = newEntry.Branch,
            AccountNo = newEntry.AccountNo,
            AccountType = newEntry.AccountType,
            Status = newEntry.Status,
            DeclineReason = newEntry.DeclineReason,
            File = newEntry.File,
            LastUpdateDate = empBankingDto.LastUpdateDate,
            PendingUpdateDate = newEntry.PendingUpdateDate
        };
        return banking;
    }
}
