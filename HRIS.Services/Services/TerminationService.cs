using HRIS.Models;
using HRIS.Services.Interfaces;
using HRIS.Services.Session;
using RR.UnitOfWork;
using RR.UnitOfWork.Entities.HRIS;

namespace HRIS.Services.Services;

public class TerminationService : ITerminationService
{
    private readonly IUnitOfWork _db;
    private readonly IEmployeeTypeService _employeeTypeService;
    private readonly IEmployeeService _employeeService;
    private readonly IAuthService _authService;
    private readonly AuthorizeIdentity _identity;

    public TerminationService(IUnitOfWork db, IEmployeeTypeService employeeTypeService, IEmployeeService employeeService, IAuthService authService, AuthorizeIdentity identity)
    {
        _db = db;
        _employeeTypeService = employeeTypeService;
        _employeeService = employeeService;
        _authService = authService;
        _identity = identity;
    }

    public async Task<bool> CheckIfModelExists(int id)
    {
        return await _db.Termination.Any(x => x.Id == id);
    }

    public async Task<TerminationDto> SaveTermination(TerminationDto terminationDto)
    {
        var modelExists = await CheckIfModelExists(terminationDto.Id);

        if(!modelExists) throw new CustomException("This model doe snot exist yet");

        if (_identity.Role is not ("SuperAdmin" or "Admin" or "Talent" or "Journey") && terminationDto.EmployeeId != _identity.EmployeeId)
            throw new CustomException("Unauthorized Access.");

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
        var modelExists = await CheckIfModelExists(terminationDto.Id);

        if (!modelExists) throw new CustomException("This model does not exist yet");

        if (_identity.Role is not ("SuperAdmin" or "Admin" or "Talent" or "Journey") && terminationDto.EmployeeId != _identity.EmployeeId)
            throw new CustomException("Unauthorized Access.");
        return (await _db.Termination.Update(new Termination(terminationDto))).ToDto();
    }

    public async Task<TerminationDto> GetTerminationByEmployeeId(int employeeId)
    {
        var modelExists = await CheckIfModelExists(employeeId);

        if (!modelExists) throw new CustomException("This model does not exist yet");

        if (_identity.Role is not ("SuperAdmin" or "Admin" or "Talent" or "Journey") && employeeId != _identity.EmployeeId)
            throw new CustomException("Unauthorized Access.");

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
