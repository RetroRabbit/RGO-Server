using HRIS.Models;
using HRIS.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using RR.UnitOfWork;
using RR.UnitOfWork.Entities.HRIS;

namespace HRIS.Services.Services;

public class EmployeeCertificationService : IEmployeeCertificationService
{
    private readonly IUnitOfWork _db;

    public EmployeeCertificationService(IUnitOfWork db)
    {
        _db = db;
    }

    public async Task<EmployeeCertificationDto> SaveEmployeeCertification(EmployeeCertificationDto employeeCertificationDto)
    {

        if (!await CheckEmployeeExists(employeeCertificationDto.EmployeeId))
            throw new CustomException("Employee not found");

        return (await _db.EmployeeCertification.Add(new EmployeeCertification(employeeCertificationDto))).ToDto();
    }

    public async Task<EmployeeCertificationDto> GetEmployeeCertification(int employeeId, int certificationId)
    {
        if (!await CheckEmployeeExists(employeeId))
            throw new CustomException("Employee not found");

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
            throw new CustomException("Employee certification record not found");
        
        return employeeCertification;
    }

    public async Task<List<EmployeeCertificationDto>> GetAllEmployeeCertifications(int employeeId)
    {
        if (!await CheckEmployeeExists(employeeId))
            throw new CustomException("Employee not found");

        return await _db.EmployeeCertification
                            .Get(certificate => certificate.EmployeeId == employeeId)
                            .Include(certificate => certificate.Employee)
                            .Select(certificate => certificate.ToDto())
                            .ToListAsync();
    }

    public async Task<EmployeeCertificationDto> UpdateEmployeeCertification(EmployeeCertificationDto employeeCertificationDto)
    {
        if (!await CheckEmployeeExists(employeeCertificationDto.EmployeeId))
            throw new CustomException("Employee not found");

        return (await _db.EmployeeCertification.Update(new EmployeeCertification(employeeCertificationDto))).ToDto();
    }

    public async Task<EmployeeCertificationDto> DeleteEmployeeCertification(int id)
    {
        return (await _db.EmployeeCertification.Delete(id)).ToDto();
    }

    public async Task<bool> CheckEmployeeExists(int employeeId)
    {
        var employee = await _db.Employee
                                .Get(employee => employee.Id == employeeId)
                                .FirstOrDefaultAsync();

        return employee != null;
    }
}