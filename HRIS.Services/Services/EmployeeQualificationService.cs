using HRIS.Models;
using HRIS.Services.Interfaces;
using HRIS.Services.Session;
using Microsoft.EntityFrameworkCore;
using RR.UnitOfWork;
using RR.UnitOfWork.Entities.HRIS;

namespace HRIS.Services.Services;

public class EmployeeQualificationService : IEmployeeQualificationService
{
    private readonly IUnitOfWork _db;
    private readonly AuthorizeIdentity _identity;
    public EmployeeQualificationService(IUnitOfWork db, AuthorizeIdentity identity)
    {
        _db = db;
        _identity = identity;
    }

    public async Task<bool> CheckIfExists(int id)
    {
        return await _db.EmployeeQualification.Any(x => x.Id == id);
    }

    public async Task<EmployeeQualificationDto> CreateEmployeeQualification(EmployeeQualificationDto employeeQualificationDto, int employeeId)
    {
        var exists = await CheckIfExists(employeeQualificationDto.Id);

        if (exists)
            throw new CustomException("Employee Already Have Existing Qualifications Saved");

        if (_identity.IsSupport == false && _identity.EmployeeId != employeeQualificationDto.EmployeeId)
            throw new CustomException("Unauthorized access.");

        var model = new EmployeeQualification(employeeQualificationDto)
        {
            EmployeeId = _identity.EmployeeId
        };
        model = await _db.EmployeeQualification.Add(model);

        return model.ToDto();
    }

    public async Task<List<EmployeeQualificationDto>> GetAllEmployeeQualifications()
    {
        return await _db.EmployeeQualification.Get().Select(x => x.ToDto()).ToListAsync();
    }

    public async Task<EmployeeQualificationDto> GetEmployeeQualificationsByEmployeeId(int employeeId)
    {
        if (_identity.IsSupport == false && _identity.EmployeeId != employeeId)
            throw new CustomException("Unauthorized access");

        var qualifications = await _db.EmployeeQualification.FirstOrDefault(x => x.EmployeeId == _identity.EmployeeId);

     return qualifications?.ToDto();
    }

    public async Task<EmployeeQualificationDto> UpdateEmployeeQualification(
        EmployeeQualificationDto employeeQualificationDto)
    {
        var exists = await CheckIfExists(employeeQualificationDto.Id);

        if (!exists)
            throw new CustomException("Employee Does Not Have Existing Qualifications Saved");

        if (_identity.IsSupport == false && _identity.EmployeeId != employeeQualificationDto.EmployeeId)
            throw new CustomException("Unauthorized access.");

        var qualification = await _db.EmployeeQualification.FirstOrDefault(x => x.Id == employeeQualificationDto.Id);

        qualification.EmployeeId = _identity.EmployeeId;
        qualification.HighestQualification = employeeQualificationDto.HighestQualification;
        qualification.School = employeeQualificationDto.School;
        qualification.FieldOfStudy = employeeQualificationDto.FieldOfStudy;
        qualification.NQFLevel = employeeQualificationDto.NQFLevel;
        qualification.Year = employeeQualificationDto.Year;
        qualification.ProofOfQualification = employeeQualificationDto.ProofOfQualification;
        qualification.DocumentName = employeeQualificationDto.DocumentName;

        qualification = await _db.EmployeeQualification.Update(qualification);

        return qualification.ToDto();
    }

    public async Task<EmployeeQualificationDto> DeleteEmployeeQualification(int id)
    {
        var exists = await CheckIfExists(id);

        if (!exists) throw new CustomException("Employee Does Not Have Existing Qualifications Saved");

        if (_identity.IsSupport == false)
            throw new CustomException("Unauthorized access.");

        var deleted = await _db.EmployeeQualification.Delete(id);
        return deleted.ToDto();
    }
}