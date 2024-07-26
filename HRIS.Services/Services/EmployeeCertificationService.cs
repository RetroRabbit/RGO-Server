using HRIS.Models;
using HRIS.Services.Interfaces;
using HRIS.Services.Session;
using Microsoft.EntityFrameworkCore;
using RR.UnitOfWork;
using RR.UnitOfWork.Entities.HRIS;

namespace HRIS.Services.Services;

public class EmployeeCertificationService : IEmployeeCertificationService
{
    private readonly IUnitOfWork _db;
    private readonly AuthorizeIdentity _identity;

    public EmployeeCertificationService(IUnitOfWork db, AuthorizeIdentity identity)
    {
        _db = db;
        _identity = identity;
    }

    public async Task<bool> CheckIfEmployeeExists(int Id)
    {
        return await _db.Employee.Any(employee => employee.Id == Id);
    }
    public async Task<bool> CheckIfCertificationExists(int Id)
    {
        return await _db.EmployeeCertification.Any(employee => employee.Id == Id);
    }

    public async Task<EmployeeCertificationDto> SaveEmployeeCertification(EmployeeCertificationDto employeeCertificationDto)
    {
        var exists = await CheckIfCertificationExists(employeeCertificationDto.EmployeeId);
        if (!exists)
            throw new CustomException("Certificate not found");

        if (!_identity.IsSupport && employeeCertificationDto.Id != _identity.EmployeeId)
            throw new CustomException("Unauthorized access.");

        return (await _db.EmployeeCertification.Add(new EmployeeCertification(employeeCertificationDto))).ToDto();
    }

    public async Task<EmployeeCertificationDto> GetEmployeeCertification(int employeeId, int certificationId)
    {
        if (!await CheckIfEmployeeExists(employeeId))
            throw new CustomException("Employee not found");

        if(!await CheckIfCertificationExists(certificationId))
            throw new CustomException("Certificate not found");

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
        if (!await CheckIfEmployeeExists(employeeId))
            throw new CustomException("Employee not found");

        return await _db.EmployeeCertification
                            .Get(certificate => certificate.EmployeeId == employeeId)
                            .Include(certificate => certificate.Employee)
                            .Select(certificate => certificate.ToDto())
                            .ToListAsync();
    }

    public async Task<EmployeeCertificationDto> UpdateEmployeeCertification(EmployeeCertificationDto employeeCertificationDto)
    {
        var exists = await CheckIfCertificationExists(employeeCertificationDto.EmployeeId);
        if (!exists)
            throw new CustomException("Certificate not found");

        if (!_identity.IsSupport && employeeCertificationDto.Id != _identity.EmployeeId)
            throw new CustomException("Unauthorized access.");

        return (await _db.EmployeeCertification.Update(new EmployeeCertification(employeeCertificationDto))).ToDto();
    }

    public async Task<EmployeeCertificationDto> DeleteEmployeeCertification(int id)
    {
        var exists = await CheckIfCertificationExists(Id);
        if (!exists)
            throw new CustomException("Certificate not found");

        if (_identity.Role is not ("SuperAdmin" or "Admin" or "Talent" or "Journey") && Id != _identity.EmployeeId)
        {
            throw new CustomException("Unauthorized access.");
        }

        return (await _db.EmployeeCertification.Delete(Id)).ToDto();
    }
}
