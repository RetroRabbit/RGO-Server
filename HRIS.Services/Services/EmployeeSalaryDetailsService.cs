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
        //var ifEmployee = await CheckEmployee(employeeCertificationDto.Employee!.Id);

        //if (!ifEmployee)
        //{
        //    var exception = new Exception("Employee not found");
        //    throw _errorLoggingService.LogException(exception);
        //}

        return await _db.EmployeeDocument
            .Get(EmployeeDocument => true)
            .AsNoTracking()
            .Include(entry => entry.Employee)
            .Include(entry => entry.Employee.EmployeeType)
            .OrderBy(EmployeeDocument => EmployeeDocument.EmployeeId)
            .Select(EmployeeDocument => new SimpleEmployeeDocumentGetAllDto
            {
                EmployeeDocumentDto = EmployeeDocument.ToDto(),
                Name = EmployeeDocument.Employee.Name,
                Surname = EmployeeDocument.Employee.Surname
            })
            .ToListAsync();
    }

    public async Task<EmployeeSalaryDetailsDto> GetEmployeeSalary(int employeeId)
    {
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
