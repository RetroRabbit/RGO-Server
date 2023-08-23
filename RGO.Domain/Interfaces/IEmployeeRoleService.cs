using RGO.Models;

namespace RGO.Services.Interfaces;

public interface IEmployeeRoleService
{
    Task<EmployeeRoleDto> SaveEmployeeRole(EmployeeRoleDto employeeRoleDto);
    Task<EmployeeRoleDto> DeleteEmployeeRole(string name);
    Task<EmployeeRoleDto> GetEmployeeRole(string name);
    Task<List<EmployeeRoleDto>> GetAllEmployeeRoles();
    Task<EmployeeRoleDto> UpdateEmployeeRole(EmployeeRoleDto employeeRoleDto);
}
