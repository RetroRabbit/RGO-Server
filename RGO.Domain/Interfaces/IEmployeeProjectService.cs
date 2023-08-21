using RGO.Models;

namespace RGO.Services.Interfaces;

public interface IEmployeeProjectService
{
    Task<EmployeeProjectDto> AddEmployeeProject(EmployeeProjectDto employeeProjectDto);
    Task<EmployeeProjectDto> DeleteEmployeeProject(int id);
    Task<EmployeeProjectDto> GetEmployeeProject(string name);
    Task<List<EmployeeProjectDto>> GetAllEmployeeProjects();
    Task<EmployeeProjectDto> UpdateEmployeeProject(EmployeeProjectDto employeeProjectDto);
}
