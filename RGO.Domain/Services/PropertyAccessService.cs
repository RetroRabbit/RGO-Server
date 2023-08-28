using RGO.Models;
using RGO.Services.Interfaces;
using RGO.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RGO.Services.Services
{
    internal class PropertyAccessService : IPropertyAccessService
    {
        private readonly IUnitOfWork _db;

        public PropertyAccessService(IUnitOfWork db)
        {
            _db = db;
        }

        public Task<List<RoleAccessDto>> GetPropertiesWithAccess(string email)
        {
            throw new NotImplementedException();
        }

        public Task<RoleAccessDto> UpdatePropertiesWithAccess(EmployeeAccessDto Fields)
        {
            throw new NotImplementedException();
        }
    }
}
