using RGO.Models;

namespace RGO.Services.Interfaces;

public interface IEmployeeProjectService
{
    Task<EmployeeProjectDto> SaveEmployeeProject(EmployeeProjectDto employeeProjectDto);
    Task<EmployeeProjectDto> DeleteEmployeeProject(string name);
    Task<EmployeeProjectDto> GetEmployeeProject(string name);
    Task<List<EmployeeProjectDto>> GetAllEmployeeProjects();
    Task<EmployeeProjectDto> UpdateEmployeeProject(EmployeeProjectDto employeeProjectDto);
}
