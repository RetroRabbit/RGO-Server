using HRIS.Models;
using HRIS.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using RR.UnitOfWork;

namespace HRIS.Services.Services;

public class BankingAndStarterKitService : IBankingAndStarterKitService
{
    private readonly IUnitOfWork _db;

    public BankingAndStarterKitService(IUnitOfWork db)
    {
        _db = db;
    }

    public async Task<List<BankingAndStarterKitDto>> GetBankingAndStarterKitAsync()
    {
        var employeeDocumentQuery = await _db.EmployeeDocument
            .Get()
            .Include(doc => doc.Employee)
            .ThenInclude(emp => emp!.EmployeeType)
            .OrderBy(doc => doc.EmployeeId)
            .AsNoTracking()
            .ToListAsync();

        var mostRecentEmployeeBankings = await _db.EmployeeBanking
            .Get(banking => true)
            .Include(banking => banking.Employee)
                .ThenInclude(employee => employee!.EmployeeType)
            .GroupBy(banking => banking.EmployeeId)
            .Select(group => group.OrderByDescending(banking => banking).FirstOrDefault())
            .ToListAsync();

        var combinedResults = new List<BankingAndStarterKitDto>();
        combinedResults.AddRange(employeeDocumentQuery.Select(document => new BankingAndStarterKitDto
        {
            EmployeeDocumentDto = document.ToDto(),
            Name = document.Employee?.Name,
            Surname = document.Employee!.Surname,
            EmployeeId = document.EmployeeId
        }));

        combinedResults.AddRange(mostRecentEmployeeBankings.Select(banking => new BankingAndStarterKitDto
        {
            EmployeeBankingDto = banking!.ToDto(),
            Name = banking.Employee?.Name,
            Surname = banking?.Employee?.Surname,
            EmployeeId = banking!.EmployeeId
        }));

        return combinedResults;
    }

    public async Task<List<BankingAndStarterKitDto>> GetBankingAndStarterKitByIdAsync(int employeeId)
    {
        var employeeDocumentQuery = await _db.EmployeeDocument
            .Get(ed => ed.EmployeeId == employeeId)
            .Include(doc => doc.Employee)
            .ThenInclude(emp => emp!.EmployeeType)
            .OrderBy(doc => doc.EmployeeId)
            .AsNoTracking()
            .ToListAsync();

        var mostRecentEmployeeBankings = await _db.EmployeeBanking
            .Get(banking => true && banking.EmployeeId == employeeId)
            .Include(banking => banking.Employee)
                .ThenInclude(employee => employee!.EmployeeType)
            .GroupBy(banking => banking.EmployeeId)
            .Select(group => group.OrderByDescending(banking => banking).FirstOrDefault())
            .ToListAsync();

        var combinedResults = new List<BankingAndStarterKitDto>();
        combinedResults.AddRange(employeeDocumentQuery.Select(document => new BankingAndStarterKitDto
        {
            EmployeeDocumentDto = document.ToDto(),
            Name = document.Employee?.Name,
            Surname = document.Employee!.Surname,
            EmployeeId = document.EmployeeId
        }));

        combinedResults.AddRange(mostRecentEmployeeBankings.Select(banking => new BankingAndStarterKitDto
        {
            EmployeeBankingDto = banking.ToDto(),
            Name = banking.Employee?.Name,
            Surname = banking?.Employee?.Surname,
            EmployeeId = banking!.EmployeeId
        }));

        return combinedResults;
    }
}
