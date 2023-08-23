using RGO.Models;

namespace RGO.Services.Interfaces;

public interface IEmployeeService
{
    Task<bool> CheckUserExist(string email);
    Task<List<EmployeeDto>> GetAll();
    Task<EmployeeDto> GetEmployee(string email);
    Task<EmployeeDto> SaveEmployee(EmployeeDto employeeDto);
    Task<EmployeeDto> UpdateEmployee(EmployeeDto employeeDto, string email);
    Task<EmployeeDto> DeleteEmployee(string email);
}
