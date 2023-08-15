﻿using RGO.Domain.Models;

namespace RGO.Domain.Interfaces.Repository;

public interface IGradGroupsRepository
{
    /// <summary>
    /// Retrieve Grad Groups
    /// </summary>
    /// <returns>List of Grad Groups</returns>
    public Task<List<GradGroupDto>> GetGradGroups();
}
