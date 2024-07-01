using RR.UnitOfWork.Entities.HRIS;

namespace RR.Tests.Data.Models.HRIS;

public class EmployeeRoleTestData
{
    public static Role RoleDtoEmployee = new() { Id = 2, Description = "Employee" };
    public static Role RoleDtoAdmin = new() { Id = 3, Description = "Admin" };
    public static List<EmployeeRole> EmployeeRolesList = new()
    {
        new()
        {
            Id = 1,
            EmployeeId = EmployeeTestData.EmployeeOne.Id,
            Employee = EmployeeTestData.EmployeeOne,
            RoleId = EmployeeRoleTestData.RoleDtoAdmin.Id,
            Role = EmployeeRoleTestData.RoleDtoAdmin,
        }
    };
    public static EmployeeRole EmployeeRoleDto = new()
    {
        Id = 1,
        Employee = EmployeeTestData.EmployeeOne,
        Role = EmployeeRoleTestData.RoleDtoAdmin
    };
}
