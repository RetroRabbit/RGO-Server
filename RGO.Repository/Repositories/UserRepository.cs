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

        public async Task<UserDto> UpdateUser(string email, UserDto updatedUserDto)
        {
            User? existingUser = await _databaseContext.users.FirstOrDefaultAsync(u => u.Email == email);

            if (existingUser == null)
            {
                throw new Exception("User not found");
            }

            existingUser.FirstName = updatedUserDto.FirstName;
            existingUser.LastName = updatedUserDto.LastName;
            existingUser.Email = updatedUserDto.Email;
            existingUser.Type = updatedUserDto.Type;
            existingUser.JoinDate = updatedUserDto.JoinDate;
            existingUser.Status = updatedUserDto.Status;
            existingUser.Bio = updatedUserDto.Bio;
            existingUser.Level = updatedUserDto.Level;
            existingUser.Phone = updatedUserDto.Phone;

            var currentUser = _databaseContext.users.Update(existingUser);
            await _databaseContext.SaveChangesAsync();

            return currentUser.Entity.ToDTO();
        }
        public async Task<List<UserDto>> GetUsers()
        {
            var allUsers = await _databaseContext.users.ToListAsync();
            var allUsersDto = new List<UserDto>();
            foreach(var user in allUsers)
            {
                allUsersDto.Add(user.ToDTO());
            }
            return allUsersDto;
        }

    }
}
