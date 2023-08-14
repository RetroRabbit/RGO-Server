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
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IProfileRepository _profileRepository;
        private readonly IUserGroupsRepository _userGroupsRepository;

        public UserService(IUserRepository userRepository, IUserGroupsRepository userGroupsRepository, IProfileRepository profileRepository)
        {
            _userRepository = userRepository;
            _profileRepository = profileRepository;
            _userGroupsRepository = userGroupsRepository;
        }

        public async Task<UserDto> AddUser(UserDto userDto)
        {
            var newUser = await _userRepository.AddUser(userDto);
            return newUser;
        }

        public async Task<UserDto> GetUserByEmail(string email)
        {
            return await _userRepository.GetUserByEmail(email);
        }

        public async Task<ProfileDto> UpdateUser(string email,ProfileDto profile)
        {
            var currentUser = await _userRepository.UpdateUser(email, profile);
            return await _profileRepository.GetUserProfileByEmail(email);
        }

        public async Task<List<UserDto>> GetUsers()
        {
            return await _userRepository.GetUsers();
        }

        public async Task<List<UserGroupDto>> GetUserGroups()
        {
            return await _userGroupsRepository.GetUserGroups();
        }
    }
}
