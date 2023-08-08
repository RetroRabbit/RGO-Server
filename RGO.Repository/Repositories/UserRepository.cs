using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using RGO.Domain.Interfaces.Repository;
using RGO.Domain.Models;
using RGO.Repository.Entities;

namespace RGO.Repository.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DatabaseContext _databaseContext;

        public UserRepository(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task<bool> UserExists(string email)
        {
            bool userExists = await _databaseContext.users.AnyAsync(u => u.Email == email);

            return userExists;
        }

        public async Task<UserDto> GetUserByEmail(string email)
        {
            bool userExists = await UserExists(email);

            if (!userExists) throw new Exception("User not found");

            User user = await _databaseContext.users.FirstAsync(u => u.Email == email);

            return user.ToDTO();
        }


        public async Task<UserDto> AddUser(UserDto userDto)
        {
            bool userExists = await UserExists(userDto.Email);

            if (userExists) throw new Exception("Email already exists");
            User user = new User(userDto);
            EntityEntry<User> newUser = _databaseContext.users.Add(user);
            await _databaseContext.SaveChangesAsync();
            return newUser.Entity.ToDTO();
        }

        public async Task<List<int>> GetUserRoles(string email)
        {
            UserDto user = await GetUserByEmail(email);

            List<int> roles = await _databaseContext.userRoles
                .Where(ur => ur.UserId == user.Id)
                .Select(ur => GetRole(ur.Role.Description.ToUpper()))
                .ToListAsync();

            return roles;
        }

        private static int GetRole(string role)
        {
            return role.ToUpper() switch
            {
                "GRAD" => 0,
                "PRESENTER" => 1,
                "MENTOR" => 2,
                _ => 3,
            };
        }
    }
}
