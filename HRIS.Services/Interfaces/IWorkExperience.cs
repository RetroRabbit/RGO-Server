using HRIS.Models;

namespace HRIS.Services.Interfaces;

public interface IWorkExperienceService
{
    Task Save(WorkExperienceDto workExperience);
    Task Delete(int workExperienceId);
    Task Update(WorkExperienceDto workExperience);
    Task<WorkExperienceDto> Get(WorkExperienceDto workExperience);
}
