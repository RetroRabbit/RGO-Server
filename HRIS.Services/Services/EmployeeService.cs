using System.Net.Mail;
using AutoMapper;
using HRIS.Models;
using HRIS.Services.Interfaces;
using HRIS.Services.Session;
using Microsoft.EntityFrameworkCore;
using RR.UnitOfWork;
using RR.UnitOfWork.Entities.HRIS;

namespace HRIS.Services.Services;

public class EmployeeService : IEmployeeService
{
    private readonly IUnitOfWork _db;
    private readonly IEmployeeTypeService _employeeTypeService;
    private readonly IRoleService _roleService;
    private readonly IErrorLoggingService _errorLoggingService;
    private readonly IEmailService _emailService;
    private readonly AuthorizeIdentity _identity;
    private readonly IMapper _mapper;

    public EmployeeService(IEmployeeTypeService employeeTypeService, IUnitOfWork db,
                           IEmployeeAddressService employeeAddressService, IRoleService roleService,
                           IErrorLoggingService errorLoggingService, IEmailService emailService, AuthorizeIdentity identity, IMapper mapper)
    {
        _employeeTypeService = employeeTypeService;
        _db = db;
        _roleService = roleService;
        _errorLoggingService = errorLoggingService;
        _emailService = emailService;
        _identity = identity;
        _mapper = mapper;
    }

    public async Task<EmployeeDto> CreateEmployee(EmployeeDto employeeDto)
    {
        if (_identity.IsSupport == false)
            throw new CustomException("Unauthorized Access");

        var modelExists = await CheckModelExist(employeeDto.Id);
        if (modelExists)
            throw new CustomException("User already created");

        var emailExists = await CheckUserEmailExist(employeeDto.Email);
        if (emailExists)
            throw new CustomException("Email Is Already in Use");

        if (employeeDto.EmployeeType == null)
            throw new CustomException("Employee Type Missing");

        var existingEmployeeType = await _employeeTypeService.GetEmployeeTypeByName(employeeDto.EmployeeType.Name);

        var employee = new Employee(employeeDto, existingEmployeeType);

        var roleDto = await _roleService.GetRole("Employee");

        employee.Active = true;

        var employeeResult = await _db.Employee.Add(employee);

        var newEmployee = _mapper.Map<EmployeeDto>(employeeResult);

        var employeeRoleDto = new EmployeeRoleDto { Id = 0, Employee = newEmployee, Role = roleDto };

        await _db.EmployeeRole.Add(new EmployeeRole(employeeRoleDto)); 
        
        try
        {
            await _emailService.Send(new MailAddress(employeeDto.Email, $"{employeeDto.Name} {employeeDto.Surname}"), "WelcomeLetter", employeeDto);
        }
        catch (Exception ex)
        {
            _errorLoggingService.LogException(ex);
        }

        return newEmployee;
    }

    public async Task<EmployeeDto> DeleteEmployee(string email)
    {
        if (_identity.IsSupport == false)
            throw new CustomException("Unauthorized Access");

        var existingEmployee = await GetEmployeeByEmail(email);

        var modelExists = await CheckModelExist(existingEmployee.Id);
        if (!modelExists)
            throw new CustomException("This model does not exist");

        if (existingEmployee!.Id == _identity.EmployeeId)
            throw new CustomException("Deleting the currently logged-in user is not permitted");

        var result = _mapper.Map<EmployeeDto>(await _db.Employee.Delete(existingEmployee!.Id));

        return result;
    }

    public async Task<List<EmployeeDto>> GetAll(string userEmail = "")
    {
        if (_identity.IsSupport == false)
            throw new CustomException("Unauthorized Access");

        if (userEmail != "" && _identity.IsJourney == true)
        {
            var peopleChampion = await GetEmployeeByEmail(userEmail);

            return await _db.Employee
                            .Get(employee => employee.PeopleChampion == peopleChampion!.Id)
                            .Include(employee => employee.EmployeeType)
                            .OrderBy(employee => employee.Name)
                            .Select(employee => _mapper.Map<EmployeeDto>(employee))
                            .ToListAsync();
        }

        return await _db.Employee
                        .Get(employee => true)
                        .AsNoTracking()
                        .Include(employee => employee.EmployeeType)
                        .OrderBy(employee => employee.Name)
                        .Select(employee => employee.ToDto())
                        .ToListAsync();
    }

    public async Task<EmployeeDto?> GetEmployeeByEmail(string email)
    {
        var emailExists = await CheckUserEmailExist(email);
        if (!emailExists)
            throw new CustomException("User email not found");

        var employee = await _db.Employee
                                .Get(employee => employee.Email == email)
                                .AsNoTracking()
                                .Include(employee => employee.EmployeeType)
                                .Select(employee => employee.ToDto())
                                .FirstOrDefaultAsync() ?? throw new CustomException("Unable to Load Employee");

        return employee;
    }

    public async Task<EmployeeDto> GetEmployeeById(int id)
    {
        var employee = await _db.Employee
                                .Get(employee => employee.Id == id)
                                .AsNoTracking()
                                .Include(employee => employee.EmployeeType)
                                .Select(employee => employee.ToDto())
                                .FirstOrDefaultAsync() ?? throw new CustomException("Unable to Load Employee");

        return employee;
    }

    public async Task<EmployeeDto> UpdateEmployee(EmployeeDto employeeDto)
    {

        if (_identity.IsSupport == false && _identity.EmployeeId != employeeDto.Id)
            throw new CustomException("Unauthorized Access");

        var employee = await _db.Employee
            .Get(employee => employee.Email == employeeDto.Email)
            .FirstOrDefaultAsync();

        if (employee == null)
            throw new CustomException("User not found");

        employee = _mapper.Map<Employee>(employeeDto);

        return _mapper.Map<EmployeeDto>(await _db.Employee.Update(employee));
    }

    public async Task<SimpleEmployeeProfileDto> GetSimpleProfile(string employeeEmail)
    {
        var modelExists = await CheckUserEmailExist(employeeEmail);
        if (!modelExists)
            throw new CustomException("Model not found");

        var employeeDto = await GetEmployeeByEmail(employeeEmail);

        var teamLeadName = "";
        var peopleChampionName = "";
        var teamLeadId = 0;
        var peopleChampionId = 0;
        var clientAllocatedId = 0;
        var clientAllocatedName = "";

        if (employeeDto!.TeamLead != null)
        {
            var teamLeadDto = await GetEmployeeById((int)employeeDto.TeamLead);
            teamLeadName = teamLeadDto!.Name + " " + teamLeadDto.Surname;
            teamLeadId = teamLeadDto.Id;
        }

        if (employeeDto.PeopleChampion != null)
        {
            var peopleChampionDto = await GetEmployeeById((int)employeeDto.PeopleChampion);
            peopleChampionName = peopleChampionDto!.Name + " " + peopleChampionDto.Surname;
            peopleChampionId = peopleChampionDto.Id;
        }

        if (employeeDto.ClientAllocated != null)
        {
            var clientDto = await _db.Client
                                     .Get(client => client.Id == employeeDto.ClientAllocated)
                                     .AsNoTracking()
                                     .Select(client => client.ToDto())
                                     .FirstAsync();

            clientAllocatedId = clientDto.Id;
            clientAllocatedName = clientDto.Name;
        }

        var simpleProfile = _mapper.Map<SimpleEmployeeProfileDto>(employeeDto);

        return simpleProfile;
    }

    public async Task<List<EmployeeFilterResponse>> FilterEmployees(int peopleChampId = 0, int employeeType = 0, bool activeStatus = true)
    {
        if (_identity.IsSupport == false)
            throw new CustomException("Unauthorized Access");

        var filteredEmployees = await _db.Employee
                        .Get(employee =>
                                   (peopleChampId == 0 || employee.PeopleChampion == peopleChampId)
                                   && (employeeType == 0 || employee.EmployeeType!.Id == employeeType)
                                   && (employee.Active == activeStatus))
                        .Include(employee => employee.EmployeeType)
                        .Include(employee => employee.EmployeeRole)
                            .ThenInclude(role => role.Role)
                        .OrderBy(employee => employee.Name)
                        .Select(x => new EmployeeFilterResponse
                        {
                            Name = x.Name,
                            Surname = x.Surname,
                            ClientAllocated = x.ClientAssigned == null ? null : x.ClientAssigned.Name,
                            Level = x.Level,
                            Id = x.Id,
                            RoleId = x.EmployeeRole == null ? 0 : x.EmployeeRole.RoleId,
                            RoleDescription = x.EmployeeRole == null || x.EmployeeRole.Role == null ? "" : x.EmployeeRole.Role.Description ?? "",
                            Email = x.Email,
                            EngagementDate = x.EngagementDate,
                            TerminationDate = x.TerminationDate,
                            InactiveReason = x.InactiveReason,
                            Position = x.EmployeeType == null ? null : x.EmployeeType.Name
                        })
                        .ToListAsync();

        if (filteredEmployees.Count < 1)
            throw new CustomException("Users not found");

        return filteredEmployees;
    }

    public async Task<bool> CheckDuplicateIdNumber(string idNumber, int employeeId)
    {
        if (_identity.IsSupport == false)
            throw new CustomException("Unauthorized Access");

        var modelExists = await CheckModelExist(employeeId);
        if (modelExists)
            throw new CustomException("Model found");

        var duplicateExists = await _db.Employee
                          .Get(employee => employee.IdNumber == idNumber && (employeeId == 0 || employee.Id != employeeId))
                          .AnyAsync();

        return duplicateExists;
    }

    public async Task<bool> CheckModelExist(int id)
    {
        return await _db.Employee.Any(x => x.Id == id);
    }


    public async Task<bool> CheckUserEmailExist(string? email)
    {
        return await _db.Employee.Any(employee => employee.Email == email);
    }
}