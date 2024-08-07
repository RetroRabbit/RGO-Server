﻿using HRIS.Models;
using HRIS.Models.Enums;
using HRIS.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using RR.UnitOfWork;
using RR.UnitOfWork.Entities.HRIS;

namespace HRIS.Services.Services;

public class PropertyAccessService : IPropertyAccessService
{
    private readonly IUnitOfWork _db;

    public PropertyAccessService(IUnitOfWork db)
    {
        _db = db;
    }

    public async Task<List<PropertyAccessDto>> GetAccessListByEmployeeId(int employeeId)
    {
        var employeeRole = _db.EmployeeRole.Get(e => e.Id == employeeId).Select(e => e.Role).FirstOrDefault();

        if (employeeRole == null) 
            return new List<PropertyAccessDto>();
        else
            return await GetAccessListByRoleId(employeeRole.Id);
    }

    public async Task<List<PropertyAccessDto>> GetAccessListByRoleId(int roleId)
    {
        return (await _db.PropertyAccess.GetAll(p => p.RoleId == roleId)).Select(x => x.ToDto()).ToList();
    }

    public async Task<List<PropertyAccessDto>> GetAll()
    {
        return await _db.PropertyAccess.Get().Include(p => p.Role).Select(p => p.ToDto()).ToListAsync();
    }

    public async Task UpdatePropertyAccess(int propertyId, PropertyAccessLevel propertyAccess)
    {
        var updatedProperty = _db.PropertyAccess.Get(p => p.Id == propertyId).FirstOrDefault();
        if (updatedProperty != null)
        {
            updatedProperty.AccessLevel = propertyAccess;
            _ = await _db.PropertyAccess.Update(updatedProperty);
        }
    }

    public async Task CreatePropertyAccessEntries()
    {
        var currentAccessProperties = await _db.PropertyAccess.GetAll();
        var tables = new List<string> { "Employee", "EmployeeData", "EmployeeRole", "EmployeeAddress", "EmployeeBanking" };
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
                        properties.Add(new PropertyAccess(propertyAccess));
                    }
                }
            }
        }));
        await _db.PropertyAccess.AddRange(properties);
    }
}