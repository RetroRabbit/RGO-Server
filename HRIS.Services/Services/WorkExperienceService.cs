using AutoMapper;
using HRIS.Models;
using HRIS.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using RR.UnitOfWork;
using RR.UnitOfWork.Entities.HRIS;

namespace HRIS.Services.Services;

public class WorkExperienceService : IWorkExperienceService
{
    private readonly IUnitOfWork _db;
    private readonly IMapper _mapper;

    public WorkExperienceService(IUnitOfWork db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    public async Task<bool> CheckIfExists(WorkExperienceDto workExperience)
    {
        return await _db.WorkExperience.Any(x => x.Id == workExperience.Id);
    }

    public async Task<WorkExperienceDto> Save(WorkExperienceDto workExperience)
    {
        var exists = await CheckIfExists(workExperience);

        if (exists)
            throw new CustomException("Work experience already exists");

        var newWorkExperiece = _mapper.Map<WorkExperience>(workExperience);

        return _mapper.Map<WorkExperienceDto>(await _db.WorkExperience.Add(newWorkExperiece));
    }

    public async Task<WorkExperienceDto> Update(WorkExperienceDto workExperience)
    {
        var exists = await CheckIfExists(workExperience);

        if (!exists)
            throw new CustomException("Employee Date does not exist");

        var workExperienceToUpdate = _mapper.Map<WorkExperience>(workExperience);

        return _mapper.Map<WorkExperienceDto>(await _db.WorkExperience.Update(workExperienceToUpdate));
    }

    public async Task<WorkExperienceDto> Delete(int workExperienceId)
    {
        return _mapper.Map<WorkExperienceDto>(await _db.WorkExperience.Delete(workExperienceId));
    }

    public async Task<List<WorkExperienceDto>> GetWorkExperienceByEmployeeId(int id)
    {
        return await _db.WorkExperience
             .Get(workExperience => workExperience.EmployeeId == id)
             .Select(workExperience => _mapper.Map<WorkExperienceDto>(workExperience))
             .ToListAsync();
    }
}

