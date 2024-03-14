using HRIS.Models;
using HRIS.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using RR.UnitOfWork;
using RR.UnitOfWork.Entities.HRIS;


namespace HRIS.Services.Services;

public class BankingAndStarterKitService : IBankingAndStarterKitService
{
    private readonly IUnitOfWork _db;
    private readonly IEmployeeService _employeeService;
    private readonly IEmployeeBankingService _employeeBankingService;
    private readonly IErrorLoggingService _errorLoggingService;

    public BankingAndStarterKitService(IUnitOfWork db, IEmployeeService employeeService, IEmployeeBankingService employeeBankingService ,IErrorLoggingService errorLoggingService)
    {
        _db = db;
        _employeeService = employeeService;
        _employeeBankingService = employeeBankingService;
        _errorLoggingService = errorLoggingService;
    }
    public async Task<List<BankingAndStarterKitDto>> GetBankingAndStarterKitAsync()
    {
        var employeeDocumentQuery = _db.EmployeeDocument
            .Get(EmployeeDocument => true)
            .AsNoTracking()
            .Include(entry => entry.Employee)
                .ThenInclude(employee => employee.EmployeeType)
            .OrderBy(EmployeeDocument => EmployeeDocument.EmployeeId);

        var employeeBankingQuery = _db.EmployeeBanking
             .Get(EmployeeBanking => true)
             .AsNoTracking()
             .Include(entry => entry.Employee)
                .ThenInclude(employee => employee.EmployeeType);

        var result = await employeeDocumentQuery.Join(employeeBankingQuery,
                                      document => document.EmployeeId,
                                      banking => banking.EmployeeId,
                                      (document, banking) => new BankingAndStarterKitDto
                                      {
                                          EmployeeDocumentDto = document.ToDto(),
                                          EmployeeBankingDto = banking.ToDto(),
                                          Name = document.Employee.Name,
                                          Surname = document.Employee.Surname
                                      })
                                 .ToListAsync();

        return result;
    }
}
