using Microsoft.EntityFrameworkCore;
using RGO.Models;
using RGO.Services.Interfaces;
using RGO.UnitOfWork;
using RGO.UnitOfWork.Entities;

namespace RGO.Services.Services;

public class EmployeeService : IEmployeeService
{
    private readonly IEmployeeTypeService _employeeTypeService;
    private readonly IUnitOfWork _db;

    public EmployeeService(IEmployeeTypeService employeeTypeService, IUnitOfWork db)
    {
        _employeeTypeService = employeeTypeService;
        _db = db;
    }

    public async Task<EmployeeDto> SaveEmployee(EmployeeDto employeeDto)
    {
        Employee employee;

        try
        {
            var ExistingEmployeeType = await _employeeTypeService
                .GetEmployeeType(employeeDto.EmployeeType.Name);

            employee = new Employee(employeeDto, ExistingEmployeeType);
        }
        catch (Exception)
        {
            EmployeeTypeDto newEmployeeType = await _employeeTypeService
                .SaveEmployeeType(new EmployeeTypeDto(0, employeeDto.EmployeeType.Name));

            employee = new Employee(employeeDto, newEmployeeType);
        }

        EmployeeDto newEmployee = await _db.Employee.Add(employee);

        return newEmployee;
    }

    public async Task<bool> CheckUserExist(string email)
    {
        return await _db.Employee
            .Any(employee => employee.Email == email);
    }

    public async Task<EmployeeDto> DeleteEmployee(string email)
    {
        var existingEmployee = await GetEmployee(email);

        return await _db.Employee.Delete(existingEmployee.Id);
    }

    public async Task<List<EmployeeDto>> GetAll()
    {
        return await _db.Employee
            .Get(employee => true)
            .AsNoTracking()
            .Include(employee => employee.EmployeeType)
            .Select(employee => employee.ToDto())
            .ToListAsync();
    }

    public async Task<EmployeeDto> GetEmployee(string email)
    {
        var employee = await _db.Employee
            .Get(employee => employee.Email == email)
            .AsNoTracking()
            .Include(employee => employee.EmployeeType)
            .Select(employee => employee.ToDto())
            .Take(1)
            .FirstOrDefaultAsync();

        if (employee == null) { throw new Exception("User not found"); }

        return employee;
    }

    public async Task<EmployeeDto> UpdateEmployee(EmployeeDto employeeDto, string email)
    {
        EmployeeTypeDto employeeTypeDto = await _employeeTypeService
            .GetEmployeeType(employeeDto.EmployeeType.Name);

        Employee employee = new Employee(employeeDto, employeeTypeDto);

        employee.Email = email;

        return await _db.Employee.Update(employee);
    }
}
