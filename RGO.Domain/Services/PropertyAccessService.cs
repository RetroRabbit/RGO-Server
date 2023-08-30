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

            var propertyAccessQuery = _db.PropertyAccess.Get(access => role.Contains(access.RoleId))
                .AsNoTracking()
                .Include(access => access.Role)
                .Include(access => access.FieldCode)
                .Include(access => access.MetaProperty)
                .Select(access => access.ToDto())
                .Select(access => new EmployeeAccessDto(
                    access.Id,
                    access.Condition,
                    access.MetaPropertyId == null ? 0 : 1, // 0 is custom 1 is meta
                    access.FieldCodeId == null ? access.FieldCodeId.Code : access.metaField,
                    access.FieldCodeId == null ? access.FieldCodeId.Name : access.metaField,
                    "text",//ToDo
                    access.FieldCodeId != null ? access.FieldCodeId.Description : null,
                    access.FieldCodeId != null ? access.FieldCodeId.Regex : null,

                    this.PassOptions(access, "text")
                    ))
                .ToList();

            /*var metaFieldOptionsQuery = _db.MetaPropertyOptions.Get(options => )*/


            return propertyAccessQuery;
        }

        public Task<RoleAccessDto> UpdatePropertiesWithAccess(EmployeeAccessDto Fields)
        {
            throw new NotImplementedException();
        }

        private static List<string> PassOptions(PropertyAccessDto access, string type)
        {
            if (type.Equals("dropdown",StringComparison.CurrentCultureIgnoreCase))
            {
                return null;
            }

            if (access.FieldCodeId != null)
                return _db.FieldCodeOptions.Get(options => options.FieldCodeId == access.FieldCodeId.Id)
                    .Select(options => options.Option).ToList();

            return _db.MetaPropertyOptions.Get(options => options.MetaPropertyId == access.MetaPropertyId.Id)
                    .Select(options => options.Option).ToList();
        }
    }
}
