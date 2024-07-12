using HRIS.Models;

namespace HRIS.Services.Interfaces;

public interface IEmployeeDateService
{
    Task<bool> CheckIfExists(EmployeeDateDto employeeDate);
    Task Delete(int employeeDateId);
    Task<EmployeeDateDto> Get(EmployeeDateDto employeeDate);
    List<EmployeeDateDto> GetAll();
    List<EmployeeDateDto> GetAllByEmployee(string email);
    List<EmployeeDateDto> GetAllBySubject(string subject);
    List<EmployeeDateDto> GetAllByDate(DateOnly Date);
    Task UpdateEmployeeDate(EmployeeDateDto employeeDate);
    Task SaveEmployeeDate(EmployeeDateInput employeeDateInput);
    List<EmployeeDateDto> GetEmployeeDates(DateOnly? date, string? email, string? subject);
}