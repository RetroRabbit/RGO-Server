using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

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

    public async Task<EmployeeSalaryDetailsDto> DeleteEmployeeSalary(EmployeeSalaryDetailsDto employeeSalaryDto)
    {
        var employeeSalary = new EmployeeSalaryDetails(employeeSalaryDto);
        var deletedEmployeeSalary = await _db.EmployeeSalaryDetails.Delete(employeeSalary.Id);

        return deletedEmployeeSalary;
    }

    public async Task<EmployeeSalaryDetailsDto> GetAllEmployeeSalaries()
    {
        try
        {
            var employeeSalaries = await _db.EmployeeSalaryDetails
                                            .Get(employeeSalary => true)
                                            .AsNoTracking()
                                            //.Include(employeeSalary => employeeSalary.Employee)
                                            .OrderBy(employeeSalary => employeeSalary.EmployeeId)
                                            .Select(employeeSalary => employeeSalary.ToDto())
                                            .ToListAsync();

            return employeeSalaries.FirstOrDefault();
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
            var employeeSalaries = await _db.EmployeeSalaryDetails
                                            .Get(employeeSalary => employeeSalary.EmployeeId == employeeId)
                                            .AsNoTracking()
                                            //.Include(employeeSalary => employeeSalary.Employee)
                                            .OrderBy(employeeSalary => employeeSalary.EmployeeId)
                                            .Select(employeeSalary => employeeSalary.ToDto())
                                            .ToListAsync();

            return employeeSalaries.FirstOrDefault();
        }
        catch (Exception)
        {
            var exception = new Exception("Employee salary details not found");
            throw _errorLoggingService.LogException(exception);
        }
    }

    public async Task<EmployeeSalaryDetailsDto> SaveEmployeeSalary(EmployeeSalaryDetailsDto employeeSalaryDto)
    {
        throw new NotImplementedException();
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
        //var updatedEmployeeSalary = await _db.EmployeeSalaryDetails.Update(employeeSalary.Id);
        var updatedEmployeeSalary = await _db.EmployeeSalaryDetails.Update(employeeSalary);

        return updatedEmployeeSalary;
    }

    public async Task<bool> CheckEmployee(int employeeId)
    {
        var employee = await _db.Employee
        .Get(employee => employee.Id == employeeId)
        .FirstOrDefaultAsync();

        if (employee == null) { return false; }
        else { return true; }
    }
}
