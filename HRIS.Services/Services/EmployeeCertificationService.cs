using HRIS.Models;
using HRIS.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using RR.UnitOfWork;
using RR.UnitOfWork.Entities.HRIS;

namespace HRIS.Services.Services;

public class EmployeeCertificationService : IEmployeeCertificationService
{
    private readonly IUnitOfWork _db;
    private readonly IErrorLoggingService _errorLoggingService;

    public EmployeeCertificationService(IUnitOfWork db, IErrorLoggingService errorLoggingService)
    {
        _db = db;
        _errorLoggingService = errorLoggingService;
    }

    public async Task<EmployeeCertificationDto> SaveEmployeeCertification(EmployeeCertificationDto employeeCertificationDto)
    {

        if (!await CheckEmployeeExists(employeeCertificationDto.EmployeeId))
        {
            var exception = new Exception("Employee not found");
            throw _errorLoggingService.LogException(exception);
        }
        EmployeeCertification certificate = new EmployeeCertification
        {
            Id = 0,
            CertificateName = employeeCertificationDto.CertificateName,
            CertificateDocument = employeeCertificationDto.CertificateDocument,
            IssueDate = employeeCertificationDto.IssueDate,
            IssueOrganization = employeeCertificationDto.IssueOrganization,
            EmployeeId = employeeCertificationDto.EmployeeId,
        };
        return await _db.EmployeeCertification.Add(certificate);
    }

    public async Task<EmployeeCertificationDto> GetEmployeeCertification(int employeeId, int certificationId)
    {
        if (!await CheckEmployeeExists(employeeId))
        {
            var exception = new Exception("Employee not found");
            throw _errorLoggingService.LogException(exception);
        }

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
        {
            var exception = new Exception("Employee certification record not found");
            throw _errorLoggingService.LogException(exception);
        }
        return employeeCertification;
    }

    public async Task<List<EmployeeCertificationDto>> GetAllEmployeeCertifications(int employeeId)
    {
        if (!await CheckEmployeeExists(employeeId))
        {
            var exception = new Exception("Employee not found");
            throw _errorLoggingService.LogException(exception);
        }

        return await _db.EmployeeCertification
                            .Get(certificate => certificate.EmployeeId == employeeId)
                            .Include(certificate => certificate.Employee)
                            .Select(certificate => certificate.ToDto())
                            .ToListAsync();
    }

    public async Task<EmployeeCertificationDto> UpdateEmployeeCertification(EmployeeCertificationDto employeeCertificationDto)
    {
        if (!await CheckEmployeeExists(employeeCertificationDto.EmployeeId))
        {
            var exception = new Exception("Employee not found");
            throw _errorLoggingService.LogException(exception);
        }
        EmployeeCertification certificate = new EmployeeCertification
        {
            Id = 0,
            CertificateName = employeeCertificationDto.CertificateName,
            CertificateDocument = employeeCertificationDto.CertificateDocument,
            IssueDate = employeeCertificationDto.IssueDate,
            IssueOrganization = employeeCertificationDto.IssueOrganization,
            EmployeeId = employeeCertificationDto.EmployeeId,
        };
        return await _db.EmployeeCertification.Update(certificate);
    }

    public async Task<EmployeeCertificationDto> DeleteEmployeeCertification(int id)
    {
        if (!await CheckEmployeeExists(id))
        {
            var exception = new Exception("Employee not found");
            throw _errorLoggingService.LogException(exception);
        }

        return await _db.EmployeeCertification.Delete(id);
    }

    private async Task<bool> CheckEmployeeExists(int employeeId)
    {
        var employee = await _db.Employee
                                .Get(employee => employee.Id == employeeId)
                                .FirstOrDefaultAsync();

        if (employee == null)
            return false;
        return true;
    }
}