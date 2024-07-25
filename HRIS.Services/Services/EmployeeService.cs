using System.Net.Mail;
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
    private readonly IEmployeeAddressService _employeeAddressService;
    private readonly IEmployeeTypeService _employeeTypeService;
    private readonly IRoleService _roleService;
    private readonly IErrorLoggingService _errorLoggingService;
    private readonly IEmailService _emailService;
    private readonly AuthorizeIdentity _identity;

    public EmployeeService(IEmployeeTypeService employeeTypeService, IUnitOfWork db,
                           IEmployeeAddressService employeeAddressService, IRoleService roleService,
                           IErrorLoggingService errorLoggingService, IEmailService emailService, AuthorizeIdentity identity)
    {
        _employeeTypeService = employeeTypeService;
        _db = db;
        _employeeAddressService = employeeAddressService;
        _roleService = roleService;
        _errorLoggingService = errorLoggingService;
        _emailService = emailService;
        _identity = identity;
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

        var existingEmployeeType = await _employeeTypeService.GetEmployeeType(employeeDto.EmployeeType.Name);

        var employee = new Employee(employeeDto, existingEmployeeType);

        var physicalAddress = await _employeeAddressService.Save(employeeDto.PhysicalAddress!);
        employee.PhysicalAddressId = physicalAddress.Id;

        var postalAddress = await _employeeAddressService.Save(employeeDto.PostalAddress!);
        employee.PostalAddressId = postalAddress.Id;

        var roleDto = await _roleService.GetRole("Employee");

        employee.Active = true;
        var newEmployee = await _db.Employee.Add(employee);

        var employeeRoleDto = new EmployeeRoleDto { Id = 0, Employee = newEmployee.ToDto(), Role = roleDto };

        await _db.EmployeeRole.Add(new EmployeeRole(employeeRoleDto));

        try
        {
            await _emailService.Send(new MailAddress(employeeDto.Email, $"{employeeDto.Name} {employeeDto.Surname}"),
                "WelcomeLetter", employeeDto);
        }
        catch (Exception ex)
        {
            _errorLoggingService.LogException(ex);
        }

        return newEmployee.ToDto();
    }

    public async Task<EmployeeDto> DeleteEmployee(string email)
    {
        if (_identity.IsSupport == false)
            throw new CustomException("Unauthorized Access");

        var existingEmployee = await GetEmployee(email);

        var modelExists = await CheckModelExist(existingEmployee.Id);
        if (!modelExists)
            throw new CustomException("This model does not exist");

        if (existingEmployee!.Id == _identity.EmployeeId)
            throw new CustomException("Deleting the currently logged-in user is not permitted");

        return (await _db.Employee.Delete(existingEmployee!.Id)).ToDto();
    }

    public async Task<List<EmployeeDto>> GetAll(string userEmail = "")
    {
        if (_identity.IsSupport == false)
            throw new CustomException("Unauthorized Access");

        if (userEmail != "" && _identity.IsJourney == true)
        {
            var peopleChampion = await GetEmployee(userEmail);

            return await _db.Employee
                            .Get(employee => employee.PeopleChampion == peopleChampion!.Id)
                            .Include(employee => employee.EmployeeType)
                            .Include(employee => employee.PhysicalAddress)
                            .Include(employee => employee.PostalAddress)
                            .OrderBy(employee => employee.Name)
                            .Select(employee => employee.ToDto())
                            .ToListAsync();
        }

        return await _db.Employee
                        .Get(employee => true)
                        .AsNoTracking()
                        .Include(employee => employee.EmployeeType)
                        .Include(employee => employee.PhysicalAddress)
                        .Include(employee => employee.PostalAddress)
                        .OrderBy(employee => employee.Name)
                        .Select(employee => employee.ToDto())
                        .ToListAsync();
    }

    public async Task<EmployeeDto?> GetEmployee(string email)
    {
        var emailExists = await CheckUserEmailExist(email);
        if (!emailExists)
            throw new CustomException("User email not found");

        var employee = await _db.Employee
                                .Get(employee => employee.Email == email)
                                .AsNoTracking()
                                .Include(employee => employee.EmployeeType)
                                .Include(employee => employee.PhysicalAddress)
                                .Include(employee => employee.PostalAddress)
                                .Select(employee => employee.ToDto())
                                .FirstOrDefaultAsync() ?? throw new CustomException("Unable to Load Employee");

        if (employee == null)
            throw new CustomException("User not found");

        if (_identity.IsSupport == false && _identity.EmployeeId != employee.Id)
            throw new CustomException("Unauthorized Access");

        return employee;
    }

    public async Task<EmployeeDto> GetEmployeeById(int id)
    {
        if (_identity.IsSupport == false && _identity.EmployeeId != id)
            throw new CustomException("Unauthorized Access");

        var employee = await _db.Employee
                                .Get(employee => employee.Id == id)
                                .AsNoTracking()
                                .Include(employee => employee.EmployeeType)
                                .Include(employee => employee.PhysicalAddress)
                                .Include(employee => employee.PostalAddress)
                                .Select(employee => employee.ToDto())
                                .FirstOrDefaultAsync() ?? throw new CustomException("Unable to Load Employee");

        return employee;
    }

    public async Task<EmployeeDto> UpdateEmployee(EmployeeDto employeeDto, string email)
    {

        if (_identity.IsSupport == false && _identity.EmployeeId != employeeDto.Id)
            throw new CustomException("Unauthorized Access");

        var employee = await _db.Employee
            .Get(employee => employee.Email == employeeDto.Email)
            .FirstOrDefaultAsync();

        if (employee == null)
            throw new CustomException("User not found");

        employee.TaxNumber = employeeDto.TaxNumber;
        employee.PeopleChampion = employeeDto.PeopleChampion;
        employee.Disability = employeeDto.Disability;
        employee.DisabilityNotes = employeeDto.DisabilityNotes;
        employee.Level = employeeDto.Level;
        employee.EmployeeTypeId = employeeDto.EmployeeType?.Id ?? employee.EmployeeTypeId;
        employee.Notes = employeeDto.Notes;
        employee.Initials = employeeDto.Initials;
        employee.Name = employeeDto.Name;
        employee.Surname = employeeDto.Surname;
        employee.DateOfBirth = employeeDto.DateOfBirth;
        employee.CountryOfBirth = employeeDto.CountryOfBirth;
        employee.Nationality = employeeDto.Nationality;
        employee.IdNumber = employeeDto.IdNumber;
        employee.PassportNumber = employeeDto.PassportNumber;
        employee.PassportExpirationDate = employeeDto.PassportExpirationDate;
        employee.PassportCountryIssue = employeeDto.PassportCountryIssue;
        employee.Race = employeeDto.Race;
        employee.Gender = employeeDto.Gender;
        employee.Email = employeeDto.Email;
        employee.PersonalEmail = employeeDto.PersonalEmail;
        employee.CellphoneNo = employeeDto.CellphoneNo;
        employee.ClientAllocated = employeeDto.ClientAllocated;
        employee.TeamLead = employeeDto.TeamLead;
        employee.PhysicalAddressId = employeeDto.PhysicalAddress?.Id;
        employee.PostalAddressId = employeeDto.PostalAddress?.Id;
        employee.HouseNo = employeeDto.HouseNo;
        employee.EmergencyContactName = employeeDto.EmergencyContactName;
        employee.EmergencyContactNo = employeeDto.EmergencyContactNo;
        employee.Photo = employeeDto.Photo;

        return (await _db.Employee.Update(employee)).ToDto();
    }

    public async Task<EmployeeDto?> GetById(int employeeId)
    {
        var employee = await _db.Employee.GetById(employeeId);

        if (employee == null)
            throw new CustomException("User not found");

        if (_identity.IsSupport == false && _identity.EmployeeId != employee.Id)
            throw new CustomException("Unauthorized Access");

        return employee.ToDto();
    }

    public async Task<SimpleEmployeeProfileDto> GetSimpleProfile(string employeeEmail)
    {
        var modelExists = await CheckUserEmailExist(employeeEmail);
        if (!modelExists)
            throw new CustomException("Model not found");

        var employeeDto = await GetEmployee(employeeEmail);

        if (_identity.IsSupport == false && _identity.EmployeeId != employeeDto.Id)
            throw new CustomException("Unauthorized Access");

        var teamLeadName = "";
        var peopleChampionName = "";
        var teamLeadId = 0;
        var peopleChampionId = 0;
        var clientAllocatedId = 0;
        var clientAllocatedName = "";

        if (employeeDto!.TeamLead != null)
        {
            var teamLeadDto = await GetById((int)employeeDto.TeamLead);
            teamLeadName = teamLeadDto!.Name + " " + teamLeadDto.Surname;
            teamLeadId = teamLeadDto.Id;
        }

        if (employeeDto.PeopleChampion != null)
        {
            var peopleChampionDto = await GetById((int)employeeDto.PeopleChampion);
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

        var simpleProfile = new SimpleEmployeeProfileDto(employeeDto)
        {
            PeopleChampionName = peopleChampionName,
            PeopleChampionId = peopleChampionId == 0 ? null : peopleChampionId,
            ClientAllocatedName = clientAllocatedName,
            ClientAllocatedId = clientAllocatedId,
            TeamLeadName = teamLeadName,
            TeamLeadId = teamLeadId,
        };

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
                        .Include(employee => employee.PhysicalAddress)
                        .Include(employee => employee.PostalAddress)
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
        if (!modelExists)
            throw new CustomException("Model not found");

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