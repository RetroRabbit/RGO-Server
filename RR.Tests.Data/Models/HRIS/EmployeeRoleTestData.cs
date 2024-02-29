using HRIS.Models;
using RR.UnitOfWork.Entities.HRIS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RR.Tests.Data.Models.HRIS;

public class EmployeeRoleTestData
{
    static public RoleDto RoleDtoEmployee = new RoleDto { Id = 2, Description = "Employee" };
    static public RoleDto RoleDtoAdmin = new RoleDto { Id = 3, Description = "Admin" };
    static public List<EmployeeRole> EmployeeRolesList = new List<EmployeeRole> {
        new EmployeeRole(
            new EmployeeRoleDto(1, EmployeeTestData.EmployeeDto, EmployeeRoleTestData.RoleDtoAdmin)
        )
    };
}