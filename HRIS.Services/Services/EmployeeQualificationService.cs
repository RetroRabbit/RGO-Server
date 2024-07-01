using HRIS.Models;
using HRIS.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using RR.UnitOfWork;
using RR.UnitOfWork.Entities.HRIS;

namespace HRIS.Services.Services;

public class EmployeeQualificationService : IEmployeeQualificationService
{
    private readonly IUnitOfWork _db;
    private readonly IEmployeeService _employeeService;
    private readonly IErrorLoggingService _errorLoggingService;

    public EmployeeQualificationService(IUnitOfWork db, IEmployeeService employeeService, IErrorLoggingService errorLoggingService)
    {
        _db = db;
        _employeeService = employeeService;
        _errorLoggingService = errorLoggingService;
    }

    public async Task<EmployeeQualificationDto> SaveEmployeeQualification(EmployeeQualificationDto employeeQualificationDto, int id)
    {
        try
        {
            var employee = await _employeeService.GetEmployeeById(id);
            if (employee == null)
            {
                throw new Exception($"No employee found with ID {id}.");
            }

            var newQualification = new EmployeeQualification
            {
                EmployeeId = id,
                HighestQualification = employeeQualificationDto.HighestQualification,
                School = employeeQualificationDto.School,
                FieldOfStudy = employeeQualificationDto.FieldOfStudy,
                NQFLevel = employeeQualificationDto.NQFLevel,
                Year = employeeQualificationDto.Year,
                ProofOfQualification = employeeQualificationDto.ProofOfQualification,
                DocumentName = employeeQualificationDto.DocumentName,
            };
            var addedQualification = await _db.EmployeeQualification.Add(newQualification);

            employeeQualificationDto.Id = addedQualification.Id;

            return addedQualification.ToDto();
        }
        catch (Exception ex)
        {
            throw _errorLoggingService.LogException(ex);
        }
    }

    public async Task<List<EmployeeQualificationDto>> GetAllEmployeeQualifications()
    {
        try
        {
            return (await _db.EmployeeQualification.GetAll()).Select(x => x.ToDto()).ToList();
        }
        catch (Exception ex)
        {
            throw _errorLoggingService.LogException(ex);
        }
    }

    public async Task<EmployeeQualificationDto> GetAllEmployeeQualificationsByEmployeeId(int employeeId)
    {
        try
        {

            var employeeExists = await _employeeService.GetEmployeeById(employeeId);
            if (employeeExists == null)
            {
                throw new KeyNotFoundException($"No employee found with ID {employeeId}");
            }

            var qualifications = await _db.EmployeeQualification
                .Get(q => q.EmployeeId == employeeId)
                .Select(q => new EmployeeQualificationDto
                {
                    Id = q.Id,
                    EmployeeId = q.EmployeeId,
                    HighestQualification = q.HighestQualification,
                    School = q.School,
                    FieldOfStudy = q.FieldOfStudy,
                    NQFLevel = q.NQFLevel,
                    Year = q.Year,
                    ProofOfQualification = q.ProofOfQualification,
                    DocumentName = q.DocumentName
                }).FirstOrDefaultAsync();

            return qualifications;
        }
        catch (Exception ex)
        {
            throw _errorLoggingService.LogException(ex);
        }
    }

    public async Task<EmployeeQualificationDto> GetEmployeeQualificationById(int id)
    {
        try
        {
            var qualification = await _db.EmployeeQualification
                .Get(q => q.Id == id)
                .Select(q => new EmployeeQualificationDto
                {
                    Id = q.Id,
                    EmployeeId = q.EmployeeId,
                    HighestQualification = q.HighestQualification,
                    School = q.School,
                    FieldOfStudy = q.FieldOfStudy,
                    NQFLevel = q.NQFLevel,
                    Year = q.Year,
                    ProofOfQualification= q.ProofOfQualification,
                    DocumentName = q.DocumentName
                })
                .LastOrDefaultAsync();

            if (qualification == null)
            {
                throw new Exception($"No employee qualification found with ID {id}.");
            }

            return qualification;
        }
        catch (Exception ex)
        {
            throw _errorLoggingService.LogException(ex);
        }
    }

    public async Task<EmployeeQualificationDto> UpdateEmployeeQualification(EmployeeQualificationDto employeeQualificationDto)
    {
        if (employeeQualificationDto == null)
            throw new ArgumentNullException(nameof(employeeQualificationDto));

        try
        {
            var qualification = await _db.EmployeeQualification.FirstOrDefault(q => q.Id == employeeQualificationDto.Id);

            if (qualification == null)
            {
                throw new KeyNotFoundException($"No employee qualification found with ID {employeeQualificationDto.Id}.");
            }

            qualification.EmployeeId = employeeQualificationDto.EmployeeId;
            qualification.HighestQualification = employeeQualificationDto.HighestQualification;
            qualification.School = employeeQualificationDto.School;
            qualification.FieldOfStudy = employeeQualificationDto.FieldOfStudy;
            qualification.NQFLevel = employeeQualificationDto.NQFLevel;
            qualification.Year = employeeQualificationDto.Year;
            qualification.ProofOfQualification = employeeQualificationDto.ProofOfQualification;
            qualification.DocumentName = employeeQualificationDto.DocumentName;

            var updatedEmplyeeQualificationDto = await _db.EmployeeQualification.Update(qualification);

            return updatedEmplyeeQualificationDto.ToDto();
        }
        catch (Exception ex)
        {
            throw _errorLoggingService.LogException(ex);
        }
    }

    public async Task<EmployeeQualificationDto> DeleteEmployeeQualification(int id)
    {
        try
        {
            var deletedQualification = await _db.EmployeeQualification.Delete(id);
            return deletedQualification.ToDto();
        }
        catch (Exception ex)
        {
            throw _errorLoggingService.LogException(ex);
        }
    }
}