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

      /*  public async Task<List<EmployeeAccessDto>> GetPropertiesWithAccess(string email)
        {
            var employee = await _db.Employee.Get(e => e.Email == email)
               .Select(e => e.ToDto())
               .FirstOrDefaultAsync();

            var employeeRoles = (await _employeeRoleService.GetEmployeeRoles(email))
                .Select(r => r.Role.Id)
                .ToList();

            var propertiesWithAccess =( await _db.PropertyAccess.Get(access => employeeRoles.Contains(access.RoleId))
                  .AsNoTracking()
                 .Include(access => access.Role)
                 .Include(access => access.FieldCode)
                 .Select(access => access.ToDto())
                 .ToListAsync());

*//*                .AsNoTracking()
                .Include(access => access.Role)
                .Include(access => access.FieldCode)
                .ToListAsync();*//*



            var results = new List<EmployeeAccessDto>();

            foreach (var access in propertiesWithAccess)
            {
                string? tableName = access.FieldCode.InternalTable;
                string query;
                var value; 


                if (!access.FieldCode.Internal)
                {
                    query = $"SELECT {access.FieldCode.Name} FROM EmployeeData WHERE fieldcodeid = @fieldcodeid && employeeId == @employeeId ";
                    value = (await _db.RawSql(query, 
                        new NpgsqlParameter("fieldcodeid", access.FieldCode.Id),
                        new NpgsqlParameter("employeeId", employee.Id)).FirstOrDefaultAsync();

                }
                else 
                {
                    if (tableName == "Employee")
                    {
                        query = $"SELECT {access.FieldCode.Name} FROM {tableName} WHERE Id = @condition";
                    }
                    else
                    {
                        query = $"SELECT {access.FieldCode.Name} FROM {tableName} WHERE employeeId = @condition";
                    }
                }




                if (access.Condition != 0)
                {
                    var dto = new EmployeeAccessDto(
                        access.Id,
                        access.Condition,
                        access.FieldCode.Internal,
                        access.FieldCode.Code,
                        access.FieldCode.Name,
                        access.FieldCode.Type.ToString().ToLower(),
                        value,  // Use the value retrieved from the above query
                        access.FieldCode.Description,
                        access.FieldCode.Regex,
                        this.PassOptions(access)
                    );
                    results.Add(dto);
                }
            }

            return results;
        }*/

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
                     access.FieldCode.Id,
                     access.Condition,
                     access.FieldCode.Internal,
                     access.FieldCode.Code,
                     access.FieldCode.Name,
                     access.FieldCode.Type.ToString().ToLower(),
                     "asd",
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
                    var query = $"UPDATE \"{field.InternalTable}\" SET \"{field.Code}\" = @value WHERE {employeeFilterByColumn} = @id";
                    NpgsqlParameter valueParam;

                    if (int.TryParse(fieldValue.value.ToString(), out int intValue) && !ShouldParse(field.Code))
                    {
                        valueParam = new NpgsqlParameter("value", intValue);
                    }
                    else
                    {
                        valueParam = new NpgsqlParameter("value", fieldValue.value.ToString());
                    }

                    
                    var idParam = new NpgsqlParameter("id", employee.Id);
                    await _db.RawSql(query, valueParam, idParam);

                    /*                    await _db.RawSql(query);
                                            new NpgsqlParameter("value", fieldValue.value);
                                            new NpgsqlParameter("id", employee.Id);*/

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
                            fieldValue.value.ToString()
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
                            fieldValue.value.ToString()
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

        public static bool ShouldParse(string code)
        {
            string[] notAllowedStrings = {
                "employeeNumber",
                "taxNumber",
                "idNumber",
                "passportNumber",
                "cellphoneNo",
                "unitNumber",
                "streetNumber",
                "postalCode",
                "accountNo"
            };

            bool allowed = notAllowedStrings.Contains(code.Trim());

            return allowed;
        }
    }
}
