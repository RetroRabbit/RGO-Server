using RGO.Models;

namespace RGO.Services.Interfaces;

public interface IEmployeeDateService
{
    Task<bool> CheckIfExists(EmployeeDateDto employeeDate);
    Task Save(EmployeeDateDto employeeDate);
    Task Delete(int employeeDateId);
    Task Update(EmployeeDateDto newEmployeeDate);
    Task<EmployeeDateDto> Get(EmployeeDateDto employeeDate);
    List<EmployeeDateDto> GetAll();
    List<EmployeeDateDto> GetAllByEmployee(string email);
    List<EmployeeDateDto> GetAllBySubject(string subject);
    List<EmployeeDateDto> GetAllByDate(DateOnly Date);
}
