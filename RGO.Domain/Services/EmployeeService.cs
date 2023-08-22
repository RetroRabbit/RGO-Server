using Microsoft.EntityFrameworkCore;
using RGO.Models;
using RGO.Services.Interfaces;
using RGO.UnitOfWork;
using RGO.UnitOfWork.Entities;
using RGO.UnitOfWork.Interfaces;

namespace RGO.Services.Services;

public class EmployeeService : IEmployeeService
{
    private readonly IUnitOfWork _db;

    public EmployeeService(IUnitOfWork db) 
    {
        _db = db;
    }

    public async Task<EmployeeDto> GetEmployee(string email)
    {
        var employee = await _db.Employee
            .Get(employee => employee.PersonalEmail == email)
            .AsNoTracking()
            .Include(employee => employee.EmployeeType)
            .Select(employee => employee.ToDto())
            .Take(1)
            .FirstOrDefaultAsync();

        if (employee == null) { throw new Exception("User not found"); }

        return employee;
    }

    public async Task<List<EmployeeDto>> GetAllEmployees()
    {
        return await _db.Employee.GetAll();
    }

    public async Task UpdateEmployee(EmployeeDto employeeDto)
    {
        var ifEmployee = await CheckEmployee(employeeDto.Id);
        if (!ifEmployee) { throw new Exception("Employee not found"); }

        Employee employee = new Employee(employeeDto);
        await _db.Employee.Update(employee);
    }

    public async Task DeleteEmployee(EmployeeDto employeeDto)
    {
        var ifEmployee = await CheckEmployee(employeeDto.Id);

        if (!ifEmployee) { throw new Exception("Employee not found"); }

        Employee employee = new Employee(employeeDto);
        await _db.Employee.Delete(employee.Id);
    }

    private async Task<bool> CheckEmployee(int employeeId)
    {
        var employee = await _db.Employee
        .Get(employee => employee.Id == employeeId)
        .FirstOrDefaultAsync();

        if (employee == null) { return false; }
        else { return true; }
    }
}
