using Microsoft.EntityFrameworkCore;
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

        public async Task<UserDto> GetUserByEmail(string email)
        {
            bool userExists = await UserExists(email);

            if (!userExists) throw new Exception("User not found");

            User user = await _databaseContext.users.Include(user => user.skills).Include(user => user.projects).Include(user => user.certifications).Include(user => user.social).FirstAsync(u => u.email == email);

            return user.ToDTO();
        }

        public async Task<bool> UserExists(string email)
        {
            bool userExists = await _databaseContext.users.AnyAsync(u => u.email == email);

            return userExists;
        }
    }
}
