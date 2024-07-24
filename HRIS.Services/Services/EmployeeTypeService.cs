using Auth0.ManagementApi.Models;
using HRIS.Models;
using HRIS.Services.Interfaces;
using HRIS.Services.Session;
using Microsoft.EntityFrameworkCore;
using RR.UnitOfWork;
using RR.UnitOfWork.Entities.HRIS;

namespace HRIS.Services.Services;

public class EmployeeTypeService : IEmployeeTypeService
{
    private readonly IUnitOfWork _db;
    private readonly AuthorizeIdentity _identity;

    public EmployeeTypeService(IUnitOfWork db, AuthorizeIdentity identity)
    {
        _db = db;
        _identity = identity;
    }

    public async Task<bool> EmployeeTypeExists(int Id)
    {
        return await _db.EmployeeType.Any(x => x.Id == Id);
    }

    public async Task<EmployeeTypeDto> CreateEmployeeType(EmployeeTypeDto employeeTypeDto)
    {
        var employeeTypeExists = await EmployeeTypeExists(employeeTypeDto.Id);

        if (employeeTypeExists)
            throw new CustomException("Employee Type does exist");

        if (_identity.IsSupport == false && employeeTypeDto.Id != _identity.EmployeeId)
            throw new CustomException("Unauthorized Access.");

        var newEmployeeType = await _db.EmployeeType
                                       .Add(new EmployeeType(employeeTypeDto));

        return newEmployeeType.ToDto();
    }

    public async Task<EmployeeTypeDto> DeleteEmployeeType(int id)
    {
        var employeeTypeExists = await EmployeeTypeExists(Id);

        if (!employeeTypeExists)
            throw new CustomException("Employee Type does not exist");

        if (_identity.IsSupport == false && Id != _identity.EmployeeId)
            throw new CustomException("Unauthorized Access.");
        
        var deletedEmployeeType = await _db.EmployeeType
                                           .Delete(Id);
 
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
        var employeeTypeExists = await EmployeeTypeExists(employeeTypeDto.Id);

        if (!employeeTypeExists)
            throw new CustomException("Employee Type does not exist");

        if (_identity.IsSupport == false && employeeTypeDto.Id != _identity.EmployeeId)
            throw new CustomException("Unauthorized Access.");

        var updatedEmployeeType = await _db.EmployeeType
                                           .Update(new EmployeeType(employeeTypeDto));

        return updatedEmployeeType.ToDto();
    }

    public async Task<List<EmployeeTypeDto>> GetEmployeeTypes()
    {
        return (await _db.EmployeeType.GetAll()).Select(x => x.ToDto()).ToList();
    }
}