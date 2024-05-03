using HRIS.Models;
using HRIS.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using RR.UnitOfWork;
using RR.UnitOfWork.Entities.HRIS;

namespace HRIS.Services.Services;

public class WorkExperienceService : IWorkExperienceService
{
  
    public Task<WorkExperienceDto> Get(WorkExperienceDto workExperience)
    {
        throw new NotImplementedException();
    }

    public Task Save(WorkExperienceDto workExperience)
    {
        throw new NotImplementedException();
    }

    public Task Update(WorkExperienceDto workExperience)
    {
        throw new NotImplementedException();
    }

    public Task Delete(int workExperienceId)
    {
        throw new NotImplementedException();
    }

}

