using System;
using System.Collections.Generic;
using System.Linq;
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
        throw new NotImplementedException();
    }

    public async Task<EmployeeSalaryDetailsDto> GetAllEmployeeSalaries()
    {
        

        try
        {
            var employeeSalary = await _db.EmployeeSalaryDetails
                                           .Get(employeeSalary => employeeSalary.EmployeeId == employeeId)
                                           .AsNoTracking()
                                           .Include(employeeSalary => employeeSalary.Employee)
                                           .Select(employeeSalary => employeeSalary.ToDto())
                                           .ToListAsync();

            return employeeSalary;
        }
        catch (Exception)
        {
            var exception = new Exception("Employee banking details not found");
            throw _errorLoggingService.LogException(exception);
        }
        //throw new NotImplementedException();
    }

    public async Task<EmployeeSalaryDetailsDto> GetEmployeeSalary(int employeeId)
    {
        //var ifEmployee = await CheckEmployee(employeeCertificationDto.Employee!.Id);

        //if (!ifEmployee)
        //{
        //    var exception = new Exception("Employee not found");
        //    throw _errorLoggingService.LogException(exception);
        //}

        throw new NotImplementedException();
    }

    public async Task<EmployeeSalaryDetailsDto> SaveEmployeeSalary(EmployeeSalaryDetailsDto employeeSalaryDto)
    {
        throw new NotImplementedException();
    }

    public async Task<EmployeeSalaryDetailsDto> UpdateEmployeeSalary(EmployeeSalaryDetailsDto employeeSalaryDto)
    {
        throw new NotImplementedException();
    }
}
