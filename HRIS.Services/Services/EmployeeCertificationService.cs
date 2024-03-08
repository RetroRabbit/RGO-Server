using HRIS.Models;
using HRIS.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using RR.UnitOfWork;
using RR.UnitOfWork.Entities.HRIS;

namespace HRIS.Services.Services;

public class EmployeeCertificationService : IEmployeeCertificationService
{
    private readonly IUnitOfWork _db;
    private readonly IErrorLoggingService _errorLoggingService;

    public EmployeeCertificationService(IUnitOfWork db, IErrorLoggingService errorLoggingService)
    {
        _db = db;
        _errorLoggingService = errorLoggingService;
    }

    public async Task<EmployeeCertificationDto> SaveEmployeeCertification(
        EmployeeCertificationDto employeeCertificationDto)
    {
        var ifEmployee = await CheckEmployee(employeeCertificationDto.Employee!.Id);

        if (!ifEmployee) 
        { 
            var exception = new Exception("Employee not found");
            throw _errorLoggingService.LogException(exception);
        } 

        var employeeCertification = new EmployeeCertification(employeeCertificationDto);
        var existingCertification = await _db.EmployeeCertification
                                             .Get(employeeCertification =>
                                                      employeeCertification.EmployeeId ==
                                                      employeeCertificationDto.Employee.Id)
                                             .AsNoTracking()
                                             .Include(employeeCertification => employeeCertification.Employee)
                                             .Select(employeeCertification => employeeCertification.ToDto())
                                             .Take(1)
                                             .FirstOrDefaultAsync();

        if (existingCertification != null)
        {
            var exception = new Exception("Existing employee certification record found");
            throw _errorLoggingService.LogException(exception);
        }
        var newEmployeeCertification = await _db.EmployeeCertification.Add(employeeCertification);

        return newEmployeeCertification;
    }

    public async Task<EmployeeCertificationDto> GetEmployeeCertification(int employeeId, int certificationId)
    {
        var ifEmployee = await CheckEmployee(employeeId);

        if (!ifEmployee)
        {
            var exception = new Exception("Employee not found");
            throw _errorLoggingService.LogException(exception);
        }

        var employeeCertification = await _db.EmployeeCertification
                                             .Get(employeeCertification =>
                                                      employeeCertification.EmployeeId == employeeId &&
                                                      employeeCertification.Id == certificationId)
                                             .AsNoTracking()
                                             .Include(employeeCertification => employeeCertification.Employee)
                                             .Select(employeeCertification => employeeCertification.ToDto())
                                             .Take(1)
                                             .FirstOrDefaultAsync();

        if (employeeCertification == null)
        {
            var exception = new Exception("Employee certification record not found");
            throw _errorLoggingService.LogException(exception);
        }
        return employeeCertification;
    }

    public async Task<List<EmployeeCertificationDto>> GetAllEmployeeCertifications(int employeeId)
    {
        var ifEmployee = await CheckEmployee(employeeId);

        if (!ifEmployee)
        {
            var exception = new Exception("Employee not found");
            throw _errorLoggingService.LogException(exception);
        }

        var employeeCertifications = await _db.EmployeeCertification
                                              .Get(employeeCertification =>
                                                       employeeCertification.EmployeeId == employeeId)
                                              .AsNoTracking()
                                              .Include(employeeCertification => employeeCertification.Employee)
                                              .Select(employeeCertification => employeeCertification.ToDto())
                                              .ToListAsync();

        if (employeeCertifications == null)
        {
            var exception = new Exception("Employee certification records not found");
            throw _errorLoggingService.LogException(exception);
        }

        return employeeCertifications;
    }

    public async Task<EmployeeCertificationDto> UpdateEmployeeCertification(
        EmployeeCertificationDto employeeCertificationDto)
    {
        var ifEmployee = await CheckEmployee(employeeCertificationDto.Employee!.Id);

        if (!ifEmployee)
        {
            var exception = new Exception("Employee not found");
            throw _errorLoggingService.LogException(exception);
        }
        var employeeCertification = new EmployeeCertification(employeeCertificationDto);
        var updatedEmployeeCertification = await _db.EmployeeCertification.Update(employeeCertification);

        return updatedEmployeeCertification;
    }

    public async Task<EmployeeCertificationDto> DeleteEmployeeCertification(
        EmployeeCertificationDto employeeCertificationDto)
    {
        var ifEmployee = await CheckEmployee(employeeCertificationDto.Employee!.Id);

        if (!ifEmployee)
        {
            var exception = new Exception("Employee not found");
            throw _errorLoggingService.LogException(exception);
        }

        var employeeCertification = new EmployeeCertification(employeeCertificationDto);
        var deletedEmployeeCertification = await _db.EmployeeCertification.Delete(employeeCertification.Id);

        return deletedEmployeeCertification;
    }

    private async Task<bool> CheckEmployee(int employeeId)
    {
        var employee = await _db.Employee
                                .Get(employee => employee.Id == employeeId)
                                .FirstOrDefaultAsync();

        if (employee == null)
            return false;
        return true;
    }
}