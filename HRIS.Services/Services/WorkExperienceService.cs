using HRIS.Models;
using HRIS.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using RR.UnitOfWork;
using RR.UnitOfWork.Entities.HRIS;

namespace HRIS.Services.Services;

public class WorkExperienceService : IWorkExperienceService
{
    private readonly IUnitOfWork _db;
    private readonly IErrorLoggingService _errorLoggingService;

    public WorkExperienceService(IUnitOfWork db, IErrorLoggingService errorLoggingService)
    {
        _db = db;
        _errorLoggingService = errorLoggingService;
    }

    public async Task<bool> CheckIfExists(WorkExperienceDto workExperience)
    {
        return await _db.WorkExperience.Any(x => x.Id == workExperience.Id);
    }

    public async Task<WorkExperienceDto> Save(WorkExperienceDto workExperience)
    {
        var exists = await CheckIfExists(workExperience);

        if (exists)
        {
            throw _errorLoggingService.LogException(new Exception("Work experience already exists"));
        }
        return (await _db.WorkExperience.Add(new WorkExperience(workExperience))).ToDto();
    }

    public async Task<WorkExperienceDto> Update(WorkExperienceDto workExperience)
    {
        var exists = await CheckIfExists(workExperience);

        if (!exists)
        {
            var exception = new Exception("Employee Date does not exist");
            throw _errorLoggingService.LogException(exception);
        }

        var workExperienceToUpdate = new WorkExperienceDto
        {
            Id = workExperience.Id,
            ClientName = workExperience.ClientName,
            ProjectName = workExperience.ProjectName,
            SkillSet = workExperience.SkillSet,
            Software = workExperience.Software,
            EmployeeId = workExperience.EmployeeId,
            StartDate = workExperience.StartDate,
            EndDate = workExperience.EndDate,
            ProjectDescription = workExperience.ProjectDescription,
        };
        return (await _db.WorkExperience.Update(new WorkExperience(workExperienceToUpdate))).ToDto();
    }

    public async Task<WorkExperienceDto> Delete(int workExperienceId)
    {
        return (await _db.WorkExperience.Delete(workExperienceId)).ToDto();
    }

    public async Task<List<WorkExperienceDto>> GetWorkExperienceByEmployeeId(int id)
    {
        return await _db.WorkExperience
             .Get(workExperience => workExperience.EmployeeId == id)
             .Select(workExperience => workExperience.ToDto())
             .ToListAsync();
    }
}

