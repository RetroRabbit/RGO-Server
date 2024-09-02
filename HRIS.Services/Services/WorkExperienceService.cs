using Auth0.ManagementApi.Models;
using AutoMapper;
using HRIS.Models;
using HRIS.Services.Interfaces;
using HRIS.Services.Session;
using Microsoft.EntityFrameworkCore;
using RR.UnitOfWork;
using RR.UnitOfWork.Entities.HRIS;

namespace HRIS.Services.Services;

public class WorkExperienceService : IWorkExperienceService
{
    private readonly IUnitOfWork _db;
    private readonly IMapper _mapper;
    private readonly AuthorizeIdentity _identity;

    public WorkExperienceService(IUnitOfWork db, IMapper mapper, AuthorizeIdentity identity)
    {
        _db = db;
        _mapper = mapper;
        _identity = identity;
    }

    public async Task<bool> CheckIfExists(int workExperienceId)
    {
        return await _db.WorkExperience.Any(x => x.Id == workExperienceId);
    }

    public async Task<WorkExperienceDto> Save(WorkExperienceDto workExperience)
    {
        var exists = await CheckIfExists(workExperience.Id);

        if (exists)
            throw new CustomException("Work experience already exists");

        if (_identity.IsSupport == false && _identity.EmployeeId != workExperience.EmployeeId)
            throw new CustomException("Unauthorized access.");

        var newWorkExperiece = _mapper.Map<WorkExperience>(workExperience);

        return _mapper.Map<WorkExperienceDto>(await _db.WorkExperience.Add(newWorkExperiece));
    }

    public async Task<WorkExperienceDto> Update(WorkExperienceDto workExperience)
    {
        var exists = await CheckIfExists(workExperience.Id);

        if (!exists)
            throw new CustomException("Employee work experience does not exist");

        if (_identity.IsSupport == false && _identity.EmployeeId != workExperience.EmployeeId)
            throw new CustomException("Unauthorized access.");

        var workExperienceToUpdate = _mapper.Map<WorkExperience>(workExperience);

        return _mapper.Map<WorkExperienceDto>(await _db.WorkExperience.Update(workExperienceToUpdate));
    }

    public async Task<WorkExperienceDto> Delete(int workExperienceId)
    {
        var exists = await CheckIfExists(workExperienceId);

        if (!exists)
            throw new CustomException("Employee work experience does not exist");

        return _mapper.Map<WorkExperienceDto>(await _db.WorkExperience.Delete(workExperienceId));
    }

    public async Task<List<WorkExperienceDto>> GetWorkExperienceByEmployeeId(int id)
    {

        if (_identity.IsSupport == false && _identity.EmployeeId != id)
            throw new CustomException("Unauthorized access.");

        return await _db.WorkExperience
            .Get(workExperience => workExperience.EmployeeId == id)
            .Select(workExperience => _mapper.Map<WorkExperienceDto>(workExperience))
            .ToListAsync();
    }
}

