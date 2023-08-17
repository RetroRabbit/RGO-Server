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

        public async Task<UserDto> UpdateUser(string email, ProfileDto updatedProfile)
        {
            User? existingUser = await _databaseContext.users
                .Include(u => u.Skills)
                .Include(u => u.Socials)
                .Include(u => u.UserProjects)
                .Include(u => u.UserCertifications)
                .FirstOrDefaultAsync(u => u.Email == email);

            if (existingUser == null)
            {
                throw new Exception("User not found");
            }

            existingUser.FirstName = updatedProfile.FirstName;
            existingUser.LastName = updatedProfile.LastName;
            existingUser.Email = updatedProfile.Email;
            existingUser.JoinDate = DateTime.UtcNow;
            existingUser.Status = updatedProfile.Status;
            existingUser.Bio = updatedProfile.Bio;
            existingUser.Level = updatedProfile.Level;
            existingUser.Phone = updatedProfile.Phone;

            existingUser.Skills.Clear();
            existingUser.Socials.Clear();
            existingUser.UserProjects.Clear();
            existingUser.UserCertifications.Clear();

            foreach (var updatedSkill in updatedProfile.Skills)
            {
                existingUser.Skills.Add(new Skill
                {
                    Id = updatedSkill.Id,
                    UserId = updatedSkill.UserId,
                    Title = updatedSkill.Title,
                    Description = updatedSkill.Description,
                });
            }
            foreach (var updatedSocial in updatedProfile.Socials)
            {
                existingUser.Socials.Add(new Social
                {
                    Id = updatedSocial.Id,
                    UserId = updatedSocial.UserId,
                    CodeWars = updatedSocial.CodeWars,
                    Discord = updatedSocial.Discord,
                    GitHub = updatedSocial.GitHub,
                    LinkedIn = updatedSocial.LinkedIn,
                });
            }
            foreach (var updatedUserProjects in updatedProfile.Projects)
            {
                existingUser.UserProjects.Add(new Projects
                {
                    Id=updatedUserProjects.Id, 
                    UserId = updatedUserProjects.UserId,
                    Name = updatedUserProjects.Name,
                    Description = updatedUserProjects.Description,
                    Role= updatedUserProjects.Role,
                });
            }
            foreach (var updatedUserCertifications in updatedProfile.Certifications)
            {
                existingUser.UserCertifications.Add(new Certifications
                {
                    Id = updatedUserCertifications.Id,
                    UserId = updatedUserCertifications.UserId,
                    Title= updatedUserCertifications.Title,
                    Description = updatedUserCertifications.Description,  
                });
            }
            _databaseContext.users.Update(existingUser);
            await _databaseContext.SaveChangesAsync();

            return existingUser.ToDTO();
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
                "EMPLOYEE" => 1,
                "PRESENTER" => 2,
                "MENTOR" => 3,
                _ => 4,
            };
        }

        public async Task<UserDto> RemoveUser(string email)
        {
            bool userExists = await UserExists(email);

            if (!userExists) throw new Exception("User already removed");

            User user = await _databaseContext.users.FirstAsync(u => u.Email == email);

            var removedUser = _databaseContext.users.Remove(user);
            await _databaseContext.SaveChangesAsync();

            return removedUser.Entity.ToDTO();

        }
    }
}