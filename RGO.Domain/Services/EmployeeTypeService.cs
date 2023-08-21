using Microsoft.EntityFrameworkCore;
using RGO.Models;
using RGO.Services.Interfaces;
using RGO.UnitOfWork.Entities;
using RGO.UnitOfWork.Interfaces;

namespace RGO.Services.Services;

public class EmployeeTypeService : IEmployeeTypeService
{
    private readonly IEmployeeTypeRepository _employeeTypeRepository;

    public EmployeeTypeService(IEmployeeTypeRepository employeeTypeRepository)
    {
        _employeeTypeRepository = employeeTypeRepository;
    }

    public async Task<EmployeeTypeDto> AddEmployeeType(EmployeeTypeDto employeeTypeDto)
    {
        EmployeeTypeDto newEmployeeType = await _employeeTypeRepository
            .Add(new EmployeeType(employeeTypeDto));

        return newEmployeeType;
    }

    public async Task<EmployeeTypeDto> DeleteEmployeeType(string name)
    {
        EmployeeTypeDto existingEmployeeType = await GetEmployeeType(name);

        EmployeeTypeDto deletedEmployeeType = await _employeeTypeRepository
            .Delete(existingEmployeeType.Id);

        return deletedEmployeeType;
    }

    public async Task<List<EmployeeTypeDto>> GetAllEmployeeType()
    {
        return await _employeeTypeRepository
            .GetAll();
    }

    public async Task<EmployeeTypeDto> GetEmployeeType(string name)
    {
        EmployeeTypeDto existingEmployeeType = await _employeeTypeRepository
            .Get(employeeType => employeeType.Name == name)
            .Select(employeeType => employeeType.ToDto())
            .FirstAsync();

        return existingEmployeeType;
    }

    public async Task<EmployeeTypeDto> UpdateEmployeeType(EmployeeTypeDto employeeTypeDto)
    {
        EmployeeTypeDto updatedEmployeeType = await _employeeTypeRepository
            .Update(new EmployeeType(employeeTypeDto));

        return updatedEmployeeType;
    }
}
