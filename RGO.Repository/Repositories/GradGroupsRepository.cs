using Microsoft.EntityFrameworkCore;
using RGO.Domain.Interfaces.Repository;
using RGO.Domain.Models;
using RGO.Repository.Entities;

namespace RGO.Repository.Repositories;

public class GradGroupsRepository : IGradGroupsRepository
{
    private readonly DatabaseContext _databaseContext;

    public GradGroupsRepository(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }


    public async Task<List<GradGroupDto>> GetGradGroups()
    {
        return await _databaseContext.gradGroups
            .Select(group => group.ToDTO())
            .ToListAsync();
    }
}
