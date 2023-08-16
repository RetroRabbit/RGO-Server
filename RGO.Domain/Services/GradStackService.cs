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
    public class GradStackService : IGradStackService
    {
        private readonly IGradStackRepository _gradStackRepository;

        public GradStackService(IGradStackRepository gradStackRepository)
        {
            _gradStackRepository = gradStackRepository;
        }

        public Task<GradStackDto> AddGradStack(int userId)
        {
            return _gradStackRepository.AddGradStack(userId);
        }

        public Task<GradStackDto> GetGradStack(int userId)
        {
            return _gradStackRepository.GetGradStack(userId);
        }

        public Task<bool> HasTechStack(int userId)
        {
            return _gradStackRepository.HasTechStack(userId);
        }

        public Task<GradStackDto> RemoveGradStack(int userId)
        {
            return _gradStackRepository.RemoveGradStack(userId);
        }

        public Task<GradStackDto> UpdateGradStack(int userId, string description)
        {
            return _gradStackRepository.UpdateGradStack(userId, description);
        }
    }
}
