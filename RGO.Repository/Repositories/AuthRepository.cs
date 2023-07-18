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

        public bool FindUserByEmail(string email)
        {
            User user = _databaseContext.users.Where(u => u.email == email).FirstOrDefault();
            if (user == null)
            {
                return false;
            }

            return true;
        }
    }
}
