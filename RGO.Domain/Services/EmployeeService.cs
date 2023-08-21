using Microsoft.EntityFrameworkCore;
using RGO.Models;
using RGO.Services.Interfaces;
using RGO.UnitOfWork.Interfaces;

namespace RGO.Services.Services;

public class EmployeeService : IEmployeeService
{
    private readonly IEmployeeRepository _employeeRepository;

    public EmployeeService(IEmployeeRepository employeeRepository) 
    {
        _employeeRepository = employeeRepository;
    }

    public async Task<EmployeeDto> GetEmployee(string email)
    {
        var employee = await _employeeRepository
            .Get(employee => employee.PersonalEmail == email)
            .Include(employee => employee.EmployeeType)
            .Select(employee => employee.ToDto())
            .Take(1)
            .FirstOrDefaultAsync();

        if (employee == null) { throw new Exception("User not found"); }

        return employee;
    }
}
