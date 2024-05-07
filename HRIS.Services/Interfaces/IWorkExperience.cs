using HRIS.Models;

namespace HRIS.Services.Interfaces;

public interface IWorkExperienceService
{
    Task<WorkExperienceDto> Save(WorkExperienceDto workExperience);
    Task<WorkExperienceDto> Delete(int workExperienceId);
    Task<WorkExperienceDto> Update(WorkExperienceDto workExperience);
    Task<WorkExperienceDto> GetWorkExperienceById(int id);
}
