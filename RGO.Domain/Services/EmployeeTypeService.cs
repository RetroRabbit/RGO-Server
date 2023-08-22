using Microsoft.EntityFrameworkCore;
using RGO.Models;
using RGO.Services.Interfaces;
using RGO.UnitOfWork;
using RGO.UnitOfWork.Entities;

namespace RGO.Services.Services;

public class EmployeeTypeService : IEmployeeTypeService
{
    private readonly IUnitOfWork _db;

    public EmployeeTypeService(IUnitOfWork db)
    {
        _db = db;
    }

    public async Task<EmployeeTypeDto> AddEmployeeType(EmployeeTypeDto employeeTypeDto)
    {
        EmployeeTypeDto newEmployeeType = await _db.EmployeeType
            .Add(new EmployeeType(employeeTypeDto));

        return newEmployeeType;
    }

    public async Task<EmployeeTypeDto> DeleteEmployeeType(string name)
    {
        EmployeeTypeDto existingEmployeeType = await GetEmployeeType(name);

        EmployeeTypeDto deletedEmployeeType = await _db.EmployeeType
            .Delete(existingEmployeeType.Id);

        return deletedEmployeeType;
    }

    public async Task<List<EmployeeTypeDto>> GetAllEmployeeType()
    {
        return await _db.EmployeeType
            .GetAll();
    }

    public async Task<EmployeeTypeDto> GetEmployeeType(string name)
    {
        EmployeeTypeDto existingEmployeeType = await _db.EmployeeType
            .Get(employeeType => employeeType.Name == name)
            .Select(employeeType => employeeType.ToDto())
            .FirstAsync();

        return existingEmployeeType;
    }

    public async Task<EmployeeTypeDto> UpdateEmployeeType(EmployeeTypeDto employeeTypeDto)
    {
        EmployeeTypeDto updatedEmployeeType = await _db.EmployeeType
            .Update(new EmployeeType(employeeTypeDto));

        return updatedEmployeeType;
    }
}
