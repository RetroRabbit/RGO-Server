using Microsoft.EntityFrameworkCore;
using RGO.Models;
using RGO.Services.Interfaces;
using RGO.UnitOfWork;
using RGO.UnitOfWork.Entities;
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
        private readonly IEmployeeRoleService _employeeRoleService; 

        public PropertyAccessService(IUnitOfWork db, IEmployeeRoleService employeeRoleService)
        {
            _db = db;
            _employeeRoleService = employeeRoleService;
        }

        public async Task<List<RoleAccessDto>> GetPropertiesWithAccess(string email)
        {
            var role = (await _employeeRoleService.GetEmployeeRole(email))
                .Select(r => r.Role.Id)
                .Tolist();

            var query = from p in _db.PropertyAccess.Get()
                        join mp in _db.MetaProperty.Get() on p.MetaPropertyId equals mp.Id into mp_join
                        from mp in mp_join.DefaultIfEmpty()

                        join fc in _db.FieldCode.Get() on p.FieldCodeId equals fc.Id into fc_join
                        from fc in fc_join.DefaultIfEmpty()
                        select new
                        {
                            PropertyAccess = p.ToDto(),
                            MetaProperty = mp !=null ? mp.ToDto() : null,
                            FieldCode = fc != null ? fc.ToDto() : null
                        };

            


        }

        public Task<RoleAccessDto> UpdatePropertiesWithAccess(EmployeeAccessDto Fields)
        {
            throw new NotImplementedException();
        }
    }
}
