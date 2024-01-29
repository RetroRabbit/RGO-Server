using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RGO.Models;
using RGO.Services.Interfaces;
using RGO.UnitOfWork;
using RGO.UnitOfWork.Entities;
using System.Runtime.InteropServices;
using System.Text;

namespace RGO.Services.Services;

public class EmployeeService : IEmployeeService
{
    private readonly IEmployeeAddressService _employeeAddressService;
    private readonly IEmployeeTypeService _employeeTypeService;
    private readonly IRoleService _roleService;
    private readonly IUnitOfWork _db;
    private const string QueueName = "employee_data_queue";
    public static ConnectionFactory _employeeFactory;
    public EmployeeService(IEmployeeTypeService employeeTypeService, IUnitOfWork db, IEmployeeAddressService employeeAddressService, IRoleService roleService)
    {
        _employeeTypeService = employeeTypeService;
        _db = db;
        _employeeAddressService = employeeAddressService;
        _roleService = roleService;
    }

    public async Task<EmployeeDto> SaveEmployee(EmployeeDto employeeDto)
    {
        bool exists = await CheckUserExist(employeeDto.Email);
        if (exists)
        {
            throw new Exception("User already exists");
        }
        if (employeeDto.EmployeeType == null)
        {
            throw new Exception("Employee type missing");
        }
        Employee employee;

        try
        {
            var ExistingEmployeeType = await _employeeTypeService
                .GetEmployeeType(employeeDto.EmployeeType!.Name);

            employee = new Employee(employeeDto, ExistingEmployeeType);

            try
            {
                PushToProducer(employee);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        catch (Exception)
        {
            EmployeeTypeDto newEmployeeType = await _employeeTypeService
                .SaveEmployeeType(new EmployeeTypeDto(0, employeeDto.EmployeeType!.Name));

            employee = new Employee(employeeDto, newEmployeeType);
        }

        bool physicalAddressExist = await _employeeAddressService
            .CheckIfExitsts(employeeDto.PhysicalAddress!);

        EmployeeAddressDto physicalAddress;

        if (!physicalAddressExist)
        {
            physicalAddress = await _employeeAddressService.Save(employeeDto.PhysicalAddress!);
        }
        else
        {
            physicalAddress = await _employeeAddressService.Get(employeeDto.PhysicalAddress!);
        }
        employee.PhysicalAddressId = physicalAddress.Id;

        bool postalAddressExist = await _employeeAddressService
            .CheckIfExitsts(employeeDto.PostalAddress!);

        EmployeeAddressDto postalAddress;

        if (!postalAddressExist)
        {
            postalAddress = await _employeeAddressService.Save(employeeDto.PostalAddress!);
        }
        else
        {
            postalAddress = await _employeeAddressService.Get(employeeDto.PostalAddress!);
        }

        employee.PostalAddressId = postalAddress.Id;

        RoleDto roleDto = await _roleService.GetRole("Employee");
        EmployeeDto newEmployee = await _db.Employee.Add(employee);

        EmployeeRoleDto employeeRoleDto = new EmployeeRoleDto(0, newEmployee, roleDto);
        await _db.EmployeeRole.Add(new EmployeeRole(employeeRoleDto));

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

    public async Task<List<EmployeeDto>> GetAll(string userEmail = "")
    {

        if(userEmail != "" && await IsJounrey(userEmail))
        {
            EmployeeDto peopleChampion = await GetEmployee(userEmail);

            return await _db.Employee
                .Get(employee => employee.PeopleChampion == peopleChampion.Id)
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

    public async Task<EmployeeDto> GetEmployee(string email)
    {
        var employee = await _db.Employee
            .Get(employee => employee.Email == email)
            .AsNoTracking()
            .Include(employee => employee.EmployeeType)
            .Include(employee => employee.PhysicalAddress)
            .Include(employee => employee.PostalAddress)
            .Select(employee => employee.ToDto())
            .Take(1)
            .FirstOrDefaultAsync();

        if (employee == null) { throw new Exception("Employee not found"); }

        return employee;
    }

    public async Task<EmployeeDto> GetEmployeeById(int id)
    {
        var employee = await _db.Employee
            .Get(employee => employee.Id == id)
            .AsNoTracking()
            .Include(employee => employee.EmployeeType)
            .Include(employee => employee.PhysicalAddress)
            .Include(employee => employee.PostalAddress)
            .Select(employee => employee.ToDto())
            .FirstOrDefaultAsync();

        if (employee == null) { throw new Exception("Employee not found"); }

        return employee;
    }

    public async Task<EmployeeDto> UpdateEmployee(EmployeeDto employeeDto, string userEmail)
    {
        EmployeeTypeDto employeeTypeDto = employeeTypeDto = await _employeeTypeService
                .GetEmployeeType(employeeDto.EmployeeType.Name);
        Employee employee = null;
        if (employeeDto.Email == userEmail)
        {
            employee = await CreateNewEmployeeEntity(employeeDto, employeeTypeDto);
        }
        else
        {
            if (await CheckUserExist(userEmail))
            {
                if (await IsAdmin(userEmail))
                {
                    employee = await CreateNewEmployeeEntity(employeeDto, employeeTypeDto);
                }
                else
                {
                    throw new Exception("Unauthorized action");
                }
            }
            else
            {
                throw new Exception("Unauthorized action");
            }

        }
        return await _db.Employee.Update(employee);
    }

    public void PushToProducer(Employee employeeData)
    {
        try
        {
            using (IConnection connection = _employeeFactory.CreateConnection())
            using (IModel channel = connection.CreateModel())
            {
                var messageBody = JsonConvert.SerializeObject(employeeData);
                var body = Encoding.UTF8.GetBytes(messageBody);

                var properties = channel.CreateBasicProperties();
                properties.Persistent = true;

                channel.QueueDeclare(QueueName, durable: true, exclusive: false, autoDelete: false, null);
                channel.BasicPublish(string.Empty, QueueName, properties, body);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
    public async Task<List<EmployeeDto>> GetEmployeesByType(string type)
    {
        var employees = await _db.Employee.Get(employee => employee.EmployeeType.Name == type).AsNoTracking()
            .Include(employee => employee.EmployeeType)
            .Include(employee => employee.PhysicalAddress)
            .Include(employee => employee.PostalAddress).Select(employee => employee.ToDto()).ToListAsync();

        return employees;
    }

    public async Task<EmployeeDto?> GetById(int employeeId)
    {
        var employee = await _db.Employee.GetById(employeeId);

        return employee;
    }

    public async Task<SimpleEmployeeProfileDto> GetSimpleProfile(string employeeEmail)
    {
        EmployeeDto employeeDto = await GetEmployee(employeeEmail);
        string teamLeadName = "";
        string peopleChampionName = "";
        int teamLeadId = 0;
        int peopleChampionId = 0;
        int clientAllocatedId = 0;
        string clientAllocatedName = "";

        if (employeeDto.TeamLead != null)
        {
            EmployeeDto teamLeadDto = await GetById((int)employeeDto.TeamLead);
            teamLeadName = teamLeadDto.Name + " " + teamLeadDto.Surname;
            teamLeadId = teamLeadDto.Id;
        }
        if (employeeDto.PeopleChampion != null)
        {
            EmployeeDto peopleChampionDto = await GetById((int)employeeDto.PeopleChampion);
            peopleChampionName = peopleChampionDto.Name + " " + peopleChampionDto.Surname;
            peopleChampionId = peopleChampionDto.Id;
        }
        if(employeeDto.ClientAllocated != null)
        {
            ClientDto clientDto = await _db.Client
                .Get(client => client.Id == employeeDto.ClientAllocated)
                .AsNoTracking()
                .Select(client => client.ToDto())
                .FirstAsync();
            
            clientAllocatedId = clientDto.Id;
            clientAllocatedName = clientDto.Name;
        }
            
        SimpleEmployeeProfileDto simpleProfile = new SimpleEmployeeProfileDto(
            employeeDto.Id,
            employeeDto.EmployeeNumber,
            employeeDto.TaxNumber,
            employeeDto.EngagementDate,
            employeeDto.TerminationDate,
            peopleChampionName,
            peopleChampionId == 0 ? null : peopleChampionId,
            employeeDto.Disability,
            employeeDto.DisabilityNotes,
            employeeDto.Level,
            employeeDto.EmployeeType,
            employeeDto.Notes,
            employeeDto.LeaveInterval,
            employeeDto.SalaryDays,
            employeeDto.PayRate,
            employeeDto.Salary,
            employeeDto.Name,
            employeeDto.Initials,
            employeeDto.Surname,
            employeeDto.DateOfBirth,
            employeeDto.CountryOfBirth,
            employeeDto.Nationality,
            employeeDto.IdNumber,
            employeeDto.PassportNumber,
            employeeDto.PassportExpirationDate,
            employeeDto.PassportCountryIssue,
            employeeDto.Race,
            employeeDto.Gender,
            employeeDto.Photo,
            employeeDto.Email,
            employeeDto.PersonalEmail,
            employeeDto.CellphoneNo,
            clientAllocatedName,
            clientAllocatedId,
            teamLeadName,
            teamLeadId,
            employeeDto.PhysicalAddress,
            employeeDto.PostalAddress,
            employeeDto.HouseNo,
            employeeDto.EmergencyContactName,
            employeeDto.EmergencyContactNo);

        return simpleProfile;
    }

    private async Task<bool> IsAdmin(string userEmail)
    {
        EmployeeDto employeeDto = await GetEmployee(userEmail);

        EmployeeRole empRole = await _db.EmployeeRole
            .Get(role => role.EmployeeId == employeeDto.Id)
            .FirstOrDefaultAsync();

        Role role = await _db.Role
            .Get(role => role.Id == empRole.RoleId)
            .FirstOrDefaultAsync();

        return role.Description is "Admin" or "SuperAdmin";
        /*
            Pattern matching is a technique where you test an expression to determine if it has certain characteristics 
            In my case it replaces my boolean check which looked like this:
                return role.Description == "Admin" || role.Description == "SuperAdmin"
            For more information: https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/functional/pattern-matching
        */
    }

    private async Task<bool> IsJounrey(string userEmail)
    {
        EmployeeDto employeeDto = await GetEmployee(userEmail);
        EmployeeRole empRole = await _db.EmployeeRole
            .Get(role => role.EmployeeId == employeeDto.Id)
            .FirstOrDefaultAsync();

        Role role = await _db.Role
            .Get(role => role.Id == empRole.RoleId)
            .FirstOrDefaultAsync();

        return role.Description is "Journey";
    }

    private async Task<Employee> CreateNewEmployeeEntity(EmployeeDto employeeDto, EmployeeTypeDto employeeTypeDto)
    {
        Employee employee = new Employee();
        string tempEmail = employeeDto.Email;
        employee = new Employee(employeeDto, employeeTypeDto);
        employee.Email = tempEmail;
        return employee;
    }
}

