using Microsoft.EntityFrameworkCore;
using RGO.Models;
using RGO.Repository.Interfaces;
using RGO.UnitOfWork.Entities;

namespace RGO.Repository.Repositories;

public class GradGroupRepository : IGradGroupRepository
{
    private readonly DatabaseContext _databaseContext;

    public GradGroupRepository(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }


    public async Task<List<GradGroupDto>> GetGradGroups()
    {
        return await _databaseContext.gradGroups
            .Select(group => group.ToDTO())
            .ToListAsync();
    }

    public async Task<GradGroupDto> AddGradGroups(GradGroupDto newGroupDto)
    {
        GradGroupDto newGradGroup = new GradGroupDto
        (
            0,
            newGroupDto.Title
        );

        try
        {
            var gradGroup = await _databaseContext.gradGroups.AddAsync(new GradGroup(newGradGroup));
            await _databaseContext.SaveChangesAsync();
            return gradGroup.Entity.ToDTO();
        }
        catch (Exception ex)
        {

            throw new Exception($"Failed to create Group.(Error: {ex.Message})");
        }
    }

    public async Task<GradGroupDto> RemoveGradGroups(int gradGroupId)
    {
        var existingGroup = await _databaseContext.gradGroups.FirstOrDefaultAsync(x => x.Id == gradGroupId);
        if (existingGroup == null)
        {
            throw new KeyNotFoundException("GradGroup Not found with the provided ID");
        }
        var removedGroup = _databaseContext.gradGroups.Remove(existingGroup);
        await _databaseContext.SaveChangesAsync();
        return removedGroup.Entity.ToDTO();
    }

    public async Task<GradGroupDto> UpdateGradGroups(GradGroupDto updatedGroup)
    {
        var existingGroup = await _databaseContext.gradGroups.FirstOrDefaultAsync(x => x.Id == updatedGroup.Id);
        if (existingGroup == null) 
        {
            throw new KeyNotFoundException("GradGroup Not found with the provided ID");
        }

        existingGroup.Title = updatedGroup.Title;
        await _databaseContext.SaveChangesAsync();
        return existingGroup.ToDTO();
    }
}
