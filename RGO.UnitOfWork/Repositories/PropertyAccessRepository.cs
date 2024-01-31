using Microsoft.EntityFrameworkCore;
using RGO.Models;
using RGO.UnitOfWork.Entities;
using RGO.UnitOfWork.Interfaces;

namespace RGO.UnitOfWork.Repositories
{
    public class PropertyAccessRepository : BaseRepository< PropertyAccess,PropertyAccessDto>, IPropertyAccessRepository
    {
        private readonly DatabaseContext _db;
        public PropertyAccessRepository(DatabaseContext db): base(db)
        {
            _db = db;
        }

        public async Task<List<PropertyAccessDto>> GetForEmployee(string email)
        {
            var employeeRoles = await _db.employeeRoles
                .Where(employeeRole => employeeRole.Employee.Email == email)
                .Select(x => x.RoleId)
                .ToListAsync();

            var list = await _db.propertyAccesses
                .Where(access => employeeRoles.Contains(access.RoleId) && access.Condition != 0)
                .AsNoTracking()
                .Include(access => access.Role)
                .Include(access => access.FieldCode)
                .Select(access => access.ToDto())
                .ToListAsync();

            foreach (var access in list)
            {
                access.FieldCode.Options = await _db.fieldCodesOptions
                .Where(options => options.FieldCodeId == access.FieldCode.Id)
                .Select(options => options.ToDto())
                .ToListAsync();
            }

            return list;
        }
    }
}
