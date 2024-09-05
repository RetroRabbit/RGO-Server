using HRIS.Models;
using HRIS.Models.Enums;
using HRIS.Services.Interfaces;
using HRIS.Services.Session;
using Microsoft.EntityFrameworkCore;
using RR.UnitOfWork;
using RR.UnitOfWork.Entities.HRIS;

namespace HRIS.Services.Services;

public class PropertyAccessService : IPropertyAccessService
{
    private readonly IUnitOfWork _db;
    private readonly AuthorizeIdentity _identity;

    public PropertyAccessService(IUnitOfWork db, AuthorizeIdentity identity)
    {
        _db = db;
        _identity = identity;
    }

    public async Task<List<PropertyAccessDto>> GetAccessListByEmployeeId(int employeeId)
    {
        var exists = await CheckEmployee(employeeId);

        if (!exists)
            throw new CustomException("Employee Not Found");

        if (!_identity.IsSupport && employeeId != _identity.EmployeeId)
            throw new CustomException("Unauthorized Access.");

        var employeeRole = _db.EmployeeRole.Get(e => e.Id == employeeId).Select(e => e.Role).FirstOrDefault();

        return await GetAccessListByRoleId(employeeRole.Id);
    }

    public async Task<List<PropertyAccessDto>> GetAccessListByRoleId(int roleId)
    {
        var exists = await CheckRoleById(roleId);

        if (!exists)
            throw new CustomException("Role Not Found");

        return (await _db.PropertyAccess.GetAll(p => p.RoleId == roleId)).Select(x => x.ToDto()).ToList();
    }

    public async Task<List<PropertyAccessDto>> GetAll()
    {
        if (_identity.IsSupport == false)
            throw new CustomException("Unauthorized Access.");

        return await _db.PropertyAccess.Get().Include(p => p.Role).Select(p => p.ToDto()).ToListAsync();
    }

    public async Task UpdatePropertyAccess(int propertyId, PropertyAccessLevel propertyAccess)
    {
        var exists = await CheckPropertyAccess(propertyId);

        if (!exists)
            throw new CustomException("Property Access Not Found");

        if (_identity.IsSupport == false)
            throw new CustomException("Unauthorized Access.");

        var updatedProperty = _db.PropertyAccess.Get(p => p.Id == propertyId).FirstOrDefault();
        if (updatedProperty != null)
        {
            updatedProperty.AccessLevel = propertyAccess;
            _ = await _db.PropertyAccess.Update(updatedProperty);
        }
    }

    public async Task<List<PropertyAccess>> CreatePropertyAccessEntries()
    {
        if (_identity.IsSupport == false)
            throw new CustomException("Unauthorized Access.");

        var currentAccessProperties = await _db.PropertyAccess.GetAll();
        var tables = new List<string> { "Employee", "EmployeeData", "EmployeeRole", "EmployeeAddress", "EmployeeBanking", "EmployeeQualification", "EmployeeSalaryDetails" };
        var roles = await _db.Role.GetAll();
        var properties = new List<PropertyAccess>();

        await Task.WhenAll(tables.Select(async table =>
        {
            var columns = await _db.GetColumnNames(table);

            foreach (var role in roles)
            {
                foreach (var column in columns)
                {
                    var exists = currentAccessProperties.Exists(p => p.Table == table && p.Field == column && p.RoleId == role.Id);
                    if (!exists)
                    {
                        var propertyAccess = new PropertyAccessDto
                        {
                            Id = 0,
                            Role = role.ToDto(),
                            Table = table,
                            Field = column,
                            AccessLevel = PropertyAccessLevel.write
                        };
                        if (column == "email")
                        {
                            propertyAccess.AccessLevel = PropertyAccessLevel.read;
                        }
                        properties.Add(new PropertyAccess(propertyAccess));
                    }
                }
            }
        }));

        await _db.PropertyAccess.AddRange(properties);

        return properties;
    }

    public async Task<bool> CheckEmployee(int employeeId)
    {
        return await _db.Employee.Any(x => x.Id == employeeId);
    }

    public Task<bool> CheckRoleById(int roleId)
    {
        return _db.Role.Any(role => role.Id == roleId);
    }

    public Task<bool> CheckPropertyAccess(int id)
    {
        return _db.PropertyAccess.Any(pa => pa.Id == id);
    }
}