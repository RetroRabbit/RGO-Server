using Microsoft.EntityFrameworkCore;
using RGO.Models;
using RGO.Models.Update;
using RGO.Services.Interfaces;
using RGO.UnitOfWork;
using System.Data;
using Npgsql;

namespace RGO.Services.Services
{
    public class PropertyAccessService : IPropertyAccessService
    {
        private readonly IUnitOfWork _db;
        private readonly IEmployeeRoleService _employeeRoleService;
        private readonly IEmployeeDataService _employeeDataService;
        private readonly IEmployeeService _employeeService;
        public PropertyAccessService(IUnitOfWork db, IEmployeeRoleService employeeRoleService, IEmployeeDataService employeeDataService, IEmployeeService employeeService)
        {
            _db = db;
            _employeeRoleService = employeeRoleService;
            _employeeDataService = employeeDataService;
            _employeeService = employeeService;
        }

        public async Task<List<EmployeeAccessDto>> GetPropertiesWithAccess(string email)
        {
            var employee = await _employeeService.GetEmployee(email);
            var properties = await _db.PropertyAccess.GetForEmployee(email);

            var result = new List<EmployeeAccessDto>();

            foreach (var access in properties.Where(a => a.Condition != 0))
            {
                string value = await GetSqlValues(access.FieldCode,  employee);
                  
                var dto = new EmployeeAccessDto(
                    access.FieldCode.Id,
                    access.Condition,
                    access.FieldCode.Internal,
                    access.FieldCode.Code,
                    access.FieldCode.Name,
                    access.FieldCode.Type.ToString().ToLower(),
                    value,
                    access.FieldCode.Description,
                    access.FieldCode.Regex,
                    access.FieldCode.Options?.Select(x => x.Option).ToList() ?? null
                );

                result.Add(dto);
            }
            return result;
        }

        public async Task UpdatePropertiesWithAccess(List<UpdateFieldValueDto> fields, string email)
        {
            var employee = await _db.Employee.Get(e => e.Email == email)
               .Select(e => e.ToDto())
               .FirstOrDefaultAsync();

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

                    switch (field.Type)
                    {
                        case Models.Enums.FieldCodeType.Date:
                            valueParam = new NpgsqlParameter("value", DateOnly.Parse(fieldValue.value.ToString()));
                            break;

                        case Models.Enums.FieldCodeType.String:
                            valueParam = new NpgsqlParameter("value", fieldValue.value.ToString());
                            break;

                        case Models.Enums.FieldCodeType.Int:
                            int.TryParse(fieldValue.value.ToString(), out int intValue);
                            valueParam = new NpgsqlParameter("value", intValue);
                            break;

                        case Models.Enums.FieldCodeType.Float:
                            valueParam = new NpgsqlParameter("value", float.Parse(fieldValue.value.ToString()));
                            break;

                        default:
                            throw new Exception("Format Invalid");
                    }

                    var idParam = new NpgsqlParameter("id", employee.Id);
                    await _db.RawSql(query, valueParam, idParam);

                }
                else
                {
                    var data = await _db.EmployeeData.Get(ed => ed.EmployeeId == employee.Id && ed.FieldCodeId == fieldValue.fieldId)
                                        .Select(ed => ed.ToDto())
                                        .FirstOrDefaultAsync();

                    if (data != null)
                    {
                        var updateEmployeeData = new EmployeeDataDto(
                            data.Id,
                            data.EmployeeId,
                            data.FieldCodeId,
                            fieldValue.value.ToString()
                            );

                        await _employeeDataService.UpdateEmployeeData(updateEmployeeData);
                    }
                    else
                    {
                        var updateEmployeeData = new EmployeeDataDto(
                            0,
                            employee.Id,
                            field.Id,
                            fieldValue.value.ToString()
                            );
                        await _employeeDataService.SaveEmployeeData(updateEmployeeData);
                    }
                }
            }
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

        public async Task<string> GetSqlValues(FieldCodeDto fieldCode, EmployeeDto employee)
        {
            var employeeFilterByColumn = fieldCode.InternalTable == "Employee" ? "id" : "employeeId";

            if (fieldCode.Internal)
            {
                return await _db.RawSqlGet($"SELECT \"{fieldCode.Code}\" FROM \"{fieldCode.InternalTable}\" WHERE {employeeFilterByColumn} = {employee.Id}");
            }
            else
            {
                return await _db.RawSqlGet($"SELECT \"value\" FROM \"EmployeeData\" WHERE \"employeeId\" = {employee.Id} AND \"fieldCodeId\" = {fieldCode.Id}");
            }
        }
    }
}
