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

        public UserService(IUnitOfWork db)
        {
            _db = db;
        }

        public async Task<UserDto> AddUser(UserDto userDto)
        {
            return await _db.User.Add(new User(userDto));
        }

        public async Task<UserDto> GetUserByEmail(string email)
        {
            return await _db.User.GetByEmail(email);
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
