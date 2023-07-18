using RGO.Domain.Interfaces.Repository;
using RGO.Domain.Models;
using RGO.Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
