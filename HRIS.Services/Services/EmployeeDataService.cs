using HRIS.Models;
using HRIS.Services.Interfaces;
using RR.UnitOfWork;
using RR.UnitOfWork.Entities.HRIS;

namespace HRIS.Services.Services;

public class EmployeeDataService : IEmployeeDataService
{
    private readonly IUnitOfWork _db;
    private readonly IErrorLoggingService _errorLoggingService;

    public EmployeeDataService(IUnitOfWork db, IErrorLoggingService errorLoggingService)
    {
        _db = db;
        _errorLoggingService = errorLoggingService;
    }

    public async Task<EmployeeDataDto> SaveEmployeeData(EmployeeDataDto employeeDataDto)
    {
        var employeesData = await _db.EmployeeData.GetAll();
        var employeeData = employeesData
                           .Where(employeeData => employeeData.EmployeeId == employeeDataDto.EmployeeId &&
                                                  employeeData.FieldCodeId == employeeDataDto.FieldCodeId)
                           .Select(employeeData => employeeData)
                           .FirstOrDefault();

        if (employeeData != null)
        {
            var exception = new Exception("Existing employee data record found");
            throw _errorLoggingService.LogException(exception);
        }
        var newEmployeeData = await _db.EmployeeData.Add(new EmployeeData(employeeDataDto));

        return newEmployeeData;
    }

    public async Task<EmployeeDataDto> GetEmployeeData(int employeeId, string value)
    {
        var employeesData = await _db.EmployeeData.GetAll();
        var employeeData = employeesData
                           .Where(employeeData => employeeData.EmployeeId == employeeId && employeeData.Value == value)
                           .Select(employeeData => employeeData)
                           .FirstOrDefault();
        if (employeeData == null)
        {
            var exception = new Exception("No employee data record found");
            throw _errorLoggingService.LogException(exception);
        }
        return employeeData;
    }

    public async Task<List<EmployeeDataDto>?> GetAllEmployeeData(int employeeId)
    {
        var employeesData = await _db.EmployeeData.GetAll();
        var employeeData = employeesData
                           .Where(employeeData => employeeData.EmployeeId == employeeId)
                           .ToList();
        return employeeData;
    }

    public async Task<EmployeeDataDto> UpdateEmployeeData(EmployeeDataDto employeeDataDto)
    {
        var employeesData = await _db.EmployeeData.GetAll();
        var employeeData = employeesData
                           .Where(employeeData => employeeData.Id == employeeDataDto.Id)
                           .Select(employeeData => employeeData)
                           .FirstOrDefault();

        if (employeeData == null)
        {
            var exception = new Exception("No employee data record found");
            throw _errorLoggingService.LogException(exception);
        }
        var updatedEmployeeData = await _db.EmployeeData.Update(new EmployeeData(employeeDataDto));

        return updatedEmployeeData;
    }

    public async Task<EmployeeDataDto> DeleteEmployeeData(int employeeDataId)
    {
        var deletedData = await _db.EmployeeData.Delete(employeeDataId);
        return deletedData;
    }
}