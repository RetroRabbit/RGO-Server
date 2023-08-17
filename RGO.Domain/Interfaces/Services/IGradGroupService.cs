using RGO.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RGO.Domain.Interfaces.Services
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
        Task<GradGroupDto> UpdateGradGroups(int gradGroupId, GradGroupDto updatedGroup);

    }
}
