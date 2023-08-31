using Microsoft.EntityFrameworkCore;
using RGO.Models;
using RGO.Models.Update;
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

            var testvar = _db.PropertyAccess.Get(access => role.Contains(access.RoleId));

            var query = (await testvar
                .AsNoTracking()
                .Include(access => access.Role)
                .Include(access => access.FieldCode)
                .Select(access => access.ToDto())
                .ToListAsync())
                .Where(access => access.Condition!=0)
                .Select(access => new EmployeeAccessDto(
                    access.Id,
                    access.Condition,
                    access.FieldCode.Internal,
                    access.FieldCode.Code,
                    access.FieldCode.Name,
                    access.FieldCode.Type.ToString().ToLower(),
                    access.FieldCode.Description,
                    access.FieldCode.Regex,
                    this.PassOptions(access)
                ))
                .ToList();

            return query;
        }

        public async Task UpdatePropertiesWithAccess(List<UpdateFieldValueDto> fields, string email)
        {
            var employee = new Employee();// TODO : Get Employee and roles to validate if we can edit the field

            foreach (var fieldValue in fields)
            {
                var field = await _db.FieldCode.GetById(fieldValue.fieldId);
                if (field.Internal)
                {
                    // TODO : Go to the table and saved the value in the selected table
                    // TODO : Check if row for employee exist in the internal table
                    // TODO : If it exist, update to new value
                    // TODO : else ...throw error
                    // TODO : We do not insert a new row for internal tables. We only do updates here.
                }
                else
                {
                    // TODO : Check if FieldCode is in EmployeeData already
                    // TODO : If it is, update existing value
                    // TODO : Else insert new row
                }
            }
        }

        private List<string> PassOptions(PropertyAccessDto access)
        {
            return _db.FieldCodeOptions
                .Get(options => options.FieldCodeId == access.FieldCode.Id)
                .Select(options => options.Option)
                .ToList();
        }
    }
}
