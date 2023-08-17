using RGO.Models;
using RGO.Services.Interfaces;
using RGO.UnitOfWork;
using RGO.UnitOfWork.Entities;

namespace RGO.Services.Services
{
    public class GradGroupService : IGradGroupService
    {
        private readonly IUnitOfWork _db;

        public GradGroupService(IUnitOfWork db)
        {
            _db = db;
        }
        public async Task<GradGroupDto> AddGradGroups(GradGroupDto newGroupDto)
        {
            return await _db.GradGroup.Add(new GradGroup(newGroupDto));
        }

        public Task<List<GradGroupDto>> GetGradGroups()
        {
            return _db.GradGroup.GetAll();
        }

        public async Task<GradGroupDto> RemoveGradGroups(int gradGroupId)
        {
            return await _db.GradGroup.Delete(gradGroupId);
        }

        public async Task<GradGroupDto> UpdateGradGroups(GradGroupDto updatedGroup)
        {
            return await _db.GradGroup.Update(new GradGroup(updatedGroup));
        }
    }
}
