using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RGO.Models;
using RGO.Services.Interfaces;
using RGO.UnitOfWork;
using RGO.UnitOfWork.Entities;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
            throw new Exception("User already exists");

        // TODO: After new employee bug is fixed, test if this condition can ever be reached and update accordingly
        if (employeeDto.EmployeeType == null)
            throw new Exception("Employee type missing");

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
            .CheckIfExists(employeeDto.PhysicalAddress!);

        EmployeeAddressDto physicalAddress;

        if (!physicalAddressExist)
            physicalAddress = await _employeeAddressService.Save(employeeDto.PhysicalAddress!);
        else
            physicalAddress = await _employeeAddressService.Get(employeeDto.PhysicalAddress!);

        employee.PhysicalAddressId = physicalAddress.Id;

        bool postalAddressExist = await _employeeAddressService
            .CheckIfExists(employeeDto.PostalAddress!);

        EmployeeAddressDto postalAddress;

        if (!postalAddressExist)
            postalAddress = await _employeeAddressService.Save(employeeDto.PostalAddress!);
        else
            postalAddress = await _employeeAddressService.Get(employeeDto.PostalAddress!);

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

        if(userEmail != "" && await IsJourney(userEmail))
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

        if (employee == null)
            throw new Exception("Employee not found"); 

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

        if (employee == null)
            throw new Exception("Employee not found");

        return employee;
    }

    public async Task<EmployeeDto> UpdateEmployee(EmployeeDto employeeDto, string userEmail)
    {
        EmployeeTypeDto employeeTypeDto = employeeTypeDto = await _employeeTypeService
                .GetEmployeeType(employeeDto.EmployeeType.Name);
        Employee employee = null;
        if (employeeDto.Email == userEmail)
            employee = await CreateNewEmployeeEntity(employeeDto, employeeTypeDto);
        else
        {
            if (await CheckUserExist(userEmail))
            {
                if (await IsAdmin(userEmail))
                    employee = await CreateNewEmployeeEntity(employeeDto, employeeTypeDto);
                else
                    throw new Exception("Unauthorized action");
            }
            else
                throw new Exception("Unauthorized action");
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

    public EmployeeCountByRoleDataCard GetEmployeeCountTotalByRole()
    {
        var devsTotal = _db.Employee.Get()
            .Where(e => e.EmployeeTypeId == 2)
            .ToList().Count;

        var designersTotal = _db.Employee.Get()
            .Where(e => e.EmployeeTypeId == 3)
            .ToList().Count;

        var scrumMastersTotal = _db.Employee.Get()
            .Where(e => e.EmployeeTypeId == 4)
            .ToList().Count;

        var businessSupportTotal = _db.Employee.Get()
            .Where(e => e.EmployeeTypeId == 5)
            .ToList().Count;

        return new EmployeeCountByRoleDataCard
        {
            DevsCount = devsTotal,
            DesignersCount = designersTotal,
            ScrumMastersCount = scrumMastersTotal,
            BusinessSupportCount = businessSupportTotal
        };
    }

    public int GetTotalNumberOfEmployeesOnClients()
    {
        var totalOfDevsDesignersAndScrumsOnClients = _db.Employee
          .Get()
          .Where(e => (e.EmployeeTypeId == 2 || e.EmployeeTypeId == 3 || e.EmployeeTypeId == 4) && e.ClientAllocated != 1)
          .ToList()
          .Count;

        return totalOfDevsDesignersAndScrumsOnClients;
    }

    public async Task<EmployeeCountDataCard> GenerateDataCardInformation()
    {
        var employeeCountTotalsByRole = GetEmployeeCountTotalByRole();

        var totalNumberOfEmployeesOnBench = GetTotalNumberOfEmployeesOnBench();

        var totalNumberOfEmployeesOnClients  = GetTotalNumberOfEmployeesOnClients();
        
        var totalNumberOfEmployeesDevsScrumsAndDesigners = totalNumberOfEmployeesOnBench.TotalNumberOfEmployeesOnBench
            + totalNumberOfEmployeesOnClients;

        var billableEmployeesPercentage = totalNumberOfEmployeesDevsScrumsAndDesigners > 0
               ? (double)totalNumberOfEmployeesOnClients / totalNumberOfEmployeesDevsScrumsAndDesigners * 100
               : 0;

        var currentMonthTotal = await GetEmployeeCurrentMonthTotal();

        var previousMonthTotal = await GetEmployeePreviousMonthTotal();

        var employeeTotalDifference = currentMonthTotal.EmployeeTotal - previousMonthTotal.EmployeeTotal;
        var isIncrease = employeeTotalDifference > 0;

        return new EmployeeCountDataCard
        {
            DevsCount = employeeCountTotalsByRole.DevsCount,
            DesignersCount = employeeCountTotalsByRole.DesignersCount,
            ScrumMastersCount = employeeCountTotalsByRole.ScrumMastersCount,
            BusinessSupportCount = employeeCountTotalsByRole.BusinessSupportCount,
            DevsOnBenchCount = totalNumberOfEmployeesOnBench.DevsOnBenchCount,
            DesignersOnBenchCount = totalNumberOfEmployeesOnBench.DesignersOnBenchCount,
            ScrumMastersOnBenchCount = totalNumberOfEmployeesOnBench.ScrumMastersOnBenchCount,
            TotalNumberOfEmployeesOnBench = totalNumberOfEmployeesOnBench.TotalNumberOfEmployeesOnBench,
            BillableEmployeesPercentage = Math.Round(billableEmployeesPercentage, 0),
            EmployeeTotalDifference = employeeTotalDifference,
            isIncrease = isIncrease,
        };
    }

    public async Task<ChurnRateDataCard> CalculateEmployeeChurnRate()
    {
        var employeeCurrentMonthTotal = await GetEmployeeCurrentMonthTotal();

        var employeePreviousMonthTotal = await GetEmployeePreviousMonthTotal();

        if (employeePreviousMonthTotal != null && employeePreviousMonthTotal.EmployeeTotal > 0 && employeePreviousMonthTotal.DeveloperTotal > 0
            && employeePreviousMonthTotal.DesignerTotal > 0 && employeePreviousMonthTotal.ScrumMasterTotal > 0
            && employeePreviousMonthTotal.BusinessSupportTotal > 0)
        {
            var churnRate = (double)(employeeCurrentMonthTotal.EmployeeTotal - employeePreviousMonthTotal.EmployeeTotal) 
                / employeePreviousMonthTotal.EmployeeTotal * 100;

            var devChurnRate = (double)(employeeCurrentMonthTotal.DeveloperTotal - employeePreviousMonthTotal.DeveloperTotal)
                / employeePreviousMonthTotal.DeveloperTotal * 100;

            var designerChurnRate = (double)(employeeCurrentMonthTotal.DesignerTotal - employeePreviousMonthTotal.DesignerTotal)
                / employeePreviousMonthTotal.DesignerTotal * 100;

            var scrumMasterChurnRate = (double)(employeeCurrentMonthTotal.ScrumMasterTotal - employeePreviousMonthTotal.ScrumMasterTotal)
                / employeePreviousMonthTotal.ScrumMasterTotal * 100;

            var businessSupportChurnRate = (double)(employeeCurrentMonthTotal.BusinessSupportTotal - employeePreviousMonthTotal.BusinessSupportTotal)
                / employeePreviousMonthTotal.BusinessSupportTotal * 100;

            return new ChurnRateDataCard
            {
                ChurnRate = Math.Round(churnRate, 0),
                DeveloperChurnRate = Math.Round(devChurnRate, 0),
                DesignerChurnRate = Math.Round(designerChurnRate, 0),
                ScrumMasterChurnRate = Math.Round(scrumMasterChurnRate, 0),
                BusinessSupportChurnRate = Math.Round(businessSupportChurnRate, 0),
                Month = employeePreviousMonthTotal.Month,
                Year = employeePreviousMonthTotal.Year,
            };
        }
        else
        {
            return new ChurnRateDataCard
            {
                ChurnRate = 0,
                DeveloperChurnRate = 0,
                DesignerChurnRate = 0,
                ScrumMasterChurnRate = 0,
                BusinessSupportChurnRate = 0,
                Month = DateTime.Now.AddMonths(-1).ToString("MMMM"),
                Year = DateTime.Now.Year,
            };
        }
    }

    public async Task<MonthlyEmployeeTotalDto> GetEmployeeCurrentMonthTotal()
    {
        var currentMonth = DateTime.Now.ToString("MMMM");

        var currentYear = DateTime.Now.Year;

        var currentEmployeeTotal = _db.MonthlyEmployeeTotal
            .Get()
            .Where(e => e.Month == currentMonth && e.Year == currentYear)
            .FirstOrDefault();

        if (currentEmployeeTotal == null)
        {
            var employeeTotalCount = await _db.Employee.GetAll();

            var employeeCountTotalsByRole =  GetEmployeeCountTotalByRole();

            MonthlyEmployeeTotalDto monthlyEmployeeTotalDto = new MonthlyEmployeeTotalDto
                (0, employeeTotalCount.Count, employeeCountTotalsByRole.DevsCount, employeeCountTotalsByRole.DesignersCount,
               employeeCountTotalsByRole.ScrumMastersCount,employeeCountTotalsByRole.BusinessSupportCount, currentMonth, currentYear);

            var newMonthlyEmployeeTotal = new MonthlyEmployeeTotal(monthlyEmployeeTotalDto);

            return await _db.MonthlyEmployeeTotal.Add(newMonthlyEmployeeTotal);
        }

        return currentEmployeeTotal.ToDto();
    }

    public async Task<MonthlyEmployeeTotalDto> GetEmployeePreviousMonthTotal()
    {
        var previousMonth = DateTime.Now.AddMonths(-1).ToString("MMMM");

        var previousEmployeeTotal = _db.MonthlyEmployeeTotal
            .Get()
            .Where(e => e.Month == previousMonth)
            .FirstOrDefault();

        if (previousEmployeeTotal == null)
        {
            return await GetEmployeeCurrentMonthTotal();
        }

        return previousEmployeeTotal.ToDto();
    }

    public EmployeeOnBenchDataCard GetTotalNumberOfEmployeesOnBench()
    {
        var totalNumberOfDevsOnBench = _db.Employee.Get()
            .Where(c => c.ClientAllocated == 1 && c.EmployeeTypeId == 2)
            .ToList().Count;

        var totalNumberOfDesignersOnBench = _db.Employee.Get()
            .Where(c => c.ClientAllocated == 1 && c.EmployeeTypeId == 3)
            .ToList().Count;

        var totalNumberOfScrumMastersOnBench = _db.Employee.Get()
            .Where(c => c.ClientAllocated == 1 && c.EmployeeTypeId == 4)
            .ToList().Count;

        var totalnumberOfEmployeesOnBench = totalNumberOfDevsOnBench +
           totalNumberOfDesignersOnBench +
           totalNumberOfScrumMastersOnBench;

        return new EmployeeOnBenchDataCard
        {
            DevsOnBenchCount = totalNumberOfDevsOnBench,
            DesignersOnBenchCount = totalNumberOfDesignersOnBench,
            ScrumMastersOnBenchCount = totalNumberOfScrumMastersOnBench,
            TotalNumberOfEmployeesOnBench = totalnumberOfEmployeesOnBench
        };
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
    }

    private async Task<bool> IsJourney(string userEmail)
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

        employee = new Employee(employeeDto, employeeTypeDto);
        employee.Email = employeeDto.Email;

        return employee;
    }
}

