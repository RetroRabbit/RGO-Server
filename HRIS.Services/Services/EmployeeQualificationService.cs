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

    public EmployeeQualificationService(IUnitOfWork db, IEmployeeService employeeService)
    {
        _db = db;
        _employeeService = employeeService;
    }

    public async Task<EmployeeQualificationDto> SaveEmployeeQualification(
        EmployeeQualificationDto employeeQualificationDto, int employeeId)
    {
        var employee = await _employeeService.GetEmployeeById(employeeId);

        var existing = await _db.EmployeeQualification.Any(x => x.EmployeeId == employee.Id);
        if (existing) throw new CustomException("Employee Already Have Existing Qualifications Saved");

        var model = new EmployeeQualification(employeeQualificationDto)
        {
            EmployeeId = employee.Id
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
        var employee = await _employeeService.GetEmployeeById(employeeId);
        var qualifications = await _db.EmployeeQualification.FirstOrDefault(x => x.EmployeeId == employee.Id);
        return qualifications?.ToDto();
    }

    public async Task<EmployeeQualificationDto> UpdateEmployeeQualification(
        EmployeeQualificationDto employeeQualificationDto)
    {
        var employee = await _employeeService.GetEmployeeById(employeeQualificationDto.EmployeeId);

        var qualification = await _db.EmployeeQualification.FirstOrDefault(x => x.Id == employeeQualificationDto.Id);
        if (qualification == null) throw new CustomException("Employee Does Not Have Existing Qualifications Saved");

        qualification.EmployeeId = employee.Id;
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
        var existing = await _db.EmployeeQualification.Any(x => x.Id == id);
        if (!existing) throw new CustomException("Employee Does Not Have Existing Qualifications Saved");
        var deleted = await _db.EmployeeQualification.Delete(id);
        return deleted.ToDto();
    }
}