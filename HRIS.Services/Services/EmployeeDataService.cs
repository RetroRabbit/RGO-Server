using HRIS.Models;
using HRIS.Services.Interfaces;
using HRIS.Services.Session;
using Microsoft.EntityFrameworkCore.Metadata;
using RR.UnitOfWork;
using RR.UnitOfWork.Entities.HRIS;

namespace HRIS.Services.Services;

public class EmployeeDataService : IEmployeeDataService
{
    private readonly IUnitOfWork _db;
    private readonly AuthorizeIdentity _identity;

    public EmployeeDataService(IUnitOfWork db, AuthorizeIdentity identity)
    {
        _db = db;
        _identity = identity;
    }

    public async Task<bool> EmployeeDataExists(int id)
    { 
        return await _db.EmployeeData.Any(x => x.Id == id);
    }

    public async Task<EmployeeDataDto> SaveEmployeeData(EmployeeDataDto employeeDataDto)
    {
        var modelExists = await EmployeeDataExists(employeeDataDto.Id);

        if (!modelExists) throw new CustomException("This model does not exist yet");

        if (_identity.IsSupport == false)
            throw new CustomException("Unauthorized Access.");

        var employeesData = await _db.EmployeeData.GetAll();
        var employeeData = employeesData
                           .Where(employeeData => employeeData.EmployeeId == employeeDataDto.EmployeeId &&
                                                  employeeData.FieldCodeId == employeeDataDto.FieldCodeId)
                           .Select(employeeData => employeeData)
                           .FirstOrDefault();

        if (employeeData != null)
            throw new CustomException("Existing employee data record found");

        var newEmployeeData = await _db.EmployeeData.Add(new EmployeeData(employeeDataDto));

        return newEmployeeData.ToDto();
    }

    public async Task<EmployeeDataDto> GetEmployeeData(int employeeId, string value)
    {
        var modelExists = await EmployeeDataExists(employeeId);

        if (!modelExists) throw new CustomException("Employee data does not exist");

        if (_identity.IsSupport == false)
            throw new CustomException("Unauthorized Access.");

        var employeesData = await _db.EmployeeData.GetAll();
        var employeeData = employeesData
                           .Where(employeeData => employeeData.EmployeeId == employeeId && employeeData.Value == value)
                           .Select(employeeData => employeeData)
                           .FirstOrDefault();
        if (employeeData == null)
            throw new CustomException("No employee data record found");

        return employeeData.ToDto();
    }

    public async Task<List<EmployeeDataDto>?> GetAllEmployeeData(int employeeId)
    {
        var modelExists = await EmployeeDataExists(employeeId);

        if (!modelExists) throw new CustomException("No employee data exists");

        if (_identity.IsSupport == false)
            throw new CustomException("Unauthorized Access.");

        var employeesData = await _db.EmployeeData.GetAll();
        var employeeData = employeesData
                           .Where(employeeData => employeeData.EmployeeId == employeeId)
                           .ToList();
        return employeeData.Select(x => x.ToDto()).ToList();
    }

    public async Task<EmployeeDataDto> UpdateEmployeeData(EmployeeDataDto employeeDataDto)
    {
        var employeesData = await _db.EmployeeData.GetAll();
        var employeeData = employeesData
                           .Where(employeeData => employeeData.Id == employeeDataDto.Id)
                           .Select(employeeData => employeeData)
                           .FirstOrDefault();

        if (employeeData == null)
            throw new CustomException("No employee data record found");

        var updatedEmployeeData = await _db.EmployeeData.Update(new EmployeeData(employeeDataDto));

        return updatedEmployeeData.ToDto();
    }

    public async Task<EmployeeDataDto> DeleteEmployeeData(int employeeDataId)
    {
        var deletedData = await _db.EmployeeData.Delete(employeeDataId);
        return deletedData.ToDto();
    }
}