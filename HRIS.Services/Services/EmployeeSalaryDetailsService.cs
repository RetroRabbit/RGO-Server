using HRIS.Models.Employee.Commons;
using HRIS.Services.Interfaces;
using HRIS.Services.Session;
using Microsoft.EntityFrameworkCore;
using RR.UnitOfWork;
using RR.UnitOfWork.Entities.HRIS;

namespace HRIS.Services.Services;

public class EmployeeSalaryDetailsService : IEmployeeSalaryDetailsService
{
    private readonly IUnitOfWork _db;
    private readonly AuthorizeIdentity _identity;

    public EmployeeSalaryDetailsService(IUnitOfWork db, AuthorizeIdentity identity)
    {
        _db = db;
        _identity = identity;
    }

    public async Task<bool> EmployeeSalaryDetailsExists(int id)
    {
        return await _db.EmployeeSalaryDetails.Any(x => x.Id == id);
    }

    public async Task<BankingSalaryDetailsDto> DeleteEmployeeSalary(int id)
    {
        var exists = await EmployeeSalaryDetailsExists(id);

        if (!exists)
            throw new CustomException("Employee not found.");

        if (_identity.IsSupport == false && id != _identity.EmployeeId)
            throw new CustomException("Unauthorized Access.");

        var deletedEmployeeSalaryDetails = await _db.EmployeeSalaryDetails.Delete(id);

        return deletedEmployeeSalaryDetails.ToDto();
    }

    public async Task<List<BankingSalaryDetailsDto>> GetAllEmployeeSalaries()
    {
        return (await _db.EmployeeSalaryDetails.GetAll()).Select(x => x.ToDto()).ToList();
    }

    public async Task<BankingSalaryDetailsDto> GetEmployeeSalaryById(int employeeId)
    {
        var ifEmployeeExists = await CheckEmployee(employeeId);

        if (!ifEmployeeExists)
            throw new CustomException("Employee not found.");

        if (_identity.IsSupport == false && employeeId != _identity.EmployeeId)
            throw new CustomException("Unauthorized Access.");

        return await _db.EmployeeSalaryDetails
                                      .Get(employeeSalary => employeeSalary.EmployeeId == employeeId)
                                      .AsNoTracking()
                                      .Select(employeeSalary => employeeSalary.ToDto())
                                      .FirstOrDefaultAsync();
    }

    public async Task<BankingSalaryDetailsDto> CreateEmployeeSalary(BankingSalaryDetailsDto employeeSalaryDto)
    {
        var exists = await EmployeeSalaryDetailsExists(employeeSalaryDto.Id);

        if (exists)
            throw new CustomException("Employee salary already exists.");

        if (_identity.IsSupport == false && employeeSalaryDto.Id != _identity.EmployeeId)
            throw new CustomException("Unauthorized Access.");

        var employeeSalary = await _db.EmployeeSalaryDetails.Add(new EmployeeSalaryDetails(employeeSalaryDto));

        return employeeSalary.ToDto();
    }

    public async Task<BankingSalaryDetailsDto> UpdateEmployeeSalary(BankingSalaryDetailsDto employeeSalaryDto)
    {
        var exists = await EmployeeSalaryDetailsExists(employeeSalaryDto.Id);

        if (!exists)
            throw new CustomException("Employee not found.");

        if (_identity.IsSupport == false && employeeSalaryDto.Id != _identity.EmployeeId)
            throw new CustomException("Unauthorized Access.");

        EmployeeSalaryDetails employeeSalary = new EmployeeSalaryDetails(employeeSalaryDto);
        var updatedEmployeeSalary = await _db.EmployeeSalaryDetails.Update(employeeSalary);

        return updatedEmployeeSalary.ToDto();
    }

    public async Task<bool> CheckEmployee(int employeeId)
    {
        return await _db.Employee.Any(x => x.Id == employeeId);
    }
}