using Microsoft.EntityFrameworkCore;
using RGO.Models;
using RGO.Models.Update;
using RGO.Services.Interfaces;
using RGO.UnitOfWork;
using RGO.UnitOfWork.Entities;
using RGO.UnitOfWork.Interfaces;
using System.Data;


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






                    /*                var field = await _db.FieldCode.GetById(fieldValue.fieldId);
                                    if (field.Internal)
                                    {
                                        switch (field.InternalTable)
                                        {
                                            case "Employee":


                                                break;

                                            case "":
                                                break;

                                            default:
                                                break;
                                        }*/
                    /*var repository = FindRepository(_db, field.InternalTable); // assuming field.TableName gives you the name of the table

                    if (repository == null)
                    {
                        throw new Exception($"Repository for {field.InternalTable} not found.");
                    }

                    // Assuming the repository has a method named 'GetByEmployeeId' 
                    // (You may need to cast the repository to its specific type or create a common interface)
                    var data = await (repository as IYourRepositoryType).GetByEmployeeId(employee.Id);

                    if (data != null)
                    {
                        // Update the field value in the selected table
                        data.FieldValue = fieldValue.NewValue; // Assuming 'NewValue' is a property in UpdateFieldValueDto that holds the new value
                        await _db.SaveChangesAsync();
                    }
                    else
                    {
                        // Error because we only do updates
                        throw new Exception("No existing data found for the employee in the internal table.");
                    }
                }
                else
                {
                    var data = await _db.EmployeeData.FirstOrDefaultAsync(ed => ed.EmployeeId == employee.Id && ed.FieldCodeId == fieldValue.fieldId);

                    if (data != null)
                    {
                        // Update existing value
                        data.FieldValue = fieldValue.NewValue;
                    }
                    else
                    {
                        // Insert new row
                        var newData = new EmployeeData
                        {
                            EmployeeId = employee.Id,
                            FieldCodeId = fieldValue.fieldId,
                            FieldValue = fieldValue.NewValue
                        };
              + /* var employee = _db.Employee.Get(e => e.Email == email)
                         .Select(e=>e.ToDto());
                     if (employee == null)
                     {
                         throw new Exception("Employee not found.");
                     }

                     var employeeRoles = (await _employeeRoleService.GetEmployeeRoles(email))
                         .Select(r => r.Role.Id)
                         .ToList(); // TODO : Get Employee and roles to validate if we can edit the field

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
                     {*/
                    /*
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
                                      */  // TODO : Else insert new row
                                          //}
                                          //}
                                          //}
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
