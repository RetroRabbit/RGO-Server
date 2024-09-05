using System.Net.Mail;
using System.Text.RegularExpressions;
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

    private readonly IAuthService _authService;

    public EmployeeService(IEmployeeTypeService employeeTypeService, IUnitOfWork db,
                           IEmployeeAddressService employeeAddressService, IRoleService roleService, IAuthService authService,
                           IErrorLoggingService errorLoggingService, IEmailService emailService, AuthorizeIdentity identity, IMapper mapper)
    {
        _employeeTypeService = employeeTypeService;
        _db = db;
        _roleService = roleService;
        _errorLoggingService = errorLoggingService;
        _emailService = emailService;
        _identity = identity;
        _mapper = mapper;
        _authService = authService;
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
        
        await _emailService.Send(new MailAddress(employeeDto.Email, $"{employeeDto.Name} {employeeDto.Surname}"), "WelcomeLetter", employeeDto);

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

    public async Task<EmployeeDto> UpdateEmployee(EmployeeProfileDto employeeDto)
    {
        if (_identity.IsSupport == false && _identity.EmployeeId != employeeDto.Id)
            throw new CustomException("Unauthorized Access");

        var employee = await _db.Employee
            .Get(e => e.Id == employeeDto.Id)
            .FirstOrDefaultAsync();

        if (employee == null)
            throw new CustomException("User not found");

        var employeeDtoToUpdate = employeeDto.ToEmployeeDto();

        if (!string.IsNullOrEmpty(employeeDto.TeamLeadName))
        {
            var nameParts = employeeDto.TeamLeadName.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (nameParts.Length >= 2)
            {
                var teamLeadData = await _db.Employee
                     .Get(e => e.Name == nameParts[0] && e.Surname == nameParts[1])
                     .FirstOrDefaultAsync();
                employeeDtoToUpdate.TeamLead = teamLeadData?.Id;
            }
        }
        if (!string.IsNullOrEmpty(employeeDto.PeopleChampionName))
        {
            var nameParts = employeeDto.PeopleChampionName.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (nameParts.Length >= 2)
            {
                var peopleChampionData = await _db.Employee
                     .Get(e => e.Name == nameParts[0] && e.Surname == nameParts[1])
                     .FirstOrDefaultAsync();
                employeeDtoToUpdate.PeopleChampion = peopleChampionData?.Id;
            }
        }
        if (!string.IsNullOrEmpty(employeeDto.ClientAllocatedName))
        {
            var clientDto = await _db.Client
                                     .Get(c => c.Name == employeeDto.ClientAllocatedName)
                                     .AsNoTracking()
                                     .Select(c => c.ToDto())
                                     .FirstOrDefaultAsync();
            employeeDtoToUpdate.ClientAllocated = clientDto?.Id;
        }

        var updated = await _db.Employee.Update(_mapper.Map<Employee>(employeeDtoToUpdate));
        return _mapper.Map<EmployeeDto>(updated);
    }

    public async Task<EmployeeProfileDto> GetEmployeeProfile(string identifier)
    {
        EmployeeDto employeeDto = new EmployeeDto();
        if (IsValidEmail(identifier))
        {
            var modelExists = await CheckUserEmailExist(identifier);
            if (!modelExists)
                throw new CustomException("Model not found");

            employeeDto = await GetEmployeeByEmail(identifier);
        }
        else if (int.TryParse(identifier, out int employeeId))
        {
            employeeDto = await GetEmployeeById(employeeId);
        }

        var simpleProfile = _mapper.Map<EmployeeProfileDto>(employeeDto);

        if (employeeDto!.TeamLead != null)
        {
            var teamLeadDto = await GetEmployeeById((int)employeeDto.TeamLead);
            simpleProfile.TeamLeadName = teamLeadDto!.Name + " " + teamLeadDto.Surname;
        }

        if (employeeDto.PeopleChampion != null)
        {
            var peopleChampionDto = await GetEmployeeById((int)employeeDto.PeopleChampion);
            simpleProfile.PeopleChampionName = peopleChampionDto!.Name + " " + peopleChampionDto.Surname;
        }

        if (employeeDto.ClientAllocated != null)
        {
            var clientDto = await _db.Client
                                     .Get(client => client.Id == employeeDto.ClientAllocated)
                                     .AsNoTracking()
                                     .Select(client => client.ToDto())
                                     .FirstAsync();
            simpleProfile.ClientAllocatedName = clientDto.Name;
        }

        return simpleProfile;
    }

    private bool IsValidEmail(string email)
    {
        return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
    }

    public async Task<List<EmployeeProfileDto>> GetAllEmployeeProfiles()
    {
        var employeeDtos = await GetAll("");

        var employeeProfiles = new List<EmployeeProfileDto>();
        
        foreach (var employeeDto in employeeDtos)
        {
            var profileDto = await CreateEmployeeProfileDto(employeeDto);

            employeeProfiles.Add(profileDto);
        }
        return employeeProfiles;
    }

    private async Task<EmployeeProfileDto> CreateEmployeeProfileDto(EmployeeDto employeeDto)
    {
        var simpleProfile = _mapper.Map<EmployeeProfileDto>(employeeDto);

        if (employeeDto.TeamLead.HasValue)
        {
            var teamLeadDto = await GetEmployeeById(employeeDto.TeamLead.Value);
            simpleProfile.TeamLeadName = $"{teamLeadDto.Name} {teamLeadDto.Surname}";
            simpleProfile.TeamLeadId = teamLeadDto.Id;
        }

        if (employeeDto.PeopleChampion.HasValue)
        {
            var peopleChampionDto = await GetEmployeeById(employeeDto.PeopleChampion.Value);
            simpleProfile.PeopleChampionName = $"{peopleChampionDto.Name} {peopleChampionDto.Surname}";
            simpleProfile.PeopleChampionId = peopleChampionDto.Id;
        }

        if (employeeDto.ClientAllocated.HasValue)
        {
            var clientDto = await _db.Client
                                     .Get(client => client.Id == employeeDto.ClientAllocated.Value)
                                     .AsNoTracking()
                                     .Select(client => client.ToDto())
                                     .FirstOrDefaultAsync();

            if (clientDto != null)
            {
                simpleProfile.ClientAllocatedName = clientDto.Name;
                simpleProfile.ClientAllocatedId = clientDto.Id;
            }
        }
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

    public async Task<bool> CheckDuplicateIdNumber(string idNumber, int employeeId, bool update = false)
    {
        if (_identity.IsInactive)
            throw new CustomException("Unauthorized Access");

        var modelExists = await CheckModelExist(employeeId);
        if (modelExists && !update)
            throw new CustomException("Model already exists and not being updated.");

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

    public async Task<EmployeeProfileDto> CheckUserAuthentication(string email, string id, string role)
    {
        var emailExists = await CheckUserEmailExist(email);
        if (!emailExists)
        {
            await _authService.DeleteUser(id);

            throw new CustomException("User not found");
        }

        var employee = await GetEmployeeProfile(email);

        if (string.IsNullOrEmpty(role))
        {
            if (employee.AuthUserId != id)
            {
                employee.AuthUserId = id;
                await UpdateEmployee(employee);
            }

            return employee;
        }

        return employee;
    }
}