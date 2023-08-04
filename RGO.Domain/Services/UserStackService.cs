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
    public class UserStackService : IUserStackService
    {
        private readonly IUserStackRepository _userStackRepository;

        public UserStackService(IUserStackRepository userStackRepository)
        {
            _userStackRepository = userStackRepository;
        }

        public Task<UserStackDto> AddUserStack(int userId)
        {
            return _userStackRepository.AddUserStack(userId);
        }

        public Task<UserStackDto> GetUserStack(int userId)
        {
            return _userStackRepository.GetUserStack(userId);
        }

        public Task<bool> HasTechStack(int userId)
        {
            return _userStackRepository.HasTechStack(userId);
        }

        public Task<UserStackDto> RemoveUserStack(int userId)
        {
            return _userStackRepository.RemoveUserStack(userId);
        }

        public Task<UserStackDto> UpdateUserStack(int userId, string description)
        {
            return _userStackRepository.UpdateUserStack(userId, description);
        }
    }
}
