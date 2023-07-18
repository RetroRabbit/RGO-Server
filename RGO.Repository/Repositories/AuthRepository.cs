using Microsoft.EntityFrameworkCore;
using RGO.Domain.Interfaces.Repository;
using RGO.Repository.Entities;

namespace RGO.Repository.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DatabaseContext _databaseContext;

        public AuthRepository(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task<bool> FindUserByEmail(string email)
        {
            return await _databaseContext.users.AnyAsync(u => u.email == email);
        }
    }
}
