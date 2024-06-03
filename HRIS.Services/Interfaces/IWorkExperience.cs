using HRIS.Models;

namespace HRIS.Services.Interfaces;

public interface IWorkExperienceService
{
    /// <summary>
    /// Save Employee work experience
    /// </summary>
    /// <param name="workExperienceDto"></param>
    /// <returns>Employee work experience</returns>
    Task<WorkExperienceDto> Save(WorkExperienceDto workExperienceDto);

    /// <summary>
    /// Delete Employee work experience
    /// </summary>
    /// <param name="workExperienceDto"></param>
    /// <returns>Delete Employee work experience</returns>
    Task<WorkExperienceDto> Delete(int workExperienceId);
    /// <summary>
    /// Update Employee work experience
    /// </summary>
    /// <param name="workExperienceDto"></param>
    /// <returns>Update Employee work experience</returns>
    Task<WorkExperienceDto> Update(WorkExperienceDto workExperience);

    /// <summary>
    /// Get  specific work experience data via the ID
    /// </summary>
    /// <param name="employeeId"></param>
    /// <param name="status"></param>
    /// <returns>Employee Work experience</returns>
    Task<List<WorkExperienceDto>> GetWorkExperienceByEmployeeId(int id);
}
