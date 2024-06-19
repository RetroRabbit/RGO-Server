using Auth0.ManagementApi.Models;
using HRIS.Models;
using HRIS.Services.Interfaces;
using RR.UnitOfWork;
using RR.UnitOfWork.Entities.HRIS;

namespace HRIS.Services.Services;

public class TerminationService : ITerminationService
{
    private readonly IUnitOfWork _db;
    private readonly IErrorLoggingService _errorLoggingService;
    private readonly IEmployeeTypeService _employeeTypeService;
    private readonly IEmployeeService _employeeService;
    private readonly IAuthService _authService;

    public TerminationService(IUnitOfWork db,IAuthService authService, IErrorLoggingService errorLoggingService, IEmployeeTypeService employeeTypeService, IEmployeeService employeeService)
    {
        _db = db;
        _errorLoggingService = errorLoggingService;
        _employeeTypeService = employeeTypeService;
        _employeeService = employeeService;
        _authService = authService;
    }

    public async Task<TerminationDto> SaveTermination(TerminationDto terminationDto)
    {
        var exists = await CheckTerminationExist(terminationDto.EmployeeId);
        if (exists)
        {
            TerminationDto updatedTermination = await UpdateTermination(terminationDto);
            return updatedTermination;
        }

        EmployeeDto currentEmployee = await _employeeService.GetEmployeeById(terminationDto.EmployeeId);
        currentEmployee.InactiveReason = terminationDto.TerminationOption.ToString();
        currentEmployee.TerminationDate = terminationDto.LastDayOfEmployment;
        currentEmployee.Active = false;
        var isRemovedFromAuth0 = await  _authService.DeleteUser(currentEmployee.AuthUserId);

        if (isRemovedFromAuth0 != true)
        {
            throw new Exception("User not terminated");
        }  

        EmployeeTypeDto employeeTypeDto = await _employeeTypeService.GetEmployeeType(currentEmployee.EmployeeType!.Name);

        await _db.Employee.Update(new Employee(currentEmployee, employeeTypeDto));

        return await _db.Termination.Add(new Termination(terminationDto));
    }

    public async Task<TerminationDto> UpdateTermination(TerminationDto terminationDto)
    {
        return await _db.Termination.Update(new Termination(terminationDto));
    }

    public async Task<TerminationDto> GetTerminationByEmployeeId(int employeeId)
    {
        var exists = await CheckTerminationExist(employeeId);
        if (!exists)
        {
            var exception = new Exception("termination not found");
            throw _errorLoggingService.LogException(exception);
        }

        return await _db.Termination.FirstOrDefault(termination => termination.EmployeeId == employeeId);
    }

    public async Task<bool> CheckTerminationExist(int employeeId)
    {
        return await _db.Termination.Any(termination => termination.EmployeeId == employeeId);
    }
}
