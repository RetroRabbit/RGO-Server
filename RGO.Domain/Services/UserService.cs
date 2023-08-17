using System.Xml.Linq;
using Microsoft.EntityFrameworkCore;
using RGO.Models;
using RGO.Services.Interfaces;
using RGO.UnitOfWork;
using RGO.UnitOfWork.Entities;

namespace RGO.Services.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _db;
        private readonly IProfileService _profileService;

        public UserService(IUnitOfWork db, IProfileService profileService)
        {
            _db = db;
            _profileService = profileService;
        }

        public async Task<UserDto> AddUser(UserDto userDto)
        {
            return await _db.User.Add(new User(userDto));
        }

        public async Task<UserDto> GetUserByEmail(string email)
        {
            return await _db.User.GetByEmail(email);
        }

        public async Task<ProfileDto> UpdateUser(string email,ProfileDto profile)
        {
            await _db.User.UpdateUser(email, profile);
            return await _profileService.GetUserProfileByEmail(email);
        }

        public async Task<List<UserDto>> GetUsers()
        {
            return await _db.User.GetUsers();
        }

        public async Task<List<GradGroupDto>> GetGradGroups()
        {
            return await _db.GradGroup.GetAll();
        }

        public async Task<UserDto> RemoveUser(string email)
        {
            var obj = _db.User.GetByEmail(email);
            return await _db.User.Delete(obj.Id);
        }
    }
}
