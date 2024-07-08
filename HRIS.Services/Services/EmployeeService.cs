using System.Net.Mail;
using HRIS.Models;
using HRIS.Services.Interfaces;
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

    public EmployeeService(IEmployeeTypeService employeeTypeService, IUnitOfWork db,
                           IEmployeeAddressService employeeAddressService, IRoleService roleService,
                           IErrorLoggingService errorLoggingService, IEmailService emailService)
    {
        _employeeTypeService = employeeTypeService;
        _db = db;
        _employeeAddressService = employeeAddressService;
        _roleService = roleService;
        _errorLoggingService = errorLoggingService;
        _emailService = emailService;
    }

    public async Task<EmployeeDto> SaveEmployee(EmployeeDto employeeDto)
    {
        var exists = await CheckUserExist(employeeDto.Email);
        if (exists)
            throw new CustomException("Email Is Already in Use");

        if (employeeDto.EmployeeType == null)
            throw new CustomException("Employee Type Missing");

        var existingEmployeeType = await _employeeTypeService.GetEmployeeType(employeeDto.EmployeeType.Name);

        var employee = new Employee(employeeDto, existingEmployeeType);

        EmployeeAddressDto physicalAddress;

        if (!await _employeeAddressService.CheckIfExists(employeeDto.PhysicalAddress!))
            physicalAddress = await _employeeAddressService.Save(employeeDto.PhysicalAddress!);
        else
            physicalAddress = await _employeeAddressService.Get(employeeDto.PhysicalAddress!);

        employee.PhysicalAddressId = physicalAddress.Id;

        EmployeeAddressDto postalAddress;

        if (!await _employeeAddressService
                .CheckIfExists(employeeDto.PostalAddress!))
            postalAddress = await _employeeAddressService.Save(employeeDto.PostalAddress!);
        else
            postalAddress = await _employeeAddressService.Get(employeeDto.PostalAddress!);

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

    public async Task<bool> CheckUserExist(string? email)
    {
        return await _db.Employee.Any(employee => employee.Email == email);
    }

    public async Task<EmployeeDto> DeleteEmployee(string email)
    {
        var existingEmployee = await GetEmployee(email);

        return (await _db.Employee.Delete(existingEmployee!.Id)).ToDto();
    }

    public async Task<List<EmployeeDto>> GetAll(string userEmail = "")
    {
        if (userEmail != "" && await IsJourney(userEmail))
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
            throw new CustomException("Unable to Load Employee");

        return employee;
    }

    public async Task<EmployeeDto> GetEmployeeById(int id)
    {
        return await _db.Employee
                                .Get(employee => employee.Id == id)
                                .AsNoTracking()
                                .Include(employee => employee.EmployeeType)
                                .Include(employee => employee.PhysicalAddress)
                                .Include(employee => employee.PostalAddress)
                                .Select(employee => employee.ToDto())
                                .FirstOrDefaultAsync() ?? throw new CustomException("Unable to Load Employee");
    }

    public async Task<EmployeeDto> UpdateEmployee(EmployeeDto employeeDto, string email)
    {
        var employee = await _db.Employee
            .Get(employee => employee.Email == employeeDto.Email)
            .FirstOrDefaultAsync();

        if (employee == null)
            throw new CustomException("Unable to Load Employee");

        if (employee.Email != email && !await IsAdmin(email)) 
            throw new CustomException("Unauthorized Access");

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

        return (await _db.Employee.Update(employee)).ToDto();

    }

    public async Task<EmployeeDto?> GetById(int employeeId)
    {
        var employee = await _db.Employee.GetById(employeeId);
        return employee.ToDto();
    }

    public async Task<EmployeeCountDataCard> GenerateDataCardInformation()
    {
        var employeeCountTotalsByRole = GetEmployeeCountTotalByRole();

        var totalNumberOfEmployeesOnBench = GetTotalNumberOfEmployeesOnBench();

        var totalNumberOfEmployeesOnClients = GetTotalNumberOfEmployeesOnClients();

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
            isIncrease = isIncrease
        };
    }

    public async Task<ChurnRateDataCardDto> CalculateEmployeeChurnRate()
    {
        var employeeCurrentMonthTotal = await GetEmployeeCurrentMonthTotal();

        var employeePreviousMonthTotal = await GetEmployeePreviousMonthTotal();

        return new ChurnRateDataCardDto
        {
            ChurnRate = CalculateChurnRate(employeeCurrentMonthTotal?.EmployeeTotal ?? 0, employeePreviousMonthTotal?.EmployeeTotal ?? 0),
            DeveloperChurnRate = CalculateChurnRate(employeeCurrentMonthTotal?.DeveloperTotal ?? 0, employeePreviousMonthTotal?.DeveloperTotal ?? 0),
            DesignerChurnRate = CalculateChurnRate(employeeCurrentMonthTotal?.DesignerTotal ?? 0, employeePreviousMonthTotal?.DesignerTotal ?? 0),
            ScrumMasterChurnRate = CalculateChurnRate(employeeCurrentMonthTotal?.ScrumMasterTotal ?? 0, employeePreviousMonthTotal?.ScrumMasterTotal ?? 0),
            BusinessSupportChurnRate = CalculateChurnRate(employeeCurrentMonthTotal?.BusinessSupportTotal ?? 0, employeePreviousMonthTotal?.BusinessSupportTotal ?? 0),
            Month = employeePreviousMonthTotal?.Month ?? DateTime.Now.AddMonths(-1).ToString("MMMM"),
            Year = employeePreviousMonthTotal?.Year ?? DateTime.Now.Year
        };
    }

    private double CalculateChurnRate(int employeeCurrentMonthTotal, int employeePreviousMonthTotal)
    {
        return Math.Round((employeePreviousMonthTotal > 0)
            ? (double)(employeeCurrentMonthTotal - employeePreviousMonthTotal)
              / employeePreviousMonthTotal * 100
            : 0, 0);
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

            var employeeCountTotalsByRole = GetEmployeeCountTotalByRole();

            var monthlyEmployeeTotalDto = new MonthlyEmployeeTotalDto
            {
                Id = 0,
                EmployeeTotal = employeeTotalCount.Count,
                DeveloperTotal = employeeCountTotalsByRole.DevsCount,
                DesignerTotal = employeeCountTotalsByRole.DesignersCount,
                ScrumMasterTotal = employeeCountTotalsByRole.ScrumMastersCount,
                BusinessSupportTotal = employeeCountTotalsByRole.BusinessSupportCount,
                Month = currentMonth,
                Year = currentYear
            };
            var newMonthlyEmployeeTotal = new MonthlyEmployeeTotal(monthlyEmployeeTotalDto);

            return (await _db.MonthlyEmployeeTotal.Add(newMonthlyEmployeeTotal)).ToDto();
        }
        return currentEmployeeTotal.ToDto();
    }

    public async Task<SimpleEmployeeProfileDto> GetSimpleProfile(string employeeEmail)
    {
        var employeeDto = await GetEmployee(employeeEmail);
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

        var simpleProfile = new SimpleEmployeeProfileDto
        {
            Id = employeeDto.Id,
            EmployeeNumber = employeeDto.EmployeeNumber,
            TaxNumber = employeeDto.TaxNumber,
            EngagementDate = employeeDto.EngagementDate,
            TerminationDate = employeeDto.TerminationDate,
            PeopleChampionName = peopleChampionName,
            PeopleChampionId = peopleChampionId == 0 ? null : peopleChampionId,
            Disability = employeeDto.Disability,
            DisabilityNotes = employeeDto.DisabilityNotes,
            Level = employeeDto.Level,
            EmployeeType = employeeDto.EmployeeType,
            Notes = employeeDto.Notes,
            LeaveInterval = employeeDto.LeaveInterval,
            SalaryDays = employeeDto.SalaryDays,
            PayRate = employeeDto.PayRate,
            Salary = employeeDto.Salary,
            Name = employeeDto.Name,
            Initials = employeeDto.Initials,
            Surname = employeeDto.Surname,
            DateOfBirth = employeeDto.DateOfBirth,
            CountryOfBirth = employeeDto.CountryOfBirth,
            Nationality = employeeDto.Nationality,
            IdNumber = employeeDto.IdNumber,
            PassportNumber = employeeDto.PassportNumber,
            PassportExpirationDate = employeeDto.PassportExpirationDate,
            PassportCountryIssue = employeeDto.PassportCountryIssue,
            Race = employeeDto.Race,
            Gender = employeeDto.Gender,
            Photo = employeeDto.Photo,
            Email = employeeDto.Email,
            PersonalEmail = employeeDto.PersonalEmail,
            CellphoneNo = employeeDto.CellphoneNo,
            ClientAllocatedName = clientAllocatedName,
            ClientAllocatedId = clientAllocatedId,
            TeamLeadName = teamLeadName,
            TeamLeadId = teamLeadId,
            PhysicalAddress = employeeDto.PhysicalAddress,
            PostalAddress = employeeDto.PostalAddress,
            HouseNo = employeeDto.HouseNo,
            EmergencyContactName = employeeDto.EmergencyContactName,
            EmergencyContactNo = employeeDto.EmergencyContactNo
        };
        return simpleProfile;
    }

    public async Task<List<EmployeeFilterResponse>> FilterEmployees(int peopleChampId = 0, int employeeType = 0, bool activeStatus = true)
    {
        return await _db.Employee
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
                                      .Where(e => e.EmployeeTypeId > 4)
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
                                                        .Where(e => (e.EmployeeTypeId == 2 || e.EmployeeTypeId == 3 ||
                                                                     e.EmployeeTypeId == 4) && e.ClientAllocated != 1)
                                                        .ToList()
                                                        .Count;

        return totalOfDevsDesignersAndScrumsOnClients;
    }

    public async Task<MonthlyEmployeeTotalDto> GetEmployeePreviousMonthTotal()
    {
        var previousMonth = DateTime.Now.AddMonths(-1).ToString("MMMM");

        var previousEmployeeTotal = _db.MonthlyEmployeeTotal
                                       .Get()
                                       .Where(e => e.Month == previousMonth)
                                       .FirstOrDefault();

        if (previousEmployeeTotal == null) return await GetEmployeeCurrentMonthTotal();

        return previousEmployeeTotal.ToDto();
    }

    public EmployeeOnBenchDataCard GetTotalNumberOfEmployeesOnBench()
    {
        var totalNumberOfDevsOnBench = _db.Employee.Get()
                                          .Where(c => c.ClientAllocated == null && c.EmployeeTypeId == 2)
                                          .ToList().Count;

        var totalNumberOfDesignersOnBench = _db.Employee.Get()
                                               .Where(c => c.ClientAllocated == null && c.EmployeeTypeId == 3)
                                               .ToList().Count;

        var totalNumberOfScrumMastersOnBench = _db.Employee.Get()
                                                  .Where(c => c.ClientAllocated == null && c.EmployeeTypeId == 4)
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

    private async Task<bool> IsAdmin(string userEmail)
    {
        var employeeDto = await GetEmployee(userEmail);

        var empRole = await _db.EmployeeRole
                               .Get(role => role.EmployeeId == employeeDto!.Id)
                               .FirstOrDefaultAsync();

        var role = await _db.Role
                            .Get(role => role.Id == empRole!.RoleId)
                            .FirstOrDefaultAsync();

        return role!.Description is "Admin" or "SuperAdmin";
    }

    private async Task<bool> IsJourney(string userEmail)
    {
        var employeeDto = await GetEmployee(userEmail);
        var empRole = await _db.EmployeeRole
                               .Get(role => role.EmployeeId == employeeDto!.Id)
                               .FirstOrDefaultAsync();

        var role = await _db.Role
                            .Get(role => role.Id == empRole!.RoleId)
                            .FirstOrDefaultAsync();

        return role!.Description is "Journey";
    }

    public async Task<bool> CheckDuplicateIdNumber(string idNumber, int employeeId)
    {
        var duplicateExists = await _db.Employee
                          .Get(employee => employee.IdNumber == idNumber && (employeeId == 0 || employee.Id != employeeId))
                          .AnyAsync();

        return duplicateExists;
    }
}
