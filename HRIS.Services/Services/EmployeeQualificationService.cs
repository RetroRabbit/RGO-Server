using HRIS.Models;
using HRIS.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using RR.UnitOfWork;
using RR.UnitOfWork.Entities.HRIS;

namespace HRIS.Services.Services;

public class EmployeeQualificationService : IEmployeeQualificationService
{
    private readonly IUnitOfWork _db;

    public EmployeeQualificationService(IUnitOfWork db)
    {
        _db = db;
    }

    public async Task<EmployeeQualificationDto> SaveEmployeeQualification(EmployeeQualificationDto employeeQualificationDto)
    {
        var newEmployeeQualification = await _db.EmployeeQualification
            .Add(new EmployeeQualification(employeeQualificationDto));

        return newEmployeeQualification;
    }

    public async Task<EmployeeQualificationDto> DeleteEmployeeQualification(int id)
    {
        var existsingEmployeeQualification = await GetEmployeeQualification(id);

        var employeeQualificationDto = await _db.EmployeeQualification
            .Delete(existsingEmployeeQualification.Id);

        return employeeQualificationDto;
    }

    public async Task<List<EmployeeQualificationDto>> GetAllEmployeeQualifications()
    {
        return await _db.EmployeeQualification.GetAll();
    }

    public async Task<EmployeeQualificationDto> GetEmployeeQualification(int id)
    {
        var existingEmployeeQualification = await _db.EmployeeQualification
            .Get(employeeQualification => employeeQualification.Id == id)
            .Select(employeeQualification => employeeQualification.ToDto())
            .FirstAsync();

        return existingEmployeeQualification;
    }

    public async Task<EmployeeQualificationDto> UpdateEmployeeQualification(EmployeeQualificationDto employeeQualificationDto)
    {
        var existingEmployeeQualification = await GetEmployeeQualification(employeeQualificationDto.Id!);

        var updatedEmployeeQualification = await _db.EmployeeQualification
                .Update(new EmployeeQualification(existingEmployeeQualification));

        return updatedEmployeeQualification;
    }
}