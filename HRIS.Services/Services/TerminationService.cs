using HRIS.Models;
using HRIS.Services.Interfaces;
using RR.UnitOfWork;
using RR.UnitOfWork.Entities.HRIS;

namespace HRIS.Services.Services;

public class TerminationService : ITerminationService
{
    private readonly IUnitOfWork _db;
    private readonly IEmployeeTypeService _employeeTypeService;
    private readonly IEmployeeService _employeeService;
    private readonly IAuthService _authService;

    public TerminationService(IUnitOfWork db, IAuthService authService, IEmployeeTypeService employeeTypeService, IEmployeeService employeeService)
    {
        _db = db;
        _employeeTypeService = employeeTypeService;
        _employeeService = employeeService;
        _authService = authService;
    }

    public async Task<TerminationDto> SaveTermination(TerminationDto terminationDto)
    {
        var exists = await CheckTerminationExist(terminationDto.EmployeeId);
        if (exists)
        {
            var updatedTermination = await UpdateTermination(terminationDto);
            return updatedTermination;
        }

        var currentEmployee = await _employeeService.GetEmployeeById(terminationDto.EmployeeId);
        currentEmployee.InactiveReason = terminationDto.TerminationOption.ToString();
        currentEmployee.TerminationDate = terminationDto.LastDayOfEmployment;
        currentEmployee.Active = false;
        await _authService.DeleteUser(currentEmployee.AuthUserId);

        var employeeTypeDto = await _employeeTypeService.GetEmployeeType(currentEmployee.EmployeeType!.Name);
        await _db.Employee.Update(new Employee(currentEmployee, employeeTypeDto));

        return (await _db.Termination.Add(new Termination(terminationDto))).ToDto();
    }

    public async Task<TerminationDto> UpdateTermination(TerminationDto terminationDto)
    {
        return (await _db.Termination.Update(new Termination(terminationDto))).ToDto();
    }

    public async Task<TerminationDto> GetTerminationByEmployeeId(int employeeId)
    {
        var exists = await CheckTerminationExist(employeeId);
        if (!exists)
            throw new CustomException("termination not found");

        return (await _db.Termination.FirstOrDefault(termination => termination.EmployeeId == employeeId)).ToDto();
    }

    public async Task<bool> CheckTerminationExist(int employeeId)
    {
        return await _db.Termination.Any(termination => termination.EmployeeId == employeeId);
    }
}
