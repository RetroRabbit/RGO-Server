using HRIS.Models;
using HRIS.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using RR.UnitOfWork;
using RR.UnitOfWork.Entities.HRIS;

namespace HRIS.Services.Services;

public class EmployeeSalaryDetailsService : IEmployeeSalarayDetailsService
{
    private readonly IUnitOfWork _db;
    private readonly IErrorLoggingService _errorLoggingService;

    public EmployeeSalaryDetailsService(IUnitOfWork db, IErrorLoggingService errorLoggingService)
    {
        _db = db;
        _errorLoggingService = errorLoggingService;
    }

    public async Task<EmployeeSalaryDetailsDto> DeleteEmployeeSalary(int employeeId)
    {
        var deletedEmployeeSalary = await _db.EmployeeSalaryDetails.Delete(employeeId);

        return deletedEmployeeSalary.ToDto();
    }
    
    public async Task<List<EmployeeSalaryDetailsDto>> GetAllEmployeeSalaries()
    {
        try
        {
            return (await _db.EmployeeSalaryDetails.GetAll()).Select(x => x.ToDto()).ToList();
        }
        catch (Exception)
        {
            var exception = new Exception("Employee salary details not found");
            throw _errorLoggingService.LogException(exception);
        }
    }

    public async Task<EmployeeSalaryDetailsDto> GetEmployeeSalary(int employeeId)
    {
        var ifEmployeeExists = await CheckEmployee(employeeId);

        if (!ifEmployeeExists)
        {
            var exception = new Exception("Employee not found");
            throw _errorLoggingService.LogException(exception);
        }
        try
        {
            return await _db.EmployeeSalaryDetails
                                            .Get(employeeSalary => employeeSalary.EmployeeId == employeeId)
                                            .AsNoTracking()
                                            .Select(employeeSalary => employeeSalary.ToDto())
                                            .FirstOrDefaultAsync();
        }
        catch (Exception)
        {
            var exception = new Exception("Employee salary details not found");
            throw _errorLoggingService.LogException(exception);
        }
    }

    public async Task<EmployeeSalaryDetailsDto> SaveEmployeeSalary(EmployeeSalaryDetailsDto employeeSalaryDto)
    {
        var exists = await CheckIfExists(employeeSalaryDto);

        if (exists)
        {
            var exception = new Exception("Employee salary already exists");
            throw _errorLoggingService.LogException(exception);
        }

        var employeeSalary = await _db.EmployeeSalaryDetails.Add(new EmployeeSalaryDetails(employeeSalaryDto));

        return employeeSalary.ToDto();
    }

    public async Task<EmployeeSalaryDetailsDto> UpdateEmployeeSalary(EmployeeSalaryDetailsDto employeeSalaryDto)
    {
        var ifEmployeeExists = await CheckEmployee(employeeSalaryDto.EmployeeId);

        if (!ifEmployeeExists)
        {
            var exception = new Exception("Employee not found");
            throw _errorLoggingService.LogException(exception);
        }

        EmployeeSalaryDetails employeeSalary = new EmployeeSalaryDetails(employeeSalaryDto);
        var updatedEmployeeSalary = await _db.EmployeeSalaryDetails.Update(employeeSalary);

        return updatedEmployeeSalary.ToDto();
    }


    public async Task<bool> CheckEmployee(int employeeId)
    {
        var employee = await _db.Employee
        .Get(employee => employee.Id == employeeId)
        .FirstOrDefaultAsync();

        if (employee == null) { return false; }
        else { return true; }
    }

    public async Task<bool> CheckIfExists(EmployeeSalaryDetailsDto employeeSalaryDetailsDto)
    {
        if (employeeSalaryDetailsDto.Id == 0)
        {
            return false;
        }
        var exists = await _db.EmployeeSalaryDetails.GetById(employeeSalaryDetailsDto.Id);
        return exists == null;
    }
}