using RGO.Domain.Models;

namespace RGO.Domain.Interfaces.Repository;

public interface IGradGroupRepository
{
    /// <summary>
    /// Retrieve Grad Groups
    /// </summary>
    /// <returns>List of Grad Groups</returns>
    Task<List<GradGroupDto>> GetGradGroups();

    /// <summary>
    /// Add new grad droup
    /// </summary>
    Task<GradGroupDto> AddGradGroups(GradGroupDto newGroupDto);

    /// <summary>
    /// Removes grad group
    /// </summary>
    /// <param name="gradGroupId"></param>
    Task<GradGroupDto> RemoveGradGroups(int gradGroupId);

    /// <summary>
    /// Update
    /// </summary>
    /// <param name="gradGroupId"></param>
    Task<GradGroupDto> UpdateGradGroups(GradGroupDto updatedGroup);
}
