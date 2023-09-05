using Microsoft.EntityFrameworkCore;
using RGO.Models;
using RGO.Models.Update;
using RGO.Services.Interfaces;
using RGO.UnitOfWork;
using RGO.UnitOfWork.Entities;
using RGO.UnitOfWork.Interfaces;
using System.Data;
using Npgsql;


namespace RGO.Services.Services
{
    internal class PropertyAccessService : IPropertyAccessService
    {
        private readonly IUnitOfWork _db;
        private readonly IEmployeeRoleService _employeeRoleService;
        private readonly IEmployeeDataService _employeeDataService;

        public PropertyAccessService(IUnitOfWork db, IEmployeeRoleService employeeRoleService, IEmployeeDataService employeeDataService )
        {
            _db = db;
            _employeeRoleService = employeeRoleService;
            _employeeDataService = employeeDataService;

        }

        public async Task<List<EmployeeAccessDto>> GetPropertiesWithAccess(string email)
        {
            var employeeRoles = (await _employeeRoleService.GetEmployeeRoles(email))
                .Select(r => r.Role.Id)
                .ToList();


            var query = (await _db.PropertyAccess.Get(access => employeeRoles.Contains(access.RoleId))
                .AsNoTracking()
                .Include(access => access.Role)
                .Include(access => access.FieldCode)
                .Select(access => access.ToDto())
                .ToListAsync())
                .Where(access => access.Condition != 0)
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
            var employee = await _db.Employee.Get(e => e.Email == email)
               .Select(e => e.ToDto())
               .FirstOrDefaultAsync(); // ensure you get the first item or default

            if (employee == null)
            {
                throw new Exception("Employee not found.");
            }

            var employeeRoles = (await _employeeRoleService.GetEmployeeRoles(email))
                .Select(r => r.Role.Id)
                .ToList();

            bool canEdit = (await _db.PropertyAccess.Get(access => employeeRoles.Contains(access.RoleId))
                .AsNoTracking()
                .Include(access => access.Role)
                .Include(access => access.FieldCode)
                .Select(access => access.ToDto())
                .ToListAsync())
                .Where(access => access.Condition != 2).Any();

            if (!canEdit)
            {
                throw new Exception("No edit access");
            }

            foreach (var fieldValue in fields)
            {

                var field = await _db.FieldCode.GetById(fieldValue.fieldId);
                if (field.Internal)
                {
                    var table = field.InternalTable;
                    var employeeFilterByColumn = table == "Employee" ? "id" : "employeeId";
                    var query = $"UPDATE {field.InternalTable} SET {field.Name} = @value WHERE {employeeFilterByColumn} = @id";

                    await _db.RawSql(query,
                        new NpgsqlParameter("value", fieldValue.value),
                        new NpgsqlParameter("id", employee.Id));

                    

                    // TODO : Go to the table and saved the value in the selected table
                    // TODO : Check if row for employee exist in the internal table
                    // TODO : If it exist, update to new value
                    // TODO : else ...throw error
                    // TODO : We do not insert a new row for internal tables. We only do updates here.

                }
                else
                {
                    // TODO : Check if FieldCode is in EmployeeData already
                    var data = await _db.EmployeeData.Get(ed => ed.EmployeeId == employee.Id && ed.FieldCodeId == fieldValue.fieldId)
                                        .Select(ed => ed.ToDto())
                                        .FirstOrDefaultAsync();

                    if (data != null)
                    {
                        // TODO : If it is, update existing value
                        // Update existing value
                        var updateEmployeeData = new EmployeeDataDto(
                            data.Id,
                            data.Employee,
                            data.FieldCode,
                            fieldValue.value
                            );

                        await _employeeDataService.UpdateEmployeeData(updateEmployeeData);
                    }
                    else
                    {
                        // TODO : Else insert new row
                        //Create new EmployeeData record
                        var updateEmployeeData = new EmployeeDataDto(
                            0,
                            employee,
                            field,
                            fieldValue.value
                            );
                        await _employeeDataService.SaveEmployeeData(updateEmployeeData);
                    }
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

        public static object FindRepository(IUnitOfWork unitOfWork, string table)
        {

            var repository = typeof(IUnitOfWork)
            .GetProperties()
            .Where(property => property.Name.Contains(table, StringComparison.OrdinalIgnoreCase))
            .Select(property => property.GetValue(unitOfWork))
            .FirstOrDefault();
            if (repository == null)
            {
                throw new Exception("table not found");
            }
            return repository;
        }
    }
}
