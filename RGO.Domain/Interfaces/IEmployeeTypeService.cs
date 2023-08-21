using RGO.Models;

namespace RGO.Services.Interfaces;

public interface IEmployeeTypeService
{
    Task<EmployeeTypeDto> AddEmployeeType(EmployeeTypeDto employeeTypeDto);
    Task<EmployeeTypeDto> DeleteEmployeeType(string name);
    Task<EmployeeTypeDto> GetEmployeeType(string name);
    Task<List<EmployeeTypeDto>> GetAllEmployeeType();
    Task<EmployeeTypeDto> UpdateEmployeeType(EmployeeTypeDto employeeTypeDto);
}
