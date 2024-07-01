using HRIS.Models;
using HRIS.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using RR.UnitOfWork;
using RR.UnitOfWork.Entities.HRIS;

namespace HRIS.Services.Services;

public class EmployeeTypeService : IEmployeeTypeService
{
    private readonly IUnitOfWork _db;

    public EmployeeTypeService(IUnitOfWork db)
    {
        _db = db;
    }

    public async Task<EmployeeTypeDto> SaveEmployeeType(EmployeeTypeDto employeeTypeDto)
    {
        var newEmployeeType = await _db.EmployeeType
                                       .Add(new EmployeeType(employeeTypeDto));

        return newEmployeeType.ToDto();
    }

    public async Task<EmployeeTypeDto> DeleteEmployeeType(string name)
    {
        var existingEmployeeType = await GetEmployeeType(name);

        var deletedEmployeeType = await _db.EmployeeType
                                           .Delete(existingEmployeeType.Id);

        return deletedEmployeeType.ToDto();
    }

    public async Task<List<EmployeeTypeDto>> GetAllEmployeeType()
    {
        return (await _db.EmployeeType.GetAll()).Select(x => x.ToDto()).ToList();
    }

    public async Task<EmployeeTypeDto> GetEmployeeType(string name)
    {
        var existingEmployeeType = await _db.EmployeeType
                                            .Get(employeeType => employeeType.Name == name)
                                            .Select(employeeType => employeeType.ToDto())
                                            .FirstAsync();

        return existingEmployeeType;
    }

    public async Task<EmployeeTypeDto> UpdateEmployeeType(EmployeeTypeDto employeeTypeDto)
    {
        var updatedEmployeeType = await _db.EmployeeType
                                           .Update(new EmployeeType(employeeTypeDto));

        return updatedEmployeeType.ToDto();
    }

    public async Task<List<EmployeeTypeDto>> GetEmployeeTypes()
    {
        return (await _db.EmployeeType.GetAll()).Select(x => x.ToDto()).ToList();
    }
}