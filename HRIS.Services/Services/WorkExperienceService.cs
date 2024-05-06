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
        var exists = await _db.WorkExperience.Any(x =>  x.Id == workExperience.Id);

        return exists;
    }

    public async Task Save(WorkExperienceDto workExperience)
    {
        var exists = await CheckIfExists(workExperience);

        if (exists)
        {
            var exception = new Exception("Work experience already exists");
            throw _errorLoggingService.LogException(exception);
        }
        await _db.WorkExperience.Add(new WorkExperience(workExperience));
    }

    public async Task Update(WorkExperienceDto workExperience)
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
            Title = workExperience.Title,
            EmploymentType = workExperience.EmploymentType,
            CompanyName = workExperience.CompanyName,
            Location = workExperience.Location,
            StartDate = workExperience.StartDate,
            EndDate = workExperience.EndDate,
        };

        await _db.WorkExperience.Update(new WorkExperience(workExperienceToUpdate));
    }

    public async Task Delete(int workExperienceId)
    {
        await _db.WorkExperience.Delete(workExperienceId);
    }

    public Task<WorkExperienceDto> Get(WorkExperienceDto workExperience)
    {
        throw new NotImplementedException();
    }
}

