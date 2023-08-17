using RGO.Domain.Interfaces.Repository;
using RGO.Domain.Interfaces.Services;
using RGO.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RGO.Domain.Services
{
    public class GradGroupService : IGradGroupService
    {
        private readonly IGradGroupRepository _gradGroupRepository;

        public GradGroupService(IGradGroupRepository gradGroupRepository)
        {
            _gradGroupRepository = gradGroupRepository;
        }
        public Task<GradGroupDto> AddGradGroups(GradGroupDto newGroupDto)
        {
            return _gradGroupRepository.AddGradGroups(newGroupDto);
        }

        public Task<List<GradGroupDto>> GetGradGroups()
        {
            return _gradGroupRepository.GetGradGroups();
        }

        public Task<GradGroupDto> RemoveGradGroups(int gradGroupId)
        {
            return _gradGroupRepository.RemoveGradGroups(gradGroupId);
        }

        public Task<GradGroupDto> UpdateGradGroups(GradGroupDto updatedGroup)
        {
            return _gradGroupRepository.UpdateGradGroups(updatedGroup);
        }
    }
}
