using RGO.Models;

namespace RGO.Services.Interfaces;

public interface IEmployeeRoleService
{
    Task<EmployeeRoleDto> AddEmployeeRole(EmployeeRoleDto employeeRoleDto);
    Task<EmployeeRoleDto> DeleteEmployeeRole(int id);
    Task<EmployeeRoleDto> GetEmployeeRole(string name);
    Task<List<EmployeeRoleDto>> GetAllEmployeeRoles();
    Task<EmployeeRoleDto> UpdateEmployeeRole(EmployeeRoleDto employeeRoleDto);
}
