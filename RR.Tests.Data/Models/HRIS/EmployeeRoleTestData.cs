using HRIS.Models;
using RR.UnitOfWork.Entities.HRIS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RR.Tests.Data.Models.HRIS;

public class EmployeeRoleTestData
{
    static public RoleDto RoleDtoEmployee = new RoleDto(3, "Employee");
    static public RoleDto RoleDtoAdmin = new RoleDto(2, "Admin");
    static public List<EmployeeRole> EmployeeRolesList = new List<EmployeeRole> {
            new EmployeeRole(
                new EmployeeRoleDto(1, EmployeeTestData.EmployeeDto, EmployeeRoleTestData.RoleDtoAdmin)
            )};
}