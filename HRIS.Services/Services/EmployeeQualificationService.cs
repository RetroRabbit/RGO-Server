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
        _db = db ?? throw new ArgumentNullException(nameof(db));
    }

    public async Task<EmployeeQualificationDto> SaveEmployeeQualification(EmployeeQualificationDto employeeQualificationDto)
    {
        if (employeeQualificationDto == null)
        {
            throw new ArgumentNullException(nameof(employeeQualificationDto), "Employee qualification data is null.");
        }

        try
        {
            var newEmployeeQualification = await _db.EmployeeQualification.Add(new EmployeeQualification(employeeQualificationDto));
            return newEmployeeQualification;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<EmployeeQualificationDto> DeleteEmployeeQualification(int id)
    {
        try
        {
            var existsingEmployeeQualification = await GetEmployeeQualification(id);
            var deleteEmployeeQualification = await _db.EmployeeQualification.Delete(existsingEmployeeQualification.Id);
            return deleteEmployeeQualification;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<List<EmployeeQualificationDto>> GetAllEmployeeQualifications()
    {
        try
        {
            return await _db.EmployeeQualification.GetAll();
        }
        catch (Exception)
        {
            throw;
        }   
    }

    public async Task<EmployeeQualificationDto> GetEmployeeQualification(int id)
    {
        try
        {
            var existingEmployeeQualification = await _db.EmployeeQualification
             .Get(employeeQualification => employeeQualification.Id == id)
             .Select(employeeQualification => employeeQualification.ToDto())
             .FirstAsync();

            if(existingEmployeeQualification == null)
            {
                throw new KeyNotFoundException($"Employee qualification with ID {id} not f0und...");
            }

            return existingEmployeeQualification;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<EmployeeQualificationDto> UpdateEmployeeQualification(EmployeeQualificationDto employeeQualificationDto)
    {
        if (employeeQualificationDto == null)
        {
            throw new ArgumentNullException(nameof(employeeQualificationDto), "Employee qualification data is null.");
        }

        try
        {
            var existingEmployeeQualification = await GetEmployeeQualification(employeeQualificationDto.Id);

            var updatedEmployeeQualification = await _db.EmployeeQualification
                    .Update(new EmployeeQualification(existingEmployeeQualification));

            return updatedEmployeeQualification;
        }
        catch (Exception)
        {
            throw;
        } 
    }
}