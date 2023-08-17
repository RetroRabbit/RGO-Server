using RGO.Models;

namespace RGO.Services.Interfaces
{
    public interface IGradGroupService
    {
        /// <summary>
        /// Retrieve Grad Groups
        /// </summary>
        /// <returns>List of Grad Groups</returns>
        Task<List<GradGroupDto>> GetGradGroups();

        /// <summary>
        /// Add new grad droup
        /// </summary>
        /// <param GradGroupDto="new GradGroupDto Object "></param>
        /// <returns>Added GradGroup</returns>
        Task<GradGroupDto> AddGradGroups(GradGroupDto newGroupDto);

        /// <summary>
        /// Removes grad group
        /// </summary>
        /// <param gradGroupId="gradGroupId"></param>
        /// <returns> Removed GradGroup</returns>
        Task<GradGroupDto> RemoveGradGroups(int gradGroupId);

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="updatedGroup"></param>
        /// <returns>Updated GradGroup</returns>
        Task<GradGroupDto> UpdateGradGroups(GradGroupDto updatedGroup);

    }
}
