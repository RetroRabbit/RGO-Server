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
    /// <param name="newGroupDto"></param>
    /// <returns>Added Grad Group</returns>
    Task<GradGroupDto> AddGradGroups(GradGroupDto newGroupDto);

    /// <summary>
    /// Removes grad group
    /// </summary>
    /// <param name="gradGroupId"></param>
    /// <returns> Removed GradGroup</returns>
    Task<GradGroupDto> RemoveGradGroups(int gradGroupId);

    /// <summary>
    /// Update
    /// </summary>
    /// <param name="gradGroupId"></param>
    /// <returns> Updated GradGroup</returns>
    Task<GradGroupDto> UpdateGradGroups(GradGroupDto updatedGroup);
}
