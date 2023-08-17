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
        /// <param GradGroupDto="new GradGroupDto Object "></param>
        Task<GradGroupDto> AddGradGroups(GradGroupDto newGroupDto);

        /// <summary>
        /// Removes grad group
        /// </summary>
        /// <param gradGroupId="gradGroupId"></param>
        Task<GradGroupDto> RemoveGradGroups(int gradGroupId);

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="updatedGroup"></param>
        /// <returns></returns>
        Task<GradGroupDto> UpdateGradGroups(GradGroupDto updatedGroup);

    }
}
