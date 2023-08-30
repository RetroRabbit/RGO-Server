using Microsoft.EntityFrameworkCore;
using RGO.Models;
using RGO.Services.Interfaces;
using RGO.UnitOfWork;
using RGO.UnitOfWork.Entities;


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

        public async Task<List<EmployeeAccessDto>> GetPropertiesWithAccess(string email)
        {
            var role = (await _employeeRoleService.GetEmployeeRoles(email))
                .Select(r => r.Role.Id)
                .ToList();

            /*            var query = from p in _db.PropertyAccess.Get()

                                    join mp in _db.MetaProperty.Get() on p.MetaPropertyId equals mp.Id into mp_join
                                    from mp in mp_join.DefaultIfEmpty()

                                    join fc in _db.FieldCode.Get() on p.FieldCodeId equals fc.Id into fc_join
                                    from fc in fc_join.DefaultIfEmpty()

                                    where role.Contains(p.Id)

                                    select new EmployeeAccessDto(new Dictionary<string, object>()
                                    {
                                        { "PropertyAccess" ,p.ToDto() },
                                        { "MetaProperty" ,mp !=null ? mp.ToDto() : null },
                                        { "FieldCode" ,fc != null ? fc.ToDto() : null }
                                    });*/

            var query = _db.PropertyAccess.Get(access => role.Contains(access.RoleId))
                .AsNoTracking()
                .Include(access => access.Role)
                .Include(access => access.FieldCode)
                .Include(access => access.MetaProperty)
                .Select(access => new EmployeeAccessDto(new Dictionary<string, object>()
                {
                    {"PropertyAccess", access },
                    {"FieldCode", access.FieldCodeId },
                    {"MetaPropery", access.MetaPropertyId },
                }))
                .ToList();


            return query;
        }

        public Task<RoleAccessDto> UpdatePropertiesWithAccess(EmployeeAccessDto Fields)
        {
            throw new NotImplementedException();
        }
    }
}
