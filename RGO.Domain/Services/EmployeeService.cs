using Microsoft.EntityFrameworkCore;
using RGO.Models;
using RGO.Services.Interfaces;
using RGO.UnitOfWork.Entities;
using RGO.UnitOfWork.Interfaces;

namespace RGO.Services.Services;

public class EmployeeService : IEmployeeService
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IEmployeeTypeService _employeeTypeService;

    public EmployeeService(IEmployeeRepository employeeRepository, IEmployeeTypeService employeeTypeService)
    {
        _employeeRepository = employeeRepository;
        _employeeTypeService = employeeTypeService;
    }

    public async Task<EmployeeDto> AddEmployee(EmployeeDto employeeDto)
    {
        Employee employee;

        try
        {
            var ExistingEmployeeType = await _employeeTypeService
                .GetEmployeeType(employeeDto.EmployeeType);

            employee = new Employee(employeeDto, ExistingEmployeeType);
        }
        catch (Exception)
        {
            EmployeeTypeDto newEmployeeType = await _employeeTypeService
                .AddEmployeeType(new EmployeeTypeDto(0, employeeDto.EmployeeType));

            employee = new Employee(employeeDto, newEmployeeType);
        }

        EmployeeDto newEmployee = await _employeeRepository.Add(employee);

        return newEmployee;
    }

    public async Task<bool> CheckUserExist(string email)
    {
        return await _employeeRepository
            .Get(employee => employee.PersonalEmail == email)
            .AnyAsync();
    }

    public async Task<EmployeeDto> DeleteEmployee(string email)
    {
        var existingEmployee = await GetEmployee(email);

        return await _employeeRepository.Delete(existingEmployee.Id);
    }

    public async Task<List<EmployeeDto>> GetAll()
    {
        return await _employeeRepository
            .Get()
            .Include(employee => employee.EmployeeType)
            .Select(employee => employee.ToDto())
            .ToListAsync();
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

    public async Task<EmployeeDto> UpdateEmployee(EmployeeDto employeeDto, string email)
    {
        EmployeeTypeDto employeeTypeDto = await _employeeTypeService
            .GetEmployeeType(employeeDto.EmployeeType);

        Employee employee = new Employee(employeeDto, employeeTypeDto);

        employee.PersonalEmail = email;

        return await _employeeRepository.Update(employee);
    }
}
