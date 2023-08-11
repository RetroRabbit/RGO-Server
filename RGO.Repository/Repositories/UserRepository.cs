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
                .FirstOrDefaultAsync(u => u.Email == email);

            if (existingUser == null)
                throw new Exception("User not found");

            existingUser.FirstName = updatedProfile.FirstName;
            existingUser.LastName = updatedProfile.LastName;
            existingUser.Email = updatedProfile.Email;
            existingUser.Type = updatedProfile.Type;
            existingUser.JoinDate = DateTime.UtcNow;
            existingUser.Status = updatedProfile.Status;
            existingUser.Bio = updatedProfile.Bio;
            existingUser.Level = updatedProfile.Level;
            existingUser.Phone = updatedProfile.Phone;

            var currUser = _databaseContext.users.Update(existingUser);

            List<Skill> skills = await _databaseContext.skill
                .Where(s => s.UserId == existingUser.Id)
                .ToListAsync();
            foreach (Skill skill in skills)
            {
                bool skillExists = updatedProfile.Skills.Any(s =>
                s.Id == skill.Id &&
                s.Title == skill.Title &&
                s.Description == skill.Description &&
                s.UserId == existingUser.Id);

                if (!skillExists)
                {
                    _databaseContext.skill.Update(skill);
                }
                // await _databaseContext.SaveChangesAsync();
            }

            List<Social> socials = await _databaseContext.social
                .Where(s => s.UserId == existingUser.Id)
                .ToListAsync();

            foreach (Social social in socials)
            {
                bool socialExists = updatedProfile.Socials.Any(s =>
                    s.Id == social.Id
                    && s.CodeWars == social.CodeWars
                    && s.Discord == social.Discord
                    && s.GitHub == social.GitHub
                    && s.LinkedIn == social.LinkedIn
                    && s.UserId == existingUser.Id);

                if (!socialExists)
                {
                    _databaseContext.social.Update(social);
                    // await _databaseContext.SaveChangesAsync();
                }
            }

            List<Certifications> certifications = await _databaseContext.certifications
                .Where(c => c.UserId == existingUser.Id)
                .ToListAsync();

            foreach (Certifications certification in certifications)
            {
                bool certificationExists = updatedProfile.Certifications.Any(c =>
                    c.Id == certification.Id
                    && c.Title == certification.Title
                    && c.Description == certification.Description
                    && c.UserId == existingUser.Id);

                if (!certificationExists)
                {
                    _databaseContext.certifications.Update(certification);
                    //await _databaseContext.SaveChangesAsync();
                }
            }

            List<Projects> projects = await _databaseContext.projects
                .Where(p => p.UserId == existingUser.Id)
                .ToListAsync();
            foreach (Projects project in projects)
            {
                  bool projectExists = updatedProfile.Projects.Any(p =>
                    p.Id == project.Id
                    && p.Name == project.Name
                    && p.Role == project.Role
                    && p.Description == project.Description
                    && p.UserId == existingUser.Id);

                if (!projectExists)
                {
                    _databaseContext.projects.Update(project);
                    // await _databaseContext.SaveChangesAsync();
                }
            }

            await _databaseContext.SaveChangesAsync();

            return currUser.Entity.ToDTO();
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
