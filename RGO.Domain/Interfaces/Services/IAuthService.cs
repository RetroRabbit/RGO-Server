using RGO.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace RGO.Domain.Interfaces.Services
{
    public interface IAuthService
    {
        public bool CheckUserExist(UserDto user);
        public string GenerateToken();
    }
}
